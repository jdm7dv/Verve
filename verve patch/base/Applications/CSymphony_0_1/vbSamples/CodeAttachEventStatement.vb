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
			
			Dim mthd As MethodRef = New MethodRef(my(), "TestMethod")

			attach(evnt, "System.EventHandler", mthd)

			attach(my(), "TestEvent2", "System.EventHandler", mthd)

			attach(evnt, "System.EventHandler", my(), "TestMethod2")

			attach(my(), "TestEvent3", "System.EventHandler", my(), "TestMethod3")

		End Sub
	End Class
End Namespace
			