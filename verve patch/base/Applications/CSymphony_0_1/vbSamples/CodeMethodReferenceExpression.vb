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

			declClass(TypePublic, "MyClasss")
			declMethod(MemPublic, "System.Void", "MyMethod")

			Dim testMethod As MethodRef = New MethodRef (my(), "TestMethod")

			enter (ex(testMethod))
		End Sub
	End Class
End Namespace
			