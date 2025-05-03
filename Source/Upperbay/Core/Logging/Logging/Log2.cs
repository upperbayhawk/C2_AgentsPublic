//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
using System;
using System.Threading;
using Serilog;
using Serilog.Core;
using Serilog.Events;

/// <summary>
/// Standard Agent Priority Enumeration
/// </summary>
public enum Priority { LowLow = 0, Low = 1, Med = 2, High = 3, HighHigh = 4 };



namespace Upperbay.Core.Logging
{
   
    /// <summary>
    /// 
    /// </summary>
    public static class Log2
    {
        //private static int eventID = 0;
        public static string AgentName;
        public static string AgentType;
        public static string ProcessName;
        private static bool IsInitialized = false;
        public static LoggingLevelSwitch LevelSwitch;


        public static bool SetLoggingLevel(string level)
        {
            if (level.ToLower() == "debug")
            {
                LevelSwitch.MinimumLevel = LogEventLevel.Debug;
            }
            else if (level.ToLower() == "trace")
            {
                LevelSwitch.MinimumLevel = LogEventLevel.Verbose;
            }
            else if (level.ToLower() == "verbose")
            {
                LevelSwitch.MinimumLevel = LogEventLevel.Verbose;
            }
            else if (level.ToLower() == "info")
            {
                LevelSwitch.MinimumLevel = LogEventLevel.Information;
            }
            else if (level.ToLower() == "information")
            {
                LevelSwitch.MinimumLevel = LogEventLevel.Information;
            }
            else
            {
                LevelSwitch.MinimumLevel = LogEventLevel.Information;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static bool LogInit(string type,string name, string level)
        {

            // Add Windows Event Log Sink
            if (IsInitialized == true) return true;

            LevelSwitch = new LoggingLevelSwitch();
            LevelSwitch.MinimumLevel = LogEventLevel.Information;

            AgentType = type;
            AgentName = name;
            ProcessName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
           

            string logfilename = String.Format(
                    "logs\\{0}_log.txt",
                        ProcessName);

            Log.Logger = new LoggerConfiguration()
                  .MinimumLevel.ControlledBy(LevelSwitch)
                  .WriteTo.Debug()
                  //.WriteTo.File(logfilename, rollingInterval: RollingInterval.Day)
                  .WriteTo.File(logfilename, 
                                fileSizeLimitBytes: 524288000, 
                                retainedFileCountLimit: 40, 
                                rollOnFileSizeLimit: true, 
                                rollingInterval: RollingInterval.Day)
                  .CreateLogger();

            if (level.ToLower() == "debug")
            {
                LevelSwitch.MinimumLevel = LogEventLevel.Debug;
            }
            else if (level.ToLower() == "trace")
            {
                LevelSwitch.MinimumLevel = LogEventLevel.Verbose;
            }
            else if (level.ToLower() == "verbose")
            {
                LevelSwitch.MinimumLevel = LogEventLevel.Verbose;
            }
            else if (level.ToLower() == "info")
            {
                LevelSwitch.MinimumLevel = LogEventLevel.Information;
            }
            else if (level.ToLower() == "information")
            {
                LevelSwitch.MinimumLevel = LogEventLevel.Information;
            }
            else // default
            {
                LevelSwitch.MinimumLevel = LogEventLevel.Information;
            }

            Log.Information("HELLO WORLD!");
            IsInitialized = true;
            return true;
        }

        //-------------------------Special BootLog------------------------------------------

        /// <summary>
        /// Log to debug console when nothing else is running.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static bool DebugLogger(string message)
        {
            try
            {
                // Debug.WriteLine(AgentName + ": " + message);
                return (true);
            }
            catch
            {
                return false;
            }
        }


        //-------------------------AgentAlarm------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool AgentAlarm(string message)
        {
            try
            {
                string s = "AgentAlarm: " + AgentName + ": " + message;
                DebugLogger(s);
                Log.Information(s);
                return (true);
            }
            catch (Exception ex)
            {
                Log2.Error("Log2 Failed: {0}", ex.ToString());
                return false;            
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static bool AgentAlarm(string message, params object[] args)
        {
            try
            { 
                string s = String.Format("AgentAlarm: " + AgentName + ": " + message, args);
                DebugLogger(s);
                Log.Information(s);
                return (true);
            }
            catch (Exception ex)
            {
                Log2.Error("Log2 Failed: {0}", ex.ToString());
                return false;
            }
        }



        /// <summary>
        /// LogMail - Log a message to the Mail Sink
        /// </summary>
        /// <param name="message"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        public static bool AgentAlarm(string message, Priority priority)
        {
            try
            { 
                string s = "AgentAlarm: " + AgentName + ": " + message;
                DebugLogger(s);
                Log.Information(s);
                return (true);
            }
            catch (Exception ex)
            {
                Log2.Error("Log2 Failed: {0}", ex.ToString());
                return false;
            }

        }


        //-------------------------AgentMessage------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool AgentMessage(string message)
        {
            try
            {
                string s = "AgentMessage: " + AgentName + ": " + message;
                DebugLogger(s);
                Log.Information(s);
                return (true);
            }
            catch (Exception ex)
            {
                Log2.Error("Log2 Failed: {0}", ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static bool AgentMessage(string message, params object[] args)
        {
            try
            {
                string s = String.Format("AgentMessage: " + AgentName + ": " + message, args);
                DebugLogger(s);
                Log.Information(s);
                return (true);
            }
            catch (Exception ex)
            {
                Log2.Error("Log2 Failed: {0}", ex.ToString());
                return false;
            }
        }

             

        //-------------------------Mail------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool Mail(string message)
        {
            try
            {
                string s = "MAIL: " + AgentName + ": " + message;
                DebugLogger(s); 
                Log.Information(s);
                return (true);
            }
            catch (Exception ex)
            {
                Log2.Error("Log2 Failed: {0}", ex.ToString());
                return false;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static bool Mail(string message, params object[] args)
        {
            try 
            { 
                string s = "MAIL: " + String.Format(AgentName + ": " + message, args);
                DebugLogger(s); 
                Log.Information(s);
                return (true);
            }
            catch (Exception ex)
            {
                Log2.Error("Log2 Failed: {0}", ex.ToString());
                return false;
            }
        }



//-------------------------Trace------------------------------------------

        /// <summary>
        /// Log a trace message to the Trace Sink
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool Trace(string message)
        {
            
            try
            {
                string agentName; 
                Thread thread = Thread.CurrentThread;
                agentName= thread.Name;
                if (agentName == null) agentName = ProcessName;
                string s = agentName + ": " + message;
                DebugLogger(s);
                Log.Verbose(s);
                return (true);
            }
            catch (Exception ex)
            {
                Log2.Error("Log2 Failed: {0}", ex.ToString());
                return false;
            }
        }
       
        /// <summary>
        /// Log a trace message to the Trace Sink
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool Trace(string message, params object[] args)
        {
            try 
            {
                string agentName;
                Thread thread = Thread.CurrentThread;
                agentName = thread.Name;
                if (agentName == null) agentName = ProcessName;
                string s = String.Format(agentName + ": " + message, args);
                DebugLogger(s);
                Log.Verbose(s);
                return (true);
            }
            catch (Exception ex)
            {
                Log2.Error("Log2 Failed: {0}", ex.ToString());
                return false;
            }
        }

        //-------------------------Debug------------------------------------------

        /// <summary>
        /// Log a trace message to the Trace Sink
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool Debug(string message)
        {
            try
            {
                string agentName;
                Thread thread = Thread.CurrentThread;
                agentName = thread.Name;
                if (agentName == null) agentName = ProcessName;
                string s = agentName + ": " + message;
                DebugLogger(s);
                Log.Debug(s);
                return (true);
            }
            catch (Exception ex)
            {
                Log2.Error("Log2 Failed: {0}", ex.ToString());
                return false;
            }
        }


        /// <summary>
        /// Log a trace message to the Trace Sink
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool Debug(string message, params object[] args)
        {
            try
            {
                string agentName;
                Thread thread = Thread.CurrentThread;
                agentName = thread.Name;
                if (agentName == null) agentName = ProcessName;
                string s = String.Format(agentName + ": " + message, args);
                DebugLogger(s);
                Log.Debug(s);
                return (true);
            }
            catch (Exception ex)
            {
                Log2.Error("Log2 Failed: {0}", ex.ToString());
                return false;
            }
        }


        //-------------------------Info------------------------------------------

        /// <summary>
        /// Log an informational message to the Info Sink
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool Info(string message)
        {
            try
            {
                string agentName;
                Thread thread = Thread.CurrentThread;
                agentName = thread.Name;
                if (agentName == null) agentName = ProcessName;
                string s = agentName + ": " + message;
                DebugLogger(s);
                Log.Information(s);
                return (true);
            }
            catch (Exception ex)
            {
                Log2.Error("Log2 Failed: {0}", ex.ToString());
                return false;
            }
        }
  
        /// <summary>
        /// Log an informational message to the Info Sink
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool Info(string message, params object[] args)
        {
            try
            {
                string agentName;
                Thread thread = Thread.CurrentThread;
                agentName = thread.Name;
                if (agentName == null) agentName = ProcessName;
                string s = String.Format(agentName + ": " + message, args);
                DebugLogger(s);
                Log.Information(s);
                return (true);
            }
            catch (Exception ex)
            {
                Log2.Error("Log2 Failed: {0}", ex.ToString());
                return false;
            }

        }



        /// <summary>
        /// Log an informational message to the Info Sink
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool Event(string message, params object[] args)
         {
            try
            {
                string agentName;
                Thread thread = Thread.CurrentThread;
                agentName = thread.Name;
                if (agentName == null) agentName = ProcessName;
                string s = String.Format(agentName + ": " + message, args);
                DebugLogger(s);
                Log.Information(s);
                return (true);
            }
            catch (Exception ex)
            {
                Log2.Error("Log2 Failed: {0}", ex.ToString());
                return false;
            }
        }



       
        //-------------------------Error------------------------------------------

        /// <summary>
        /// Log an error message to the Error Sink
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool Error(string message)
        {
            try 
            {
                string agentName;
                Thread thread = Thread.CurrentThread;
                agentName = thread.Name;
                if (agentName == null) agentName = ProcessName;
                string s = agentName + ": " + message;
                DebugLogger(s);
                Log.Error(s);
                return (true);
            }
            catch (Exception ex)
            {
                Log2.Error("Log2 Failed: {0}", ex.ToString());
                return false;
            }

        }



        /// <summary>
        /// Log an error message to the Error Sink
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool Error(string message, params object[] args)
        {
            try
            {
                string agentName;
                Thread thread = Thread.CurrentThread;
                agentName = thread.Name;
                if (agentName == null) agentName = ProcessName;
                string s = String.Format(agentName + ": " + message, args);
                DebugLogger(s);
                Log.Error(s);
                return (true);
            }
            catch (Exception ex)
            {
                Log2.Error("Log2 Failed: {0}", ex.ToString());
                return false;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool WmiEvent(string message, params object[] args)
        {
            try
            {
                string agentName;
                Thread thread = Thread.CurrentThread;
                agentName = thread.Name;
                if (agentName == null) agentName = ProcessName;
                string s = String.Format(agentName + ": " + message, args);
                DebugLogger(s);
                Log.Information(s);
                return (true);
            }
            catch (Exception ex)
            {
                Log2.Error("Log2 Failed: {0}", ex.ToString());
                return false;
            }

        }
    }
}
