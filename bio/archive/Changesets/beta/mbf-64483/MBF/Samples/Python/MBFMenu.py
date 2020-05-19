#// *****************************************************************
#//    Copyright (c) Microsoft. All rights reserved.
#//    This code is licensed under the Microsoft Public License.
#//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
#//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
#//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
#//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
#// *****************************************************************

import clr
import sys
import time

# Adding the dll reference will throw an exception if we're debugging in VS from the Python
# development dir, instead of the standard non-dev method of running from the bin\Debug dir or an
# installation dir.
try:
    clr.AddReferenceToFile("MBF.IronPython.dll")
except:
    pass

from MBFIronPython.Algorithms import *
from MBFIronPython.IO import *
from MBFIronPython.Util import *
from MBFIronPython.Web import *
from MBFIronPython.SequenceManipulationApplications import *
from MBFIronPython.ListOr import *
from MBFIronPython.MBFDemo import *

again = "y"
while "yY".find(again[0]) != -1:

   option = ""
   
   # Ensuring that the user chooses a number between 1 and 6
   while(option < "1" or option > "6"):
        print "--------------------------------------------------------"
        print "\nPlease choose the application that you want to run:"
        print "\n1-> MBF integerated demo"
        print "\n2-> Concatenate sequences"
        print "\n3-> Strip non-alphabetic characters in a sequence"
        print "\n4-> Remove Poly-A tail from a sequence"
        print "\n5-> Perform logical union of two sequence files"
        print "\n6-> Find differences between two sequences\n"
        print "--------------------------------------------------------"
        
        option = raw_input("\n\nPlease enter the application number (1-6):")
        
   if option == "1":
      MBFDemo() 
   elif option == "2":
      ConcatenateSequences() 
   elif option == "3":
      StripNonAlphabets()
   elif option == "4":
      RemovePolyATail()
   elif option == "5":
      ListOr()
   elif option == "6":
      DiffSeq()
      
   again = " "
   option = ""
   while "yYnN".find(again[0]) == -1:
        again = raw_input("Would you like to choose applications again? (y/n): ")
        if len(again) == 0:
            again = " "
            
