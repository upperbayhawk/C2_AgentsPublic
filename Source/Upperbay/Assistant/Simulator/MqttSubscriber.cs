// Copyright (C) Upperbay Systems, LLC - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and confidential
// Written by Dave Hardin <dave@upperbay.com>, 2001-2020

using System;
using System.Reflection;
using System.Collections;

using Upperbay.Core.Logging;
using Upperbay.Core.Library;
using Upperbay.Agent.Interfaces;
using Upperbay.Agent.ColonyMatrix;
using Upperbay.Worker.JSON;



namespace Upperbay.Assistant
{
    public class MqttSubscriber : IAgentObjectAssistant
    {
                
        public AgentPassPort AgentPassPort { get { return this._agentPassPort; } set { this._agentPassPort = value; } }

        #region Methods

        public MqttSubscriber()
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

                    Log2.Trace("{0} MqttSubscriber: Class: {1}", _myAgentObjectName, _myType.ToString());

                    _myProperties = Utilities.GetDecoratedProperties(_myType, _attributeString);
                    if (_myProperties != null)
                    {
                        foreach (string prop in _myProperties)
                        {
                            Log2.Trace("{0}: MqttSubscriber Attribute: {1}", _myAgentObjectName, prop);
                           // _agentData.AddAgentProperty(_myAgentObjectName, prop);
                        }

                        _activeState = true;
                    }
                    else
                    {
                        Log2.Trace("{0}: No MqttSubscriber Attributes", _myAgentObjectName);
                    }
                }
                else
                {
                    Log2.Error("Start MqttSubscriber Failed!");
                }
            }
            catch (Exception Ex)
            {
                Log2.Error("Start MqttSubscriber Exception: {0}", Ex.ToString());
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
                    
                    JsonDataVariable jData = new JsonDataVariable();
                    foreach (string prop in _myProperties)
                    {
                        PropertyInfo propInfo = _myType.GetProperty(prop);
                        Log2.Trace("{0}: Agent MqttSubscriber Property {1}", _myAgentObjectName, prop);
                        // Check Units
                        object[] attributes = propInfo.GetCustomAttributes(typeof(TypeAttribute), false);
                        if (attributes != null && attributes.Length > 0)
                        {
                            TypeAttribute type = (TypeAttribute)attributes[0];
                        }

                     
                        DataVariable var = (DataVariable)propInfo.GetValue(_myAgentObject, null);


                        Log2.Trace("Subscribe: Getting: {0}", prop);
                        DataVariable dv = DataVariableCache.GetObject(prop);
                        if (dv == null)
                        {
                            Log2.Trace("Subscribe: NULL for {0}", prop);
                        }
                        else
                        {
                            propInfo.SetValue(_myAgentObject, dv, null);
                            Log2.Trace("Subscribe: {0} {1}", prop, dv.Value);
                            var = (DataVariable)propInfo.GetValue(_myAgentObject, null);
                            if (var.Value == dv.Value)
                                Log2.Trace("BIGUS!!!!: {0} {1}", prop, dv.Value);


                        }

                        // _agentData.UpdatePropertyValue(_myAgentObjectName, prop, var.Value, var.Quality, var.UpdateTime);

                    }
                }
                catch (Exception Ex)
                {
                    Log2.Error("{0}: Exception in MqttSubscriber: {1}", _myAgentObjectName, Ex.ToString());

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
                        Log2.Trace("{0}: Agent MqttSubscriber Property {1}", _myAgentObjectName, prop);
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
                }
                catch (Exception Ex)
                {
                    Log2.Error("{0}: Exception in MqttSubscriber.Stop: {1}", _myAgentObjectName, Ex.ToString());
                }
            }
            _activeState = false;
            return true;
        }

        #endregion


        #region Private State Variables


        //private string _celestial = null;
        //public string Celestial { get { return this._celestial; } set { this._celestial = value; } }

        //private string _collective = null;
        //public string Collective { get { return this._collective; } set { this._collective = value; } }

        //private string _community = null;
        //public string Community { get { return this._community; } set { this._community = value; } }

        //private string _cluster = null;
        //public string Cluster { get { return this._cluster; } set { this._cluster = value; } }

        //private string _carrier = null;
        //public string Carrier { get { return this._carrier; } set { this._carrier = value; } }

        //private string _colony = null;
        //public string Colony { get { return this._colony; } set { this._colony = value; } }

        //private string _service = null;
        //public string Service { get { return this._service; } set { this._service = value; } }

        //private string _nickName = null;
        //public string NickName { get { return this._nickName; } set { this._nickName = value; } }

        //private string _description = null;
        //public string Description { get { return this._description; } set { this._description = value; } }
        private AgentPassPort _agentPassPort = null;

        private string _myAgentClassName = null;
        private string _myAgentObjectName = null;
        private object _myAgentObject = null;
       
        private bool _activeState = false;
        private ArrayList _myProperties = null;
        private Type _myType = null;

        private string _attributeString = "subscribe";

        // Private Members
        //public AgentData _agentData = null;
        #endregion
    }
}


