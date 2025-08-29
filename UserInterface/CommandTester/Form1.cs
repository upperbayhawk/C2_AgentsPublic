//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

using Upperbay.Worker.Voice;
using Upperbay.Core.Logging;
using Upperbay.Core.Library;

namespace CommandTester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            MyAppConfig.SetMyAppConfig("ClusterAgent");
        }
        private void SelectFileButton_Click(object sender, EventArgs e)
        {
            // Show the dialog and get result.
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void RunFileButton_Click(object sender, EventArgs e)
        {
            ConfigurationManager.RefreshSection("appSettings");
            ConfigurationManager.GetSection("appSettings");
            string voiceEnabled = ConfigurationManager.AppSettings["VoiceEnabled"];
            string voiceCommandsEnabled = ConfigurationManager.AppSettings["VoiceCommandsEnabled"];

            //Voice.SetVoice("Microsoft Hazel Desktop");
            Voice.SpeakVoiceCommandFile(textBox1.Text);
        }
    }
}
