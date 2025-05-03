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
using Upperbay.Worker.ODBC;

namespace Upperbay.Worker.EtherAccess
{
    public class YearToDatePlayerAwards
    {
        #region Methods

        /// <summary>
        /// Roll up data for a playerID
        /// </summary>
        /// <param name="playerID"></param>
        public YearToDatePlayerAwards(string playerID)
        {
            _playerID = playerID;
            _totalGamesPlayed = 0;
            _YtdPoints = new Dictionary<string, double>();
            _YtdValue = new Dictionary<string, double>();
            _YtdPercentPoints = new Dictionary<string, double>();
            _YtdDeltaWatts = new Dictionary<string, double>();
            // Log2.Info("YearToDateC2CAwards Created");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="points"></param>
        /// <param name="value"></param>
        public void AddPointsAndValue(string year, 
                                    double points, 
                                    double percentPoints, 
                                    double value, 
                                    double watts, 
                                    string gameTime)
        {
            try
            {
                DateTime myDateTime;
                if (DateTime.TryParse(gameTime, out myDateTime))
                {
                    if (myDateTime > _lastGameTime)
                    {
                        _lastGameTime = myDateTime;
                    }
                }
            }
            catch (Exception ex)
            {
                Log2.Error("Bad DateTime conversion {0}", ex.Message);
            }

            _totalGamesPlayed = _totalGamesPlayed + 1;


            double oldPoints = 0;
            if(_YtdPoints.TryGetValue(year, out oldPoints))
            {
                double newPoints = 0;
                
                newPoints = oldPoints + points;
                _YtdPoints.Remove(year);
                _YtdPoints.Add(year, newPoints); 
                Log2.Trace("YTD Points Accumulator {0}: {1}, OLD = {2}, NEW = {3}", 
                                            _playerID, year, oldPoints.ToString(), newPoints.ToString());
            }
            else
                _YtdPoints.Add(year, points);

            /////////////////////////////

            double oldPercentPoints = 0;
            if (_YtdPercentPoints.TryGetValue(year, out oldPercentPoints))
            {
                double newPercentPoints = 0;

                newPercentPoints = oldPercentPoints + percentPoints;
                _YtdPercentPoints.Remove(year);
                _YtdPercentPoints.Add(year, newPercentPoints);
                Log2.Trace("YTD PercentPoints Accumulator {0}: {1}, OLD = {2}, NEW = {3}",
                            _playerID, year, oldPercentPoints.ToString(), newPercentPoints.ToString());
            }
            else
                _YtdPercentPoints.Add(year, percentPoints);

            //////////////////////////////

            double oldValue = 0;
            if (_YtdValue.TryGetValue(year, out oldValue))
            {
                double newValue = 0;

                newValue = oldValue + value;
                _YtdValue.Remove(year);
                _YtdValue.Add(year, newValue);
                Log2.Trace("YTD Value Accumulator {0}: {1}, OLD = {2}, NEW = {3}", 
                                _playerID, year, oldValue.ToString(), newValue.ToString());
            }
            else
                _YtdValue.Add(year, value);


            //////////////////////////////

            double oldWatts = 0;
            if (_YtdDeltaWatts.TryGetValue(year, out oldWatts))
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
                _YtdDeltaWatts.Remove(year);
                _YtdDeltaWatts.Add(year, newWatts);
                Log2.Trace("YTD Watts Accumulator {0}: {1}, OLD = {2}, NEW = {3}", 
                                _playerID, year, oldWatts.ToString(), newWatts.ToString());
            }
            else
                _YtdDeltaWatts.Add(year, watts);
        }



        /// <summary>
        /// 
        /// </summary>
        public void LogAllPointsAndValues()
        { 
            string nowDate = DateTime.Now.ToString("MM/dd/yyyy");

            foreach (var element in _YtdPoints)
            {
                double value = 0;
                if (_YtdValue.TryGetValue(element.Key, out value))
                {
                    double watts = 0;
                    if (_YtdDeltaWatts.TryGetValue(element.Key, out watts))
                    {
                        double percentpoints = 0;
                        if (_YtdPercentPoints.TryGetValue(element.Key, out percentpoints))
                        {
                            string events =
                                "\"" + _playerID + "\",\"" + nowDate + "\",\"" + element.Key + "\",\"" + watts + "\",\"" + element.Value + "\",\"" + percentpoints + "\",\"" + value + "\"";
                            Log2.Debug(events);
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
        public void LogMyPointsAndValuesToFile(string filePath)
        {
            string nowDate = DateTime.Now.ToString("MM/dd/yyyy");

            using (StreamWriter outputFile = new StreamWriter(filePath, false))
            {
                string header =
                        "\"" + "GamePlayerID" + "\",\"" + "Date" + "\",\"" + "Year" + "\",\"" + "TotalWatts" + "\",\"" + "TotalPoints" + "\",\"" + "TotalPercentPoints" + "\",\"" + "TotalValue" + "\"";
                outputFile.WriteLine(header);

                foreach (var element in _YtdPoints)
                {
                    double value = 0;
                    if (_YtdValue.TryGetValue(element.Key, out value))
                    {
                        double watts = 0;
                        if (_YtdDeltaWatts.TryGetValue(element.Key, out watts))
                        {
                            double percentpoints = 0;
                            if (_YtdPercentPoints.TryGetValue(element.Key, out percentpoints))
                            {
                                string events =
                                    "\"" + _playerID + "\",\"" + nowDate + "\",\"" + element.Key + "\",\"" + watts + "\",\"" + element.Value + "\",\"" + percentpoints + "\",\"" + value + "\"";
                                outputFile.WriteLine(events);
                                try
                                {
                                    string thisYear = DateTime.Now.Year.ToString();
                                    if (thisYear == element.Key.ToString())
                                    {
                                        var xmlDoc = new XmlDocument();
                                        string path = Directory.GetCurrentDirectory();
                                        string fullPath = path + "\\config\\ClusterAgent.app.config";
                                        Log2.Trace("YTD Config file = {0}, {1}, {2}, {3}", fullPath, element.Key.ToString(), element.Value.ToString(), value.ToString());
                                        xmlDoc.Load(fullPath);
                                        xmlDoc.SelectSingleNode("//appSettings/add[@key='GameLatestYTDYear']").Attributes["value"].Value = element.Key.ToString();
                                        xmlDoc.SelectSingleNode("//appSettings/add[@key='GameLatestYTDPoints']").Attributes["value"].Value = element.Value.ToString();
                                        xmlDoc.SelectSingleNode("//appSettings/add[@key='GameLatestYTDAwards']").Attributes["value"].Value = value.ToString();
                                        xmlDoc.Save(fullPath);
                                    }
                                }
                                catch(Exception ex)
                                {
                                    Log2.Error("Can't write to app.config file. {0}", ex.Message);
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
                    else
                    {
                        Log2.Error("Matching Value not found. This should never happen.");
                    }
                }
            }
        }

        /// <summary>
        /// Update _lastGameTime 
        /// </summary>
        public void LogMyLastGameTimeToODBC()
        {
            try
            {
                ODBCDatabaseDriver odbcDatabaseDriver = new ODBCDatabaseDriver();
                odbcDatabaseDriver.Init();
                odbcDatabaseDriver.UpdatePlayerLastGameTime(_playerID, _lastGameTime.ToString());
                odbcDatabaseDriver.UpdatePlayerTotalGamesPlayed(_playerID, _totalGamesPlayed.ToString());
            }
            catch (Exception ex)
            {
                Log2.Error("LogMyLastGameTimeToODBC Failed: {0}", ex.Message);
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
                string nowDate = DateTime.Now.ToString("MM/dd/yyyy");

                //string header =
                //    "\"" + "GamePlayerID" + "\",\"" + "Date" + "\",\"" + "Year" + "\",\"" + "TotalWatts" + "\",\"" + "TotalPoints" + "\",\"" + "TotalPercentPoints" + "\",\"" + "TotalValue" + "\"";
                //outputFile.WriteLine(header);
                
                foreach (var element in _YtdPoints)
                {
                    double value = 0;
                    if (_YtdValue.TryGetValue(element.Key, out value)) //Date,value = points
                    {
                        double watts = 0;
                        if (_YtdDeltaWatts.TryGetValue(element.Key, out watts))
                        {
                            double percentpoints = 0;
                            if (_YtdPercentPoints.TryGetValue(element.Key, out percentpoints))
                            {
                                string events =
                                "\"" + _playerID + "\",\"" + nowDate + "\",\"" + element.Key + "\",\"" + watts + "\",\"" + element.Value + "\",\"" + percentpoints + "\",\"" + value + "\"";
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
        private string _playerID;
        private Dictionary<string, double> _YtdPoints;
        private Dictionary<string, double> _YtdValue;
        private Dictionary<string, double> _YtdPercentPoints;
        private Dictionary<string, double> _YtdDeltaWatts;
        private DateTime _lastGameTime = new DateTime(1900,01,01);
        private Int64 _totalGamesPlayed = 0;
        #endregion
    }
}
