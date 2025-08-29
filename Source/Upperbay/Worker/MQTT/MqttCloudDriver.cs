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

using Upperbay.Core.Library;
using Upperbay.Core.Logging;
using Upperbay.Worker.JSON;
using Upperbay.Agent.ColonyMatrix;
using Upperbay.Agent.Interfaces;

using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using System.Threading;
using MQTTnet.Client.Options;

namespace Upperbay.Worker.MQTT
{
    //TODO: Generate searchable MQTT clientID
    //TODO: Enable TLS of cloudmqtt
    public class MqttCloudDriver
    {

        #region Methods
        static MqttCloudDriver()
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


            Log2.Trace("MqttCloudDriver.MqttInitializeAsync Entered");
            Log2.Debug("MqttCloudDriver {0} {1} {2} {3}", 
                                    mymqttURI, 
                                    mymqttUser, 
                                    mymqttPassword, 
                                    mymqttPort);
            Log2.Debug("MqttCloudDriver ClientId: {0}", clientId);

            // Setup and start a managed MQTT client.
            var myLivingWill = new MqttApplicationMessage()
            {
                Topic = "status",
                Payload = Encoding.UTF8.GetBytes("server/disconnect"),
                Retain = false
            };

            var options = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(60))
                .WithMaxPendingMessages(20)
                .WithClientOptions(new MqttClientOptionsBuilder()
                    .WithClientId(clientId)
                    .WithCredentials(mymqttUser, mymqttPassword)
                    .WithTcpServer(mymqttURI, mymqttPort)
                    .WithTls()
                     .WithCommunicationTimeout(TimeSpan.FromSeconds(30))
                     //.WithAuthentication()
                     .WithKeepAlivePeriod(TimeSpan.FromSeconds(60))
                     .WithRequestProblemInformation(true)
                     .WithCleanSession(true)
                     .WithWillMessage(myLivingWill)
                     .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V311)
                    .Build())
                .Build();
        
            mqttCloud = new MqttFactory().CreateManagedMqttClient();

            Log2.Trace("MqttCloudDriver.MqttFactory Called");

            mqttCloud.UseConnectedHandler (e =>
            {
               Log2.Debug("MqttCloud Connected successfully with MQTT Broker.");
            });

            mqttCloud.UseDisconnectedHandler(e =>
            {
                Log2.Debug("MqttCloud Disconnected from MQTT Broker.");
            });

            mqttCloud.UseApplicationMessageReceivedHandler(e =>
            {
                string receivedTopic = e.ApplicationMessage.Topic;
                try
                {
                    if (string.IsNullOrWhiteSpace(receivedTopic) == false)
                    {
                        if (receivedTopic.Equals(TOPICS.GAME_START_TOPIC))
                        {
                            string adminOnly = MyAppConfig.GetParameter("AdminOnly");
                            if (adminOnly == "false")
                            {

                                string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);

                                JsonGameEventVariable jev = new JsonGameEventVariable();
                                GameEventVariable ev = new GameEventVariable();

                                Log2.Debug("Cloud Topic: {0}, Message Received: {1}", receivedTopic, payload);
                                ev = jev.Json2GameEventVariable(payload);

                                if (ev.GridZone.Equals("ALL"))
                                {
                                    Log2.Trace("GAMEEVENT for GridZone {0} Queued: {1} {2}", ev.GridZone, ev.GameName, ev.GameId);
                                    GameEventVariableCache.WriteEventQueue(ev);
                                }
                                else if (ev.GridZone.Equals(_gamePlayerState))
                                {
                                    Log2.Trace("GAMEEVENT for GridZone {0} Queued: {1} {2}", ev.GridZone, ev.GameName, ev.GameId);
                                    GameEventVariableCache.WriteEventQueue(ev);
                                }
                                else if (ev.GridZone.Equals(_gamePlayerCity))
                                {
                                    Log2.Trace("GAMEEVENT for GridZone {0} Queued: {1} {2}", ev.GridZone, ev.GameName, ev.GameId);
                                    GameEventVariableCache.WriteEventQueue(ev);
                                }
                                else if (ev.GridZone.Equals(_gamePlayerZipcode))
                                {
                                    Log2.Trace("GAMEEVENT for GridZone {0} Queued: {1} {2}", ev.GridZone, ev.GameName, ev.GameId);
                                    GameEventVariableCache.WriteEventQueue(ev);
                                }
                                else if (ev.GridZone.Equals(_gamePlayerStreet))
                                {
                                    Log2.Trace("GAMEEVENT for GridZone {0} Queued: {1} {2}", ev.GridZone, ev.GameName, ev.GameId);
                                    GameEventVariableCache.WriteEventQueue(ev);
                                }
                                else if (ev.GridZone.Equals(_gamePlayerName))
                                {
                                    Log2.Trace("GAMEEVENT for GridZone {0} Queued: {1} {2}", ev.GridZone, ev.GameName, ev.GameId);
                                    GameEventVariableCache.WriteEventQueue(ev);
                                }
                                else if (ev.GridZone.Equals(_gamePlayerElectricCo))
                                {
                                    Log2.Trace("GAMEEVENT for GridZone {0} Queued: {1} {2}", ev.GridZone, ev.GameName, ev.GameId);
                                    GameEventVariableCache.WriteEventQueue(ev);
                                }
                                else
                                {
                                    Log2.Trace("GAMEEVENT GridZone {0} UNKNOWN: {1} {2}", ev.GridZone, ev.GameName, ev.GameId);
                                }

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
                    Log2.Error("MqttCloudDriver Mqtt Startup Failed!: {0}", counter.ToString());
                    break;
                }
                counter++;
                Thread.Sleep(1000);
            }

            Log2.Trace("MqttCloudDriver MqttInitializeAsync Completed");

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
            if (MqttCloudDriver.IsConnected())
            {
                lock (_writeLock)
                {
                    try
                    {
                        Log2.Trace("MqttCloudDriver.MqttPublishAsync Entered {0} {1}", topic, payload);

                        mqttCloud.PublishAsync(new MqttApplicationMessageBuilder()
                        .WithTopic(topic)
                        .WithPayload(payload)
                        .WithQualityOfServiceLevel((MQTTnet.Protocol.MqttQualityOfServiceLevel)qos)
                        .WithRetainFlag(retainFlag)
                        .WithResponseTopic(topic)
                        .Build());

                        //int count = mqttCloud.PendingApplicationMessagesCount;
                        Log2.Trace("MqttCloudDriver.MqttPublishAsync Completed");
                    }
                    catch (Exception e)
                    {
                        Log2.Error("MqttCloudDriver.MqttPublishAsync Exception: {0}", e.Message);
                    }
                }
            }
            else
            {
                Log2.Error("MqttCloudDriver.MqttPublishAsync CloudDriver Not Connected {0}", topic);
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
            if (MqttCloudDriver.IsConnected())
            {
                lock (_writeLock)
                {
                    try
                    {
                        Log2.Trace("MqttCloudDriver.MqttSubscribeAsync Entered");

                        MqttTopicFilter MqttTopic = new MqttTopicFilter();
                        MqttTopic.Topic = topic;
                        MqttTopic.QualityOfServiceLevel = (MQTTnet.Protocol.MqttQualityOfServiceLevel)qos;
                        mqttCloud.SubscribeAsync(MqttTopic);

                        Log2.Trace("MqttCloudDriver.MqttSubscribeAsync Completed {0}", topic);
                    }
                    catch (Exception e)
                    {
                        Log2.Error("MqttCloudDriver.MqttPublishAsync Exception: {0}", e.Message);
                    }
                }
            }
            else
            {
                Log2.Error("MqttCloudDriver.MqttSubscribeAsync CloudDriver Not Connected {0}", topic);
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
                        Log2.Error("MqttCloudDriver Mqtt Stop Failed!: {0}", counter.ToString());
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

        #endregion
    }

}
