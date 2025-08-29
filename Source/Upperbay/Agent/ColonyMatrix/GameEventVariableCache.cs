//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
using System.Collections.Concurrent;
using Upperbay.Agent.Interfaces;

namespace Upperbay.Agent.ColonyMatrix
{
    /// <summary>
    /// Static Class using ConcurrentQueue
    /// Used as high speed shared memory for threads executing in the agent colony process
    /// </summary>
    public static class GameEventVariableCache
	{
        #region Methods
        public static GameEventVariable ReadEventQueue()
		{
				if (_eventQueue.Count > 0)
				{
					GameEventVariable ev;
					_eventQueue.TryDequeue(out ev);
					return ev;
				}
				else
					return null;
		}

        public static void WriteEventQueue(GameEventVariable o)
        {
			_eventQueue.Enqueue(o);
        }

        #endregion

        #region Private State

        private static ConcurrentQueue<GameEventVariable> _eventQueue = new ConcurrentQueue<GameEventVariable>();

        #endregion

    }
}
