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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using System.Xml;
using System.ServiceProcess;
using System.Windows.Forms;

using Upperbay.Core.Logging;
using Upperbay.Core.Library;
using Upperbay.Worker.ODBC;

namespace Upperbay.Worker.EtherAccess
{
    public class GameAwards
    {
        #region Methods

       /// <summary>
       /// 
       /// </summary>
        public GameAwards()
        {
            _gamePoints = new Dictionary<string, double>(); //gameID, Points
            _gameValue = new Dictionary<string, double>();
            _gamePercentPoints = new Dictionary<string, double>();
            _gameDeltaWatts = new Dictionary<string, double>();
            _gameCount = new Dictionary<string, int>();
            _gameMin = new Dictionary<string, double>();
            _gameMax = new Dictionary<string, double>();
            _gameMean = new Dictionary<string, double>();

            _gameStdDev = new Dictionary<string, double>();
            _gameRawPoints = new Dictionary<string, List<double>>();

            _enableStatistics= MyAppConfig.GetParameter("EnableStatistics");
            // Log2.Info("YearToDateGameAwards Created");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameID"></param>
        /// <param name="points"></param>
        /// <param name="percentPoints"></param>
        /// <param name="value"></param>
        /// <param name="watts"></param>
        public void AddPointsAndValue(string gameID,
                                    double points, 
                                    double percentPoints, 
                                    double value, 
                                    double watts)
        {

            lock (awardLock)
            {

                /////////////////////////////
                double oldPoints = 0;
                if (_gamePoints.TryGetValue(gameID, out oldPoints))
                {
                    double newPoints = 0;
                    newPoints = oldPoints + points;
                    _gamePoints.Remove(gameID);
                    _gamePoints.Add(gameID, newPoints);
                    Log2.Trace("Game Points Accumulator {0}: OLD = {1}, NEW = {2}",
                                                gameID, oldPoints.ToString(), newPoints.ToString());
                }
                else
                    _gamePoints.Add(gameID, points);

                /////////////////////////////

                double oldPercentPoints = 0;
                if (_gamePercentPoints.TryGetValue(gameID, out oldPercentPoints))
                {
                    double newPercentPoints = 0;
                    newPercentPoints = oldPercentPoints + percentPoints;
                    _gamePercentPoints.Remove(gameID);
                    _gamePercentPoints.Add(gameID, newPercentPoints);
                    Log2.Trace("Game PercentPoints Accumulator {0}: {1}, OLD = {2}, NEW = {3}",
                                gameID, gameID, oldPercentPoints.ToString(), newPercentPoints.ToString());
                }
                else
                    _gamePercentPoints.Add(gameID, percentPoints);

                //////////////////////////////

                double oldValue = 0;
                if (_gameValue.TryGetValue(gameID, out oldValue))
                {
                    double newValue = 0;
                    newValue = oldValue + value;
                    _gameValue.Remove(gameID);
                    _gameValue.Add(gameID, newValue);
                    Log2.Trace("Game Value Accumulator {0}: OLD = {1}, NEW = {2}",
                                    gameID, oldValue.ToString(), newValue.ToString());
                }
                else
                    _gameValue.Add(gameID, value);


                //////////////////////////////

                double oldWatts = 0;
                if (_gameDeltaWatts.TryGetValue(gameID, out oldWatts))
                {
                    double newWatts = 0;
                    if (watts > 0)
                    {
                        watts = 0;
                        newWatts = oldWatts + watts;
                    }
                    else
                    {
                        newWatts = oldWatts + watts;
                    }
                    _gameDeltaWatts.Remove(gameID);
                    _gameDeltaWatts.Add(gameID, newWatts);
                    Log2.Trace("Game Watts Accumulator {0}:  OLD = {1}, NEW = {2}",
                                    gameID, oldWatts.ToString(), newWatts.ToString());
                }
                else
                    _gameDeltaWatts.Add(gameID, watts);



                int oldCount = 0;
                if (_gameCount.TryGetValue(gameID, out oldCount))
                {
                    int newCount;
                    newCount = oldCount + 1;
                    _gameCount.Remove(gameID);
                    _gameCount.Add(gameID, newCount);
                    Log2.Trace("Game Count Accumulator {0}: OLD = {1}, NEW = {2}",
                                                gameID, oldCount.ToString(), newCount.ToString());
                }
                else
                    _gameCount.Add(gameID, 1);


                /////////////////////////////
                double oldMin = 0;
                if (_gameMin.TryGetValue(gameID, out oldMin))
                {
                    if (points < oldMin)
                    {
                        double newMin;
                        newMin = points;
                        _gameMin.Remove(gameID);
                        _gameMin.Add(gameID, newMin);
                        Log2.Trace("Game Min Accumulator {0}: OLD = {1}, NEW = {2}",
                                                    gameID, oldMin.ToString(), newMin.ToString());
                    }
                }
                else
                    _gameMin.Add(gameID, points);

                /////////////////////////////
                double oldMax = 0;
                if (_gameMax.TryGetValue(gameID, out oldMax))
                {
                    if (points > oldMax)
                    {
                        double newMax;
                        newMax = points;
                        _gameMax.Remove(gameID);
                        _gameMax.Add(gameID, newMax);
                        Log2.Trace("Game Max Accumulator {0}: OLD = {1}, NEW = {2}",
                                                    gameID, oldMax.ToString(), newMax.ToString());
                    }
                }
                else
                    _gameMax.Add(gameID, points);
                /////////////////////////////
                double oldMean = 0;
                if (_gameMean.TryGetValue(gameID, out oldMean))
                {
                    int myCount;
                    double myPoints;
                    _gameCount.TryGetValue(gameID, out myCount);
                    _gamePoints.TryGetValue(gameID, out myPoints);
                    double newMean = myPoints / myCount;
                    _gameMean.Remove(gameID);
                    _gameMean.Add(gameID, newMean);
                    Log2.Trace("Game Mean Accumulator {0}: OLD = {1}, NEW = {2}",
                                                gameID, oldMean.ToString(), newMean.ToString());
                }
                else
                    _gameMean.Add(gameID, points);


                //Needed for std dev only

                if (_enableStatistics == "true")
                {
                    List<double> oldList;
                    if (_gameRawPoints.TryGetValue(gameID, out oldList))
                    {
                        oldList.Add(points);
                        Log2.Trace("Game Raw Points List {0} {1}", gameID, points);
                    }
                    else
                    {
                        List<double> myList = new List<double>();
                        myList.Add(points);
                        _gameRawPoints.Add(gameID, myList);
                    }
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns></returns>
        private double StandardDeviation(IEnumerable<double> sequence)
        {
            double result = 0;

            if (sequence.Any())
            {
                double average = sequence.Average();
                double sum = sequence.Sum(d => Math.Pow(d - average, 2));
                result = Math.Sqrt((sum) / (sequence.Count() - 1));
            }
            return result;
        }

      
        /// <summary>
        /// 
        /// </summary>
        public void LoadTestList()
        {
            List<double> oldList;
            if (_gameRawPoints.TryGetValue("Game637611093123595188", out oldList))
            {
                oldList.Add(1.0);
                oldList.Add(2.0);
                oldList.Add(3.0);
                oldList.Add(4.0);
                oldList.Add(5.0);
                oldList.Add(6.0);
                oldList.Add(7.0);
                oldList.Add(8.0);
                oldList.Add(9.0);
                oldList.Add(10.0);
                oldList.Add(11.0);
                oldList.Add(12.0);
                oldList.Add(13.0);
                oldList.Add(14.0);
                oldList.Add(15.0);
                oldList.Add(16.0);
                oldList.Add(17.0);
                oldList.Add(18.0);
                oldList.Add(19.0);
                oldList.Add(20.0);
                Log2.Debug("LoadTestList");
            }
            List<double> oldList1;
            if (_gameRawPoints.TryGetValue("Game637611101269286599", out oldList1))
            {
                oldList1.Add(10.0);
                oldList1.Add(20.0);
                oldList1.Add(30.0);
                oldList1.Add(40.0);
                oldList1.Add(50.0);
                oldList1.Add(60.0);
                oldList1.Add(70.0);
                oldList1.Add(80.0);
                oldList1.Add(90.0);
                oldList1.Add(100.0);
                oldList1.Add(110.0);
                oldList1.Add(120.0);
                oldList1.Add(130.0);
                oldList1.Add(140.0);
                oldList1.Add(150.0);
                oldList1.Add(160.0);
                oldList1.Add(170.0);
                oldList1.Add(180.0);
                oldList1.Add(190.0);
                oldList1.Add(200.0);
                Log2.Debug("LoadTestList");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void CalcStats()
        {
            if (_enableStatistics == "true")
            {
                foreach (var gameID in _gameRawPoints)
                {
                    try
                    {
                        List<double> myList;
                        if (_gameRawPoints.TryGetValue(gameID.Key, out myList))
                        {
                            double myStdDev = StandardDeviation(myList);
                            _gameStdDev.Add(gameID.Key, myStdDev);
                        }
                        //Log2.Info("CalcStats Completed");
                    }
                    catch (Exception ex)
                    {
                        Log2.Error("CalcStats: {0}", ex.Message);
                    }
                }
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        public void LogAllPointsAndValues()
        { 

            foreach (var element in _gamePoints)
            {
                double value = 0;
                if (_gameValue.TryGetValue(element.Key, out value))
                {
                    double watts = 0;
                    if (_gameDeltaWatts.TryGetValue(element.Key, out watts))
                    {
                        double percentpoints = 0;
                        if (_gamePercentPoints.TryGetValue(element.Key, out percentpoints))
                        {
                            double min = 0;
                            _gameMin.TryGetValue(element.Key, out min);
                            double max = 0;
                            _gameMax.TryGetValue(element.Key, out max);
                            double mean = 0;
                            _gameMean.TryGetValue(element.Key, out mean);
                            int count = 0;
                            _gameCount.TryGetValue(element.Key, out count);
                            double stdDev = 0;
                            _gameStdDev.TryGetValue(element.Key, out stdDev);


                            string events =
                                //"\"" + element.Key + "\",\"" + watts + "\",\"" + element.Value + "\",\"" + percentpoints + "\",\"" + value + "\"";
                                "\"" + element.Key + "\",\"" + watts + "\",\"" + element.Value + "\",\"" + percentpoints + "\",\"" + value + "\",\"" + min + "\",\"" + max + "\",\"" + count + "\",\"" + mean + "\",\"" + stdDev + "\"";
                            Log2.Trace(events);
                        }
                        else
                        {
                            Log2.Error("Matching Value not found. This should never happen.");
                        }
                    }
                    else
                    {
                        Log2.Error("Matching Value not found. This should never happen.");
                    }
                }
                else
                {
                    Log2.Error("Matching Value not found. This should never happen.");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        public void LogAllPointsAndValuesToFile(string filePath)
        {
            using (StreamWriter outputFile = new StreamWriter(filePath, true))
            {
                //string header =
                //    "\"" +  "GameID" + "\",\"" + "TotalWatts" + "\",\"" + "TotalPoints" + "\",\"" + "TotalPercentPoints" + "\",\"" + "TotalValue" + "\"";
                //outputFile.WriteLine(header);
                
                foreach (var element in _gamePoints)
                {
                    double value = 0;
                    if (_gameValue.TryGetValue(element.Key, out value)) //Date,value = points
                    {
                        double watts = 0;
                        if (_gameDeltaWatts.TryGetValue(element.Key, out watts))
                        {
                            double percentpoints = 0;
                            if (_gamePercentPoints.TryGetValue(element.Key, out percentpoints))
                            {
                                double min = 0;
                                _gameMin.TryGetValue(element.Key, out min);
                                double max = 0;
                                _gameMax.TryGetValue(element.Key, out max);
                                double mean = 0;
                                _gameMean.TryGetValue(element.Key, out mean);
                                int count = 0;
                                _gameCount.TryGetValue(element.Key, out count);
                                double stdDev = 0;
                                _gameStdDev.TryGetValue(element.Key, out stdDev);

                                string events =
//                                "\"" + element.Key + "\",\"" + watts + "\",\"" + element.Value + "\",\"" + percentpoints + "\",\"" + value + "\",\"" + min + "\",\"" + max + "\",\"" + mean + "\",\"" + "N/A" + "\"";
                                "\"" + element.Key + "\",\"" + watts + "\",\"" + element.Value + "\",\"" + percentpoints + "\",\"" + value + "\",\"" + min + "\",\"" + max + "\",\"" + count + "\",\"" + mean + "\",\"" + stdDev + "\"";
                                outputFile.WriteLine(events);
                            }
                            else
                            {
                                Log2.Error("Matching Value not found. This should never happen.");
                            }
                        }
                        else
                        {
                            Log2.Error("Matching Value not found. This should never happen.");
                        }
                    }
                    else
                    {
                        Log2.Error("Matching Value not found. This should never happen.");
                    }
                }
            }
        }

        #endregion

        #region Private State
      
        private Dictionary<string, double> _gamePoints;
        private Dictionary<string, double> _gameValue;
        private Dictionary<string, double> _gamePercentPoints;
        private Dictionary<string, double> _gameDeltaWatts;
        private Dictionary<string, int> _gameCount;
        private Dictionary<string, double> _gameMin;
        private Dictionary<string, double> _gameMax;
        private Dictionary<string, double> _gameMean;

        private Dictionary<string, double> _gameStdDev;
        private Dictionary<string, List<double>> _gameRawPoints;
        private string _enableStatistics = "true";

        private readonly object awardLock = new object();
        #endregion
    }
}
