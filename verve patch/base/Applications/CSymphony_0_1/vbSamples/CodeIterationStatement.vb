Option Strict Off
Option Explicit On

Imports System
Imports System.CodeDom
Imports System.CodeDom.Compiler
Imports System.Collections
Imports CSymphony.Lib

Namespace CSymphony.CSy
	Public Class CSyUnit
		 Inherits GraphUnit
		Overrides Public Sub Build()
			ns("MyNameSpace")

			declClass(TypePublic, "MyClass")
			declMethod(MemPublic, "System.Void", "MonMethod")

			Dim testInt As VarRef = declVar("System.Int32", "testInt", newLit(0))
			
			forOn(equate (testInt.Expr(), newLit(1)))
			forTest(oper(testInt.Expr(), lt, newLit(10)))
			forInc(oper(testInt.Expr(), add, newLit(1)))
			hi ("for statements come here")
			
			Dim writeLine As MethodRef  = New MethodRef ("System.Console", "WriteLine")
			Dim testIntToStr As MethodRef = New MethodRef (testInt.Expr(), "ToString")

			enter (ex (writeLine, ex(testIntToStr)))
			forOff()
	
		End Sub
	End Class
End Namespace
			