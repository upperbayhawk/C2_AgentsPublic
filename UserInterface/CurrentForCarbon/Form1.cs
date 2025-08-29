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
using System.Threading;
using System.Windows.Forms;
using System.Configuration;
using System.Xml;
using System.IO;
using System.Security.Cryptography;
using System.ServiceProcess;
using System.Deployment.Application;
using System.Reflection;

using Upperbay.Worker.EtherAccess;
using Upperbay.Core.Logging;
using Upperbay.Core.Library;
using Upperbay.Assistant;

namespace CurrentForCarbon
{
    public partial class Form1 : Form
    {

        /// <summary>
        /// 
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            MyAppConfig.SetMyAppConfig("ClusterAgent");
            string traceMode = MyAppConfig.GetParameter("TraceMode");
            Log2.LogInit("CurrentForCarbon", "ClusterAgent", traceMode);

            string currentDir = Directory.GetCurrentDirectory();
            DirectoryInfo dir = new DirectoryInfo(currentDir);
            string dirName = dir.Name;
            Log2.Debug("Current Dir Name = " + dirName);
            string playerServiceName = dirName; //"CurrentForCarbon"
            labelServiceName.Text = playerServiceName;
            labelCluster.Text = MyAppConfig.GetParameter("ClusterName");
            labelVersion.Text = MyAppConfig.GetParameter("ClusterVersion");

            //labelVersion.Text = CurrentVersion;

            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.File = "config\\ClusterAgent.app.config";
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            ConfigurationManager.GetSection("appSettings");

            textBoxEthereumAddress.Text = ConfigurationManager.AppSettings["EthereumExternalAddress"];
            textBoxEthereumKey.Text = ConfigurationManager.AppSettings["EthereumExternalPrivateKey"];
            textBoxDataConnectString.Text = ConfigurationManager.AppSettings["CurrentForCarbonWatts"];
            textBoxPlayerIdentifier.Text = ConfigurationManager.AppSettings["GamePlayerID"];
            textBoxGamePlayerName.Text = ConfigurationManager.AppSettings["GamePlayerName"];
            textBoxGamePlayerStreet.Text = ConfigurationManager.AppSettings["GamePlayerStreet"];
            textBoxGamePlayerCity.Text = ConfigurationManager.AppSettings["GamePlayerCity"];
            textBoxGamePlayerState.Text = ConfigurationManager.AppSettings["GamePlayerState"];
            textBoxGamePlayerElectricCo.Text = ConfigurationManager.AppSettings["GamePlayerElectricCo"];
            textBoxGamePlayerZipcode.Text = ConfigurationManager.AppSettings["GamePlayerZipcode"];
            textBoxPlayerTimeZone.Text = ConfigurationManager.AppSettings["GamePlayerTimeZone"];


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
            
        }


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
            string playerServiceName = dirName; //CurrentForCarbon"

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
            string playerServiceName = dirName; //CurrentForCarbon"

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
            string playerServiceName = dirName; //CurrentForCarbon"

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
            string playerServiceName =  dirName; //"CurrentForCarbon"

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
        //private void buttonRegisterPlayer_Click(object sender, EventArgs e)
        //{
           
        //    GameLedger gameLedger = new GameLedger();
        //    gameLedger.AddPlayer();
        //    MessageBoxButtons buttons = MessageBoxButtons.OK;
        //    MessageBox.Show("Completed", "Register", buttons);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        //static string ComputeSha256Hash(string rawData)
        //{
        //    // Create a SHA256   
        //    using (SHA256 sha256Hash = SHA256.Create())
        //    {
        //        // ComputeHash - returns byte array  
        //        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

        //        // Convert byte array to a string   
        //        StringBuilder builder = new StringBuilder();
        //        for (int i = 0; i < bytes.Length; i++)
        //        {
        //            builder.Append(bytes[i].ToString("x2"));
        //        }
        //        return builder.ToString();
        //    }
        //}

       
  
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

      

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void buttonLogStatistics_Click(object sender, EventArgs e)
        //{
        //    GamePlayerStatistics.CalcPlayerGameStatistics();

        //    string maxEthereumSlices = MyAppConfig.GetParameter("MaxEthereumSlices");
        //    uint slices = uint.Parse(maxEthereumSlices);
        //    GamePlayerStatistics.InitPlayer(textBoxPlayerIdentifier.Text);

        //    string outFile = "logs\\MyGameResults_ALL.csv";
        //    if (File.Exists(outFile))
        //        File.Delete(outFile);
        //    using (TextWriter tw = new StreamWriter(outFile, true))
        //    {
        //        for (uint i = 0; i < slices; i++)
        //        {
        //            string inFile = "logs\\MyGameResults" + i + ".csv";
        //            if (File.Exists(inFile))
        //            {
        //                using (TextReader tr = new StreamReader(inFile))
        //                {
        //                    tw.WriteLine(tr.ReadToEnd());
        //                    tr.Close();
        //                    tr.Dispose();
        //                }
        //                Log2.Debug("File Processed : " + inFile);
        //            }
        //        }
        //        tw.Close();
        //        tw.Dispose();
        //    }


        //    string outFile1 = "logs\\MY_GAME_COMBINEDEVENTRESULTS_ALL.csv";
        //    if (File.Exists(outFile1))
        //        File.Delete(outFile1);
        //    using (TextWriter tw = new StreamWriter(outFile1, true))
        //    {
        //        for (uint i = 0; i < slices; i++)
        //        {
        //            string inFile1 = "logs\\MY_GAME_COMBINEDEVENTRESULTS" + i + ".csv";
        //            if (File.Exists(inFile1))
        //            {
        //                using (TextReader tr = new StreamReader(inFile1))
        //                {
        //                    tw.WriteLine(tr.ReadToEnd());
        //                    tr.Close();
        //                    tr.Dispose();
        //                }
        //                Log2.Debug("File Processed : " + inFile1);
        //            }
        //        }
        //        tw.Close();
        //        tw.Dispose();
        //    }
        //}
    }
}

