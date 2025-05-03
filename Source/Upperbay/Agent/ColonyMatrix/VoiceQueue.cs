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
    public static class VoiceQueue
	{
        /// <summary>
        /// TBD IN FUTURE
        /// </summary>
        /// <returns></returns>
        #region Methods
        public static string ReadVoiceQueue()
		{
				if (_voiceQueue.Count > 0)
				{
					string ev;
                    _voiceQueue.TryDequeue(out ev);
					return ev;
				}
				else
					return null;
		}

        public static void WriteVoiceQueue(string o)
        {
            _voiceQueue.Enqueue(o);
        }

        #endregion

        #region Private State

        private static ConcurrentQueue<string> _voiceQueue = new ConcurrentQueue<string>();

        #endregion

    }
}
