@echo off
setlocal

set SSCBOOGIE_DIR=.
set COMPILER_DIR=..\..\SpecSharp\Microsoft.SpecSharp\Registration
set DEST_DIR=export
set DEST_BIN_DIR=%DEST_DIR%\Binaries

if exist %DEST_BIN_DIR%\1033 del /q %DEST_BIN_DIR%\1033\*
if exist %DEST_BIN_DIR% del /q %DEST_BIN_DIR%\*
if exist %DEST_DIR%\Templates\ProjectItems del /q %DEST_DIR%\Templates\ProjectItems\*
if exist %DEST_DIR%\Templates\Projects del /q %DEST_DIR%\Templates\Projects\*
if exist %DEST_DIR%\Templates del /q %DEST_DIR%\Templates\*

if not exist %DEST_DIR% mkdir %DEST_DIR%
if not exist %DEST_BIN_DIR% mkdir %DEST_BIN_DIR%
if not exist %DEST_BIN_DIR%\1033 mkdir %DEST_BIN_DIR%\1033

REM Copy the compiler stuff ----------------------
for %%f in (
  ContractsPropertyPane.dll ContractsPropertyPane.pdb
  IPropertyPane.dll IPropertyPane.pdb
  ITaskManager.dll ITaskManager.pdb
  Microsoft.SpecSharp.Runtime.dll Microsoft.SpecSharp.Runtime.pdb
  Microsoft.SpecSharp.dll Microsoft.SpecSharp.pdb
  Microsoft.SpecSharp.targets
  Microsoft.VisualStudio.Designer.Interfaces.dll
  Microsoft.VisualStudio.IntegrationHelper.dll Microsoft.VisualStudio.IntegrationHelper.pdb
  Microsoft.VisualStudio.Package.dll Microsoft.VisualStudio.Package.pdb
  Microsoft.VisualStudio.Shell.Interop.dll
  Microsoft.VisualStudio.TextManager.Interop.dll
  Microsoft.Visualstudio.OLE.Interop.dll
  Mscorlib.Contracts.dll Mscorlib.Contracts.pdb
  PropertyPage.dll PropertyPage.pdb
  System.Compiler.Contracts.dll System.Compiler.Contracts.pdb
  System.Compiler.Framework.Contracts.dll System.Compiler.Framework.Contracts.pdb
  System.Compiler.Framework.dll System.Compiler.Framework.pdb
  System.Compiler.Runtime.dll System.Compiler.Runtime.pdb
  System.Compiler.dll System.Compiler.pdb
  System.Contracts.dll System.Contracts.pdb
  System.Xml.Contracts.dll System.Xml.Contracts.pdb
  TaskManager.dll TaskManager.pdb
  ssc.exe ssc.pdb
) do (
  copy %COMPILER_DIR%\Binaries\%%f %DEST_BIN_DIR%
)
REM ...and copy these to a different name --------------------
for %%f in (
  Clean.cmd Register.cmd
) do (
  copy %COMPILER_DIR%\Export%%f %DEST_BIN_DIR%\%%f
)
REM ...and copy the 1033 directory and its contents --------------------
for %%f in (
  Microsoft.SpecSharp.resources.dll
  PropertyPageUI.dll PropertyPageUI.pdb
  TaskManagerUI.dll TaskManagerUI.pdb
) do (
  copy %COMPILER_DIR%\Binaries\1033\%%f %DEST_BIN_DIR%\1033
)

rem Copy SscBoogie ---------------------------------------
rem First, pieces from Boogie ----------------------------
for %%f in (
  AbsInt.dll AbsInt.pdb
  AIFramework.dll AIFramework.pdb
  Basetypes.dll Basetypes.pdb
  CodeContractsExtender.dll CodeContractsExtender.pdb
  Core.dll Core.pdb
  Graph.dll Graph.pdb
  Model.dll Model.pdb
  ParserHelper.dll ParserHelper.pdb
  Provers.Simplify.dll Provers.Simplify.pdb
  Provers.SMTLib.dll Provers.SMTLib.pdb
  Provers.Z3.dll Provers.Z3.pdb
  VCExpr.dll VCExpr.pdb
  VCGeneration.dll VCGeneration.pdb
  TypedUnivBackPred2.sx UnivBackPred2.sx UnivBackPred2.smt
  FSharp.Core.dll
  FSharp.PowerPack.dll
  Microsoft.Contracts.dll
) do (
  copy %SSCBOOGIE_DIR%\%%f %DEST_BIN_DIR%
)
rem Next, SscBoogie specific pieces ----------------------
for %%f in (
  BoogiePlugin.dll BoogiePlugin.pdb
  BytecodeTranslation.dll BytecodeTranslation.pdb
  DriverHelper.dll DriverHelper.pdb
  PRELUDE.bpl
  Prelude.dll Prelude.pdb
  SscBoogie.exe SscBoogie.pdb
) do (
  copy %SSCBOOGIE_DIR%\%%f %DEST_BIN_DIR%
)

rem Finally, copy the Templates, which are needed to create new projects in VS 2008
xcopy /Y /I /S %COMPILER_DIR%\Templates %DEST_DIR%\Templates

echo Done.  Now, manually put the contents of the %DEST_DIR% directory into SpecSharp.zip
