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
			
			Dim testStr As FieldRef = New FieldRef (my(), "testStringField")
			
			declProperty(MemPublic, "System.String", "StringProperty")
			declGet()
			hi ("Get Statements come here")
			rtn(testStr.Expr())

			declSet()
			hi("Set statements come here")
			enter( equate (testStr.Expr(), propval()))
		End Sub
	End Class
End Namespace
			