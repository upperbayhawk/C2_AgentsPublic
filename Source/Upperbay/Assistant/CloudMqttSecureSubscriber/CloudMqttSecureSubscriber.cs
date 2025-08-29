//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
using System;
using System.Reflection;
using System.Collections;
using System.Configuration;

using Upperbay.Core.Logging;
using Upperbay.Core.Library;
using Upperbay.Agent.Interfaces;
using Upperbay.Worker.MQTT;
using Upperbay.Worker.JSON;
using Upperbay.Worker.Timers;
using Upperbay.Assistant;

/// <summary>
/// 
/// </summary>

namespace Upperbay.Assistant
{
    public class CloudMqttSecureSubscriber : IAgentObjectAssistant
    {
        public AgentPassPort AgentPassPort { get { return this._agentPassPort; } set { this._agentPassPort = value; } }

        #region Methods
        public CloudMqttSecureSubscriber()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="myAgentClassName"></param>
        /// <param name="myAgentObjectName"></param>
        /// <param name="myAgentObject"></param>
        public bool Initialize(string myAgentClassName, string myAgentObjectName, object myAgentObject)
        {
            try
            {

                if ((myAgentClassName != null) && (myAgentObject != null) && (myAgentObjectName != null))
                {
                    _myAgentClassName = myAgentClassName;
                    _myAgentObjectName = myAgentObjectName;
                    _myAgentObject = myAgentObject;

                    //`````````````````````````````````````````````````````````````````````````````````````
                    //_agentData = new AgentData();

                    //_agentData.Celestial = _agentPassPort.Celestial;
                    //_agentData.Collective = _agentPassPort.Collective;
                    //_agentData.Community = _agentPassPort.Community;
                    //_agentData.Cluster = _agentPassPort.Cluster;
                    //_agentData.Carrier = _agentPassPort.Carrier;
                    //_agentData.Colony = _agentPassPort.Colony;
                    //_agentData.Service = _agentPassPort.ServiceName;

                  //  _agentData.AgentPassPort = this._agentPassPort;

                    //Log2.Trace("{0}: Carrier = {1}", _myAgentObjectName, _agentData.Carrier);

                    //Log2.Trace("{0}: Adding Nick Agent = {1}",
                    //    _myAgentObjectName,
                    //    _agentPassPort.AgentNickName);

                    //_agentData.AddAgent(_myAgentObjectName,
                    //                    _agentPassPort.AgentNickName,
                    //                    _agentPassPort.Description,
                    //                    "C4Agent");

                    //`````````````````````````````````````````````````````````````````````````````````````
                    
                    _myType = _myAgentObject.GetType();

                    Log2.Trace("{0} CloudMqttSecureSubscriber: Class: {1}", _myAgentObjectName, _myType.ToString());

                    _myProperties = Utilities.GetDecoratedProperties(_myType, _attributeString);
                    if (_myProperties != null)
                    {
                        foreach (string prop in _myProperties)
                        {
                            Log2.Trace("{0}: CloudMqttSecureSubscriber Attribute: {1}", _myAgentObjectName, prop);
                           // _agentData.AddAgentProperty(_myAgentObjectName, prop);
                        }

 
                        string mqttCloudEnable = MyAppConfig.GetParameter("MqttCloudSecureEnable");
                        if (mqttCloudEnable.Equals("true"))
                        {
                            _bCloudEnable = true;
                            string cluster = MyAppConfig.GetParameter("ClusterName");
                            string mqttCloudIpAddress = MyAppConfig.GetClusterParameter(cluster,"MqttCloudSecureIpAddress");
                            string mqttCloudLoginName = MyAppConfig.GetClusterParameter(cluster,"MqttCloudSecureLoginName");
                            string mqttCloudPassword = MyAppConfig.GetClusterParameter(cluster,"MqttCloudSecurePassword");
                            string mqttCloudPort = MyAppConfig.GetClusterParameter(cluster,"MqttCloudSecurePort");
                     
                            MqttCloudSecureDriver.MqttInitializeAsync(mqttCloudIpAddress,
                                                               mqttCloudLoginName,
                                                               mqttCloudPassword,
                                                               int.Parse(mqttCloudPort));
                            
                            MqttCloudSecureDriver.MqttSubscribeAsync(TOPICS.GAME_PLAYER_CONFIDENTIAL_TOPIC);

                            Log2.Debug("CloudMqttSecureSubscriber.MqttInitializeAsync Completed");
                        }

                        _activeState = true;
                    }
                    else
                    {
                        Log2.Trace("CloudMqttSecureSubscriber: No MqttPublisher Attributes");
                    }
                }
                else
                {
                    Log2.Error("CloudMqttSecureSubscriber Start MqttPublisher Failed!");
                }
            }
            catch (Exception Ex)
            {
                Log2.Error("CloudMqttSecureSubscriber Start MqttPublisher Exception: {0}", Ex.ToString());
            }
            return true;
        }
       

        /// <summary>
        /// 
        /// </summary>
        public bool Start()
        {
            if (_activeState)
            {
            }
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Fire()
        {
            if (_activeState)
            {
                try
                {
                    //string json;
                    JsonDataVariable jData = new JsonDataVariable();
                    foreach (string prop in _myProperties)
                    {
                        PropertyInfo propInfo = _myType.GetProperty(prop);
                        Log2.Trace("{0}: Agent Publish Property {1}", _myAgentObjectName, prop);
                        // Check Units
                        object[] attributes = propInfo.GetCustomAttributes(typeof(TypeAttribute), false);
                        if (attributes != null && attributes.Length > 0)
                        {
                            TypeAttribute type = (TypeAttribute)attributes[0];
                        }
                        ////TODO VAR
                        DataVariable var = (DataVariable)propInfo.GetValue(_myAgentObject, null);
                        // _agentData.UpdatePropertyValue(_myAgentObjectName, prop, var.Value, var.Quality, var.UpdateTime);
                    }
                }
                catch (Exception Ex)
                {
                    Log2.Error("{0}: Exception in CloudMqttSecureSubscriber: {1}", _myAgentObjectName, Ex.ToString());
                }
               
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Stop()
        {
            if (_activeState)
            {
                try
                {
                    foreach (string prop in _myProperties)
                    {
                        PropertyInfo propInfo = _myType.GetProperty(prop);
                        Log2.Trace("{0}: Agent Publish Property {1}", _myAgentObjectName, prop);
                        // Check Units
                        object[] attributes = propInfo.GetCustomAttributes(typeof(TypeAttribute), false);
                        if (attributes != null && attributes.Length > 0)
                        {
                            TypeAttribute type = (TypeAttribute)attributes[0];
                        }

                        ////TODO VAR

                        DataVariable var = (DataVariable)propInfo.GetValue(_myAgentObject, null);
                       // _agentData.RemoveAgentProperty(_myAgentObjectName, prop);
                    }
                    // _agentData.RemoveAgent(_myAgentObjectName);

                    if (_bCloudEnable)
                        MqttCloudSecureDriver.MqttStopAsync();
                   
                }
                catch (Exception Ex)
                {
                    Log2.Error("{0}: Exception in CloudMqttSecureSubscriber.Stop: {1}", _myAgentObjectName, Ex.ToString());

                }
            }
            _activeState = false;
            return true;
        }

        #endregion

        #region Private State Variables

        private AgentPassPort _agentPassPort = null;

        private TimeTrigger _timeTrigger = new TimeTrigger();
        private bool _bCloudEnable = false;

        private string _myAgentClassName = null;
        private string _myAgentObjectName = null;
        private object _myAgentObject = null;

        private bool _activeState = false;
        private ArrayList _myProperties = null;
        private Type _myType = null;

        private string _attributeString = "publish";

        // Private Members
        //public AgentData _agentData = null;

        #endregion

    }
}


