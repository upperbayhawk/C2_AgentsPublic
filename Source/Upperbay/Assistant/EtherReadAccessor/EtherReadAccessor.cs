//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
using System;
using System.Collections;
using System.Configuration;

using Upperbay.Core.Logging;
using Upperbay.Core.Library;
using Upperbay.Agent.Interfaces;
using Upperbay.Worker.EtherAccess;


namespace Upperbay.Assistant
{
    public class EtherReadAccessor : IAgentObjectAssistant
    {
        public AgentPassPort AgentPassPort { get { return this._agentPassPort; } set { this._agentPassPort = value; } }


        #region Methods

        public EtherReadAccessor()
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
            _numberOfProperties = 0;
            Log2.Trace("{0} EtherReadTestAccessor Initialize", _myAgentObjectName);
            try
            {
                if ((myAgentClassName != null) && (myAgentObject != null) && (myAgentObjectName != null))
                {
                    _myAgentClassName = myAgentClassName;
                    _myAgentObjectName = myAgentObjectName;
                    _myAgentObject = myAgentObject;

                    _myType = _myAgentObject.GetType();

                    Log2.Trace("EtherReadTestAccessor: Class: {0}", _myType.ToString());

                    _myProperties = Utilities.GetDecoratedProperties(_myType, _etherReadTestAttributeString);
                    if (_myProperties != null)
                    {
                        foreach (string prop in _myProperties)
                        {
                            _numberOfProperties++;
                            Log2.Trace("EtherReadTestAccessor Attribute: {0}", prop);
                        }
                        //FOUND ONE!
                        _activeState = true;
                    }
                    else
                    {
                        //Log2.Trace("{0}: No EtherReadTestAccessor Attributes", _myAgentObjectName);
                    }
                }
                else
                {
                    //Log2.Error("Start EtherReadTestAccessor Failed!");
                }
            }
            catch (Exception Ex)
            {
                Log2.Error("Start EtherReadTestAccessor Exception: {0}", Ex.ToString());
            }
            Log2.Trace("Number of Tags Found: {0}", _numberOfProperties);
            return true;
        }
       

        /// <summary>
        /// 
        /// </summary>
        public bool Start()
        {
            Log2.Trace("{0}: EtherReadTest Start", _myAgentObjectName); 
            try
            {
                _serverUrl = MyAppConfig.GetParameter("EthereumServerURL");

                if (_serverUrl != null)
                {
                    Log2.Trace("EthereumServerURL {0}", _serverUrl);
                    _etherTestAccess = new EtherAccess();
                    

                    string privateKey = MyAppConfig.GetParameter("EthereumExternalPrivateKey");
                    if (privateKey != null)
                    {
                        _externalPrivateKey = privateKey;
                        Log2.Trace("EthereumExternalPrivateKey {0}", _externalPrivateKey);

                        string cluster = MyAppConfig.GetParameter("ClusterName");
                        string contractAddress  = MyAppConfig.GetClusterParameter(cluster,"EthereumContractAddress");
                        string externalAddress = MyAppConfig.GetParameter("EthereumExternalAddress");
                        string sChainId = MyAppConfig.GetClusterParameter(cluster,"EthereumChainId");
                        
                        OracleName = MyAppConfig.GetParameter("EthereumOracleName");
                        
                        if ((contractAddress != null) && (externalAddress != null))
                        {
                            _contractAddress = contractAddress;
                            _externalAddress = externalAddress;
                            _chainId = Int32.Parse(sChainId);
                            Log2.Trace("EthereumOracleName {0}", OracleName);
                            Log2.Trace("EthereumContractAddress {0}", _contractAddress);
                            Log2.Trace("EthereumExternalAddress {0}", _externalAddress);
                            Log2.Trace("EthereumChainId {0}", _chainId.ToString());
                            //------------------------------------------------------
                            _etherTestAccess = new EtherAccess();
                            _etherTestAccess.ConfigureOracle(OracleName, 
                                                            _serverUrl, 
                                                            _contractAddress, 
                                                            _externalAddress, 
                                                            _externalPrivateKey,
                                                            _chainId);
                            _etherTestAccess.DockOracle().Wait();

                            foreach (string prop in _myProperties)
                            {
                                string qualifiedInputProperty = MyAppConfig.GetParameter(prop);
                                if (qualifiedInputProperty != null)
                                {
                                    //_uaDataAccess.AddNodeUrl(prop, qualifiedInputProperty);
                                    Log2.Trace("Agent Ether DataAccess Data = {0}", qualifiedInputProperty);
                                }
                            }
                        }
                        else
                        {
                            Log2.Error("EthereumContractAddress or EthereumExternalAddress Not Found");
                        }
                    }
                    else
                    {
                        Log2.Error("EthereumExternalPrivateKey Not Found");
                    }
                }
                else
                {
                    Log2.Error("EthereumServerURL Not Found");
                }

            }
            catch (Exception Ex)
            {
                Log2.Error("Exception in EtherReadTestAccessor {0}", Ex.ToString());

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
                //string qualifiedInputProperty = null;
                //string qualifiedInputPropertyValue = null;
                //string readbackValue = null;
                //DataVariable dataVar;

                Log2.Trace("EtherRead Fire");

                _etherTestAccess.PingOracle().Wait();

                _etherTestAccess.SendEtherToContract().Wait();

               // _etherTestAccess.GetEvents().Wait();
               

                //Log2.Trace("calling TS GetValue");
                //readbackValue = _tsDataAccess.GetValue();
                //Log2.Trace("TS GetValue {0}", readbackValue);

                //                try
                //                {
                //                    //````````````````````````````````````````````````````````````````````````````````````````````
                //                    foreach (string prop in _myProperties)
                //                    {
                //                        PropertyInfo propInfo = _myType.GetProperty(prop);
                //                        Log2.Trace("Agent Ether Input Property {0}", prop);
                //                        // Check Units
                //                        object[] attributes = propInfo.GetCustomAttributes(typeof(TypeAttribute), false);
                //                        if (attributes != null && attributes.Length > 0)
                //                        {
                //                            TypeAttribute type = (TypeAttribute)attributes[0];
                //                            Log2.Trace(
                //                                "The type for " + _myAgentObjectName + "." + prop + " = " + type.TypeString);
                //                        }

                //                        qualifiedInputProperty = ConfigurationManager.AppSettings[prop];
                //                        if (qualifiedInputProperty != null)
                //                        {
                //                            Log2.Trace("Agent Ether Input Data = {0}", qualifiedInputProperty);
                //                            //TODO
                //                            dataVar = _tsDataAccess.GetValue(qualifiedInputProperty);
                //                            //qualifiedInputPropertyValue = _tsDataAccess.GetValue(qualifiedInputProperty);
                //                            if (dataVar.Value != null)
                //                            {
                ////                                Log2.Trace("{0}: Agent TS Input GetQualifiedPropertyValue: {1}", _myAgentObjectName, qualifiedInputPropertyValue);
                //                                Log2.Trace("Agent Ether Input GetQualifiedPropertyValue: {0}",dataVar.Value);
                //                                //TODO
                //                                // Set Time, Quality and Status if Variable

                //                                DataVariable var = (DataVariable)propInfo.GetValue(_myAgentObject, null);

                //                                DateTime timeStamp;
                //                                   var.Value = dataVar.Value;
                //                                var.Status = dataVar.Status;
                //                                var.Quality = dataVar.Quality;
                //                                var.InternalName = prop;
                //                                var.ExternalName = dataVar.ExternalName;
                //                                var.UpdateTime = dataVar.UpdateTime;
                //                                if (var.Value.Equals(var.LastValue) == false)
                //                                {
                //                                    var.LastValue = var.Value;
                //                                    var.LastValueTime = var.UpdateTime;
                //                                    var.ChangeFlag = true;
                //                                }
                //                                propInfo.SetValue(_myAgentObject, var, null);

                //                                //TODO
                //                               // var = (DataVariable)propInfo.GetValue(_myAgentObject, null);
                //                                readbackValue = var.Value;

                //                                Log2.Trace("Agent Ether Input Readback Value: {0}", readbackValue);

                //                                Thread.Sleep(1000);
                //                            }
                //                            else
                //                            {
                //                                Log2.Error("Agent Ether DataAccess Property NOT in Database: {0}", prop);
                //                            }
                //                        }
                //                        else
                //                        {
                //                            Log2.Error("Agent Ether DataAccess Property NOT in App.Config: {0}", prop);
                //                        }
                //                    }
                //                }
                //                catch (Exception Ex)
                //                {
                //                    Log2.Error("Exception in AccessEtherData: {0}", Ex.ToString());

                //                }
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
                    _etherTestAccess.UndockOracle().Wait();
                    //_tsDataAccess.Close();
                }
                catch (Exception Ex)
                {
                    Log2.Error("Exception in StopEtherService: {0}", Ex.ToString());

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
        private int _numberOfProperties = 0;
        private string _etherReadTestAttributeString = "ether_read_test";
        private EtherAccess _etherTestAccess = null;

        private string _externalPrivateKey = null;
        private string _externalAddress = null;
        private string _contractAddress = null;
        private int _chainId = 3; //Rinkeby testnet
        private string _serverUrl = null;
        private string OracleName = null;

        #endregion
    }
}
