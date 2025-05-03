using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Reflection;
using System.Collections;
using System.Configuration;
using System.Management;
using System.Management.Instrumentation;

using Upperbay.Core.Logging;
using Upperbay.Core.Library;
using Upperbay.Agent.Interfaces;


namespace Upperbay.Assistant
{
    public class UaDataAccessor : IAgentObjectAssistant
    {
        private AgentPassPort _agentPassPort = null;
        public AgentPassPort AgentPassPort { get { return this._agentPassPort; } set { this._agentPassPort = value; } }

        public UaDataAccessor()
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

                    _myType = _myAgentObject.GetType();

                    Log2.Trace("{0} UaDataAccessor: Class: {1}", _myAgentObjectName, _myType.ToString());
                    Log2.Trace("{0} UaDataAccessor: Class: {1}", _myAgentObjectName, _myType.ToString());


                    _myProperties = Utilities.GetDecoratedProperties(_myType, _attributeString);
                    if (_myProperties != null)
                    {
                        foreach (string prop in _myProperties)
                        {
                            Log2.Trace("{0}: UaDataAccessor Attribute: {1}", _myAgentObjectName, prop);
                        }

                        _activeState = true;
                    }
                    else
                    {
                        Log2.Trace("{0}: No UaDataAccessor Attributes", _myAgentObjectName);
                    }
                }
                else
                {
                    Log2.Error("Start UaDataAccessor Failed!");
                }
            }
            catch (Exception Ex)
            {
                Log2.Error("Start UaDataAccessor Exception: {0}", Ex.ToString());
            }
            return true;
        }
        // ---------------------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public bool Start()
        {
            Log2.Trace("UaDataAccessor Start");
            try
            {
                string serverUrl = ConfigurationManager.AppSettings["UaReferenceServerURL"];
                if (serverUrl != null)
                {
                    Log2.Trace("{0}: UaReferenceServerURL = {1}", _myAgentObjectName, serverUrl);

                    _uaDataAccess = new UADataAccess();
                    _uaDataAccess.ServerUrl = serverUrl;
                    _uaDataAccess.Configure();
                    //---------------------------------------------------------------------------
                    // _uaDataAccessor.AddNodeId("4:Data/4:Static/4:Scalar/4:Int32Value");
                    // _uaDataAccessor.AddNodeId("4:Data/4:Static/4:Scalar/4:FloatValue");
                    // _uaDataAccessor.AddNodeId("4:Data/4:Static/4:Scalar/4:DoubleValue");
                    // _uaDataAccessor.AddNodeId("2:COM DA Server 1/2:Dynamic/2:Analog Types/2:Int");
                    // _uaDataAccessor.AddNodeId("5:Boiler1/5:FCX001/5:Measurement");

                    foreach (string prop in _myProperties)
                    {
                        string qualifiedInputProperty = ConfigurationManager.AppSettings[prop];
                        if (qualifiedInputProperty != null)
                        {
                            _uaDataAccess.AddNodeUrl(prop, qualifiedInputProperty);
                            Log2.Trace("{0}: Agent UA DataAccess Data = {1}", _myAgentObjectName, qualifiedInputProperty);
                            //---------------------------------------------------------------------------
                            //_uaDataAccessor.AddNodeUrl("uanode://UAReferenceServerURL/5:Boiler1/5:FCX001/5:Measurement?5=http://opcfoundation.org/UA/Sample/");
                            //_uaDataAccessor.AddNodeUrl("uanode://UAReferenceServerURL/5:Boiler1/5:FCX001/5:Measurement?5=http://opcfoundation.org/UA/Sample/");
                            //_uaDataAccessor.AddNodeUrl("uanode://UAReferenceServerURL/4:Data/4:Static/4:Scalar/4:Int32Value?4=http://opcfoundation.org/UA/Test/");
                            //_uaDataAccessor.AddNodeUrl("uanode://UAReferenceServerURL/4:Data/4:Static/4:Scalar/4:FloatValue?4=http://opcfoundation.org/UA/Test/");
                            //_uaDataAccessor.AddNodeUrl("uanode://UAReferenceServerURL/4:Data/4:Static/4:Scalar/4:DoubleValue?4=http://opcfoundation.org/UA/Test/");
                            //---------------------------------------------------------------------------
                        }

                    }
                    //_uaDataAccess.Run();
                }
                else
                {
                    Log2.Error("{0}: UA Server NOT Specified in Configuration => UAReferenceServerURL", _myAgentObjectName);
                }

            }
            catch (Exception Ex)
            {
                Log2.Error("{0}: Exception in StartUAService: {1}", _myAgentObjectName, Ex.ToString());

            }
            return true;
        }


        // ---------------------------------------------------------------------------------------------------
        public bool Fire()
        {
            if (_activeState)
            {
                string qualifiedInputProperty = null;
                string qualifiedInputPropertyValue = null;
                string readbackValue = null;

                try
                {
                    //````````````````````````````````````````````````````````````````````````````````````````````
                    foreach (string prop in _myProperties)
                    {
                        PropertyInfo propInfo = _myType.GetProperty(prop);
                        Log2.Trace("{0}: Agent UA Input Property {1}", _myAgentObjectName, prop);
                        // Check Units
                        object[] attributes = propInfo.GetCustomAttributes(typeof(TypeAttribute), false);
                        if (attributes != null && attributes.Length > 0)
                        {
                            TypeAttribute type = (TypeAttribute)attributes[0];
                            Log2.Trace(
                                "The type for " + _myAgentObjectName + "." + prop + " = " + type.TypeString);
                        }

                        qualifiedInputProperty = ConfigurationManager.AppSettings[prop];
                        if (qualifiedInputProperty != null)
                        {
                            Log2.Trace("{0}: Agent UA Input Data = {1}", _myAgentObjectName, qualifiedInputProperty);
                            //TODO
                            //qualifiedInputPropertyValue = _uaDataAccess.GetDataValue(prop);
                            //qualifiedInputPropertyValue = _uaDataAccessor.GetDataValue(qualifiedInputProperty);
                            if (qualifiedInputPropertyValue != null)
                            {
                                Log2.Trace("{0}: Agent UA Input GetQualifiedPropertyValue: {1}", _myAgentObjectName, qualifiedInputPropertyValue);
                                
                                //TODO
                                // Set Time, Quality and Status if Variable

                                DataVariable var = (DataVariable)propInfo.GetValue(_myAgentObject, null);
                                var.Value = qualifiedInputPropertyValue;
                                DateTime timeStamp;
                                if (DateTime.TryParse(_uaDataAccess.GetDataTime(prop), out timeStamp))
                                {
                                    var.UpdateTime = timeStamp;
                                }
                                else
                                {
                                    var.UpdateTime = DateTime.Now;
                                }

                                //var.Quality = _uaDataAccess.GetDataQuality(prop);
                                //var.Status = _uaDataAccess.GetDataStatus(prop);

                                //propInfo.SetValue(_myAgentObject, var, null);

                                //TODO
                                //var = (DataVariable)propInfo.GetValue(_myAgentObject, null);
                                //readbackValue = var.Value;
                                readbackValue = "69";
                                Log2.Trace("{0}: Agent UA Input Readback Value: {1}", _myAgentObjectName, readbackValue);
                            }
                            else
                            {
                                Log2.Error("{0}: Agent UA DataAccess Property NOT in Database: {1}", _myAgentObjectName, prop);
                            }
                        }
                        else
                        {
                            Log2.Error("{0}: Agent UA DataAccess Property NOT in App.Config: {1}", _myAgentObjectName, prop);
                        }
                    }
                }
                catch (Exception Ex)
                {
                    Log2.Error("{0}: Exception in AccessUAData: {1}", _myAgentObjectName, Ex.ToString());

                }
            }
            return true;
        }

        // ---------------------------------------------------------------------------------------------------
        public bool Stop()
        {
            if (_activeState)
            {
                try
                {
                    //_uaDataAccess.Close();
                }
                catch (Exception Ex)
                {
                    Log2.Error("{0}: Exception in StopUAService: {1}", _myAgentObjectName, Ex.ToString());

                }
            }
            _activeState = false;
            return true;
        }

        // ---------------------------------------------------------------------------------------------------
        /// <summary>
        /// Private State Variables
        /// </summary>
        /// 
        private string _myAgentClassName = null;
        private string _myAgentObjectName = null;
        private object _myAgentObject = null;
        //private TraceSwitch _myTraceSwitch = Upperbay.Core.Logging.Log2.AgentSwitch;

        private bool _activeState = false;
        private ArrayList _myProperties = null;
        private Type _myType = null;

        private string _attributeString = "dataaccess";
        private UADataAccess _uaDataAccess = null;

    }
}
