Imports System
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Windows.Forms

'Design by Pongsakorn Poosankam
Class Helper

    Public Shared Sub SaveImageCapture(ByVal image As System.Drawing.Image)

        Dim s As New SaveFileDialog()
        s.FileName = "Image"
        ' Default file name
        s.DefaultExt = ".Jpg"
        ' Default file extension
        s.Filter = "Image (.jpg)|*.jpg"
        ' Filter files by extension
        ' Show save file dialog box
        ' Process save file dialog box results
        If s.ShowDialog() = DialogResult.OK Then
            ' Save Image
            Dim filename As String = s.FileName
            Dim fstream As New FileStream(filename, FileMode.Create)
            image.Save(fstream, System.Drawing.Imaging.ImageFormat.Jpeg)

            fstream.Close()

        End If
    End Sub
End Class

