//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// .NET Agent Framework - A Job Scheduler and Notification Service
//
//
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

using System;
using System.Reflection;
using System.Threading;
using System.Timers;
using System.Diagnostics;

using Upperbay.Core.Library;
using Upperbay.Agent.Interfaces;

using Upperbay.Core.Logging;


namespace Upperbay.Worker.Events
{

  //============================================================================
  /// <summary>
  /// This is the .NET Agent Framework's base class for a manager.</summary>
  /// <remarks>
  /// The AgentManager has a collection of jobs and checks the jobs'
  /// schedules at regular intervals. The Start and Stop methods are
  /// used to control the manager, and the Init method may also be
  /// called directly. Classes that inherit from this class may override
  /// OnInit, OnStart, and OnStop to implement custom code.</remarks>
  //----------------------------------------------------------------------------
  [Serializable]
  public class EventFarm
  {

    string agentName;

    /// <summary>
    /// Use this TraceSwitch when tracing in derived classes.</summary>
    //protected static TraceSwitch DataFarmSwitch 
    //{
    //  get { return dataFarmSwitch; }
    //}

    /// <summary>
    /// True when the manager has started.</summary>
    public bool IsRunning 
    {
      get { return isRunning; }
    }

    /// <summary>
    /// Determine the interval at which the job schedules are checked.</summary>
    public TimeSpan HeartbeatFrequency 
    {
      get { return TimeSpan.FromMilliseconds(_timer.Interval); }
      set { _timer.Interval = value.TotalMilliseconds; }
    }

    /// <summary>
    /// Returns the collection of jobs under management.</summary>
    public BaseEventCollection Events 
    {
        get { return events; }
    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    // for logging & debugging
      //private static TraceSwitch dataFarmSwitch = new TraceSwitch("DataFarm", "DataFarm TraceSwitch");
     // private TraceSwitch _myTraceSwitch = Upperbay.Core.Logging.Log2.AgentSwitch;

    // current state of the manager
    private bool _isInitialized = false;
    private bool isRunning = false;

    // timer used to fire events to check the schedules
    private System.Timers.Timer _timer = new System.Timers.Timer();
		
    // The ManualResetEvent allows threads to communicate with each other.
    // In this case, I use it to signal that the Manager's Stop method
    // has been called so that the Timer's Elapsed event know whether or
    // not to do anything.
    private ManualResetEvent _stopSignal = new ManualResetEvent(false);

    // collection of Jobs loaded into the manager
    private BaseEventCollection events = new BaseEventCollection();


    // set/clear whether or not to collect garbage
    //private bool _collectGarbage = true;

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    /// <summary>
    /// Intializes the timer used by the manager.</summary>
    public EventFarm() 
    {
      // initialize the Timer control
      _timer = new System.Timers.Timer();
      _timer.Elapsed += new ElapsedEventHandler( Timer_Elapsed );
      _timer.AutoReset = true;
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    /// <summary>
    /// Intializes the manager, including calling OnInit.</summary>
    public void Init(string agentName) 
		{

          this.agentName = agentName;

          // logging

          Log2.Trace(string.Format("EventFarm: Enter Init on {0}",
              agentName));

          // derived classes may override OnInit() with their own initialization code
	      OnInit();

          // Initialize Devices
          foreach (BaseEvent eevent in this.events)
          {
              eevent.Init(this, agentName);
          }

        // record that Init() has been called
		_isInitialized = true;

        Log2.Trace(string.Format("EventFarm: Exit Init on {0}", agentName));

    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    /// <summary>
    /// Starts the manager, including calling OnStart.</summary>
    public void Start() 
    {

      // logging
        Log2.Trace(string.Format("EventFarm: Enter Start on {0}", agentName));

      // initialize, if not already done
        if (!_isInitialized)
            throw (new InvalidOperationException("Not Initialized."));

      // hook for derived classes to add custom Start code
			OnStart();

      // note status change
            isRunning = true;

      // reset Stop events
			_stopSignal.Reset();

      // start the heartbeat timer
      _timer.Interval = HeartbeatFrequency.TotalMilliseconds;
      _timer.Start();

      // logging
      Log2.Trace(string.Format("EventFarm: Exit Start on {0}", agentName));

    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    /// <summary>
    /// Stops the manager, including calling OnStop.</summary>
    public void Stop() 
		{
            Log2.Trace(string.Format("EventFarm: Enter Stop on {0}", agentName));

            if (!isRunning) 
                throw(new InvalidOperationException("Not started."));

          _stopSignal.Set();
          _timer.Stop();

          isRunning = false;

          // hook for derived classes to add custom Stop code
          OnStop();

          Log2.Trace(string.Format("EventFarm: Exit Stop on {0}", agentName));
      }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    /// <summary>
    /// Repeatedly called to check the jobs' schedules.</summary>
    protected void Timer_Elapsed(object sender, ElapsedEventArgs args) 
	{
      // The Timer is multi-threaded and the Elapsed event might might be called
      // even after the Stop method has been called. 
      if ( !_stopSignal.WaitOne(0, true) ) 
      {
          Log2.Trace(string.Format("EventFarm: Heartbeat on {0}", agentName));

           // bool anyJobsScheduled = false;
            try 
            {
                // Initialize Devices
                foreach (BaseEvent eevent in this.events)
                {
                    eevent.Value = eevent.Value + 1;
                }
            } 
            catch (Exception e) 
            {
                Log2.Error(string.Format("EventFarm: Exception in Timer_Elapsed: {0}", e.ToString()));
            }

            //Log2.TraceIf(dataFarmSwitch, string.Format("DataFarm: Exit Heartbeat on {0}", myHost.AgentName));
        }
	}

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    /// <summary>
    /// Provides a hook implement custom initialization code.</summary>
    protected virtual void OnInit() { }
		
    /// <summary>
    /// Provides a hook implement custom start code.</summary>
    protected virtual void OnStart() { }
		
    /// <summary>
    /// Provides a hook implement custom stop code.</summary>
    protected virtual void OnStop() { }

  }//class

}//namespace
