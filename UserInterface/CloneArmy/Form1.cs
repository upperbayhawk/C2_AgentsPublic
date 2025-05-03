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
using System.Xml;
using System.IO;
using System.Security.Cryptography;
using System.ServiceProcess;
using System.Deployment.Application;
using System.Reflection;
using System.Threading;

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
            Log2.LogInit("CloneArmy", "ClusterAgent", traceMode);

            string currentDir = Directory.GetCurrentDirectory();
            DirectoryInfo currentDirInfo = new DirectoryInfo(currentDir);
            string dirName = currentDirInfo.Name;
            Log2.Debug("Current Dir Name = " + dirName);
            string playerServiceName = dirName; //"CurrentForCarbon"

            labelServiceName.Text = playerServiceName;
            labelCluster.Text = MyAppConfig.GetParameter("ClusterName");
            labelVersion.Text = MyAppConfig.GetParameter("ClusterVersion");

            buttonActivateClones.BackColor = Color.LightCoral;
            buttonDeactivateClones.BackColor = Color.LightCoral;
            buttonStartClones.BackColor = Color.LightCoral;
            buttonStopClones.BackColor = Color.LightCoral;

            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.File = "config\\ClusterAgent.app.config";
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            ConfigurationManager.GetSection("appSettings");

            textBoxStartingNumber.Text = ConfigurationManager.AppSettings["CloneArmyStart"];
            textBoxCount.Text = ConfigurationManager.AppSettings["CloneArmyCount"];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonClone_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBoxButtons buttons = MessageBoxButtons.OK;

                Int32 cloneCounter = Int32.Parse(textBoxCount.Text);
                Int32 cloneStartingNumber = Int32.Parse(textBoxStartingNumber.Text);

                string currentDirPath = Directory.GetCurrentDirectory();
                DirectoryInfo currentDirInfo = new DirectoryInfo(currentDirPath);
                string currentDirName = currentDirInfo.Name;
                Log2.Debug("Current Dir Name = " + currentDirName);
                string baseServiceName = currentDirName; //"CurrentForCarbon"

                DirectoryInfo parentDirInfo = Directory.GetParent(".");
                string parentDir = parentDirInfo.FullName;
                Log2.Debug("Parent Dir Name = " + parentDir);

                for (int i = 0; i < cloneCounter; i++)
                {
                    //copy currentforcarbon directory with number appended to directory name
                    string sourceDir = parentDir + "\\" + baseServiceName;
                    Log2.Debug("sourceDir Name = " + sourceDir);
                    Int32 dirExtension = cloneStartingNumber + i;
                    string targetDir = parentDir + "\\" + baseServiceName + dirExtension.ToString();
                    Log2.Debug("targetDir Name = " + targetDir);
                    //Just Do It
                    DirectoryCopy(sourceDir, targetDir, true);
                    //string fileDir = Path.Combine(Application.StartupPath, "logs\\");

                    try
                    {
                        string logfileDir = targetDir + "\\logs\\";
                        Log2.Debug("Deleting = " + logfileDir);
                        string[] filePaths = Directory.GetFiles(logfileDir);
                        foreach (string filePath in filePaths)
                        {
                            File.Delete(filePath);
                        }
                    }
                    catch (Exception)
                    {//nop
                    }
                    try
                    {
                        string datafileDir = targetDir + "\\data\\";
                        Log2.Debug("Deleting = " + datafileDir);
                        string[] filePaths = Directory.GetFiles(datafileDir);
                        foreach (string filePath in filePaths)
                        {
                            File.Delete(filePath);
                        }
                    }
                    catch (Exception)
                    {  //nop
                    }
                }
                MessageBox.Show("Completed", "Cloning", buttons);

            }
            catch (Exception ex)
            {
                Log2.Error("CLONE EXCEPTION = " + ex.ToString());

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            //Note:  Must stop services and deactivate services!!!!!!!!!

            //loop through directories
            //mkdir config_backup directory if not exists
            //backup config file to config_backup directory
            //delete files in working directory
            //copy files from currentfor carbon directory
            //do not touch subdirectories!
            //restore config file from config_backup directory

            try
            {
                Int32 cloneCounter = Int32.Parse(textBoxCount.Text);
                Int32 cloneStartingNumber = Int32.Parse(textBoxStartingNumber.Text);

                string currentDirPath = Directory.GetCurrentDirectory();
                DirectoryInfo currentDirInfo = new DirectoryInfo(currentDirPath);
                string currentDirName = currentDirInfo.Name;
                Log2.Debug("Current Dir Name = " + currentDirName);
                string baseServiceName = currentDirName; //"CurrentForCarbon"

                DirectoryInfo parentDirInfo = Directory.GetParent(".");
                string parentDir = parentDirInfo.FullName;
                Log2.Debug("Parent Dir Name = " + parentDir);

                for (int i = 0; i < cloneCounter; i++)
                {
                    //copy currentforcarbon directory with number appended to directory name

                    Int32 dirExtension = cloneStartingNumber + i;
                    string targetDir = parentDir + "\\" + baseServiceName + dirExtension.ToString();
                    string configDir = targetDir + "\\config";
                    Log2.Debug("configDir Name = " + configDir);
                    string backupDir = targetDir + "\\config_backup";
                    Log2.Debug("backupDir Name = " + backupDir);

                    //////////////////////////////////////////////     
                    //Phase 1
                    //////////////////////////////////////////////     

                    //save config files
                    Log2.Debug("Saving Config");
                    if (!Directory.Exists(backupDir))
                    {
                        //The below code will create a folder if the folder is not exists in C#.Net.            
                        DirectoryInfo folder = Directory.CreateDirectory(backupDir);
                    }
                    //copy only program files
                    Array.ForEach(Directory.GetFiles(backupDir), File.Delete);
                    DirectoryCopy(configDir, backupDir, true);

                    //////////////////////////////////////////////     
                    //Phase 2
                    //////////////////////////////////////////////

                    Log2.Debug("Copying New Program Files");
                    string sourceDir = parentDir + "\\" + baseServiceName;
                    Log2.Debug("sourceDir Name = " + sourceDir);
                    Log2.Debug("targetDir Name = " + targetDir);
                    //copy only program files
                    Array.ForEach(Directory.GetFiles(targetDir), File.Delete);
                    DirectoryCopy(sourceDir, targetDir, false);

                    //////////////////////////////////////////////
                    //Phase 3
                    //////////////////////////////////////////////     

                    //restore config files
                    Log2.Debug("Restoring Config");
                    Array.ForEach(Directory.GetFiles(configDir), File.Delete);
                    DirectoryCopy(backupDir, configDir, true);
                }
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show("Completed", "Update", buttons);
            }
            catch (Exception ex)
            {
                Log2.Error("UPDATE EXCEPTION = " + ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRemove_Click(object sender, EventArgs e)
        {
            //delete directories with number appended to directory name

            try
            {
                Int32 cloneCounter = Int32.Parse(textBoxCount.Text);
                Int32 cloneStartingNumber = Int32.Parse(textBoxStartingNumber.Text);

                string currentDirPath = Directory.GetCurrentDirectory();
                DirectoryInfo currentDirInfo = new DirectoryInfo(currentDirPath);
                string currentDirName = currentDirInfo.Name;
                Log2.Debug("Current Dir Name = " + currentDirName);
                string baseServiceName = currentDirName; //"CurrentForCarbon"

                DirectoryInfo parentDirInfo = Directory.GetParent(".");
                string parentDir = parentDirInfo.FullName;
                Log2.Debug("Parent Dir Name = " + parentDir);

                for (int i = 0; i < cloneCounter; i++)
                {
                    Int32 dirExtension = cloneStartingNumber + i;
                    string targetDir = parentDir + "\\" + baseServiceName + dirExtension.ToString();
                    Log2.Debug("Deleting Dir = " + targetDir);
                    Directory.Delete(targetDir, true);
                }
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show("Completed", "Remove", buttons);
            }
            catch (Exception ex)
            {
                Log2.Error("REMOVE EXCEPTION = " + ex.ToString());

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 cloneCounter = Int32.Parse(textBoxCount.Text);
                Int32 cloneStartingNumber = Int32.Parse(textBoxStartingNumber.Text);

                string currentDirPath = Directory.GetCurrentDirectory();
                DirectoryInfo currentDirInfo = new DirectoryInfo(currentDirPath);
                string currentDirName = currentDirInfo.Name;
                Log2.Debug("Current Dir Name = " + currentDirName);
                string baseServiceName = currentDirName; //"CurrentForCarbon"

                DirectoryInfo parentDirInfo = Directory.GetParent(".");
                string parentDir = parentDirInfo.FullName;
                Log2.Debug("Parent Dir Name = " + parentDir);

                //copy all ClusterAgent.app.config files to Configurations with service name prefix

                for (int i = 0; i < cloneCounter; i++)
                {
                    Int32 fileExtension = cloneStartingNumber + i;
                    string sourceDir = parentDir + "\\" + baseServiceName + fileExtension.ToString();
                    string sourceFile = sourceDir + "\\config\\ClusterAgent.app.config";
                    Log2.Debug("sourceFile Name = " + sourceFile);

                    string targetDir = parentDir + "\\Configurations\\";
                    if (!Directory.Exists(targetDir))
                    {
                        //The below code will create a folder if the folder does not exists.            
                        DirectoryInfo folder = Directory.CreateDirectory(targetDir);
                    }
                    string targetFile = targetDir + baseServiceName + fileExtension.ToString() + "_ClusterAgent.app.config";
                    Log2.Debug("targetFile Name = " + targetFile);

                    File.Copy(sourceFile, targetFile, true);
                }
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show("Completed", "Save", buttons);
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
        private void buttonRestore_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 cloneCounter = Int32.Parse(textBoxCount.Text);
                Int32 cloneStartingNumber = Int32.Parse(textBoxStartingNumber.Text);

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

                for (int i = 0; i < cloneCounter; i++)
                {
                    Int32 fileExtension = cloneStartingNumber + i;
                    string sourceFile = parentDir + "\\Configurations\\" + baseServiceName + fileExtension.ToString() + "_ClusterAgent.app.config";
                    Log2.Debug("sourceFile Name = " + sourceFile);

                    string targetDir = parentDir + "\\" + baseServiceName + fileExtension.ToString();
                    string targetFile = targetDir + "\\config\\ClusterAgent.app.config";
                    Log2.Debug("targetFile Name = " + targetFile);

                    File.Copy(sourceFile, targetFile, true);

                }
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show("Completed", "Restore", buttons);
            }
            catch (Exception ex)
            {
                Log2.Error("RESTORE EXCEPTION = " + ex.ToString());

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonActivateClones_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 cloneCounter = Int32.Parse(textBoxCount.Text);
                Int32 cloneStartingNumber = Int32.Parse(textBoxStartingNumber.Text);

                string currentDirPath = Directory.GetCurrentDirectory();
                DirectoryInfo currentDirInfo = new DirectoryInfo(currentDirPath);
                string currentDirName = currentDirInfo.Name;
                Log2.Debug("Current Dir Name = " + currentDirName);
                string baseServiceName = currentDirName; //"CurrentForCarbon"

                DirectoryInfo parentDirInfo = Directory.GetParent(".");
                string parentDir = parentDirInfo.FullName;
                Log2.Debug("Parent Dir Name = " + parentDir);

                for (int i = 0; i < cloneCounter; i++)
                {
                    Int32 fileExtension = cloneStartingNumber + i;
                    string playerServiceName = baseServiceName + fileExtension.ToString();
                    Directory.SetCurrentDirectory(parentDir + "\\" + playerServiceName);

                    ServiceController controller = ServiceController.GetServices()
                              .FirstOrDefault(s => s.ServiceName == playerServiceName);
                    if (controller == null)
                    {
                        string filename = "installutil.exe";
                        string cParam = "Upperbay.Agent.Colony.exe";
                        var proc = System.Diagnostics.Process.Start(filename, cParam);
                        string message = "Player Activated: " + playerServiceName;
                        Log2.Debug(message);
                    }
                    else
                    {
                        string message = "Player Already Activated: " + playerServiceName;
                        Log2.Debug(message);
                    }
                    Thread.Sleep(250);
                }
                Directory.SetCurrentDirectory(currentDirPath);
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show("Completed", "Activate", buttons);
            }
            catch (Exception ex)
            {
                Log2.Error("ActivateClones EXCEPTION = " + ex.ToString());

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStartClones_Click(object sender, EventArgs e)
        {

            try
            {
                Int32 cloneCounter = Int32.Parse(textBoxCount.Text);
                Int32 cloneStartingNumber = Int32.Parse(textBoxStartingNumber.Text);

                string currentDirPath = Directory.GetCurrentDirectory();
                DirectoryInfo currentDirInfo = new DirectoryInfo(currentDirPath);
                string currentDirName = currentDirInfo.Name;
                Log2.Debug("Current Dir Name = " + currentDirName);
                string baseServiceName = currentDirName; //"CurrentForCarbon"

                DirectoryInfo parentDirInfo = Directory.GetParent(".");
                string parentDir = parentDirInfo.FullName;
                Log2.Debug("Parent Dir Name = " + parentDir);

                for (int i = 0; i < cloneCounter; i++)
                {
                    Int32 fileExtension = cloneStartingNumber + i;
                    string playerServiceName = baseServiceName + fileExtension.ToString();
                    Directory.SetCurrentDirectory(parentDir + "\\" + playerServiceName);

                    ServiceController controller = ServiceController.GetServices()
                                .FirstOrDefault(s => s.ServiceName == playerServiceName);
                    if (controller == null)
                    {
                        string message = "Player NOT Activated: " + playerServiceName;
                        Log2.Error(message);
                    }
                    else if (controller.Status == ServiceControllerStatus.Running)
                    {
                    }
                    else if (controller.Status == ServiceControllerStatus.Stopped)
                    {
                        controller.Start();
                    }
                    Thread.Sleep(250);
                }
                Directory.SetCurrentDirectory(currentDirPath);
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show("Completed", "Start", buttons);
            }
            catch (Exception ex)
            {
                Log2.Error("StartClones EXCEPTION = " + ex.ToString());

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStopClones_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 cloneCounter = Int32.Parse(textBoxCount.Text);
                Int32 cloneStartingNumber = Int32.Parse(textBoxStartingNumber.Text);

                string currentDirPath = Directory.GetCurrentDirectory();
                DirectoryInfo currentDirInfo = new DirectoryInfo(currentDirPath);
                string currentDirName = currentDirInfo.Name;
                Log2.Debug("Current Dir Name = " + currentDirName);
                string baseServiceName = currentDirName; //"CurrentForCarbon"

                DirectoryInfo parentDirInfo = Directory.GetParent(".");
                string parentDir = parentDirInfo.FullName;
                Log2.Debug("Parent Dir Name = " + parentDir);

                for (int i = 0; i < cloneCounter; i++)
                {
                    Int32 fileExtension = cloneStartingNumber + i;
                    string playerServiceName = baseServiceName + fileExtension.ToString();
                   
                    Directory.SetCurrentDirectory(parentDir + "\\" + playerServiceName);

                    ServiceController controller = ServiceController.GetServices()
                                .FirstOrDefault(s => s.ServiceName == playerServiceName);
                    if (controller == null)
                    {
                        string message = "Player Not Activated: " + playerServiceName;
                        Log2.Error(message);
                    }
                    else if (controller.Status == ServiceControllerStatus.Running)
                    {
                        controller.Stop();
                    }
                    else if (controller.Status == ServiceControllerStatus.Stopped)
                    {
                    }
                    Thread.Sleep(250);
                }
                Directory.SetCurrentDirectory(currentDirPath);
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show("Completed", "Stop", buttons);
            }
            catch (Exception ex)
            {
                Log2.Error("StopClones EXCEPTION = " + ex.ToString());
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDeactivateClones_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 cloneCounter = Int32.Parse(textBoxCount.Text);
                Int32 cloneStartingNumber = Int32.Parse(textBoxStartingNumber.Text);

                string currentDirPath = Directory.GetCurrentDirectory();
                DirectoryInfo currentDirInfo = new DirectoryInfo(currentDirPath);
                string currentDirName = currentDirInfo.Name;
                Log2.Debug("Current Dir Name = " + currentDirName);
                string baseServiceName = currentDirName; //"CurrentForCarbon"
                
                DirectoryInfo parentDirInfo = Directory.GetParent(".");
                string parentDir = parentDirInfo.FullName;
                Log2.Debug("Parent Dir Name = " + parentDir);

                for (int i = 0; i < cloneCounter; i++)
                {
                    Int32 fileExtension = cloneStartingNumber + i;
                    string playerServiceName = baseServiceName + fileExtension.ToString();
                    Log2.Debug("playerServiceName = " + playerServiceName);

                    Directory.SetCurrentDirectory(parentDir + "\\" + playerServiceName);

                    ServiceController controller = ServiceController.GetServices()
                                .FirstOrDefault(s => s.ServiceName == playerServiceName);
                    if (controller == null)
                    {
                        // do nothing
                    }
                    else
                    {
                        string filename = "installutil.exe";
                        string cParam = "Upperbay.Agent.Colony.exe /u";
                        var proc = System.Diagnostics.Process.Start(filename, cParam);
                        string message = "Player DeActivated: " + playerServiceName;
                        Log2.Debug(message);
                    }
                    Thread.Sleep(250);
                }
                Directory.SetCurrentDirectory(currentDirPath);
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show("Completed", "Deactivate", buttons);
            }
            catch (Exception ex)
            {
                Log2.Error("DeactivateClones EXCEPTION = " + ex.ToString());
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceDirName"></param>
        /// <param name="destDirName"></param>
        /// <param name="copySubDirs"></param>
        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
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
        private void textBoxStartingNumber_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBoxStartingNumber.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\ClusterAgent.app.config");
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='CloneArmyStart']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBoxStartingNumber_TextChanged", buttons);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxCount_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBoxCount.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\ClusterAgent.app.config");
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='CloneArmyCount']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBoxCount_TextChanged", buttons);
            }
        }
    }
}
