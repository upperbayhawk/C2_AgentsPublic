//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================

// Services/ChatGptService.cs
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Upperbay.Core.Logging;
using System.Collections.Concurrent;

using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Client.Options;
using System.ComponentModel;

namespace ChatterBoxGPT
{
   
    public class MQTTPipe
    {
        private static IManagedMqttClient mqttLocal;
        private static string MQTT_TOPIC_TOASSISTANT = "openai/assistant/GridLoadMan-2-0-0/ToAssistant";
        private static string MQTT_TOPIC_TOCLIENT = "openai/assistant/GridLoadMan-2-0-0/ToClient";
        
        private static ConcurrentQueue<string> queue = new ConcurrentQueue<string>();


        #region Methods
        static MQTTPipe()
        {
        }

        /// <summary>
        /// 
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

            Log2.Trace("Local MqttInitializeAsync Entered");

          
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

            mqttLocal = new MqttFactory().CreateManagedMqttClient();

            Log2.Trace("Local MqttFactory Called");

            mqttLocal.UseConnectedHandler(e =>
            {
                Log2.Debug("Local Connected successfully with MQTT Broker: {0}", e);
            });

            mqttLocal.UseDisconnectedHandler(e =>
            {
                Log2.Error("Local Disconnected from MQTT Broker: {0}",
                                                            e.Exception);
            });

            mqttLocal.UseApplicationMessageReceivedHandler(e =>
            {
                string receivedTopic = e.ApplicationMessage.Topic;
                try
                {
                    if (string.IsNullOrWhiteSpace(receivedTopic) == false)
                    {
                        if (receivedTopic.Equals(MQTT_TOPIC_TOCLIENT))
                        {
                            string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                            Log2.Info("Got ToClient Topic: {0}. Message Received: {1}", receivedTopic, payload);
                            Console.WriteLine("Got Local Topic: {0}. Message Received: {1}", receivedTopic, payload);
                            queue.Enqueue(payload);
                        }
                        else
                        {
                            string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                            Log2.Info("Got NOT ToClient Topic: {0}. Message Received: {1}", receivedTopic, payload);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log2.Error(ex.Message, ex);
                }
            });


            mqttLocal.StartAsync(options);
            int counter = 0;
            while (mqttLocal.IsConnected == false)
            {
                if (counter > 20)
                {
                    Log2.Error("Local Mqtt Startup Failed!: {0}", counter.ToString());
                    break;
                }
                counter++;
                Thread.Sleep(1000);
            }

            
            Log2.Trace("Local MqttInitializeAsync Completed");

            return true;
        }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static string ReadMessage()
    {
        string message = null;
        // Dequeue items and print them

        while (!queue.TryDequeue(out message))
        {
            Thread.Sleep(500);
        }
        
        Console.WriteLine($"Dequeued: {message}");
        Log2.Debug($"Dequeued: {message}");
        return message;

    }

    /// <summary>
    /// Publish Message.
    /// </summary>
    /// <param name="topic">Topic.</param>
    /// <param name="payload">Payload.</param>
    /// <param name="retainFlag">Retain flag.</param>
    /// <param name="qos">Quality of Service.</param>
    /// <returns>Task.</returns>

    public static bool PublishMessage(string payload,
                                        bool retainFlag = false,
                                        int qos = 2)
        {
            Log2.Trace("Local MqttPublishAsync Entered");
          

            mqttLocal.PublishAsync(new MqttApplicationMessageBuilder()
            .WithTopic(MQTT_TOPIC_TOASSISTANT)
            .WithPayload(payload)
            .WithQualityOfServiceLevel((MQTTnet.Protocol.MqttQualityOfServiceLevel)qos)
            .WithRetainFlag(retainFlag)
            .Build());

            Log2.Trace("Local MqttPublishAsync Completed");

            return true;
        }


        /// <summary>
        /// Subscribe topic.
        /// </summary>
        /// <param name="topic">Topic.</param>
        /// <param name="qos">Quality of Service.</param>
        /// <returns>Task.</returns>
        public static bool MqttSubscribeAsync(int qos = 2)
        {

            Log2.Trace("Local MqttSubscribeAsync Entered");

            MqttTopicFilter MqttTopic = new MqttTopicFilter();
            MqttTopic.Topic = MQTT_TOPIC_TOCLIENT;
            MqttTopic.QualityOfServiceLevel = (MQTTnet.Protocol.MqttQualityOfServiceLevel)qos;
            mqttLocal.SubscribeAsync(MqttTopic);

            MqttTopicFilter MqttTopic1 = new MqttTopicFilter();
            MqttTopic1.Topic = MQTT_TOPIC_TOASSISTANT;
            MqttTopic1.QualityOfServiceLevel = (MQTTnet.Protocol.MqttQualityOfServiceLevel)qos;
            mqttLocal.SubscribeAsync(MqttTopic1);


            Log2.Trace("Local MqttSubscribeAsync Completed");
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool MqttStopAsync()
        {
            int counter = 0;
            mqttLocal.StopAsync();
            while (mqttLocal.IsConnected == true)
            {
                if (counter > 20)
                {
                    Log2.Error("Local Mqtt Stop Failed!: {0}", counter.ToString());
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
            bool bit = mqttLocal.IsConnected;
            return bit;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool IsStarted()
        {
            bool bit = mqttLocal.IsStarted;
            return bit;
        }

    }


    #endregion
}
