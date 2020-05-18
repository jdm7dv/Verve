import os
import argparse
import subprocess
import signal
import re
import sys

def arguments():

  parser = argparse.ArgumentParser()

  parser.add_argument('input_file', metavar='input-file',
    help = 'source file (*.bpl or *.c)')

  parser.add_argument('prop', metavar='property',
    help = 'property to check (null, double-free, resource leak)')
  
  parser.add_argument('-v', '--verbose', action='store_true', default=False,
    help = 'verbose mode')

  parser.add_argument('-g', '--general', action='store_true', default=False,
    help = 'check general assertion (do not run smackinst.exe)')

  #parser.add_argument('--checkNULL', action='store_true', default=False,
  #  help = 'check NULL pointer deference')

  smack_group = parser.add_argument_group("SMACK options")

  smack_group.add_argument('--smack-options', metavar='OPTIONS', default='',
    help = 'additional SMACK arguments (e.g., --smack-options="-bc a.bc")')

  si_group = parser.add_argument_group("SmackInst options")

  si_group.add_argument('-init-mem', action='store_true', default=False,
    help = 'initialize memory')

  avh_group = parser.add_argument_group("AvHarnessInstrument options")
  
  avh_group.add_argument('-aa', action='store_true', default=False,
    help = 'use alias analysis')

  avh_group.add_argument('--unknown-procs', metavar='PROC', nargs='+',
    default=['malloc', '$alloc'], help = 'specify angelic unknown procedures [default: %(default)s]')

  avh_group.add_argument('--assert-procs', metavar='PROC', nargs='+',
    default=[], help = 'specify procedures with assertions [default: %(default)s]')

  avh_group.add_argument('--harness-options', metavar='OPTIONS', default='',
    help = 'additional AvHarnessInstrumentation arugments (e.g., --harness-options="x")'
  )

  avh_group.add_argument('--use-entry-points', action='store_true', default=False,
    help = 'use entry points only')

  avn_group = parser.add_argument_group("AngelicVerifierNull options")

  avn_group.add_argument('--unroll', metavar='N', type=int,
    default=5, help = 'loop unrolling bound [default: %(default)s]')

  avn_group.add_argument('-sdv', action='store_true', default=False, 
    help = 'use sdv output format')

  avh_group.add_argument('--verifier-options', metavar='OPTIONS', default='',
    help = 'additional AngelicVerifierNull arugments (e.g., --verifer-options="y")'
  )

  return parser.parse_args()

def checkNULL(args):
  if args.prop == 'null':
    return True
  else:
    return False

def find_exe(args):
  args.si_exe = GetBinary('SmackInst')
  args.pi_exe = GetBinary('PropInst')
  args.avh_exe = GetBinary('AvHarnessInstrumentation')
  args.avn_exe = GetBinary('AngelicVerifierNull')

def GetBinary(BinaryName):
  up = os.path.dirname
  corralRoot = up(up(up(up(up(os.path.abspath('__file__'))))))
  avRoot = os.path.join(corralRoot, 'AddOns')
  root = os.path.join(avRoot, BinaryName) if (BinaryName == 'SmackInst' or BinaryName == 'PropInst') else os.path.join(avRoot, 'AngelicVerifierNull')
  return os.path.join(
          os.path.join(
           os.path.join(
            os.path.join(root, BinaryName), 'bin'),'Debug'),
             BinaryName + '.exe')

# ported from SMACK top.py
# time-out is not supoorted

def try_command(args, cmd, console = False):
  console = console or args.verbose
  output = '' 
  proc = None

  try:
    if args.verbose:
      print 'Running %s' %(' '.join(cmd))
    
    proc = subprocess.Popen(cmd,
      stdout=subprocess.PIPE, stderr=subprocess.STDOUT)
    if console:
      while True:
        line = proc.stdout.readline()
        if line:
          output += line
          print line,
        elif proc.poll() is not None:
          break
      proc.wait
    else:
      output = proc.communicate()[0]

    rc = proc.returncode
    proc = None
    if rc:
      raise RuntimeError("%s returned non-zero." % cmd[0])
    else:
      return output

  except (RuntimeError, OSError) as err:
    print >> sys.stderr, output
    sys.exit("Error invoking command:\n%s\n%s" % (" ".join(cmd), err))

  finally:
    if proc: os.killpg(os.getpgid(proc.pid), signal.SIGKILL)

run_cmds = []

def runsmack(args):
  #print 'Running SMACK'
  if os.name != 'posix':
    print 'OS not supported'

  cmd = ['smack', '--no-verify']
  cmd += [args.input_file]
  cmd += ['-bpl', args.file_name + '.bpl']
  cmd += args.smack_options.split()
  
  return try_command(args, cmd, False)  
    
def runsi(args):
  #print "Running SmackInst at: '{}'".format(args.si_exe)
  global run_cmds
  if (not os.path.exists(args.si_exe)):
    print "SmackInst not found" 

  cmd = [args.si_exe]
  if os.name == 'posix':
    cmd = ['mono'] + cmd   
  cmd += [args.file_name + '.bpl']
  cmd += [args.file_name + '.inst.bpl']
  if args.init_mem:
    cmd += ['/initMem']
  if checkNULL(args):
    cmd += ['/checkNULL']
  
  run_cmds += ['// RUN: %si "%s" "%t0.bpl"' + ' '.join(cmd[4 if os.name == 'posix' else 3 :])]
  return try_command(args, cmd, False) 

def runpi(args):
  global run_cmds
  if (not os.path.exists(args.pi_exe)):
    print "PropInst not found"

  cmd = [args.pi_exe]
  if os.name == 'posix':
    cmd = ['mono'] + cmd
  prop_name = args.prop
  prop_file = prop_name + '.avp'
  inst_file_name = args.file_name + '-' + prop_name
  cmd += [prop_file]
  cmd += [args.file_name + '.bpl']
  cmd += [inst_file_name + '.bpl']
  args.file_name = inst_file_name
#TODO: add the command to run commands
  return try_command(args, cmd, False)

def runavh(args):
  #print "Running AvHarnessInstrumentation at: '{}'".format(args.avh_exe) 
  global run_cmds
  if (not os.path.exists(args.avh_exe)):
    print "AvHarnessInstrument not found" 

  cmd = [args.avh_exe]
  if os.name == 'posix':
    cmd = ['mono'] + cmd
  cmd += [args.file_name + ('.bpl' if args.general else '.inst.bpl')]
  cmd += [args.file_name + '.harness.bpl']
  if args.use_entry_points:
    cmd += ['/useEntryPoints']
  cmd += args.harness_options.split()

  if args.aa:
    cmd += ['/noAA:0']
  else:
    cmd += ['/noAA']

  cmd += ['/unknownProc:' + proc for proc in args.unknown_procs]

  if len(args.assert_procs) > 0:
    cmd += ['/assertProc:' + proc for proc in args.assert_procs]

  run_cmds += ['// RUN: %avh "%t0.bpl" "%t1.bpl" ' + ' '.join(cmd[4 if os.name == 'posix' else 3 :])]
  return try_command(args, cmd, True)

def runavn(args):
  #print "Running AngelicVerifierNull at: '{}'".format(args.avn_exe)
  global run_cmds
  if (not os.path.exists(args.avn_exe)):
    print "AngelicVerifierNull not found" 

  cmd = [args.avn_exe]
  if os.name == 'posix':
    cmd = ['mono'] + cmd
  cmd += [args.file_name + '.harness.bpl']
  cmd += ['/nodup']
  cmd += ['/traceSlicing']
  cmd += ['/copt:recursionBound:' + str(args.unroll)]
  cmd += ['/copt:k:1']
  cmd += ['/dontGeneralize']
  if args.sdv:
    cmd += ['/sdv']
  else:
    cmd += ['/copt:tryCTrace']
  cmd += ['/EE:ignoreAllAssumes+']
  cmd += ['/EE:onlySlicAssumes-']
  cmd += args.verifier_options.split()
  run_cmds += ['// RUN: %avn "%t1.bpl" ' + ' '.join(cmd[3 if os.name == 'posix' else 2 :]) + ' | %grep > %t3']
  return try_command(args, cmd, False) 

def output_summary(output):
  av_output = ''

  for line in output.splitlines(True):
    if re.search('AV_OUTPUT', line):
      av_output += line
  
  return av_output

def add_commands_to_bpl(args):
  with open(args.file_name + '.bpl', 'r+') as f:
    bpl = '\n'.join(run_cmds) + '\n// RUN: %diff '+ \
      '"%s.expect" %t3\n\n' + ''.join(filter(lambda x: re.search(r'// RUN: %(si|avh|avn|diff)', x) is None,\
        f.readlines())).lstrip()
    f.seek(0)
    f.truncate()
    f.write(bpl)

if __name__ == '__main__':
  args = arguments()
  args.file_name = os.path.splitext(args.input_file)[0]

  if (os.path.splitext(args.input_file)[1][1:] != 'bpl'):
    smack_output = runsmack(args)

  
  find_exe(args)

  if (not checkNULL(args)):
    pi_output = runpi(args)

  if (not args.general):
    si_output = runsi(args)

  avh_output = runavh(args)
  avn_output = runavn(args)

  add_commands_to_bpl(args)

  print output_summary(avn_output).strip()
