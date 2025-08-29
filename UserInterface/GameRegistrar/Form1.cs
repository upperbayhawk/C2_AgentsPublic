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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Xml;
using System.IO;
using System.Security.Cryptography;
using System.ServiceProcess;
using System.Deployment.Application;
using System.Reflection;

using Upperbay.Worker.EtherAccess;
using Upperbay.Core.Library;
using Upperbay.Core.Logging;
using Upperbay.Assistant;
using Upperbay.Worker.MQTT;
using Upperbay.Worker.JSON;
using Upperbay.Agent.Interfaces;

namespace CurrentForCarbon
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            InitForm();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitForm()
        {
            MyAppConfig.SetMyAppConfig("ClusterAgent");
            string traceMode = MyAppConfig.GetParameter("TraceMode");
            Log2.LogInit("GameRegistrar", "ClusterAgent", traceMode);

            string currentDir = Directory.GetCurrentDirectory();
            DirectoryInfo dir = new DirectoryInfo(currentDir);
            string dirName = dir.Name;
            Log2.Debug("Current Dir Name = " + dirName);
            string playerServiceName = dirName; //"CurrentForCarbon"
            labelServiceName.Text = playerServiceName;
            labelCluster.Text = MyAppConfig.GetParameter("ClusterName");
            labelVersion.Text = MyAppConfig.GetParameter("ClusterVersion");


            ServiceController controller = ServiceController.GetServices()
                          .FirstOrDefault(s => s.ServiceName == playerServiceName);
            if (controller == null)
            {
                buttonActivatePlayer.BackColor = Color.Aqua;
                buttonDeactivatePlayer.BackColor = Color.Aqua;
                buttonStartPlayer.BackColor = Color.Aqua;
                buttonStopPlayer.BackColor = Color.Aqua;
            }
            else if (controller.Status == ServiceControllerStatus.Running)
            {
                buttonActivatePlayer.BackColor = Color.LightGreen;
                buttonDeactivatePlayer.BackColor = Color.LightGreen;
                buttonStartPlayer.BackColor = Color.LightGreen;
                buttonStopPlayer.BackColor = Color.LightGreen;
            }
            else if (controller.Status == ServiceControllerStatus.Stopped)
            {
                buttonActivatePlayer.BackColor = Color.LightGreen;
                buttonDeactivatePlayer.BackColor = Color.LightGreen;
                buttonStartPlayer.BackColor = Color.Aqua;
                buttonStopPlayer.BackColor = Color.Aqua;
            }

            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.File = "config\\ClusterAgent.app.config";
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            ConfigurationManager.GetSection("appSettings");

            textBoxEthereumAddress.Text = ConfigurationManager.AppSettings["EthereumExternalAddress"];
            textBoxEthereumKey.Text = ConfigurationManager.AppSettings["EthereumExternalPrivateKey"];
            textBoxDataConnectString.Text = ConfigurationManager.AppSettings["CurrentForCarbonWatts"];
            textBoxPlayerIdentifier.Text = ConfigurationManager.AppSettings["GamePlayerID"];

            string cluster = MyAppConfig.GetParameter("ClusterName");
            textBoxContractAddress.Text = MyAppConfig.GetClusterParameter(cluster, "EthereumContractAddress");

            textBoxGamePlayerName.Text = ConfigurationManager.AppSettings["GamePlayerName"];
            textBoxGamePlayerStreet.Text = ConfigurationManager.AppSettings["GamePlayerStreet"];
            textBoxGamePlayerCity.Text = ConfigurationManager.AppSettings["GamePlayerCity"];
            textBoxGamePlayerState.Text = ConfigurationManager.AppSettings["GamePlayerState"];
            textBoxGamePlayerZipcode.Text = ConfigurationManager.AppSettings["GamePlayerZipcode"];
            textBoxGamePlayerElectricCo.Text = ConfigurationManager.AppSettings["GamePlayerElectricCo"];
            textBoxGamePlayerEmail.Text = ConfigurationManager.AppSettings["GamePlayerEmail"];
            textBoxGamePlayerPhone.Text = ConfigurationManager.AppSettings["GamePlayerPhone"];
            textBoxGamePlayerNumber.Text = ConfigurationManager.AppSettings["GamePlayerNumber"];
            textBoxMqttUserName.Text = ConfigurationManager.AppSettings["MqttCloudLoginName"];
            textBoxMqttPassword.Text = ConfigurationManager.AppSettings["MqttCloudPassword"];
            textBoxTimezone.Text = ConfigurationManager.AppSettings["GamePlayerTimeZone"];

            return;
        }



        #region SERVICES
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void installServiceMenuItem_Click(object sender, EventArgs e)
        {
            string filename = "installutil.exe";
            string cParam = "Upperbay.Agent.Colony.exe";
            var proc = System.Diagnostics.Process.Start(filename, cParam);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startServiceMenuItem_Click(object sender, EventArgs e)
        {
            string currentDir = Directory.GetCurrentDirectory();
            DirectoryInfo dir = new DirectoryInfo(currentDir);
            string dirName = dir.Name;
            Log2.Debug("Current Dir Name = " + dirName);
            string playerServiceName = dirName; //"CurrentForCarbon"

            try
            {
                ServiceController controller = new ServiceController();

                controller.MachineName = ".";
                controller.ServiceName = playerServiceName;

                // Start the service
                controller.Start();


            }
            catch (Exception ex)
            {
                string message = "Error Starting Service. " + ex.Message;
                string title = "Start Service";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.OK)
                {
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stopServiceMenuItem_Click(object sender, EventArgs e)
        {
            string currentDir = Directory.GetCurrentDirectory();
            DirectoryInfo dir = new DirectoryInfo(currentDir);
            string dirName = dir.Name;
            Log2.Debug("Current Dir Name = " + dirName);
            string playerServiceName = dirName; //"CurrentForCarbon"

            try
            {
                ServiceController controller = new ServiceController();

                controller.MachineName = ".";
                controller.ServiceName = playerServiceName;

                // Stop the service
                controller.Stop();
            }
            catch (Exception ex)
            {
                string message = "Error Stopping Service. " + ex.Message;
                string title = "Stop Service";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.OK)
                {
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uninstallServiceMenuItem_Click(object sender, EventArgs e)
        {
            string filename = "installutil.exe";
            string cParam = "Upperbay.Agent.Colony.exe /u";
            var proc = System.Diagnostics.Process.Start(filename, cParam);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            string message = "A Gentle Reminder to UNINSTALL the service before Uninstalling the Application!";
            string title = "Close Window";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons);
            if (result == DialogResult.Yes)
            {
                System.Environment.Exit(1);
            }
            else
            {
                // Do something  
            }

        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonActivatePlayer_Click(object sender, EventArgs e)
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
                    buttonActivatePlayer.BackColor = Color.LightGreen;
                    buttonDeactivatePlayer.BackColor = Color.LightGreen;
                }
                else
                {
                    string message = "Player Already Activated";
                    string title = "Activate Player";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result = MessageBox.Show(message, title, buttons);
                    if (result == DialogResult.OK)
                    {
                    }
                    buttonActivatePlayer.BackColor = Color.LightGreen;
                    buttonDeactivatePlayer.BackColor = Color.LightGreen;
                }

            }
            catch (Exception ex)
            {
                string message = "Error Activating Player. " + ex.Message;
                string title = "Activate Player";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.OK)
                {
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStartPlayer_Click(object sender, EventArgs e)
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
                    string message = "Please Activate Player";
                    string title = "Start Player";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result = MessageBox.Show(message, title, buttons);
                    if (result == DialogResult.OK)
                    {
                    }

                    buttonActivatePlayer.BackColor = Color.Aqua;
                    buttonDeactivatePlayer.BackColor = Color.Aqua;
                    buttonStartPlayer.BackColor = Color.Aqua;
                    buttonStopPlayer.BackColor = Color.Aqua;
                }
                else if (controller.Status == ServiceControllerStatus.Running)
                {
                    buttonActivatePlayer.BackColor = Color.LightGreen;
                    buttonDeactivatePlayer.BackColor = Color.LightGreen;
                    buttonStartPlayer.BackColor = Color.LightGreen;
                    buttonStopPlayer.BackColor = Color.LightGreen;
                }
                else if (controller.Status == ServiceControllerStatus.Stopped)
                {
                    controller.Start();

                    buttonActivatePlayer.BackColor = Color.LightGreen;
                    buttonDeactivatePlayer.BackColor = Color.LightGreen;
                    buttonStartPlayer.BackColor = Color.LightGreen;
                    buttonStopPlayer.BackColor = Color.LightGreen;
                }
            }
            catch (Exception ex)
            {
                string message = "Error Starting Player" + ex.Message;
                string title = "Start Player";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.OK)
                {
                }
                //buttonActivatePlayer.BackColor = Color.Green;
                //buttonDeactivatePlayer.BackColor = Color.Green;
                //buttonStartPlayer.BackColor = Color.Aqua;
                //buttonStopPlayer.BackColor = Color.Aqua;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStopPlayer_Click(object sender, EventArgs e)
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
                    string message = "Please Activate Player";
                    string title = "Stop Player";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result = MessageBox.Show(message, title, buttons);
                    if (result == DialogResult.OK)
                    {
                    }

                    buttonActivatePlayer.BackColor = Color.Aqua;
                    buttonDeactivatePlayer.BackColor = Color.Aqua;
                    buttonStartPlayer.BackColor = Color.Aqua;
                    buttonStopPlayer.BackColor = Color.Aqua;
                }
                else if (controller.Status == ServiceControllerStatus.Running)
                {
                    controller.Stop();

                    buttonActivatePlayer.BackColor = Color.LightGreen;
                    buttonDeactivatePlayer.BackColor = Color.LightGreen;
                    buttonStartPlayer.BackColor = Color.Aqua;
                    buttonStopPlayer.BackColor = Color.Aqua;
                }
                else if (controller.Status == ServiceControllerStatus.Stopped)
                {

                    buttonActivatePlayer.BackColor = Color.LightGreen;
                    buttonDeactivatePlayer.BackColor = Color.LightGreen;
                    buttonStartPlayer.BackColor = Color.Aqua;
                    buttonStopPlayer.BackColor = Color.Aqua;
                }
            }
            catch (Exception ex)
            {
                string message = "Error Stopping Player. " + ex.Message;
                string title = "Stop Player";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.OK)
                {
                }
                //buttonActivatePlayer.BackColor = Color.Aqua;
                //buttonDeactivatePlayer.BackColor = Color.Aqua;
                //buttonStartPlayer.BackColor = Color.Aqua;
                //buttonStopPlayer.BackColor = Color.Aqua;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDeactivatePlayer_Click(object sender, EventArgs e)
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
                    buttonActivatePlayer.BackColor = Color.Aqua;
                    buttonDeactivatePlayer.BackColor = Color.Aqua;
                    buttonStartPlayer.BackColor = Color.Aqua;
                    buttonStopPlayer.BackColor = Color.Aqua;
                }
                else
                {
                    if(controller.Status == ServiceControllerStatus.Running)
                        controller.Stop();
                    string filename = "installutil.exe";
                    string cParam = "Upperbay.Agent.Colony.exe /u";
                    var proc = System.Diagnostics.Process.Start(filename, cParam);
                    buttonActivatePlayer.BackColor = Color.Aqua;
                    buttonDeactivatePlayer.BackColor = Color.Aqua;
                    buttonStartPlayer.BackColor = Color.Aqua;
                    buttonStopPlayer.BackColor = Color.Aqua;
                }

            }
            catch (Exception ex)
            {
                string message = "Error Deactivating Player. " + ex.Message;
                string title = "Deactivate Player";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.OK)
                {
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxEthereumAddress_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBoxEthereumAddress.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            //DialogResult result = MessageBox.Show(newText, "textBoxEthereumAddress_TextChanged", buttons);
            //textBoxEthereumAddress.Text = ConfigurationManager.AppSettings["EthereumExternalAddress"];
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\ClusterAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='EthereumExternalAddress']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            } catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBoxEthereumAddress_TextChanged", buttons);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxEthereumKey_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBoxEthereumKey.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            //DialogResult result = MessageBox.Show(newText, "textBoxEthereumAddress_TextChanged", buttons);
            //textBoxEthereumAddress.Text = ConfigurationManager.AppSettings["EthereumExternalAddress"];
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\ClusterAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='EthereumExternalPrivateKey']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBoxEthereumAddress_TextChanged", buttons);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxDataConnectString_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBoxDataConnectString.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            //DialogResult result = MessageBox.Show(newText, "textBoxEthereumAddress_TextChanged", buttons);
            //textBoxEthereumAddress.Text = ConfigurationManager.AppSettings["EthereumExternalAddress"];
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\ClusterAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='CurrentForCarbonWatts']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBoxEthereumAddress_TextChanged", buttons);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxGamePlayerName_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBoxGamePlayerName.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            //DialogResult result = MessageBox.Show(newText, "textBoxEthereumAddress_TextChanged", buttons);
            //textBoxEthereumAddress.Text = ConfigurationManager.AppSettings["EthereumExternalAddress"];
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\ClusterAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerName']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBoxGamePlayerName_TextChanged", buttons);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxGamePlayerStreet_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBoxGamePlayerStreet.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            //DialogResult result = MessageBox.Show(newText, "textBoxEthereumAddress_TextChanged", buttons);
            //textBoxEthereumAddress.Text = ConfigurationManager.AppSettings["EthereumExternalAddress"];
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\ClusterAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerStreet']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBoxGamePlayerStreet_TextChanged", buttons);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxGamePlayerCity_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBoxGamePlayerCity.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            //DialogResult result = MessageBox.Show(newText, "textBoxEthereumAddress_TextChanged", buttons);
            //textBoxEthereumAddress.Text = ConfigurationManager.AppSettings["EthereumExternalAddress"];
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\ClusterAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerCity']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBoxGamePlayerCity_TextChanged", buttons);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxGamePlayerState_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBoxGamePlayerState.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            //DialogResult result = MessageBox.Show(newText, "textBoxEthereumAddress_TextChanged", buttons);
            //textBoxEthereumAddress.Text = ConfigurationManager.AppSettings["EthereumExternalAddress"];
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\ClusterAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerState']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBoxGamePlayerState_TextChanged", buttons);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxGamePlayerZipcode_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBoxGamePlayerZipcode.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            //DialogResult result = MessageBox.Show(newText, "textBoxEthereumAddress_TextChanged", buttons);
            //textBoxEthereumAddress.Text = ConfigurationManager.AppSettings["EthereumExternalAddress"];
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\ClusterAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerZipcode']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBoxGamePlayerZipcode_TextChanged", buttons);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxGamePlayerElectricCo_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBoxGamePlayerElectricCo.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            //DialogResult result = MessageBox.Show(newText, "textBoxEthereumAddress_TextChanged", buttons);
            //textBoxEthereumAddress.Text = ConfigurationManager.AppSettings["EthereumExternalAddress"];
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\ClusterAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerElectricCo']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBoxPlayerElectricCo_TextChanged", buttons);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxGamePlayerEmail_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBoxGamePlayerEmail.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            //DialogResult result = MessageBox.Show(newText, "textBoxEthereumAddress_TextChanged", buttons);
            //textBoxEthereumAddress.Text = ConfigurationManager.AppSettings["EthereumExternalAddress"];
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\ClusterAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerEmail']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBoxGamePlayerEmail_TextChanged", buttons);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxGamePlayerPhone_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBoxGamePlayerPhone.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            //DialogResult result = MessageBox.Show(newText, "textBoxEthereumAddress_TextChanged", buttons);
            //textBoxEthereumAddress.Text = ConfigurationManager.AppSettings["EthereumExternalAddress"];
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\ClusterAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerPhone']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBoxGamePlayerPhone_TextChanged", buttons);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxPlayerIdentifier_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBoxPlayerIdentifier.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            //DialogResult result = MessageBox.Show(newText, "textBoxEthereumAddress_TextChanged", buttons);
            //textBoxEthereumAddress.Text = ConfigurationManager.AppSettings["EthereumExternalAddress"];
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\ClusterAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerID']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBoxEthereumAddress_TextChanged", buttons);
            }

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRegisterPlayer_Click(object sender, EventArgs e)
        {
            //string _ethereumNodeURL = "HTTP://127.0.0.1:8545";

            //string _ethereumExternalAddress = "0xcf982xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
            //string _ethereumExternalPrivateKey = "0x522490c9xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx381585";
            //string _dataConnectString = "channelID=1114974,fieldID=1,readKey=USIxxxxxxxxxxHBX";
            //string _playerId = "theman";

            //string _ethereumContractAddress = this.textBoxContractAddress.Text;
            //string _ethereumExternalAddress = this.textBoxEthereumAddress.Text;
            //string _ethereumExternalPrivateKey = this.textBoxEthereumKey.Text;
            //string _dataConnectString = this.textBoxDataConnectString.Text;
            //string _playerId = this.textBoxPlayerIdentifier.Text;
            //string _playerSignature = ComputeSha256Hash(_dataConnectString + _ethereumExternalAddress + _playerId);
            //Log2.Info("_playerSignature: {0}", _playerSignature);

            try
            {

                GameLedger gameLedger = new GameLedger();
                gameLedger.AddPlayer();
                //gameLedger.LogAllPlayers();

                //EtherCurrentCarbonAccess etherCurrentCarbonAccess = new EtherCurrentCarbonAccess();

                //etherCurrentCarbonAccess.ConfigureAccess(
                //                        "DaveHardin",
                //                        _ethereumNodeURL,
                //                        _ethereumContractAddress,
                //                        _ethereumExternalAddress,
                //                        _ethereumExternalPrivateKey);

                //etherCurrentCarbonAccess.AddPlayer(
                //                        _playerId,
                //                        _dataConnectString
                //                         );

                //  etherCurrentCarbonAccess.LogAllPlayers();

                //Send PlayerConfidential data to pearlygates

                string clusterName = MyAppConfig.GetParameter("ClusterName");
                string clusterVersion = MyAppConfig.GetParameter("ClusterVersion");

                string cluster = MyAppConfig.GetParameter("ClusterName");
                string mqttCloudIpAddress = MyAppConfig.GetClusterParameter(cluster,"MqttCloudSecureIpAddress");
                string mqttCloudLoginName = MyAppConfig.GetClusterParameter(cluster,"MqttCloudSecureLoginName");
                string mqttCloudPassword = MyAppConfig.GetClusterParameter(cluster,"MqttCloudSecurePassword");
                string mqttCloudPort = MyAppConfig.GetClusterParameter(cluster,"MqttCloudSecurePort");

                string playerNumber = ConfigurationManager.AppSettings["GamePlayerNumber"];

                MqttCloudSecureDriver.MqttInitializeAsyncPub(mqttCloudIpAddress,
                                                            mqttCloudLoginName,
                                                            mqttCloudPassword,
                                                            int.Parse(mqttCloudPort));

                //MqttCloudSecureDriver.MqttSubscribeAsync(TOPICS.GAME_PLAYER_CONFIDENTIAL_TOPIC);
                

                GamePlayerConfidential gamePlayerConfidential = new GamePlayerConfidential();
                gamePlayerConfidential.GamePlayerNumber = playerNumber;
                gamePlayerConfidential.GamePlayerName = textBoxGamePlayerName.Text;
                gamePlayerConfidential.GamePlayerId = textBoxPlayerIdentifier.Text;
                gamePlayerConfidential.GamePlayerStreet = textBoxGamePlayerStreet.Text;
                gamePlayerConfidential.GamePlayerCity = textBoxGamePlayerCity.Text;
                gamePlayerConfidential.GamePlayerState = textBoxGamePlayerState.Text;
                gamePlayerConfidential.GamePlayerZipcode = textBoxGamePlayerZipcode.Text;
                gamePlayerConfidential.GamePlayerEmail = textBoxGamePlayerEmail.Text;
                gamePlayerConfidential.GamePlayerPhone = textBoxGamePlayerPhone.Text;
                gamePlayerConfidential.GamePlayerElectricCo = textBoxGamePlayerElectricCo.Text;
                gamePlayerConfidential.GamePlayerClusterName = clusterName;
                gamePlayerConfidential.GamePlayerClusterVersion = clusterVersion;
                gamePlayerConfidential.EthereumExternalAddress = textBoxEthereumAddress.Text;
                gamePlayerConfidential.EthereumExternalPrivateKey = textBoxEthereumKey.Text;
                gamePlayerConfidential.DataConnectString = textBoxDataConnectString.Text;
                gamePlayerConfidential.EthereumContractAddress = textBoxContractAddress.Text;
                gamePlayerConfidential.MqttUserName = textBoxMqttUserName.Text;
                gamePlayerConfidential.MqttPassword = textBoxMqttPassword.Text;
                gamePlayerConfidential.GamePlayerTimeZone = textBoxTimezone.Text;

                JsonGamePlayerConfidential jsonGamePlayerConfidential = new JsonGamePlayerConfidential();
                string gamePlayerConfidentialJson = jsonGamePlayerConfidential.GamePlayerConfidential2Json(gamePlayerConfidential);

                Log2.Debug("MQTT GamePlayerConfidential Message Sent: {0}", gamePlayerConfidentialJson);
                MqttCloudSecureDriver.MqttPublishAsync(TOPICS.GAME_PLAYER_CONFIDENTIAL_TOPIC, gamePlayerConfidentialJson);
            }
            catch (Exception ex)
            {
                string message = "Error Registering Player" + ex.Message;
                string title = "Register Player";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.OK)
                {
                }
            }
            MessageBoxButtons buttons1 = MessageBoxButtons.OK;
            MessageBox.Show("Completed", "Register", buttons1);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxContractAddress_TextChanged(object sender, EventArgs e)
        {
            //string newText = this.textBoxContractAddress.Text;
            //MessageBoxButtons buttons = MessageBoxButtons.OK;
            ////DialogResult result = MessageBox.Show(newText, "textBoxEthereumAddress_TextChanged", buttons);
            ////textBoxEthereumAddress.Text = ConfigurationManager.AppSettings["EthereumExternalAddress"];
            //try
            //{
            //    var xmlDoc = new XmlDocument();
            //    string fileName = Path.Combine(Application.StartupPath, "config\\ClusterAgent.app.config");
            //    //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
            //    xmlDoc.Load(fileName);
            //    xmlDoc.SelectSingleNode("//appSettings/add[@key='EthereumContractAddress']").Attributes["value"].Value = newText;
            //    xmlDoc.Save(fileName);
            //    ConfigurationManager.RefreshSection("appSettings");
            //}
            //catch (Exception ex)
            //{
            //    DialogResult result1 = MessageBox.Show(ex.ToString(), "textBoxContractAddress_TextChanged", buttons);
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonGetPlayerData_Click(object sender, EventArgs e)
        {
            GameLedger gameLedger = new GameLedger();

            string maxEthereumSlices = MyAppConfig.GetParameter("MaxEthereumSlices");
            uint slices = uint.Parse(maxEthereumSlices);
            GamePlayerStatistics.InitPlayer(textBoxPlayerIdentifier.Text);
            for (uint i = 0; i < slices; i++)
            {
                //gameLedger.LogAllPlayers(i);
                gameLedger.LogMyResults(i, textBoxPlayerIdentifier.Text);
                gameLedger.LogMyCombinedEventResults(i, textBoxPlayerIdentifier.Text);
            }
            

            //gameLedger.LogAllEvents();
            //gameLedger.LogAllResults();
            //gameLedger.LogPlayer(this.textBoxPlayerIdentifier.Text);
            //gameLedger.LogEvent(xx);
            //gameLedger.LogResultsForPlayer(this.textBoxPlayerIdentifier.Text);
            //gameLedger.LogResultsForEvent(xx);
            MessageBoxButtons buttons1 = MessageBoxButtons.OK;
            MessageBox.Show("Check log directory...", "GetPlayerData", buttons1);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonGetAllData_Click(object sender, EventArgs e)
        {
            GameLedger gameLedger = new GameLedger();

            string maxEthereumSlices = MyAppConfig.GetParameter("MaxEthereumSlices");
            uint slices = uint.Parse(maxEthereumSlices);
            GameAllStatistics.InitAll();
            for (uint i = 0; i < slices; i++)
            {
                gameLedger.LogAllPlayers(i);
                gameLedger.LogAllEvents(i);
                gameLedger.LogAllResults(i);
                gameLedger.LogAllCombinedEventResults(i);
            }
            


            //gameLedger.LogPlayer(this.textBoxPlayerIdentifier.Text);
            //gameLedger.LogEvent(xx);
            //gameLedger.LogResultsForPlayer(this.textBoxPlayerIdentifier.Text);
            //gameLedger.LogResultsForEvent(xx);
            MessageBoxButtons buttons1 = MessageBoxButtons.OK;
            MessageBox.Show("Check log directory...", "GetAllData", buttons1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCreatePlayerID_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.OKCancel;
            string hash = Utilities.ComputeSha256Hash(textBoxGamePlayerName.Text + textBoxGamePlayerStreet.Text + textBoxGamePlayerCity.Text + textBoxGamePlayerState.Text + textBoxGamePlayerZipcode.Text + textBoxEthereumAddress.Text + textBoxEthereumKey.Text + textBoxDataConnectString.Text + textBoxGamePlayerEmail.Text + textBoxGamePlayerPhone.Text);

            DialogResult result1 = MessageBox.Show(hash, "GamePlayerID", buttons);
            if (result1 == DialogResult.OK)
            {
                try
                {
                    var xmlDoc = new XmlDocument();
                    string fileName = Path.Combine(Application.StartupPath, "config\\ClusterAgent.app.config");
                    //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                    xmlDoc.Load(fileName);
                    xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerID']").Attributes["value"].Value = hash;
                    xmlDoc.Save(fileName);
                    ConfigurationManager.RefreshSection("appSettings");
                    textBoxPlayerIdentifier.Text = ConfigurationManager.AppSettings["GamePlayerID"];
                }
                catch (Exception ex)
                {
                    DialogResult result2 = MessageBox.Show(ex.ToString(), "textBoxGamePlayerID_TextChanged", buttons);
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private string CurrentVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSavePlayer_Click(object sender, EventArgs e)
        {
            SavePlayerConfig();
            MessageBoxButtons buttons1 = MessageBoxButtons.OK;
            MessageBox.Show("Completed", "Save", buttons1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRestorePlayer_Click(object sender, EventArgs e)
        {
            try
            {             
                string currentDirPath = Directory.GetCurrentDirectory();
                DirectoryInfo currentDirInfo = new DirectoryInfo(currentDirPath);
                string currentDirName = currentDirInfo.Name;
                Log2.Debug("Current Dir Name = " + currentDirName);
                string baseServiceName = currentDirName; //"CurrentForCarbon"

                DirectoryInfo parentDirInfo = Directory.GetParent(".");
                string parentDir = parentDirInfo.FullName;
                Log2.Debug("Parent Dir Name = " + parentDir);

                string sourceDir = parentDir + "\\Configurations";
                if (!Directory.Exists(sourceDir))
                {
                    Log2.Error("Source directory does not exist = " + sourceDir);
                    return;
                }

                //copy all ClusterAgent.app.config files to Configurations with service name prefix

                string sourceFile = parentDir + "\\Configurations\\" + baseServiceName + "_ClusterAgent.app.config";
                Log2.Debug("sourceFile Name = " + sourceFile);

                string targetDir = parentDir + "\\" + baseServiceName;
                string targetFile = targetDir + "\\config\\ClusterAgent.app.config";
                Log2.Debug("targetFile Name = " + targetFile);

                File.Copy(sourceFile, targetFile, true);
            }
            catch (Exception ex)
            {
                Log2.Error("RESTORE EXCEPTION = " + ex.ToString());

            }
            MessageBoxButtons buttons1 = MessageBoxButtons.OK;
            MessageBox.Show("Completed", "Restore", buttons1);
            InitForm();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxGamePlayerNumber_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBoxGamePlayerNumber.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\ClusterAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerNumber']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBoxPlayerNumber_TextChanged", buttons);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void SavePlayerConfig()
        {
            try
            {
                string currentDirPath = Directory.GetCurrentDirectory();
                DirectoryInfo currentDirInfo = new DirectoryInfo(currentDirPath);
                string currentDirName = currentDirInfo.Name;
                Log2.Debug("Current Dir Name = " + currentDirName);
                string baseServiceName = currentDirName; //"CurrentForCarbon"

                DirectoryInfo parentDirInfo = Directory.GetParent(".");
                string parentDir = parentDirInfo.FullName;
                Log2.Debug("Parent Dir Name = " + parentDir);

                //copy all ClusterAgent.app.config files to Configurations with service name prefix

                string sourceDir = parentDir + "\\" + baseServiceName;
                string sourceFile = sourceDir + "\\config\\ClusterAgent.app.config";
                Log2.Debug("sourceFile Name = " + sourceFile);

                string targetDir = parentDir + "\\Configurations\\";
                if (!Directory.Exists(targetDir))
                {
                    //The below code will create a folder if the folder does not exists.            
                    DirectoryInfo folder = Directory.CreateDirectory(targetDir);
                }
                string targetFile = targetDir + baseServiceName + "_ClusterAgent.app.config";
                Log2.Debug("targetFile Name = " + targetFile);

                File.Copy(sourceFile, targetFile, true);

            }
            catch (Exception ex)
            {
                Log2.Error("SAVE EXCEPTION = " + ex.ToString());
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            SavePlayerConfig();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxMqttPassword_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBoxMqttPassword.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\ClusterAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='MqttCloudPassword']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBoxMqttPassword_TextChanged", buttons);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxMqttUserName_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBoxMqttUserName.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\ClusterAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='MqttCloudLoginName']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBoxMqttUserName_TextChanged", buttons);
            }
        }

        private void textBoxTimezone_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBoxTimezone.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\ClusterAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerTimeZone']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBoxTimezone_TextChanged", buttons);
            }

        }
    }
}
