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
using System.Collections.Concurrent;
using Upperbay.Core.Logging;

namespace Upperbay.Worker.EtherAccess
{
    public class GameAllStatistics
    {
        public static bool InitAll()
        {

            AllYTDAwards = new ConcurrentDictionary<string, YearToDatePlayerAwards>(16,500);
            gameAwards = new GameAwards();
            return true;
        }

        public static bool CalcAllGameStatistics()
        {
            Log2.Debug("CalcAllGameStatistics");
            //Write Player Year to Date file
            try
            {
                gameAwards.CalcStats();

                string myFilePath = "logs\\PlayerYearToDate.csv";
                if (File.Exists(myFilePath))
                    File.Delete(myFilePath);

                foreach (var player in AllYTDAwards)
                {
                    string playerID = player.Key;
                    YearToDatePlayerAwards PlayerAwards = player.Value;
                    PlayerAwards.LogAllPointsAndValues();
                    PlayerAwards.LogAllPointsAndValuesToFile(myFilePath);
                    PlayerAwards.LogMyLastGameTimeToODBC();
                }

                string myFilePath1 = "logs\\AllGameAwards.csv";
                if (File.Exists(myFilePath1))
                    File.Delete(myFilePath1);

                gameAwards.LogAllPointsAndValues();
                gameAwards.LogAllPointsAndValuesToFile(myFilePath1);

            }
            catch (Exception ex)
            {
                Log2.Error("CalcAllGameStatistics {0}", ex.Message);
            }

            return true;
        }

        public static ConcurrentDictionary<string, YearToDatePlayerAwards> AllYTDAwards = null;
        public static GameAwards gameAwards = null;
    }
}
