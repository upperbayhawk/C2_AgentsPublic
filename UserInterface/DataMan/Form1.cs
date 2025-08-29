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

namespace DataMan
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            MyAppConfig.SetMyAppConfig("DataAgent");
            string traceMode = MyAppConfig.GetParameter("TraceMode");
            Log2.LogInit("DataMan", "DataAgent", traceMode);

            string currentDir = Directory.GetCurrentDirectory();
            DirectoryInfo dir = new DirectoryInfo(currentDir);
            string dirName = dir.Name;
            Log2.Debug("Current Dir Name = " + dirName);
            string playerServiceName = dirName; //"CurrentForCarbon"

            labelServiceName.Text = playerServiceName;
           

            //labelVersion.Text = CurrentVersion;
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.File = "config\\DataAgent.app.config";
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            ConfigurationManager.GetSection("appSettings");

            labelClusterName.Text = MyAppConfig.GetParameter("DataManCluster");
            labelVersion.Text = MyAppConfig.GetParameter("DataManVersion");

            textBoxDataManEnabled.Text = ConfigurationManager.AppSettings["DataManEnabled"];
            textBoxPeriodSecs.Text = ConfigurationManager.AppSettings["DataManPeriodSecs"];
            textBox1Format.Text = ConfigurationManager.AppSettings["DataManFormat"];

            textBox1.Text = ConfigurationManager.AppSettings["DataManTagAttrib1"];
            textBox2.Text = ConfigurationManager.AppSettings["DataManTagAttrib2"];
            textBox3.Text = ConfigurationManager.AppSettings["DataManTagAttrib3"];
            textBox4.Text = ConfigurationManager.AppSettings["DataManTagAttrib4"];
            textBox5.Text = ConfigurationManager.AppSettings["DataManTagAttrib5"];
            textBox6.Text = ConfigurationManager.AppSettings["DataManTagAttrib6"];
            textBox7.Text = ConfigurationManager.AppSettings["DataManTagAttrib7"];
            textBox8.Text = ConfigurationManager.AppSettings["DataManTagAttrib8"];
            textBox9.Text = ConfigurationManager.AppSettings["DataManTagAttrib9"];
            textBox10.Text = ConfigurationManager.AppSettings["DataManTagAttrib10"];


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
                string fileName = Path.Combine(Application.StartupPath, "config\\DataAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='DataManPeriodSecs']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBoxPeriodSecs", buttons);
            }
        }

        private void textboxDataManEnabled(object sender, EventArgs e)
        {
            string newText = this.textBoxDataManEnabled.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\DataAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='DataManEnabled']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBoxPowerManEnabled", buttons);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBox1.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\DataAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='DataManTagAttrib1']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBox1_TextChanged", buttons);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBox2.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\DataAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='DataManTagAttrib2']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBox2_TextChanged", buttons);
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBox3.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\DataAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='DataManTagAttrib3']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBox3_TextChanged", buttons);
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBox4.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\DataAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='DataManTagAttrib4']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBox4_TextChanged", buttons);
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBox5.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\DataAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='DataManTagAttrib5']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBox5_TextChanged", buttons);
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBox6.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\DataAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='DataManTagAttrib6']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBox6_TextChanged", buttons);
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBox7.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\DataAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='DataManTagAttrib7']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBox7_TextChanged", buttons);
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBox8.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\DataAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='DataManTagAttrib8']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBox8_TextChanged", buttons);
            }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBox9.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\DataAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='DataManTagAttrib9']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBox9_TextChanged", buttons);
            }
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBox10.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\DataAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='DataManTagAttrib10']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBox10_TextChanged", buttons);
            }
        }

        private void textBox1Format_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBox1Format.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\DataAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='DataManFormat']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBoxFormat_TextChanged", buttons);
            }
        }
    }
}
