@ECHO OFF
SET param=%1
IF (%param%) == (clean) GOTO CLEAN

:BUILD
IF NOT EXIST kittyLexer.fs fslex kittyLexer.fsl
IF NOT EXIST kittyParser.fs fsyacc kittyParser.fsy
fsc -o kitty.exe kittyAst.fs kittyParser.fs kittyLexer.fs main.fs
fsc -a -o KittySyntax.dll kittyAst.fs kittyParser.fs kittyLexer.fs
exit

:CLEAN
del kittyLexer.fs
del kittyParser.fs kittyParser.fsi
del *.exe
