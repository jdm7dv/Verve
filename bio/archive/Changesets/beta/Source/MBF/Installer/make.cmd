@Echo Off
@if not "%ECHO%"=="" Echo %ECHO%

SET SOURCEROOT=%1
IF "%SOURCEROOT%" == "" GOTO SOURCEROOTMISSING

SET SETUPTMPFOLDER=%2
IF "%SETUPTMPFOLDER%" == "" GOTO SETUPTMPFOLDERMISSING

REM ** Note: current directory is MBI\Source\MBF\Installer**

echo ************************************************************
echo  MBF msi creation started
echo ************************************************************

REM ** Set Framework current version number (this is used internally by Wix)**
SET ProductVersion=2.1

PUSHD ..\..
SET MBISOURCE=%CD%
POPD
SET SETUP=%MBISOURCE%\MBF\Installer
SET UI=%MBISOURCE%\MBF\Installer\UI
SET BITMAPS=%MBISOURCE%\MBF\Installer\Bitmaps

SET WIXTOOLS=%SOURCEROOT%\BuildTools\ToolSource\ExternalTools\wix3

REM ** set temporary output folders **
SET OUTDIR=%MBISOURCE%\MBF\Installer\OUTPUT
SET TEMP=%MBISOURCE%\MBF\Installer\TEMP

REM ** create temporary output folders **
IF EXIST %TEMP% rmdir /S /Q %TEMP%
mkdir %TEMP%
IF EXIST %OUTDIR% rmdir /S /Q %OUTDIR%
mkdir %OUTDIR%

REM ** delete existing installer **
IF EXIST MBF.msi del /F MBF.msi

echo --- Copying readme.txt file to temporary output folder ---
xcopy /y /q %SETUP%\source\readme.txt %OUTDIR%

echo --- Harvesting source folders ---
CALL "%WIXTOOLS%\heat" dir "%SETUPTMPFOLDER%\Microsoft Biology Foundation\Framework" -srd -dr FRAMEWORKFOLDER -gg -g1 -nologo -cg FrameworkComponentGroup -sfrag -template:fragment -ke -out %SETUP%\source\FrameworkComponents.wxs
CALL "%WIXTOOLS%\heat" dir "%SETUPTMPFOLDER%\Microsoft Biology Foundation\SDK" -srd -dr SDKFolder -gg -g1 -nologo -cg SDKComponentGroup -sfrag -template:fragment -ke -out %SETUP%\source\SDKComponents.wxs

echo --- Adding Framework components to merge module source file ---
PUSHD %SETUP%\source
Attrib -r MergeModule.wxs
%SETUP%\MergeMBFwxs FrameworkComponents.wxs MergeModule.wxs
Attrib +r MergeModule.wxs
POPD

echo --- Copying Bitmaps to temporary output folder ---
IF NOT EXIST %OUTDIR%\Bitmaps mkdir %OUTDIR%\Bitmaps
xcopy /E /H /R /C /Y /Q %BITMAPS%\*.* %OUTDIR%\Bitmaps\*.*

echo --- Copying license to temporary output folder ---
xcopy /y /Q %SETUP%\License.rtf %OUTDIR%

echo --- Copying source files to temporary output folder ---
xcopy /y /q /e "%SETUPTMPFOLDER%\Microsoft Biology Foundation\Framework" %OUTDIR%
xcopy /y /q /e "%SETUPTMPFOLDER%\Microsoft Biology Foundation\SDK" %OUTDIR%

echo --- Copying Visual Studio MBF project template files to output folder ---
xcopy /y /q "%SETUPTMPFOLDER%\Microsoft Biology Foundation\MBFConsoleApplicationTemplate.zip" %OUTDIR%
xcopy /y /q /e "%SETUPTMPFOLDER%\Microsoft Biology Foundation\MBF.TemplateWizard.dll" %OUTDIR%

echo --- Creating merge module for Framework ---
IF EXIST %SETUP%\source\MergeModule.msm DEL /Q %SETUP%\source\MergeModule.msm
xcopy /y /q %SETUP%\source\MergeModule.wxs %OUTDIR%
PUSHD %OUTDIR%
CALL "%WIXTOOLS%\candle" -nologo MergeModule.wxs
CALL "%WIXTOOLS%\light" -nologo MergeModule.wixobj -out %SETUP%\source\MergeModule.msm
POPD

echo --- Copying MergeModule.msm to temporary output folder ---
xcopy /y /q %SETUP%\source\MergeModule.msm %OUTDIR%

echo --- Deleting MergeModule.msm from MBI\Source\MBT\Installer\Common ---
IF EXIST %MBISOURCE%\MBT\Installer\Common\MergeModule.msm (
DEL /F %MBISOURCE%\MBT\Installer\Common\MergeModule.msm )

echo --- Copying MergeModule.msm to MBI\Source\MBT\Installer\Common ---
xcopy /y /q %SETUP%\source\MergeModule.msm %MBISOURCE%\MBT\Installer\Common

PUSHD %UI%

echo --- Compiling UI dialogs ---
CALL "%WIXTOOLS%\candle" -nologo -out %TEMP%\ BrowseDlg.wxs Common.wxs CommonFonts.wxs DiskCostDlg.wxs FrameworkUI.wxs CustomMBFSetupTypeDlg.wxs ErrorDlg.wxs ErrorProgressText.wxs ExitDialog.wxs InstallDirDlg.wxs LicenseAgreementDlg.wxs MaintenanceTypeDlg.wxs MaintenanceWelcomeDlg.wxs MsiRMFilesInUse.wxs PrepareDlg.wxs ProgressDlg.wxs ResumeDlg.wxs UserExit.wxs VerifyReadyDlg.wxs WelcomeDlg.wxs CancelDlg.wxs FatalError.wxs OutOfDiskDlg.wxs OutOfRbDiskDlg.wxs WaitForCostingDlg.wxs CustomizeDlg.wxs MBFPrerequisiteDeterminationDlg.wxs
POPD

PUSHD %TEMP%

REM ** Link UI dialogs **
CALL "%WIXTOOLS%\lit" -nologo -out %OUTDIR%\FrameworkUILib.wixlib BrowseDlg.wixobj Common.wixobj CommonFonts.wixobj DiskCostDlg.wixobj FrameworkUI.wixobj CustomMBFSetupTypeDlg.wixobj ErrorDlg.wixobj ErrorProgressText.wixobj ExitDialog.wixobj InstallDirDlg.wixobj LicenseAgreementDlg.wixobj MaintenanceTypeDlg.wixobj MaintenanceWelcomeDlg.wixobj MsiRMFilesInUse.wixobj PrepareDlg.wixobj ProgressDlg.wixobj ResumeDlg.wixobj UserExit.wixobj VerifyReadyDlg.wixobj WelcomeDlg.wixobj  CancelDlg.wixobj FatalError.wixobj OutOfDiskDlg.wixobj OutOfRbDiskDlg.wixobj WaitForCostingDlg.wixobj CustomizeDlg.wixobj MBFPrerequisiteDeterminationDlg.wixobj
POPD

PUSHD %SETUP%\source

echo --- Compiling core installer files ---
CALL "%WIXTOOLS%\candle" -nologo -out %OUTDIR%\ Framework.wxs SDKComponents.wxs VStemplateComponents.wxs

POPD

PUSHD %OUTDIR%

REM ** Linking installer object files and UI library, referencing localization file and external assemblies **
CALL "%WIXTOOLS%\light" -nologo -sice:ICE05 -sw1076 -out %SETUP%\MBF.msi Framework.wixobj SDKComponents.wixobj VStemplateComponents.wixobj FrameworkUILib.wixlib -loc %UI%\MBF_WixUI_en-us.wxl -ext "%WIXTOOLS%\WixUIExtension.dll" -ext "%WIXTOOLS%\WixUtilExtension.dll"

POPD

echo --- Cleaning up temporary files ---
IF EXIST %TEMP% RMDIR /S /Q %TEMP%
IF EXIST %OUTDIR% RMDIR /S /Q %OUTDIR%
IF EXIST %SETUP%\*.wixpdb DEL /Q %SETUP%\*.wixpdb
IF EXIST %SETUP%\source\*.wixpdb DEL /Q %SETUP%\source\*.wixpdb
IF EXIST %SETUP%\source\FrameworkComponents.wxs DEL /Q %SETUP%\source\FrameworkComponents.wxs
IF EXIST %SETUP%\source\SDKComponents.wxs DEL /Q %SETUP%\source\SDKComponents.wxs

echo ************************************************************
echo  MBF msi creation complete
echo ************************************************************

GOTO EOF

:SOURCEROOTMISSING
echo Error: Source root path not specified.

:SETUPTMPFOLDERMISSING
echo Error: Setup folder not specified.

:EOF