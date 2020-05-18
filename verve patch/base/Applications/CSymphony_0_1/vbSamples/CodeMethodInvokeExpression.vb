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

			Dim selfDispose As MethodRef = New MethodRef (my(), "Dispose")

			enter (ex (selfDispose, newLit(true)))

			enter(ex (New MethodRef(my(), "SomeMethod"), newLit(false)))
		End Sub
	End Class
End Namespace
			