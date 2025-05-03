//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
using System.Collections;
using System.Collections.Concurrent;
using Upperbay.Core.Logging;
using Upperbay.Agent.Interfaces;
using System.IO;
using Upperbay.Worker.JSON;

namespace Upperbay.Agent.ColonyMatrix
{
    /// <summary>
    /// Static Class using ConcurrentQueue
    /// Used as high speed shared memory for threads executing in the agent colony process
    /// </summary>
    public static class EventVariableCache
	{

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static EventVariable ReadEventQueue()
		{
				if (_eventQueue.Count > 0)
				{
					EventVariable ev;
					_eventQueue.TryDequeue(out ev);
					return ev;
				}
				else
					return null;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        public static void WriteEventQueue(EventVariable o)
        {
				_eventQueue.Enqueue(o);
        }

		public static void DumpQueue()
		{
				foreach (EventVariable ev in _eventQueue)
				{
					Log2.Trace("EVENTCACHE: {0}, {1}", (string)ev.EventName, (string)ev.EventType);
				}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
		public static void Queue2File(string fileName)
		{
			using (StreamWriter writer = new StreamWriter(fileName))
			{
				JsonEventVariable json = new JsonEventVariable();
				string s;
				foreach (EventVariable ev in _eventQueue)
				{
					s = json.EventVariable2Json(ev);
					Log2.Trace("EVENTFILE: {0}, {1}", (string)ev.EventName, (string)ev.EventType);
					writer.WriteLine(s);
				}
			}
		}
        #endregion

        #region Private State
        /// <summary>
        /// Private State
        /// </summary>
        private static Hashtable _alarmTable = new Hashtable(); //future
        private static ConcurrentQueue<EventVariable> _eventQueue = new ConcurrentQueue<EventVariable>();
        private static Hashtable _jsonAlarmHashCodeTable = new Hashtable(); //future
        private static object _queueLock = new object();

        #endregion
    }
}
