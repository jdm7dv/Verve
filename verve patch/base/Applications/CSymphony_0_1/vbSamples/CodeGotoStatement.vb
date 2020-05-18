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
			declMethod(MemPublic, "System.Void", "MonMethod")

			jumpTo("JumpToLabel")

			Dim w As MethodRef  = New MethodRef ("System.Console", "WriteLine")

			enter (ex (w , newLit("Test Output")))

			declLabel("JumpToLabel", ex (w, newLit("Output from Labeled statement")))
		End Sub
	End Class
End Namespace
			