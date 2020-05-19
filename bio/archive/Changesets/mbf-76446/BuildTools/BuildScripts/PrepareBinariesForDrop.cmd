REM -- ***********************************************************
REM --     Description
REM -- ***********************************************************
REM -- Prepares the folder structure as required in Drop location.
REM -- Copies required files to newly created folder structure.
REM -- ***********************************************************

@Echo Off
@if not "%ECHO%"=="" Echo %ECHO%

echo ************************************************************
echo Preparing folder structure.
echo ************************************************************

SET Local_BINARYROOT=%BINARYROOT%
SET Local_SOURCEROOT=%SOURCEROOT%
IF "%CopySource%"=="" SET CopySource=False

IF NOT "%1"=="" SET Local_BINARYROOT=%1
IF NOT "%2"=="" SET Local_SOURCEROOT=%2

IF "%Local_BINARYROOT%"=="" goto EnErr
IF "%Local_SOURCEROOT%"=="" goto EnErr

pushd "%Local_BINARYROOT%"
IF EXIST .\Target RD /S /Q .\Target

REN Binaries Target

echo ************************************************************
echo Copying Docs
echo ************************************************************

IF NOT EXIST .\Binaries\docs MKDIR .\Binaries\docs

XCopy /y /s /i "%Local_SOURCEROOT%\Doc\*.*" .\Binaries\docs\*.*

REM Copy CHM FILE
Copy /y .\Target\Docs\*.*  .\Binaries\docs\*.*


echo ************************************************************
echo Copying Symbols
echo ************************************************************

mkdir .\Binaries\symbols\debug

XCopy /s /y /i .\Target\Debug\*.pdb .\Binaries\symbols\debug\*.pdb

mkdir .\Binaries\symbols\release
XCopy /s /y /i .\Target\release\*.pdb .\Binaries\symbols\release\*.pdb


echo ************************************************************
echo copying Debug Binaries
echo ************************************************************

mkdir .\Binaries\Binaries\debug


XCopy /y /i .\Target\Debug\*.exe .\Binaries\Binaries\debug\*.exe
XCopy /y /i .\Target\Debug\*.config .\Binaries\Binaries\debug\*.config
XCopy /y /i .\Target\Debug\*.dll .\Binaries\Binaries\debug\*.dll
XCopy /y /i .\Target\Debug\BioConsoleApplicationTemplate*.zip .\Binaries\Binaries\debug\BioConsoleApplicationTemplate*.zip
XCopy /y /i .\Target\Debug\*.vsto .\Binaries\Binaries\debug\*.vsto
XCopy /y /i .\Target\Debug\*.bas .\Binaries\Binaries\debug\*.bas
XCopy /y /i .\Target\Debug\*.dll.manifest .\Binaries\Binaries\debug\*.dll.manifest


echo ************************************************************
echo copying Release Binaries
echo ************************************************************

mkdir .\Binaries\Binaries\release
XCopy /y /i .\Target\release\*.exe .\Binaries\Binaries\release\*.exe
XCopy /y /i .\Target\release\*.config .\Binaries\Binaries\release\*.config
XCopy /y /i .\Target\release\*.dll .\Binaries\Binaries\release\*.dll
XCopy /y /i .\Target\release\BioConsoleApplicationTemplate*.zip .\Binaries\Binaries\release\BioConsoleApplicationTemplate*.zip
XCopy /y /i .\Target\release\*.vsto .\Binaries\Binaries\release\*.vsto
XCopy /y /i .\Target\release\*.bas .\Binaries\Binaries\release\*.bas
XCopy /y /i .\Target\release\*.dll.manifest .\Binaries\Binaries\release\*.dll.manifest
XCopy /y /i .\Target\release\Microsoft.Office.Tools.Common.v4.0.Utilities*.xml .\Binaries\Binaries\release\Microsoft.Office.Tools.Common.v4.0.Utilities*.xml


echo ************************************************************
echo Copying UnitTest Files
echo ************************************************************

mkdir .\Binaries\Binaries\debug\TestData
mkdir .\Binaries\Binaries\release\TestData
XCopy /s /y /i .\Target\Debug\TestData\*.* .\Binaries\Binaries\debug\TestData\*.*
XCopy /s /y /i .\Target\release\TestData\*.* .\Binaries\Binaries\release\TestData\*.*


IF %CopySource% NEQ "TRUE" goto SkipCopySource
echo ************************************************************
echo Copying Source files
echo ************************************************************
mkdir .\Binaries\Source

XCopy /s /y /i /EXCLUDE:%Local_SOURCEROOT%\BuildTools\BuildScripts\ExcludeList.txt "%Local_SOURCEROOT%\Source\*.*" .\Binaries\Source\*.*
XCopy /s /y /i /EXCLUDE:%Local_SOURCEROOT%\BuildTools\BuildScripts\ExcludeList.txt "%Local_SOURCEROOT%\Tests\*.*" .\Binaries\Tests\*.*

goto CopyLogFiles

:SkipCopySource
echo Skipped copying source file

:CopyLogFiles
echo ************************************************************
echo Copying log files
echo ************************************************************

MD .\Binaries\Logs
XCopy /s /y /i .\Target\Debug\*.lastcodeanalysissucceeded .\Binaries\Logs\*.lastcodeanalysissucceeded
XCopy /s /y /i .\Target\Debug\*.CodeAnalysisLog.xml .\Binaries\Logs\*.CodeAnalysisLog.xml

popd

IF EXIST .\BuildLog.txt Copy .\BuildLog.txt .\Binaries\Logs

Goto END
REM Environment error.
:EnErr 
Echo Environment variables not found.

:END
SET Local_BINARYROOT=
SET Local_SOURCEROOT=
