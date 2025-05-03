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
using Upperbay.Agent.ColonyMatrix;

namespace Upperbay.Assistant
{
    public class GameEventReactor : IAgentObjectAssistant
    {
        
        public AgentPassPort AgentPassPort { get { return this._agentPassPort; } set { this._agentPassPort = value; } }

        #region Methods
        public GameEventReactor()
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

                    Log2.Trace("Game EventReactor: {0} Class: {1}", _myAgentObjectName, _myType.ToString());

                    _myEventProperties = Utilities.GetDecoratedProperties(_myType, _eventAttributeString);
                    if (_myEventProperties != null)
                    {
                        foreach (string prop in _myEventProperties)
                        {
                            Log2.Trace("Game Event Attribute:{0} {1}", _myAgentObjectName, prop);
                        }
                    }

                    _myActionProperties = Utilities.GetDecoratedProperties(_myType, _actionAttributeString);
                    if (_myActionProperties != null)
                    {
                        foreach (string prop in _myActionProperties)
                        {
                            Log2.Trace("Game Event Attribute: {0} {1}", _myAgentObjectName, prop);
                        }
                    }
                    // Force Active
                    _activeState = true;
               }
                else
                {
                    Log2.Error("Start GameEventReactor Failed!");
                }
            }
            catch (Exception Ex)
            {
                Log2.Error("Start GameEventReactor Exception: {0}", Ex.ToString());
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
                            Log2.Trace("Event Attribute: {0} {1}", _myAgentObjectName, prop);
                        }
                    }
                    Log2.Trace("Starting GameEventReactor {0}", _myAgentObjectName);
                }
                catch (Exception Ex)
                {
                    Log2.Error("Exception in GameEventReactor Start: {0} {1}", _myAgentObjectName, Ex.ToString());
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
                    string adminOnly = MyAppConfig.GetParameter("AdminOnly");
                    bool bAdminOnly;
                    if (!bool.TryParse(adminOnly, out bAdminOnly))
                        bAdminOnly = false;
                    GameEventVariable gev = null;
                    while ((gev = GameEventVariableCache.ReadEventQueue()) != null)
                    {
                        if (!bAdminOnly)
                        {
                            //Process Event by Type
                            //The last one wins if more than one in queue
                            switch (gev.GameType)
                            {
                                case ("SHEDPOWER"):
                                    if (gev.StartTime > DateTime.Now)
                                    {
                                        //Insert PlayId into Game Event Variable
                                        string gamePlayerId = MyAppConfig.GetParameter("GamePlayerID");
                                        gev.GamePlayerId = gamePlayerId;

                                        _gameEvent = new GameEvent(gev);
                                        GameAction gameAction = new GameAction(_gameEvent.GameEventVariable);
                                        gameAction.ReceivedShedAction();
                                    }
                                    break;
                                case ("HARVESTPOWER"):
                                    if (gev.StartTime > DateTime.Now)
                                    {
                                        //Insert PlayId into Game Event Variable
                                        string gamePlayerId = MyAppConfig.GetParameter("GamePlayerID");
                                        gev.GamePlayerId = gamePlayerId;

                                        _gameEvent = new GameEvent(gev);
                                        GameAction gameAction = new GameAction(_gameEvent.GameEventVariable);
                                        gameAction.ReceivedHarvestAction();
                                    }
                                    break;
                                default:
                                    Log2.Error("GameType NOT Recognized: {0}", gev.GameType);
                                    break;
                            }
                        }
                    }

                    if (_gameEvent != null)
                    {
                        if ((!_gameEvent.GameRunning) && (!_gameEvent.GameFinished))
                        {
                            if (_gameEvent.Start())
                            {
                                GameAction gameAction = new GameAction(_gameEvent.GameEventVariable);
                                if (_gameEvent.GameEventVariable.GameType == "SHEDPOWER")
                                    gameAction.StartShedAction();
                                else if (_gameEvent.GameEventVariable.GameType == "HARVESTPOWER")
                                    gameAction.StartHarvestAction();
                            }
                        }

                        if ((_gameEvent.GameRunning) && (!_gameEvent.GameFinished))
                        {
                            if (_gameEvent.IsFinished())
                            {
                                GameAction gameAction = new GameAction(_gameEvent.GameEventVariable);
                                if (_gameEvent.GameEventVariable.GameType == "SHEDPOWER")
                                    gameAction.FinishShedAction();
                                else if (_gameEvent.GameEventVariable.GameType == "HARVESTPOWER")
                                    gameAction.FinishHarvestAction();
                            }
                        }
                    }
                }
                catch (Exception Ex)
                {
                    Log2.Error("Exception in GameEventReactor Start: {0} {1}", _myAgentObjectName, Ex.ToString());
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
                    Log2.Error("Exception in GameEventReactor Stop: {0} {1}", _myAgentObjectName, Ex.ToString());

                }
            }
            _activeState = false;
            return true;
        }
        #endregion

        #region Private State Variables
        
        private AgentPassPort _agentPassPort = null;
        private GameEvent _gameEvent = null;

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
