using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Upperbay.Worker.EtherAccess;
using Upperbay.Core.Logging;
using Upperbay.Core.Library;
using System.Security.Policy;
using System.Xml.Linq;
using System.Security.Cryptography;
using System.Deployment.Application;
using System.Reflection;
using System.Xml;
using System.Configuration;

namespace CloudBurst
{
    public partial class Form1 : Form
    {
        public Form1()
        {
           
            InitializeComponent();

            MyAppConfig.SetMyAppConfig("RainAgent");
            string traceMode = MyAppConfig.GetParameter("TraceMode");
            Log2.LogInit("RainMan", "RainAgent", traceMode);

            string currentDir = Directory.GetCurrentDirectory();
            DirectoryInfo dir = new DirectoryInfo(currentDir);
            string dirName = dir.Name;
            Log2.Debug("Current Dir Name = " + dirName);
            string playerServiceName = dirName; 

            labelServiceName.Text = playerServiceName;
            GetAllSoilRecords.BackColor = Color.Azure;

            //labelVersion.Text = CurrentVersion;
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.File = "config\\RainAgent.app.config";
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            ConfigurationManager.GetSection("appSettings");

            labelClusterName.Text = MyAppConfig.GetParameter("RainManCluster");
            labelVersion.Text = MyAppConfig.GetParameter("RainManVersion");

            textBoxSlice.Text = "0";

         

        }

        private void GetAllSoilRecords_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            try
            {
                RainLedger rainLedger = new RainLedger();

                int count = rainLedger.GetSliceCount();

                for (int i = 0; i < count; i++)
                {
                    uint j = (uint)i;
                    var Task = rainLedger.LogSoilEvents(j);
                }



                MessageBox.Show("Please check log directory for completion", "Get All Data From Ethereum", buttons);
            }
            catch (Exception ex)
            {
                Log2.Error("ERROR: ", ex);
            }
        }

        private void labelSamples_Click(object sender, EventArgs e)
        {
            try
            {
                RainLedger rainLedger = new RainLedger();

                int count = rainLedger.GetSoilCount();

                labelSamples.Text = count.ToString();

            }
            catch (Exception ex)
            {
                Log2.Error("ERROR: ", ex);
            }
        }

        private void labelSlices_Click(object sender, EventArgs e)
        {
            try
            {
                RainLedger rainLedger = new RainLedger();

                int count = rainLedger.GetSliceCount();

                labelSlices.Text = count.ToString();

            }
            catch (Exception ex)
            {
                Log2.Error("ERROR: ", ex);
            }
        }

        private void textBoxSlice_TextChanged(object sender, EventArgs e)
        {

        }

        private void Version_Click(object sender, EventArgs e)
        {

        }

        private void lblVersion_Click(object sender, EventArgs e)
        {

        }

        private void buttonGetDataSlice_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            try
            {
                string slice = textBoxSlice.Text;
                uint iSlice = uint.Parse(slice);

                RainLedger rainLedger = new RainLedger();

                var Task = rainLedger.LogSoilEvents(iSlice);

                MessageBox.Show("Please check log directory for completion", "Get All Data From Ethereum", buttons);
            }
            catch (Exception ex)
            {
                Log2.Error("ERROR: ", ex);
            }
        }
    }
}
