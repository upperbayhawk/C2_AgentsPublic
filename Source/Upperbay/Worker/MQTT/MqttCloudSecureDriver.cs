//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
using System;
using System.Text;
using System.Configuration;
using System.IO;

using Upperbay.Core.Logging;
using Upperbay.Core.Library;
using Upperbay.Worker.JSON;
using Upperbay.Agent.ColonyMatrix;
using Upperbay.Agent.Interfaces;
using Upperbay.Worker.ODBC;


using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using System.Threading;
using MQTTnet.Client.Options;

namespace Upperbay.Worker.MQTT
{
    //TODO: Generate searchable MQTT clientID
    //TODO: Enable TLS of cloudmqtt
    public class MqttCloudSecureDriver
    {

        #region Methods
        static MqttCloudSecureDriver()
        {
        }

        /// <summary>
        /// MqttInitializeAsync
        /// </summary>
        /// <param name="mqttURI"></param>
        /// <param name="mqttUser"></param>
        /// <param name="mqttPassword"></param>
        /// <param name="mqttPort"></param>
        /// <returns></returns>
        public static bool MqttInitializeAsync(string mqttURI,
                                                string mqttUser,
                                                string mqttPassword,
                                                int mqttPort)
        {
           
            string clientId = Guid.NewGuid().ToString();
           
            string mymqttURI = mqttURI;
            string mymqttUser = mqttUser;
            string mymqttPassword = mqttPassword;
            int mymqttPort = mqttPort;

            _gamePlayerName = MyAppConfig.GetParameter("GamePlayerName");
            _gamePlayerStreet = MyAppConfig.GetParameter("GamePlayerStreet");
            _gamePlayerCity = MyAppConfig.GetParameter("GamePlayerCity");
            _gamePlayerState = MyAppConfig.GetParameter("GamePlayerState");
            _gamePlayerZipcode = MyAppConfig.GetParameter("GamePlayerZipcode");
            _gamePlayerElectricCo = MyAppConfig.GetParameter("GamePlayerElectricCo");


            Log2.Trace("MqttCloudSecureDriver.MqttInitializeAsync Entered");
            Log2.Debug("MqttCloudSecureDriver {0} {1} {2} {3}", 
                                    mymqttURI, 
                                    mymqttUser, 
                                    mymqttPassword, 
                                    mymqttPort);
            Log2.Debug("MqttCloudSecureDriver ClientId: {0}", clientId);

            // Setup and start a managed MQTT client.
            var myLivingWill = new MqttApplicationMessage()
            {
                Topic = "status",
                Payload = Encoding.UTF8.GetBytes("server/disconnect"),
                Retain = false
            };

            var options = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(30))
                .WithMaxPendingMessages(20)
                .WithClientOptions(new MqttClientOptionsBuilder()
                    .WithClientId(clientId)
                    .WithCredentials(mymqttUser, mymqttPassword)
                    .WithTcpServer(mymqttURI, mymqttPort)
                    .WithTls()
                     .WithCommunicationTimeout(TimeSpan.FromSeconds(10))
                     // .WithAuthentication()
                     .WithKeepAlivePeriod(TimeSpan.FromSeconds(20))
                     .WithRequestProblemInformation(true)
                     .WithCleanSession(true)
                     .WithWillMessage(myLivingWill)
                     .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V311)
                    .Build())
                .Build();
        
            mqttCloud = new MqttFactory().CreateManagedMqttClient();

            Log2.Trace("MqttCloudSecureDriver.MqttFactory Called");

            mqttCloud.UseConnectedHandler (e =>
            {
               Log2.Debug("MqttCloudSecureDriver Connected successfully with MQTT Broker: {0}", e.ToString());
            });

            mqttCloud.UseDisconnectedHandler(e =>
            {
                Log2.Debug("MqttCloudSecureDriver Disconnected from MQTT Broker: {0}", 
                                                         e.ToString());
            });

            mqttCloud.UseApplicationMessageReceivedHandler(e =>
            {
                string receivedTopic = e.ApplicationMessage.Topic;
                try
                {
                    if (string.IsNullOrWhiteSpace(receivedTopic) == false)
                    {
                        if (receivedTopic.Equals(TOPICS.GAME_PLAYER_CONFIDENTIAL_TOPIC))
                        {
                            string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);

                            JsonGamePlayerConfidential jpc = new JsonGamePlayerConfidential();
                            GamePlayerConfidential pc = new GamePlayerConfidential();

                            //Log2.Debug("Topic Received: {0}", receivedTopic);
                            Log2.Debug("Cloud Secure Topic: {0}, Message Received: {1}", receivedTopic, payload);
                            pc = jpc.Json2GamePlayerConfidential(payload);


                            // must edit file with start = "{ records: [" and end = ]} with no comma in last record to make it a valid json file
                            // {
                            // "records": [
                            // and
                            // ]
                            //}

                            //if (!_isFirstMessage)
                            //{
                                // this to to make it a little harder to access player confidential data
                                using (StreamWriter w = File.AppendText("logs\\GamePlayerConfidential.json"))
                                {
                                    w.WriteLine(payload + ",");
                                }
                            
                                ODBCDatabaseDriver odbcDatabaseDriver = new ODBCDatabaseDriver();
                                odbcDatabaseDriver.Init();
                                odbcDatabaseDriver.InsertPlayerConfidential(pc);
                                
                                //Remove, for testing only
                                odbcDatabaseDriver.QueryPlayerConfidential();
                            //}
                            //else if (_isFirstMessage)
                            //{
                            //    _isFirstMessage = false;
                            //}


                            //GameEventVariableCache.DumpQueue();

                            //JsonDataVariable jdv = new JsonDataVariable();
                            //DataVariable dv = new DataVariable();
                            //string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                            //Log2.Trace("Remote Topic: {0}. Message Received: {1}", receivedTopic, payload);
                            ////TODO: Send Data to TankFarm!!!!
                            ////DataHasher hasher = new DataHasher();
                            ////int hashCode = hasher.HashDataJson(payload);
                            //Int32 hash = payload.GetHashCode();
                            //Log2.Trace("Remote Payload Hash {0}", hash.ToString());
                            //dv = jdv.Json2DataVariable(payload);
                            //DataVariableCache.PutObject(dv.ExternalName, dv, (int)hash);
                            //Log2.Trace("Remote DV Updated {0} {1}", dv.ExternalName, hash);
                            //Log2.Trace("Remote Now Dumping");

                            //DataVariableCache.DumpCache();

                        }
                        else
                        {
                            string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                            Log2.Error("!!!!!!!!!!BAD DATA RECEIVED: Topic: {0}. Message Received: {1}!!!!!!!!!", receivedTopic, payload);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log2.Error(ex.Message, ex);
                }
            });

            mqttCloud.StartAsync(options);
            int counter = 0;
            while (mqttCloud.IsConnected == false)
            {
                if (counter > 20)
                {
                    Log2.Error("MqttCloudSecureDriver Mqtt Startup Failed!: {0}", counter.ToString());
                    break;
                }
                counter++;
                Thread.Sleep(1000);
            }

            Log2.Trace("MqttCloudSecureDriver MqttInitializeAsync Completed");

            return true;
        }

        /// <summary>
        /// MqttInitializeAsyncPub
        /// </summary>
        /// <param name="mqttURI"></param>
        /// <param name="mqttUser"></param>
        /// <param name="mqttPassword"></param>
        /// <param name="mqttPort"></param>
        /// <returns></returns>
        public static bool MqttInitializeAsyncPub(string mqttURI,
                                                string mqttUser,
                                                string mqttPassword,
                                                int mqttPort)
        {

            string clientId = Guid.NewGuid().ToString();

            string mymqttURI = mqttURI;
            string mymqttUser = mqttUser;
            string mymqttPassword = mqttPassword;
            int mymqttPort = mqttPort;


            Log2.Trace("MqttCloudSecureDriver.MqttInitializeAsyncPub Entered");
            Log2.Debug("MqttInitializeAsyncPub {0} {1} {2} {3}",
                                                        mymqttURI,
                                                        mymqttUser,
                                                        mymqttPassword,
                                                        mymqttPort);
            Log2.Debug("MqttInitializeAsyncPub ClientId: {0}", clientId);

            // Setup and start a managed MQTT client.
            var myLivingWill = new MqttApplicationMessage()
            {
                Topic = "status",
                Payload = Encoding.UTF8.GetBytes("server/disconnect"),
                Retain = false
            };

            var options = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(30))
                .WithMaxPendingMessages(20)
                .WithClientOptions(new MqttClientOptionsBuilder()
                    .WithClientId(clientId)
                    .WithCredentials(mymqttUser, mymqttPassword)
                    .WithTcpServer(mymqttURI, mymqttPort)
                     //.WithTls()
                     .WithCommunicationTimeout(TimeSpan.FromSeconds(10))
                     // .WithAuthentication()
                     .WithKeepAlivePeriod(TimeSpan.FromSeconds(20))
                     .WithRequestProblemInformation(true)
                     .WithCleanSession(true)
                     .WithWillMessage(myLivingWill)
                     .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V311)
                    .Build())
                .Build();

            mqttCloud = new MqttFactory().CreateManagedMqttClient();

            Log2.Trace("MqttInitializeAsyncPub.MqttFactory Called");

            mqttCloud.UseConnectedHandler(e =>
            {
                Log2.Debug("MqttInitializeAsyncPub Connected successfully with MQTT Broker: {0}", e.ToString());
            });

            mqttCloud.UseDisconnectedHandler(e =>
            {
                Log2.Debug("MqttInitializeAsyncPub Disconnected from MQTT Broker: {0}",
                                                         e.ToString());
            });

            mqttCloud.UseApplicationMessageReceivedHandler(e =>
            {
                string receivedTopic = e.ApplicationMessage.Topic;
                try
                {
                    if (string.IsNullOrWhiteSpace(receivedTopic) == false)
                    {
                        if (receivedTopic.Equals(TOPICS.GAME_PLAYER_CONFIDENTIAL_TOPIC))
                        {
                            string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);

                            JsonGamePlayerConfidential jev = new JsonGamePlayerConfidential();
                            GamePlayerConfidential ev = new GamePlayerConfidential();

                            Log2.Debug("MqttInitializeAsyncPub Topic Received: {0}", receivedTopic);
                            //Log2.Debug("Topic: {0}, Message Received: {1}", receivedTopic, payload);
                            ev = jev.Json2GamePlayerConfidential(payload);

                            //GameEventVariableCache.DumpQueue();

                            //JsonDataVariable jdv = new JsonDataVariable();
                            //DataVariable dv = new DataVariable();
                            //string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                            //Log2.Trace("Remote Topic: {0}. Message Received: {1}", receivedTopic, payload);
                            ////TODO: Send Data to TankFarm!!!!
                            ////DataHasher hasher = new DataHasher();
                            ////int hashCode = hasher.HashDataJson(payload);
                            //Int32 hash = payload.GetHashCode();
                            //Log2.Trace("Remote Payload Hash {0}", hash.ToString());
                            //dv = jdv.Json2DataVariable(payload);
                            //DataVariableCache.PutObject(dv.ExternalName, dv, (int)hash);
                            //Log2.Trace("Remote DV Updated {0} {1}", dv.ExternalName, hash);
                            //Log2.Trace("Remote Now Dumping");

                            //DataVariableCache.DumpCache();

                        }
                        else
                        {
                            string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                            Log2.Error("!!!!!!!!!!BAD DATA RECEIVED: Topic: {0}. Message Received: {1}!!!!!!!!!", receivedTopic, payload);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log2.Error(ex.Message, ex);
                }
            });

            mqttCloud.StartAsync(options);
            int counter = 0;
            while (mqttCloud.IsConnected == false)
            {
                if (counter > 20)
                {
                    Log2.Error("MqttInitializeAsyncPub Mqtt Startup Failed!: {0}", counter.ToString());
                    break;
                }
                counter++;
                Thread.Sleep(1000);
            }

            Log2.Trace("MqttCloudSecureDriver MqttInitializeAsyncPub Completed");

            return true;
        }


        /// <summary>
        /// Publish Message.
        /// </summary>
        /// <param name="topic">Topic.</param>
        /// <param name="payload">Payload.</param>
        /// <param name="retainFlag">Retain flag.</param>
        /// <param name="qos">Quality of Service.</param>
        /// <returns>Task.</returns>

        public static bool MqttPublishAsync(string topic, 
                                        string payload, 
                                        bool retainFlag = false, 
                                        int qos = 2)
        {
            if (MqttCloudSecureDriver.IsConnected())
            {
                lock (_writeLock)
                {
                    try
                    {
                        Log2.Trace("MqttCloudSecureDriver.MqttPublishAsync Entered {0} {1}", topic, payload);

                        mqttCloud.PublishAsync(new MqttApplicationMessageBuilder()
                        .WithTopic(topic)
                        .WithPayload(payload)
                        .WithQualityOfServiceLevel((MQTTnet.Protocol.MqttQualityOfServiceLevel)qos)
                        .WithRetainFlag(retainFlag)
                        .WithResponseTopic(topic)
                        .Build());

                        //int count = mqttCloud.PendingApplicationMessagesCount;
                        Log2.Trace("MqttCloudSecureDriver.MqttPublishAsync Completed");
                    }
                    catch (Exception e)
                    {
                        Log2.Error("MqttCloudSecureDriver.MqttPublishAsync Exception: {0}", e.Message);
                    }
                }
            }
            else
            {
                Log2.Error("MqttCloudSecureDriver.MqttPublishAsync CloudDriver Not Connected {0}", topic);
            }
            return true;
        }


        /// <summary>
        /// Subscribe topic.
        /// </summary>
        /// <param name="topic">Topic.</param>
        /// <param name="qos">Quality of Service.</param>
        /// <returns>Task.</returns>
        public static bool MqttSubscribeAsync(string topic, int qos = 2)
        {
            if (MqttCloudSecureDriver.IsConnected())
            {
                lock (_writeLock)
                {
                    try
                    {
                        Log2.Trace("MqttCloudSecureDriver.MqttSubscribeAsync Entered");

                        MqttTopicFilter MqttTopic = new MqttTopicFilter();
                        MqttTopic.Topic = topic;
                        MqttTopic.QualityOfServiceLevel = (MQTTnet.Protocol.MqttQualityOfServiceLevel)qos;
                        mqttCloud.SubscribeAsync(MqttTopic);

                        Log2.Trace("MqttCloudSecureDriver.MqttSubscribeAsync Completed {0}", topic);
                    }
                    catch (Exception e)
                    {
                        Log2.Error("MqttCloudSecureDriver.MqttPublishAsync Exception: {0}", e.Message);
                    }
                }
            }
            else
            {
                Log2.Error("MqttCloudSecureDriver.MqttSubscribeAsync CloudDriver Not Connected {0}", topic);
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool MqttStopAsync()
        {
            lock (_writeLock)
            {
                int counter = 0;
                mqttCloud.StopAsync();
                while (mqttCloud.IsStarted == true)
                {
                    if (counter > 20)
                    {
                        Log2.Error("MqttCloudSecureDriver Mqtt Stop Failed!: {0}", counter.ToString());
                        break;
                    }
                    counter++;
                    Thread.Sleep(1000);
                }
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool IsConnected()
        {
            lock (_writeLock)
            {
                bool bit = mqttCloud.IsConnected;
                return bit;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool IsStarted()
        {
            lock (_writeLock)
            {
                bool bit = mqttCloud.IsStarted;
                return bit;
            }
        }

        #endregion

        #region Private State
        private static IManagedMqttClient mqttCloud;
        private static object _writeLock = new object();
        private static string _gamePlayerName;
        private static string _gamePlayerStreet;
        private static string _gamePlayerState;
        private static string _gamePlayerCity;
        private static string _gamePlayerZipcode;
        private static string _gamePlayerElectricCo;
        //private static bool _isFirstMessage = true;
        #endregion

    }

}
