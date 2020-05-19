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

SET WorkingFolder=%CD%
CD %BINARYROOT%
CD ..
IF EXIST .\Target RD /S /Q .\Target

REN Binaries Target

IF EXIST .\Target\Debug\MBFConsoleApplicationTemplate.zip del .\Target\Debug\MBFConsoleApplicationTemplate.zip
IF EXIST .\Target\Release\MBFConsoleApplicationTemplate.zip del .\Target\Release\MBFConsoleApplicationTemplate.zip

IF NOT EXIST .\Target\Debug\nul MD .\Target\Debug
%SOURCEROOT%\Public\ext\DotNetZipConsole\DotNetZipConsole .\Target\Debug\MBFConsoleApplicationTemplate.zip %SOURCEROOT%\MBF\MBF.TemplateWizard\MBFConsoleApplicationTemplate
xCopy /i /s /y .\Target\Debug\MBFConsoleApplicationTemplate*.zip .\Target\Release\MBFConsoleApplicationTemplate*.zip

echo ************************************************************
echo Copying CHM file
echo ************************************************************

MD .\Binaries\docs
IF NOT EXIST .\Target\Debug\Doctemp\doc\output\MBF.chm (
IF NOT EXIST .\Target\Release\Doctemp\doc\output\MBF.chm (
echo MBF.chm not found
)ELSE XCopy /s /y /i .\Target\Release\Doctemp\doc\output\*.chm .\Binaries\docs\*.chm
)ELSE XCopy /s /y /i .\Target\Debug\Doctemp\doc\output\*.chm .\Binaries\docs\*.chm

echo ************************************************************
echo Copying Docs
echo ************************************************************

XCopy /y /i %SOURCEROOT%\Doc\MBF_*.docx .\Binaries\docs\
XCopy /y /i %SOURCEROOT%\Doc\MSR_Sequence_Assembler_*.docx .\Binaries\docs\

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
XCopy /y /i .\Target\Debug\MBF*.dll .\Binaries\Binaries\debug\MBF*.dll
XCopy /y /i .\Target\Debug\MBF.TestUtils*.dll .\Binaries\Binaries\debug\MBF.TestUtils*.dll
XCopy /y /i .\Target\Debug\MBF.Tests*.dll .\Binaries\Binaries\debug\MBF.Tests*.dll
XCopy /y /i .\Target\Debug\MBF.TestAutomation*.dll .\Binaries\Binaries\debug\MBF.TestAutomation*.dll

XCopy /y /i .\Target\Debug\MBF.WebServiceHandlers*.dll .\Binaries\Binaries\debug\MBF.WebServiceHandlers*.dll

XCopy /y /i .\Target\Debug\MBF.PaDeNA*.dll .\Binaries\Binaries\debug\MBF.PaDeNA*.dll

XCopy /y /i .\Target\Debug\MBF.Pamsam*.dll .\Binaries\Binaries\debug\MBF.Pamsam*.dll

XCopy /y /i .\Target\Debug\MBF.Workflow*.dll .\Binaries\Binaries\debug\MBF.Workflow*.dll

XCopy /y /i .\Target\Debug\ReadGenerator*.exe .\Binaries\Binaries\debug\ReadGenerator*.exe

XCopy /y /i .\Target\Debug\SequenceAssembler*.exe .\Binaries\Binaries\debug\SequenceAssembler*.exe
XCopy /y /i .\Target\Debug\QUT*.dll .\Binaries\Binaries\debug\QUT*.dll
XCopy /y /i .\Target\Debug\WPFToolkit*.dll .\Binaries\Binaries\debug\WPFToolkit*.dll

XCopy /y /i .\Target\Debug\MBT.VennToNodeXL*.dll .\Binaries\Binaries\debug\MBT.VennToNodeXL*.dll
XCopy /y /i .\Target\Debug\BedStats*.exe .\Binaries\Binaries\debug\BedStats*.exe
XCopy /y /i .\Target\Debug\BedStats.exe.* .\Binaries\Binaries\debug\BedStats.exe.*
XCopy /y /i .\Target\Debug\VennTool*.exe .\Binaries\Binaries\debug\VennTool*.exe
XCopy /y /i .\Target\Debug\VennTool.exe.* .\Binaries\Binaries\debug\VennTool.exe.*

XCopy /y /i .\Target\Debug\MBF.TemplateWizard*.dll .\Binaries\Binaries\debug\MBF.TemplateWizard*.dll
XCopy /y /i .\Target\Debug\MBFConsoleApplicationTemplate*.zip .\Binaries\Binaries\debug\MBFConsoleApplicationTemplate*.zip

XCopy /y /i .\Target\Debug\*.config .\Binaries\Binaries\debug\*.config

XCopy /y /i .\Target\Debug\SAMUtils*.exe .\Binaries\Binaries\debug\SAMUtils*.exe

echo ************************************************************
echo copying Release Binaries
echo ************************************************************

mkdir .\Binaries\Binaries\release

XCopy /y /i .\Target\release\MBF*.dll .\Binaries\Binaries\release\MBF*.dll
XCopy /y /i .\Target\release\MBF.TestUtils*.dll .\Binaries\Binaries\release\MBF.TestUtils*.dll
XCopy /y /i .\Target\release\MBF.Tests*.dll .\Binaries\Binaries\release\MBF.Tests*.dll
XCopy /y /i .\Target\release\MBF.TestAutomation*.dll .\Binaries\Binaries\release\MBF.TestAutomation*.dll

XCopy /y /i .\Target\release\MBF.WebServiceHandlers*.dll .\Binaries\Binaries\release\MBF.WebServiceHandlers*.dll

XCopy /y /i .\Target\release\MBF.PaDeNA*.dll .\Binaries\Binaries\release\MBF.PaDeNA*.dll

XCopy /y /i .\Target\release\MBF.Pamsam*.dll .\Binaries\Binaries\release\MBF.Pamsam*.dll

XCopy /y /i .\Target\release\MBF.Workflow*.dll .\Binaries\Binaries\release\MBF.Workflow*.dll

XCopy /y /i .\Target\release\ReadGenerator*.exe .\Binaries\Binaries\release\ReadGenerator*.exe

XCopy /y /i .\Target\release\SequenceAssembler*.exe .\Binaries\Binaries\release\SequenceAssembler*.exe
XCopy /y /i .\Target\release\QUT*.dll .\Binaries\Binaries\release\QUT*.dll
XCopy /y /i .\Target\release\WPFToolkit*.dll .\Binaries\Binaries\release\WPFToolkit*.dll

XCopy /y /i .\Target\release\MBT.VennToNodeXL*.dll .\Binaries\Binaries\release\MBT.VennToNodeXL*.dll
XCopy /y /i .\Target\release\BedStats*.exe .\Binaries\Binaries\release\BedStats*.exe
XCopy /y /i .\Target\release\BedStats.exe.* .\Binaries\Binaries\release\BedStats.exe.*
XCopy /y /i .\Target\release\VennTool*.exe .\Binaries\Binaries\release\VennTool*.exe
XCopy /y /i .\Target\release\VennTool.exe.* .\Binaries\Binaries\release\VennTool.exe.*

XCopy /y /i .\Target\release\MBF.TemplateWizard*.dll .\Binaries\Binaries\release\MBF.TemplateWizard*.dll
XCopy /y /i .\Target\release\MBFConsoleApplicationTemplate*.zip .\Binaries\Binaries\release\MBFConsoleApplicationTemplate*.zip

XCopy /y /i .\Target\release\*.config .\Binaries\Binaries\release\*.config

XCopy /y /i .\Target\release\SAMUtils*.exe .\Binaries\Binaries\release\SAMUtils*.exe

echo ************************************************************
echo Copying UnitTest Files
echo ************************************************************

mkdir .\Binaries\Binaries\debug\TestData
mkdir .\Binaries\Binaries\release\TestData
XCopy /s /y /i .\Target\Debug\TestData\*.* .\Binaries\Binaries\debug\TestData\*.*
XCopy /s /y /i .\Target\release\TestData\*.* .\Binaries\Binaries\release\TestData\*.*

echo ************************************************************
echo Copying TestAutomation Files
echo ************************************************************

mkdir .\Binaries\Binaries\debug\TestUtils
mkdir .\Binaries\Binaries\release\TestUtils
XCopy /s /y /i .\Target\Debug\TestUtils\*.* .\Binaries\Binaries\debug\TestUtils\*.*
XCopy /s /y /i .\Target\release\TestUtils\*.* .\Binaries\Binaries\release\TestUtils\*.*

IF %CopySource% NEQ "TRUE" goto SkipCopySource
echo ************************************************************
echo Copying Source files
echo ************************************************************
mkdir .\Binaries\Source

XCopy /s /y /i /EXCLUDE:%SOURCEROOT%\BuildScripts\excludelist.txt %SOURCEROOT%\MBF\*.* .\Binaries\Source\MBF\*.*
XCopy /s /y /i /EXCLUDE:%SOURCEROOT%\BuildScripts\excludelist.txt %SOURCEROOT%\MBT\*.* .\Binaries\Source\MBT\*.*

goto CopyLogFiles

:SkipCopySource
echo Skipped copying source file

:CopyLogFiles
echo ************************************************************
echo Copying log files
echo ************************************************************

MD .\Binaries\Logs

Copy %WorkingFolder%\BuildLog.txt .\Binaries\Logs

CD %WorkingFolder%
