<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class mainWinForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.bntVideoSource = New System.Windows.Forms.Button
        Me.bntVideoFormat = New System.Windows.Forms.Button
        Me.bntSave = New System.Windows.Forms.Button
        Me.bntCapture = New System.Windows.Forms.Button
        Me.bntContinue = New System.Windows.Forms.Button
        Me.bntStop = New System.Windows.Forms.Button
        Me.bntStart = New System.Windows.Forms.Button
        Me.imgCapture = New System.Windows.Forms.PictureBox
        Me.imgVideo = New System.Windows.Forms.PictureBox
        CType(Me.imgCapture, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.imgVideo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'bntVideoSource
        '
        Me.bntVideoSource.Location = New System.Drawing.Point(425, 127)
        Me.bntVideoSource.Name = "bntVideoSource"
        Me.bntVideoSource.Size = New System.Drawing.Size(147, 23)
        Me.bntVideoSource.TabIndex = 17
        Me.bntVideoSource.Text = "Video Source"
        Me.bntVideoSource.UseVisualStyleBackColor = True
        '
        'bntVideoFormat
        '
        Me.bntVideoFormat.Location = New System.Drawing.Point(425, 98)
        Me.bntVideoFormat.Name = "bntVideoFormat"
        Me.bntVideoFormat.Size = New System.Drawing.Size(147, 23)
        Me.bntVideoFormat.TabIndex = 16
        Me.bntVideoFormat.Text = "Video Format"
        Me.bntVideoFormat.UseVisualStyleBackColor = True
        '
        'bntSave
        '
        Me.bntSave.Location = New System.Drawing.Point(326, 214)
        Me.bntSave.Name = "bntSave"
        Me.bntSave.Size = New System.Drawing.Size(79, 23)
        Me.bntSave.TabIndex = 15
        Me.bntSave.Text = "Save Image"
        Me.bntSave.UseVisualStyleBackColor = True
        '
        'bntCapture
        '
        Me.bntCapture.Location = New System.Drawing.Point(242, 214)
        Me.bntCapture.Name = "bntCapture"
        Me.bntCapture.Size = New System.Drawing.Size(85, 23)
        Me.bntCapture.TabIndex = 14
        Me.bntCapture.Text = "Capture Image"
        Me.bntCapture.UseVisualStyleBackColor = True
        '
        'bntContinue
        '
        Me.bntContinue.Location = New System.Drawing.Point(157, 214)
        Me.bntContinue.Name = "bntContinue"
        Me.bntContinue.Size = New System.Drawing.Size(61, 23)
        Me.bntContinue.TabIndex = 13
        Me.bntContinue.Text = "Continue"
        Me.bntContinue.UseVisualStyleBackColor = True
        '
        'bntStop
        '
        Me.bntStop.Location = New System.Drawing.Point(102, 214)
        Me.bntStop.Name = "bntStop"
        Me.bntStop.Size = New System.Drawing.Size(49, 23)
        Me.bntStop.TabIndex = 12
        Me.bntStop.Text = "Stop"
        Me.bntStop.UseVisualStyleBackColor = True
        '
        'bntStart
        '
        Me.bntStart.Location = New System.Drawing.Point(55, 214)
        Me.bntStart.Name = "bntStart"
        Me.bntStart.Size = New System.Drawing.Size(41, 23)
        Me.bntStart.TabIndex = 11
        Me.bntStart.Text = "Start"
        Me.bntStart.UseVisualStyleBackColor = True
        '
        'imgCapture
        '
        Me.imgCapture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.imgCapture.Location = New System.Drawing.Point(242, 39)
        Me.imgCapture.Name = "imgCapture"
        Me.imgCapture.Size = New System.Drawing.Size(163, 160)
        Me.imgCapture.TabIndex = 10
        Me.imgCapture.TabStop = False
        '
        'imgVideo
        '
        Me.imgVideo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.imgVideo.Location = New System.Drawing.Point(55, 39)
        Me.imgVideo.Name = "imgVideo"
        Me.imgVideo.Size = New System.Drawing.Size(163, 160)
        Me.imgVideo.TabIndex = 9
        Me.imgVideo.TabStop = False
        '
        'mainWinForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(584, 362)
        Me.Controls.Add(Me.bntVideoSource)
        Me.Controls.Add(Me.bntVideoFormat)
        Me.Controls.Add(Me.bntSave)
        Me.Controls.Add(Me.bntCapture)
        Me.Controls.Add(Me.bntContinue)
        Me.Controls.Add(Me.bntStop)
        Me.Controls.Add(Me.bntStart)
        Me.Controls.Add(Me.imgCapture)
        Me.Controls.Add(Me.imgVideo)
        Me.Name = "mainWinForm"
        Me.Text = "WinForm VB WebCam"
        CType(Me.imgCapture, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.imgVideo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents bntVideoSource As System.Windows.Forms.Button
    Private WithEvents bntVideoFormat As System.Windows.Forms.Button
    Private WithEvents bntSave As System.Windows.Forms.Button
    Private WithEvents bntCapture As System.Windows.Forms.Button
    Private WithEvents bntContinue As System.Windows.Forms.Button
    Private WithEvents bntStop As System.Windows.Forms.Button
    Private WithEvents bntStart As System.Windows.Forms.Button
    Private WithEvents imgCapture As System.Windows.Forms.PictureBox
    Private WithEvents imgVideo As System.Windows.Forms.PictureBox

End Class
