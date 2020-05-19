REM -- ****************************************
REM --     Description
REM -- ****************************************
REM -- Generates chm file for MBF.dll. 
REM -- This script is called by DocForTFS.cmd.
REM -- ****************************************

@Echo Off
@if not "%ECHO%"=="" Echo %ECHO%

REM -- SET Env vars
IF EXIST ResetEnvironmentVariable.bat CALL ResetEnvironmentVariable.bat

echo ************************************************************
echo  Generating class documentation...
echo ************************************************************

REM Try to generate doc from debug if not possible, try to generate it from release binaries.

REM -- CALL Doc geneartor for Debug binaries
IF NOT EXIST %BINARYROOT%\Debug\MBF.dll GOTO ReleaseDoc
mkdir %BINARYROOT%\Debug\Doctemp

copy %BINARYROOT%\Debug\MBF.dll %BINARYROOT%\Debug\Doctemp\MBF.dll
copy %BINARYROOT%\Debug\MBF.xml %BINARYROOT%\Debug\Doctemp\MBF.xml

call %SOURCEROOT%\BuildScripts\DocGenForTFS.cmd %BINARYROOT%\Debug\Doctemp %SOURCEROOT%

IF EXIST %BINARYROOT%\Debug\Doctemp\doc\output\MBF.chm goto EOF

:ReleaseDoc
REM -- CALL Doc geneartor for Release binaries
mkdir %BINARYROOT%\Release\Doctemp

copy %BINARYROOT%\Release\MBF.dll %BINARYROOT%\Release\Doctemp\MBF.dll
copy %BINARYROOT%\Release\MBF.xml %BINARYROOT%\Release\Doctemp\MBF.xml

call %SOURCEROOT%\BuildScripts\DocGenForTFS.cmd %BINARYROOT%\Release\Doctemp %SOURCEROOT%

IF NOT EXIST %BINARYROOT%\Release\Doctemp\doc\output\MBF.chm GOTO DOCERROR
goto EOF

:DOCERROR
ECHO Failed to generate MBF SDK Documentation 

:EOF