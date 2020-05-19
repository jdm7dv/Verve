REM -- ***************************************************************************************************************************
REM --     Description
REM -- ***************************************************************************************************************************
REM -- Prepares folder and files needed for generating chm file and then calls DocGenForTFS.cmd to create chm file.
REM -- This script first tries to create chm file from debug binalries if it fails then tries to create chm from release binaries.
REM -- ***************************************************************************************************************************

@Echo Off
@if not "%ECHO%"=="" Echo %ECHO%

if "%1" == "" GOTO EOF
if "%2" == "" GOTO EOF

IF NOT EXIST %1\MBF.dll echo %1\MBF.dll is missing
IF NOT EXIST %1\MBF.dll GOTO EOF
IF NOT EXIST %1\MBF.xml echo %1\MBF.xml is missing, XML comments in code will be ignored

SET TARGETFOLDER=%1
SET SOURCEROOT=%2


REM ********** Set path for sandcastle ****************************

SET DXROOT=%SANDCASTLE_INSTALLPATH%
SET DXALTPATH=%SOURCEROOT%\BuildScripts

REM ********** output folder to stage the documentation ****************************
mkdir %TARGETFOLDER%\Doc

pushd "%TARGETFOLDER%\DOC"

REM ********** Call MRefBuilder ****************************

"%DXROOT%\ProductionTools\MRefBuilder" "%TARGETFOLDER%\MBF.dll" /config:"%SOURCEROOT%\BuildScripts\MBFRefBuilder.config" /internal- /out:"%TARGETFOLDER%\Doc\reflection.org"

REM ********** Apply Transforms ****************************

"%DXROOT%\ProductionTools\XslTransform" /xsl:"%DXROOT%\ProductionTransforms\ApplyVSDocModel.xsl" "%TARGETFOLDER%\Doc\reflection.org" /xsl:"%DXROOT%\ProductionTransforms\AddFriendlyFilenames.xsl" /out:"%TARGETFOLDER%\Doc\reflection.xml"

"%DXROOT%\ProductionTools\XslTransform" /xsl:"%DXROOT%\ProductionTransforms\ReflectionToManifest.xsl"  "%TARGETFOLDER%\Doc\reflection.xml" /out:"%TARGETFOLDER%\Doc\manifest.xml"

call "%DXROOT%\Presentation\vs2005\copyOutput.bat"

REM ********** Call BuildAssembler ****************************

set CommentsDir="%TARGETFOLDER%"& "%DXROOT%\ProductionTools\BuildAssembler" /config:"%DXROOT%\Presentation\vs2005\configuration\sandcastle.config" "%TARGETFOLDER%\Doc\manifest.xml"
 
"%DXROOT%\ProductionTools\XslTransform" /xsl:"%DXALTPATH%\ReflectionToChmProject.xsl" "%TARGETFOLDER%\Doc\reflection.xml" /out:"%TARGETFOLDER%\Doc\Output\MBF.hhp"

REM **************Generate an intermediate Toc file that simulates the Whidbey TOC format.

"%DXROOT%\ProductionTools\XslTransform" /xsl:"%DXROOT%\ProductionTransforms\createvstoc.xsl" "%TARGETFOLDER%\Doc\reflection.xml" /out:"%TARGETFOLDER%\Doc\toc.xml"

REM ************ Generate CHM help project ******************************

"%DXROOT%\ProductionTools\XslTransform" /xsl:"%DXROOT%\ProductionTransforms\TocToChmContents.xsl" "%TARGETFOLDER%\Doc\toc.xml" /arg:html="%TARGETFOLDER%\Doc\Output\html" /out:"%TARGETFOLDER%\Doc\Output\MBF.hhc"

"%DXROOT%\ProductionTools\XslTransform" /xsl:"%DXALTPATH%\ReflectionToChmProject.xsl" "%TARGETFOLDER%\Doc\reflection.xml" /out:"%TARGETFOLDER%\Doc\Output\MBF.hhk"

"%HHC_INSTALLPATH%\hhc" "%TARGETFOLDER%\Doc\output\MBF.hhp"

popd

:EOF
