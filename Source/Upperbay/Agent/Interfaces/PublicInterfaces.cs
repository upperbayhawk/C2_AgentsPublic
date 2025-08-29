//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
using System;
using System.ServiceModel.Channels;

namespace Upperbay.Agent.Interfaces
{

    #region Colony Interfaces
    /// <summary>
    /// Simple Interface for a class that supports a Windows Service
    /// </summary>
    /// 
    public interface ICell
    {
        // Interface Methods
        void OnCellStart();
        void OnCellStop();
        bool OnCellInitialize(IHostColonyServices hostServices);
    }


    /// <summary>
    /// The executable Agent Interface for command and control
    /// </summary>
    public interface INativeAgent
    {
        bool OnInitialize(IHostColonyServices myHost);
        void OnStart(System.Threading.CancellationToken token);
        bool OnFire();
        bool OnStop();
    }

    public interface IAgentObjectAssistant
    {
        bool Initialize(string myClassName, string myObjectName, object me);
        bool Start();
        bool Fire();
        bool Stop();
    }
    #endregion


    #region Colony Services
    public interface IHostColonyServices
    {
        // Environment Methods
        string CelestialName { get; set; }
        string CollectiveName { get; set; }
        string CommunityName { get; set; }
        string ClusterName { get; set; }
        string CarrierName { get; set; }
        string ColonyName { get; set; }
        string ServiceName { get; set; }
        string ColonyServiceName { get; set; }
        string ClassName { get; set; }
        string Version { get; set; }
        string Description { get; set; }
        string LogicalName { get; set; }
        string AssemblyPath { get; set; }
        string AssemblyName { get; set; }
        string AssemblyDirectory { get; set; }

        // Filled in by Cell
        string AgentName { get; set; }
        string AgentNickName { get; set; }
        string AgentRole { get; set; }

    }



    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public struct HostColonyContext
    {
        public string CelestialName;
        public string CollectiveName;
        public string CommunityName;
        public string ClusterName;
        public string CarrierName;
        public string ColonyName;
        public string ServiceName;
        public string ColonyServiceName;
        public string ClassName;
        public string Version;
        public string Description;
        public string LogicalName;
        public string AssemblyPath;
        public string AssemblyName;
        public string AssemblyDirectory;
        public string AgentName;
        public string AgentNickName;
        public string AgentRole;
    };

    #endregion


    #region Messaging Interfaces
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class DataVariable
    {
        private string tagname;
        public string TagName { get { return this.tagname; } set { this.tagname = value; } }

        private string externalname;
        public string ExternalName { get { return this.externalname; } set { this.externalname = value; } }

        private string description;
        public string Description { get { return this.description; } set { this.description = value; } }

        private string units;
        public string Units { get { return this.units; } set { this.units = value; } }

        private string status;
        public string Status { get { return this.status; } set { this.status = value; } }

        private string val;
        public string Value { get { return this.val; } set { this.val = value; } }

        private string quality;
        public string Quality { get { return this.quality; } set { this.quality = value; } }

        private DateTime updatetime;
        public DateTime UpdateTime { get { return this.updatetime; } set { this.updatetime = value; } }

        private DateTime servertime;
        public DateTime ServerTime { get { return this.servertime; } set { this.servertime = value; } }

        private string lastvalue;
        public string LastValue { get { return this.lastvalue; } set { this.lastvalue = value; } }

        private DateTime lastvaluetime;
        public DateTime LastValueTime { get { return this.lastvaluetime; } set { this.lastvaluetime = value; } }

        private bool readwrite;
        public bool ReadWrite { get { return this.readwrite; } set { this.readwrite = value; } }

        private bool changeFlag;
        public bool ChangeFlag { get { return this.changeFlag; } set { this.changeFlag = value; } }

        private string path;
        public string Path { get { return this.path; } set { this.path = value; } }
        public DataVariable()
        {
            this.TagName = "NONAME";
            this.ExternalName = "NONAME";
            this.Description = "NODESC";
            this.Units = "NOUNITS";
            this.Status = "OFFLINE";
            this.Value = "NOVALUE";
            this.Quality = "BAD";
            this.UpdateTime = DateTime.Now;
            this.ServerTime = DateTime.Now;
            this.ChangeFlag = false;
            this.LastValue = "NOVALUE";
            this.LastValueTime = DateTime.Now;
            this.ReadWrite = true;
            this.Path = "NOPATH";
        }
        public DataVariable(string tagname, string xname, string description, string units)
        {
            this.TagName = tagname;
            this.ExternalName = xname;
            this.Description = description;
            this.Units = units;
            this.Status = "OFFLINE";
            this.Value = "NOVALUE";
            this.Quality = "BAD";
            this.UpdateTime = DateTime.Now;
            this.ServerTime = DateTime.Now;
            this.ChangeFlag = false;
            this.LastValue = "NOVALUE";
            this.LastValueTime = DateTime.Now;
            this.ReadWrite = true;
            this.Path = "NOPATH";
        }
        public DataVariable(string tagname, string xname, string description, string units, string path)
        {
            this.TagName = tagname;
            this.ExternalName = xname;
            this.Description = description;
            this.Units = units;
            this.Status = "OFFLINE";
            this.Value = "NOVALUE";
            this.Quality = "BAD";
            this.UpdateTime = DateTime.Now;
            this.ServerTime = DateTime.Now;
            this.ChangeFlag = false;
            this.LastValue = "NOVALUE";
            this.LastValueTime = DateTime.Now;
            this.ReadWrite = true;
            this.Path = path;
        }
    }

    [Serializable]
    public class EventVariable
    {
        private string eventname;
        public string EventName { get { return this.eventname; } set { this.eventname = value; } }

        private string eventtype;
        public string EventType { get { return this.eventtype; } set { this.eventtype = value; } }

        private string description;
        public string Description { get { return this.description; } set { this.description = value; } }

        private string program;
        public string Program { get { return this.program; } set { this.program = value; } }

        private DateTime starttime;
        public DateTime StartTime { get { return this.starttime; } set { this.starttime = value; } }

        private DateTime endtime;
        public DateTime EndTime { get { return this.endtime; } set { this.endtime = value; } }

        private string status;
        public string Status { get { return this.status; } set { this.status = value; } }

        private string value;
        public string Value { get { return this.value; } set { this.value = value; } }

        private string quality;
        public string Quality { get { return this.quality; } set { this.quality = value; } }


        public EventVariable()
        {
            //this.TagName = "NONAME";
            //this.ExternalName = "NONAME";
            //this.Description = "NODESC";
            //this.Units = "NOUNITS";
            //this.Status = "OFFLINE";
            //this.Value = "NOVALUE";
            //this.Quality = "BAD";
            //this.UpdateTime = DateTime.Now;
            //this.ServerTime = DateTime.Now;
            //this.ChangeFlag = false;
            //this.LastValue = "NOVALUE";
            //this.LastValueTime = DateTime.Now;
            //this.ReadWrite = true;
        }
        //public EventVariable(string tagname, string xname, string description, string units)
        //{
        //    this.TagName = tagname;
        //    this.ExternalName = xname;
        //    this.Description = description;
        //    this.Units = units;
        //    this.Status = "OFFLINE";
        //    this.Value = "NOVALUE";
        //    this.Quality = "BAD";
        //    this.UpdateTime = DateTime.Now;
        //    this.ServerTime = DateTime.Now;
        //    this.ChangeFlag = false;
        //    this.LastValue = "NOVALUE";
        //    this.LastValueTime = DateTime.Now;
        //    this.ReadWrite = true;
        //}
    }

    /// <summary>
    /// 
    /// </summary>
    public class TOPICS
    {
        public const string GAME_START_TOPIC = "CurrentForCarbon/Game/Start/ALL";
        public const string GAME_RESULTS_TOPIC = "CurrentForCarbon/Game/Results";
        public const string GAME_WINNER_TOPIC = "CurrentForCarbon/Game/Winner";
        public const string DATAVARIABLE_TOPIC = "DataVariable/All";
        public const string COMMAND_TOPIC = "CommandVariable/All";
        public const string EVENT_TOPIC = "EventVariable/ALL";
        public const string GAME_COMMAND_TOPIC = "CurrentForCarbon/Game/Command/ALL";
        public const string GAME_PLAYER_CONFIDENTIAL_TOPIC = "CurrentForCarbon/Game/Player/ALL";
        public const string C2AGENT_INCOMING_TOPIC = "C2Agent";
    }


    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class GameEventVariable
    {
        private string gamename = "NA";
        public string GameName { get { return this.gamename; } set { this.gamename = value; } }

        private string gameid = "NA";
        public string GameId { get { return this.gameid; } set { this.gameid = value; } }

        private string gridzone = "NA";
        public string GridZone { get { return this.gridzone; } set { this.gridzone = value; } }

        private string gameplayerid = "NA";
        public string GamePlayerId { get { return this.gameplayerid; } set { this.gameplayerid = value; } }

        private string gametype = "SHEDPOWER";
        public string GameType { get { return this.gametype; } set { this.gametype = value; } }

        private string gameawardrank = "NA";
        public string GameAwardRank { get { return this.gameawardrank; } set { this.gameawardrank = value; } }

        private DateTime starttime;
        public DateTime StartTime { get { return this.starttime; } set { this.starttime = value; } }

        private DateTime endtime;
        public DateTime EndTime { get { return this.endtime; } set { this.endtime = value; } }

        private int durationinminutes = 30;
        public int DurationInMinutes { get { return this.durationinminutes; } set { this.durationinminutes = value; } }

        private double dollarperpoint = 0.001;
        public double DollarPerPoint { get { return this.dollarperpoint; } set { this.dollarperpoint = value; } }

        private double pointsperwatt = 1.0;
        public double PointsPerWatt { get { return this.pointsperwatt; } set { this.pointsperwatt = value; } }

        private double pointsperpercent = .064;
        public double PointsPerPercent { get { return this.pointsperpercent; } set { this.pointsperpercent = value; } }

        private double winningpoints = 0;
        public double WinningPoints { get { return this.winningpoints; } set { this.winningpoints = value; } }

        private double bonuspoints = 0;
        public double BonusPoints { get { return this.bonuspoints; } set { this.bonuspoints = value; } }

        private int bonuspool = 0;
        public int BonusPool { get { return this.bonuspool; } set { this.bonuspool = value; } }

        private string prestartalert = "true";
        public string PreStartAlert { get { return this.prestartalert; } set { this.prestartalert = value; } }

        // Placeholders
        private string status = "OK";
        public string Status { get { return this.status; } set { this.status = value; } }

        private string signals = "NA";
        public string Signals { get { return this.signals; } set { this.signals = value; } }

        private string targets = "NA";
        public string Targets { get { return this.targets; } set { this.targets = value; } }

        private string description = "NA";
        public string Description { get { return this.description; } set { this.description = value; } }

        private string program = "NA";
        public string Program { get { return this.program; } set { this.program = value; } }

        public GameEventVariable()
        {
            this.StartTime = DateTime.Now;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class GameResultsVariable
    {
        private string gamename = "NA";
        public string GameName { get { return this.gamename; } set { this.gamename = value; } }

        private string gameid = "NA";
        public string GameId { get { return this.gameid; } set { this.gameid = value; } }

        private string gameplayerid = "NA";
        public string GamePlayerId { get { return this.gameplayerid; } set { this.gameplayerid = value; } }

        private string gameplayersignature = "NA";
        public string GamePlayerSignature { get { return this.gameplayersignature; } set { this.gameplayersignature = value; } }

        private string gametype = "SHEDPOWER";
        public string GameType { get { return this.gametype; } set { this.gametype = value; } }

        private DateTime starttime;
        public DateTime StartTime { get { return this.starttime; } set { this.starttime = value; } }

        private DateTime endtime;
        public DateTime EndTime { get { return this.endtime; } set { this.endtime = value; } }

        private int durationinminutes = 0;
        public int DurationInMinutes { get { return this.durationinminutes; } set { this.durationinminutes = value; } }

        private double gameavgpowerinwatts = 0.0;
        public double GameAvgPowerInWatts { get { return this.gameavgpowerinwatts; } set { this.gameavgpowerinwatts = value; } }

        private double gameenergyinkwh = 0.0;
        public double GameEnergyInKWH { get { return this.gameenergyinkwh; } set { this.gameenergyinkwh = value; } }

        private double baselineavgpowerinwatts = 1.0;
        public double BaselineAvgPowerInWatts { get { return this.baselineavgpowerinwatts; } set { this.baselineavgpowerinwatts = value; } }

        private double baselineenergyinkwh = 0.0;
        public double BaselineEnergyInKWH { get { return this.baselineenergyinkwh; } set { this.baselineenergyinkwh = value; } }

        private double deltapowerinwatts = 0;
        public double DeltaPowerInWatts { get { return this.deltapowerinwatts; } set { this.deltapowerinwatts = value; } }

        private double deltapowerinpercent = 0;
        public double DeltaPowerInPercent { get { return this.deltapowerinpercent; } set { this.deltapowerinpercent = value; } }

        private double dollarperpoint = 1.0;
        public double DollarPerPoint { get { return this.dollarperpoint; } set { this.dollarperpoint = value; } }

        private double pointsperwatt = 1.0;
        public double PointsPerWatt { get { return this.pointsperwatt; } set { this.pointsperwatt = value; } }

        private double pointsperpercent = 1.0;
        public double PointsPerPercent { get { return this.pointsperpercent; } set { this.pointsperpercent = value; } }

        private double pointsawarded = 1.0;
        public double PointsAwarded { get { return this.pointsawarded; } set { this.pointsawarded = value; } }

        private double percentpoints = 1.0;
        public double PercentPoints { get { return this.percentpoints; } set { this.percentpoints = value; } }

        private double wattpoints = 1.0;
        public double WattPoints { get { return this.wattpoints; } set { this.wattpoints = value; } }

        private double awardvalue = 1.0;
        public double AwardValue { get { return this.awardvalue; } set { this.awardvalue = value; } }

        public GameResultsVariable()
        {
        }
    }


    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class GameWinnerVariable
    {
        private string gamename = "NA";
        public string GameName { get { return this.gamename; } set { this.gamename = value; } }

        private string gameid = "NA";
        public string GameId { get { return this.gameid; } set { this.gameid = value; } }

        private string winnerid = "NA";
        public string WinnerId { get { return this.winnerid; } set { this.winnerid = value; } }

        private string gametype = "SHEDPOWER";
        public string GameType { get { return this.gametype; } set { this.gametype = value; } }

        private DateTime starttime;
        public DateTime StartTime { get { return this.starttime; } set { this.starttime = value; } }

        private DateTime endtime;
        public DateTime EndTime { get { return this.endtime; } set { this.endtime = value; } }

        private int durationinminutes = 0;
        public int DurationInMinutes { get { return this.durationinminutes; } set { this.durationinminutes = value; } }

        private double gameavgpowerinwatts = 0.0;
        public double GameAvgPowerInWatts { get { return this.gameavgpowerinwatts; } set { this.gameavgpowerinwatts = value; } }

        private double gameenergyinkwh = 0.0;
        public double GameEnergyInKWH { get { return this.gameenergyinkwh; } set { this.gameenergyinkwh = value; } }

        private double baselineavgpowerinwatts = 1.0;
        public double BaselineAvgPowerInWatts { get { return this.baselineavgpowerinwatts; } set { this.baselineavgpowerinwatts = value; } }

        private double baselineenergyinkwh = 0.0;
        public double BaselineEnergyInKWH { get { return this.baselineenergyinkwh; } set { this.baselineenergyinkwh = value; } }

        private double deltapowerinwatts = 0;
        public double DeltaPowerInWatts { get { return this.deltapowerinwatts; } set { this.deltapowerinwatts = value; } }

        private double deltapowerinpercent = 0;
        public double DeltaPowerInPercent { get { return this.deltapowerinpercent; } set { this.deltapowerinpercent = value; } }

        private double dollarperpoint = .001;
        public double DollarPerPoint { get { return this.dollarperpoint; } set { this.dollarperpoint = value; } }

        private double pointsperwatt = 1.0;
        public double PointsPerWatt { get { return this.pointsperwatt; } set { this.pointsperwatt = value; } }

        private double pointsperpercent = .064;
        public double PointsPerPercent { get { return this.pointsperpercent; } set { this.pointsperpercent = value; } }

        private double pointsawarded = 0.0;
        public double PointsAwarded { get { return this.pointsawarded; } set { this.pointsawarded = value; } }

        private double percentpoints = 0.0;
        public double PercentPoints { get { return this.percentpoints; } set { this.percentpoints = value; } }

        private double wattpoints = 0.0;
        public double WattPoints { get { return this.wattpoints; } set { this.wattpoints = value; } }

        private double awardvalue = 0.0;
        public double AwardValue { get { return this.awardvalue; } set { this.awardvalue = value; } }

        //The Good Stuff
        private double winningpoints = 0.0;
        public double WinningPoints { get { return this.winningpoints; } set { this.winningpoints = value; } }

        private double bonuspoints = 0.0;
        public double BonusPoints { get { return this.bonuspoints; } set { this.bonuspoints = value; } }

        private int bonuspool = 0;
        public int BonusPool { get { return this.bonuspool; } set { this.bonuspool = value; } }
    }

    /// <summary>
    /// For remotely starting a game
    /// </summary>
    [Serializable]
    public class GameCommandVariable
    {
        private string gamename = "NA";
        public string GameName { get { return this.gamename; } set { this.gamename = value; } }

        private string gameid = "NA";
        public string GameId { get { return this.gameid; } set { this.gameid = value; } }

        private string gridzone = "NA";
        public string GridZone { get { return this.gridzone; } set { this.gridzone = value; } }


        private string gameplayerid = "NA";
        public string GamePlayerId { get { return this.gameplayerid; } set { this.gameplayerid = value; } }

        private string gametype = "SHEDPOWER";
        public string GameType { get { return this.gametype; } set { this.gametype = value; } }

        private DateTime starttime;
        public DateTime StartTime { get { return this.starttime; } set { this.starttime = value; } }

        private int durationinminutes = 30;
        public int DurationInMinutes { get { return this.durationinminutes; } set { this.durationinminutes = value; } }

        private double dollarperpoint = 0.001;
        public double DollarPerPoint { get { return this.dollarperpoint; } set { this.dollarperpoint = value; } }

        private double pointsperwatt = 1.0;
        public double PointsPerWatt { get { return this.pointsperwatt; } set { this.pointsperwatt = value; } }

        private double pointsperpercent = .064;
        public double PointsPerPercent { get { return this.pointsperpercent; } set { this.pointsperpercent = value; } }

        private double winningpoints = 0;
        public double WinningPoints { get { return this.winningpoints; } set { this.winningpoints = value; } }

        private double bonuspoints = 0;
        public double BonusPoints { get { return this.bonuspoints; } set { this.bonuspoints = value; } }

        private int bonuspool = 0;
        public int BonusPool { get { return this.bonuspool; } set { this.bonuspool = value; } }

        // Placeholders
        private string status = "OK";
        public string Status { get { return this.status; } set { this.status = value; } }

        private string signals = "NA";
        public string Signals { get { return this.signals; } set { this.signals = value; } }

        private string targets = "NA";
        public string Targets { get { return this.targets; } set { this.targets = value; } }

        private string description = "NA";
        public string Description { get { return this.description; } set { this.description = value; } }

        private string program = "NA";
        public string Program { get { return this.program; } set { this.program = value; } }

        public GameCommandVariable()
        {
            this.StartTime = DateTime.Now;
        }

    }

    /// <summary>
    /// NOT IMPLEMENTED - Place Holder
    /// </summary>
    [Serializable]
    public class CommandVariable
    {
        private string gamename = "NA";
        public string GameName { get { return this.gamename; } set { this.gamename = value; } }

        private string gameid = "NA";
        public string GameId { get { return this.gameid; } set { this.gameid = value; } }

        private string gridzone = "NA";
        public string GridZone { get { return this.gridzone; } set { this.gridzone = value; } }


        private string gameplayerid = "NA";
        public string GamePlayerId { get { return this.gameplayerid; } set { this.gameplayerid = value; } }

        private string gametype = "SHEDPOWER";
        public string GameType { get { return this.gametype; } set { this.gametype = value; } }

        private DateTime starttime;
        public DateTime StartTime { get { return this.starttime; } set { this.starttime = value; } }

        private int durationinminutes = 30;
        public int DurationInMinutes { get { return this.durationinminutes; } set { this.durationinminutes = value; } }

        private double dollarperpoint = 0.001;
        public double DollarPerPoint { get { return this.dollarperpoint; } set { this.dollarperpoint = value; } }

        private double pointsperwatt = 1.0;
        public double PointsPerWatt { get { return this.pointsperwatt; } set { this.pointsperwatt = value; } }

        private double pointsperpercent = .064;
        public double PointsPerPercent { get { return this.pointsperpercent; } set { this.pointsperpercent = value; } }

        private double winningpoints = 0;
        public double WinningPoints { get { return this.winningpoints; } set { this.winningpoints = value; } }

        private double bonuspoints = 0;
        public double BonusPoints { get { return this.bonuspoints; } set { this.bonuspoints = value; } }

        private int bonuspool = 0;
        public int BonusPool { get { return this.bonuspool; } set { this.bonuspool = value; } }

        // Placeholders
        private string status = "OK";
        public string Status { get { return this.status; } set { this.status = value; } }

        private string signals = "NA";
        public string Signals { get { return this.signals; } set { this.signals = value; } }

        private string targets = "NA";
        public string Targets { get { return this.targets; } set { this.targets = value; } }

        private string description = "NA";
        public string Description { get { return this.description; } set { this.description = value; } }

        private string program = "NA";
        public string Program { get { return this.program; } set { this.program = value; } }

        public CommandVariable()
        {
            this.StartTime = DateTime.Now;
        }


    }
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class GamePlayerConfidential
    {

        private string gameplayerid = "NA";
        public string GamePlayerId { get { return this.gameplayerid; } set { this.gameplayerid = value; } }

        private string gameplayernumber = "NA";
        public string GamePlayerNumber { get { return this.gameplayernumber; } set { this.gameplayernumber = value; } }

        private string gameplayername = "NA";
        public string GamePlayerName { get { return this.gameplayername; } set { this.gameplayername = value; } }

        private string gameplayerstreet = "NA";
        public string GamePlayerStreet { get { return this.gameplayerstreet; } set { this.gameplayerstreet = value; } }

        private string gameplayercity = "NA";
        public string GamePlayerCity { get { return this.gameplayercity; } set { this.gameplayercity = value; } }

        private string gameplayerstate = "NA";
        public string GamePlayerState { get { return this.gameplayerstate; } set { this.gameplayerstate = value; } }

        private string gameplayerzipcode = "NA";
        public string GamePlayerZipcode { get { return this.gameplayerzipcode; } set { this.gameplayerzipcode = value; } }

        private string gameplayeremail = "NA";
        public string GamePlayerEmail { get { return this.gameplayeremail; } set { this.gameplayeremail = value; } }

        private string gameplayerphone = "NA";
        public string GamePlayerPhone { get { return this.gameplayerphone; } set { this.gameplayerphone = value; } }

        private string gameplayertimezone = "NA";
        public string GamePlayerTimeZone { get { return this.gameplayertimezone; } set { this.gameplayertimezone = value; } }

        private string gameplayerelectricco = "NA";
        public string GamePlayerElectricCo { get { return this.gameplayerelectricco; } set { this.gameplayerelectricco = value; } }

        private string gameplayerclustername = "NA";
        public string GamePlayerClusterName { get { return this.gameplayerclustername; } set { this.gameplayerclustername = value; } }

        private string gameplayerclusterversion = "NA";
        public string GamePlayerClusterVersion { get { return this.gameplayerclusterversion; } set { this.gameplayerclusterversion = value; } }

        private string ethereumexternaladdress = "NA";
        public string EthereumExternalAddress { get { return this.ethereumexternaladdress; } set { this.ethereumexternaladdress = value; } }

        private string ethereumxternalprivatekey = "NA";
        public string EthereumExternalPrivateKey { get { return this.ethereumxternalprivatekey; } set { this.ethereumxternalprivatekey = value; } }

        private string ethereumcontractaddress = "NA";
        public string EthereumContractAddress { get { return this.ethereumcontractaddress; } set { this.ethereumcontractaddress = value; } }

        private string dataconnectstring = "NA";
        public string DataConnectString { get { return this.dataconnectstring; } set { this.dataconnectstring = value; } }

        private string mqttusername = "NA";
        public string MqttUserName { get { return this.mqttusername; } set { this.mqttusername = value; } }

        private string mqttpassword = "NA";
        public string MqttPassword { get { return this.mqttpassword; } set { this.mqttpassword = value; } }

        public GamePlayerConfidential()
        {
        }
    }


    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class GridPeakDetectedObject
    {
        public string type_name;

        public string agent_name;

        public string message;

        public string start_date_time;

        public string duration_mins;

        public string peak_lmp;

        public string grid_node;

        public string award_level;

        public string game_type;

        public GridPeakDetectedObject()
        {
        }


    }
    #endregion
}

