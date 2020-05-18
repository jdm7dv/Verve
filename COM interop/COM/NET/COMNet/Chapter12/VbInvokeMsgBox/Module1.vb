Imports System.Runtime.InteropServices

Module Module1
    ' A Declare function for the MessageBox API
    Declare Auto Function WinMsgBox Lib "user32.dll" _
    Alias "MessageBox" (ByVal hWnd As Integer, _
       ByVal txt As String, ByVal caption As String, _
       ByVal Typ As Integer) As Integer

    <DllImport("User32.dll")> _
    Public Function MessageBox(ByVal hWnd As Integer, _
   ByVal txt As String, ByVal caption As String, _
   ByVal Typ As Integer) As Integer
    End Function

    Class WinApi
        ' Using DllImport
        <DllImport("User32.dll")> _
        Public Shared Function MessageBox(ByVal hWnd As Integer, _
       ByVal txt As String, ByVal caption As String, _
       ByVal Typ As Integer) As Integer
        End Function
    End Class

    Sub Main()
        WinMsgBox(0, "Hello", "Using Declare", 0)

        MessageBox(0, "Hello again", "Using DllImport", 0)
        'WinApi.MessageBox(0, "Hello again", "Using DllImport", 0)
    End Sub

End Module
