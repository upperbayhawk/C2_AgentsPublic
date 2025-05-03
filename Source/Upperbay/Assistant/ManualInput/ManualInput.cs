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



namespace Upperbay.Assistant
{
    public class ManualInput : IAgentObjectAssistant
    {

        public AgentPassPort AgentPassPort { get { return this._agentPassPort; } set { this._agentPassPort = value; } }

        #region Methods

        public ManualInput()
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

                    //```````````````````````````````````````````````````````````````````````````````
                    //_agentData = new AgentData();
                    //_agentData.AgentPassPort = this._agentPassPort;

                    //_agentData.Celestial = _agentPassPort.Celestial;
                    //_agentData.Collective = _agentPassPort.Collective;
                    //_agentData.Community = _agentPassPort.Community;
                    //_agentData.Cluster = _agentPassPort.Cluster;
                    //_agentData.Carrier = _agentPassPort.Carrier;
                    //_agentData.Colony = _agentPassPort.Colony;
                    //_agentData.Service = _agentPassPort.ServiceName;

                    Log2.Trace("{0}: ManualInput Carrier = {1}", _myAgentObjectName, _agentPassPort.Carrier);

                    //Log2.Trace("{0}: Adding Nick Agent = {1}",
                    //    _myAgentObjectName,
                    //    _agentPassPort.AgentNickName);

                    //_agentData.AddAgent(_myAgentObjectName,
                    //                    _agentPassPort.AgentNickName,
                    //                    _agentPassPort.Description,
                    //                    "Degrees");
                    //```````````````````````````````````````````````````````````````````````````````

                    _myType = _myAgentObject.GetType();

                    Log2.Trace("{0} ManualInput: Class: {1}", _myAgentObjectName, _myType.ToString());

                    _myProperties = Utilities.GetDecoratedProperties(_myType, _attributeString);
                    if (_myProperties != null)
                    {
                        foreach (string prop in _myProperties)
                        {
                            Log2.Trace("{0}: ManualInput Attribute: {1}", _myAgentObjectName, prop);
                        }

                        _activeState = true;
                    }
                    else
                    {
                        Log2.Trace("{0}: No ManualInput Attributes", _myAgentObjectName);
                    }
                }
                else
                {
                    Log2.Error("Start ManualInput Failed!");
                }
            }
            catch (Exception Ex)
            {
                Log2.Error("Start ManualInput Exception: {0}", Ex.ToString());
            }
            return true;
        }
        

        /// <summary>
        /// 
        /// </summary>
        public bool Start( )
        {

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
                    string qualifiedInputProperty = null;
                    string qualifiedInputPropertyValue = null;
                    string readbackValue = null;

                    //````````````````````````````````````````````````````````````````````````````````````````````
                    foreach (string prop in _myProperties)
                    {
                        PropertyInfo propInfo = _myType.GetProperty(prop);
                        Log2.Trace("{0}: ManualInput Agent Input Property {1}", _myAgentObjectName, prop);
                        // Check Units
                        object[] attributes = propInfo.GetCustomAttributes(typeof(TypeAttribute), false);
                        if (attributes != null && attributes.Length > 0)
                        {
                            TypeAttribute type = (TypeAttribute)attributes[0];
                            Log2.Trace(
                                "ManualInput The units for " + _myAgentObjectName + "." + prop + " = " + type.TypeString);
                        }

                        qualifiedInputProperty = MyAppConfig.GetParameter(prop);
                        if (qualifiedInputProperty != null)
                        {
                            Log2.Trace("ManualInput Input Value = {0}", qualifiedInputProperty);

                           
                            //qualifiedInputPropertyValue = _agentData.GetQualifiedPropertyValue(qualifiedInputProperty, out quality, out time);

                            if (qualifiedInputProperty != null)
//                                if (qualifiedInputPropertyValue != null)
                                {
                                    Log2.Trace("{0}: ManualInput  Agent Input GetQualifiedPropertyValue: {1}", _myAgentObjectName, qualifiedInputPropertyValue);
                              //  Log2.Trace("{0}: Agent Input GetQualifiedPropertyValue: {1}", _myAgentObjectName, qualifiedInputPropertyValue);

                                //
                                DataVariable var = (DataVariable)propInfo.GetValue(_myAgentObject, null);
                                var.Value = qualifiedInputProperty;
                             
                                propInfo.SetValue(_myAgentObject, var, null);

                                var = (DataVariable)propInfo.GetValue(_myAgentObject, null);
                                readbackValue = var.Value;
                                Log2.Trace("ManualInput Input Readback Value: {0}", readbackValue);
                            }
                            else
                            {
                                Log2.Error("{0}: ManualInput Input Property NOT in Database: {1}", _myAgentObjectName, prop);
                            }
                        }
                        else
                        {
                            Log2.Error("{0}: ManualInput Input Property NOT in App.Config: {1}", _myAgentObjectName, prop);
                        }
                    }
                }
                catch (Exception Ex)
                {
                    Log2.Error("{0}: Exception in ManualInput InputPropertyData: {1}", _myAgentObjectName, Ex.ToString());

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
            _activeState = false;
            return true;
        }

        #endregion

        #region Private State
        // Public Properties

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
        //private TraceSwitch _myTraceSwitch = Upperbay.Core.Logging.Log2.AgentSwitch;

        private bool _activeState = false;
        private ArrayList _myProperties = null;
        private Type _myType = null;

        private string _attributeString = "manualinput";

        // Private Members
        // public AgentData _agentData = null;
        #endregion
    }
}


