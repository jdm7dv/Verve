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

			declClass(TypePublic, "Type1")
			
			declField(MemPrivate, "System.Int32", "integerField")

			Dim intFieldRef As FieldRef = New FieldRef (my(), "integerField")

			declCtor(MemPublic)
			
			declProperty(MemPrivate, "System.Int32", "integerProperty")
			
			declGet()
			rtn(intFieldRef.Expr())

			declSet()
			enter (equate(intFieldRef.Expr(), propval()))
		End Sub
	End Class
End Namespace
			