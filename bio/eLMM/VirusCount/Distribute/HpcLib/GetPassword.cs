//*********************************************************
//
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Apache License, Version 2.0.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MBT.Escience.HpcLib
{
    public partial class GetPassword : Form
    {
        public Dictionary<string, string> KludgeDirectory;

        public GetPassword()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            KludgeDirectory.Add("id", this.textBox1.Text);
            KludgeDirectory.Add("password", this.maskedTextBox1.Text);
            Close();
        }


    }
}
