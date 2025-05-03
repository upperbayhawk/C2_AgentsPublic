//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
using System;

using Upperbay.Core.Logging;
using Upperbay.Agent.Interfaces;

namespace Upperbay.Assistant
{
    class GameEvent
    {
        #region Methods
        public GameEvent(GameEventVariable gev)
        {
            _gameEventVariable = gev;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            if ((!GameRunning) && (!GameFinished))
            {
                //TimeSpan timeSpan = new TimeSpan();
                TimeSpan timeSpan = _gameEventVariable.StartTime - DateTime.Now;

                if (timeSpan.TotalSeconds >= -20 && timeSpan.TotalSeconds <= 20)
                {
                    Log2.Debug("GAME JUST STARTED!!!!!");
                    GameRunning = true;
                    GameStatus.SetState("on");
                    GameStatus.SetLevel(_gameEventVariable.GameAwardRank);
                    return true;
                }
                else
                {
                    //Log2.Debug("Game will start in " + timeSpan.TotalSeconds.ToString() + " seconds");
                    GameAction gameAction = new GameAction(_gameEventVariable);
                    gameAction.CountdownToStartAction(timeSpan);
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsFinished()
        {
            if ((GameRunning) && (!GameFinished))
            {
                //Add 1 minute to make sure the latest meter data has been updated
                DateTime finishTime = _gameEventVariable.StartTime.AddMinutes(_gameEventVariable.DurationInMinutes + 1);
                TimeSpan timeSpan = finishTime - DateTime.Now;
                if (timeSpan.TotalSeconds >= -20 && timeSpan.TotalSeconds <= 20)
                {
                    Log2.Debug("GAME FINISHED!!!!!");
                    GameRunning = false;
                    GameFinished = true;
                    GameStatus.SetState("off");
                    GameStatus.SetLevel("NONE");
                    return true;
                }
                else
                {
                    //Log2.Debug("Game will End in " + timeSpan.TotalSeconds.ToString() + " seconds");
                    GameAction gameAction = new GameAction(_gameEventVariable);
                    gameAction.CountdownToFinishAction(timeSpan);
                }
                return false;
            }
            return false;
        }

        #endregion

        #region Private State
        private bool _gameRunning = false;
        public bool GameRunning { get { return this._gameRunning; } set { this._gameRunning = value; } }

        private bool _gameFinished = false;
        public bool GameFinished { get { return this._gameFinished; } set { this._gameFinished = value; } }

        private GameEventVariable _gameEventVariable;
        public GameEventVariable GameEventVariable { get { return this._gameEventVariable; } set { this._gameEventVariable = value; } }
        #endregion
    }


}
