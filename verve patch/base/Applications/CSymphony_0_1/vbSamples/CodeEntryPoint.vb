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
			uses("System")

			Dim ConsoleWrite As MethodRef = New MethodRef ("System.Console",  "WriteLine")
			'Dim HelloWorld As Literal = New Literal("Hello World !!")

			declClass(TypePublic, "Class1")
			declEntry()
			enter (ex (ConsoleWrite, newLit("Hello World")))
		End Sub
	End Class
End Namespace
			