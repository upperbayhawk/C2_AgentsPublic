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
using Upperbay.Core.Logging;
using Upperbay.Core.Library;
using System.Security.Policy;
using System.Xml.Linq;
using System.Security.Cryptography;
using System.ServiceProcess;
using System.Deployment.Application;
using System.Reflection;
using System.Xml;

namespace PowerMan
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            MyAppConfig.SetMyAppConfig("PowerAgent");
            string traceMode = MyAppConfig.GetParameter("TraceMode");
            Log2.LogInit("PowerMan", "PowerAgent", traceMode);

            string currentDir = Directory.GetCurrentDirectory();
            DirectoryInfo dir = new DirectoryInfo(currentDir);
            string dirName = dir.Name;
            Log2.Debug("Current Dir Name = " + dirName);
            string playerServiceName = dirName; //"CurrentForCarbon"

            labelServiceName.Text = playerServiceName;
           

            //labelVersion.Text = CurrentVersion;
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.File = "config\\PowerAgent.app.config";
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            ConfigurationManager.GetSection("appSettings");

            labelClusterName.Text = MyAppConfig.GetParameter("PowerManCluster");
            labelVersion.Text = MyAppConfig.GetParameter("PowerManVersion");

            textBoxPowerManEnabled.Text = ConfigurationManager.AppSettings["PowerManEnabled"];
            textBoxPeriodSecs.Text = ConfigurationManager.AppSettings["PowerManPeriodSecs"];

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

       

        private void textBoxPeriodSecs_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBoxPeriodSecs.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\PowerAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='PowerManPeriodSecs']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBoxPeriodSecs", buttons);
            }
        }

        private void textboxPowerManEnabled(object sender, EventArgs e)
        {
            string newText = this.textBoxPowerManEnabled.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\PowerAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='PowerManEnabled']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBoxPowerManEnabled", buttons);
            }
        }
    }
}
