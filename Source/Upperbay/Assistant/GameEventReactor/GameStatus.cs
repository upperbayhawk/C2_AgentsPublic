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

namespace Upperbay.Assistant
{
    public static class GameStatus
    {
        public static void SetState (string state)
        {
            _gameState = state;
        }
        public static string GetState()
        {
            return _gameState;
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
        public static string GetLevel()
        {
            return _gameLevel;
        }
        private static string _gameState = "off";
        private static string _gameLevel = "0";
    }
}
