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
using System.Configuration;

using Upperbay.Worker.EtherAccess;
using Upperbay.Core.Logging;
using Upperbay.Core.Library;
using System.Security.Policy;
using System.Xml.Linq;
using System.Security.Cryptography;
using System.ServiceProcess;
using System.Deployment.Application;
using System.Reflection;
using System.Xml;



namespace RainMan
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
            string playerServiceName = dirName; //"CurrentForCarbon"
         
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

            textBoxRainManEnabled.Text = ConfigurationManager.AppSettings["RainManEnabled"];
            textBoxRainManChainEnabled.Text = ConfigurationManager.AppSettings["RainManChainEnabled"];
            Community.Text = ConfigurationManager.AppSettings["RainManCommunity"];
            TextBoxLotNumber.Text = ConfigurationManager.AppSettings["RainManLotNumber"];
            textBoxZipcode.Text = ConfigurationManager.AppSettings["RainManZipcode"];
            textBoxPeriodInHours.Text = ConfigurationManager.AppSettings["RainManPeriodHours"];
            textBoxChainServerURL.Text = ConfigurationManager.AppSettings["RainManServerURL"];
            textBoxChainContract.Text = ConfigurationManager.AppSettings["RainManChainContract"];
            textBoxChainAddress.Text = ConfigurationManager.AppSettings["RainManChainAddress"];
            textBoxChainID.Text = ConfigurationManager.AppSettings["RainManChainID"];
            textBoxSlice.Text = "0";

            ServiceController controller = ServiceController.GetServices()
                          .FirstOrDefault(s => s.ServiceName == playerServiceName);
            if (controller == null)
            {
                buttonActivate.BackColor = Color.Aqua;
                buttonDeactivate.BackColor = Color.Aqua;
                buttonRun.BackColor = Color.Aqua;
                buttonStop.BackColor = Color.Aqua;
            }
            else if (controller.Status == ServiceControllerStatus.Running)
            {
                buttonActivate.BackColor = Color.LightGreen;
                buttonDeactivate.BackColor = Color.LightGreen;
                buttonRun.BackColor = Color.LightGreen;
                buttonStop.BackColor = Color.LightGreen;
            }
            else if (controller.Status == ServiceControllerStatus.Stopped)
            {
                buttonActivate.BackColor = Color.LightGreen;
                buttonDeactivate.BackColor = Color.LightGreen;
                buttonRun.BackColor = Color.Aqua;
                buttonStop.BackColor = Color.Aqua;
            }


       
         
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

        private void Community_TextChanged(object sender, EventArgs e)
        {
            string newText = this.Community.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\RainAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='RainManCommunity']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "Community_TextChanged", buttons);
            }

        }

        private void TextBoxLotNumber_TextChanged(object sender, EventArgs e)
        {
            string newText = this.TextBoxLotNumber.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\RainAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='RainManLotNumber']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "TextBoxLotNumber_TextChanged", buttons);
            }
        }

        private void textBoxZipcode_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBoxZipcode.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\RainAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='RainManZipcode']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBoxZipcode_TextChanged", buttons);
            }
        }

        private void textBoxPeriodInHours_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBoxPeriodInHours.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\RainAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='RainManPeriodHours']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBoxPeriodInHours", buttons);
            }
        }

        private void textBoxChainID_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBoxChainID.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\RainAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='RainManChainID']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBoxPeriodInHours", buttons);
            }
        }

        private void textBoxChainContract_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBoxChainContract.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\RainAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='RainManChainContract']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBoxChainContract_TextChanged", buttons);
            }

        }

        private void textBoxChainKey_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxChainServerURL_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBoxChainServerURL.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\RainAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='RainManServerURL']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBoxChainServerURL_TextChanged", buttons);
            }
        }

        private void textBoxSlice_TextChanged(object sender, EventArgs e)
        {

        }

        private void labelClusterName_Click(object sender, EventArgs e)
        {

        }

        private void buttonActivate_Click(object sender, EventArgs e)
        {
            string currentDir = Directory.GetCurrentDirectory();
            DirectoryInfo dir = new DirectoryInfo(currentDir);
            string dirName = dir.Name;
            Log2.Debug("Current Dir Name = " + dirName);
            string playerServiceName = dirName; //"CurrentForCarbon"

            try
            {
                ServiceController controller = ServiceController.GetServices()
                          .FirstOrDefault(s => s.ServiceName == playerServiceName);
                if (controller == null)
                {
                    string filename = "installutil.exe";
                    string cParam = "Upperbay.Agent.Colony.exe";
                    var proc = System.Diagnostics.Process.Start(filename, cParam);
                    buttonActivate.BackColor = Color.LightGreen;
                    buttonDeactivate.BackColor = Color.LightGreen;
                }
                else
                {
                    string message = "Already Activated";
                    string title = "Activate";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result = MessageBox.Show(message, title, buttons);
                    if (result == DialogResult.OK)
                    {
                    }
                    buttonActivate.BackColor = Color.LightGreen;
                    buttonDeactivate.BackColor = Color.LightGreen;
                }

            }
            catch (Exception ex)
            {

                string message = "Error Activating! " + ex.Message;
                Log2.Error(message);
                string title = "Activate";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.OK)
                {
                }
            }
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            string currentDir = Directory.GetCurrentDirectory();
            DirectoryInfo dir = new DirectoryInfo(currentDir);
            string dirName = dir.Name;
            Log2.Debug("Current Dir Name = " + dirName);
            string playerServiceName = dirName; //"CurrentForCarbon"

            try
            {
                ServiceController controller = ServiceController.GetServices()
                         .FirstOrDefault(s => s.ServiceName == playerServiceName);
                if (controller == null)
                {
                    string message = "Please Activate";
                    string title = "Start Player";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result = MessageBox.Show(message, title, buttons);
                    if (result == DialogResult.OK)
                    {
                    }

                    buttonActivate.BackColor = Color.Aqua;
                    buttonDeactivate.BackColor = Color.Aqua;
                    buttonRun.BackColor = Color.Aqua;
                    buttonStop.BackColor = Color.Aqua;
                }
                else if (controller.Status == ServiceControllerStatus.Running)
                {
                    buttonActivate.BackColor = Color.LightGreen;
                    buttonDeactivate.BackColor = Color.LightGreen;
                    buttonRun.BackColor = Color.LightGreen;
                    buttonStop.BackColor = Color.LightGreen;
                }
                else if (controller.Status == ServiceControllerStatus.Stopped)
                {
                    controller.Start();

                    buttonActivate.BackColor = Color.LightGreen;
                    buttonDeactivate.BackColor = Color.LightGreen;
                    buttonRun.BackColor = Color.LightGreen;
                    buttonStop.BackColor = Color.LightGreen;
                }
            }
            catch (Exception ex)
            {
                string message = "Error Starting Player" + ex.Message;
                Log2.Error(message);
                string title = "Start Player";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.OK)
                {
                }
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            string currentDir = Directory.GetCurrentDirectory();
            DirectoryInfo dir = new DirectoryInfo(currentDir);
            string dirName = dir.Name;
            Log2.Debug("Current Dir Name = " + dirName);
            string playerServiceName = dirName; //CurrentForCarbon"

            try
            {
                ServiceController controller = ServiceController.GetServices()
                         .FirstOrDefault(s => s.ServiceName == playerServiceName);
                if (controller == null)
                {
                    string message = "Please Activate";
                    string title = "Stop Player";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result = MessageBox.Show(message, title, buttons);
                    if (result == DialogResult.OK)
                    {
                    }

                    buttonActivate.BackColor = Color.Aqua;
                    buttonDeactivate.BackColor = Color.Aqua;
                    buttonRun.BackColor = Color.Aqua;
                    buttonStop.BackColor = Color.Aqua;
                }
                else if (controller.Status == ServiceControllerStatus.Running)
                {
                    controller.Stop();

                    buttonActivate.BackColor = Color.LightGreen;
                    buttonDeactivate.BackColor = Color.LightGreen;
                    buttonRun.BackColor = Color.Aqua;
                    buttonStop.BackColor = Color.Aqua;
                }
                else if (controller.Status == ServiceControllerStatus.Stopped)
                {

                    buttonActivate.BackColor = Color.LightGreen;
                    buttonDeactivate.BackColor = Color.LightGreen;
                    buttonRun.BackColor = Color.Aqua;
                    buttonStop.BackColor = Color.Aqua;
                }
            }
            catch (Exception ex)
            {
                string message = "Error Stopping!" + ex.Message;
                Log2.Error(message);
                string title = "Stop Player";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.OK)
                {
                }
            }
        }

        private void buttonDeactivate_Click(object sender, EventArgs e)
        {
            string currentDir = Directory.GetCurrentDirectory();
            DirectoryInfo dir = new DirectoryInfo(currentDir);
            string dirName = dir.Name;
            Log2.Debug("Current Dir Name = " + dirName);
            string playerServiceName = dirName; //"CurrentForCarbon"

            try
            {
                ServiceController controller = ServiceController.GetServices()
                          .FirstOrDefault(s => s.ServiceName == playerServiceName);
                if (controller == null)
                {
                    string message = "Player Already Deactivated";
                    string title = "Deactivate Player";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result = MessageBox.Show(message, title, buttons);
                    if (result == DialogResult.OK)
                    {
                    }
                    buttonActivate.BackColor = Color.Aqua;
                    buttonDeactivate.BackColor = Color.Aqua;
                    buttonRun.BackColor = Color.Aqua;
                    buttonStop.BackColor = Color.Aqua;
                }
                else
                {
                    string filename = "installutil.exe";
                    string cParam = "Upperbay.Agent.Colony.exe /u";
                    var proc = System.Diagnostics.Process.Start(filename, cParam);
                    buttonActivate.BackColor = Color.Aqua;
                    buttonDeactivate.BackColor = Color.Aqua;
                    buttonRun.BackColor = Color.Aqua;
                    buttonStop.BackColor = Color.Aqua;
                }

            }
            catch (Exception ex)
            {
                string message = "Error Deactivating! " + ex.Message;
                Log2.Error(message);
                string title = "Deactivate Player";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.OK)
                {
                }
            }
        }

        private void textBoxRainManEnabled_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBoxRainManEnabled.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\RainAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='RainManEnabled']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBoxRainManEnabled", buttons);
            }
        }

        private void textBoxRainManChainEnabled_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBoxRainManChainEnabled.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\RainAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='RainManChainEnabled']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBoxRainManChainEnabled_TextChanged", buttons);
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

   

        private void lblSamples_Click(object sender, EventArgs e)
        {

        }

        private void buttonGetSlice_Click(object sender, EventArgs e)
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

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
