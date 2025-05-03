//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
using System;
using System.Configuration;
using System.IO;
using System.Threading;

using Upperbay.Core.Logging;
using Upperbay.Core.Library;
using Upperbay.Agent.Interfaces;
using Upperbay.Worker.PostOffice;
using Upperbay.Worker.JSON;
using Upperbay.Worker.MQTT;
using Upperbay.Worker.Voice;
using Upperbay.Worker.EtherAccess;

namespace Upperbay.Assistant
{
    class GamePublisher
    {
        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grv"></param>
        public GamePublisher(GameResultsVariable grv)
        {
            _gameResults = grv;

            string gameLedgerEnabled = MyAppConfig.GetParameter("GameLedgerEnabled");
            if (gameLedgerEnabled != null) Boolean.TryParse(gameLedgerEnabled, out _gameLedgerEnabled);

            string gameTextingEnabled = MyAppConfig.GetParameter("GameTextingEnabled");
            if (gameTextingEnabled != null) Boolean.TryParse(gameTextingEnabled, out _gameTextingEnabled);

            string gameMQTTEnabled = MyAppConfig.GetParameter("GameMQTTEnabled");
            if (gameMQTTEnabled != null) Boolean.TryParse(gameMQTTEnabled, out _gameMQTTEnabled);

            string gameVoiceEnabled = MyAppConfig.GetParameter("GameVoiceEnabled");
            if (gameVoiceEnabled != null) Boolean.TryParse(gameVoiceEnabled, out _gameVoiceEnabled);

            string gameResultsJsonFileName = MyAppConfig.GetParameter("GameResultsJsonFileName");
            _gameResultsJsonFileName = gameResultsJsonFileName;

            string gameResultsTextFileName = MyAppConfig.GetParameter("GameResultsTextFileName");
            _gameResultsTextFileName = gameResultsTextFileName;

            _enableShadowMode = MyAppConfig.GetParameter("EnableShadowMode");
            _gamePlayerNumber = MyAppConfig.GetParameter("GamePlayerNumber");
            _publishingDelay = MyAppConfig.GetParameter("PublishingDelay");

        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Publish()
        {
            JsonGameResultsVariable jsonGameResultsVariable = new JsonGameResultsVariable();
            _gameResultsJson = jsonGameResultsVariable.GameResultsVariable2Json(_gameResults);
            Log2.Debug("GAME RESULTS JSON: {0}", _gameResultsJson);

            _gameResultsText = "GameName = " + _gameResults.GameName + ", " +
                                 "GameId = " + _gameResults.GameId + ", " +
                                 "GamePlayerId = " + _gameResults.GamePlayerId + ", " +
                                 "GameType = " + _gameResults.GameType + ", " +
                                 "StartTime = " + _gameResults.StartTime + ", " +
                                 "EndTime = " + _gameResults.EndTime + ", " +
                                 "DurationInMinutes = " + _gameResults.DurationInMinutes + ", " +
                                 "GameAvgPowerInWatts = " + _gameResults.GameAvgPowerInWatts + ", " +
                                 "GameEnergyInKWH = " + _gameResults.GameEnergyInKWH + ", " +
                                 "BaselineAvgPowerInWatts = " + _gameResults.BaselineAvgPowerInWatts + ", " +
                                 "BaselineEnergyInKWH = " + _gameResults.BaselineEnergyInKWH + ", " +
                                 "DeltaPowerInWatts = " + _gameResults.DeltaPowerInWatts + ", " +
                                 "DeltaPowerInPercent = " + _gameResults.DeltaPowerInPercent + ", " +
                                 "GamePlayerSignature  = " + _gameResults.GamePlayerSignature;

            Log2.Debug("GAME RESULTS PUBLISHED: {0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11} {12} {13}",
                                        _gameResults.GameName,
                                        _gameResults.GameId,
                                        _gameResults.GamePlayerId,
                                        _gameResults.GameType,
                                        _gameResults.StartTime,
                                        _gameResults.EndTime,
                                        _gameResults.DurationInMinutes,
                                        _gameResults.GameAvgPowerInWatts,
                                        _gameResults.GameEnergyInKWH,
                                        _gameResults.BaselineAvgPowerInWatts,
                                        _gameResults.BaselineEnergyInKWH,
                                        _gameResults.DeltaPowerInWatts,
                                        _gameResults.DeltaPowerInPercent,
                                        _gameResults.GamePlayerSignature
                                        );

            Write2JsonFile();
            Write2TextFile();

            if (_enableShadowMode == "false")
            {
                if (_gameTextingEnabled)
                {
                    Publish2Texting();
                }
                if (_gameMQTTEnabled)
                {
                    Publish2MQTT();
                }
                if (_gameLedgerEnabled)
                {
                    int gamePlayerNumber = int.Parse(_gamePlayerNumber);
                    if (gamePlayerNumber > 0)
                    {
                        int publishingDelay = int.Parse(_publishingDelay);
                        int sleepNumber = 1000 * ((gamePlayerNumber % 10) + 1) * publishingDelay;
                        Log2.Debug("Publisher sleeping {0} millisecs", sleepNumber);
                        Thread.Sleep(sleepNumber);
                    }
                    Publish2Ledger();
                }
                if (_gameVoiceEnabled)
                {
                    Publish2Voice();
                }
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool Publish2Texting()
        {
            if (_gameResults.PointsAwarded > 0)
            {
                string format = "###.#";
                string points = (_gameResults.PointsAwarded).ToString(format);
                string value = (_gameResults.AwardValue).ToString(format);
                string name = _gameResults.GameName;
                string textstring = "You just received an award of " + points + " points that are worth " + value + " dollars";

                MessageMailer messageMailer = new MessageMailer();
                string smsMessageBody = textstring;
                messageMailer.SendSMSText(smsMessageBody);
            }
            else
            {
                string notgoodstring = "You used more power and did not win an award this time. Try again next time.";
                MessageMailer messageMailer = new MessageMailer();
                string smsMessageBody = notgoodstring;
                messageMailer.SendSMSText(smsMessageBody);
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool Publish2MQTT()
        {
            MqttCloudDriver.MqttPublishAsync(TOPICS.GAME_RESULTS_TOPIC, _gameResultsJson);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool Publish2Ledger()
        {

            GameLedger gameLedger = new GameLedger();
            gameLedger.AddResult(_gameResults);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool Publish2Voice()
        {
            string format = "###.#";
            string points = (_gameResults.PointsAwarded).ToString(format);
            string value = (_gameResults.AwardValue).ToString(format);
            string name = _gameResults.GameName;
            string speakstring = "You just received an award of " + points + " points that are worth " + value + " dollars from the game " + name;
            string notgoodstring = "You did not win an award this time. Try again next time.";

            if (_gameResults.PointsAwarded > 0)
            {
                Log2.Debug(speakstring);
                Voice.Speak(speakstring);
            }
            else
            {
                Log2.Debug(notgoodstring);
                Voice.Speak(notgoodstring);
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool Write2JsonFile()
        {

            // Append text to an existing file named "WriteLines.txt".

            string filePath = "logs\\" + _gameResultsJsonFileName;
            using (StreamWriter outputFile = new StreamWriter(filePath, true))
            {
                outputFile.WriteLine(_gameResultsJson);
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool Write2TextFile()
        {

            // Append text to an existing file named "WriteLines.txt".
            string filePath = "logs\\" + _gameResultsTextFileName;
            using (StreamWriter outputFile = new StreamWriter(filePath, true))
            {
                outputFile.WriteLine(_gameResultsText);
            }
            return true;
        }

        #endregion

        #region Private State

        private GameResultsVariable _gameResults;

        private string _gameResultsJsonFileName = "";
        private string _gameResultsTextFileName = "";
        private string _gameResultsJson = "";
        private string _gameResultsText = "";
        private string _enableShadowMode = "false";
        private string _gamePlayerNumber = "0";

        private bool _gameLedgerEnabled = false;
        private bool _gameMQTTEnabled = false;
        private bool _gameTextingEnabled = false;
        private bool _gameVoiceEnabled = false;

        private string _publishingDelay = "1";
        #endregion
    }


}
