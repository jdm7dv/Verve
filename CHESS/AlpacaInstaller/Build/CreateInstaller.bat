@ECHO OFF

:: Make sure we've included the VS environment vars
SET VCVarsBat="%VS100COMNTOOLS%..\..\VC\vcvarsall.bat"
CALL %VCVarsBat% x86
IF ERRORLEVEL 1 PAUSE

:: Note: The xml sets the default target to CreateInstaller
msbuild alpaca.build.xml

PAUSE
