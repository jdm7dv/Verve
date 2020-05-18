@ECHO OFF
SET param=%1
IF (%param%) == (clean) GOTO CLEAN

:BUILD
IF NOT EXIST text2htmllex.fs fslex text2htmllex.fsl
fsc -o text2html.exe text2htmllex.fs text2html.fs
exit

:CLEAN
del text2htmllex.fs
del *.exe
del *.html

