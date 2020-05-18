Const ForReading = 1
Const ForWriting = 2

Set objFSO = CreateObject("Scripting.FileSystemObject")
fullpath = objFSO.GetAbsolutePathName("..")
Set objFile = objFSO.OpenTextFile(fullpath & "\configure.vbs", ForReading)

strText = objFile.ReadAll
objFile.Close

strNewText = Replace(strText, "\singularity.ico","\windows.ico")

Set objFile = objFSO.OpenTextFile(fullpath & "\configure.vbs", ForWriting)
objFile.WriteLine strNewText 

objFile.Close
