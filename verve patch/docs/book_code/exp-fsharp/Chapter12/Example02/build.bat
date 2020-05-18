@ECHO OFF
SET param=%1
IF (%param%) == (clean) GOTO CLEAN

:BUILD
IF NOT EXIST ExprLexer.fs fslex ExprLexer.fsl
IF NOT EXIST ExprParser.fs fsyacc ExprParser.fsy
fsc -o SymDiff.exe Expr.fs ExprParser.fs ExprLexer.fs ExprUtil.fs VisualExpr.fs Main.fs
exit

:CLEAN
del ExprLexer.fs
del ExprParser.fs ExprParser.fsi
del *.exe
