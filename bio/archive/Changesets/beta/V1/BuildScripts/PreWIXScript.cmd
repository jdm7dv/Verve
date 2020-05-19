REM -- ********************************************************************************
REM --     Description
REM -- ********************************************************************************
REM -- Prepares folder structure required for installer and copies the required files.
REM -- ********************************************************************************


@Echo Off
@if not "%ECHO%"=="" Echo %ECHO%

echo ************************************************************
echo Preparing folder structure.
echo ************************************************************

SET SourceFolder=%1
SET TargetFolder=%2
SET SETUP_MBF_ONLY=%3

CD %TargetFolder%

IF EXIST .\Setup.Tmp RMDIR /S /Q .\Setup.Tmp
MD .\Setup.Tmp
echo ************************************************************
echo Copying Framework binaries
echo ************************************************************

SET MBFFolder=".\Setup.Tmp\Microsoft Biology Foundation"

MD %MBFFolder%

echo ************************************************************
echo Copying Visual Studio template
echo ************************************************************
Xcopy /y /i %SourceFolder%\Binaries\Release\MBF.TemplateWizard.dll %MBFFolder%
Xcopy /y /i %SourceFolder%\Binaries\Release\MBFConsoleApplicationTemplate.zip %MBFFolder%

Xcopy /y /i %SourceFolder%\Binaries\Release\MBF.dll %MBFFolder%\Framework\
XCopy /y /i %SourceFolder%\Binaries\Release\MBF.WebServiceHandlers.dll %MBFFolder%\Framework\

echo ************************************************************
echo Copying Add-ins Source
echo ************************************************************

MD %MBFFolder%\Framework\Add-ins
XCopy /y /i %SourceFolder%\Binaries\Release\MBF.PaDeNA.dll %MBFFolder%\Framework\Add-ins\
XCopy /y /i %SourceFolder%\Binaries\Release\MBF.PAMSAM.dll %MBFFolder%\Framework\Add-ins\

echo ************************************************************
echo Copying SDK
echo ************************************************************

MD %MBFFolder%\SDK
XCopy /y /i %SourceFolder%\docs\MBF.chm %MBFFolder%\SDK\

echo ************************************************************
echo Copying SDK Documents
echo ************************************************************

Xcopy /y /i %SourceFolder%\docs\MBF_*.docx %MBFFolder%\SDK\

MD %MBFFolder%\SDK\Samples

echo ************************************************************
echo Copying IronPython Scripts
echo ************************************************************

MD %MBFFolder%\SDK\Samples\IronPython

echo ************************************************************
echo Copying IronPython Source
echo ************************************************************

MD %MBFFolder%\SDK\Samples\IronPython\MBFIronPython
XCopy /y /i %SourceFolder%\Source\MBF\Samples\Python\MBFDebug.py %MBFFolder%\SDK\Samples\IronPython\
XCopy /y /i %SourceFolder%\Source\MBF\Samples\Python\MBFDemo.py %MBFFolder%\SDK\Samples\IronPython\
XCopy /y /i %SourceFolder%\Source\MBF\Samples\Python\MBFMenu.py %MBFFolder%\SDK\Samples\IronPython\
XCopy /s /y /i %SourceFolder%\Source\MBF\Samples\Python\MBFIronPython\*.* %MBFFolder%\SDK\Samples\IronPython\MBFIronPython\

echo ************************************************************
echo Copying ReadGenerator binaries
echo ************************************************************

MD %MBFFolder%\SDK\Samples\ReadGenerator
XCopy /y /i %SourceFolder%\Binaries\Release\MBF.dll %MBFFolder%\SDK\Samples\ReadGenerator\
XCopy /y /i %SourceFolder%\Binaries\Release\ReadGenerator.exe %MBFFolder%\SDK\Samples\ReadGenerator\

echo ************************************************************
echo Copying ReadGenerator Source
echo ************************************************************

MD %MBFFolder%\SDK\Samples\ReadGenerator\Source
XCopy /s /y /i %SourceFolder%\Source\MBF\Samples\ReadGenerator\*.* %MBFFolder%\SDK\Samples\ReadGenerator\Source\

echo ************************************************************
echo Copying SAMUtils binaries
echo ************************************************************

MD %MBFFolder%\SDK\Samples\SAMUtils
XCopy /y /i %SourceFolder%\Binaries\Release\MBF.dll %MBFFolder%\SDK\Samples\SAMUtils\
XCopy /y /i %SourceFolder%\Binaries\Release\SAMUtils.exe %MBFFolder%\SDK\Samples\SAMUtils\

echo ************************************************************
echo Copying SAMUtils Source
echo ************************************************************

MD %MBFFolder%\SDK\Samples\SAMUtils\Source
XCopy /y /i %SourceFolder%\Source\MBF\Samples\SAMUtils\Program.cs %MBFFolder%\SDK\Samples\SAMUtils\Source\

echo ************************************************************
echo Copying TridentWorkflows binaries
echo ************************************************************

MD %MBFFolder%\SDK\Samples\TridentWorkflows
XCopy /y /i %SourceFolder%\Binaries\Release\MBF.dll %MBFFolder%\SDK\Samples\TridentWorkflows\
XCopy /y /i %SourceFolder%\Binaries\Release\MBF.Workflow.dll %MBFFolder%\SDK\Samples\TridentWorkflows\

echo ************************************************************
echo Copying TridentWorkflows Source
echo ************************************************************

MD %MBFFolder%\SDK\Samples\TridentWorkflows\Source
XCopy /s /y /i %SourceFolder%\Source\MBF\Samples\MBF.Workflow\*.* %MBFFolder%\SDK\Samples\TridentWorkflows\Source\

IF "%SETUP_MBF_ONLY%" == "true" GOTO EOF

echo ************************************************************
echo Copying Tools
echo ************************************************************

SET MBTFolder=".\Setup.Tmp\Microsoft Biology Tools"
MD %MBTFolder%


echo ************************************************************
echo Copying SequenceAssembler binaries
echo ************************************************************

SET SequenceAssemblerFolder=%MBTFolder%"\Sequence Assembler"
MD %SequenceAssemblerFolder%

XCopy /y /i %SourceFolder%\Binaries\Release\MBF.dll %SequenceAssemblerFolder%\
XCopy /y /i %SourceFolder%\Binaries\Release\MBF.WebServiceHandlers.dll %SequenceAssemblerFolder%\
XCopy /y /i %SourceFolder%\Binaries\Release\QUT.Bio.dll %SequenceAssemblerFolder%\
XCopy /y /i %SourceFolder%\Binaries\Release\WPFToolkit.dll %SequenceAssemblerFolder%\
XCopy /y /i %SourceFolder%\Binaries\Release\SequenceAssembler.exe.config %SequenceAssemblerFolder%\
XCopy /y /i %SourceFolder%\Binaries\Release\SequenceAssembler.exe %SequenceAssemblerFolder%\
XCopy /y /i %SourceFolder%\Binaries\Release\MBF.PAMSAM.dll %SequenceAssemblerFolder%\
XCopy /y /i %SourceFolder%\Binaries\Release\MBF.PaDeNA.dll %SequenceAssemblerFolder%\

echo ************************************************************
echo Copying SequenceAssembler Document
echo ************************************************************

MD %SequenceAssemblerFolder%\Docs
Xcopy /y /i %SourceFolder%\docs\MSR_Sequence_Assembler_User_Guide*.docx %SequenceAssemblerFolder%\Docs\MSR_Sequence_Assembler_User_Guide*.docx

:EOF
