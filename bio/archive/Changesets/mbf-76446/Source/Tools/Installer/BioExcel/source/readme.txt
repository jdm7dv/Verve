Microsoft Research Biology Extension for Excel: Readme.txt
Version 2.0 Beta1 , April 2011

This document describes how to use the Microsoft Research Biology Extension for Excel, a Microsoft® Office Excel 2007/2010 add-in that provides a simple and flexible way to work with genomic sequences, metadata, and interval data in an Excel document.

The Biology Extension implements several features of the Microsoft Biology Foundation: a set of parsers for common genome file formats; a set of sequencing algorithms for assembly of a consensus DNA strand; a set of connectors to several Basic Local Alignment Search Tool (BLAST) Web services for genome identification; and genomic interval functions that allow the manipulation of BED files inside Excel. 

The Biology Extension can be programmatically extended to use other features in the Microsoft Biology Foundation as well. The Biology Extension is available at http://bioexcel.codeplex.com, and is licensed under the MS-LPL terms which can be found here:  http://bioexcel.codeplex.com/license.

KNOWN ISSUES
============

- In order for the Venn diagram generator to work in BioExcel, you must install the NodeXL runtime from the NodeXL site, found here:  http://nodexl.codeplex.com/.

- Make sure to enable add-ins in Excel to ensure the BioExcel ribbon “Bioinformatics” appears. See this link for details on how to enable add-ins in Excel:  http://office.microsoft.com/en-us/excel/HA100341271033.aspx.

- “Canceling” an algorithm while in progress will give the impression that the processing has stopped, but in fact the algorithm will continue to process and consume CPU/memory resources until completed. The application will act as if it is cancelled by ignoring any response (success or failure), but performance may be impacted until the process is completed.

- AzureBLAST Web service functionality has been disabled by default, as the service is not normally available and is intended only for test and evaluation purposes. To enable this option in the add-in, set an environment variable in a command prompt with this command before launching Excel:

  SET ENABLE_AZUREBLAST=true

To disable the AzureBLAST web service, use this command:
  
  SET ENABLE_AZUREBLAST=
