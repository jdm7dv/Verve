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

			declClass(TypePublic, "MyClasss", "ParentClass")
			declMethod(MemPublic, "System.Void", "MyMethod")

			Dim selfDispose As MethodRef = New MethodRef (mySuper(), "Dispose")

			enter (ex (selfDispose))

			enter(ex (New MethodRef(mySuper(), "SomeMethod")))
		End Sub
	End Class
End Namespace
			