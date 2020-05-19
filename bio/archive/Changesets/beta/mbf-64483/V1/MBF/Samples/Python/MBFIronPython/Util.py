# Copyright Microsoft Corporation. All rights reserved.
import clr
import os
import sys
import random

_cwd = os.getcwd()
_solution_path = _cwd[ : _cwd.rindex("\\") + 1]

def add_mbfdotnet_reference(dll_name):
    "Adds a MBF dll reference to the clr so that its contents can be imported."
    # An exception will be thrown if we're debugging in VS from the Python development dir, instead
    # of the standard non-dev method of running from the bin\Debug dir or an installation dir.  If
    # we are debugging in VS, we just need to add bin\Debug to the path.
    try:
        clr.AddReferenceToFile(dll_name + ".dll")
    except:
        sys.path += [_solution_path + r"..\..\Build\Binaries\Debug"]
        print _solution_path
        clr.AddReferenceToFile(dll_name + ".dll")
    
add_mbfdotnet_reference("MBF")
from MBF import *

def split_sequence(seq, coverage, fragment_length):
    "Splits a sequence into overlapping fragments of the given length with the given coverage."
    num_fragments = seq.Count * coverage / fragment_length
    fragment_list = []
    for i in range(0, num_fragments):
        fragment = Sequence(seq.Alphabet)
        fragment.IsReadOnly = 0
        fragment.ID = seq.ID + " (Split " + `i` + ")"
        start = random.randrange(seq.Count - fragment_length + 1)
        for item in seq.Range(start, fragment_length):
            fragment.Add(item)
        fragment_list.append(fragment)
    return fragment_list
 
def GetInputFileName(message):
    "Gets the file name from the command prompt"

    # Display the message.
    fileName = raw_input(message)
    
    # If the input is blank then prompt the user to enter a valid name
    while fileName.lstrip() == "":
        fileName = raw_input("Please enter a validFileName:")
    
       
    return fileName