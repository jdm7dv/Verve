REM -- ****************************************************************************************************
REM --     Description
REM -- ****************************************************************************************************
REM -- This script will be called by TFS on Daily build or by CreateSetup.cmd script in installer folder.
REM -- This will internally calls DocForTFS.cmd , PrepareBinariesForDrop.cmd and PreWixScript.cmd.
REM -- ****************************************************************************************************

@Echo Off
@if not "%ECHO%"=="" Echo %ECHO%

echo ************************************************************
echo  Post build script Started.
echo ************************************************************

if "%1" == "" GOTO EOF
if "%2" == "" GOTO EOF

SET SOURCEROOT=%1
SET BINARYROOT=%2
SET SETUP_MBF_ONLY=%3

SET CreateDOC="TRUE"
SET CopySource="TRUE"

SET SilentValidation=
if "%4" == "true" SET SilentValidation="/S"

%SOURCEROOT%\BuildScripts\DeveloperPreRequisiteCheck.exe %SilentValidation%

IF EXIST ResetEnvironmentVariable.bat CALL ResetEnvironmentVariable.bat

if %CreateDOC% NEQ "TRUE" goto SkipCreateDoc
call %SOURCEROOT%\BuildScripts\DocForTFS.cmd

goto PrepareFolderForDrop
:SkipCreateDoc
echo skipped Doc creating

:PrepareFolderForDrop
call %SOURCEROOT%\BuildScripts\PrepareBinariesForDrop.cmd

SET WorkingFolder=%CD%
CD %BINARYROOT%
CD ..

REM --- Call WIX pre build scripts here
CALL %SOURCEROOT%\BuildScripts\PreWixScript.cmd %CD%\Binaries %CD% %SETUP_MBF_ONLY%

SET SETUPTMPFOLDER=%CD%\setup.tmp
MD %BINARYROOT%\Installer

REM --- CALL MBF WIX setup here
CD %SOURCEROOT%\MBF\Installer

call make.cmd %SETUPTMPFOLDER%

REM -- Copy the mbf Installer under %BINARYROOT%\Installer
Copy /y .\MBF.msi %BINARYROOT%\Installer\MBF.msi

IF "%SETUP_MBF_ONLY%" == "true" GOTO SkipMBTSetup

CD %BINARYROOT%
CD ..

REM --- CALL MBT WIX setup here
CD %SOURCEROOT%\MBT\Installer
call make.cmd %SETUPTMPFOLDER%

REM -- Copy the Installers under %BINARYROOT%\Installer
Copy /y .\SequenceAssembler.msi %BINARYROOT%\Installer\SequenceAssembler.msi

:SkipMBTSetup
CD %WorkingFolder%

:EOF
set errorlevel=0
echo ************************************************************
echo  Post build script completed.
echo ************************************************************
