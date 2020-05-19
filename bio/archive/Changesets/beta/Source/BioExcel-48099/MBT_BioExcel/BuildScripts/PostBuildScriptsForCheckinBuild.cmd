REM -- *************************************************************************
REM --     Description
REM -- *************************************************************************
REM -- This will be called Whenever a checkin build happens.
REM -- Prepares the folder structure as required in Drop location and 
REM -- copies required files to newly created folder structure.
REM -- *************************************************************************

@echo off
echo ************************************************************
echo  Post build script Started.
echo ************************************************************

if "%1" == "" GOTO EOF
if "%2" == "" GOTO EOF

SET SOURCEROOT=%1
SET BINARYROOT=%2

SET SilentValidation=

if "%3" == "true" SET SilentValidation="/S"

"%SOURCEROOT%\MBT_BioExcel\BuildScripts\DeveloperPreRequisiteCheck.exe" %SilentValidation%

IF EXIST ResetEnvironmentVariable.bat CALL ResetEnvironmentVariable.bat

echo ************************************************************
echo Preparing folder structure.
echo ************************************************************

SET WorkingFolder=%CD%
CD %BINARYROOT%
CD ..
IF EXIST .\Target RD /S /Q .\Target

REN Binaries Target

echo ************************************************************
echo Copying Symbols
echo ************************************************************

mkdir .\Binaries\symbols\debug
XCopy /s /y /i .\Target\Debug\*.pdb .\Binaries\symbols\debug\*.pdb

mkdir .\Binaries\symbols\release
XCopy /s /y /i .\Target\release\*.pdb .\Binaries\symbols\release\*.pdb

echo ************************************************************
echo copying Binaries
echo ************************************************************

mkdir .\Binaries\Binaries\debug

XCopy /y /i .\Target\Debug\*.xml .\Binaries\Binaries\debug\*.xml

XCopy /s /y /i .\Target\Debug\*.dll .\Binaries\Binaries\debug\*.dll
XCopy /y /i .\Target\Debug\*.vsto .\Binaries\Binaries\debug\*.vsto
XCopy /y /i .\Target\Debug\*.bas .\Binaries\Binaries\debug\*.bas
XCopy /y /i .\Target\Debug\*.dll.manifest .\Binaries\Binaries\debug\*.dll.manifest


mkdir .\Binaries\Binaries\release

XCopy /y /i .\Target\release\*.xml .\Binaries\Binaries\release\*.xml

XCopy /s /y /i .\Target\release\*.dll .\Binaries\Binaries\release\*.dll

XCopy /y /i .\Target\release\*.dll.manifest .\Binaries\Binaries\release\*.dll.manifest
XCopy /y /i .\Target\release\*.vsto .\Binaries\Binaries\release\*.vsto
XCopy /y /i .\Target\release\*.bas .\Binaries\Binaries\release\*.bas


:CopyLogFiles
echo ************************************************************
echo Copying log files
echo ************************************************************

MD .\Binaries\Logs

Copy %WorkingFolder%\BuildLog.txt .\Binaries\Logs

CD %WorkingFolder%

:EOF
set errorlevel=0
echo ************************************************************
echo  Post build script completed.
echo ************************************************************

