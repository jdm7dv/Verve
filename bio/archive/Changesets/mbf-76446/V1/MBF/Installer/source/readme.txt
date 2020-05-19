Microsoft Biology Foundation: Readme.txt
Version 1.0, June 2010

The Microsoft Biology Foundation (MBF) is an open source, reusable .NET library and application programming interface (API) for bioinformatics research.

The Microsoft Biology Foundation is available at http://mbf.codeplex.com.  It is licensed under the OSI approved MS-PL, found here:  http://mbf.codeplex.com/license.

KNOWN ISSUES
============

- It is highly recommended to install the MBF library before installing the tools if development work is going to be performed using the library.  It is also highly recommended to uninstall the tools prior to uninstalling the library.  Failure to do so could cause components to end up on multiple locations.

- “Cancelling” an algorithm from an application will give the impression that the processing has stopped and control of the app goes back to the user, but in fact the algorithm will continue to process until completed and consume CPU/memory resources until completed. The apps will ignore any response (success or failure), but it may be observed that performance is impacted until the process is complete.

- If you have installed the Beta 2 version of any of the MBF components or tools (Library, SDK, Sequence Assembler or Excel add-in), be sure to uninstall the .NET 4 Beta 2 components before installing the final v1.0 version of these MBF components.

- The data virtualization system in MBF supports files up to 2GB in size.  Attempting to work with files larger than this may cause instability on machines with insufficient memory.
