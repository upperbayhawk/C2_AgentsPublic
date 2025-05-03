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

using Upperbay.Core.Logging;
using Upperbay.Core.Library;
using Upperbay.Agent.Interfaces;
using Upperbay.Agent.ColonyMatrix;

namespace Upperbay.Assistant
{
    public class EventReactor : IAgentObjectAssistant
    {
        
        public AgentPassPort AgentPassPort { get { return this._agentPassPort; } set { this._agentPassPort = value; } }

        #region Methods
        public EventReactor()
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

                    Log2.Trace("{0} EventReactor: Class: {1}", _myAgentObjectName, _myType.ToString());

                    _myEventProperties = Utilities.GetDecoratedProperties(_myType, _eventAttributeString);
                    if (_myEventProperties != null)
                    {
                        foreach (string prop in _myEventProperties)
                        {
                            Log2.Trace("{0}: Event Attribute: {1}", _myAgentObjectName, prop);
                        }
                    }

                    _myActionProperties = Utilities.GetDecoratedProperties(_myType, _actionAttributeString);
                    if (_myActionProperties != null)
                    {
                        foreach (string prop in _myActionProperties)
                        {
                            Log2.Trace("{0}: Event Attribute: {1}", _myAgentObjectName, prop);
                        }
                    }
                    // Force Active
                    _activeState = true;
               }
                else
                {
                    Log2.Error("Start EventReactor Failed!");
                }
            }
            catch (Exception Ex)
            {
                Log2.Error("Start EventReactor Exception: {0}", Ex.ToString());
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
                    if (_myEventProperties != null)
                    {
                        foreach (string prop in _myEventProperties)
                        {
                            Log2.Trace("{0}: Event Attribute: {1}", _myAgentObjectName, prop);
                        }
                    }
                    Log2.Trace("{0}: Starting EventReactor.", _myAgentObjectName);
                }
                catch (Exception Ex)
                {
                    Log2.Error("{0}: Exception in EventReactor Start: {1}", _myAgentObjectName, Ex.ToString());
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

                    EventVariable ev = null;
                    while ((ev = EventVariableCache.ReadEventQueue()) != null)
                    {
                        //Process Event by Type
                        switch (ev.EventType)
                        {
                            case ("ADR"):

                                break;
                            case ("ALARM"):

                                break;
                            default:

                                break;
                        }
                        //Log event to file
                        Log2.Trace("Event Reacted: {0} {1}", ev.EventName, ev.EventType);
                    }
                }
                catch (Exception Ex)
                {
                    Log2.Error("{0}: Exception in EventReactor Start: {1}", _myAgentObjectName, Ex.ToString());
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
                
                }
                catch (Exception Ex)
                {
                    Log2.Error("{0}: Exception in EventReactor Stop: {1}", _myAgentObjectName, Ex.ToString());

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
        private Type _myType = null;

        private string _eventAttributeString = "event";
        private ArrayList _myEventProperties = null;
        private string _actionAttributeString = "action";
        private ArrayList _myActionProperties = null;
        #endregion
    }
}
