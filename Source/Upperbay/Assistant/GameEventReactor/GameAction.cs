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
using System.Threading;

using Upperbay.Core.Logging;
using Upperbay.Core.Library;
using Upperbay.Agent.Interfaces;
using Upperbay.Worker.Voice;
using Upperbay.Worker.PostOffice;

namespace Upperbay.Assistant
{
    class GameAction
    {
        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gev"></param>
        public GameAction(GameEventVariable gev)
        {
            _gameEvent = gev;

            string gameTextingEnabled = MyAppConfig.GetParameter("GameTextingEnabled");
            if (gameTextingEnabled != null) Boolean.TryParse(gameTextingEnabled, out _gameTextingEnabled);

            string gameVoiceEnabled = MyAppConfig.GetParameter("GameVoiceEnabled");
            if (gameVoiceEnabled != null) Boolean.TryParse(gameVoiceEnabled, out _gameVoiceEnabled);

            string gameVoiceShedCommandFileEnabled = MyAppConfig.GetParameter("GameVoiceShedCommandsEnabled");
            if (gameVoiceShedCommandFileEnabled != null) Boolean.TryParse(gameVoiceShedCommandFileEnabled, out _gameVoiceShedCommandFileEnabled);

            string gameVoiceQuarterlyShedCommandsEnabled = MyAppConfig.GetParameter("GameVoiceQuarterlyShedCommandsEnabled");
            if (gameVoiceQuarterlyShedCommandsEnabled != null) Boolean.TryParse(gameVoiceQuarterlyShedCommandsEnabled, out _gameVoiceQuarterlyShedCommandsEnabled);

            string gameVoiceMonthlyShedCommandsEnabled = MyAppConfig.GetParameter("GameVoiceMonthlyShedCommandsEnabled");
            if (gameVoiceMonthlyShedCommandsEnabled != null) Boolean.TryParse(gameVoiceMonthlyShedCommandsEnabled, out _gameVoiceMonthlyShedCommandsEnabled);

            string gameVoiceHarvestCommandFileEnabled = MyAppConfig.GetParameter("GameVoiceHarvestCommandsEnabled");
            if (gameVoiceHarvestCommandFileEnabled != null) Boolean.TryParse(gameVoiceHarvestCommandFileEnabled, out _gameVoiceHarvestCommandFileEnabled);

            string gameVoiceQuarterlyHarvestCommandsEnabled = MyAppConfig.GetParameter("GameVoiceQuarterlyHarvestCommandsEnabled");
            if (gameVoiceQuarterlyHarvestCommandsEnabled != null) Boolean.TryParse(gameVoiceQuarterlyHarvestCommandsEnabled, out _gameVoiceQuarterlyHarvestCommandsEnabled);

            string gameVoiceMonthlyHarvestCommandsEnabled = MyAppConfig.GetParameter("GameVoiceMonthlyHarvestCommandsEnabled");
            if (gameVoiceMonthlyHarvestCommandsEnabled != null) Boolean.TryParse(gameVoiceMonthlyHarvestCommandsEnabled, out _gameVoiceMonthlyHarvestCommandsEnabled);

            _enableShadowMode = MyAppConfig.GetParameter("EnableShadowMode");

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ReceivedShedAction()
        {
            Log2.Info("GAME RECEIVED: {0} {1} {2} {3} {4}",
                                        _gameEvent.GameName,
                                        _gameEvent.GameId,
                                        _gameEvent.GamePlayerId,
                                        _gameEvent.StartTime,
                                        _gameEvent.DurationInMinutes);

            TimeSpan timeSpan = _gameEvent.StartTime - DateTime.Now;
            //string format = "###.";
            //string message = "The Current For Carbon Game will start in " + timeSpan.TotalMinutes.ToString() + " minutes.";
            string message = 
                "Shed Game Has Been Received and will start at  " + _gameEvent.StartTime;
            

            if ((_enableShadowMode == "false") && (_gameEvent.PreStartAlert == "true"))
            {
                //if (_gameTextingEnabled)
                //{
                //    MessageMailer messageMailer = new MessageMailer();
                //    string smsMessageBody = message;
                //    messageMailer.SendSMSText(smsMessageBody);
                //}

                if (_gameVoiceEnabled)
                {
                    Voice.Speak(message);
                }

                if (_gameVoiceShedCommandFileEnabled)
                {
                    string cmdFileName = MyAppConfig.GetParameter("GameVoiceReceivedShedCommands");
                    string cmdPath = "actions\\" + cmdFileName;
                    Log2.Trace("Alexa Filename: {0}", cmdPath);
                    Voice.SpeakVoiceCommandFile(cmdPath);
                }
            }
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ReceivedHarvestAction()
        {
            Log2.Info("GAME RECEIVED: {0} {1} {2} {3} {4}",
                                        _gameEvent.GameName,
                                        _gameEvent.GameId,
                                        _gameEvent.GamePlayerId,
                                        _gameEvent.StartTime,
                                        _gameEvent.DurationInMinutes);

            TimeSpan timeSpan = _gameEvent.StartTime - DateTime.Now;
            //string format = "###.";
            //string message = "The Current For Carbon Game will start in " + timeSpan.TotalMinutes.ToString() + " minutes.";
            string message =
                "Harvest Game Has Been Received and will start at  " + _gameEvent.StartTime;


            if ((_enableShadowMode == "false") && (_gameEvent.PreStartAlert == "true"))
            {
                //if (_gameTextingEnabled)
                //{
                //    MessageMailer messageMailer = new MessageMailer();
                //    string smsMessageBody = message;
                //    messageMailer.SendSMSText(smsMessageBody);
                //}

                if (_gameVoiceEnabled)
                {
                    Voice.Speak(message);
                }

                if (_gameVoiceHarvestCommandFileEnabled)
                {
                    string cmdFileName = MyAppConfig.GetParameter("GameVoiceReceivedHarvestCommands");
                    string cmdPath = "actions\\" + cmdFileName;
                    Log2.Trace("Alexa Filename: {0}", cmdPath);
                    Voice.SpeakVoiceCommandFile(cmdPath);
                }
            }
            return true;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public bool CountdownToStartAction(TimeSpan timeSpan)
        {
            //if ((timeSpan.TotalMinutes <= 10) && ((timeSpan.TotalMinutes % 5) == 0))
            //{
            //    string message = "The Current For Carbon Game will start in " + timeSpan.TotalMinutes.ToString() + " minutes.";
            //    Log2.Info(message);

            //    if (_smsEnabled)
            //    {
            //        MessageMailer messageMailer = new MessageMailer();
            //        string smsMessageBody = message;
            //        messageMailer.SendSMSText(smsMessageBody);
            //    }

            //    if (_gameVoiceEnabled)
            //    {
            //        Voice.Speak(message);
            //    }
            //}
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public bool CountdownToFinishAction(TimeSpan timeSpan)
        {
            //if ((timeSpan.TotalMinutes <= 10) && ((timeSpan.TotalMinutes % 5) == 0))
            //{
            //    string message = "The Current For Carbon Game will finish in " + timeSpan.TotalMinutes.ToString() + " minutes.";
            //    Log2.Info(message);

            //    if (_smsEnabled)
            //    {
            //        MessageMailer messageMailer = new MessageMailer();
            //        string smsMessageBody = message;
            //        messageMailer.SendSMSText(smsMessageBody);
            //    }

            //    if (_gameVoiceEnabled)
            //    {
            //        Voice.Speak(message);
            //    }
            //}

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool StartShedAction()
        {
            Log2.Info("GAME STARTED: {0} {1} {2} {3} {4} {5}",
                                            _gameEvent.GameName,
                                            _gameEvent.GameId,
                                            _gameEvent.GamePlayerId,
                                            _gameEvent.StartTime,
                                            _gameEvent.DurationInMinutes,
                                            _gameEvent.GameAwardRank);

            string message = "Shed Game is starting at " + _gameEvent.StartTime + " and will run for " + _gameEvent.DurationInMinutes + " minutes. This game is paying " + _gameEvent.GameAwardRank + " awards!";

            if (_enableShadowMode == "false")
            {
                if (_gameTextingEnabled)
                {
                    MessageMailer messageMailer = new MessageMailer();
                    string smsMessageBody = message;
                    messageMailer.SendSMSText(smsMessageBody);
                }


                if (_gameVoiceEnabled)
                {
                    Voice.Speak(message);
                }

                if (_gameVoiceShedCommandFileEnabled)
                {
                    string cmdFileName = MyAppConfig.GetParameter("GameVoiceStartShedCommands");
                    string cmdPath = "actions\\" + cmdFileName;
                    Log2.Trace("Alexa Filename: {0}", cmdPath);
                    Voice.SpeakVoiceCommandFile(cmdPath);

                    //Execute quarterly commands
                    if (_gameVoiceQuarterlyShedCommandsEnabled)
                    {
                        try
                        {
                            int month = DateTime.Now.Month;
                            if ((month >= 1) && (month <= 3))
                                Voice.SpeakVoiceCommandFile("actions\\Q1StartShedActions.txt");
                            if ((month >= 4) && (month <= 6))
                                Voice.SpeakVoiceCommandFile("actions\\Q2StartShedActions.txt");
                            if ((month >= 7) && (month <= 9))
                                Voice.SpeakVoiceCommandFile("actions\\Q3StartShedActions.txt");
                            if ((month >= 10) && (month <= 12))
                                Voice.SpeakVoiceCommandFile("actions\\Q4StartShedActions.txt");
                        }
                        catch (Exception)
                        {
                            //Voice.Speak("Quarterly action file not found.");
                        }
                    }


                    // execute monthly commands

                    //Execute quarterly commands
                    if (_gameVoiceMonthlyShedCommandsEnabled)
                    {
                        try
                        {
                            int month = DateTime.Now.Month;

                            switch (month)
                            {
                                case 1:
                                    Voice.SpeakVoiceCommandFile("actions\\JanStartShedActions.txt");
                                    break;
                                case 2:
                                    Voice.SpeakVoiceCommandFile("actions\\FebStartShedActions.txt");
                                    break;
                                case 3:
                                    Voice.SpeakVoiceCommandFile("actions\\MarStartShedActions.txt");
                                    break;
                                case 4:
                                    Voice.SpeakVoiceCommandFile("actions\\AprStartShedActions.txt");
                                    break;
                                case 5:
                                    Voice.SpeakVoiceCommandFile("actions\\MayStartShedActions.txt");
                                    break;
                                case 6:
                                    Voice.SpeakVoiceCommandFile("actions\\JunStartShedActions.txt");
                                    break;
                                case 7:
                                    Voice.SpeakVoiceCommandFile("actions\\JulStartShedActions.txt");
                                    break;
                                case 8:
                                    Voice.SpeakVoiceCommandFile("actions\\AugStartShedActions.txt");
                                    break;
                                case 9:
                                    Voice.SpeakVoiceCommandFile("actions\\SeptStartShedActions.txt");
                                    break;
                                case 10:
                                    Voice.SpeakVoiceCommandFile("actions\\OctStartShedActions.txt");
                                    break;
                                case 11:
                                    Voice.SpeakVoiceCommandFile("actions\\NovStartShedActions.txt");
                                    break;
                                case 12:
                                    Voice.SpeakVoiceCommandFile("actions\\DecStartShedActions.txt");
                                    break;
                                default:
                                    break;
                            }
                        }
                        catch (Exception)
                        {
                            // Voice.Speak("Month action file not found.");
                        }
                    }
                }
            }
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool StartHarvestAction()
        {
            Log2.Info("GAME STARTED: {0} {1} {2} {3} {4} {5}",
                                            _gameEvent.GameName,
                                            _gameEvent.GameId,
                                            _gameEvent.GamePlayerId,
                                            _gameEvent.StartTime,
                                            _gameEvent.DurationInMinutes,
                                            _gameEvent.GameAwardRank);

            string message = "Harvest Game is starting at " + _gameEvent.StartTime + " and will run for " + _gameEvent.DurationInMinutes + " minutes. This game is paying " + _gameEvent.GameAwardRank + " awards!";

            if (_enableShadowMode == "false")
            {
                if (_gameTextingEnabled)
                {
                    MessageMailer messageMailer = new MessageMailer();
                    string smsMessageBody = message;
                    messageMailer.SendSMSText(smsMessageBody);
                }


                if (_gameVoiceEnabled)
                {
                    Voice.Speak(message);
                }

                if (_gameVoiceHarvestCommandFileEnabled)
                {
                    string cmdFileName = MyAppConfig.GetParameter("GameVoiceStartHarvestCommands");
                    string cmdPath = "actions\\" + cmdFileName;
                    Log2.Trace("Alexa Filename: {0}", cmdPath);
                    Voice.SpeakVoiceCommandFile(cmdPath);

                    //Execute quarterly commands
                    if (_gameVoiceQuarterlyHarvestCommandsEnabled)
                    {
                        try
                        {
                            int month = DateTime.Now.Month;
                            if ((month >= 1) && (month <= 3))
                                Voice.SpeakVoiceCommandFile("actions\\Q1StartHarvestActions.txt");
                            if ((month >= 4) && (month <= 6))
                                Voice.SpeakVoiceCommandFile("actions\\Q2StartHarvestActions.txt");
                            if ((month >= 7) && (month <= 9))
                                Voice.SpeakVoiceCommandFile("actions\\Q3StartHarvestActions.txt");
                            if ((month >= 10) && (month <= 12))
                                Voice.SpeakVoiceCommandFile("actions\\Q4StartHarvestActions.txt");
                        }
                        catch (Exception)
                        {
                            //Voice.Speak("Quarterly action file not found.");
                        }
                    }


                    // execute monthly commands

                    //Execute quarterly commands
                    if (_gameVoiceMonthlyHarvestCommandsEnabled)
                    {
                        try
                        {
                            int month = DateTime.Now.Month;

                            switch (month)
                            {
                                case 1:
                                    Voice.SpeakVoiceCommandFile("actions\\JanStartHarvestActions.txt");
                                    break;
                                case 2:
                                    Voice.SpeakVoiceCommandFile("actions\\FebStartHarvestActions.txt");
                                    break;
                                case 3:
                                    Voice.SpeakVoiceCommandFile("actions\\MarStartHarvestActions.txt");
                                    break;
                                case 4:
                                    Voice.SpeakVoiceCommandFile("actions\\AprStartHarvestActions.txt");
                                    break;
                                case 5:
                                    Voice.SpeakVoiceCommandFile("actions\\MayStartHarvestActions.txt");
                                    break;
                                case 6:
                                    Voice.SpeakVoiceCommandFile("actions\\JunStartHarvestActions.txt");
                                    break;
                                case 7:
                                    Voice.SpeakVoiceCommandFile("actions\\JulStartHarvestActions.txt");
                                    break;
                                case 8:
                                    Voice.SpeakVoiceCommandFile("actions\\AugStartHarvestActions.txt");
                                    break;
                                case 9:
                                    Voice.SpeakVoiceCommandFile("actions\\SeptStartHarvestActions.txt");
                                    break;
                                case 10:
                                    Voice.SpeakVoiceCommandFile("actions\\OctStartHarvestActions.txt");
                                    break;
                                case 11:
                                    Voice.SpeakVoiceCommandFile("actions\\NovStartHarvestActions.txt");
                                    break;
                                case 12:
                                    Voice.SpeakVoiceCommandFile("actions\\DecStartHarvestActions.txt");
                                    break;
                                default:
                                    break;
                            }
                        }
                        catch (Exception)
                        {
                            // Voice.Speak("Month action file not found.");
                        }
                    }
                }
            }
            return true;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool FinishShedAction()
        {
            Log2.Info("GAME FINISHED: {0} {1} {2} {3} {4}",
                                            _gameEvent.GameName,
                                            _gameEvent.GameId,
                                            _gameEvent.GamePlayerId,
                                            _gameEvent.StartTime,
                                            _gameEvent.DurationInMinutes);

            //string message = "The CurrentForCarbon Game has finished.";

            //if (_gameTextingEnabled)
            //{
            //    MessageMailer messageMailer = new MessageMailer();
            //    string smsMessageBody = message;
            //    messageMailer.SendSMSText(smsMessageBody);
            //}

            //if (_gameVoiceEnabled)
            //{
            //    Voice.Speak(message);
            //}

            if (_enableShadowMode == "false")
            {

                if (_gameVoiceShedCommandFileEnabled)
                {
                    string cmdFileName = MyAppConfig.GetParameter("GameVoiceFinishedShedCommands");
                    string cmdPath = "actions\\" + cmdFileName;
                    Log2.Trace("Alexa Filename: {0}", cmdPath);
                    Voice.SpeakVoiceCommandFile(cmdPath);

                    //Execute quarterly commands
                    if (_gameVoiceQuarterlyShedCommandsEnabled)
                    {
                        try
                        {
                            int month = DateTime.Now.Month;
                            if ((month >= 1) && (month <= 3))
                                Voice.SpeakVoiceCommandFile("actions\\Q1FinishedShedActions.txt");
                            if ((month >= 4) && (month <= 6))
                                Voice.SpeakVoiceCommandFile("actions\\Q2FinishedShedActions.txt");
                            if ((month >= 7) && (month <= 9))
                                Voice.SpeakVoiceCommandFile("actions\\Q3FinishedShedActions.txt");
                            if ((month >= 10) && (month <= 12))
                                Voice.SpeakVoiceCommandFile("actions\\Q4FinishedShedActions.txt");
                        }
                        catch (Exception)
                        {
                            //Voice.Speak("Quarterly action file not found.");
                        }
                    }


                    // execute monthly commands

                    //Execute quarterly commands
                    if (_gameVoiceMonthlyShedCommandsEnabled)
                    {
                        try
                        {
                            int month = DateTime.Now.Month;

                            switch (month)
                            {
                                case 1:
                                    Voice.SpeakVoiceCommandFile("actions\\JanFinishedShedActions.txt");
                                    break;
                                case 2:
                                    Voice.SpeakVoiceCommandFile("actions\\FebFinishedShedActions.txt");
                                    break;
                                case 3:
                                    Voice.SpeakVoiceCommandFile("actions\\MarFinishedShedActions.txt");
                                    break;
                                case 4:
                                    Voice.SpeakVoiceCommandFile("actions\\AprFinishedShedActions.txt");
                                    break;
                                case 5:
                                    Voice.SpeakVoiceCommandFile("actions\\MayFinishedShedActions.txt");
                                    break;
                                case 6:
                                    Voice.SpeakVoiceCommandFile("actions\\JunFinishedShedActions.txt");
                                    break;
                                case 7:
                                    Voice.SpeakVoiceCommandFile("actions\\JulFinishedShedActions.txt");
                                    break;
                                case 8:
                                    Voice.SpeakVoiceCommandFile("actions\\AugFinishedShedActions.txt");
                                    break;
                                case 9:
                                    Voice.SpeakVoiceCommandFile("actions\\SeptFinishedShedActions.txt");
                                    break;
                                case 10:
                                    Voice.SpeakVoiceCommandFile("actions\\OctFinishedShedActions.txt");
                                    break;
                                case 11:
                                    Voice.SpeakVoiceCommandFile("actions\\NovFinishedShedActions.txt");
                                    break;
                                case 12:
                                    Voice.SpeakVoiceCommandFile("actions\\DecFinishedShedActions.txt");
                                    break;
                                default:
                                    break;
                            }
                        }
                        catch (Exception)
                        {
                            // Voice.Speak("Month action file not found.");
                        }
                    }
                }
            }

            GameScorer gameScorer = new GameScorer(_gameEvent);
            
            _gameResult = gameScorer.CalculateScore();

            GamePublisher gamePublisher = new GamePublisher(_gameResult);
            gamePublisher.Publish();

            return true;
        }


            


            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public bool FinishHarvestAction()
            {
                Log2.Info("HARVEST GAME FINISHED: {0} {1} {2} {3} {4}",
                                                _gameEvent.GameName,
                                                _gameEvent.GameId,
                                                _gameEvent.GamePlayerId,
                                                _gameEvent.StartTime,
                                                _gameEvent.DurationInMinutes);

                //string message = "The CurrentForCarbon Game has finished.";

                //if (_gameTextingEnabled)
                //{
                //    MessageMailer messageMailer = new MessageMailer();
                //    string smsMessageBody = message;
                //    messageMailer.SendSMSText(smsMessageBody);
                //}

                //if (_gameVoiceEnabled)
                //{
                //    Voice.Speak(message);
                //}

                if (_enableShadowMode == "false")
                {

                    if (_gameVoiceHarvestCommandFileEnabled)
                    {
                        string cmdFileName = MyAppConfig.GetParameter("GameVoiceFinishedHarvestCommands");
                        string cmdPath = "actions\\" + cmdFileName;
                        Log2.Trace("Alexa Filename: {0}", cmdPath);
                        Voice.SpeakVoiceCommandFile(cmdPath);

                        //Execute quarterly commands
                        if (_gameVoiceQuarterlyHarvestCommandsEnabled)
                        {
                            try
                            {
                                int month = DateTime.Now.Month;
                                if ((month >= 1) && (month <= 3))
                                    Voice.SpeakVoiceCommandFile("actions\\Q1FinishedHarvestActions.txt");
                                if ((month >= 4) && (month <= 6))
                                    Voice.SpeakVoiceCommandFile("actions\\Q2FinishedHarvestActions.txt");
                                if ((month >= 7) && (month <= 9))
                                    Voice.SpeakVoiceCommandFile("actions\\Q3FinishedHarvestActions.txt");
                                if ((month >= 10) && (month <= 12))
                                    Voice.SpeakVoiceCommandFile("actions\\Q4FinishedHarvestActions.txt");
                            }
                            catch (Exception)
                            {
                                //Voice.Speak("Quarterly action file not found.");
                            }
                        }


                        // execute monthly commands

                        //Execute quarterly commands
                        if (_gameVoiceMonthlyHarvestCommandsEnabled)
                        {
                            try
                            {
                                int month = DateTime.Now.Month;

                                switch (month)
                                {
                                    case 1:
                                        Voice.SpeakVoiceCommandFile("actions\\JanFinishedHarvestActions.txt");
                                        break;
                                    case 2:
                                        Voice.SpeakVoiceCommandFile("actions\\FebFinishedHarvestActions.txt");
                                        break;
                                    case 3:
                                        Voice.SpeakVoiceCommandFile("actions\\MarFinishedHarvestActions.txt");
                                        break;
                                    case 4:
                                        Voice.SpeakVoiceCommandFile("actions\\AprFinishedHarvestActions.txt");
                                        break;
                                    case 5:
                                        Voice.SpeakVoiceCommandFile("actions\\MayFinishedHarvestActions.txt");
                                        break;
                                    case 6:
                                        Voice.SpeakVoiceCommandFile("actions\\JunFinishedHarvestActions.txt");
                                        break;
                                    case 7:
                                        Voice.SpeakVoiceCommandFile("actions\\JulFinishedHarvestActions.txt");
                                        break;
                                    case 8:
                                        Voice.SpeakVoiceCommandFile("actions\\AugFinishedHarvestActions.txt");
                                        break;
                                    case 9:
                                        Voice.SpeakVoiceCommandFile("actions\\SeptFinishedHarvestActions.txt");
                                        break;
                                    case 10:
                                        Voice.SpeakVoiceCommandFile("actions\\OctFinishedHarvestActions.txt");
                                        break;
                                    case 11:
                                        Voice.SpeakVoiceCommandFile("actions\\NovFinishedHarvestActions.txt");
                                        break;
                                    case 12:
                                        Voice.SpeakVoiceCommandFile("actions\\DecFinishedHarvestActions.txt");
                                        break;
                                    default:
                                        break;
                                }
                            }
                            catch (Exception)
                            {
                                // Voice.Speak("Month action file not found.");
                            }
                        }
                    }
                }


                GameScorer gameScorer = new GameScorer(_gameEvent);
                _gameResult = gameScorer.CalculateScore();

                GamePublisher gamePublisher = new GamePublisher(_gameResult);
                gamePublisher.Publish();

                return true;
            }
        
        
      


        #endregion


        #region Private State
        private GameEventVariable _gameEvent;
        private GameResultsVariable _gameResult;
        private bool _gameVoiceShedCommandFileEnabled = false;
        private bool _gameVoiceQuarterlyShedCommandsEnabled = false;
        private bool _gameVoiceMonthlyShedCommandsEnabled = false;
        private bool _gameVoiceHarvestCommandFileEnabled = false;
        private bool _gameVoiceQuarterlyHarvestCommandsEnabled = false;
        private bool _gameVoiceMonthlyHarvestCommandsEnabled = false;
        private bool _gameVoiceEnabled = false;
        private bool _gameTextingEnabled = false;
        private string _enableShadowMode = "false";
        #endregion
    }


}
