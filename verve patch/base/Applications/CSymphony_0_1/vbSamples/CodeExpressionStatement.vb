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

			Dim ConsoleWrite As MethodRef  = New MethodRef ("Console", "Write")
			
			enter (ex (ConsoleWrite, newLit("Example String")))
		End Sub
	End Class
End Namespace
			