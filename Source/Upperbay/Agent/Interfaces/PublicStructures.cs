using System;
using System.Diagnostics;

namespace Upperbay.Agent.Interfaces
{

 


    /// <summary>
    /// 
    /// </summary>
    //[Serializable]
    //public class TriggerVariable
    //{
    //    private string name;
    //    public string Name { get { return this.name; } set { this.name = value; } }

    //    private string status;
    //    public string Status { get { return this.status; } set { this.status = value; } }

    //    private string val;
    //    public string Value { get { return this.val; } set { this.val = value; } }

    //    private string quality;
    //    public string Quality { get { return this.quality; } set { this.quality = value; } }

    //    private DateTime time;
    //    public DateTime Time { get { return this.time; } set { this.time = value; } }

    //    private string units;
    //    public string Units { get { return this.units; } set { this.units = value; } }

    //    public TriggerVariable() { }
    //    public TriggerVariable(string name, string val, string qual, DateTime dt, string units)
    //    {
    //        this.name = name;
    //        this.val = val;
    //        this.quality = qual;
    //        this.time = dt;
    //        this.units = units;
    //    }
    //}


    //[Serializable]
    //public class EventVariable
    //{

    //    private string name;
    //    public string Name { get { return this.name; } set { this.name = value; } }

    //    private string status;
    //    public string Status { get { return this.status; } set { this.status = value; } }

    //    private bool val;
    //    public bool Value { get { return this.val; } set { this.val = value; } }

    //    private string quality;
    //    public string Quality { get { return this.quality; } set { this.quality = value; } }

    //    private DateTime time;
    //    public DateTime Time { get { return this.time; } set { this.time = value; } }

    //    private string units;
    //    public string Units { get { return this.units; } set { this.units = value; } }

    //    public EventVariable() { }
    //    public EventVariable(string name, bool val, string qual, DateTime dt, string units)
    //    {
    //        this.name = name;
    //        this.val = val;
    //        this.quality = qual;
    //        this.time = dt;
    //        this.units = units;
    //    }

    }

    //[Serializable]
    //public class ActionVariable
    //{
    //    private string name;
    //    public string Name { get { return this.name; } set { this.name = value; } }

    //    private string status;
    //    public string Status { get { return this.status; } set { this.status = value; } }

    //    private string val;
    //    public string Value { get { return this.val; } set { this.val = value; } }

    //    private string quality;
    //    public string Quality { get { return this.quality; } set { this.quality = value; } }

    //    private DateTime time;
    //    public DateTime Time { get { return this.time; } set { this.time = value; } }

    //    private string units;
    //    public string Units { get { return this.units; } set { this.units = value; } }

    //    public ActionVariable() { }
    //    public ActionVariable(string name, string val, string qual, DateTime dt, string units)
    //    {
    //        this.name = name;
    //        this.val = val;
    //        this.quality = qual;
    //        this.time = dt;
    //        this.units = units;
    //    }

   // }



    //public class AgentDataValue
    //{
    //    public AgentDataValue();
    //    public AgentDataValue(AgentDataValue value);
    //    public AgentDataValue(StatusCode statusCode);
    //    public AgentDataValue(Variant value);
    //    public AgentDataValue(StatusCode statusCode, DateTime serverTimestamp);
    //    public AgentDataValue(Variant value, StatusCode statusCode);
    //    public AgentDataValue(Variant value, StatusCode statusCode, DateTime sourceTimestamp);
    //    public AgentDataValue(Variant value, StatusCode statusCode, DateTime sourceTimestamp, DateTime serverTimestamp);

    //    public ushort ServerPicoseconds { get; set; }
    //    public DateTime ServerTimestamp { get; set; }
    //    public ushort SourcePicoseconds { get; set; }
    //    public DateTime SourceTimestamp { get; set; }
    //    public StatusCode StatusCode { get; set; }
    //    public object Value { get; set; }
    //    public Variant WrappedValue { get; set; }

    //    public object Clone();
    //    public override bool Equals(object obj);
    //    public override int GetHashCode();
    //    public object GetValue(Type expectedType);
    //    public static bool IsBad(AgentDataValue value);
    //    public static bool IsGood(AgentDataValue value);
    //    public static bool IsNotBad(AgentDataValue value);
    //    public static bool IsNotGood(AgentDataValue value);
    //    public static bool IsNotUncertain(AgentDataValue value);
    //    public static bool IsUncertain(AgentDataValue value);
    //    public override string ToString();
    //    public string ToString(string format, IFormatProvider provider);
    //}
//}