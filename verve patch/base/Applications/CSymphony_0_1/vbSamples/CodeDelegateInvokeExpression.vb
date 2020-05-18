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

			Dim evnt As EventRef = New EventRef(my(), "MouseEvent")
			
			trigger(evnt, my(), newObj("System.EventArgs"))
		End Sub
	End Class
End Namespace
			