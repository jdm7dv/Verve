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

			Dim argText As ArgRef = New ArgRef("text")

			declMethod(MemPublic, "System.String","ReturnString", "System.String", argText)
			rtn(argText.Expr())
		End Sub
	End Class
End Namespace
			