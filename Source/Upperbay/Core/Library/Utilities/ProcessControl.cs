//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================

/*
* **************************************************************************
* 
* FILE NAME: ServiceEventLogger.cs
*
* Author: Dave Hardin
*
* COMPONENT HISTORY
* 
*   Date       Name    Reason for Change
* ----------  ------  ------------------------------------------------------
* 04/07/02     DBH		Initial version
* **************************************************************************
*/
using System;
using System.Runtime.InteropServices;


namespace Upperbay.Core.Library
{
    /// <summary>
    /// Thread safe EventLogger.
    /// </summary>
    [ComVisible(false)]
	[ClassInterface(ClassInterfaceType.None)]
	public class ServiceEnvironment
	{

		private const int NORMAL_PRIORITY_CLASS = 0x00000020;
		private const int IDLE_PRIORITY_CLASS = 0x00000040;	
		private const int HIGH_PRIORITY_CLASS = 0x00000080;
		private const int REALTIME_PRIORITY_CLASS = 0x00000100;

		private const int THREAD_PRIORITY_IDLE = -15;
		private const int THREAD_PRIORITY_LOWEST = -2;
		private const int THREAD_PRIORITY_BELOW_NORMAL = -1;
		private const int THREAD_PRIORITY_NORMAL = 0;
		private const int THREAD_PRIORITY_ABOVE_NORMAL = 1;
		private const int THREAD_PRIORITY_HIGHEST = 2;
		private const int THREAD_PRIORITY_TIME_CRITICAL = 15;

		[DllImport("kernel32.dll")]
		public static extern uint GetCurrentProcess();
		[DllImport("kernel32.dll")]
		public static extern int SetPriorityClass(uint ProcessHandle, int ClassPriorityClass);
		[DllImport("kernel32.dll")]
		public static extern int SetThreadPriority(uint ProcessHandle, int ThreadPriorityClass);
		[DllImport("user32.dll")]
		public static extern int MessageBeep(int uType);

		// Private Member Variables

		public ServiceEnvironment()
		{
		}

        
		//
		// IEnvironment Methods
		//

		[ComVisible(false)]
		public bool Initialize( )
		{
			return true;
		}


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool SetProcessHigh()
        {
            ServiceEnvironment env = new ServiceEnvironment();
            SetPriority("HIGH");
            return (true);
        }

        public static bool SetProcessRealtime()
        {
            ServiceEnvironment env = new ServiceEnvironment();
            SetPriority("REALTIME");
            return (true);
        }


		[ComVisible(false)]
		public static bool SetPriority(String soPriority)
		{
			uint hMyProcess = GetCurrentProcess();

			String soMyPriority = soPriority.ToUpper();
			if (soMyPriority == "REALTIME")
			{
				SetPriorityClass(hMyProcess,REALTIME_PRIORITY_CLASS);
			}
			else if (soMyPriority == "HIGH")
			{
				SetPriorityClass(hMyProcess,HIGH_PRIORITY_CLASS);
			}
			else if (soMyPriority == "NORMAL")
			{
				SetPriorityClass(hMyProcess,NORMAL_PRIORITY_CLASS);
			}
			else if (soMyPriority == "LOW")
			{
				SetPriorityClass(hMyProcess,IDLE_PRIORITY_CLASS);
			}

			return true;
		}

		[ComVisible(false)]
		public static bool SetThreadPriority(String soThreadPriority)
		{

			String soMyThreadPriority = soThreadPriority.ToUpper();

			uint hMyProcess = GetCurrentProcess();

			if (soMyThreadPriority == "IDLE")
			{
				SetThreadPriority(hMyProcess,THREAD_PRIORITY_IDLE);
			}
			else if (soMyThreadPriority == "LOWEST")
			{
				SetThreadPriority(hMyProcess,THREAD_PRIORITY_LOWEST);
			}
			else if (soMyThreadPriority == "BELOW_NORMAL")
			{
				SetThreadPriority(hMyProcess,THREAD_PRIORITY_BELOW_NORMAL);
			}
			else if (soMyThreadPriority == "NORMAL")
			{
				SetThreadPriority(hMyProcess,THREAD_PRIORITY_NORMAL);
			}
			else if (soMyThreadPriority == "ABOVE_NORMAL")
			{
				SetThreadPriority(hMyProcess,THREAD_PRIORITY_ABOVE_NORMAL);
			}
			else if (soMyThreadPriority == "HIGHEST")
			{
				SetThreadPriority(hMyProcess,THREAD_PRIORITY_HIGHEST);
			}
			else if (soMyThreadPriority == "TIME_CRITICAL")
			{
				SetThreadPriority(hMyProcess,THREAD_PRIORITY_TIME_CRITICAL);
			}
			return true;
		}


		[ComVisible(false)]
		public void Beep(int lBeep)
		{
			MessageBeep(lBeep);
		}
	}
}

