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
using Upperbay.Core.Logging;
using Upperbay.Agent.Interfaces;
using Upperbay.Agent.ColonyMatrix;
using Upperbay.Worker.JSON;

using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using System.Threading;
using MQTTnet.Client.Options;

namespace Upperbay.Worker.MQTT
{
    public class MqttRemoteDriver
    {
        private static IManagedMqttClient mqttRemote;

        #region Methods
        static MqttRemoteDriver()
        {
        }

        /// <summary>
        /// async Task
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

            Log2.Trace("Remote MqttInitializeAsync Entered");

            // Setup and start a managed MQTT client.
            var myLivingWill = new MqttApplicationMessage()
            {
                Topic = "status",
                Payload = Encoding.UTF8.GetBytes("server/disconnect"),
                Retain = true
            };

            var options = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(480))
                .WithMaxPendingMessages(20)
                .WithClientOptions(new MqttClientOptionsBuilder()
                    .WithClientId(clientId)
                    .WithCredentials(mymqttUser, mymqttPassword)
                    .WithTcpServer(mymqttURI, mymqttPort)
                     //.WithTls()
                     .WithCommunicationTimeout(TimeSpan.FromSeconds(15))
                     // .WithAuthentication()
                     .WithKeepAlivePeriod(TimeSpan.FromSeconds(120))
                     .WithRequestProblemInformation(true)
                     .WithCleanSession(true)
                     .WithWillMessage(myLivingWill)
                     .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V311)
                    .Build())
                .Build();


            //var messageBuilder = new MqttClientOptionsBuilder()
            //        .WithClientId(clientId)
            //        .WithCredentials(mymqttUser, mymqttPassword)
            //        .WithTcpServer(mymqttURI, mymqttPort)
            //        .WithCleanSession();

            //var options = mqttSecure
            //        ? messageBuilder
            //        .WithTls()
            //        .Build()
            //        : messageBuilder
            //        .Build();

            //var managedOptions = new ManagedMqttClientOptionsBuilder()
            //            .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
            //            .WithClientOptions(options)
            //            .Build();

            mqttRemote = new MqttFactory().CreateManagedMqttClient();

            Log2.Trace("Remote MqttFactory Called");

            mqttRemote.UseConnectedHandler (e =>
            {
               Log2.Debug("Remote Connected successfully with MQTT Broker.");
            });

            mqttRemote.UseDisconnectedHandler(e =>
            {
                Log2.Error("Remote Disconnected from MQTT Broker.");
            });

            mqttRemote.UseApplicationMessageReceivedHandler(e =>
            {
                string receivedTopic = e.ApplicationMessage.Topic;
                try
                {
                    if (string.IsNullOrWhiteSpace(receivedTopic) == false)
                    {
                        if (receivedTopic.Equals(TOPICS.DATAVARIABLE_TOPIC))
                        {
                            JsonDataVariable jdv = new JsonDataVariable();
                            DataVariable dv = new DataVariable();
                            string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                            Log2.Trace("Remote Topic: {0}. Message Received: {1}", receivedTopic, payload);
                            //TODO: Send Data to TankFarm!!!!
                            //DataHasher hasher = new DataHasher();
                            //int hashCode = hasher.HashDataJson(payload);
                            Int32 hash = payload.GetHashCode();
                            Log2.Trace("Remote Payload Hash {0}", hash.ToString());
                            dv = jdv.Json2DataVariable(payload);
                            DataVariableCache.PutObject(dv.ExternalName, dv, (int)hash);
                            Log2.Trace("Remote DV Updated {0} {1}", dv.ExternalName, hash);
                            //Log2.Trace("Remote Now Dumping");

                            //DataVariableCache.DumpCache();
                        }
                        else if (receivedTopic.Equals(TOPICS.COMMAND_TOPIC))
                        {
                            //CHANGE TO MESSAGE CACHE!
                            JsonDataVariable jdv = new JsonDataVariable();
                            DataVariable dv = new DataVariable();
                            string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                            Log2.Trace("Remote Topic: {0}. Message Received: {1}", receivedTopic, payload);
                            //TODO: Send Data to TankFarm!!!!
                            //DataHasher hasher = new DataHasher();
                            //int hashCode = hasher.HashDataJson(payload);
                            Int32 hash = payload.GetHashCode();
                            Log2.Trace("Remote Payload Hash {0}", hash.ToString());
                            dv = jdv.Json2DataVariable(payload);
                            DataVariableCache.PutObject(dv.ExternalName, dv, (int)hash);
                            Log2.Trace("Remote DV Updated {0} {1}", dv.ExternalName, hash);
                            Log2.Trace("Remote Now Dumping");

                            //DataVariableCache.DumpCache();
                        }
                        else if (receivedTopic.Equals(TOPICS.GAME_START_TOPIC))
                        {

                            //JsonEventVariable jev = new JsonEventVariable();
                            //EventVariable ev = new EventVariable();
                            string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                            Log2.Debug("Remote Topic: {0}. Message Received: {1}", receivedTopic, payload);
                            //ev = jev.Json2EventVariable(payload);
                            //EventVariableCache.WriteEventQueue(ev);
                            //Log2.Trace("Remote ADREVENT Queued {0} {1}", ev.EventName, ev.EventType);
                            //Log2.Trace("Remote Now Dumping");
                            //EventVariableCache.DumpQueue();
                        }
                        else if (receivedTopic.StartsWith("smartthings"))
                        {
                            JsonDataVariable jdv = new JsonDataVariable();
                            DataVariable dv = new DataVariable();
                            string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                            Log2.Debug("Remote Topic: {0}. Message Received: {1}", receivedTopic, payload);
                            //TODO: Send Data to TankFarm!!!!
                            //DataHasher hasher = new DataHasher();
                            //int hashCode = hasher.HashDataJson(payload);
                            //place holder for now
                            //DataHasher dataHasher = new DataHasher();
                            //Int32 hash = payload.GetHashCode();
                            //Log2.Trace("Local Payload Hash {0}", hash.ToString());
                            //dv = jdv.Json2DataVariable(payload);
                            //DataVariableCache.PutObject(dv.ExternalName, dv, (int)hash);
                            //Log2.Trace("Local DV Updated {0} {1}", dv.ExternalName, hash);
                            //Log2.Trace("Local Now Dumping");
                        }
                        else if (receivedTopic.Equals(TOPICS.C2AGENT_INCOMING_TOPIC))
                        {
                            JsonDataVariable jsonDataVariable = new JsonDataVariable();
                            DataVariable dataVariable = new DataVariable();

                            JsonGridPeakDetected jsonGridPeakDetected = new JsonGridPeakDetected();

                            string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                            Log2.Debug("Remote XDATA Topic: {0}. Message Received: {1}", receivedTopic, payload);

                            GridPeakDetectedObject gridPeakDetected = jsonGridPeakDetected.Json2GridPeakDetected(payload);
                            Log2.Debug("GOT PEAK FROM " + gridPeakDetected.agent_name);

                            Int32 hash = payload.GetHashCode();
                            Log2.Debug("Remote XDATA Payload Hash {0}", hash.ToString());
                            dataVariable.TagName = gridPeakDetected.type_name;
                            dataVariable.ExternalName = gridPeakDetected.type_name;
                            dataVariable.Value = payload;
                            dataVariable.Description = "A peak has been detected.";
                            DataVariableCache.PutObject(dataVariable.ExternalName, dataVariable, (int)hash);
                            Log2.Debug("Remote XDATA DV Updated {0} {1}", dataVariable.ExternalName, hash);


                            //JsonDataVariable jdv = new JsonDataVariable();
                            //DataVariable dv = new DataVariable();
                            //string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                            //Log2.Trace("Remote XDATAVARIABLE Topic: {0}. Message Received: {1}", receivedTopic, payload);
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
                }
                catch (Exception ex)
                {
                    Log2.Error(ex.Message, ex);
                }
            });

            mqttRemote.StartAsync(options);
            int counter = 0;
            while (mqttRemote.IsConnected == false)
            {
                if (counter > 20)
                {
                    Log2.Error("Remote Mqtt Startup Failed!: {0}", counter.ToString());
                    break;
                }
                counter++;
                Thread.Sleep(1000);
            }

            //mqttRemote.SubscribeAsync(new MqttTopicFilter().Topic = topic);
            
            Log2.Trace("Remote MqttInitializeAsync Completed");

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
                                            bool retainFlag = false, int qos = 2)
        {
            Log2.Trace("Remote MqttPublishAsync Entered");

            mqttRemote.PublishAsync(new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(payload)
            .WithQualityOfServiceLevel((MQTTnet.Protocol.MqttQualityOfServiceLevel)qos)
            .WithRetainFlag(retainFlag)
            .Build());

            Log2.Trace("Remote MqttPublishAsync Completed");

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

            Log2.Trace("Remote MqttSubscribeAsync Entered");

            MqttTopicFilter MqttTopic = new MqttTopicFilter();
            MqttTopic.Topic = topic;
            MqttTopic.QualityOfServiceLevel = (MQTTnet.Protocol.MqttQualityOfServiceLevel)qos;
            mqttRemote.SubscribeAsync(MqttTopic);

            Log2.Trace("Remote MqttSubscribeAsync Completed");
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool MqttStopAsync()
        {
            int counter = 0;
            mqttRemote.StopAsync();
            while (mqttRemote.IsStarted == true)
            {
                if (counter > 20)
                {
                    Log2.Error("Remote Mqtt Stop Failed!: {0}", counter.ToString());
                    break;
                }
                counter++;
                Thread.Sleep(1000);
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool IsConnected()
        {
            bool bit = mqttRemote.IsConnected;
            return bit;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool IsStarted()
        {
            bool bit = mqttRemote.IsStarted;
            return bit;
        }

    }
    #endregion
}
