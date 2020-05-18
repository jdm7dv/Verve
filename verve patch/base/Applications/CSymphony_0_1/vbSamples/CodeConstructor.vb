Option Strict On
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

			declClass(TypePublic, "BaseType")

			declCtor(MemPublic)
			
			declCtor(MemPublic, "System.String", "TestStringParameter")

			declClass(TypePublic, "DerivedType", "BaseType")
			
			Dim argTestParam As ArgRef  = New ArgRef("TestStringParameter")

			declCtor(MemPublic, "System.String" , argTestParam.Expr())
			parent(argTestParam.Expr())

			declCtor(MemPublic)
			parent(newLit("test"))

			declCtor(MemPublic, "System.Int32", "TestIntParameter")
			peer()
		End Sub
	End Class
End Namespace
			