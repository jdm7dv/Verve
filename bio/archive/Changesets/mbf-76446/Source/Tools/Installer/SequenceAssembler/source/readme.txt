Sequence Assembler: Readme.txt
Version 2.0 Beta1 , April 2011

The Microsoft Research Sequence Assembler is a proof-of-concept-application that demonstrates how the Microsoft Biology Foundation can be used to create applications for bioinformatics research. It uses rich UI elements to enable the manipulation and visualization of genomic data. 

The Sequence Assembler implements several features of the Microsoft Biology Foundation: a set of parsers for common genome file formats; a set of algorithms for alignment and/or assembly of DNA, RNA or Protein strands; and a set of connectors to several Basic Local Alignment Search Tool (BLAST) Web services for genomic identification. Reports from BLAST services can be viewed as single-line reports, or in the SilverMap visualization component integrated by the Queensland University of Technology.

The Sequence Assembler is available at http://mbf.codeplex.com.  It is licensed under the OSI approved Apache License, which can be found here:  http://mbf.codeplex.com/license.

The SilverMap visualization component can be found at http://qutbio.codeplex.com/.  It is  licensed under the OSI approved MS-PL, which can be found here:  http://qutbio.codeplex.com/license.


KNOWN ISSUES
============

- “Canceling” an algorithm while in progress will give the impression that the processing has stopped, but in fact the algorithm will continue to process and consume CPU/memory resources until completed. The application will act as if it is cancelled by ignoring any response (success or failure), but performance may be impacted until the process is completed.

- Opening a file with a very large number of sequences, or a file with a very large amount of non-sequence (textual) information, may cause the Sequence Assembler to hang. 

- If attempting to cancel the load of a very large file, the application may appear to hang while it finishes loading the file before it can cancel the load operation.  This is a known threading issue.

- AzureBLAST Web service functionality has been disabled by default, as the service is not normally available and is intended only for test and evaluation purposes. To enable this option in Sequence Assembler, set an environment variable in a command prompt with this command before launching the application:

  SET ENABLE_AZUREBLAST=true

To disable the AzureBLAST web service, use this command:
  
  SET ENABLE_AZUREBLAST=

- When using NUCMer to perform sequence alignment, un-aligned sequences will not be reported the application's tree view.

- When using the PaDeNa Assembler, un-aligned regions of sequences in contgs will be displayed in lower-case, while the aligned regions will remain in upper case.
