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

using Upperbay.Core.Logging;
using Upperbay.Core.Library;
using Upperbay.Agent.Interfaces;


namespace Upperbay.Assistant
{
    public class Simulator : IAgentObjectAssistant
    {

        public AgentPassPort AgentPassPort { get { return this._agentPassPort; } set { this._agentPassPort = value; } }

        #region Methods
        public Simulator()
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

                    Log2.Trace("{0} Simulator: Class: {1}", _myAgentObjectName, _myType.ToString());

                    _myProperties = Utilities.GetDecoratedProperties(_myType, _attributeString);
                    if (_myProperties != null)
                    {
                        foreach (string prop in _myProperties)
                        {
                            Log2.Trace("{0}: Simulated Attribute: {1}", _myAgentObjectName, prop);
                        }

                        _activeState = true;
                    }
                    else
                    {
                        Log2.Trace("{0}: No Simulated Attributes", _myAgentObjectName);
                    }
                }
                else
                {
                    Log2.Error("Start Simulator Failed!");
                }
            }
            catch (Exception Ex)
            {
                Log2.Error("Start Simulator Exception: {0}", Ex.ToString());
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
                    foreach (string prop in _myProperties)
                    {
                        PropertyInfo propInfo = _myType.GetProperty(prop);
                        DataVariable var = (DataVariable)propInfo.GetValue(_myAgentObject, null);
                        double currentValue = 0.0;
                        if (var.Value.Equals("NOVALUE") )
                        {
                            var.Value = "0.0";
                        }
                        if (Double.TryParse(var.Value, out currentValue))
                        {
                            currentValue = currentValue + 1;
                            if (currentValue > 1411.0)
                                currentValue = 0.0;
                            var.Quality = "Good";
                            var.LastValue = var.Value;
                            var.LastValueTime = var.UpdateTime;
                            var.ChangeFlag = true;
                            var.Value = currentValue.ToString();
                            var.UpdateTime = DateTime.Now;
                            propInfo.SetValue(_myAgentObject, var, null);
                            Log2.Trace("{0}: Agent Simulate Property {1} = {2}", _myAgentObjectName, prop, var.Value);
                        }
                    }
                }
                catch (Exception Ex)
                {
                    Log2.Error("{0}: Exception in Upperbay.AgentObject.Assistant.Simulator: {1}", _myAgentObjectName, Ex.ToString());

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
                        
                try
                {
                    foreach (string prop in _myProperties)
                    {
                        PropertyInfo propInfo = _myType.GetProperty(prop);
                        DataVariable var = (DataVariable)propInfo.GetValue(_myAgentObject, null);
                        double currentValue = 0.0;
                        if (Double.TryParse(var.Value, out currentValue))
                        {
                            var.Value = currentValue.ToString();
                            var.UpdateTime = DateTime.Now;
                            var.Quality = "Bad";
                            propInfo.SetValue(_myAgentObject, var, null);
                            Log2.Trace("{0}: Agent Simulate Quality = Bad", _myAgentObjectName);
                        }
                    }
                }
                catch (Exception Ex)
                {
                    Log2.Error("{0}: Exception in Upperbay.AgentObject.Assistant.Simulator: {1}", _myAgentObjectName, Ex.ToString());

                }
            _activeState = false;
            return true;
        }

        #endregion

        #region Private State Variables

        private AgentPassPort _agentPassPort = null;
        private string _myAgentClassName = null;
        private string _myAgentObjectName = null;
        private object _myAgentObject = null;

        private bool _activeState = false;
        private ArrayList _myProperties = null;
        private Type _myType = null;

        private string _attributeString = "simulated";
        #endregion
    }
}
