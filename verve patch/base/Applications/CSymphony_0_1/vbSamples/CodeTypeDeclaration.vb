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

			declClass("TestType", "BaseType")

			declClass(TypePublic OR TypeSealed, "TestdType2","BaseType", "BaseType2")

			declInterface("aa", "bb")
		End Sub
	End Class
End Namespace
			