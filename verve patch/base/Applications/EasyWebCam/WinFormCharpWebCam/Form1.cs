using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;



namespace WinFormCharpWebCam
{
    //Design by Pongsakorn Poosankam
    public partial class mainWinForm : Form
    {
        public mainWinForm()
        {
            InitializeComponent();
        }
        WebCam webcam;
        private void mainWinForm_Load(object sender, EventArgs e)
        {
            webcam = new WebCam();
            webcam.InitializeWebCam(ref imgVideo);
        }

        private void bntStart_Click(object sender, EventArgs e)
        {
            webcam.Start();
        }

        private void bntStop_Click(object sender, EventArgs e)
        {
            webcam.Stop();
        }

        private void bntContinue_Click(object sender, EventArgs e)
        {
            webcam.Continue();
        }

        private void bntCapture_Click(object sender, EventArgs e)
        {
            imgCapture.Image = imgVideo.Image;
        }

        private void bntSave_Click(object sender, EventArgs e)
        {
            Helper.SaveImageCapture(imgCapture.Image);
        }

        private void bntVideoFormat_Click(object sender, EventArgs e)
        {
            webcam.ResolutionSetting();
        }

        private void bntVideoSource_Click(object sender, EventArgs e)
        {
            webcam.AdvanceSetting();
        }

        
    }
}
