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



namespace Upperbay.Assistant
{
    public class MqttPublisher : IAgentObjectAssistant
    {

        
        public AgentPassPort AgentPassPort { get { return this._agentPassPort; } set { this._agentPassPort = value; } }

        #region Methods
        public MqttPublisher()
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

                    Log2.Trace("{0} MqttPublisher: Class: {1}", _myAgentObjectName, _myType.ToString());

                    _myProperties = Utilities.GetDecoratedProperties(_myType, _attributeString);
                    if (_myProperties != null)
                    {
                        foreach (string prop in _myProperties)
                        {
                            Log2.Trace("{0}: MqttPublisher Attribute: {1}", _myAgentObjectName, prop);
                            // _agentData.AddAgentProperty(_myAgentObjectName, prop);
                        }

                        //Fire up MQTT
                        //Local is REQUIRED
                        string mqttLocalEnable = MyAppConfig.GetParameter("MqttLocalEnable");
                        if (mqttLocalEnable.Equals("true"))
                        {
                            _bLocalEnable = true;
                            string mqttLocalIpAddress = MyAppConfig.GetParameter("MqttLocalIpAddress");
                            string mqttLocalLoginName = MyAppConfig.GetParameter("MqttLocalLoginName");
                            string mqttLocalPassword = MyAppConfig.GetParameter("MqttLocalPassword");
                            string mqttLocalPort = MyAppConfig.GetParameter("MqttLocalPort");

                            MqttLocalDriver.MqttInitializeAsync(mqttLocalIpAddress,
                                       mqttLocalLoginName,
                                       mqttLocalPassword,
                                       int.Parse(mqttLocalPort));
                            MqttLocalDriver.MqttSubscribeAsync(TOPICS.DATAVARIABLE_TOPIC);
                            MqttLocalDriver.MqttSubscribeAsync(TOPICS.COMMAND_TOPIC);
                            MqttLocalDriver.MqttSubscribeAsync(TOPICS.EVENT_TOPIC);
                            MqttLocalDriver.MqttSubscribeAsync(TOPICS.C2AGENT_INCOMING_TOPIC);
                            MqttLocalDriver.MqttSubscribeAsync("CREATEGAME");

                            Log2.Debug("{0}: MqttPublisher: MqttLocalDriver.MqttInitializeAsync Started", _myAgentObjectName);
                        }


                        string mqttRemoteEnable = MyAppConfig.GetParameter("MqttRemoteEnable");
                        if (mqttRemoteEnable.Equals("true"))
                        {
                            _bRemoteEnable = true;
                            string mqttRemoteIpAddress = MyAppConfig.GetParameter("MqttRemoteIpAddress");
                            string mqttRemoteLoginName = MyAppConfig.GetParameter("MqttRemoteLoginName");
                            string mqttRemotePassword = MyAppConfig.GetParameter("MqttRemotePassword");
                            string mqttRemotePort = MyAppConfig.GetParameter("MqttRemotePort");
                       
                            MqttRemoteDriver.MqttInitializeAsync(mqttRemoteIpAddress,
                                                            mqttRemoteLoginName,
                                                            mqttRemotePassword,
                                                            int.Parse(mqttRemotePort));
                            MqttRemoteDriver.MqttSubscribeAsync(TOPICS.DATAVARIABLE_TOPIC);
                            MqttRemoteDriver.MqttSubscribeAsync(TOPICS.COMMAND_TOPIC);
                            MqttRemoteDriver.MqttSubscribeAsync(TOPICS.EVENT_TOPIC);
                            MqttRemoteDriver.MqttSubscribeAsync(TOPICS.C2AGENT_INCOMING_TOPIC);

                            Log2.Debug("MqttPublisher: MqttRemoteDriver.MqttInitializeAsync Started");
                        }
                        _activeState = true;
                    }
                    else
                    {
                        Log2.Trace("{0}: No MqttPublisher Attributes", _myAgentObjectName);
                    }
                }
                else
                {
                    Log2.Error("Start MqttPublisher Failed!");
                }
            }
            catch (Exception Ex)
            {
                Log2.Error("Start MqttPublisher Exception: {0}", Ex.ToString());
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
                    string json;
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
                            //Log2.TraceIf(_myTraceSwitch,
                            //    "The units for " + _myAgentObjectName + "." + prop + " = " + type.TypeString);
                        }

                        ////TODO VAR

                        DataVariable var = (DataVariable)propInfo.GetValue(_myAgentObject, null);

                        //AgentValue16.Value = count.ToString();
                        //AgentValue16.ExternalName = "TrashyTag3";

                        json = jData.DataVariable2Json(var);
                        Log2.Trace("Starting to write MQTT {0}", json);
                        if (_bLocalEnable)
                            MqttLocalDriver.MqttPublishAsync(TOPICS.DATAVARIABLE_TOPIC, json);
                        if(_bRemoteEnable)
                            MqttRemoteDriver.MqttPublishAsync(TOPICS.DATAVARIABLE_TOPIC, json);

                        // _agentData.UpdatePropertyValue(_myAgentObjectName, prop, var.Value, var.Quality, var.UpdateTime);

                        //Log2.TraceIf(_myTraceSwitch,
                        //    "Published: " + _myAgentObjectName + "." + prop + " = " + propInfo.GetValue(_myAgentObject, null));

                    }
                }
                catch (Exception Ex)
                {
                    Log2.Error("{0}: Exception in PublishPropertyData: {1}", _myAgentObjectName, Ex.ToString());

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
                            //Log2.TraceIf(_myTraceSwitch,
                            //    "The units for " + _myAgentObjectName + "." + prop + " = " + type.TypeString);
                        }

                        ////TODO VAR

                        DataVariable var = (DataVariable)propInfo.GetValue(_myAgentObject, null);
                       // _agentData.RemoveAgentProperty(_myAgentObjectName, prop);

                        //Log2.TraceIf(_myTraceSwitch,
                        //    "Published: " + _myAgentObjectName + "." + prop + " = " + propInfo.GetValue(_myAgentObject, null));


                    }
                    // _agentData.RemoveAgent(_myAgentObjectName);

                    if (_bLocalEnable) 
                        MqttLocalDriver.MqttStopAsync();
                    if (_bRemoteEnable)
                        MqttRemoteDriver.MqttStopAsync();
                   
                }
                catch (Exception Ex)
                {
                    Log2.Error("{0}: Exception in PublishPropertyData.Stop: {1}", _myAgentObjectName, Ex.ToString());

                }
            }
            _activeState = false;
            return true;
        }

        #endregion


        #region Private State Variables

        private AgentPassPort _agentPassPort = null;

        private TimeTrigger _5minTimeTrigger = new TimeTrigger();

        private bool _bRemoteEnable = false;
        private bool _bLocalEnable = false;

        private string _myAgentClassName = null;
        private string _myAgentObjectName = null;
        private object _myAgentObject = null;
        //private TraceSwitch _myTraceSwitch = Upperbay.Core.Logging.Log2.AgentSwitch;

        private bool _activeState = false;
        private ArrayList _myProperties = null;
        private Type _myType = null;

        private string _attributeString = "publish";

        // Private Members
        //public AgentData _agentData = null;
        #endregion
    }
}


