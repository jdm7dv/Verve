import Util
Util.add_mbfdotnet_reference("MBF")
from MBF import *
from MBF.Algorithms import *
from MBF.Algorithms.Alignment import *
from MBF.Algorithms.Assembly import *
from MBFIronPython.Algorithms import *
from MBFIronPython.IO import *
from MBFIronPython.Util import *
from MBFIronPython.Web import *
from MBFIronPython.SequenceManipulationApplications import *
import string
from System.Collections.Generic import List

def ListOr():
    "Reads in two sets of sequences and writes out a list file that result from the logical union of the two sets"
    
    # This program reads two files and compares the sequences according to the following rule:
    # OR  - gives all the sequences that occur in either file.
    # AND - gives only those sequences which occur in both files.
    # XOR - gives those which only occur in either file, but not in both.
    # NOT - gives those which occur in the first file but not in the second.

    print ListOr.__doc__

    # Get the first file name
    firstFile = GetInputFileName("\nPlease enter the first sequence filename: ")
    
    # Get a list of all sequences in the string
    firstSet = open_seq(firstFile)
    if firstSet == None:
        firstFile = GetInputFileName("\nInvalid filename. Please enter the first sequence filename: ")
        firstSet = open_seq(firstFile)
        if firstSet == None:
            return None

    # Get the second file name
    secondFile = GetInputFileName("\nPlease enter the second sequence filename: ")
    
    secondSet = open_seq(secondFile)
    if secondSet == None:
        secondFile = GetInputFileName("\nnInvalid filename. Please enter the second sequence filename: ") 
        secondSet = open_seq(secondFile)
        if secondSet == None:
            return None

    option = ""
    again = "y"
    while "yY".find(again[0]) != -1:    
        outputFile = GetInputFileName("\nPlease enter the output filename: ")
        
        # Ensuring that the user chooses a number between 1 and 4
        while(option < "1" or option > "4"):
            print "\nPlease choose the logical union type by pressing the correct digit:"
            option = raw_input("1-> OR 2-> AND 3-> XOR 4->NOT:\n")

            # Perform logical-union
            if option == "1":
                outputSet = PerformOrOperation(firstSet, secondSet)
            elif option == "2":
                outputSet = PerformAndOperation(firstSet, secondSet)
            elif option == "3":
                outputSet = PerformXorOperation(firstSet, secondSet)
            elif option == "4":
                outputSet = PerformNotOperation(firstSet, secondSet)

            # Convert a python list to C-sharp list
            c_sharp_list = List[ISequence](outputSet)    

            # Save the sequence to the outputFile
            save_seq(c_sharp_list, outputFile)

            print "\nThe file containing the logical union is stored at:",outputFile 
        again = " "
        option = ""
        while "yYnN".find(again[0]) == -1:
            again = raw_input("Would you like to perform another logical operation? (y/n): ")
            if len(again) == 0:
                again = " "

def PerformOrOperation(firstSet, secondSet):

    "PerformOrOperation gives all the sequences that occur in either file.\nFor eg File1 has :'AAATAA' and 'AAAAAA'\nFile2 has 'AAATAA' and 'AAAGAA'. The output is 'AAATAA', 'AAAAAA' and 'AAAGAA'"
    
    print PerformOrOperation.__doc__
    
    # Add all sequences in the first set to the output list
    finalList = list(firstSet)
    
    # Hashtable which stores the mapping of sequence-string v\s ISequence object.
    sequenceStringToSequence = {}
    
    # Add all the sequences in the first file to the hashtable
    for sequence in firstSet:
        sequenceString = sequence.ToString()
        sequenceStringToSequence[sequenceString.upper()] = sequence
    
    # For every sequence in the second-set
    for newSequence in secondSet:
        sequenceString = newSequence.ToString()
        
        # Check if the sequence is present in the hash-table
        present = sequenceStringToSequence.get(sequenceString.upper(), None)
        
        # If not present then add to the final list
        if present == None:
           finalList.append(newSequence)
    
    # Return the final list       
    return finalList
    
def PerformAndOperation(firstSet, secondSet):
    "PerformAndOperation gives only those sequences which occur in both files.\nFor eg File1 has :'AAATAA' and 'AAAAAA'\nFile2 has 'AAATAA' and 'AAAGAA'. The output is 'AAATAA'"
    
    print PerformAndOperation.__doc__

    finalList = list()
    
    # Hashtable which stores the mapping of sequence-string v\s ISequence object.
    sequenceStringToSequence = {}
    
    # Add all the sequences in the first file to the hashtable
    for sequence in firstSet:
        sequenceString = sequence.ToString()
        sequenceStringToSequence[sequenceString.upper()] = sequence
    
    # For every sequence in the second-set
    for newSequence in secondSet:
        sequenceString = newSequence.ToString()
        
        # Check if the sequence is present in the hash-table
        present = sequenceStringToSequence.get(sequenceString.upper(), None)
        
        # If present then add to the final list      
        if present != None:
            finalList.append(newSequence)   
            
    # Return the final list   
    return finalList
 
def PerformXorOperation(firstSet, secondSet):
    "PerformXorOperation gives those which only occur in either file, but not in both.\nFor eg File1 has :'AAATAA' and 'AAAAAA'\nFile2 has 'AAATAA' and 'AAAGAA'. The output is 'AAAAAA' and 'AAAGAA'"
    
    print PerformXorOperation.__doc__
 
    # Add all sequences in the first set to the output list
    finalList = list(firstSet)
    
    # Hashtable which stores the mapping of sequence-string v\s ISequence object.
    sequenceStringToSequence = {}
    
    # Add all the sequences in the first file to the hashtable
    for sequence in firstSet:
        sequenceString = sequence.ToString()
        sequenceStringToSequence[sequenceString.upper()] = sequence
    
    # For every sequence in the second-set
    for newSequence in secondSet:
        sequenceString = newSequence.ToString()
        
        # Check if the sequence is present in the hash-table
        present = sequenceStringToSequence.get(sequenceString.upper(), None)
        
        # If the sequence is not present in the hash-table
        # then add it to the final list
        if present == None:
            finalList.append(newSequence)
        else:
        
            # Remove it from the final-list.
            finalList.remove(present)
    
    # Return the final list         
    return finalList        

def PerformNotOperation(firstSet, secondSet):

    "PerformNotOperation gives those which only occur only in first file, but not in both.\nFor eg File1 has :'AAATAA' , 'AAAACA' and 'AAAAAA'\nFile2 has 'AAATAA' and 'AAAGAA'. The output is 'AAAACA' and 'AAAAAA'"
    
    print PerformNotOperation.__doc__

    # Add all sequences in the first set to the output list
    finalList = list(firstSet)
    
    # Hashtable which stores the mapping of sequence-string v\s ISequence object.
    sequenceStringToSequence = {}
    
    # Add all the sequences in the first file to the hashtable
    for sequence in firstSet:
        sequenceString = sequence.ToString()
        sequenceStringToSequence[sequenceString.upper()] = sequence
    
    # For every sequence in the second-set
    for newSequence in secondSet:
        sequenceString = newSequence.ToString()        
        present = sequenceStringToSequence.get(sequenceString.upper(), None)
        
        # If present then remove the final list
        if present != None:
            finalList.remove(present)
     
     # Return the final list           
    return finalList     