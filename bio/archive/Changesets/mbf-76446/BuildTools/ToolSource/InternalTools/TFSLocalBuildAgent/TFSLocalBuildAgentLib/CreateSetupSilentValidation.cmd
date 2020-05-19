@Echo Off
@if not "%ECHO%"=="" Echo %ECHO%

echo ************************************************************
echo Creating MBT Setups Locally - Start
echo ************************************************************

PUSHD %1	

PUSHD ..\..\..
SET MBIROOT=%CD%
echo %cd%
POPD

set BINARYPATH=%MBIROOT%\Build\LocalBuild

%MBIROOT%\Buildtools\Bin\DeveloperPreRequisiteCheck.exe /S
IF %ERRORLEVEL% NEQ 0 GOTO PREREQERROR

call %MBIROOT%\BuildTools\BuildScripts\BuildMBI.cmd %MBIROOT%\Build\LocalBuild %MBIROOT%
IF %ERRORLEVEL% NEQ 0 GOTO END

Echo %MBIROOT%
Echo %BINARYPATH%

CALL %MBIROOT%\BuildTools\BuildScripts\PostBuildScriptsForDailyBuild.cmd %MBIROOT% %BINARYPATH% MBT true

echo ************************************************************
echo Creating MBT Setups Locally - End
echo ************************************************************

GOTO END

:PREREQERROR
echo -----------------------------------------------------------------------------
echo Please install the missing prerequisite(s) and run this script again.
echo -----------------------------------------------------------------------------

:END