'Design by Pongsakorn Poosankam
Public Class mainWinForm
    Private webcam As WebCam
    Private Sub mainWinForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        webcam = New WebCam()
        webcam.InitializeWebCam(imgVideo)
    End Sub

    Private Sub bntStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bntStart.Click
        webcam.Start()
    End Sub

    Private Sub bntStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bntStop.Click
        webcam.Stop()
    End Sub

    Private Sub bntContinue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bntContinue.Click
        webcam.Continue()
    End Sub

    Private Sub bntCapture_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bntCapture.Click
        imgCapture.Image = imgVideo.Image
    End Sub

    Private Sub bntSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bntSave.Click
        Helper.SaveImageCapture(imgCapture.Image)
    End Sub

    Private Sub bntVideoFormat_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bntVideoFormat.Click
        webcam.ResolutionSetting()
    End Sub

    Private Sub bntVideoSource_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bntVideoSource.Click
        webcam.AdvanceSetting()
    End Sub
End Class
