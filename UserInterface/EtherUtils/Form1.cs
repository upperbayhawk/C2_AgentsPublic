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
using System.Data.Odbc;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Configuration;
using System.Xml;
using System.IO;


using Upperbay.Worker.EtherAccess;
using Upperbay.Core.Logging;
using Upperbay.Core.Library;
//using Upperbay.Assistant;
using Upperbay.Worker.ODBC;
using Upperbay.Worker.MQTT;
using Upperbay.Worker.JSON;
using Upperbay.Agent.Interfaces;
using System.Xml.Linq;
using System.Data.Common;


namespace EtherUtils
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

            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.File = "config\\ClusterAgent.app.config";
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            ConfigurationManager.GetSection("appSettings");

            string cluster = MyAppConfig.GetParameter("ClusterName");
            textBoxKey.Text = MyAppConfig.GetClusterParameter(cluster, "EthereumClusterKey");

        }

        private void textBoxKey_TextChanged(object sender, EventArgs e)
        {
        
        }

        private void textBoxEth_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxAddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonTransfer_Click(object sender, EventArgs e)
        {
            string key = textBoxKey.Text;
            string address = textBoxAddress.Text;
            string eth = textBoxEth.Text;
           
            Upperbay.Assistant.GameLedger gameLedger = new Upperbay.Assistant.GameLedger();
            gameLedger.SendMyEther("noname", key, address, eth);
            Log2.Info("Sent Ether From " + key + " to " + address + " :ETH = " + eth);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSendETHToPlayers_Click(object sender, EventArgs e)
        {
            string cluster = MyAppConfig.GetParameter("ClusterName");
            string mainkey = MyAppConfig.GetClusterParameter(cluster, "EthereumClusterKey");
            string mainaddress = MyAppConfig.GetClusterParameter(cluster, "EthereumClusterAddress");

            //string key = textBoxKey.Text;
            //string eth = textBoxEth.Text;

            List<GamePlayerConfidential> gpcList = new List<GamePlayerConfidential>();

            try
            {
                Upperbay.Assistant.GameLedger gameLedger = new Upperbay.Assistant.GameLedger();
                gameLedger.GetMyEther(cluster, mainkey, mainaddress);
            }
            catch (Exception ex)
            {
                Log2.Error("buttonSendETHToPlayers_Click: {0}", ex.Message);
                return;
            }

            //select players from database
            try
            {
                string queryString = "SELECT * FROM PlayerConfidential WHERE TRIM(PlayerStatus) = 'active'";
                string _connectionString = MyAppConfig.GetParameter("ODBCConnectionString");

                using (OdbcConnection connection = new OdbcConnection(_connectionString))
                {
                    OdbcCommand myCommand = new OdbcCommand(queryString, connection);
                    connection.Open();
                    OdbcDataReader myDataReader = myCommand.ExecuteReader();
                    while (myDataReader.Read())
                    {
                        //Log2.Debug("Registering from PlayerConfidential: {0},{1},{2}",
                        //                                        myDataReader.GetString(1),
                        //                                        myDataReader.GetString(2),
                        //                                        myDataReader.GetString(3));

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

                        if (mainaddress != gamePlayerConfidential.EthereumExternalAddress)
                            gpcList.Add(gamePlayerConfidential);

                    }
                    //Close all resources
                    myDataReader.Close();
                }

                Upperbay.Assistant.GameLedger gameLedger = new Upperbay.Assistant.GameLedger();
                gameLedger.TransferEtherFromConfidential(gpcList, textBoxEth.Text);


                MessageBox.Show("Completed", "buttonSendETHToPlayers_Click", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                Log2.Error("buttonSendETHToPlayers_Click: ODBC Failed: {0}", ex.Message);
            }
        
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonGetEther_Click(object sender, EventArgs e)
        {
            string cluster = MyAppConfig.GetParameter("ClusterName");
            string mainkey = MyAppConfig.GetClusterParameter(cluster,"EthereumClusterKey");
            string mainaddress = MyAppConfig.GetClusterParameter(cluster, "EthereumClusterAddress");

            try
            {
                Upperbay.Assistant.GameLedger gameLedger = new Upperbay.Assistant.GameLedger();
                gameLedger.GetMyEther(cluster, mainkey, mainaddress);
            }
            catch (Exception ex)
            {
                Log2.Error("buttonGetEther_Click: {0}", ex.Message);
                return;
            }

            //select players from database
            try
            {
                string queryString = "SELECT * FROM PlayerConfidential WHERE TRIM(PlayerStatus) = 'active'";
                string _connectionString = MyAppConfig.GetParameter("ODBCConnectionString");

                using (OdbcConnection connection = new OdbcConnection(_connectionString))
                {
                    OdbcCommand myCommand = new OdbcCommand(queryString, connection);
                    connection.Open();
                    OdbcDataReader myDataReader = myCommand.ExecuteReader();
                    while (myDataReader.Read())
                    {
                        //Log2.Debug("PlayerConfidential: {0},{1},{2}",
                        //                                        myDataReader.GetString(1),
                        //                                        myDataReader.GetString(2),
                        //                                        myDataReader.GetString(3));

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

                        string name = gamePlayerConfidential.GamePlayerName;
                        string address = gamePlayerConfidential.EthereumExternalAddress;
                        string key = gamePlayerConfidential.EthereumExternalPrivateKey;

                        Log2.Trace("Player ETH: {0},{1},{2}", name, address, key);

                        if (mainaddress != gamePlayerConfidential.EthereumExternalAddress)
                        {
                            Upperbay.Assistant.GameLedger gameLedger = new Upperbay.Assistant.GameLedger();
                            gameLedger.GetMyEther(name, mainkey, gamePlayerConfidential.EthereumExternalAddress);
                        }

                        //GameLedger gameLedger = new GameLedger();
                        //gameLedger.AddPlayerFromConfidential(gamePlayerConfidential, index);
                        //JsonGamePlayerConfidential jsonGamePlayerConfidential = new JsonGamePlayerConfidential();
                        //string gamePlayerConfidentialJson = jsonGamePlayerConfidential.GamePlayerConfidential2Json(gamePlayerConfidential);
                        //Log2.Debug("ETH GamePlayerConfidential Transaction Sent: {0}", gamePlayerConfidentialJson);
                        //index++;
                        //Sleep 10 mins
                        //Thread.Sleep(1000);
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



                //JsonGamePlayerConfidential jsonGamePlayerConfidential = new JsonGamePlayerConfidential();
                //string gamePlayerConfidentialJson = jsonGamePlayerConfidential.GamePlayerConfidential2Json(gamePlayerConfidential);
                //Log2.Debug("ETH GamePlayerConfidential Transaction Sent: {0}", gamePlayerConfidentialJson);

                MessageBox.Show("Completed", "buttonGetEther_Click", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                Log2.Error("buttonGetEther_Click: ODBC Failed: {0}", ex.Message);
            }

        }
    }
}
