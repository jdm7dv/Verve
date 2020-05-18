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

			Dim evnt As EventRef = New EventRef(my(), "TestEvent")
			
			Dim mthd as MethodRef = New MethodRef(my(), "TestMethod")

			detach(evnt, "System.EventHandler", mthd)

			detach(my(), "TestEvent2", "System.EventHandler", mthd)

			detach(evnt, "System.EventHandler", my(), "TestMethod2")

			detach(my(), "TestEvent3", "System.EventHandler", my(), "TestMethod3")

		End Sub
	End Class
End Namespace
			