//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Upperbay.Agent.Interfaces;
using Upperbay.Core.Library;
using Upperbay.Core.Logging;
using Upperbay.Worker.JSON;
using Upperbay.Worker.MQTT;

namespace Upperbay.Assistant
{
    public static class GameStarter
    {
        public static bool StartGame ()
        {
            // Cloud MQTT already initialized
            //Log2.Info("Starting Game");

            //string cluster = MyAppConfig.GetParameter("ClusterName");
            //string mqttCloudIpAddress = MyAppConfig.GetClusterParameter(cluster, "MqttCloudIpAddress");
            //string mqttCloudPort = MyAppConfig.GetClusterParameter(cluster, "MqttCloudPort");
            //string mqttCloudLoginName = MyAppConfig.GetParameter("MqttCloudLoginName");
            //string mqttCloudPassword = MyAppConfig.GetParameter("MqttCloudPassword");

            //Log2.Info("StartGame: {0} {1} {2} {3}", mqttCloudIpAddress,
            //                             mqttCloudPort,
            //                             mqttCloudLoginName,
            //                             mqttCloudPassword);

            //MqttCloudDriver.MqttInitializeAsync(mqttCloudIpAddress,
            //                                   mqttCloudLoginName,
            //                                   mqttCloudPassword,
            //                                   int.Parse(mqttCloudPort));

            //MqttCloudDriver.MqttSubscribeAsync(TOPICS.GAME_START_TOPIC);
            
            Log2.Info("StartGame: Sending - {0}", _gameJson);

            MqttCloudDriver.MqttPublishAsync(TOPICS.GAME_START_TOPIC, _gameJson);

            Log2.Info("StartGame: Sending Game to Ledger - {0}", _gameJson);
            GameLedger gameLedger = new GameLedger();
            gameLedger.AddEvent(_gameEventVariable);
            return true;

        }

        public static bool CreateGameJson(GridPeakDetectedObject gridPeakDetectedObject)
        {
            _gameEventVariable = new GameEventVariable();
            _jsonGameEventVariable = new JsonGameEventVariable();

            _gameEventVariable.GameName = "AI Triggered Game";
            _gameEventVariable.GridZone = "ALL";
            _gameEventVariable.GameType = gridPeakDetectedObject.game_type;
            _gameEventVariable.GameAwardRank = gridPeakDetectedObject.award_level;

            DateTime dateTime;
            if (DateTime.TryParse(gridPeakDetectedObject.start_date_time, out dateTime))
            {
                _gameEventVariable.StartTime = dateTime;
            }
            else
            {
                Log2.Error("Invalid datetime format: " + gridPeakDetectedObject.start_date_time);
                return false;
            }

            _gameEventVariable.DurationInMinutes = int.Parse(gridPeakDetectedObject.duration_mins);
            _gameEventVariable.EndTime = _gameEventVariable.StartTime.AddMinutes(_gameEventVariable.DurationInMinutes);
            string hash = Utilities.ComputeSha256Hash(_gameEventVariable.GameName + _gameEventVariable.StartTime + _gameEventVariable.DurationInMinutes);
            _gameEventVariable.GameId = hash;

            _gameEventVariable.DollarPerPoint = .001;
            _gameEventVariable.PointsPerWatt = 1;
            _gameEventVariable.PointsPerPercent = 64;
            _gameEventVariable.BonusPoints = 0;
            _gameEventVariable.BonusPool = 0;
            _gameEventVariable.PreStartAlert = "false";

            _gameJson = _jsonGameEventVariable.GameEventVariable2Json(_gameEventVariable);
            return true;
        }


        public static void SetLevel(string level)
        {
            if (level == "GOLD")
                _gameLevel = "75";
            else if (level == "SILVER")
                _gameLevel = "50";
            else if (level == "BRONZE")
                _gameLevel = "25";
            else if (level == "NONE")
                _gameLevel = "0";
          
        }

        private static string _gameLevel = "0";
        private static string _gameJson;
        private static GameEventVariable _gameEventVariable;
        private static JsonGameEventVariable _jsonGameEventVariable;
    }
}
