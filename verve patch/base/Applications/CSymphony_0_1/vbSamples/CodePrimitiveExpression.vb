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
			declMethod(MemPublic, "System.Void", "MyMethod")

			'Literal stringPrimitive = "Test-String"
			'Literal intPrimitive = 10 
			'Literal floatPrimitive = New Literal(10.2)
			'Literal nullPrimitive = New Literal(null)

			Dim SomeVar As VarRef = New VarRef("SomeVar")

			enter (equate (SomeVar.Expr(), newLit("TestString")))
			enter (equate (SomeVar.Expr(), newLit(10)))
			enter (equate (SomeVar.Expr(), newLit(10.2)))
			enter (equate (SomeVar.Expr(), newLit(Nothing)))
		End Sub
	End Class
End Namespace
			