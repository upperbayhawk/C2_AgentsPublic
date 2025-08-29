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

using Upperbay.Core.Library;
using Upperbay.Core.Logging;
using Upperbay.Agent.Interfaces;
using Upperbay.Worker.ThingSpeak;

namespace Upperbay.Assistant
{
    class GameScorer
    {
        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gev"></param>
        public GameScorer(GameEventVariable gev)
        {
            _gameEvent = gev;
            _currentForCarbonWatts = MyAppConfig.GetParameter("CurrentForCarbonWatts");
            _saveRawPowerMeterData = MyAppConfig.GetParameter("SaveRawPowerMeterData");
            _enableShadowMode = MyAppConfig.GetParameter("EnableShadowMode");
            _timeZone = MyAppConfig.GetParameter("GamePlayerTimeZone");
            _calcAveragesUsingRawData = MyAppConfig.GetParameter("CalcAveragesFromRawData");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public GameResultsVariable CalculateScore()
        {

            _gameResults = new GameResultsVariable();

            Log2.Debug("GAME CalculateScore: {0} {1} {2} {3} {4}",
                                        _gameEvent.GameName,
                                        _gameEvent.GameId,
                                        _gameEvent.GamePlayerId,
                                        _gameEvent.StartTime,
                                        _gameEvent.DurationInMinutes);

            DateTime gameStartTime = _gameEvent.StartTime;
            DateTime gameEndTime = _gameEvent.StartTime.AddMinutes(_gameEvent.DurationInMinutes);

            TimeSpan baselineBegin = TimeSpan.FromMinutes(_gameEvent.DurationInMinutes);
            DateTime baselineStartTime = _gameEvent.StartTime.Subtract(baselineBegin);
            DateTime baselineEndTime = _gameEvent.StartTime;

            //for 30 second sampling
            //int numOf30SecIntervals = _gameEvent.DurationInSeconds / 60 * 2;

            ThingSpeakAccess thingSpeakAccess = new ThingSpeakAccess();
            bool calcAveragesUsingRawData = bool.Parse(_calcAveragesUsingRawData);
            bool saveRawPowerMeterData = bool.Parse(_saveRawPowerMeterData);

            if (calcAveragesUsingRawData)
            {
                string ssss = thingSpeakAccess.GetAverageValueFromRawData(
                                                "baseline",
                                                _gameEvent.GamePlayerId,
                                                _gameEvent.GameId,
                                                _gameEvent.GameName,
                                                _currentForCarbonWatts,
                                                baselineStartTime,
                                                baselineEndTime,
                                                _gameEvent.DurationInMinutes,
                                                _timeZone,
                                                saveRawPowerMeterData);
                Log2.Debug("Raw Baseline Average: {0}", ssss);
                _avgBaselinePower = Double.Parse(ssss);

                System.Threading.Thread.Sleep(2000);

                string sss = thingSpeakAccess.GetAverageValueFromRawData(
                                                    "event",
                                                    _gameEvent.GamePlayerId,
                                                    _gameEvent.GameId,
                                                    _gameEvent.GameName,
                                                    _currentForCarbonWatts,
                                                    gameStartTime,
                                                    gameEndTime,
                                                    _gameEvent.DurationInMinutes,
                                                    _timeZone,
                                                    saveRawPowerMeterData);
                Log2.Debug("Raw Event Average: {0}", sss);
                _avgIntervalPower = Double.Parse(sss);
            }
            else
            {
                // Get values from thingspeak
                string s = thingSpeakAccess.GetAverageValue(_currentForCarbonWatts,
                                                gameStartTime,
                                                gameEndTime,
                                                _gameEvent.DurationInMinutes,
                                                _timeZone);
                //numOf30SecIntervals);//numOf30SecIntervals);
                Log2.Debug("Average Power: {0}", s);
                _avgIntervalPower = Double.Parse(s);

                System.Threading.Thread.Sleep(2000);

                string ss = thingSpeakAccess.GetAverageValue(_currentForCarbonWatts,
                                                baselineStartTime,
                                                baselineEndTime,
                                                _gameEvent.DurationInMinutes,
                                                _timeZone);
                //numOf30SecIntervals);//numOf30SecIntervals);
                Log2.Debug("Baseline Power: {0}", ss);
                _avgBaselinePower = Double.Parse(ss);
            }

            _deltaPower = _avgIntervalPower - _avgBaselinePower;
            Log2.Debug("Delta Power: {0}", _deltaPower.ToString());

            double gameEnergyInKWH = _avgIntervalPower / 1000 * TimeSpan.FromMinutes(_gameEvent.DurationInMinutes).TotalHours;
            double baselineEnergyInKWH = _avgBaselinePower / 1000 * TimeSpan.FromMinutes(_gameEvent.DurationInMinutes).TotalHours;
            double deltaPowerInPercent = _deltaPower / _avgIntervalPower;

            double percentPoints = 0.0;
            double wattPoints = 0.0;
            double pointsAwarded = 0.0;
            double awardValue = 0.0;

            if (_gameEvent.GameType == "SHEDPOWER")
            {
                if (_deltaPower < 0)
                {
                    percentPoints = ((_deltaPower * _gameEvent.PointsPerPercent) * -1) / _avgBaselinePower;
                    wattPoints = (_deltaPower * _gameEvent.PointsPerWatt) * -1;

                    if (percentPoints > wattPoints)
                        pointsAwarded = percentPoints;
                    else
                        pointsAwarded = wattPoints;
                    awardValue = pointsAwarded * _gameEvent.DollarPerPoint;
                    Log2.Info("GOOD JOB! You Are Using Less Power!!!");
                    Log2.Info("CONGRATS! YOUR REWARD is {0} Points, worth ${1} dollars!", pointsAwarded.ToString(), awardValue.ToString());
                }
                else
                {
                    Log2.Info("UGH...You Are Using MORE Power!!! You can do better.");
                }
            }
            else if (_gameEvent.GameType == "HARVESTPOWER")
            {
                if (_deltaPower > 0)
                {
                    percentPoints = ((_deltaPower * _gameEvent.PointsPerPercent) * 1) / _avgBaselinePower;
                    wattPoints = (_deltaPower * _gameEvent.PointsPerWatt) * 1;

                    if (percentPoints > wattPoints)
                        pointsAwarded = percentPoints;
                    else
                        pointsAwarded = wattPoints;
                    awardValue = pointsAwarded * _gameEvent.DollarPerPoint;
                    Log2.Info("GOOD JOB! You Are Using More Power!!!");
                    Log2.Info("CONGRATS! YOUR REWARD is {0} Points, worth ${1} dollars!", pointsAwarded.ToString(), awardValue.ToString());
                }
                else
                {
                    Log2.Info("UGH...You Are Using LESS Power!!! You can do better.");
                }
            }
            
            //Populate gameResultsVariable

            _gameResults.PercentPoints = percentPoints;
            _gameResults.WattPoints = wattPoints;
            _gameResults.PointsAwarded = pointsAwarded;
            _gameResults.AwardValue = awardValue;

            // Now fill in the rest
            _gameResults.GameName = _gameEvent.GameName;
            _gameResults.GameId = _gameEvent.GameId;
            _gameResults.GamePlayerId = _gameEvent.GamePlayerId;
            _gameResults.GameType = _gameEvent.GameType;
            _gameResults.DollarPerPoint = _gameEvent.DollarPerPoint;
            _gameResults.PointsPerWatt = _gameEvent.PointsPerWatt;
            _gameResults.PointsPerPercent = _gameEvent.PointsPerPercent;
            _gameResults.StartTime = _gameEvent.StartTime;
            _gameResults.EndTime = _gameEvent.StartTime.AddMinutes(_gameEvent.DurationInMinutes);
            _gameResults.DurationInMinutes = _gameEvent.DurationInMinutes;
            _gameResults.GameAvgPowerInWatts = _avgIntervalPower;
            _gameResults.GameEnergyInKWH = gameEnergyInKWH;
            _gameResults.BaselineAvgPowerInWatts = _avgBaselinePower;
            _gameResults.BaselineEnergyInKWH = baselineEnergyInKWH;
            _gameResults.DeltaPowerInWatts = _deltaPower;
            _gameResults.DeltaPowerInPercent = deltaPowerInPercent;

            GamePlayer gamePlayer = new GamePlayer();
            _gameResults.GamePlayerSignature = gamePlayer.CreatePlayerSignature();

            return _gameResults;
        }



        #endregion

        #region Private State
        private GameEventVariable _gameEvent;
        private GameResultsVariable _gameResults;
        private string _currentForCarbonWatts;
        private string _saveRawPowerMeterData;
        private double _avgIntervalPower;
        private double _avgBaselinePower;
        private double _deltaPower;
        private string _enableShadowMode;
        private string _timeZone;
        private string _calcAveragesUsingRawData;
        #endregion
    }
}
