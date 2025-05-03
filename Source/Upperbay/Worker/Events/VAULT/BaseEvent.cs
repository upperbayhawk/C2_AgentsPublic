
using System;
using System.Collections;
using System.Diagnostics;


using Upperbay.Agent.Interfaces;

namespace Upperbay.Worker.Events
{

  //============================================================================
  /// <summary>
  /// This is the .NET Agent Framework's base class for a job.</summary>
  /// <remarks>
  /// The AgentJob has a collection of schedulers, a collection of notifiers,
  /// and a worker.</remarks>
  //----------------------------------------------------------------------------
  [Serializable]
  public class BaseEvent
  {
        // Use this TraceSwitch when tracing in derived classes.</summary>
        //private static TraceSwitch deviceSwitch = new TraceSwitch("Device", "Device TraceSwitch");
        //protected static TraceSwitch DeviceSwitch
        //{
        //    get { return deviceSwitch; }
        //}

        //private TraceSwitch _myTraceSwitch = Upperbay.Core.Logging.Log2.AgentSwitch;


        private EventFarm eventFarm;

        // Configured Properties

        private string typename;
        public string Typename  { get { return typename; } set { typename = value; } }

        private string tagname;
        public string Tagname {get { return tagname; } set { tagname = value; }  }

        private string pathname;
        public string PathName { get { return pathname; } set { pathname = value; } }
      
        private string description;
        public string Description  {get { return description; }  set { description = value; }  }

        private string vendor;
        public string Vendor {   get { return vendor; }  set { vendor = value; }    }

        private string egu;
        public string Egu   {  get { return egu; }   set { egu = value; }   }

        private string accessRights;
        public string AccessRights { get { return accessRights; } set { accessRights = value; } }

        private string scanRate;
        public string ScanRate { get { return scanRate; } set { scanRate = value; } }

        private string maxValue;
        public string MaxValue { get { return maxValue; } set { maxValue = value; } }

        private string minValue;
        public string MinValue { get { return minValue; } set { minValue = value; } }

        private string waveForm;
        public string WaveForm { get { return waveForm; } set { waveForm = value; } }

        private string samplingRate;
        public string SamplingRate { get { return samplingRate; } set { samplingRate = value; } }

        private string period;
        public string Period { get { return period; } set { period = value; } }

        // Runtime Properties

        private System.Type datatype = null;
        public System.Type Datatype { get { return datatype; } set { datatype = value; } }

        private double myValue;
        public double Value  {   get { return myValue; }  set { myValue = value; } }

        private double quality;
        public double Quality  { get { return quality; }set { quality = value; }  }

        private DateTime timestamp;
        public DateTime Timestamp  {  get { return timestamp; }   set { timestamp = value; } }



      
        // From Randy's DA Server

        //private string m_itemID = null;
        //private System.Type m_datatype = null;
        //private object m_value = null;
        //private Quality m_quality = Quality.Good;
        //private DateTime m_timestamp = DateTime.MinValue;
        //private accessRights m_accessRights = accessRights.readWritable;
        //private float m_scanRate = 0;
        //private euType m_euType = euType.noEnum;
        //private string[] m_euInfo = null;
        //private double m_maxValue = Double.MaxValue;
        //private double m_minValue = Double.MinValue;
        //private int m_period = 0;
        //private int m_samplingRate = 0;
        //private int m_waveform = 1;
        //private Hashtable m_properties = new Hashtable();




    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    /// <summary>
    /// Default the device's name to the name of the devices's class.</summary>
      public BaseEvent()
      {
          Typename = this.GetType().Name;
	  }

      public void Init(EventFarm eventFarm, string agentName)
      {
          this.eventFarm = eventFarm;
          //this.myHost = myHost;

          Value = 1.0;

          OnInit(eventFarm, agentName);


      }

      //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
      //public BaseEventProxy CreateProxy()
      //{
      //    BaseEventProxy px = new BaseEventProxy();
      //    px.Tagname = this.Tagname;
      //    px.Typename = this.Typename;
      //    px.PathName = this.PathName;
      //    px.Description = this.Description;
      //    px.Vendor = this.Vendor;
      //    px.Egu = this.Egu;
      //    px.AccessRights = this.AccessRights;
      //    px.ScanRate = this.ScanRate;
      //    px.MaxValue = this.MaxValue;
      //    px.MinValue = this.MinValue;
      //    px.WaveForm = this.WaveForm;
      //    px.SamplingRate = this.SamplingRate;
      //    px.Period = this.Period;
      //    px.Datatype = this.Datatype;
      //    px.Value = this.Value;
      //    px.Quality = this.Quality;
      //    px.Timestamp = this.Timestamp;

      //    return px;
      //}

      protected virtual void OnInit(EventFarm eventFarm, string agentName)
      {
      }

      protected virtual void OnRun()
      {
      }

  }//class


  //============================================================================
  /// <summary>
  /// This is the .NET Agent Framework's job collection.</summary>
  /// <remarks>
  /// AgentDeviceCollection derives from CollectionBase.</remarks>
  //----------------------------------------------------------------------------


  [Serializable]
	public class BaseEventCollection : CollectionBase 
	{
    /// <summary>
    /// Add a job to the collection.</summary>
    public void Add(BaseEvent device) 
		{
            List.Add(device);
		}

    /// <summary>
    /// Remove a job from the collection.</summary>
        public void Remove(BaseEvent device)
		{
            List.Remove(device);
		}

    /// <summary>
    /// Set or retrieve a job at the specific index in the collection.</summary>
    public BaseEvent this[int index] 
		{
			get 
			{
                return (BaseEvent)List[index];
			}
			set 
			{
				List[index] = value;
			}
		}
	}//class

  /// <summary>
  /// 
  /// </summary>
  [Serializable]
  public class BaseEventProxy
  {

      // Configured Properties

      private string typename;
      public string Typename { get { return typename; } set { typename = value; } }

      private string tagname;
      public string Tagname { get { return tagname; } set { tagname = value; } }

      private string pathname;
      public string PathName { get { return pathname; } set { pathname = value; } }

      private string description;
      public string Description { get { return description; } set { description = value; } }

      private string vendor;
      public string Vendor { get { return vendor; } set { vendor = value; } }

      private string egu;
      public string Egu { get { return egu; } set { egu = value; } }

      private string accessRights;
      public string AccessRights { get { return accessRights; } set { accessRights = value; } }

      private string scanRate;
      public string ScanRate { get { return scanRate; } set { scanRate = value; } }

      private string maxValue;
      public string MaxValue { get { return maxValue; } set { maxValue = value; } }

      private string minValue;
      public string MinValue { get { return minValue; } set { minValue = value; } }

      private string waveForm;
      public string WaveForm { get { return waveForm; } set { waveForm = value; } }

      private string samplingRate;
      public string SamplingRate { get { return samplingRate; } set { samplingRate = value; } }

      private string period;
      public string Period { get { return period; } set { period = value; } }

      // Runtime Properties

      private System.Type datatype = null;
      public System.Type Datatype { get { return datatype; } set { datatype = value; } }

      private double myValue;
      public double Value { get { return myValue; } set { myValue = value; } }

      private double quality;
      public double Quality { get { return quality; } set { quality = value; } }

      private DateTime timestamp;
      public DateTime Timestamp { get { return timestamp; } set { timestamp = value; } }



  }//class


}//namespace
