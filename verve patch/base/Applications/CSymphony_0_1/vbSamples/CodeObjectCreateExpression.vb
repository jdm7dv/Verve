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
			declMethod(MemPublic, "System.Void", "MyMethod")
			enter (newObj("System.DateTime") )

		End Sub
	End Class
End Namespace
			