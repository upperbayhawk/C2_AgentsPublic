//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Threading;

namespace Upperbay.Worker.Timers
{
    public class HiPerfTimer
    {
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);  

		[DllImport("Kernel32.dll")]
		private static extern bool QueryPerformanceFrequency(out long lpFrequency);
		
		private long _startTime, _stopTime;
		private long _freq;
		
        // Constructor
		public HiPerfTimer()
		{
            _startTime = 0;
            _stopTime  = 0;

            if (QueryPerformanceFrequency(out _freq) == false)
            {
                // high-performance counter not supported 
                throw new Win32Exception(); 
            }
		}
		
		// Start the timer
		public void Start()
		{
            // lets do the waiting threads there work
            Thread.Sleep(0);  

			QueryPerformanceCounter(out _startTime);
		}

		
		/// <summary>
		///  Stop the timer
		/// </summary>
		public void Stop()
		{
		    QueryPerformanceCounter(out _stopTime);
		}
		

		/// <summary>
		/// Returns the duration of the timer (in seconds)
		/// </summary>
        public double Duration
        {
        	get
        	{
            	return (double)(_stopTime - _startTime) / (double) _freq;
            }
        }

		/// <summary>
		/// Returns the duration of the timer (in milliseconds)
		/// </summary>
		public double MilliSeconds
		{
			get
			{
				return (double)(_stopTime - _startTime)/ (double) _freq *(double)(1000);
			}
		}
	}
}
