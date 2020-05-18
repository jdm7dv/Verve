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

			Dim handler As HandlerType = New HandlerType("System.EventHandler", mthd)
			
			Dim handler2 As HandlerType = New HandlerType("System.EventHandler", my(), "Method2")


			attach(evnt, handler)

			attach(evnt, handler2)
		End Sub
	End Class
End Namespace
			