//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
using System;
using System.Threading;
using System.Reflection;
using System.Collections;
using System.Configuration;

using Upperbay.Core.Logging;
using Upperbay.Core.Library;
using Upperbay.Agent.Interfaces;
using Upperbay.Worker.EtherAccess;


namespace Upperbay.Assistant
{
    public class EtherWriteAccessor : IAgentObjectAssistant
    {
        
        public AgentPassPort AgentPassPort { get { return this._agentPassPort; } set { this._agentPassPort = value; } }

        #region Methods
        public EtherWriteAccessor()
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
            Log2.Trace("EtherWriteAccessor Initialize {0}", _myAgentObjectName);
            try
            {

                if ((myAgentClassName != null) && (myAgentObject != null) && (myAgentObjectName != null))
                {
                    _myAgentClassName = myAgentClassName;
                    _myAgentObjectName = myAgentObjectName;
                    _myAgentObject = myAgentObject;

                    _myType = _myAgentObject.GetType();

                    Log2.Trace("EtherWriteAccessor: Class: {0}", _myType.ToString());

                    _myProperties = Utilities.GetDecoratedProperties(_myType, _thingSpeakAttributeString);
                    if (_myProperties != null)
                    {
                        foreach (string prop in _myProperties)
                        {
                            Log2.Trace("EtherWriteTestAccessor Attribute: {0}", prop);
                        }
                        //FOUND ONE!
                        _activeState = true;
                    }
                    else
                    {
                        //Log2.Trace("{0}: No EtherWriteTestAccessor Attributes", _myAgentObjectName);
                    }
                }
                else
                {
                    //Log2.Error("Start EtherWriteTestAccessor Failed!");
                }
            }
            catch (Exception Ex)
            {
                Log2.Error("Start EtherWriteTestAccessor Exception: {0}", Ex.ToString());
            }
            return true;
        }
        

        /// <summary>
        /// 
        /// </summary>
        public bool Start()
        {
            Log2.Trace("EtherWriteTestAccessor Start {0}", _myAgentObjectName); 
            try
            {
                string serverUrl = MyAppConfig.GetParameter("EtherWriteTestReferenceServerURL");

                if (serverUrl != null)
                {
                    Log2.Trace("EtherWriteTestReferenceServerURL {0}", serverUrl);
                    _tsDataAccess = new EtherAccess();
                    //---------------------------------------------------------------------------

                    foreach (string prop in _myProperties)
                    {
                        string qualifiedInputProperty = MyAppConfig.GetParameter(prop);
                        if (qualifiedInputProperty != null)
                        {
                            //_uaDataAccess.AddNodeUrl(prop, qualifiedInputProperty);
                            Log2.Trace("EtherWriteTestAccessor Write Data = {0}", qualifiedInputProperty);
                            //---------------------------------------------------------------------------
                        }
                    }
                }
                else
                {
                    Log2.Error("{0}: EtherWriteTestReferenceServerURL Not Found", _myAgentObjectName);
                }
            }
            catch (Exception Ex)
            {
                Log2.Error("{0}: Exception in EtherWriteTestAccessor {1}", _myAgentObjectName, Ex.ToString());
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
                Log2.Trace("ThingSpeak Fire");

                string qualifiedInputProperty = null;
                string qualifiedInputPropertyValue = null;
                string readbackValue = null;
                //DataVariable dataVar;
                //bool bResult;

                //Log2.Trace("calling TS GetValue");
                //readbackValue = _tsDataAccess.GetValue();
                //Log2.Trace("TS GetValue {0}", readbackValue);

                try
                {
                    //````````````````````````````````````````````````````````````````````````````````````````````
                    foreach (string prop in _myProperties)
                    {
                        PropertyInfo propInfo = _myType.GetProperty(prop);
                        Log2.Trace("EtherWriteTestAccessor Write Property {0}", prop);
                        // Check Units
                        object[] attributes = propInfo.GetCustomAttributes(typeof(TypeAttribute), false);
                        if (attributes != null && attributes.Length > 0)
                        {
                            TypeAttribute type = (TypeAttribute)attributes[0];
                            Log2.Trace(
                                "The type for " + _myAgentObjectName + "." + prop + " = " + type.TypeString);
                        }

                        qualifiedInputProperty = MyAppConfig.GetParameter(prop);
                        if (qualifiedInputProperty != null)
                        {
                            Log2.Trace("AEtherWriteTestAccessor Output Data = {0}", qualifiedInputProperty);
                            //TODO
                            //qualifiedInputPropertyValue = _tsDataAccess.GetValue(qualifiedInputProperty);
                            if (qualifiedInputProperty != null)
                            {
                                Log2.Trace("AEtherWriteTestAccessor Input GetQualifiedPropertyValue: {0}", qualifiedInputPropertyValue);
                                //Log2.Trace("Agent TS Output GetQualifiedPropertyValue for Write {0}", _myAgentObjectName);
                                //TODO
                                // Set Time, Quality and Status if Variable

                                DataVariable var = (DataVariable)propInfo.GetValue(_myAgentObject, null);
                                //var.Value = qualifiedInputPropertyValue;
                                //var.Value = dataVar.Value;
                                //var.Status = dataVar.Status;
                                //var.Time = dataVar.Time;
                                if (var.ChangeFlag == true)
                                {
                                   // DateTime timeStamp;
                                    //if (DateTime.TryParse(_uaDataAccess.GetDataTime(prop), out timeStamp))
                                    //{
                                    //    var.Time = timeStamp;
                                    //}
                                    //else
                                    //{
                                    //    var.Time = DateTime.Now;
                                    //}
                                    //bResult = _tsDataAccess.SetValue(qualifiedInputProperty, var);
                                    var.ChangeFlag = false;
                                    //                                propInfo.SetValue(_myAgentObject, var, null);

                                    //TODO
                                    // var = (DataVariable)propInfo.GetValue(_myAgentObject, null);
                                    //readbackValue = var.Value;
                                    Thread.Sleep(1000);
                                    Log2.Trace("AEtherWriteTestAccessor Input Set Value: {0}", readbackValue);
                                }
                            }
                            else
                            {
                                Log2.Error("AEtherWriteTestAccessor DataAccess Property NOT in Database: {0}", prop);
                            }
                        }
                        else
                        {
                            Log2.Error("EtherWriteTestAccessor DataAccess Property NOT in App.Config: {0}", prop);
                        }
                    }
                }
                catch (Exception Ex)
                {
                    Log2.Error("Exception in EtherWriteTestAccessor: {0}", Ex.ToString());
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
                     //_tsDataAccess.Close();
                }
                catch (Exception Ex)
                {
                    Log2.Error("Exception in EtherWriteTestAccessor" +
                        ": {0}", Ex.ToString());
                }
            }
            _activeState = false;
            return true;
        }

        #endregion

        #region Private State
        
        private AgentPassPort _agentPassPort = null;
        private string _myAgentClassName = null;
        private string _myAgentObjectName = null;
        private object _myAgentObject = null;

        private bool _activeState = false;
        private ArrayList _myProperties = null;
        private Type _myType = null;

        private string _thingSpeakAttributeString = "ether_write_test";
        private EtherAccess _tsDataAccess = null;

        #endregion
    }
}
