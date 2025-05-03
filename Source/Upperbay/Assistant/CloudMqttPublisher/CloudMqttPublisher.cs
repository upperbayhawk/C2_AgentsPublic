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
using Upperbay.Worker.LMP;
using Upperbay.Assistant;

/// <summary>
/// TODO Rename to GameAssistant
/// </summary>

namespace Upperbay.Assistant
{
    public class CloudMqttPublisher : IAgentObjectAssistant
    {

        public AgentPassPort AgentPassPort { get { return this._agentPassPort; } set { this._agentPassPort = value; } }

        #region Methods

        public CloudMqttPublisher()
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

 
                        string mqttCloudEnable = MyAppConfig.GetParameter("MqttCloudEnable");
                        if (mqttCloudEnable.Equals("true"))
                        {
                            _bCloudEnable = true;
                            string cluster = MyAppConfig.GetParameter("ClusterName");
                            string mqttCloudIpAddress = MyAppConfig.GetClusterParameter(cluster,"MqttCloudIpAddress");
                            string mqttCloudLoginName = MyAppConfig.GetParameter("MqttCloudLoginName");
                            string mqttCloudPassword = MyAppConfig.GetParameter("MqttCloudPassword");
                            string mqttCloudPort = MyAppConfig.GetClusterParameter(cluster,"MqttCloudPort");
                     
                            MqttCloudDriver.MqttInitializeAsync(mqttCloudIpAddress,
                                                               mqttCloudLoginName,
                                                               mqttCloudPassword,
                                                               int.Parse(mqttCloudPort));
                            
                            MqttCloudDriver.MqttSubscribeAsync(TOPICS.GAME_START_TOPIC);

                            Log2.Debug("MqttPublisher: MqttCloudDriver.MqttInitializeAsync Completed");
                        }

                        _activeState = true;
                    }
                    else
                    {
                        Log2.Trace("CloudMqttPublisher: No MqttPublisher Attributes");
                    }
                }
                else
                {
                    Log2.Error("CloudMqttPublisher Start MqttPublisher Failed!");
                }
            }
            catch (Exception Ex)
            {
                Log2.Error("CloudMqttPublisher Start MqttPublisher Exception: {0}", Ex.ToString());
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
                    //string json;
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
                        }
                        ////TODO VAR
                        DataVariable var = (DataVariable)propInfo.GetValue(_myAgentObject, null);
                        // _agentData.UpdatePropertyValue(_myAgentObjectName, prop, var.Value, var.Quality, var.UpdateTime);
                    }
                }
                catch (Exception Ex)
                {
                    Log2.Error("{0}: Exception in PublishPropertyData: {1}", _myAgentObjectName, Ex.ToString());
                }

                ////////////////////////////////////////////////////////////////////////////////////////
                ///FIVEMINUTETESTSUITE
                ////////////////////////////////////////////////////////////////////////////////////////
                
                string fiveMinuteTestingEnabled = MyAppConfig.GetParameter("FiveMinuteTestingEnabled");
                if ((fiveMinuteTestingEnabled.Equals("true") && (_bCloudEnable)))
                {
                    // fire every ten minutes
                    int currentMinute = DateTime.Now.Minute;
                    int remainder = currentMinute % 10;
                    if ((remainder == 0) && (!_b10mRan))
                    {
                        _b10mRan = true;
                        Log2.Debug("10Min Test Fired: {0}", DateTime.Now.ToShortTimeString());

                        RealTimeLMP realTimeLMP = new RealTimeLMP();
                        double lmp = realTimeLMP.GetRealTimeLMP();
                        string color = realTimeLMP.GetRealTimeLMPColor(lmp);
                        double ppw = realTimeLMP.GetRealTimeLMPPointsPerWatt(lmp);
                        Log2.Debug("10 min LMP: {0}, {1}, {2}", lmp, color, ppw);

                        GameEventVariable gameEventVariable = new GameEventVariable();
                        gameEventVariable.GameName = "Current For Carbon";
                        gameEventVariable.GameType = "SHEDPOWER";
                        gameEventVariable.GridZone = "ALL";
                        gameEventVariable.GameAwardRank = "GOLD";
                        gameEventVariable.DurationInMinutes = 5;
                        gameEventVariable.StartTime = DateTime.Now.AddMinutes(1);
                        gameEventVariable.EndTime = gameEventVariable.StartTime.AddMinutes(gameEventVariable.DurationInMinutes);
                        gameEventVariable.DollarPerPoint = 0.0025;
                        gameEventVariable.PointsPerWatt = 1.00;
                        gameEventVariable.PointsPerPercent = 0.064;

                        string hash = Utilities.ComputeSha256Hash(gameEventVariable.GameName + gameEventVariable.StartTime + gameEventVariable.DurationInMinutes);
                        gameEventVariable.GameId = hash;

                        JsonGameEventVariable jsonGameEventVariable = new JsonGameEventVariable();
                        string gameEventVariableJson = jsonGameEventVariable.GameEventVariable2Json(gameEventVariable);

                        Log2.Trace("10 min MQTT Game Event Message Sent: {0}", gameEventVariableJson);
                        MqttCloudDriver.MqttPublishAsync(TOPICS.GAME_START_TOPIC, gameEventVariableJson);

                        GameLedger gameLedger = new GameLedger();
                        gameLedger.AddEvent(gameEventVariable);
                    }
                    else if (remainder != 0)
                    {
                        _b10mRan = false;
                    }
                }
            

                ////////////////////////////////////////////////////////////////////////////////////////
                // BASE TEST SUITE
                ////////////////////////////////////////////////////////////////////////////////////////
                string testingEnabled = MyAppConfig.GetParameter("TestingEnabled");
                if ((testingEnabled.Equals("true") && (_bCloudEnable)))
                {
                   
                    // >= 9 <9
                    if ((DateTime.Now.Hour >= 21) || (DateTime.Now.Hour <= 8)) // Night Run
                    {
                        //Log2.Info("Test Time: {0}", DateTime.Now.ToShortTimeString());


                        // 1 hr at 9:35 - 10:35 
                        if ((DateTime.Now.Hour == 21) && (DateTime.Now.Minute == 30) && !_b1hrRan)
                        {
                            _b1hrRan = true;
                            _count++;
                            Log2.Debug("1hr Test Fired: {0}", DateTime.Now.ToShortTimeString());

                            RealTimeLMP realTimeLMP = new RealTimeLMP();
                            double lmp = realTimeLMP.GetRealTimeLMP();
                            string color = realTimeLMP.GetRealTimeLMPColor(lmp);
                            double ppw = realTimeLMP.GetRealTimeLMPPointsPerWatt(lmp);
                            Log2.Debug("1hr LMP: {0}, {1}, {2}", lmp, color, ppw);

                            GameEventVariable gameEventVariable = new GameEventVariable();
                            gameEventVariable.GameName = "Current For Carbon";
                            gameEventVariable.GameType = "SHEDPOWER";
                            gameEventVariable.GridZone = "ALL";
                            gameEventVariable.GameAwardRank = color;
                            gameEventVariable.DurationInMinutes = 60;
                            gameEventVariable.StartTime = DateTime.Now.AddMinutes(5);
                            gameEventVariable.EndTime = gameEventVariable.StartTime.AddMinutes(gameEventVariable.DurationInMinutes);
                            gameEventVariable.DollarPerPoint = 0.0025;
                            gameEventVariable.PointsPerWatt = ppw;
                            gameEventVariable.PointsPerPercent = 0.064;

                            string hash = Utilities.ComputeSha256Hash(gameEventVariable.GameName + gameEventVariable.StartTime + gameEventVariable.DurationInMinutes);
                            gameEventVariable.GameId = hash;

                            JsonGameEventVariable jsonGameEventVariable = new JsonGameEventVariable();
                            string gameEventVariableJson = jsonGameEventVariable.GameEventVariable2Json(gameEventVariable);

                            Log2.Debug("1 hr MQTT Game Event Message Sent: {0}", gameEventVariableJson);
                            MqttCloudDriver.MqttPublishAsync(TOPICS.GAME_START_TOPIC, gameEventVariableJson);

                            GameLedger gameLedger = new GameLedger();
                            gameLedger.AddEvent(gameEventVariable);


                        }
                        else if ((DateTime.Now.Hour == 21) && (DateTime.Now.Minute == 31))
                        {
                            _b1hrRan = false;
                        }




                        // 2 hr at 10:40 - 10:45 - 12:45p
                        if ((DateTime.Now.Hour == 22) && (DateTime.Now.Minute == 40) && !_b2hrRan)
                        {
                            _count++;
                            _b2hrRan = true;
                            Log2.Debug("2 Hr Test Fired: {0}", DateTime.Now.ToShortTimeString());

                            RealTimeLMP realTimeLMP = new RealTimeLMP();
                            double lmp = realTimeLMP.GetRealTimeLMP();
                            string color = realTimeLMP.GetRealTimeLMPColor(lmp);
                            double ppw = realTimeLMP.GetRealTimeLMPPointsPerWatt(lmp);
                            Log2.Debug("2hr LMP: {0}, {1}, {2}", lmp, color, ppw);

                            GameEventVariable gameEventVariable = new GameEventVariable();
                            gameEventVariable.GameName = "Current For Carbon";
                            gameEventVariable.GameType = "SHEDPOWER";
                            gameEventVariable.GridZone = "ALL";
                            gameEventVariable.GameAwardRank = color;
                            gameEventVariable.DurationInMinutes = 120;
                            gameEventVariable.StartTime = DateTime.Now.AddMinutes(5);
                            gameEventVariable.EndTime = gameEventVariable.StartTime.AddMinutes(gameEventVariable.DurationInMinutes);
                            gameEventVariable.DollarPerPoint = 0.0015;
                            gameEventVariable.PointsPerWatt = ppw;
                            gameEventVariable.PointsPerPercent = 0.064;

                            string hash = Utilities.ComputeSha256Hash(gameEventVariable.GameName + gameEventVariable.StartTime + gameEventVariable.DurationInMinutes);
                            gameEventVariable.GameId = hash;


                            JsonGameEventVariable jsonGameEventVariable = new JsonGameEventVariable();
                            string gameEventVariableJson = jsonGameEventVariable.GameEventVariable2Json(gameEventVariable);

                            Log2.Debug("2 hr MQTT Game Event Message Sent: {0}", gameEventVariableJson);
                            MqttCloudDriver.MqttPublishAsync(TOPICS.GAME_START_TOPIC, gameEventVariableJson);

                            GameLedger gameLedger = new GameLedger();
                            gameLedger.AddEvent(gameEventVariable);

                        }
                        else if ((DateTime.Now.Hour == 22) && (DateTime.Now.Minute == 41))
                        {
                            _b2hrRan = false;
                        }


                        // 3 hr at 1:00 - 1:05 - 4:05
                        if ((DateTime.Now.Hour == 1) && (DateTime.Now.Minute == 0) && !_b3hrRan)
                        {
                            _b3hrRan = true;
                            _count++;
                            Log2.Debug("3 Hr Test Fired: {0}", DateTime.Now.ToShortTimeString());

                            RealTimeLMP realTimeLMP = new RealTimeLMP();
                            double lmp = realTimeLMP.GetRealTimeLMP();
                            string color = realTimeLMP.GetRealTimeLMPColor(lmp);
                            double ppw = realTimeLMP.GetRealTimeLMPPointsPerWatt(lmp);
                            Log2.Debug("3hr LMP: {0}, {1}, {2}", lmp, color, ppw);

                            GameEventVariable gameEventVariable = new GameEventVariable();
                            gameEventVariable.GameName = "Current For Carbon";
                            gameEventVariable.GameType = "SHEDPOWER";
                            gameEventVariable.GridZone = "ALL";
                            gameEventVariable.GameAwardRank = color;
                            gameEventVariable.DurationInMinutes = 180;
                            gameEventVariable.StartTime = DateTime.Now.AddMinutes(5);
                            gameEventVariable.EndTime = gameEventVariable.StartTime.AddMinutes(gameEventVariable.DurationInMinutes);
                            gameEventVariable.DollarPerPoint = 0.005;
                            gameEventVariable.PointsPerWatt = ppw;
                            gameEventVariable.PointsPerPercent = 0.064;

                            string hash = Utilities.ComputeSha256Hash(gameEventVariable.GameName + gameEventVariable.StartTime + gameEventVariable.DurationInMinutes);
                            gameEventVariable.GameId = hash;

                            JsonGameEventVariable jsonGameEventVariable = new JsonGameEventVariable();
                            string gameEventVariableJson = jsonGameEventVariable.GameEventVariable2Json(gameEventVariable);

                            Log2.Debug("3 hr MQTT Game Event Message Sent: {0}", gameEventVariableJson);
                            MqttCloudDriver.MqttPublishAsync(TOPICS.GAME_START_TOPIC, gameEventVariableJson);

                            GameLedger gameLedger = new GameLedger();
                            gameLedger.AddEvent(gameEventVariable);
                        }
                        else if ((DateTime.Now.Hour == 1) && (DateTime.Now.Minute == 1))
                        {
                            _b3hrRan = false;
                        }



                        // 4 hr at 5:00 5:05 - 9:05
                        if ((DateTime.Now.Hour == 5) && (DateTime.Now.Minute == 0) && !_b4hrRan)
                        {
                            _count++;
                            _b4hrRan = true;
                            Log2.Debug("4 Hr Test Fired: {0}", DateTime.Now.ToShortTimeString());

                            RealTimeLMP realTimeLMP = new RealTimeLMP();
                            double lmp = realTimeLMP.GetRealTimeLMP();
                            string color = realTimeLMP.GetRealTimeLMPColor(lmp);
                            double ppw = realTimeLMP.GetRealTimeLMPPointsPerWatt(lmp);
                            Log2.Debug("4 hr LMP: {0}, {1}, {2}", lmp, color, ppw);

                            GameEventVariable gameEventVariable = new GameEventVariable();
                            gameEventVariable.GameName = "Current For Carbon";
                            gameEventVariable.GameType = "SHEDPOWER";
                            gameEventVariable.GridZone = "ALL";
                            gameEventVariable.GameAwardRank = color;
                            gameEventVariable.DurationInMinutes = 240;
                            gameEventVariable.StartTime = DateTime.Now.AddMinutes(5);
                            gameEventVariable.EndTime = gameEventVariable.StartTime.AddMinutes(gameEventVariable.DurationInMinutes);
                            gameEventVariable.DollarPerPoint = 0.005;
                            gameEventVariable.PointsPerWatt = ppw;
                            gameEventVariable.PointsPerPercent = 0.064;

                            string hash = Utilities.ComputeSha256Hash(gameEventVariable.GameName + gameEventVariable.StartTime + gameEventVariable.DurationInMinutes);
                            gameEventVariable.GameId = hash;

                            JsonGameEventVariable jsonGameEventVariable = new JsonGameEventVariable();
                            string gameEventVariableJson = jsonGameEventVariable.GameEventVariable2Json(gameEventVariable);

                            Log2.Debug("4 hr MQTT Game Event Message Sent: {0}", gameEventVariableJson);
                            MqttCloudDriver.MqttPublishAsync(TOPICS.GAME_START_TOPIC, gameEventVariableJson);

                            GameLedger gameLedger = new GameLedger();
                            gameLedger.AddEvent(gameEventVariable);

                        }
                        else if ((DateTime.Now.Hour == 5) && (DateTime.Now.Minute == 1))
                        {
                            _b4hrRan = false;
                        }

                    }

                    ////////////////////////////////////////////////////////////////////////////////



                    if ((DateTime.Now.Hour >= 10) && (DateTime.Now.Hour <= 20)) // Day Run
                    {


                        ////10 min 1 -11 min
                        //if ((DateTime.Now.Minute == 0) && !_b10mRan)
                        //{
                        //    _count++;
                        //    Log2.Info("10 min Test Fired: {0}", DateTime.Now.ToShortTimeString());

                        //    GameEventVariable gameEventVariable = new GameEventVariable();
                        //    gameEventVariable.GameName = "Current For Carbon";
                        //    gameEventVariable.GameId = "Game" + DateTime.Now.Ticks.ToString();
                        //    gameEventVariable.StartTime = DateTime.Now.AddMinutes(1);
                        //    gameEventVariable.DollarPerPoint = 0.001;
                        //    gameEventVariable.PointsPerWatt = 1.00;
                        //    gameEventVariable.PointsPerPercent = 0.064;

                        //    gameEventVariable.DurationInMinutes = 10; //60*10 min, 60*60 = 3600
                        //    JsonGameEventVariable jsonGameEventVariable = new JsonGameEventVariable();
                        //    string gameEventVariableJson = jsonGameEventVariable.GameEventVariable2Json(gameEventVariable);

                        //    Log2.Info("MQTT Game Event Message Sent: {0}", gameEventVariableJson);
                        //    MqttCloudDriver.MqttPublishAsync(TOPICS.GAME_START_TOPIC, gameEventVariableJson);

                        //    _b10mRan = true;
                        //}
                        // 12- 22
                        if ((DateTime.Now.Minute == 0) && !_b20mRan)
                        {
                            _b20mRan = true;
                            _count++;
                            Log2.Debug("20 min Test Fired: {0}", DateTime.Now.ToShortTimeString());

                            RealTimeLMP realTimeLMP = new RealTimeLMP();
                            double lmp = realTimeLMP.GetRealTimeLMP();
                            string color = realTimeLMP.GetRealTimeLMPColor(lmp);
                            double ppw = realTimeLMP.GetRealTimeLMPPointsPerWatt(lmp);
                            Log2.Debug("20 min LMP: {0}, {1}, {2}", lmp, color, ppw);

                            GameEventVariable gameEventVariable = new GameEventVariable();
                            gameEventVariable.GameName = "Current For Carbon";
                            gameEventVariable.GameType = "HARVESTPOWER";
                            gameEventVariable.GridZone = "ALL";
                            gameEventVariable.GameAwardRank = color;
                            gameEventVariable.DurationInMinutes = 20;
                            gameEventVariable.StartTime = DateTime.Now.AddMinutes(1);
                            gameEventVariable.EndTime = gameEventVariable.StartTime.AddMinutes(gameEventVariable.DurationInMinutes);
                            gameEventVariable.DollarPerPoint = 0.0025;
                            gameEventVariable.PointsPerWatt = ppw;
                            gameEventVariable.PointsPerPercent = 0.064;

                            string hash = Utilities.ComputeSha256Hash(gameEventVariable.GameName + gameEventVariable.StartTime + gameEventVariable.DurationInMinutes);
                            gameEventVariable.GameId = hash;

                            JsonGameEventVariable jsonGameEventVariable = new JsonGameEventVariable();
                            string gameEventVariableJson = jsonGameEventVariable.GameEventVariable2Json(gameEventVariable);

                            Log2.Debug("20m MQTT Game Event Message Sent: {0}", gameEventVariableJson);
                            MqttCloudDriver.MqttPublishAsync(TOPICS.GAME_START_TOPIC, gameEventVariableJson);

                            GameLedger gameLedger = new GameLedger();
                            gameLedger.AddEvent(gameEventVariable);

                        }
                        else if ((DateTime.Now.Minute == 1) && (_b20mRan))
                        {
                            _b20mRan = false;
                        }

                            // 23
                        if ((DateTime.Now.Minute == 25) && !_b30mRan)
                        {
                            _b30mRan = true;
                            _count++;
                            Log2.Debug("30 min Test Fired: {0}", DateTime.Now.ToShortTimeString());

                            RealTimeLMP realTimeLMP = new RealTimeLMP();
                            double lmp = realTimeLMP.GetRealTimeLMP();
                            string color = realTimeLMP.GetRealTimeLMPColor(lmp);
                            double ppw = realTimeLMP.GetRealTimeLMPPointsPerWatt(lmp);
                            Log2.Debug("30 min LMP: {0}, {1}, {2}", lmp, color, ppw);

                            GameEventVariable gameEventVariable = new GameEventVariable();
                            gameEventVariable.GameName = "Current For Carbon";
                            gameEventVariable.GameType = "HARVESTPOWER";
                            gameEventVariable.GridZone = "ALL";
                            gameEventVariable.GameAwardRank = color;
                            gameEventVariable.DurationInMinutes = 30;
                            gameEventVariable.StartTime = DateTime.Now.AddMinutes(1);
                            gameEventVariable.EndTime = gameEventVariable.StartTime.AddMinutes(gameEventVariable.DurationInMinutes);
                            gameEventVariable.DollarPerPoint = 0.0015;
                            gameEventVariable.PointsPerWatt = ppw;
                            gameEventVariable.PointsPerPercent = 0.064;
                            string hash = Utilities.ComputeSha256Hash(gameEventVariable.GameName + gameEventVariable.StartTime + gameEventVariable.DurationInMinutes);
                            gameEventVariable.GameId = hash;


                            JsonGameEventVariable jsonGameEventVariable = new JsonGameEventVariable();
                            string gameEventVariableJson = jsonGameEventVariable.GameEventVariable2Json(gameEventVariable);

                            Log2.Debug("30m MQTT Game Event Message Sent: {0}", gameEventVariableJson);
                            MqttCloudDriver.MqttPublishAsync(TOPICS.GAME_START_TOPIC, gameEventVariableJson);

                            GameLedger gameLedger = new GameLedger();
                            gameLedger.AddEvent(gameEventVariable);

                        }
                        else if ((DateTime.Now.Minute == 26) && (_b30mRan))
                        {
                            _b30mRan = false;
                        }


                        //if (_timeTrigger.Is20Min(0))
                        //{//120min, 3600
                        //    _count++;
                        //    //Log2.Info("Cloud 60Min Trigger Fired: {0}", DateTime.Now.ToShortTimeString());

                        //    GameEventVariable gameEventVariable = new GameEventVariable();
                        //    gameEventVariable.GameName = "Current For Carbon";
                        //    gameEventVariable.GameId = "Game" + DateTime.Now.Ticks.ToString();
                        //    gameEventVariable.StartTime = DateTime.Now.AddMinutes(1);
                        //    gameEventVariable.DollarPerPoint = 0.001;
                        //    gameEventVariable.PointsPerWatt = 1.00;
                        //    gameEventVariable.PointsPerPercent = 0.064;

                        //    gameEventVariable.DurationInMinutes = 10; //60*10 min, 60*60 = 3600
                        //    JsonGameEventVariable jsonGameEventVariable = new JsonGameEventVariable();
                        //    string gameEventVariableJson = jsonGameEventVariable.GameEventVariable2Json(gameEventVariable);

                        //    Log2.Info("MQTT Game Event Message Sent: {0}", gameEventVariableJson);
                        //    MqttCloudDriver.MqttPublishAsync(TOPICS.GAME_START_TOPIC, gameEventVariableJson);
                        //}

                        // reset night flags after all tests ran

                    }
                }
            ///////////////////////////////////////////////////////////////////////////

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
                        }

                        ////TODO VAR

                        DataVariable var = (DataVariable)propInfo.GetValue(_myAgentObject, null);
                       // _agentData.RemoveAgentProperty(_myAgentObjectName, prop);

                    }
                    // _agentData.RemoveAgent(_myAgentObjectName);

                    if (_bCloudEnable)
                        MqttCloudDriver.MqttStopAsync();
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

        private TimeTrigger _timeTrigger = new TimeTrigger();
        private bool _bCloudEnable = false;

        private int _count = 0;

        private string _myAgentClassName = null;
        private string _myAgentObjectName = null;
        private object _myAgentObject = null;

        private bool _activeState = false;
        private ArrayList _myProperties = null;
        private Type _myType = null;

        private string _attributeString = "publish";

        //public AgentData _agentData = null;

        bool _b1hrRan = false;
        bool _b2hrRan = false;
        bool _b3hrRan = false;
        bool _b4hrRan = false;

        //bool _b10mRan = false;
        bool _b20mRan = false;
        bool _b30mRan = false;
        bool _b10mRan = false;

        #endregion

    }
}


