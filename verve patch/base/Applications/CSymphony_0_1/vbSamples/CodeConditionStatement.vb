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

			'Dim boolVar As VarRef = New VarRef("boolVar")
			'Dim otherBoolVar As VarRef  = New VarRef("otherBoolVar")

			ifOn(newVar("boolVar"))
				hi ("If Outer If condition is true execute these statements")
				ifOn(newVar("otherBoolVar"))
					hi("If the Inner If is true execute these statements")
				elseOn()
					hi ("If the Inner If is false execute these statements")
				ifOff()

				hi("Continue with Other statements in the Outer If")

			elseOn()
				hi("Outer Else block. If condition is false, execute these statements.")
			ifOff()
	
		End Sub
	End Class
End Namespace
			