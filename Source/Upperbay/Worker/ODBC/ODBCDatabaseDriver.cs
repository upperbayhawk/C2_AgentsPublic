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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;
using System.IO;
using System.Globalization;
using System.Xml.Linq;
using System.Xml;

using Upperbay.Core.Logging;
using Upperbay.Core.Library;
using Upperbay.Worker.JSON;
using Upperbay.Worker.ODBC;
using Upperbay.Worker.PostOffice;

using Upperbay.Agent.Interfaces;

namespace Upperbay.Worker.ODBC
{
    /// <summary>
    /// 
    /// </summary>
    public class ODBCDatabaseDriver
    {

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public ODBCDatabaseDriver()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public void Init()
        {
            try
            {
                _hasODBCDatabase = MyAppConfig.GetParameter("hasODBCDatabase");
                if (_hasODBCDatabase == "true")
                {
                    _connectionString = MyAppConfig.GetParameter("ODBCConnectionString");
                    _isInitialized = true;
                }
            }
            catch (Exception ex)
            {
                Log2.Error("Init: ODBC connection Failed: {0}", ex.Message);
            }
        }


        #region PLAYERCONFIDENTIAL
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gamePlayerConfidential"></param>
        public void InsertPlayerConfidential(GamePlayerConfidential gamePlayerConfidential)
        {
            if (_isInitialized)
            {
                if (gamePlayerConfidential == null)
                    return;
                               
                string playerName = gamePlayerConfidential.GamePlayerName;
                string playerID = gamePlayerConfidential.GamePlayerId;
                string playerNumber = gamePlayerConfidential.GamePlayerNumber;
                string playerStreet = gamePlayerConfidential.GamePlayerStreet;
                string playerCity = gamePlayerConfidential.GamePlayerCity;
                string playerState = gamePlayerConfidential.GamePlayerState;
                string playerZipcode = gamePlayerConfidential.GamePlayerZipcode;
                string playerEmail = gamePlayerConfidential.GamePlayerEmail;
                string playerPhone = gamePlayerConfidential.GamePlayerPhone;
                string playerElectricCo = gamePlayerConfidential.GamePlayerElectricCo;
                string playerClusterName = gamePlayerConfidential.GamePlayerClusterName;
                string playerClusterVersion = gamePlayerConfidential.GamePlayerClusterVersion;
                string playerETHAddress = gamePlayerConfidential.EthereumExternalAddress;
                string playerETHKey = gamePlayerConfidential.EthereumExternalPrivateKey;
                string playerETHContract = gamePlayerConfidential.EthereumContractAddress;
                string playerDataConnectString = gamePlayerConfidential.DataConnectString;
                string playerMqttUserName = gamePlayerConfidential.MqttUserName;
                string playerMqttPassword = gamePlayerConfidential.MqttPassword;
                string playerTimeZone = gamePlayerConfidential.GamePlayerTimeZone;

                try
                {
                    // string queryString =
                    //   "INSERT INTO Customers (CustomerID, CompanyName) Values('NWIND', 'Northwind Traders')";

                    string queryString =
             "INSERT INTO PlayerConfidential (PlayerName,PlayerID,PlayerNumber,PlayerStreet,PlayerCity,PlayerState,PlayerZipcode,PlayerEmail,PlayerPhone,PlayerElectricCo,PlayerClusterName,PlayerClusterVersion,PlayerETHAddress,PlayerETHKey,PlayerETHContract,PlayerDataConnectString,PlayerMQTTUserName,PlayerMQTTPassword,PlayerLastGame,PlayerMaxDeviation,PlayerTotalGamesPlayed,PlayerRating,PlayerStatus,PlayerTimeZone) Values";

                    queryString = queryString + "('" + playerName + "',";
                    queryString = queryString + "'" + playerID + "',";
                    queryString = queryString + "'" + playerNumber + "',";
                    queryString = queryString + "'" + playerStreet + "',";
                    queryString = queryString + "'" + playerCity + "',";
                    queryString = queryString + "'" + playerState + "',";
                    queryString = queryString + "'" + playerZipcode + "',";
                    queryString = queryString + "'" + playerEmail + "',";
                    queryString = queryString + "'" + playerPhone + "',";
                    queryString = queryString + "'" + playerElectricCo + "',";
                    queryString = queryString + "'" + playerClusterName + "',";
                    queryString = queryString + "'" + playerClusterVersion + "',";
                    queryString = queryString + "'" + playerETHAddress + "',";
                    queryString = queryString + "'" + playerETHKey + "',";
                    queryString = queryString + "'" + playerETHContract + "',";
                    queryString = queryString + "'" + playerDataConnectString + "',";
                    queryString = queryString + "'" + playerMqttUserName + "',";
                    queryString = queryString + "'" + playerMqttPassword + "',";
                    queryString = queryString + "'" + "N/A" + "',";
                    queryString = queryString + "'" + "N/A" + "',";
                    queryString = queryString + "'" + "N/A" + "',";
                    queryString = queryString + "'" + "N/A" + "',";
                    queryString = queryString + "'" + "N/A" + "',";
                    queryString = queryString + "'" + playerTimeZone + "'";
                    queryString = queryString + ");";

                    Log2.Debug("queryString {0}", queryString);
                    OdbcCommand command = new OdbcCommand(queryString);

                    using (OdbcConnection connection = new OdbcConnection(_connectionString))
                    {
                        command.Connection = connection;
                        connection.Open();
                        command.ExecuteNonQuery();

                        // The connection is automatically closed at
                        // the end of the Using block.
                    }
                }
                catch (Exception ex)
                {
                    Log2.Error("InsertPlayerConfidential Error: {0}", ex.Message);
                }


                //APPEND TO LOAD FILE FOR SAFE KEEPING

                DirectoryInfo parentDirInfo = Directory.GetParent(".");
                string parentDir = parentDirInfo.FullName;
                Log2.Debug("Parent Dir Name = " + parentDir);
                string filePath = parentDir + "\\Configurations\\GamePlayerConfidential.csv";

                using (StreamWriter outputFile = new StreamWriter(filePath, true))
                {
                    string events =
                      "\"" + playerName + "\",\"" + playerID + "\",\"" + playerNumber + "\",\"" + playerStreet + "\",\"" + playerCity + "\",\"" + playerState + "\",\"" + playerZipcode + "\",\"" + playerEmail + "\",\"" + playerPhone + "\",\"" + playerElectricCo + "\",\"" + playerClusterName + "\",\"" + playerClusterVersion + "\",\"" + playerETHAddress + "\",\"" + playerETHKey + "\",\"" + playerETHContract + "\",\"" + playerDataConnectString + "\",\"" + playerMqttUserName + "\",\"" + playerMqttPassword + "\",\"" + "N/A" + "\",\"" + "N/A" + "\",\"" + "N/A" + "\",\"" + "N/A" + "\",\"" + "N/A" + "\",\"" + playerTimeZone + "\"";
                    outputFile.WriteLine(events);
                }

                //CREATE APP.CONFIG FILE FOR SAFE KEEPING

                //copy and modify app.config template
                try
                {
                    string currentDir = Directory.GetCurrentDirectory();

                    string newFile = parentDir + "\\Configurations\\" + playerName + "_ClusterAgent.app.config";
                    string oldFile = currentDir + "\\config\\template.app.config";
                    Log2.Debug("Creating user app.config file: {0}, {1}", oldFile, newFile);
                    File.Copy(oldFile, newFile);

                    var xmlDoc = new XmlDocument();
                    xmlDoc.Load(newFile);
                    xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerNumber']").Attributes["value"].Value = playerNumber;
                    xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerName']").Attributes["value"].Value = playerName;
                    xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerID']").Attributes["value"].Value = playerID;
                    xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerStreet']").Attributes["value"].Value = playerStreet;
                    xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerCity']").Attributes["value"].Value = playerCity;
                    xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerState']").Attributes["value"].Value = playerState;
                    xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerZipcode']").Attributes["value"].Value = playerZipcode;
                    xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerEmail']").Attributes["value"].Value = playerEmail;
                    xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerPhone']").Attributes["value"].Value = playerPhone;
                    xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerElectricCo']").Attributes["value"].Value = playerElectricCo;
                    xmlDoc.SelectSingleNode("//appSettings/add[@key='EthereumExternalPrivateKey']").Attributes["value"].Value = playerETHKey;
                    xmlDoc.SelectSingleNode("//appSettings/add[@key='EthereumExternalAddress']").Attributes["value"].Value = playerETHAddress;
                    xmlDoc.SelectSingleNode("//appSettings/add[@key='CurrentForCarbonWatts']").Attributes["value"].Value = playerDataConnectString;
                    xmlDoc.SelectSingleNode("//appSettings/add[@key='MqttCloudLoginName']").Attributes["value"].Value = playerMqttUserName;
                    xmlDoc.SelectSingleNode("//appSettings/add[@key='MqttCloudPassword']").Attributes["value"].Value = playerMqttPassword;
                    xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerTimeZone']").Attributes["value"].Value = playerTimeZone;
                    xmlDoc.Save(newFile);
                }
                catch (Exception ex)
                {
                    Log2.Error("Error creating player app.config file {0}", ex.Message);
                }
            }
            else
            {
                Log2.Error("InsertPlayerConfidential ODBC connection is not initialized");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void UnloadPlayerConfidential()
        {
            if (_isInitialized)
            {
                try
                {
                    //Delete unload file if exists
                    string filePath = "C:\\Program Files\\Upperbay Systems\\Configurations\\GamePlayerConfidentialPlus.csv";
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }

                    //SELECT   orderNumber, status, orderDate, requiredDate, comments FROM     orders WHERE     status = 'Cancelled' INTO OUTFILE 'C:/tmp/cancelled_orders.csv' FIELDS ENCLOSED BY '"' TERMINATED BY ';' ESCAPED BY '"' LINES TERMINATED BY '\r\n'; 

                    string unloadString =
     "select PlayerName,PlayerID,PlayerNumber,PlayerStreet,PlayerCity,PlayerState,PlayerZipcode,PlayerEmail,PlayerPhone,PlayerElectricCo,PlayerClusterName,PlayerClusterVersion,PlayerETHAddress,PlayerETHKey,PlayerETHContract,PlayerDataConnectString,PlayerMQTTUserName,PlayerMQTTPassword,PlayerLastGame,PlayerMaxDeviation,PlayerTotalGamesPlayed,PlayerRating,PlayerStatus,PlayerTimeZone FROM CurrentForCarbon.PlayerConfidential WHERE PlayerID IS NOT NULL INTO OUTFILE \'C:/Program Files/Upperbay Systems/Configurations/GamePlayerConfidentialPlus.csv\' FIELDS ENCLOSED BY \'\"\' TERMINATED BY \',\' ESCAPED BY \'\"\' LINES TERMINATED BY \'\r\n\'";

                    OdbcCommand command = new OdbcCommand(unloadString);

                    Log2.Debug("loadString {0}", unloadString);

                    using (OdbcConnection connection = new OdbcConnection(_connectionString))
                    {
                        command.Connection = connection;
                        connection.Open();
                        command.ExecuteNonQuery();

                        // The connection is automatically closed at
                        // the end of the Using block.
                    }
                    Log2.Info("UnloadPlayerConfidential Completed");
                }
                catch (Exception ex)
                {
                    Log2.Error("UnloadPlayerConfidential: ODBC Failed: {0}", ex.Message);
                }
            }
            else
            {
                Log2.Error("UnloadPlayerConfidential ODBC connection is not initialized");
            }
        }



        /// <summary>
        /// 
        /// </summary>
        public void CreateAppConfigsFromPlayerConfidential()
        {
            if (_isInitialized)
            {
                try
                {
                    DirectoryInfo parentDirInfo = Directory.GetParent(".");
                    string parentDir = parentDirInfo.FullName;
                    Log2.Debug("Parent Dir Name = " + parentDir);
                    string filePath = parentDir + "\\Configurations\\";
                    string currentDir = Directory.GetCurrentDirectory();
                    System.IO.Directory.CreateDirectory(parentDir + "\\Configurations");
                    System.IO.Directory.CreateDirectory(parentDir + "\\Configurations\\Auto");


                    string queryString = "SELECT * FROM PlayerConfidential";

                    using (OdbcConnection connection = new OdbcConnection(_connectionString))
                    {
                        OdbcCommand myCommand = new OdbcCommand(queryString, connection);
                        connection.Open();
                        OdbcDataReader myReader = myCommand.ExecuteReader();
                        while (myReader.Read())
                        {
                            try
                            {
                                Log2.Debug("Creating AppConfig from PlayerConfidential: {0},{1},{2}",
                                                                        myReader.GetString(1),
                                                                        myReader.GetString(2),
                                                                        myReader.GetString(3));


                                string newFile = parentDir + "\\Configurations\\Auto\\" + myReader.GetString(1) + "_ClusterAgent.app.config";
                                string oldFile = currentDir + "\\config\\template.app.config";
                                Log2.Debug("Generating user app.config file: {0}, {1}", oldFile, newFile);
                                if (File.Exists(newFile))
                                {
                                    File.Delete(newFile);
                                }
                                File.Copy(oldFile, newFile);

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
                                //MqttUser 17
                                //Mqtt password 18

                                var xmlDoc = new XmlDocument();
                                xmlDoc.Load(newFile);
                                xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerNumber']").Attributes["value"].Value = myReader.GetString(3);
                                xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerName']").Attributes["value"].Value = myReader.GetString(1);
                                xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerID']").Attributes["value"].Value = myReader.GetString(2);
                                xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerStreet']").Attributes["value"].Value = myReader.GetString(4);
                                xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerCity']").Attributes["value"].Value = myReader.GetString(5);
                                xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerState']").Attributes["value"].Value = myReader.GetString(6);
                                xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerZipcode']").Attributes["value"].Value = myReader.GetString(7);
                                xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerEmail']").Attributes["value"].Value = myReader.GetString(8);
                                xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerPhone']").Attributes["value"].Value = myReader.GetString(9);
                                xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerElectricCo']").Attributes["value"].Value = myReader.GetString(10);
                                xmlDoc.SelectSingleNode("//appSettings/add[@key='EthereumExternalPrivateKey']").Attributes["value"].Value = myReader.GetString(14);
                                xmlDoc.SelectSingleNode("//appSettings/add[@key='EthereumExternalAddress']").Attributes["value"].Value = myReader.GetString(13);
                                xmlDoc.SelectSingleNode("//appSettings/add[@key='CurrentForCarbonWatts']").Attributes["value"].Value = myReader.GetString(16);
                                xmlDoc.SelectSingleNode("//appSettings/add[@key='MqttCloudLoginName']").Attributes["value"].Value = myReader.GetString(17);
                                xmlDoc.SelectSingleNode("//appSettings/add[@key='MqttCloudPassword']").Attributes["value"].Value = myReader.GetString(18);
                                xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerTimeZone']").Attributes["value"].Value = myReader.GetString(24);
                                xmlDoc.Save(newFile);
                            }
                            catch (Exception ex)
                            {
                                Log2.Error("CreateAppConfigsFromPlayerConfidential: Error creating player app.config file {0}", ex.Message);
                            }
                        }
                        //Close all resources
                        myReader.Close();
                    }
                }
                catch (Exception ex)
                {
                    Log2.Error("CreateAppConfigsFromPlayerConfidential: ODBC Failed: {0}", ex.Message);
                }
            }
            else
            {
                Log2.Error("CreateAppConfigsFromPlayerConfidential ODBC connection is not initialized");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void CreateCloneAppConfigsFromPlayerConfidential()
        {
            if (_isInitialized)
            {
                try
                {
                    DirectoryInfo parentDirInfo = Directory.GetParent(".");
                    string parentDir = parentDirInfo.FullName;
                    Log2.Debug("Parent Dir Name = " + parentDir);
                    string filePath = parentDir + "\\Configurations\\";
                    string currentDir = Directory.GetCurrentDirectory();
                    System.IO.Directory.CreateDirectory(parentDir + "\\Configurations");
                    System.IO.Directory.CreateDirectory(parentDir + "\\Configurations\\Clone");


                    string queryString = "SELECT * FROM PlayerConfidential";

                    using (OdbcConnection connection = new OdbcConnection(_connectionString))
                    {
                        OdbcCommand myCommand = new OdbcCommand(queryString, connection);
                        connection.Open();
                        OdbcDataReader myReader = myCommand.ExecuteReader();
                        while (myReader.Read())
                        {
                            try
                            {
                                Log2.Debug("Creating Clone AppConfig from PlayerConfidential: {0},{1},{2}",
                                                                        myReader.GetString(1),
                                                                        myReader.GetString(2),
                                                                        myReader.GetString(3));

                                int playerNum = int.Parse(myReader.GetString(3));
                                if (playerNum > 0)
                                {
                                    string newFile = parentDir + "\\Configurations\\Clone\\CurrentForCarbon" + myReader.GetString(3) + "_ClusterAgent.app.config";
                                    string oldFile = currentDir + "\\config\\template.app.config";
                                    Log2.Debug("Generating clone app.config file: {0}, {1}, {2}", playerNum.ToString(), oldFile, newFile);
                                    if (File.Exists(newFile))
                                    {
                                        File.Delete(newFile);
                                    }
                                    File.Copy(oldFile, newFile);

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

                                    var xmlDoc = new XmlDocument();
                                    xmlDoc.Load(newFile);
                                    xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerNumber']").Attributes["value"].Value = myReader.GetString(3);
                                    xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerName']").Attributes["value"].Value = myReader.GetString(1);
                                    xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerID']").Attributes["value"].Value = myReader.GetString(2);
                                    xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerStreet']").Attributes["value"].Value = myReader.GetString(4);
                                    xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerCity']").Attributes["value"].Value = myReader.GetString(5);
                                    xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerState']").Attributes["value"].Value = myReader.GetString(6);
                                    xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerZipcode']").Attributes["value"].Value = myReader.GetString(7);
                                    xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerEmail']").Attributes["value"].Value = myReader.GetString(8);
                                    xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerPhone']").Attributes["value"].Value = myReader.GetString(9);
                                    xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerElectricCo']").Attributes["value"].Value = myReader.GetString(10);
                                    xmlDoc.SelectSingleNode("//appSettings/add[@key='EthereumExternalPrivateKey']").Attributes["value"].Value = myReader.GetString(14);
                                    xmlDoc.SelectSingleNode("//appSettings/add[@key='EthereumExternalAddress']").Attributes["value"].Value = myReader.GetString(13);
                                    xmlDoc.SelectSingleNode("//appSettings/add[@key='CurrentForCarbonWatts']").Attributes["value"].Value = myReader.GetString(16);
                                    xmlDoc.SelectSingleNode("//appSettings/add[@key='MqttCloudLoginName']").Attributes["value"].Value = myReader.GetString(17);
                                    xmlDoc.SelectSingleNode("//appSettings/add[@key='MqttCloudPassword']").Attributes["value"].Value = myReader.GetString(18);
                                    xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerTimeZone']").Attributes["value"].Value = myReader.GetString(24);
                                    xmlDoc.Save(newFile);
                                }
                            }
                            catch (Exception ex)
                            {
                                Log2.Error("CreateCloneAppConfigsFromPlayerConfidential: Error creating player app.config file {0}", ex.Message);
                            }
                        }
                        //Close all resources
                        myReader.Close();
                    }
                }
                catch (Exception ex)
                {
                    Log2.Error("CreateCloneAppConfigsFromPlayerConfidential: ODBC Failed: {0}", ex.Message);
                }
            }
            else
            {
                Log2.Error("CreateCloneAppConfigsFromPlayerConfidential ODBC connection is not initialized");
            }
        }




        /// <summary>
        /// 
        /// </summary>
        public void QueryPlayerConfidential()
        {
            if (_isInitialized)
            {
                try
                {
                    string queryString = "SELECT * FROM PlayerConfidential";

                    using (OdbcConnection connection = new OdbcConnection(_connectionString))
                    {
                        OdbcCommand myCommand = new OdbcCommand(queryString, connection);
                        connection.Open();
                        OdbcDataReader myDataReader = myCommand.ExecuteReader();
                        while (myDataReader.Read())
                        {
                            Log2.Debug("Query PlayerConfidential: {0},{1},{2}",
                                                                    myDataReader.GetString(1),
                                                                    myDataReader.GetString(2),
                                                                    myDataReader.GetString(3));


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

                        }
                        //Close all resources
                        myDataReader.Close();
                    }
                }
                catch (Exception ex)
                {
                    Log2.Error("QueryPlayerConfidential: ODBC Failed: {0}", ex.Message);
                }
            }
            else
            {
                Log2.Error("QueryPlayerConfidential ODBC connection is not initialized");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void DeletePlayerConfidential()
        {
            if (_isInitialized)
            {
                try
                {
                    //delete from currentforcarbon.PlayerConfidential where idx is not null
                    string queryString = "DELETE FROM CURRENTFORCARBON.PlayerConfidential where idx is not null";

                    OdbcCommand command = new OdbcCommand(queryString);

                    using (OdbcConnection connection = new OdbcConnection(_connectionString))
                    {
                         command.Connection = connection;
                         connection.Open();
                         command.ExecuteNonQuery();
                    }
                    Log2.Info("DeletePlayerConfidential Completed");
                }
                catch (Exception ex)
                {
                    Log2.Error("DeletePlayerConfidential: ODBC Failed: {0}", ex.Message);
                }
            }
            else
            {
                Log2.Error("DeletePlayerConfidential ODBC connection is not initialized");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadPlayerConfidential()
        {
            if (_isInitialized)
            {
                try
                {
                    string loadString =
     "load data infile \'C:/Program Files/Upperbay Systems/Configurations/GamePlayerConfidential.csv\' into table CurrentForCarbon.PlayerConfidential FIELDS TERMINATED BY ',' ENCLOSED BY '\"'  LINES TERMINATED BY '\r\n' ( PlayerName,PlayerID,PlayerNumber,PlayerStreet,PlayerCity,PlayerState,PlayerZipcode,PlayerEmail,PlayerPhone,PlayerElectricCo,PlayerClusterName,PlayerClusterVersion,PlayerETHAddress,PlayerETHKey,PlayerETHContract,PlayerDataConnectString,PlayerMQTTUserName,PlayerMQTTPassword,PlayerLastGame,PlayerMaxDeviation,PlayerTotalGamesPlayed,PlayerRating,PlayerStatus,PlayerTimeZone) SET Idx = NULL;";

                    OdbcCommand command = new OdbcCommand(loadString);

                    Log2.Debug("loadString {0}", loadString);

                    using (OdbcConnection connection = new OdbcConnection(_connectionString))
                    {
                        command.Connection = connection;
                        connection.Open();
                        command.ExecuteNonQuery();

                        // The connection is automatically closed at
                        // the end of the Using block.
                    }
                    Log2.Info("LoadPlayerConfidential Completed");
                }
                catch (Exception ex)
                {
                    Log2.Error("LoadPlayerConfidential: ODBC Failed: {0}", ex.Message);
                }
            }
            else
            {
                Log2.Error("LoadPlayerConfidential ODBC connection is not initialized");
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerID"></param>
        /// <param name="lastGameTime"></param>
        public void UpdatePlayerLastGameTime(string playerID, string lastGameTime)
        {
            if (_isInitialized)
            {
                if (playerID == null)
                    return;
                //string nowDate = DateTime.Now.ToString("MM/dd/yyyy");
                try
                {
                    string updateString =
     "UPDATE PlayerConfidential SET PlayerLastGame='" + lastGameTime + "' where PlayerID='" + playerID + "'";

                    OdbcCommand command = new OdbcCommand(updateString);
                    //"UPDATE people SET age=10", db);

                    Log2.Debug("updateString {0}", updateString);

                    using (OdbcConnection connection = new OdbcConnection(_connectionString))
                    {
                        command.Connection = connection;
                        connection.Open();
                        command.ExecuteNonQuery();

                        // The connection is automatically closed at
                        // the end of the Using block.
                    }
                }
                catch (Exception ex)
                {
                    Log2.Error("UpdatePlayerLastGameTime: ODBC connection is not initialized: {0}", ex.Message);
                }
            }
            else
            {
                Log2.Error("UpdatePlayerLastGameTime: ODBC connection is not initialized");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerID"></param>
        /// <param name="lastGameTime"></param>
        public void UpdatePlayerTotalGamesPlayed(string playerID, string totalGamesPlayed)
        {
            if (_isInitialized)
            {
                if (playerID == null)
                    return;
                //string nowDate = DateTime.Now.ToString("MM/dd/yyyy");
                try
                {
                    string updateString =
     "UPDATE PlayerConfidential SET PlayerTotalGamesPlayed='" + totalGamesPlayed + "' where PlayerID='" + playerID + "'";

                    OdbcCommand command = new OdbcCommand(updateString);
                    //"UPDATE people SET age=10", db);

                    Log2.Debug("updateString {0}", updateString);

                    using (OdbcConnection connection = new OdbcConnection(_connectionString))
                    {
                        command.Connection = connection;
                        connection.Open();
                        command.ExecuteNonQuery();

                        // The connection is automatically closed at
                        // the end of the Using block.
                    }
                }
                catch (Exception ex)
                {
                    Log2.Error("UpdatePlayerTotalGamesPlayed: ODBC connection is not initialized: {0}", ex.Message);
                }
            }
            else
            {
                Log2.Error("UpdatePlayerTotalGamesPlayed: ODBC connection is not initialized");
            }
        }

        #endregion


        #region AWARDS
        /// <summary>
        /// 
        /// </summary>
        public void UnloadPlayerYTDAwards()
        {
            if (_isInitialized)
            {
                try
                {

                    //Delete unload file if exists
                    string filePath = "C:\\Program Files\\Upperbay Systems\\CurrentForCarbon\\logs\\GamePlayerYTDAwardsPlus.csv";
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    //SELECT   orderNumber, status, orderDate, requiredDate, comments FROM     orders WHERE     status = 'Cancelled' INTO OUTFILE 'C:/tmp/cancelled_orders.csv' FIELDS ENCLOSED BY '"' TERMINATED BY ';' ESCAPED BY '"' LINES TERMINATED BY '\r\n'; 

                    string unloadString =
     "select PlayerName, PlayerPhone, PlayerEmail, DateCalculated, YTDYear, TotalWatts, TotalPoints, TotalPercentPoints, TotalAwardValue FROM CurrentForCarbon.PlayerYTDAwards WHERE PlayerName IS NOT NULL INTO OUTFILE \'C:/Program Files/Upperbay Systems/CurrentForCarbon/logs/GamePlayerYTDAwardsPlus.csv\' FIELDS ENCLOSED BY \'\"\' TERMINATED BY \',\' ESCAPED BY \'\"\' LINES TERMINATED BY \'\r\n\'";

                    OdbcCommand command = new OdbcCommand(unloadString);

                    Log2.Debug("loadString {0}", unloadString);

                    using (OdbcConnection connection = new OdbcConnection(_connectionString))
                    {
                        command.Connection = connection;
                        connection.Open();
                        command.ExecuteNonQuery();

                        // The connection is automatically closed at
                        // the end of the Using block.
                    }
                    Log2.Info("UnloadPlayerYTDAwards Completed");
                }
                catch (Exception ex)
                {
                    Log2.Error("UnloadPlayerYTDAwards: ODBC Failed: {0}", ex.Message);
                }
            }
            else
            {
                Log2.Error("UnloadPlayerYTDAwards ODBC connection is not initialized");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void UnloadGameTotalAwards()
        {
            if (_isInitialized)
            {
                try
                {

                    //SELECT   orderNumber, status, orderDate, requiredDate, comments FROM     orders WHERE     status = 'Cancelled' INTO OUTFILE 'C:/tmp/cancelled_orders.csv' FIELDS ENCLOSED BY '"' TERMINATED BY ';' ESCAPED BY '"' LINES TERMINATED BY '\r\n'; 

                    //Delete unload file if exists
                    string filePath = "C:\\Program Files\\Upperbay Systems\\CurrentForCarbon\\logs\\GameTotalAwardsPlus.csv";
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }


                    string unloadString =
     "SELECT EventName, EventType, StartTime, EndTime, Duration, DollarPerPoint ,PointsPerWatt ,PointsPerPercent, TotalPoints, TotalPercentPoints, TotalAwardValue,PointMin, PointMax,PointCount,PointMean, PointStdDev FROM CurrentForCarbon.GameTotalAwards WHERE EventName IS NOT NULL INTO OUTFILE \'C:/Program Files/Upperbay Systems/CurrentForCarbon/logs/GameTotalAwardsPlus.csv\' FIELDS ENCLOSED BY \'\"\' TERMINATED BY \',\' ESCAPED BY \'\"\' LINES TERMINATED BY \'\r\n\'";

                    OdbcCommand command = new OdbcCommand(unloadString);

                    Log2.Debug("loadString {0}", unloadString);

                    using (OdbcConnection connection = new OdbcConnection(_connectionString))
                    {
                        command.Connection = connection;
                        connection.Open();
                        command.ExecuteNonQuery();

                        // The connection is automatically closed at
                        // the end of the Using block.
                    }
                    Log2.Info("UnloadGameTotalAwards Completed");
                }
                catch (Exception ex)
                {
                    Log2.Error("UnloadGameTotalAwards: ODBC Failed: {0}", ex.Message);
                }
            }
            else
            {
                Log2.Error("UnloadGameTotalAwards ODBC connection is not initialized");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void DeleteYearToDate()
        {
            if (_isInitialized)
            {
                try
                {
                    string queryString =
                             "delete from CurrentForCarbon.YearToDateAwards where PlayerID is not null";

                    OdbcCommand command = new OdbcCommand(queryString);
                    //"UPDATE people SET age=10", db);

                    Log2.Debug("loadString {0}", queryString);

                    using (OdbcConnection connection = new OdbcConnection(_connectionString))
                    {
                        command.Connection = connection;
                        connection.Open();
                        command.ExecuteNonQuery();

                        // The connection is automatically closed at
                        // the end of the Using block.
                    }
                    Log2.Info("DeleteYearToDate Completed");
                }
                catch (Exception ex)
                {
                    Log2.Error("DeleteYearToDate: ODBC Failed: {0}", ex.Message);
                }
            }
            else
            {
                Log2.Error("DeleteYearToDate ODBC connection is not initialized");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void DeleteGameAwards()
        {
            if (_isInitialized)
            {
                try
                {
                    string queryString =
                             "delete from CurrentForCarbon.GameAwards where EventID is not null";

                    OdbcCommand command = new OdbcCommand(queryString);
                    //"UPDATE people SET age=10", db);

                    Log2.Debug("deleteString {0}", queryString);

                    using (OdbcConnection connection = new OdbcConnection(_connectionString))
                    {
                        command.Connection = connection;
                        connection.Open();
                        command.ExecuteNonQuery();

                        // The connection is automatically closed at
                        // the end of the Using block.
                    }
                    Log2.Debug("DeleteGameAwards Completed");
                }
                catch (Exception ex)
                {
                    Log2.Error("DeleteGameAwards: ODBC Failed: {0}", ex.Message);
                }
            }
            else
            {
                Log2.Error("DeleteYearToDate ODBC connection is not initialized");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void LoadYearToDate()
        {
            if (_isInitialized)
            {
                try
                {
                    string loadString =
     "load data infile \'C:/Program Files/Upperbay Systems/CurrentForCarbon/logs/PlayerYearToDate.csv\' into table CurrentForCarbon.YearToDateAwards  FIELDS TERMINATED BY ',' ENCLOSED BY '\"'  LINES TERMINATED BY '\r\n' (PlayerID,DateCalculated,YTDYear,TotalWatts,TotalPoints,TotalPercentPoints,TotalAwardValue) SET Idx = NULL;";

                    OdbcCommand command = new OdbcCommand(loadString);
                    //"UPDATE people SET age=10", db);

                    Log2.Debug("loadString {0}", loadString);

                    using (OdbcConnection connection = new OdbcConnection(_connectionString))
                    {
                        command.Connection = connection;
                        connection.Open();
                        command.ExecuteNonQuery();

                        // The connection is automatically closed at
                        // the end of the Using block.
                    }
                    Log2.Debug("LoadYearToDate Completed");
                }
                catch (Exception ex)
                {
                    Log2.Error("LoadYearToDate: ODBC Failed: {0}", ex.Message);
                }
            }
            else
            {
                Log2.Error("LoadYearToDate ODBC connection is not initialized");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void LoadGameAwards()
        {
            if (_isInitialized)
            {
                try
                {
                    string loadString =
     "load data infile \'C:/Program Files/Upperbay Systems/CurrentForCarbon/logs/AllGameAwards.csv\' into table CurrentForCarbon.GameAwards  FIELDS TERMINATED BY ',' ENCLOSED BY '\"'  LINES TERMINATED BY '\r\n' (EventID,TotalWatts,TotalPoints,TotalPercentPoints,TotalAwardValue,PointMin, PointMax,PointCount,PointMean, PointStdDev) SET Idx = NULL;";

                    OdbcCommand command = new OdbcCommand(loadString);
                    //"UPDATE people SET age=10", db);

                    Log2.Debug("loadString {0}", loadString);

                    using (OdbcConnection connection = new OdbcConnection(_connectionString))
                    {
                        command.Connection = connection;
                        connection.Open();
                        command.ExecuteNonQuery();

                        // The connection is automatically closed at
                        // the end of the Using block.
                    }
                    Log2.Info("LoadGameAwards Completed");
                }
                catch (Exception ex)
                {
                    Log2.Error("LoadGameAwards: ODBC Failed: {0}", ex.Message);
                }
            }
            else
            {
                Log2.Error("LoadGameAwards ODBC connection is not initialized");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void QueryPlayerYTDAwards()
        {
            if (_isInitialized)
            {
                try
                {
                    string queryString = "SELECT * FROM PlayerYTDAwards";

                    using (OdbcConnection connection = new OdbcConnection(_connectionString))
                    {
                        OdbcCommand myCommand = new OdbcCommand(queryString, connection);
                        connection.Open();
                        OdbcDataReader myDataReader = myCommand.ExecuteReader();
                        while (myDataReader.Read())
                        {
                            Log2.Debug("Query PlayerYTDAwards: {0},{1},{2},{3},{4},{5},{6},{7}",
                                                                    myDataReader.GetString(0),
                                                                    myDataReader.GetString(1),
                                                                    myDataReader.GetString(2),
                                                                    myDataReader.GetString(3),
                                                                    myDataReader.GetString(4),
                                                                    myDataReader.GetString(5),
                                                                    myDataReader.GetString(6),
                                                                    myDataReader.GetString(7));

                            //PlayerName, 0
                            //PlayerPhone, 1
                            //PlayerEmail, 2
                            //DateCalculated, 3
                            //YTDYear, 4
                            //TotalPoints, 5
                            //TotalPercentPoints, 6
                            //TotalAwardValue 7
                        }
                        //Close all resources
                        myDataReader.Close();
                    }
                }
                catch (Exception ex)
                {
                    Log2.Error("QueryPlayerYTDAwards: ODBC Failed: {0}", ex.Message);
                }
            }
            else
            {
                Log2.Error("QueryPlayerYTDAwards ODBC connection is not initialized");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fireTexts"></param>
        public void TextPlayerYTDAwards(bool fireTexts)
        {
            if (_isInitialized)
            {
                try
                {
                    string queryString = "SELECT * FROM PlayerYTDAwards";

                    using (OdbcConnection connection = new OdbcConnection(_connectionString))
                    {
                        OdbcCommand myCommand = new OdbcCommand(queryString, connection);
                        connection.Open();
                        OdbcDataReader myDataReader = myCommand.ExecuteReader();
                        while (myDataReader.Read())
                        {
                            Log2.Debug("Text PlayerYTDAwards: {0},{1},{2},{3},{4},{5},{6},{7}",
                                                                    myDataReader.GetString(0),
                                                                    myDataReader.GetString(1),
                                                                    myDataReader.GetString(2),
                                                                    myDataReader.GetString(3),
                                                                    myDataReader.GetString(4),
                                                                    myDataReader.GetString(5),
                                                                    myDataReader.GetString(6),
                                                                    myDataReader.GetString(7));
                            //PlayerName, 0
                            //PlayerPhone, 1
                            //PlayerEmail, 2
                            //DateCalculated, 3
                            //YTDYear, 4
                            //TotalPoints, 5
                            //TotalPercentPoints, 6
                            //TotalAwardValue 7

                            string playerName = myDataReader.GetString(0);
                            string playerPhone = myDataReader.GetString(2);
                            string dateCalculated = myDataReader.GetString(3);
                            string YTDyear = myDataReader.GetString(4);
                            string totalPoints = myDataReader.GetString(5);
                            string totalPercentPoints = myDataReader.GetString(6);
                            string totalAwardValue = myDataReader.GetString(7);

                            CultureInfo culture = new CultureInfo("en-US");
                            string decimalPoints = Convert.ToDecimal(totalPoints, culture).ToString("0.00");
                            string decimalAwardValue = Convert.ToDecimal(totalAwardValue, culture).ToString("0.00");

                            string textBody =
       "Congratulations " + playerName + "! You have earned " + decimalPoints + " points this year valued at " + decimalAwardValue + " dollars!";

                            DateTime now = DateTime.Now;
                            string thisYear = now.Year.ToString();

                            if (YTDyear == thisYear)
                            {
                                Log2.Debug(textBody);

                                if (fireTexts)
                                {
                                    Log2.Info("Sending YTD Award Texts to Players");
                                    MessageMailer messageMailer = new MessageMailer();
                                    messageMailer.SendSMSText(textBody);
                                }
                            }
                            else
                            {
                                Log2.Trace("YTD data not for this year so skip: {0}", YTDyear);
                            }
                        }
                        //Close all resources
                        myDataReader.Close();
                    }
                }
                catch (Exception ex)
                {
                    Log2.Error("TextPlayerYTDAwards: ODBC Failed: {0}", ex.Message);
                }
            }
            else
            {
                Log2.Error("TextPlayerYTDAwards ODBC connection is not initialized");
            }
        }

        #endregion

        #region METERDATA

        /// <summary>
        /// 
        /// </summary>
        public void InsertRawMeterData ()
        {
            if (_isInitialized)
            {
                //if ("xx" == null)
                //    return;

                try
                {
                    // string queryString =
                    //   "INSERT INTO Customers (CustomerID, CompanyName) Values('NWIND', 'Northwind Traders')";

                    //string playerName = gamePlayerConfidential.GamePlayerName;
                    //string playerID = gamePlayerConfidential.GamePlayerId;
                    //string playerNumber = gamePlayerConfidential.GamePlayerNumber;
                    //string playerStreet = gamePlayerConfidential.GamePlayerStreet;
                    //string playerCity = gamePlayerConfidential.GamePlayerCity;
                    //string playerState = gamePlayerConfidential.GamePlayerState;
                    //string playerZipcode = gamePlayerConfidential.GamePlayerZipcode;
                    //string playerEmail = gamePlayerConfidential.GamePlayerEmail;
                    //string playerPhone = gamePlayerConfidential.GamePlayerPhone;
                    //string playerElectricCo = gamePlayerConfidential.GamePlayerElectricCo;
                    //string playerETHAddress = gamePlayerConfidential.EthereumExternalAddress;
                    //string playerETHKey = gamePlayerConfidential.EthereumExternalPrivateKey;
                    //string playerETHContract = gamePlayerConfidential.EthereumContractAddress;
                    //string playerDataConnectString = gamePlayerConfidential.DataConnectString;


                    //string queryString =
             //"INSERT INTO PlayerConfidential (PlayerName,PlayerID,PlayerNumber,PlayerStreet,PlayerCity,PlayerState,PlayerZipcode,PlayerEmail,PlayerPhone,PlayerElectricCo,PlayerETHAddress,PlayerETHKey,PlayerETHContract,PlayerDataConnectString) Values";

             //       queryString = queryString + "('" + playerName + "',";
             //       queryString = queryString + "'" + playerID + "',";
             //       queryString = queryString + "'" + playerNumber + "',";
             //       queryString = queryString + "'" + playerStreet + "',";
             //       queryString = queryString + "'" + playerCity + "',";
             //       queryString = queryString + "'" + playerState + "',";
             //       queryString = queryString + "'" + playerZipcode + "',";
             //       queryString = queryString + "'" + playerEmail + "',";
             //       queryString = queryString + "'" + playerPhone + "',";
             //       queryString = queryString + "'" + playerElectricCo + "',";
             //       queryString = queryString + "'" + playerETHAddress + "',";
             //       queryString = queryString + "'" + playerETHKey + "',";
             //       queryString = queryString + "'" + playerETHContract + "',";
             //       queryString = queryString + "'" + playerDataConnectString + "'";
             //       queryString = queryString + ");";

             //       Log2.Info("queryString {0}", queryString);
                    OdbcCommand command = new OdbcCommand("");

                    using (OdbcConnection connection = new OdbcConnection(_connectionString))
                    {
                        command.Connection = connection;
                        connection.Open();
                        command.ExecuteNonQuery();

                        // The connection is automatically closed at
                        // the end of the Using block.
                    }
                }
                catch (Exception ex)
                {
                    Log2.Error("InsertRawMeterData ODBC Failed: {0}", ex.Message);
                }
            }
            else
            {
                Log2.Error("InsertRawMeterData ODBC connection is not initialized");
            }
        }
        #endregion


        #region EVENTRESULTS
        /// <summary>
        /// 
        /// </summary>
        public void DeleteEventResults()
        {
            if (_isInitialized)
            {
                try
                {
                    string queryString =
                             "delete from CurrentForCarbon.EventResults where PlayerID is not null";

                    OdbcCommand command = new OdbcCommand(queryString);
                    //"UPDATE people SET age=10", db);

                    Log2.Debug("queryString {0}", queryString);

                    using (OdbcConnection connection = new OdbcConnection(_connectionString))
                    {
                        command.Connection = connection;
                        connection.Open();
                        command.ExecuteNonQuery();

                        // The connection is automatically closed at
                        // the end of the Using block.
                    }
                    Log2.Info("DeleteEventResults Completed");
                }
                catch (Exception ex)
                {
                    Log2.Error("DeleteEventResults: ODBC Failed:  {0}", ex.Message);
                }
            }
            else
            {
                Log2.Error("DeleteEventResults ODBC connection is not initialized");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadEventResults(int sliceNumber)
        {
            if (_isInitialized)
            {
                string fileName = "ALL_GAME_EVENTRESULTS" + sliceNumber + ".csv";

                try
                {
                    string loadString =
     "load data infile \'C:/Program Files/Upperbay Systems/CurrentForCarbon/logs/" + fileName + "\' into table CurrentForCarbon.EventResults  FIELDS TERMINATED BY ',' ENCLOSED BY '\"'  LINES TERMINATED BY '\r\n' IGNORE 1 LINES (PlayerID,EventID,StartTime,Duration,PointsPerWatt,AveragePowerInWatts,BaselineAveragePowerInWatts,DeltaAveragePowerInWatts,WattPoints,AwardValue,EventEnergyInKWHR,BaselineEnergyInKWHR,Slice) SET Idx = NULL;";

                    OdbcCommand command = new OdbcCommand(loadString);
                    //"UPDATE people SET age=10", db);

                    Log2.Debug("loadString {0}", loadString);

                    using (OdbcConnection connection = new OdbcConnection(_connectionString))
                    {
                        command.Connection = connection;
                        connection.Open();
                        command.ExecuteNonQuery();

                        // The connection is automatically closed at
                        // the end of the Using block.
                    }
                    Log2.Info("LoadEventResults Completed");
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("No such file or directory"))
                    {
                        Log2.Debug("LoadEventResults File Not Found: {0}", fileName);
                    }
                    else
                        Log2.Error("LoadEventResults: ODBC Failed:  {0}", ex.Message);
                }
            }
            else
            {
                Log2.Error("LoadEventResults ODBC connection is not initialized");
            }
        }
        #endregion

        #region EVENTS

        /// <summary>
        /// 
        /// </summary>
        public void DeleteEvents()
        {
            if (_isInitialized)
            {
                try
                {
                    string queryString =
                             "delete from CurrentForCarbon.GameEvents where EventID is not null";

                    OdbcCommand command = new OdbcCommand(queryString);
                    //"UPDATE people SET age=10", db);

                    Log2.Debug("loadString {0}", queryString);

                    using (OdbcConnection connection = new OdbcConnection(_connectionString))
                    {
                        command.Connection = connection;
                        connection.Open();
                        command.ExecuteNonQuery();

                        // The connection is automatically closed at
                        // the end of the Using block.
                    }
                    Log2.Info("DeleteEvents Completed");
                }
                catch (Exception ex)
                {
                    Log2.Error("DeleteEvents: ODBC Failed: {0}", ex.Message);
                }
            }
            else
            {
                Log2.Error("DeleteEvents ODBC connection is not initialized");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadEvents(int sliceNumber)
        {
            if (_isInitialized)
            {
                string fileName = "AllGameEvents" + sliceNumber + ".csv";

                try
                {
                    string loadString =
     "load data infile \'C:/Program Files/Upperbay Systems/CurrentForCarbon/logs/" + fileName + "\' into table CurrentForCarbon.GameEvents FIELDS TERMINATED BY ',' ENCLOSED BY '\"'  LINES TERMINATED BY '\r\n' IGNORE 1 LINES ( EventID,EventName,EventType,StartTime,EndTime,Duration,DollarPerPoint,PointsPerWatt,PointsPerPercent,Slice) SET Idx = NULL;";

                    OdbcCommand command = new OdbcCommand(loadString);
                    //"UPDATE people SET age=10", db);

                    Log2.Debug("loadString {0}", loadString);

                    using (OdbcConnection connection = new OdbcConnection(_connectionString))
                    {
                        command.Connection = connection;
                        connection.Open();
                        command.ExecuteNonQuery();

                        // The connection is automatically closed at
                        // the end of the Using block.
                    }
                    Log2.Info("LoadEvents Completed");
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("No such file or directory"))
                    {
                        Log2.Debug("LoadEvents File Not Found: {0}", fileName);
                    }
                    else
                        Log2.Error("LoadEvents: ODBC Failed: {0}", ex.Message);
                }
            }
            else
            {
                Log2.Error("LoadEventResults ODBC connection is not initialized");
            }
        }

        #endregion

        #region PLAYERS
        /// <summary>
        /// 
        /// </summary>
        public void DeletePlayers()
        {
            if (_isInitialized)
            {
                try
                {
                    string queryString =
                             "delete from CurrentForCarbon.GamePlayers where PlayerID is not null";

                    OdbcCommand command = new OdbcCommand(queryString);
                    

                    Log2.Debug("queryString {0}", queryString);

                    using (OdbcConnection connection = new OdbcConnection(_connectionString))
                    {
                        command.Connection = connection;
                        connection.Open();
                        command.ExecuteNonQuery();

                        // The connection is automatically closed at
                        // the end of the Using block.
                    }
                    Log2.Info("DeletePlayers Completed");
                }
                catch (Exception ex)
                {
                    Log2.Error("DeletePlayers: ODBC Failed: {0}", ex.Message);
                }
            }
            else
            {
                Log2.Error("DeletePlayers ODBC connection is not initialized");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadPlayers(int sliceNumber)
        {
            if (_isInitialized)
            {
                string fileName = "AllGamePlayers" + sliceNumber + ".csv";

                try
                {
                    string loadString =
     "load data infile \'C:/Program Files/Upperbay Systems/CurrentForCarbon/logs/" + fileName + "\' into table CurrentForCarbon.GamePlayers FIELDS TERMINATED BY ',' ENCLOSED BY '\"'  LINES TERMINATED BY '\r\n' IGNORE 1 LINES (PlayerID,DataConnectionString,PlayerETHAddress,PlayerStatus,Slice) SET Idx = NULL;";

                    OdbcCommand command = new OdbcCommand(loadString);


                    Log2.Debug("loadString {0}", loadString);

                    using (OdbcConnection connection = new OdbcConnection(_connectionString))
                    {
                        command.Connection = connection;
                        connection.Open();
                        command.ExecuteNonQuery();

                        // The connection is automatically closed at
                        // the end of the Using block.
                    }
                    Log2.Info("LoadPlayers Completed");
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("No such file or directory"))
                    {
                        Log2.Debug("LoadPlayers File Not Found: {0}", fileName);
                    }
                    else
                        Log2.Error("LoadPlayers: ODBC Failed: {0}", ex.Message);
                }
            }
            else
            {
                Log2.Error("LoadPlayers ODBC connection is not initialized");
            }
        }
        #endregion

        #region RESULTS

        /// <summary>
        /// 
        /// </summary>
        public void DeleteResults()
        {
            if (_isInitialized)
            {
                try
                {
                    string queryString =
                             "delete from CurrentForCarbon.GameResults where PlayerID is not null";

                    OdbcCommand command = new OdbcCommand(queryString);
                    //"UPDATE people SET age=10", db);

                    Log2.Debug("loadString {0}", queryString);

                    using (OdbcConnection connection = new OdbcConnection(_connectionString))
                    {
                        command.Connection = connection;
                        connection.Open();
                        command.ExecuteNonQuery();

                        // The connection is automatically closed at
                        // the end of the Using block.
                    }
                    Log2.Info("DeleteResults Completed");
                }
                catch (Exception ex)
                {
                    Log2.Error("DeleteResults: ODBC Failed: {0}", ex.Message);
                }
            }
            else
            {
                Log2.Error("DeleteResults ODBC connection is not initialized");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void LoadResults(int sliceNumber)
        {
            string fileName = "AllGameResults" + sliceNumber + ".csv";

            if (_isInitialized)
            {
                try
                {
                    string loadString =
     "load data infile \'C:/Program Files/Upperbay Systems/CurrentForCarbon/logs/" + fileName + "\' into table CurrentForCarbon.GameResults  FIELDS TERMINATED BY ',' ENCLOSED BY '\"'  LINES TERMINATED BY '\r\n' IGNORE 1 LINES (PlayerID,EventID,AveragePowerInWatts,BaselineAveragePowerInWatts,DeltaAveragePowerInWatts,PercentPoints,WattPoints,TotalPointsAwarded,AwardValue,Slice) SET Idx = NULL;";

                    OdbcCommand command = new OdbcCommand(loadString);
                    //"UPDATE people SET age=10", db);

                    Log2.Debug("loadString {0}", loadString);

                    using (OdbcConnection connection = new OdbcConnection(_connectionString))
                    {
                        command.Connection = connection;
                        connection.Open();
                        command.ExecuteNonQuery();

                        // The connection is automatically closed at
                        // the end of the Using block.
                    }
                    Log2.Info("LoadResults Completed");
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("No such file or directory"))
                    {
                        Log2.Debug("LoadResults File Not Found: {0}", fileName);
                    }
                    else
                        Log2.Error("LoadResults: Failed: {0}", ex.Message);
                }
            }
            else
            {
                Log2.Error("LoadResults ODBC connection is not initialized");
            }
        }
        #endregion


        #endregion

        #region Private State

        string _connectionString = null;
        string _hasODBCDatabase = null;
        bool _isInitialized = false;
        #endregion


    }
}
