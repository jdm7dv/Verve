@Echo Off
@if not "%ECHO%"=="" Echo %ECHO%

SET SETUPTMPFOLDER=%1
IF "%SETUPTMPFOLDER%" == "" GOTO EOF

REM ** Sequence Assembler **

REM ** Note: current directory is TFS_MBI\MBT\Installer **

echo ************************************************************
echo  Sequence Assembler MSI creation started
echo ************************************************************

REM ** Set Framework current version number (this is used internally by Wix)**
SET ProductVersion=1.0

PUSHD ..\..
SET TFS_MBI=%CD%
POPD
REM ** SETUP variable is used internally by Wix **
SET SETUP=%TFS_MBI%\MBT\Installer

SET UI=%TFS_MBI%\MBT\Installer\UI
SET BITMAPS=%TFS_MBI%\MBT\Installer\Bitmaps

SET WIXTOOLS=%WIX_INSTALLPATH%

REM ** temporary output folders **
SET OUTDIR=%TFS_MBI%\MBT\Installer\OUTPUT
SET TEMP=%TFS_MBI%\MBT\Installer\TEMP

IF EXIST %TEMP% rmdir /S /Q %TEMP%
mkdir %TEMP%
IF EXIST %OUTDIR% rmdir /S /Q %OUTDIR%
mkdir %OUTDIR%

REM ** delete existing installer **
IF EXIST SequenceAssembler.msi del /F SequenceAssembler.msi

echo --- Copying readme.txt file to temporary output folder ---
xcopy /y /q %SETUP%\source\readme.txt %OUTDIR%

echo --- Copying Bitmaps to temporary output folder ---
IF NOT EXIST %OUTDIR%\Bitmaps mkdir %OUTDIR%\Bitmaps
xcopy /E /H /R /C /Y /Q %BITMAPS%\*.* %OUTDIR%\Bitmaps\*.*

echo --- Copying license to the output folder ---
xcopy /y /Q %SETUP%\License.rtf %OUTDIR%

echo --- Copying source files to output folder ---
xcopy /y /q /e "%SETUPTMPFOLDER%\Microsoft Biology Tools\Sequence Assembler" %OUTDIR%

echo --- Copying Framework merge module to output folder ---
xcopy /y /q %SETUP%\source\MergeModule.msm %OUTDIR%\

echo --- Harvesting source folders ---
CALL "%WIXTOOLS%\heat" dir "%SETUPTMPFOLDER%\Microsoft Biology Tools\Sequence Assembler" -srd -dr SequenceAssemblerFolder -gg -g1 -nologo -sw5151 -cg SequenceAssemblerComponentGroup -sfrag -template:fragment -ke -out %SETUP%\source\SequenceAssemblerComponents.wxs

PUSHD %UI%

echo --- Compiling UI dialogs ---
CALL "%WIXTOOLS%\candle" -nologo -out %TEMP%\ BrowseDlg.wxs CancelDlg.wxs Common.wxs CommonFonts.wxs DiskCostDlg.wxs SequenceAssemblerUI.wxs CustomSetupTypeDlg.wxs ErrorDlg.wxs ErrorProgressText.wxs ExitDialog.wxs FileAssociationsDlg.wxs InstallDirDlg.wxs LicenseAgreementDlg.wxs MaintenanceTypeDlg.wxs MaintenanceWelcomeDlg.wxs MsiRMFilesInUse.wxs PrepareDlg.wxs ProgressDlg.wxs ResumeDlg.wxs UserExit.wxs VerifyReadyDlg.wxs WaitForCostingDlg.wxs WelcomeDlg.wxs FatalError.wxs OutOfDiskDlg.wxs OutOfRbDiskDlg.wxs CustomizeDlg.wxs SequencePrerequisiteDeterminationDlg.wxs
POPD

PUSHD %TEMP%

REM ** Link UI dialogs **
CALL "%WIXTOOLS%\lit" -nologo -out %OUTDIR%\SequenceAssemblerUILib.wixlib BrowseDlg.wixobj CancelDlg.wixobj Common.wixobj CommonFonts.wixobj DiskCostDlg.wixobj SequenceAssemblerUI.wixobj CustomSetupTypeDlg.wixobj ErrorDlg.wixobj ErrorProgressText.wixobj ExitDialog.wixobj FileAssociationsDlg.wixobj InstallDirDlg.wixobj LicenseAgreementDlg.wixobj MaintenanceTypeDlg.wixobj MaintenanceWelcomeDlg.wixobj MsiRMFilesInUse.wixobj PrepareDlg.wixobj ProgressDlg.wixobj ResumeDlg.wixobj UserExit.wixobj VerifyReadyDlg.wixobj WaitForCostingDlg.wixobj WelcomeDlg.wixobj FatalError.wixobj OutOfDiskDlg.wixobj OutOfRbDiskDlg.wixobj CustomizeDlg.wixobj SequencePrerequisiteDeterminationDlg.wixobj
POPD

PUSHD %SETUP%\source

echo --- Compiling core installer files ---
CALL "%WIXTOOLS%\candle" -nologo -out %OUTDIR%\ FixedComponents.wxs SequenceAssembler.wxs SequenceAssemblerComponents.wxs

POPD

PUSHD %OUTDIR%

REM ** Linking installer object files and UI library **
REM ** referencing localization file and external assemblies **
CALL "%WIXTOOLS%\light" -nologo -sice:ICE05 -sw1076 -out %SETUP%\SequenceAssembler.msi FixedComponents.wixobj SequenceAssembler.wixobj SequenceAssemblerComponents.wixobj SequenceAssemblerUILib.wixlib -loc %UI%\SequenceAssembler_WixUI_en-us.wxl -ext "%WIXTOOLS%\WixUIExtension.dll" -ext "%WIXTOOLS%\WixUtilExtension.dll"
 
POPD

REM ** Clean up temporary files ** 
IF EXIST %TEMP% RMDIR /S /Q %TEMP%
IF EXIST %OUTDIR% RMDIR /S /Q %OUTDIR%
IF EXIST %SETUP%\*.wixpdb DEL /Q %SETUP%\*.wixpdb
IF EXIST %SETUP%\source\SequenceAssemblerComponents.wxs DEL /Q %SETUP%\source\SequenceAssemblerComponents.wxs

echo ************************************************************
echo  Sequence Assembler MSI creation complete
echo ************************************************************

:EOF
