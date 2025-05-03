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
using System.Data.Odbc;

using Upperbay.Worker.EtherAccess;
using Upperbay.Core.Logging;
using Upperbay.Core.Library;
using Upperbay.Assistant;
using Upperbay.Worker.ODBC;
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

            MyAppConfig.SetMyAppConfig("ClusterAgent");
            string traceMode = MyAppConfig.GetParameter("TraceMode");
            Log2.LogInit("GameAdmin", "ClusterAgent", traceMode);

            string currentDir = Directory.GetCurrentDirectory();
            DirectoryInfo dir = new DirectoryInfo(currentDir);
            string dirName = dir.Name;
            Log2.Debug("Current Dir Name = " + dirName);
            string playerServiceName = dirName; //"CurrentForCarbon"
            labelServiceName.Text = playerServiceName;
            labelCluster.Text = MyAppConfig.GetParameter("ClusterName");
            labelVersion.Text = MyAppConfig.GetParameter("ClusterVersion");

            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.File = "config\\ClusterAgent.app.config";
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            ConfigurationManager.GetSection("appSettings");

            textBoxTestingEnabled.Text = ConfigurationManager.AppSettings["TestingEnabled"];
            textBoxTextingEnabled.Text = ConfigurationManager.AppSettings["SMSAccountEnabled"];
            textBoxSecurePipe.Text = ConfigurationManager.AppSettings["MqttCloudSecureEnable"];
            textBoxVoiceEnabled.Text = ConfigurationManager.AppSettings["VoiceEnabled"];
            textBoxAdminOnly.Text = ConfigurationManager.AppSettings["AdminOnly"];
            textBoxTraceMode.Text = ConfigurationManager.AppSettings["TraceMode"];
            textBoxSlice.Text = "0";
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonGetAllData_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.OK;

            GameLedger gameLedger = new GameLedger();

            string maxEthereumSlices = MyAppConfig.GetParameter("MaxEthereumSlices");
            Log2.Info("maxEthereumSlices = {0}", maxEthereumSlices);
            uint slices = uint.Parse(maxEthereumSlices);
            GameAllStatistics.InitAll();
            int sleepyTime = 5000;
            for (uint i = 0; i < slices; i++)
            {
                gameLedger.LogAllPlayers(i);
                System.Threading.Thread.Sleep(sleepyTime);
                gameLedger.LogAllEvents(i);
                System.Threading.Thread.Sleep(sleepyTime);
                gameLedger.LogAllResults(i);
                System.Threading.Thread.Sleep(sleepyTime);
                gameLedger.LogAllCombinedEventResults(i);
                System.Threading.Thread.Sleep(sleepyTime);
            }


            //gameLedger.LogPlayer(this.textBoxPlayerIdentifier.Text);
            //gameLedger.LogEvent(xx);
            //gameLedger.LogResultsForPlayer(this.textBoxPlayerIdentifier.Text);
            //gameLedger.LogResultsForEvent(xx);
            MessageBox.Show("Please check log directory for completion", "Get All Data From Ethereum", buttons);

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labelGetSlice_Click(object sender, EventArgs e)
        {
            //MessageBoxButtons buttons = MessageBoxButtons.OK;
            GameLedger gameLedger = new GameLedger();
            uint iSlice = uint.Parse(textBoxSlice.Text);

            if (iSlice == 0)
                GameAllStatistics.InitAll();
            int sleepyTime = 5000;
            gameLedger.LogAllEvents(iSlice);
            //MessageBox.Show("Wait for Events completion", "Get Slice From Ethereum", buttons);
            System.Threading.Thread.Sleep(20000);
            gameLedger.LogAllPlayers(iSlice);
            System.Threading.Thread.Sleep(sleepyTime);
            //MessageBox.Show("Wait for Players completion", "Get Slice From Ethereum", buttons);
            gameLedger.LogAllResults(iSlice);
            System.Threading.Thread.Sleep(sleepyTime);
            //MessageBox.Show("Wait for Results completion", "Get Slice From Ethereum", buttons);
            gameLedger.LogAllCombinedEventResults(iSlice);
            System.Threading.Thread.Sleep(sleepyTime);
            //MessageBox.Show("Wait for CombinedEventResults completion", "Get Slice From Ethereum", buttons);

            //MessageBox.Show("Please check log directory for completion", "Get Slice From Ethereum", buttons);
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSlice0_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            GameLedger gameLedger = new GameLedger();
            GameAllStatistics.InitAll();

            int sleepyTime = 5000;
            gameLedger.LogAllEvents(0);
            System.Threading.Thread.Sleep(20000);
            gameLedger.LogAllPlayers(0);
            System.Threading.Thread.Sleep(sleepyTime);
            for (uint i = 0; i < 5; i++)
            {
                gameLedger.LogAllResults(i);
                System.Threading.Thread.Sleep(sleepyTime); 
                gameLedger.LogAllCombinedEventResults(i);
                System.Threading.Thread.Sleep(sleepyTime);
            }
            MessageBox.Show("Please check log directory for completion", "Get Slices 0:5 From Ethereum", buttons);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSlice1_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            GameLedger gameLedger = new GameLedger();

            int sleepyTime = 5000;
            for (uint i = 5; i < 10; i++)
            {
                //gameLedger.LogAllPlayers(i);
                //System.Threading.Thread.Sleep(sleepyTime);
                //gameLedger.LogAllEvents(i);
                //System.Threading.Thread.Sleep(sleepyTime);
                gameLedger.LogAllResults(i);
                System.Threading.Thread.Sleep(sleepyTime);
                gameLedger.LogAllCombinedEventResults(i);
                System.Threading.Thread.Sleep(sleepyTime);
            }
            MessageBox.Show("Please check log directory for completion", "Get Slices 5:9 From Ethereum", buttons);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSlice2_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            GameLedger gameLedger = new GameLedger();

            int sleepyTime = 5000;
            for (uint i = 10; i < 15; i++)
            {
                //gameLedger.LogAllPlayers(i);
                //System.Threading.Thread.Sleep(sleepyTime);
                //gameLedger.LogAllEvents(i);
                //System.Threading.Thread.Sleep(sleepyTime);
                gameLedger.LogAllResults(i);
                System.Threading.Thread.Sleep(sleepyTime);
                gameLedger.LogAllCombinedEventResults(i);
                System.Threading.Thread.Sleep(sleepyTime);
            }
            MessageBox.Show("Please check log directory for completion", "Get Slices 10:14 From Ethereum", buttons);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSlice3_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            GameLedger gameLedger = new GameLedger();

            int sleepyTime = 5000;
            for (uint i = 15; i < 20; i++)
            {
                //gameLedger.LogAllPlayers(i);
                //System.Threading.Thread.Sleep(sleepyTime);
                //gameLedger.LogAllEvents(i);
                //System.Threading.Thread.Sleep(sleepyTime);
                gameLedger.LogAllResults(i);
                System.Threading.Thread.Sleep(sleepyTime);
                gameLedger.LogAllCombinedEventResults(i);
                System.Threading.Thread.Sleep(sleepyTime);
            }
            MessageBox.Show("Please check log directory for completion", "Get Slices 15:19 From Ethereum", buttons);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSlice4_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            GameLedger gameLedger = new GameLedger();

            int sleepyTime = 5000;
            for (uint i = 20; i < 25; i++)
            {
                //gameLedger.LogAllPlayers(i);
                //System.Threading.Thread.Sleep(sleepyTime);
                //gameLedger.LogAllEvents(i);
                //System.Threading.Thread.Sleep(sleepyTime);
                gameLedger.LogAllResults(i);
                System.Threading.Thread.Sleep(sleepyTime);
                gameLedger.LogAllCombinedEventResults(i);
                System.Threading.Thread.Sleep(sleepyTime);
            }
            MessageBox.Show("Please check log directory for completion", "Get Slices 20:24 From Ethereum", buttons);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSlice5_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            GameLedger gameLedger = new GameLedger();

            int sleepyTime = 5000;
            for (uint i = 25; i < 30; i++)
            {
                //System.Threading.Thread.Sleep(sleepyTime);
                //gameLedger.LogAllPlayers(i);
                //System.Threading.Thread.Sleep(sleepyTime);
                //gameLedger.LogAllEvents(i);
                System.Threading.Thread.Sleep(sleepyTime);
                gameLedger.LogAllResults(i);
                System.Threading.Thread.Sleep(sleepyTime);
                gameLedger.LogAllCombinedEventResults(i);
                System.Threading.Thread.Sleep(sleepyTime);
            }
            MessageBox.Show("Please check log directory for completion", "Get Slices 25:29 From Ethereum", buttons);
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
        private void buttonLoadAllData_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.OK;

            string maxEthereumSlices = MyAppConfig.GetParameter("MaxEthereumSlices");
            Log2.Info("maxEthereumSlices = {0}", maxEthereumSlices);
            uint slices = uint.Parse(maxEthereumSlices);

            try
            {


                ODBCDatabaseDriver odbcDatabaseDriver = new ODBCDatabaseDriver();
                odbcDatabaseDriver.Init();

                odbcDatabaseDriver.DeleteEventResults();
                odbcDatabaseDriver.DeleteEvents();
                odbcDatabaseDriver.DeleteResults();
                odbcDatabaseDriver.DeletePlayers();
                odbcDatabaseDriver.DeleteYearToDate();
                odbcDatabaseDriver.DeleteGameAwards();

                for (int i = 0; i < slices; i++)
                {
                    odbcDatabaseDriver.LoadEventResults(i);
                    odbcDatabaseDriver.LoadEvents(i);
                    odbcDatabaseDriver.LoadResults(i);
                    odbcDatabaseDriver.LoadPlayers(i);
                }
                odbcDatabaseDriver.LoadYearToDate();
                odbcDatabaseDriver.LoadGameAwards();

                MessageBox.Show("Completed", "Load All SQL Data", buttons);

            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "ERROR LoadEventResults", buttons);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonPlayerConfidential_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.OK;

            try
            {
                ODBCDatabaseDriver odbcDatabaseDriver = new ODBCDatabaseDriver();
                odbcDatabaseDriver.Init();
                odbcDatabaseDriver.DeletePlayerConfidential();
                odbcDatabaseDriver.LoadPlayerConfidential();
                MessageBox.Show("Completed", "LoadPlayerConfidential", buttons);

            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "ERROR PlayerConfidential", buttons);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonQueryYDTAwards_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.OK;

            try
            {
                ODBCDatabaseDriver odbcDatabaseDriver = new ODBCDatabaseDriver();
                odbcDatabaseDriver.Init();
                odbcDatabaseDriver.QueryPlayerYTDAwards();
                MessageBox.Show("Completed", "QueryYDTAwards", buttons);
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "ERROR QueryPlayerYTDAwards", buttons);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxTestingEnabled_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBoxTestingEnabled.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            //DialogResult result = MessageBox.Show(newText, "textBoxEthereumAddress_TextChanged", buttons);
            //textBoxEthereumAddress.Text = ConfigurationManager.AppSettings["EthereumExternalAddress"];
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\ClusterAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='TestingEnabled']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBoxTestingEnabled_TextChanged", buttons);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonTextYTDAwards_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.OK;

            try
            {
                ODBCDatabaseDriver odbcDatabaseDriver = new ODBCDatabaseDriver();
                odbcDatabaseDriver.Init();
                odbcDatabaseDriver.TextPlayerYTDAwards(true);
                MessageBox.Show("Completed", "TextYTDAwards", buttons);
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "ERROR TextPlayerYTDAwards", buttons);
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxTextingEnabled_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBoxTextingEnabled.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\ClusterAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='SMSAccountEnabled']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBoxTextingEnabled_TextChanged", buttons);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxSecurePipe_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBoxSecurePipe.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\ClusterAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='MqttCloudSecureEnable']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBoxSecurePipe_TextChanged", buttons);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCreateAppConfigs_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.OK;

            try
            {
                ODBCDatabaseDriver odbcDatabaseDriver = new ODBCDatabaseDriver();
                odbcDatabaseDriver.Init();
                odbcDatabaseDriver.CreateAppConfigsFromPlayerConfidential();
                MessageBox.Show("Completed", "CreateAppConfigs", buttons);
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "ERROR buttonCreateAppConfigs_Click", buttons);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCreateCloneAppConfigs_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.OK;

            try
            {
                ODBCDatabaseDriver odbcDatabaseDriver = new ODBCDatabaseDriver();
                odbcDatabaseDriver.Init();
                odbcDatabaseDriver.CreateCloneAppConfigsFromPlayerConfidential();
                MessageBox.Show("Completed", "CreateCloneAppConfigs", buttons);
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "ERROR buttonCreateCloneAppConfigs_Click", buttons);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxVoiceEnabled_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBoxVoiceEnabled.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\ClusterAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='VoiceEnabled']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBoxVoiceEnabled_TextChanged", buttons);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDeleteLogs_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            try
            {
                string fileDir = Path.Combine(Application.StartupPath, "logs\\");

                string[] filePaths = Directory.GetFiles(fileDir);
                foreach (string filePath in filePaths)
                {
                    try
                    {
                        File.Delete(filePath);
                    }
                    catch (Exception)
                    {
                        //nop
                    }
                }
                Log2.Info("Log files deleted");
                MessageBox.Show("Completed", "DeleteLogs", buttons);
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "FileDelete", buttons);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonUnloadPlayerConfidential_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.OK;

            try
            {
                ODBCDatabaseDriver odbcDatabaseDriver = new ODBCDatabaseDriver();
                odbcDatabaseDriver.Init();
                odbcDatabaseDriver.UnloadPlayerConfidential();
                MessageBox.Show("Completed", "UnloadPlayerConfidential", buttons);
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "ERROR buttonUnloadPlayerConfidential_Click", buttons);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonUnloadPlayerYTDAwards_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.OK;

            try
            {
                ODBCDatabaseDriver odbcDatabaseDriver = new ODBCDatabaseDriver();
                odbcDatabaseDriver.Init();
                odbcDatabaseDriver.UnloadPlayerYTDAwards();
                MessageBox.Show("Completed", "UnloadPlayerYTDAwards", buttons);
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "ERROR buttonUnloadPlayerYTDAwards_Click", buttons);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonGameAwards_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.OK;

            try
            {
                ODBCDatabaseDriver odbcDatabaseDriver = new ODBCDatabaseDriver();
                odbcDatabaseDriver.Init();
                odbcDatabaseDriver.UnloadGameTotalAwards();
                MessageBox.Show("Completed", "UnloadGameTotalAwards", buttons);
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "ERROR buttonUnloadPlayerYTDAwards_Click", buttons);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonGetClusterParms_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            string cluster = MyAppConfig.GetParameter("ClusterName");
            Log2.Debug("{0}", cluster);
            string s = MyAppConfig.GetClusterParameter(cluster, "EthereumContractAddress");
            Log2.Debug("{0}", s);

            string s1 = MyAppConfig.GetClusterParameter(cluster, "EthereumChainId");
            Log2.Debug("{0}", s1);
            string s2 = MyAppConfig.GetClusterParameter("BETA2", "EthereumContractAddress");
            Log2.Debug("{0}", s2);
            string s3 = MyAppConfig.GetClusterParameter("BETA2", "EthereumChainId");
            Log2.Debug("{0}", s3);
            string s4 = s + s1 + s2 + s3;
            Log2.Debug("{0}, {1}, {2}, {3}, {4}, {5}", cluster, s, s1, s2, s3, s4);
            MessageBox.Show(s4, "GetClusterParms", buttons);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxAdminOnly_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBoxAdminOnly.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\ClusterAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='AdminOnly']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBoxAdminOnly_TextChanged", buttons);
            }
        }

        private void textBoxTraceMode_TextChanged(object sender, EventArgs e)
        {
            string newText = this.textBoxTraceMode.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            try
            {
                var xmlDoc = new XmlDocument();
                string fileName = Path.Combine(Application.StartupPath, "config\\ClusterAgent.app.config");
                //DialogResult result1 = MessageBox.Show(fileName, "filename", buttons);
                xmlDoc.Load(fileName);
                xmlDoc.SelectSingleNode("//appSettings/add[@key='TraceMode']").Attributes["value"].Value = newText;
                xmlDoc.Save(fileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                DialogResult result1 = MessageBox.Show(ex.ToString(), "textBoxTraceMode_TextChanged", buttons);
            }
        }

        private void buttonLogStatistics_Click(object sender, EventArgs e)
        {
            GameAllStatistics.CalcAllGameStatistics();
        }

        private void button1_RegisterSqlPlayers(object sender, EventArgs e)
        {
            
            string cluster = MyAppConfig.GetParameter("ClusterName");
            string mqttCloudIpAddress = MyAppConfig.GetClusterParameter(cluster, "MqttCloudSecureIpAddress");
            string mqttCloudLoginName = MyAppConfig.GetClusterParameter(cluster, "MqttCloudSecureLoginName");
            string mqttCloudPassword = MyAppConfig.GetClusterParameter(cluster, "MqttCloudSecurePassword");
            string mqttCloudPort = MyAppConfig.GetClusterParameter(cluster, "MqttCloudSecurePort");

            MqttCloudSecureDriver.MqttInitializeAsyncPub(mqttCloudIpAddress,
                                                        mqttCloudLoginName,
                                                        mqttCloudPassword,
                                                        int.Parse(mqttCloudPort));

            List<GamePlayerConfidential> gpcList = new List<GamePlayerConfidential>();

            
            //select players from database
            try
            {
                string queryString = "SELECT * FROM PlayerConfidential";
                string _connectionString = MyAppConfig.GetParameter("ODBCConnectionString");

                using (OdbcConnection connection = new OdbcConnection(_connectionString))
                {
                    OdbcCommand myCommand = new OdbcCommand(queryString, connection);
                    connection.Open();
                    OdbcDataReader myDataReader = myCommand.ExecuteReader();
                    while (myDataReader.Read())
                    {
                        Log2.Debug("Registering from PlayerConfidential: {0},{1},{2}",
                                                                myDataReader.GetString(1),
                                                                myDataReader.GetString(2),
                                                                myDataReader.GetString(3));

                        //PlayerName VARCHAR(255) NOT NULL,
                        //PlayerID VARCHAR(255) NOT NULL,
                        //playerNumber  VARCHAR(255) NOT NULL,
                        //playerStreet   VARCHAR(255) NOT NULL,
                        //playerCity  VARCHAR(255) NOT NULL,
                        //PlayerState VARCHAR(255) NOT NULL,
                        //PlayerZipcode VARCHAR(255) NOT NULL,
                        //PlayerEmail VARCHAR(255) NOT NULL,
                        //PlayerPhone VARCHAR(255) NOT NULL,
                        //PlayerElectricCo VARCHAR(255) NOT NULL,
                        //PlayerClusterName VARCHAR(255) NOT NULL,
                        //PlayerClusterVersion VARCHAR(255) NOT NULL,
                        //PlayerETHAddress VARCHAR(255) NOT NULL,
                        //PlayerETHKey VARCHAR(255) NOT NULL,
                        //PlayerETHContract VARCHAR(255) NOT NULL,
                        //PlayerDataConnectString VARCHAR(255) NOT NULL,
                        //PlayerMQTTUserName VARCHAR(255) NOT NULL,
                        //PlayerMQTTPassword VARCHAR(255) NOT NULL,
                        //PlayerLastGame VARCHAR(255) NOT NULL,
                        //PlayerMaxDeviation  VARCHAR(255) NOT NULL,
                        //PlayerTotalGamesPlayed VARCHAR(255) NOT NULL,
                        //PlayerRating VARCHAR(255) NOT NULL,
                        //PlayerStatus VARCHAR(255) NOT NULL,
                        //PlayerTimeZone VARCHAR(255) NOT NULL,

                        //string playerName = gamePlayerConfidential.GamePlayerName; = 1
                        //string playerID = gamePlayerConfidential.GamePlayerId;2
                        //string playerNumber = gamePlayerConfidential.GamePlayerNumber;3
                        //string playerStreet = gamePlayerConfidential.GamePlayerStreet;4
                        //string playerCity = gamePlayerConfidential.GamePlayerCity;5
                        //string playerState = gamePlayerConfidential.GamePlayerState;6
                        //string playerZipcode = gamePlayerConfidential.GamePlayerZipcode;7
                        //string playerEmail = gamePlayerConfidential.GamePlayerEmail;8
                        //string playerPhone = gamePlayerConfidential.GamePlayerPhone;9
                        //string playerElectricCo = gamePlayerConfidential.GamePlayerElectricCo;10
                        //clustername11
                        //clusterversion12
                        //string playerETHAddress = gamePlayerConfidential.EthereumExternalAddress;13
                        //string playerETHKey = gamePlayerConfidential.EthereumExternalPrivateKey;14
                        //string playerETHContract = gamePlayerConfidential.EthereumContractAddress;15
                        //string playerDataConnectString = gamePlayerConfidential.DataConnectString;16

                        GamePlayerConfidential gamePlayerConfidential = new GamePlayerConfidential();
                        gamePlayerConfidential.GamePlayerNumber = myDataReader.GetString(3);
                        gamePlayerConfidential.GamePlayerName = myDataReader.GetString(1);
                        gamePlayerConfidential.GamePlayerId = myDataReader.GetString(2);
                        gamePlayerConfidential.GamePlayerStreet = myDataReader.GetString(4);
                        gamePlayerConfidential.GamePlayerCity = myDataReader.GetString(5);
                        gamePlayerConfidential.GamePlayerState = myDataReader.GetString(6);
                        gamePlayerConfidential.GamePlayerZipcode = myDataReader.GetString(7);
                        gamePlayerConfidential.GamePlayerEmail = myDataReader.GetString(8);
                        gamePlayerConfidential.GamePlayerPhone = myDataReader.GetString(9);
                        gamePlayerConfidential.GamePlayerElectricCo = myDataReader.GetString(10);
                        gamePlayerConfidential.GamePlayerClusterName = myDataReader.GetString(11);
                        gamePlayerConfidential.GamePlayerClusterVersion = myDataReader.GetString(12);
                        gamePlayerConfidential.EthereumExternalAddress = myDataReader.GetString(13);
                        gamePlayerConfidential.EthereumExternalPrivateKey = myDataReader.GetString(14);
                        gamePlayerConfidential.DataConnectString = myDataReader.GetString(16);
                        gamePlayerConfidential.EthereumContractAddress = myDataReader.GetString(15);
                        gamePlayerConfidential.MqttUserName = myDataReader.GetString(17);
                        gamePlayerConfidential.MqttPassword = myDataReader.GetString(18);
                        gamePlayerConfidential.GamePlayerTimeZone = myDataReader.GetString(24);

                        gpcList.Add(gamePlayerConfidential);

                        //GameLedger gameLedger = new GameLedger();
                        //gameLedger.AddPlayerFromConfidential(gamePlayerConfidential, index);
                        //JsonGamePlayerConfidential jsonGamePlayerConfidential = new JsonGamePlayerConfidential();
                        //string gamePlayerConfidentialJson = jsonGamePlayerConfidential.GamePlayerConfidential2Json(gamePlayerConfidential);
                        //Log2.Debug("ETH GamePlayerConfidential Transaction Sent: {0}", gamePlayerConfidentialJson);
                        //index++;
                        //Sleep 10 mins
                        //Thread.Sleep(5000);
                        //string display = gamePlayerConfidential.GamePlayerName + " Registering";
                        //DialogResult dialogResult = MessageBox.Show(display, "RegisterSqlPlayers", MessageBoxButtons.OKCancel);
                        //if (dialogResult == DialogResult.Cancel)
                        //{
                        //    break;
                        //}                        
                    }
                    //Close all resources
                    myDataReader.Close();
                }

                GameLedger gameLedger = new GameLedger();
                gameLedger.AddPlayerFromConfidential(gpcList);

                //JsonGamePlayerConfidential jsonGamePlayerConfidential = new JsonGamePlayerConfidential();
                //string gamePlayerConfidentialJson = jsonGamePlayerConfidential.GamePlayerConfidential2Json(gamePlayerConfidential);
                //Log2.Debug("ETH GamePlayerConfidential Transaction Sent: {0}", gamePlayerConfidentialJson);

                MessageBox.Show("Completed", "RegisterSqlPlayers", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                Log2.Error("RegisterSqlPlayers: ODBC Failed: {0}", ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxSlice_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonGetPlayers_Click(object sender, EventArgs e)
        {
            GameLedger gameLedger = new GameLedger();
            uint iSlice = uint.Parse(textBoxSlice.Text);
            gameLedger.LogAllPlayers(iSlice);
        }

        private void buttonGetEvents_Click(object sender, EventArgs e)
        {
            GameLedger gameLedger = new GameLedger();
            uint iSlice = uint.Parse(textBoxSlice.Text);
            gameLedger.LogAllEvents(iSlice);
        }

        private void buttonGetResults_Click(object sender, EventArgs e)
        {
            GameLedger gameLedger = new GameLedger();
            uint iSlice = uint.Parse(textBoxSlice.Text);
            if (iSlice == 0)
                GameAllStatistics.InitAll();
            gameLedger.LogAllResults(iSlice);
        }

        private void buttonGetEventResults_Click(object sender, EventArgs e)
        {
            GameLedger gameLedger = new GameLedger();
            uint iSlice = uint.Parse(textBoxSlice.Text);
            gameLedger.LogAllCombinedEventResults(iSlice);
        }

        private void buttonKillContract_Click(object sender, EventArgs e)
        {

            DialogResult dialogResult = MessageBox.Show("DANGER!!!ARE YOU SURE!!!", "KILL SMART CONTRACT", MessageBoxButtons.OKCancel);

            if (dialogResult == DialogResult.OK)
            {
                GameLedger gameLedger = new GameLedger();
                gameLedger.KillContract();
            }
            else
            {
            }              

        }
    }
}


