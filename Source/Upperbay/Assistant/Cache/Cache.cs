//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
using System;

using Upperbay.Core.Logging;
using Upperbay.Core.Library;
using Upperbay.Agent.Interfaces;
using Upperbay.Agent.ColonyMatrix;
//using Upperbay.Worker.Devices;

namespace Upperbay.Assistant
{
    public class Cache : IAgentObjectAssistant
    {
        public AgentPassPort AgentPassPort { get { return this._agentPassPort; } set { this._agentPassPort = value; } }

        #region Methods        
        public Cache()
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

                    Log2.Trace("{0} CacheAssistant: Class: {1}", _myAgentObjectName, _myType.ToString());

                    //_myProperties = Utilities.GetDecoratedProperties(_myType, _attributeString);
                    //if (_myProperties != null)
                    //{
                    //    foreach (string prop in _myProperties)
                    //    {
                    //        Log2.Trace("{0}: Simulated Attribute: {1}", _myAgentObjectName, prop);
                    //    }

                    try
                    {
                        // Inflate Cache

                        try
                        {
                            Log2.Trace("{0}: Initializing CacheAssistant", _myAgentObjectName);

                            //string reloadCacheOnStart = ConfigurationManager.AppSettings["ReloadCacheOnStart"];
                            //string cacheDataFile = ConfigurationManager.AppSettings["CacheDataFile"];
                            //Boolean.TryParse(reloadCacheOnStart, out bool bReload);
                            //if (bReload)
                            //{
                            //    DataVariableCache.LoadCache(cacheDataFile);
                            //}
                        }
                        catch (Exception Ex)
                        {
                            Log2.Error("Exception in  CacheAssistant Initialize: {0}", Ex.ToString());
                        }
                        //````````````````````````````````````````````````````````````````````````````````
                        //int j = 0;
                        try
                        {
                           
                        }
                        catch (Exception Ex)
                        {
                            Log2.Error("Exception in CacheAssistant Start: {0}", Ex.ToString());
                        }
                    }
                    catch (Exception Ex)
                    {
                        Log2.Error("{0}: Exception in CacheAssistant Start: {1}", _myAgentObjectName, Ex.ToString());

                    }
                    
                    _activeState = true;
                }
                else
                {
                    Log2.Error("Start CacheAssistant Failed!");
                }
            }
            catch (Exception Ex)
            {
                Log2.Error("Start CacheAssistant Exception: {0}", Ex.ToString());
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
                try
                {
                }
                catch (Exception Ex)
                {
                    Log2.Error("{0}: Exception in CacheAssistant Start: {1}", _myAgentObjectName, Ex.ToString());

                }
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
                    DataVariableCache.ProcessCache();
                }
                catch (Exception Ex)
                {
                    Log2.Error("{0}: Exception in CacheAssistant Fire: {1}", _myAgentObjectName, Ex.ToString());

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
                    DataVariableCache.DumpCache();
                    //foreach (BaseDevice dev in _dataFarm.Devices)
                    //{

                    //}

                    //Log2.Trace("{0}: Stopping DataFarm.", _myAgentObjectName);
                    //_dataFarm.Stop();
                }
                catch (Exception Ex)
                {
                    Log2.Error("{0}: Exception in CacheAssistant Stop: {1}", _myAgentObjectName, Ex.ToString());

                }
            }
            _activeState = false;
            return true;
        }
        #endregion

        #region Private State Variables

        private string _myAgentClassName = null;
        private string _myAgentObjectName = null;
        private object _myAgentObject = null;
        private bool _activeState = false;
        private Type _myType = null;
        private AgentPassPort _agentPassPort = null;

        #endregion
    }
}
