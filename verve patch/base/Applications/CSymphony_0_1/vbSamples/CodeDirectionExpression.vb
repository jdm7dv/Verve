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

			Dim TestParam As FieldRef = New FieldRef (my(), "TestParameter")

			Dim ThisMethod As MethodRef = New MethodRef (my(), "ThisMethod" )
			
			declClass(TypePublic, "Class1")
			declMethod(MemPublic, "System.Void", "ClassMethod")

			enter (ex (ThisMethod, refp(TestParam.Expr()) ))
		End Sub
	End Class
End Namespace
			