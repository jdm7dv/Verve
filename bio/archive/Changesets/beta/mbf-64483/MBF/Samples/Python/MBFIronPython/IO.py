# Copyright Microsoft Corporation. All rights reserved.
import os
import Util
Util.add_mbfdotnet_reference("MBF")
from MBF.IO import *
from System.IO import *

def open_seq(filename):
    "Parses a sequence file, returning a list of ISequence objects."
    filename = filename.Trim('"').Trim('\'')
    if not File.Exists(filename):
        print "\nFile does not exists: " + filename
        return None
    parser = SequenceParsers.FindParserByFile(filename)
    if parser == None:
        print "\nInvalid file extension: " + filename
        return None
    return parser.Parse(filename)
    
def open_all_seq(dir_name):
    "Parses all of the sequence files in a directory, returning a list of ISequence objects."
    seq_list = []
    for filename in os.listdir(dir_name):
        seq_list.extend(open_seq(filename))
    return seq_list
    
def save_seq(seq_list, filename):
    "Saves a list of ISequence objects to file."
    filename = filename.Trim('"').Trim('\'')
    formatter = SequenceFormatters.FindFormatterByFile(filename)
    if formatter == None:
        raise Exception, "Failed to recognize sequence file extension: " + filename
    formatter.Format(seq_list, filename)
    
def save_all_seq(seq_list, dir_name, file_extension):
    "Saves a list of ISequence objects to separate files."
    for seq in seq_list:
        save_seq(filename_base + "\\" + seq.ID + file_extension, seq)

