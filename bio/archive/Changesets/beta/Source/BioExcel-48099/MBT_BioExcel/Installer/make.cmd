@echo OFF

SET SETUPTMPFOLDER=%1
IF "%SETUPTMPFOLDER%" == "" GOTO EOF

REM ** Excel Addin **

REM ** Note: current directory is TFS_MBI\MBF\Bio.NET\MBF\Setup\ExcelAddin **

echo ************************************************************
echo  Excel Workbench MSI creation started
echo ************************************************************

REM ** Set Framework current version number **
SET ProductVersion=1.0

REM ** Set Custom Actions path (used internally by Wix) **
SET CustomActions=%TFS_MBI%\MBT_BioExcel\Installer\CustomActions

PUSHD ..\..
SET TFS_MBI=%CD%
POPD

SET SETUP=%TFS_MBI%\MBT_BioExcel\Installer

SET UI=%TFS_MBI%\MBT_BioExcel\Installer\UI
SET BITMAPS=%TFS_MBI%\MBT_BioExcel\Installer\Bitmaps

SET WIXTOOLS=%WIX_INSTALLPATH%

REM ** temporary output folders **
SET OUTDIR=%TFS_MBI%\MBT_BioExcel\Installer\OUTPUT
SET TEMP=%TFS_MBI%\MBT_BioExcel\Installer\TEMP

IF EXIST %TEMP% rmdir /S /Q %TEMP%
mkdir %TEMP%
IF EXIST %OUTDIR% rmdir /S /Q %OUTDIR%
mkdir %OUTDIR%

REM ** delete existing installer **
IF EXIST BioExcel.msi del /F BioExcel.msi

echo --- Copying readme.txt file to temporary output folder ---
xcopy /y /q %SETUP%\source\readme.txt %OUTDIR%

echo --- Copying Bitmaps to temporary output folder ---
IF NOT EXIST %OUTDIR%\Bitmaps mkdir %OUTDIR%\Bitmaps
xcopy /E /H /R /C /Y /Q %BITMAPS%\*.* %OUTDIR%\Bitmaps\*.*

echo --- Copying license to the output folder ---
xcopy /y /Q %SETUP%\License.rtf %OUTDIR%

echo --- Copying source files to output folder ---
xcopy /y /q /e "%SETUPTMPFOLDER%\Microsoft Biology Tools\Excel Biology Extension" %OUTDIR%

echo --- Copying Framework merge module to output folder ---
xcopy /y /q %SETUP%\source\MergeModule.msm %OUTDIR%\

echo --- Harvesting source folders ---
CALL "%WIXTOOLS%\heat" dir "%SETUPTMPFOLDER%\Microsoft Biology Tools\Excel Biology Extension" -srd -dr EXCELADDINFOLDER -gg -g1 -nologo -cg ExcelAddinComponentGroup -sfrag -template:fragment -ke -out %SETUP%\source\ExcelAddinComponents.wxs

PUSHD %UI%

echo --- Compiling UI dialogs ---
CALL "%WIXTOOLS%\candle" -nologo -out %TEMP%\ BrowseDlg.wxs Common.wxs CommonFonts.wxs DiskCostDlg.wxs ExcelAddinUI.wxs CustomSetupTypeDlg.wxs ErrorDlg.wxs ErrorProgressText.wxs ExitDialog.wxs InstallDirDlg.wxs LicenseAgreementDlg.wxs MaintenanceTypeDlg.wxs MaintenanceWelcomeDlg.wxs MsiRMFilesInUse.wxs PrepareDlg.wxs ProgressDlg.wxs ResumeDlg.wxs UserExit.wxs VerifyReadyDlg.wxs WelcomeDlg.wxs CancelDlg.wxs FatalError.wxs OutOfDiskDlg.wxs OutOfRbDiskDlg.wxs WaitForCostingDlg.wxs CustomizeDlg.wxs ExcelPrerequisiteDeterminationDlg.wxs
POPD

PUSHD %TEMP%

REM ** Link UI dialogs **
CALL "%WIXTOOLS%\lit" -nologo -out %OUTDIR%\ExcelAddinUILib.wixlib BrowseDlg.wixobj Common.wixobj CommonFonts.wixobj DiskCostDlg.wixobj ExcelAddinUI.wixobj ErrorDlg.wixobj ErrorProgressText.wixobj ExitDialog.wixobj InstallDirDlg.wixobj LicenseAgreementDlg.wixobj MaintenanceTypeDlg.wixobj MaintenanceWelcomeDlg.wixobj MsiRMFilesInUse.wixobj PrepareDlg.wixobj ProgressDlg.wixobj ResumeDlg.wixobj UserExit.wixobj VerifyReadyDlg.wixobj WelcomeDlg.wixobj  CancelDlg.wixobj FatalError.wixobj OutOfDiskDlg.wixobj OutOfRbDiskDlg.wixobj WaitForCostingDlg.wixobj CustomSetupTypeDlg.wixobj CustomizeDlg.wixobj ExcelPrerequisiteDeterminationDlg.wixobj
POPD

PUSHD %SETUP%\source

echo --- Compiling core installer files ---
CALL "%WIXTOOLS%\candle" -nologo -out %OUTDIR%\ ExcelAddin.wxs ExcelAddinComponents.wxs

POPD

PUSHD %OUTDIR%

REM ** Linking installer object files and UI library **
REM ** referencing localization file and external assemblies **
CALL "%WIXTOOLS%\light" -nologo -sice:ICE05 -out %SETUP%\BioExcel.msi ExcelAddin.wixobj ExcelAddinComponents.wixobj ExcelAddinUILib.wixlib -loc %UI%\ExcelAddin_WixUI_en-us.wxl -ext "%WIXTOOLS%\WixUIExtension.dll" -ext "%WIXTOOLS%\WixUtilExtension.dll"

POPD

REM ** Clean up temporary files ** 
IF EXIST %TEMP% RMDIR /S /Q %TEMP%
IF EXIST %OUTDIR% RMDIR /S /Q %OUTDIR%
IF EXIST %SETUP%\*.wixpdb DEL /Q %SETUP%\*.wixpdb
IF EXIST %SETUP%\source\ExcelAddinComponents.wxs DEL /Q %SETUP%\source\ExcelAddinComponents.wxs

echo ************************************************************
echo  Excel Workbench Setup creation complete
echo ************************************************************

:EOF