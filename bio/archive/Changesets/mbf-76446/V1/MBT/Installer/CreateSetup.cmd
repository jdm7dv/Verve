@Echo Off
@if not "%ECHO%"=="" Echo %ECHO%

echo ************************************************************
echo Creating MBF Setup Locally - Start
echo ************************************************************
CALL Clear.cmd

SET SPATH=..\..
SET BPATH=..\..\Build\Binaries

REM Get Absolute paths.
SET PPTAH=%CD%

CD %SPATH%
SET SPATH=%CD%

CD %PPTAH%
CD %BPATH%
SET BPATH=%CD%

CD %PPTAH%

:CHECK
PUSHD %BPATH%\Release

if exist *.vshost.exe (
del *.vshost.exe )

if exist *.vshost.exe (
echo **************************************************************
echo ERROR: A host process is holding certain required resources in the release binaries folder.
echo Please close the MBF\MBI solution before proceeding.
echo **************************************************************
pause 
POPD
GOTO CHECK ) else ( POPD )

set errorlevel=0

PUSHD %BPATH%\Debug

if exist *.vshost.exe (
del *.vshost.exe )

if exist *.vshost.exe (
echo **************************************************************
echo ERROR: A host process is holding certain required resources in the debug binaries folder.
echo Please close the MBF\MBI solution before proceeding.
echo **************************************************************
pause 
POPD
GOTO CHECK ) else ( POPD )

set errorlevel=0

Echo %SPATH%
Echo %BPATH%
Echo %PPTAH%

CALL ..\..\BuildScripts\PostBuildScriptsForDailyBuild.cmd %SPATH% %BPATH%

echo ************************************************************
echo Creating MBF Setup Locally - End
echo ************************************************************
