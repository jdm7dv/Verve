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
SET SETUP_BIO_ONLY=%3

CD %TargetFolder%

IF EXIST .\Setup.Tmp RMDIR /S /Q .\Setup.Tmp
MD .\Setup.Tmp
echo ************************************************************
echo Copying MBF binaries
echo ************************************************************

SET BioFolder=".\Setup.Tmp\Microsoft Biology Foundation"

MD %BioFolder%

echo ************************************************************
echo Copying Visual Studio template
echo ************************************************************
echo SOURCE FOLDER PATH IS: %SourceFolder%
Xcopy /y /i %SourceFolder%\Binaries\Release\Bio.TemplateWizard.dll %BioFolder%
Xcopy /y /i %SourceFolder%\Binaries\Release\BioConsoleApplicationTemplate.zip %BioFolder%

Xcopy /y /i %SourceFolder%\Binaries\Release\Bio.dll %BioFolder%\MBF\
XCopy /y /i %SourceFolder%\Binaries\Release\Bio.WebServiceHandlers.dll %BioFolder%\MBF\

echo ************************************************************
echo Copying Add-ins Source
echo ************************************************************

MD %BioFolder%\MBF\Add-ins
XCopy /y /i %SourceFolder%\Binaries\Release\Bio.Padena.dll %BioFolder%\MBF\Add-ins\
XCopy /y /i %SourceFolder%\Binaries\Release\Bio.Pamsam.dll %BioFolder%\MBF\Add-ins\
XCopy /y /i %SourceFolder%\Binaries\Release\Bio.Comparative.dll %BioFolder%\MBF\Add-ins\

echo ************************************************************
echo Copying SDK
echo ************************************************************

MD %BioFolder%\SDK
XCopy /y /i %SourceFolder%\docs\Bio.chm %BioFolder%\SDK\

echo ************************************************************
echo Copying SDK Documents
echo ************************************************************

Xcopy /y /i %SourceFolder%\docs\Becoming_A_Committer_v2.docx %BioFolder%\SDK\
Xcopy /y /i %SourceFolder%\docs\Coding_Conventions.docx %BioFolder%\SDK\
Xcopy /y /i %SourceFolder%\docs\Commenting_Conventions.docx %BioFolder%\SDK\
Xcopy /y /i %SourceFolder%\docs\Committer_Guide_v2.docx %BioFolder%\SDK\
Xcopy /y /i %SourceFolder%\docs\Committer_Onboarding_v2.docx %BioFolder%\SDK\
Xcopy /y /i %SourceFolder%\docs\Contribution_Documentation_Template_v2.docx %BioFolder%\SDK\
Xcopy /y /i %SourceFolder%\docs\Contributor_Guide_v2.docx %BioFolder%\SDK\
Xcopy /y /i %SourceFolder%\docs\Getting_Started.docx %BioFolder%\SDK\
Xcopy /y /i %SourceFolder%\docs\IronPython Programming_Guide_v2.docx %BioFolder%\SDK\
Xcopy /y /i %SourceFolder%\docs\ReadSimulator_V2.docx %BioFolder%\SDK\
Xcopy /y /i %SourceFolder%\docs\Overview_v2.docx %BioFolder%\SDK\
Xcopy /y /i %SourceFolder%\docs\PaDeNA.docx %BioFolder%\SDK\
Xcopy /y /i %SourceFolder%\docs\Programming_Guide_v2.docx %BioFolder%\SDK\
Xcopy /y /i %SourceFolder%\docs\Testing_Guide_v2.docx %BioFolder%\SDK\

MD %BioFolder%\SDK\Samples

echo ************************************************************
echo Copying IronPython Scripts
echo ************************************************************

MD %BioFolder%\SDK\Samples\IronPython

echo ************************************************************
echo Copying IronPython Source
echo ************************************************************

MD %BioFolder%\SDK\Samples\IronPython\BioIronPython
XCopy /y /i %SourceFolder%\Source\Tools\Python\BioDebug.py %BioFolder%\SDK\Samples\IronPython\
XCopy /y /i %SourceFolder%\Source\Tools\Python\BioDemo.py %BioFolder%\SDK\Samples\IronPython\
XCopy /y /i %SourceFolder%\Source\Tools\Python\BioMenu.py %BioFolder%\SDK\Samples\IronPython\
XCopy /s /y /i %SourceFolder%\Source\Tools\Python\BioIronPython\*.* %BioFolder%\SDK\Samples\IronPython\BioIronPython\

echo ************************************************************
echo Copying ReadSimulator binaries
echo ************************************************************

MD %BioFolder%\SDK\Samples\ReadSimulator
XCopy /y /i %SourceFolder%\Binaries\Release\Bio.dll %BioFolder%\SDK\Samples\ReadSimulator\
XCopy /y /i %SourceFolder%\Binaries\Release\ReadSimulator.exe %BioFolder%\SDK\Samples\ReadSimulator\

echo ************************************************************
echo Copying ReadSimulator Source
echo ************************************************************

MD %BioFolder%\SDK\Samples\ReadSimulator\Source
XCopy /s /y /i %SourceFolder%\Source\Tools\ReadSimulator\*.* %BioFolder%\SDK\Samples\ReadSimulator\Source\

echo ************************************************************
echo Copying SAMUtils binaries
echo ************************************************************

MD %BioFolder%\SDK\Samples\SAMUtils
XCopy /y /i %SourceFolder%\Binaries\Release\Bio.dll %BioFolder%\SDK\Samples\SAMUtils\
XCopy /y /i %SourceFolder%\Binaries\Release\SAMUtils.exe %BioFolder%\SDK\Samples\SAMUtils\

echo ************************************************************
echo Copying SAMUtils Source
echo ************************************************************

MD %BioFolder%\SDK\Samples\SAMUtils\Source
XCopy /y /i %SourceFolder%\Source\Tools\SAMUtil\*.* %BioFolder%\SDK\Samples\SAMUtils\Source\

echo ************************************************************
echo Copying TridentWorkflows binaries
echo ************************************************************

MD %BioFolder%\SDK\Samples\TridentWorkflows
XCopy /y /i %SourceFolder%\Binaries\Release\Bio.dll %BioFolder%\SDK\Samples\TridentWorkflows\
XCopy /y /i %SourceFolder%\Binaries\Release\Bio.Workflow.dll %BioFolder%\SDK\Samples\TridentWorkflows\

echo ************************************************************
echo Copying TridentWorkflows Source
echo ************************************************************

MD %BioFolder%\SDK\Samples\TridentWorkflows\Source
XCopy /s /y /i %SourceFolder%\Source\Tools\Bio.Workflow\*.* %BioFolder%\SDK\Samples\TridentWorkflows\Source\

echo ************************************************************
echo Copying MumUtil binaries
echo ************************************************************

MD %BioFolder%\SDK\Samples\MumUtil
XCopy /y /i %SourceFolder%\Binaries\Release\Bio.dll %BioFolder%\SDK\Samples\MumUtil\
XCopy /y /i %SourceFolder%\Binaries\Release\MumUtil.exe %BioFolder%\SDK\Samples\MumUtil\

echo ************************************************************
echo Copying MumUtil Source
echo ************************************************************

MD %BioFolder%\SDK\Samples\MumUtil\Source
XCopy /s /y /i %SourceFolder%\Source\Tools\MumUtil\*.* %BioFolder%\SDK\Samples\MumUtil\Source\

echo ************************************************************
echo Copying LisUtil binaries
echo ************************************************************

MD %BioFolder%\SDK\Samples\LisUtil
XCopy /y /i %SourceFolder%\Binaries\Release\Bio.dll %BioFolder%\SDK\Samples\LisUtil\
XCopy /y /i %SourceFolder%\Binaries\Release\LISUtil.exe %BioFolder%\SDK\Samples\LisUtil\

echo ************************************************************
echo Copying LisUtil Source
echo ************************************************************

MD %BioFolder%\SDK\Samples\LisUtil\Source
XCopy /s /y /i %SourceFolder%\Source\Tools\LisUtil\*.* %BioFolder%\SDK\Samples\LisUtil\Source\

echo ************************************************************
echo Copying NucmerUtil binaries
echo ************************************************************

MD %BioFolder%\SDK\Samples\NucmerUtil
XCopy /y /i %SourceFolder%\Binaries\Release\Bio.dll %BioFolder%\SDK\Samples\NucmerUtil\
XCopy /y /i %SourceFolder%\Binaries\Release\NucmerUtil.exe %BioFolder%\SDK\Samples\NucmerUtil\

echo ************************************************************
echo Copying NucmerUtil Source
echo ************************************************************

MD %BioFolder%\SDK\Samples\NucmerUtil\Source
XCopy /s /y /i %SourceFolder%\Source\Tools\NucmerUtil\*.* %BioFolder%\SDK\Samples\NucmerUtil\Source\

echo ************************************************************
echo Copying PadenaUtil binaries
echo ************************************************************

MD %BioFolder%\SDK\Samples\PadenaUtil
XCopy /y /i %SourceFolder%\Binaries\Release\Bio.dll %BioFolder%\SDK\Samples\PadenaUtil\
XCopy /y /i %SourceFolder%\Binaries\Release\Bio.Padena.dll %BioFolder%\SDK\Samples\PadenaUtil\
XCopy /y /i %SourceFolder%\Binaries\Release\PadenaUtil.exe %BioFolder%\SDK\Samples\PadenaUtil\

echo ************************************************************
echo Copying PadenaUtil Source
echo ************************************************************

MD %BioFolder%\SDK\Samples\PadenaUtil\Source
XCopy /s /y /i %SourceFolder%\Source\Tools\PadenaUtil\*.* %BioFolder%\SDK\Samples\PadenaUtil\Source\

echo ************************************************************
echo Copying RepeatResolutionUtil binaries
echo ************************************************************

MD %BioFolder%\SDK\Samples\RepeatResolutionUtil
XCopy /y /i %SourceFolder%\Binaries\Release\Bio.dll %BioFolder%\SDK\Samples\RepeatResolutionUtil\
XCopy /y /i %SourceFolder%\Binaries\Release\Bio.Comparative.dll %BioFolder%\SDK\Samples\RepeatResolutionUtil\
XCopy /y /i %SourceFolder%\Binaries\Release\RepeatResolutionUtil.exe %BioFolder%\SDK\Samples\RepeatResolutionUtil\

echo ************************************************************
echo Copying RepeatResolutionUtil Source
echo ************************************************************

MD %BioFolder%\SDK\Samples\RepeatResolutionUtil\Source
XCopy /s /y /i %SourceFolder%\Source\Tools\RepeatResolutionUtil\*.* %BioFolder%\SDK\Samples\RepeatResolutionUtil\Source\

echo ************************************************************
echo Copying LayoutRefinementUtil binaries
echo ************************************************************

MD %BioFolder%\SDK\Samples\LayoutRefinementUtil
XCopy /y /i %SourceFolder%\Binaries\Release\Bio.dll %BioFolder%\SDK\Samples\LayoutRefinementUtil\
XCopy /y /i %SourceFolder%\Binaries\Release\Bio.Comparative.dll %BioFolder%\SDK\Samples\LayoutRefinementUtil\
XCopy /y /i %SourceFolder%\Binaries\Release\LayoutRefinementUtil.exe %BioFolder%\SDK\Samples\LayoutRefinementUtil\

echo ************************************************************
echo Copying LayoutRefinementUtil Source
echo ************************************************************

MD %BioFolder%\SDK\Samples\LayoutRefinementUtil\Source
XCopy /s /y /i %SourceFolder%\Source\Tools\LayoutRefinementUtil\*.* %BioFolder%\SDK\Samples\LayoutRefinementUtil\Source\

echo ************************************************************
echo Copying ConsensusUtil binaries
echo ************************************************************

MD %BioFolder%\SDK\Samples\ConsensusUtil
XCopy /y /i %SourceFolder%\Binaries\Release\Bio.dll %BioFolder%\SDK\Samples\ConsensusUtil\
XCopy /y /i %SourceFolder%\Binaries\Release\Bio.Comparative.dll %BioFolder%\SDK\Samples\ConsensusUtil\
XCopy /y /i %SourceFolder%\Binaries\Release\ConsensusUtil.exe %BioFolder%\SDK\Samples\ConsensusUtil\

echo ************************************************************
echo Copying ConsensusUtil Source
echo ************************************************************

MD %BioFolder%\SDK\Samples\ConsensusUtil\Source
XCopy /s /y /i %SourceFolder%\Source\Tools\ConsensusUtil\*.* %BioFolder%\SDK\Samples\ConsensusUtil\Source\

echo ************************************************************
echo Copying ScaffoldUtil binaries
echo ************************************************************

MD %BioFolder%\SDK\Samples\ScaffoldUtil
XCopy /y /i %SourceFolder%\Binaries\Release\Bio.dll %BioFolder%\SDK\Samples\ScaffoldUtil\
XCopy /y /i %SourceFolder%\Binaries\Release\Bio.Comparative.dll %BioFolder%\SDK\Samples\ScaffoldUtil\
XCopy /y /i %SourceFolder%\Binaries\Release\Bio.Padena.dll %BioFolder%\SDK\Samples\ScaffoldUtil\
XCopy /y /i %SourceFolder%\Binaries\Release\ScaffoldUtil.exe %BioFolder%\SDK\Samples\ScaffoldUtil\

echo ************************************************************
echo Copying ScaffoldUtil Source
echo ************************************************************

MD %BioFolder%\SDK\Samples\ScaffoldUtil\Source
XCopy /s /y /i %SourceFolder%\Source\Tools\ScaffoldUtil\*.* %BioFolder%\SDK\Samples\ScaffoldUtil\Source\

echo ************************************************************
echo Copying ComparativeUtil binaries
echo ************************************************************

MD %BioFolder%\SDK\Samples\ComparativeUtil
XCopy /y /i %SourceFolder%\Binaries\Release\Bio.dll %BioFolder%\SDK\Samples\ComparativeUtil\
XCopy /y /i %SourceFolder%\Binaries\Release\Bio.Comparative.dll %BioFolder%\SDK\Samples\ComparativeUtil\
XCopy /y /i %SourceFolder%\Binaries\Release\ComparativeUtil.exe %BioFolder%\SDK\Samples\ComparativeUtil\

echo ************************************************************
echo Copying ComparativeUtil Source
echo ************************************************************

MD %BioFolder%\SDK\Samples\ComparativeUtil\Source
XCopy /s /y /i %SourceFolder%\Source\Tools\ComparativeUtil\*.* %BioFolder%\SDK\Samples\ComparativeUtil\Source\

IF "%SETUP_BIO_ONLY%" == "true" GOTO EOF

echo ************************************************************
echo Copying MBT
echo ************************************************************

SET ToolsFolder=".\Setup.Tmp\Microsoft Biology Tools"
MD %ToolsFolder%


echo ************************************************************
echo Copying SequenceAssembler binaries
echo ************************************************************

SET SequenceAssemblerFolder=%ToolsFolder%"\Sequence Assembler"
MD %SequenceAssemblerFolder%

XCopy /y /i %SourceFolder%\Binaries\Release\Bio.dll %SequenceAssemblerFolder%\
XCopy /y /i %SourceFolder%\Binaries\Release\Bio.WebServiceHandlers.dll %SequenceAssemblerFolder%\
XCopy /y /i %SourceFolder%\Binaries\Release\QUT.Bio.dll %SequenceAssemblerFolder%\
XCopy /y /i %SourceFolder%\Binaries\Release\WPFToolkit.dll %SequenceAssemblerFolder%\
XCopy /y /i %SourceFolder%\Binaries\Release\SequenceAssembler.exe.config %SequenceAssemblerFolder%\
XCopy /y /i %SourceFolder%\Binaries\Release\SequenceAssembler.exe %SequenceAssemblerFolder%\
XCopy /y /i %SourceFolder%\Binaries\Release\Bio.Pamsam.dll %SequenceAssemblerFolder%\
XCopy /y /i %SourceFolder%\Binaries\Release\Bio.Padena.dll %SequenceAssemblerFolder%\
XCopy /y /i %SourceFolder%\Binaries\Release\Bio.Comparative.dll %SequenceAssemblerFolder%\

echo ************************************************************
echo Copying SequenceAssembler Document
echo ************************************************************

MD %SequenceAssemblerFolder%\Docs
Xcopy /y /i %SourceFolder%\docs\MSR_Sequence_Assembler_User_Guide*.docx %SequenceAssemblerFolder%\Docs\MSR_Sequence_Assembler_User_Guide*.docx


echo ************************************************************
echo Copying ExcelworkBench binaries
echo ************************************************************

SET ExcelFolder=%ToolsFolder%"\Excel Biology Extension"
MD %ExcelFolder%

Copy /y %SourceFolder%\Binaries\Release\BioExcel.dll %ExcelFolder%\BioExcel.dll
Copy /y %SourceFolder%\Binaries\Release\BioExcel.Visualizations.Common.dll %ExcelFolder%\BioExcel.Visualizations.Common.dll
Copy /y %SourceFolder%\Binaries\Release\Bio.dll %ExcelFolder%\Bio.dll
Copy /y %SourceFolder%\Binaries\Release\Bio.WebServiceHandlers.dll %ExcelFolder%\Bio.WebServiceHandlers.dll
Copy /y %SourceFolder%\Binaries\Release\Tools.VennToNodeXL.dll %ExcelFolder%\Tools.VennToNodeXL.dll
Copy /y %SourceFolder%\Binaries\Release\Microsoft.Office.Tools.Common.v4.0.Utilities.dll %ExcelFolder%\Microsoft.Office.Tools.Common.v4.0.Utilities.dll
Copy /y %SourceFolder%\Binaries\Release\BioExcel.vsto %ExcelFolder%\BioExcel.vsto
Copy /y %SourceFolder%\Binaries\Release\BioExcel.dll.manifest %ExcelFolder%\BioExcel.dll.manifest
Copy /y %SourceFolder%\Binaries\Release\DisplayDNASequenceDistribution.bas %ExcelFolder%\DisplayDNASequenceDistribution.bas
Copy /y %SourceFolder%\Binaries\Release\Microsoft.Office.Tools.Common.v4.0.Utilities.xml %ExcelFolder%\Microsoft.Office.Tools.Common.v4.0.Utilities.xml

echo ************************************************************
echo Copying ExcelworkBench Document
echo ************************************************************

MD %ExcelFolder%\Docs
XCopy /y %SourceFolder%\Docs\MSR_Biology_Extension_User_Guide*.docx %ExcelFolder%\Docs\MSR_Biology_Extension_User_Guide*.docx

:EOF
SET SourceFolder=
SET TargetFolder=
SET SETUP_BIO_ONLY=
