'Design by Pongsakorn Poosankam
Class Window1
    Private webcam As WebCam
    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        webcam = New WebCam()
        webcam.InitializeWebCam(imgVideo)
    End Sub

    Private Sub bntStart_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        webcam.Start()
    End Sub

    Private Sub bntStop_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        webcam.Stop()
    End Sub

    Private Sub bntContinue_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        webcam.Continue()
    End Sub

    Private Sub bntCapture_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        imgCapture.Source = imgVideo.Source
    End Sub

    Private Sub bntSaveImage_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Helper.SaveImageCapture(DirectCast(imgCapture.Source, BitmapSource))
    End Sub

    Private Sub bntResolution_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        webcam.ResolutionSetting()
    End Sub

    Private Sub bntSetting_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        webcam.AdvanceSetting()
    End Sub


End Class
