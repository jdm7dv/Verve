REM -- ********************************************************************************
REM --     Description
REM -- ********************************************************************************
REM -- Prepares folder structure required for installer and copies the required files.
REM -- ********************************************************************************

@echo off

echo ************************************************************
echo Preparing folder structure.
echo ************************************************************

SET SourceFolder=%1
SET TargetFolder=%2

CD %TargetFolder%

IF EXIST .\Setup.Tmp RMDIR /S /Q .\Setup.Tmp
MD .\Setup.Tmp

SET MBTFolder=".\Setup.Tmp\Microsoft Biology Tools"
MD %MBTFolder%

echo ************************************************************
echo Copying ExcelworkBench binaries
echo ************************************************************

SET ExcelFolder=%MBTFolder%"\Excel Biology Extension"
MD %ExcelFolder%

XCopy /y %SourceFolder%\Binaries\Release\*.dll %ExcelFolder%\*.dll
XCopy /y %SourceFolder%\Binaries\Release\*.vsto %ExcelFolder%\*.vsto
XCopy /y %SourceFolder%\Binaries\Release\*.dll.manifest %ExcelFolder%\*.dll.manifest
XCopy /y %SourceFolder%\Binaries\Release\*.bas %ExcelFolder%\*.bas
XCopy /y %SourceFolder%\Binaries\Release\*.xml %ExcelFolder%\*.xml

echo ************************************************************
echo Copying ExcelworkBench Document
echo ************************************************************

MD %ExcelFolder%\Docs
XCopy /y %SourceFolder%\Docs\MSR_Biology_Extension_User_Guide*.docx %ExcelFolder%\Docs\MSR_Biology_Extension_User_Guide*.docx