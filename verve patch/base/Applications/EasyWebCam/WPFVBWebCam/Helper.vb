Imports System
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Collections.Generic
Imports System.Windows.Media.Imaging

'Design by Pongsakorn Poosankam
Class Helper
    'Block Memory Leak
    <System.Runtime.InteropServices.DllImport("gdi32.dll")> _
    Public Shared Function DeleteObject(ByVal handle As IntPtr) As Boolean
    End Function
    Public Shared bs As BitmapSource
    Public Shared ip As IntPtr
    Public Shared Function LoadBitmap(ByVal source As System.Drawing.Bitmap) As BitmapSource

        ip = source.GetHbitmap()


        bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ip, IntPtr.Zero, System.Windows.Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions())

        DeleteObject(ip)

        Return bs

    End Function
    Public Shared Sub SaveImageCapture(ByVal bitmap As BitmapSource)
        Dim encoder As New JpegBitmapEncoder()
        encoder.Frames.Add(BitmapFrame.Create(bitmap))
        encoder.QualityLevel = 100


        ' Configure save file dialog box
        Dim dlg As New Microsoft.Win32.SaveFileDialog()
        dlg.FileName = "Image"
        ' Default file name
        dlg.DefaultExt = ".Jpg"
        ' Default file extension
        dlg.Filter = "Image (.jpg)|*.jpg"
        ' Filter files by extension
        ' Show save file dialog box
        Dim result As Nullable(Of Boolean) = dlg.ShowDialog()

        ' Process save file dialog box results
        If result = True Then
            ' Save Image
            Dim filename As String = dlg.FileName
            Dim fstream As New FileStream(filename, FileMode.Create)
            encoder.Save(fstream)
            fstream.Close()
        End If

    End Sub
End Class
