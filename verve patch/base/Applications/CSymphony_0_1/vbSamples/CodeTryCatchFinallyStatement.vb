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
			declMethod(MemPublic, "System.Void", "MonMethod")

			tryOn()
				enter (ex (my(), "ThrowApplicationException"))
			catchOn("System.ApplicationException", "ex")
				hi("Handle any SystemApplicationException here")
			catchOn("System.CriticalException", "ex")
				hi("Handle any other exception type here")
			finallyOn()
				hi ("Handle any finally block statements")
			tryOff()
		End Sub
	End Class
End Namespace
			