<html>
<body>
<pre>
<h1>Build Log</h1>
<h3>
--------------------Configuration: NoteDemo - Win32 (WCE ARMV4) Debug--------------------
</h3>
<h3>Command Lines</h3>
Creating command line "rc.exe /l 0x409 /fo"ARMV4Dbg/NoteDemo.res" /d UNDER_CE=410 /d _WIN32_WCE=410 /d "DEBUG" /d "UNICODE" /d "_UNICODE" /d "WCE_PLATFORM_SA_TABLET" /d "ARM" /d "_ARM_" /d "ARMV4" /r "C:\Book Stuff\CeBook30\11. Notifications\NoteDemo\NoteDemo.rc"" 
Creating temporary file "C:\DOCUME~1\Doug\LOCALS~1\Temp\RSP110C.tmp" with contents
[
/nologo /W3 /Zi /Od /D "DEBUG" /D "ARM" /D "_ARM_" /D "ARMV4" /D UNDER_CE=410 /D _WIN32_WCE=410 /D "WCE_PLATFORM_SA_TABLET" /D "UNICODE" /D "_UNICODE" /Fp"ARMV4Dbg/NoteDemo.pch" /YX /Fo"ARMV4Dbg/" /Fd"ARMV4Dbg/" /MC /c 
"C:\Book Stuff\CeBook30\11. Notifications\NoteDemo\NoteDemo.cpp"
]
Creating command line "clarm.exe @C:\DOCUME~1\Doug\LOCALS~1\Temp\RSP110C.tmp" 
Creating temporary file "C:\DOCUME~1\Doug\LOCALS~1\Temp\RSP110D.tmp" with contents
[
commctrl.lib coredll.lib /nologo /base:"0x00010000" /stack:0x10000,0x1000 /entry:"WinMainCRTStartup" /incremental:yes /pdb:"ARMV4Dbg/NoteDemo.pdb" /debug /nodefaultlib:"libc.lib /nodefaultlib:libcd.lib /nodefaultlib:libcmt.lib /nodefaultlib:libcmtd.lib /nodefaultlib:msvcrt.lib /nodefaultlib:msvcrtd.lib" /out:"ARMV4Dbg/NoteDemo.exe" /subsystem:windowsce,4.10 /align:"4096" /MACHINE:ARM 
".\ARMV4Dbg\NoteDemo.obj"
".\ARMV4Dbg\NoteDemo.res"
]
Creating command line "link.exe @C:\DOCUME~1\Doug\LOCALS~1\Temp\RSP110D.tmp"
<h3>Output Window</h3>
Compiling resources...
Compiling...
NoteDemo.cpp
Linking...




<h3>Results</h3>
NoteDemo.exe - 0 error(s), 0 warning(s)
</pre>
</body>
</html>
