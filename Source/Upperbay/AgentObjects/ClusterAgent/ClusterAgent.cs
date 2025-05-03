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
using System.Configuration;

// Assemblies needed for Agentness
using Upperbay.Core.Library;
using Upperbay.Core.Logging;
using Upperbay.Agent.Interfaces;
using Upperbay.Worker.Voice;
using Upperbay.Worker.Timers;
using Upperbay.Worker.PostOffice;
using Upperbay.Worker.JSON;
using Upperbay.Agent.ColonyMatrix;
using Upperbay.Assistant;
using System.Globalization;



namespace Upperbay.AgentObject
{
    /// <summary>
    /// Summary description for DataAgent.
    /// </summary>
    ///     

    //    [InstrumentationClass(InstrumentationType.Instance)]
    //    [Serializable]
    public class ClusterAgent : INativeAgent
    {
        public ClusterAgent()
        {
        }

        #region Agent Properties

        private DataVariable _agentValue = new DataVariable("AgentValue", "AgentValue","A Test Variable", "Feet");
        [Ua("publish"), Ua("simulated")]
        public DataVariable AgentValue { get { return this._agentValue; } set { this._agentValue = value; } }

        private DataVariable _agentValue1 = new DataVariable("AgentValue1", "AgentValue1", "A Test Variable", "Feet");
        [Ua("publish"), Ua("simulated")]
        public DataVariable AgentValue1 { get { return this._agentValue1; } set { this._agentValue1 = value; } }

        private DataVariable _agentValue2 = new DataVariable("AgentValue2", "AgentValue2", "A Test Variable", "Feet");
        [Ua("publish"), Ua("simulated")]
        public DataVariable AgentValue2 { get { return this._agentValue2; } set { this._agentValue2 = value; } }

        private DataVariable _agentValue3 = new DataVariable("AgentValue3", "AgentValue3", "A Test Variable", "Feet");
        [Ua("publish"), Ua("simulated")]
        public DataVariable AgentValue3 { get { return this._agentValue3; } set { this._agentValue3 = value; } }

        private DataVariable _agentValue4 = new DataVariable("AgentValue4", "AgentValue4", "A Test Variable", "Feet");
        [Ua("publish"), Ua("simulated")]
        public DataVariable AgentValue4 { get { return this._agentValue4; } set { this._agentValue4 = value; } }

        private DataVariable _agentValue5 = new DataVariable("AgentValue5", "AgentValue5", "A Test Variable", "Feet");
        [Ua("publish"), Ua("simulated")]
        public DataVariable AgentValue5 { get { return this._agentValue5; } set { this._agentValue5 = value; } }

        private DataVariable _agentValue6 = new DataVariable("AgentValue6", "AgentValue6", "A Test Variable", "Feet");
        [Ua("publish"), Ua("simulated")]
        public DataVariable AgentValue6 { get { return this._agentValue6; } set { this._agentValue6 = value; } }

        private DataVariable _agentValue7 = new DataVariable("AgentValue7", "AgentValue7", "A Test Variable", "Feet");
        [Ua("publish"), Ua("simulated")]
        public DataVariable AgentValue7 { get { return this._agentValue7; } set { this._agentValue7 = value; } }

        private DataVariable _agentValue8 = new DataVariable("AgentValue8", "AgentValue8", "A Test Variable", "Feet");
        [Ua("publish"), Ua("simulated")]
        public DataVariable AgentValue8 { get { return this._agentValue8; } set { this._agentValue8 = value; } }

        private DataVariable _agentValue9 = new DataVariable("AgentValue9", "AgentValue9", "A Test Variable", "Feet");
        [Ua("publish"), Ua("simulated")]
        public DataVariable AgentValue9 { get { return this._agentValue9; } set { this._agentValue9 = value; } }

        private DataVariable _agentValue10 = new DataVariable();
        [Ua("publish"), Ua("simulated")]
        public DataVariable AgentValue10 { get { return this._agentValue10; } set { this._agentValue10 = value; } }

        private DataVariable _agentValue11 = new DataVariable("AgentValue11", "AgentValue11", "A Test Variable", "Feet");
        [Ua("publish"), Ua("simulated")]
        public DataVariable AgentValue11 { get { return this._agentValue11; } set { this._agentValue11 = value; } }

        private DataVariable _agentValue12 = new DataVariable("AgentValue12", "AgentValue12", "A Test Variable", "Feet");
        [Ua("publish"), Ua("simulated")]
        public DataVariable AgentValue12 { get { return this._agentValue12; } set { this._agentValue12 = value; } }

        //All input properties are string
        private DataVariable _manualInputData = new DataVariable("ManualInputData", "ManualInputData", "A Test Input Variable", "Feet");
        [Ua("publish"), Ua("manualinput")]
        public DataVariable ManualInputData { get { return this._manualInputData; } set { this._manualInputData = value; } }

        private DataVariable _agentValue14 = new DataVariable("AgentValue14", "AgentValue14", "A Test Variable", "Feet");
        [Ua("subscribe")]
        public DataVariable AgentValue14 { get { return this._agentValue14; } set { this._agentValue14 = value; } }

        private DataVariable _agentValue15 = new DataVariable("AgentValue15", "AgentValue15", "A Test Variable", "Feet");
        [Ua("subscribe")]
        public DataVariable AgentValue15 { get { return this._agentValue15; } set { this._agentValue15 = value; } }

        private DataVariable _agentValue16 = new DataVariable("AgentValue16", "AgentValue16", "A Test Variable", "Feet");
        [Ua("subscribe")]
        public DataVariable AgentValue16 { get { return this._agentValue16; } set { this._agentValue16 = value; } }

        private DataVariable _agentValue17 = new DataVariable("AgentValue17", "AgentValue17", "A Test Variable", "Feet");
        [Ua("subscribe")]
        public DataVariable AgentValue17 { get { return this._agentValue17; } set { this._agentValue17 = value; } }

        private DataVariable _agentValue18 = new DataVariable("AgentValue18", "AgentValue18", "A Test Variable", "Feet");
        [Ua("subscribe")]
        public DataVariable AgentValue18 { get { return this._agentValue18; } set { this._agentValue18 = value; } }

        private DataVariable _timeString = new DataVariable("TimeString", "TimeString", "A Test Variable", "Seconds");
        [Ua("subscribe")]
        public DataVariable TimeString { get { return this._timeString; } set { this._timeString = value; } }

        //FOR TEST
        private DataVariable _agentValue20 = new DataVariable("AgentValue16", "AgentValue16", "A Test Variable", "Feet");
        [Ua("simulated"), Ua("publish")]
        public DataVariable AgentValue20 { get { return this._agentValue20; } set { this._agentValue20 = value; } }
    
        private DataVariable _currentForCarbonWatts = new DataVariable("CurrentForCarbonWatts", "CurrentForCarbonWatts", "A Test Variable", "Watts");
        //[Ua("thingspeakread"), Ua("publish")]
        public DataVariable CurrentForCarbonWatts { get { return this._currentForCarbonWatts; } set { this._currentForCarbonWatts = value; } }

        private DataVariable _wattsMirrorAtCampDavid = new DataVariable("WattsMirrorAtCampDavid", "WattsMirrorAtCampDavid", "A Test Variable", "Watts");
        //[Ua("thingspeakwrite"), Ua("publish")]
        public DataVariable WattsMirrorAtCampDavid { get { return this._wattsMirrorAtCampDavid; } set { this._wattsMirrorAtCampDavid = value; } }

        private DataVariable _voltsAtCampDavid = new DataVariable("VoltsAtCampDavid", "VoltsAtCampDavid", "A Test Variable", "Volts");
       // [Ua("thingspeakread"), Ua("publish")]
        public DataVariable VoltsAtCampDavid { get { return this._voltsAtCampDavid; } set { this._voltsAtCampDavid = value; } }

        private DataVariable _lowVoltsAtCampDavid = new DataVariable("LowVoltsAtCampDavid", "LowVoltsAtCampDavid", "A Test Variable", "Volts");
       // [Ua("thingspeakwrite"), Ua("publish")]
        public DataVariable LowVoltsAtCampDavid { get { return this._lowVoltsAtCampDavid; } set { this._lowVoltsAtCampDavid = value; } }


        private DataVariable _chargingNikkiAtCampDavid = new DataVariable("ChargingNikkiAtCampDavid", "ChargingNikkiAtCampDavid", "A Test Variable", "Volts");
       // [Ua("thingspeakwrite"), Ua("publish")]
        public DataVariable ChargingNikkiAtCampDavid { get { return this._chargingNikkiAtCampDavid; } set { this._chargingNikkiAtCampDavid = value; } }

        private DataVariable _soilMoistureAtCampDavid = new DataVariable("SoilMoistureAtCampDavid", "SoilMoistureAtCampDavid", "A Test Variable", "Humidity");
       // [Ua("thingspeakread"), Ua("publish")]
        public DataVariable SoilMoistureAtCampDavid { get { return this._soilMoistureAtCampDavid; } set { this._soilMoistureAtCampDavid = value; } }

        private DataVariable _lowSoilMoistureAtCampDavid = new DataVariable("LowSoilMoistureAtCampDavid", "LowSoilMoistureAtCampDavid", "A Test Variable", "Humidity");
        //[Ua("thingspeakwrite"), Ua("publish")]
        public DataVariable LowSoilMoistureAtCampDavid { get { return this._lowSoilMoistureAtCampDavid; } set { this._lowSoilMoistureAtCampDavid = value; } }


        private DataVariable _thingSpeakWriteTag1 = new DataVariable("ThingSpeakWriteTag1", "ThingSpeakWriteTag1", "A Test Variable", "Test");
       // [Ua("thingspeakwriteX"), Ua("publish")]
        public DataVariable ThingSpeakWriteTag1 { get { return this._thingSpeakWriteTag1; } set { this._thingSpeakWriteTag1 = value; } }
        //````````````````````````````````````````````````````````````````````````````````
        private DataVariable _EtherReadTestTag1 = new DataVariable("EtherReadTestTag1", "EtherReadTestTag1", "A Test Variable", "Test");
        //[Ua("ether_read_test"), Ua("publish")]
        public DataVariable EtherReadTestTag1 { get { return this._EtherReadTestTag1; } set { this._EtherReadTestTag1 = value; } }
        //````````````````````````````````````````````````````````````````````````````````
        private DataVariable _localTemperature = new DataVariable();
       // [Ua("publish")]
        public DataVariable LocalTemperature { get { return this._localTemperature; } set { this._localTemperature = value; } }

        private DataVariable _weatherAlert = new DataVariable();
      //  [Ua("publish")]
        public DataVariable WeatherAlert { get { return this._weatherAlert; } set { this._weatherAlert = value; } }

        private DataVariable _heatIndex = new DataVariable();
//        [Ua("publish")]
        public DataVariable HeatIndex { get { return this._heatIndex; } set { this._heatIndex = value; } }

        private DataVariable _weather = new DataVariable();
  //      [Ua("publish")]
        public DataVariable Weather { get { return this._weather; } set { this._weather = value; } }

        private DataVariable _sunriseTime = new DataVariable();
    //    [Ua("publish")]
        public DataVariable SunriseTime { get { return this._sunriseTime; } set { this._sunriseTime = value; } }

        private DataVariable _sunsetTime = new DataVariable();
      //  [Ua("publish")]
        public DataVariable SunsetTime { get { return this._sunsetTime; } set { this._sunsetTime = value; } }

        private DataVariable _isSunrise = new DataVariable();
        //[Ua("event"), Ua("publish"), Ua("simulated")]
        public DataVariable IsSunrise { get { return this._isSunrise; } set { this._isSunrise = value; } }

        private DataVariable _isSunset = new DataVariable();
       // [Ua("event"), Ua("publish"), Ua("simulated")]
        public DataVariable IsSunset { get { return this._isSunset; } set { this._isSunset = value; } }

        private DataVariable _speakEventMessage = new DataVariable();
      //  [Ua("publish")]
        public DataVariable SpeakEventMessage { get { return this._speakEventMessage; } set { this._speakEventMessage = value; } }
                
        private DataVariable _energyDynamicPriceDoc = new DataVariable();
        //[Ua("publish")]
        //````````````````````````````````````````````````````````````````````````````````
        public DataVariable EnergyDynamicPriceDoc { get { return this._energyDynamicPriceDoc; } set { this._energyDynamicPriceDoc = value; } }

        private DataVariable _energyGridAlert = new DataVariable();
        //[Ua("publish")]
        public DataVariable EnergyGridAlert { get { return this._energyGridAlert; } set { this._energyGridAlert = value; } }

        private DataVariable _energyDynamicPrice = new DataVariable();
        //[Ua("publish"), Ua("broadcast_celestial"), Ua("broadcast_collective")]
        public DataVariable EnergyDynamicPrice { get { return this._energyDynamicPrice; } set { this._energyDynamicPrice = value; } }
        //````````````````````````````````````````````````````````````````````````````````
        private DataVariable _location = new DataVariable();
        [Ua("publish")]
        public DataVariable Location { get { return this._location; } set { this._location = value; } }

        //private DataVariable _voiceCommandFile = new DataVariable();
        //[Ua("manualinput")]
        //public DataVariable VoiceCommandFile { get { return this._voiceCommandFile; } set { this._voiceCommandFile = value; } }

        private string _agentState = null;
        public string AgentState { get { return this._agentState; } set { this._agentState = value; } }

        //private DataVariable _upperbay_switch = new DataVariable("UpperbaySwitch", "UpperbaySwitch", "SmartThings Switch", "on", "smartthings/switch/upperbay_switch/state");
        //[Ua("smartthings_publish")]
        //public DataVariable UpperbaySwitch { get { return this._upperbay_switch; } set { this._upperbay_switch = value; } }

        //private DataVariable _upperbay_level = new DataVariable("UpperbayLevel", "UpperbayLevel", "SmartThings Level", "on", "smartthings/level/upperbay_level/state");
        ////[Ua("smartthings_publish")]
        //public DataVariable UpperbayLevel { get { return this._upperbay_level; } set { this._upperbay_level = value; } }

        private DataVariable _beat_the_peak_switch = new DataVariable("BeatThePeak", "BeatThePeak", "SmartThings BeatThePike Switch", "off", "smartthings/switch/BeatThePeak/state");
        [Ua("smartthings_publish")]
        public DataVariable BeatThePeak { get { return this._beat_the_peak_switch; } set { this._beat_the_peak_switch = value; } }

        private DataVariable _beat_the_peak_level = new DataVariable("BeatThePeakLevel", "BeatThePeakLevel", "SmartThings BeatThePike Level", "0", "smartthings/level/BeatThePeakLevel/state");
        [Ua("smartthings_publish")]
        public DataVariable BeatThePeakLevel { get { return this._beat_the_peak_level; } set { this._beat_the_peak_level = value; } }

        private DataVariable _gridPeakDetected = new DataVariable("GridPeakDetected", "GridPeakDetected", "AI peak detector interface", "json");
        [Ua("subscribe")]
        public DataVariable GridPeakDetected { get { return this._gridPeakDetected; } set { this._gridPeakDetected = value; } }

        private DataVariable _manualGameReset = new DataVariable("ManualGameReset", "ManualGameReset", "Reset Game", "Boolean");
        [Ua("manualinput")]
        public DataVariable ManualGameReset { get { return this._manualGameReset; } set { this._manualGameReset = value; } }

        #endregion


        #region INativeAgent Members - OnInitialize

        //------------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostColony"></param>
        /// <returns></returns>
        public bool OnInitialize(IHostColonyServices hostColony)
        {

            this._hostColony = hostColony;
            this._isAlive = true;

            // Cache Host Colony Services and Initialize
            CacheColonyState();
            InitializeExternals();

            MyAppConfig.AppConfigAddNewParms();
            MyAppConfig.SyncAppConfig();
            
            InitializeAssistants();

            //````````````````````````````````````````````````````````````````````````````````
            // Read DLL App.config file settings


            string period = MyAppConfig.GetParameter("Period");

            if (period != null) _period = Int32.Parse(period);
            if (period != null) Log2.Debug("Agent Config Data: {0} period = {1}", _agentName, period);

            string location = MyAppConfig.GetParameter("Location");
            if (location != null)
            {
                this.Location.Value = location;
                this.Location.Quality = "Good";
                this.Location.UpdateTime = DateTime.Now;
                Log2.Trace("Agent Config Data: {0} Location = {1}", _agentName, location);
            }

            string clusterName = MyAppConfig.GetParameter("ClusterName");
            string clusterVersion = MyAppConfig.GetParameter("ClusterVersion");
            if (clusterName != null)
            {
                Log2.Info("ClusterName: {0}, ClusterVersion: {1}", clusterName, clusterVersion);
            }

            string adminOnly = MyAppConfig.GetParameter("AdminOnly");
            if (adminOnly == "true")
            {
                Log2.Info("Service Running in ADMIN ONLY Mode");
            }

            string aiGameEnable = MyAppConfig.GetParameter("AIGameEnable");
            if (aiGameEnable == "true")
            {
                Log2.Info("Service Running in AI GAME ENABLE!");
            }

            _variableProcessorAssistant = new VariableProcessor();
            _variableProcessorAssistant.Initialize(this._agentPassPort.ClassName, _agentName, this);

            Log2.Debug("Agent Initialized: {0}", _agentName);

            //Bump priority to make sure we don't miss the action
            //Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;

            return false;
        }

        #endregion

        #region INativeAgent Members - OnStart

        /// <summary>
        /// 
        /// </summary>
        public void OnStart()
        {
            //Set Current Thread to agent name for logging
            Thread thread = Thread.CurrentThread;
            string s = this._agentPassPort.ServiceName + "." + this._agentPassPort.AgentName;
            thread.Name = s;
           
            try
            {
                string vocalName = MyAppConfig.GetParameter("VocalName");
                string startupMessage = MyAppConfig.GetParameter("StartupMessage");
                string voice = MyAppConfig.GetParameter("Voice");

                Log2.Trace("Voice = {0}", voice);
                //Voice.SetVoice(voice);
                DateTime dateAndTime = DateTime.Now;
                Voice.Speak("This is " + vocalName + ". The time is " + dateAndTime.ToShortTimeString() + ". " + startupMessage);
            }
            catch (Exception ex)
            {
                Log2.Error("Voice Synth Died: ", ex);
            }

            GridPeakDetected.Value = "NOGAME";
            GridPeakDetected.LastValue = "NOGAME";
            GridPeakDetected.ChangeFlag = false;

            ManualGameReset.Value = "false";
            ManualGameReset.LastValue = "false";
            ManualGameReset.ChangeFlag = false;

            BeatThePeak.Value = "off";
            BeatThePeak.LastValue = "on";
            BeatThePeak.ChangeFlag = true;

            BeatThePeakLevel.Value = "0";
            BeatThePeakLevel.LastValue = "25";
            BeatThePeakLevel.ChangeFlag = true;

            try
            {
                Log2.Info("Agent Starting Up: {0}",this._agentName);
                _variableProcessorAssistant.Start();

                if (_hasGameEventAssistant) _gameEventAssistant.Start();
                if (_hasEventAssistant) _eventAssistant.Start();
                if (_hasCacheAssistant) _cacheAssistant.Start();
                if (_hasManualInputAssistant) _manualInputAssistant.Start();
                if (_hasEtherReadAccessorAssistant) _etherReadAccessorAssistant.Start();
                if (_hasEtherWriteAccessorAssistant) _etherWriteAccessorAssistant.Start();

                if (_hasSimulatorAssistant) _simulatorAssistant.Start();
                if (_hasMqttPublisherAssistant) _mqttPublisherAssistant.Start();
                if (_hasSmartthingsPublisherAssistant) _smartthingsPublisherAssistant.Start();
                if (_hasMqttSubscriberAssistant) _mqttSubscriberAssistant.Start();
                if (_hasCloudMqttPublisherAssistant) _cloudMqttPublisherAssistant.Start();
                if (_hasCloudMqttSecureSubscriberAssistant) _cloudMqttSecureSubscriberAssistant.Start();

                if (_hasThingSpeakReadAccessorAssistant) _thingSpeakReadAccessorAssistant.Start();
                if (_hasThingSpeakWriteAccessorAssistant) _thingSpeakWriteAccessorAssistant.Start();
                if (_hasWeatherServiceAssistant) _weatherServiceAssistant.Start();

                Log2.Info("Agent ONLINE {0}", this._agentName);

                MessageMailer messageMailer = new MessageMailer();
                string smsMessageBody = "Agent is ONLINE";
                messageMailer.SendSMSText(smsMessageBody);
                

            }
            catch (Exception Ex)
            {
                Log2.Error("Exception in OnStart: {0} {1}", _agentName, Ex.ToString());
            }

            OnFire();

        }
        #endregion

        #region INativeAgent Members - OnFire

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool OnFire()
        {
            // PATTERN
            //while (this.IsAlive)
            //{
            //    this.hostColony.LogInfoIf(_agentSwitch, "{0}: Agent OnFire", this.hostColonyContext.AgentName);

            //    // Do something interesting until told not to.
            //````````````````````````````````````````````````````````````````````````````````
            //````````````````````````````````````````````````````````````````````````````````
            //    // Sleep for some period of time and so it all over again.
            //    Thread.Sleep(2000);
            //}
            //return true;
            

            //int count = 0;
            while (this._isAlive)
            {
                try
                {
                    Log2.Trace("{0}: Agent OnFire", _agentName);

                    //ConfigurationManager.RefreshSection("appSettings");
                    //ConfigurationManager.GetSection("appSettings");

                    //AppConfig appConfig = new AppConfig(_agentName);
                    //string traceMode = appConfig.GetAppConfigParameter("TraceMode");
                    string traceMode = MyAppConfig.GetParameter("TraceMode");

                    Log2.SetLoggingLevel(traceMode);

                    // Run Input Exec Assistants
                    _variableProcessorAssistant.Fire();
                    if (_hasCacheAssistant) _cacheAssistant.Fire();
                    if (_hasManualInputAssistant) _manualInputAssistant.Fire();
                    if (_hasSimulatorAssistant) _simulatorAssistant.Fire();
                    if (_hasEtherReadAccessorAssistant) _etherReadAccessorAssistant.Fire();
                    if (_hasThingSpeakReadAccessorAssistant) _thingSpeakReadAccessorAssistant.Fire();
                    if (_hasMqttSubscriberAssistant) _mqttSubscriberAssistant.Fire();
                    if (_hasCloudMqttSecureSubscriberAssistant) _cloudMqttSecureSubscriberAssistant.Fire();
                    if (_hasEventAssistant) _eventAssistant.Fire();
                    if (_hasGameEventAssistant) _gameEventAssistant.Fire();

                    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

                    //EventVariable ev = new EventVariable();
                    //ev.EventName = "fxxx it";
                    //ev.EventType = "type adr";
                    //EventVariableCache.WriteEventQueue(ev);
                    //Log2.Info("EVENT FIRED: {0}", ev.EventName);


                    //if (SoilMoistureAtCampDavid.ChangeFlag == true) // read change flag
                    //{

                    //    Log2.Trace("SoilMoistureAtCampDavid = {0}", VoltsAtCampDavid.Value);
                    //    Log2.Trace("SoilMoistureAtCampDavid Status = {0}", VoltsAtCampDavid.Status);
                    //    Log2.Trace("SoilMoistureAtCampDavid Time= {0}", VoltsAtCampDavid.UpdateTime.ToString());

                    //    float soil = float.Parse(SoilMoistureAtCampDavid.Value);
                    //    if (soil < 22.0)
                    //    {
                    //        LowSoilMoistureAtCampDavid.Value = SoilMoistureAtCampDavid.Value;
                    //        LowSoilMoistureAtCampDavid.Status = SoilMoistureAtCampDavid.Status;
                    //        LowSoilMoistureAtCampDavid.UpdateTime = SoilMoistureAtCampDavid.UpdateTime;
                    //        LowSoilMoistureAtCampDavid.ChangeFlag = true;
                    //    }
                    //    SoilMoistureAtCampDavid.ChangeFlag = false;
                    //}

                    Log2.Trace("CurrentForCarbonWatts TagName = {0}", CurrentForCarbonWatts.TagName.ToString());
                    Log2.Trace("CurrentForCarbonWatts ExternalName = {0}", CurrentForCarbonWatts.ExternalName.ToString());
                    Log2.Trace("CurrentForCarbonWatts = {0}", CurrentForCarbonWatts.Value);
                    Log2.Trace("CurrentForCarbonWatts Status = {0}", CurrentForCarbonWatts.Status);
                    Log2.Trace("CurrentForCarbonWatts Quality = {0}", CurrentForCarbonWatts.Quality);
                    Log2.Trace("CurrentForCarbonWatts Time= {0}", CurrentForCarbonWatts.UpdateTime.ToString());

                    if (CurrentForCarbonWatts.ChangeFlag == true) // read change flag
                    {
                     
                        WattsMirrorAtCampDavid.Value = CurrentForCarbonWatts.Value;
                        WattsMirrorAtCampDavid.Status = CurrentForCarbonWatts.Status;
                        WattsMirrorAtCampDavid.UpdateTime = CurrentForCarbonWatts.UpdateTime;
                        WattsMirrorAtCampDavid.ChangeFlag = true;

                        string sVal = CurrentForCarbonWatts.Value;
                        float watts = float.Parse(sVal);
                        //if (watts > 5000.0)
                        //{
                        //    ChargingNikkiAtCampDavid.Value = WattsAtCampDavid.Value;
                        //    ChargingNikkiAtCampDavid.Status = WattsAtCampDavid.Status;
                        //    ChargingNikkiAtCampDavid.UpdateTime = DateTime.Now;
                        //    ChargingNikkiAtCampDavid.ChangeFlag = true;
                        //}
                        CurrentForCarbonWatts.ChangeFlag = false;
                    }
                    //---------------------------------

                    try
                    {
                        string adminOnly = MyAppConfig.GetParameter("AdminOnly");
                        if (adminOnly == "true")
                        {

                            if (ManualGameReset.Value != ManualGameReset.LastValue)
                            {
                                ManualGameReset.ChangeFlag = true;
                            }

                            if (ManualGameReset.ChangeFlag == true)
                            {
                                if (ManualGameReset.Value == "true")
                                {
                                    Log2.Debug("NEW ManualGameReset.Value = {0}", ManualGameReset.Value);
                                    GridPeakDetected.Value = "NOGAME";
                                }
                                else
                                {
                                    Log2.Debug("NEW ManualGameReset.Value = {0}", ManualGameReset.Value);
                                }
                                ManualGameReset.LastValue = ManualGameReset.Value;
                                ManualGameReset.ChangeFlag = false;
                            }

                            if (GridPeakDetected.Value != "NOGAME")
                            {
                                // if the game is new
                                if (GridPeakDetected.Value != GridPeakDetected.LastValue)
                                {
                                    GridPeakDetected.ChangeFlag = true;
                                    JsonGridPeakDetected jsonNewGridPeakDetected = new JsonGridPeakDetected();
                                    GridPeakDetectedObject gridNewPeakDetectedObject = jsonNewGridPeakDetected.Json2GridPeakDetected(GridPeakDetected.Value);
                                    Log2.Debug("NEW GridPeakDetected.Value = {0}", GridPeakDetected.Value);
                                    Log2.Debug("agent_name: " + gridNewPeakDetectedObject.agent_name);
                                    Log2.Debug("message: " + gridNewPeakDetectedObject.message);
                                    Log2.Debug("start_date_time: " + gridNewPeakDetectedObject.start_date_time);
                                    Log2.Debug("duration_mins: " + gridNewPeakDetectedObject.duration_mins);
                                    Log2.Debug("peak_lmp: " + gridNewPeakDetectedObject.peak_lmp);
                                    Log2.Debug("award_level: " + gridNewPeakDetectedObject.award_level);
                                    Log2.Debug("game_type: " + gridNewPeakDetectedObject.game_type);
                                    GridPeakDetected.LastValue = GridPeakDetected.Value;
                                    GridPeakDetected.ChangeFlag = false;
                                }

                                // test the game that is in the variable
                                JsonGridPeakDetected jsonGridPeakDetected = new JsonGridPeakDetected();
                                GridPeakDetectedObject gridPeakDetectedObject = jsonGridPeakDetected.Json2GridPeakDetected(GridPeakDetected.Value);
                                string targetDateTime = gridPeakDetectedObject.start_date_time;
                                //string dateTimeFormat = "MM/dd/yyyy hh:mm:ss tt";
                                // start_date_time is 5 mins from now, the just do it
                                //DateTime target = DateTime.Parse(targetDateTime, CultureInfo.CurrentCulture);
                                DateTime target = DateTime.Parse(targetDateTime, CultureInfo.InvariantCulture);
                                DateTime now = DateTime.Now;
                                TimeSpan difference = target - now;

                                // Check if the difference is greater than 0 (future) and less than or equal to 5 minutes
                                if ((difference.TotalMinutes > 0) && (difference.TotalMinutes <= 5))
                                {
                                    Log2.Debug("LET THE GAMES BEGIN!!!!: {0}", GridPeakDetected.Value);
                                    Log2.Debug("agent_name: " + gridPeakDetectedObject.agent_name);
                                    Log2.Debug("message: " + gridPeakDetectedObject.message);
                                    Log2.Debug("start_date_time: " + gridPeakDetectedObject.start_date_time);
                                    Log2.Debug("duration_mins: " + gridPeakDetectedObject.duration_mins);
                                    Log2.Debug("peak_lmp: " + gridPeakDetectedObject.peak_lmp);
                                    Log2.Debug("award_level: " + gridPeakDetectedObject.award_level);
                                    Log2.Debug("game_type: " + gridPeakDetectedObject.game_type);

                                    // if it's time, then start game
                                    if (GameStarter.CreateGameJson(gridPeakDetectedObject))
                                    {
                                        string aiGameEnable = MyAppConfig.GetParameter("AIGameEnable");
                                        if (aiGameEnable == "true")
                                        {
                                            GameStarter.StartGame();
                                            Log2.Info("Game Started");
                                            GridPeakDetected.Value = "NOGAME";
                                        }
                                        else
                                        {
                                            Log2.Info("AIGameEnable is false so game has not been started.");
                                            GridPeakDetected.Value = "NOGAME";
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log2.Error("GridPeak Error" + ex.Message);
                    }

                    //---------------------------------
                    BeatThePeak.Value = GameStatus.GetState();
                    if (BeatThePeak.Value != BeatThePeak.LastValue)
                    {
                        BeatThePeak.LastValue = BeatThePeak.Value;
                        BeatThePeak.ChangeFlag = true;
                        Log2.Debug("BeatThePeak.Value = {0}", BeatThePeak.Value);
                    }
                    else
                    {
                        BeatThePeak.ChangeFlag = false;
                    }


                    //if (BeatThePeak.Value == "on")
                    //    BeatThePeak.Value = "off";
                    //else if (BeatThePeak.Value == "off")
                    //    BeatThePeak.Value = "on";



                    BeatThePeakLevel.Value = GameStatus.GetLevel();
                    if (BeatThePeakLevel.Value != BeatThePeakLevel.LastValue)
                    {
                        BeatThePeakLevel.LastValue = BeatThePeakLevel.Value;
                        BeatThePeakLevel.ChangeFlag = true;
                        Log2.Debug("BeatThePeakLevel.Value = {0}", BeatThePeakLevel.Value);
                    }
                    else
                    {
                        BeatThePeakLevel.ChangeFlag = false;
                    }


                    //if (BeatThePeakLevel.Value == "40")
                    //    BeatThePeakLevel.Value = "60";
                    //else if (BeatThePeakLevel.Value == "60")
                    //    BeatThePeakLevel.Value = "40";



                    // Run Output Exec Assistants
                    if (_hasThingSpeakWriteAccessorAssistant) _thingSpeakWriteAccessorAssistant.Fire();
                    if (_hasEtherWriteAccessorAssistant) _etherWriteAccessorAssistant.Fire();
                    if (_hasMqttPublisherAssistant) _mqttPublisherAssistant.Fire();
                    if (_hasSmartthingsPublisherAssistant) _smartthingsPublisherAssistant.Fire();
                    if (_hasCloudMqttPublisherAssistant) _cloudMqttPublisherAssistant.Fire();



                    //Run Staff Assistants
                    //if (_hasWeatherServiceAssistant)
                    //{
                    //    if (_weatherServiceAssistant.Fire())
                    //    {
                    //        if (this.LocalTemperature.Value != _weatherServiceAssistant.CurrentTemperature)
                    //        {
                    //            this.LocalTemperature.Value = _weatherServiceAssistant.CurrentTemperature;
                    //            this.LocalTemperature.Quality = "Good"; this.LocalTemperature.UpdateTime = DateTime.Now;
                    //            Voice.Speak("The temperature is " + this.LocalTemperature.Value);
                    //        }

                    //        if (this.WeatherAlert.Value != _weatherServiceAssistant.CurrentAlertWarning)
                    //        {
                    //            this.WeatherAlert.Value = _weatherServiceAssistant.CurrentAlertWarning;
                    //            this.WeatherAlert.Quality = "Good"; this.WeatherAlert.UpdateTime = DateTime.Now;
                    //            //synth.Speak("There is a new weather alert.");
                    //        }

                    //        this.HeatIndex.Value = _weatherServiceAssistant.CurrentHeatIndex;
                    //        this.HeatIndex.Quality = "Good"; this.HeatIndex.UpdateTime = DateTime.Now;

                    //        if (this.Weather.Value != _weatherServiceAssistant.CurrentWeather)
                    //        {
                    //            this.Weather.Value = _weatherServiceAssistant.CurrentWeather;
                    //            this.Weather.Quality = "Good"; this.Weather.UpdateTime = DateTime.Now;
                    //            Voice.Speak("The weather is " + this.Weather.Value);
                    //        }
                    //    }

                    //    // Process Sunrise Data
                    //    this.SunriseTime.Value = _weatherServiceAssistant.TimeOfSunrise;
                    //    this.SunriseTime.Quality = "Good";
                    //    this.SunriseTime.UpdateTime = DateTime.Now;

                    //    this.IsSunrise.Value = _weatherServiceAssistant.IsSunrise;
                    //    this.IsSunrise.Quality = "Good";
                    //    this.IsSunrise.UpdateTime = DateTime.Now;


                    //    // Process Sunset Data
                    //    this.SunsetTime.Value = _weatherServiceAssistant.TimeOfSunset;
                    //    this.SunsetTime.Quality = "Good";
                    //    this.SunsetTime.UpdateTime = DateTime.Now;

                    //    this.IsSunset.Value = _weatherServiceAssistant.IsSunset;
                    //    this.IsSunset.Quality = "Good";
                    //    this.IsSunset.UpdateTime = DateTime.Now;
                    //}

                    //````````````````````````````````````````````````````````````````````````````````
                    //Time Trigger Processors

                    if (_hourTimeTrigger.IsHour(1))
                    {
                        Log2.Trace("Hourly Trigger Fired: {0}", DateTime.Now.ToShortTimeString());
                      
                    }
                    if (_5minTimeTrigger.Is1Min(0))
                    {
                        Log2.Trace("1Min Trigger Fired: {0}", DateTime.Now.ToShortTimeString());

                    }
                    if (_5minTimeTrigger.Is2Min(0))
                    {
                        Log2.Trace("2Min Trigger Fired: {0}", DateTime.Now.ToShortTimeString());
                        //ThingSpeakAccess thingSpeakAccess = new ThingSpeakAccess();
                       // thingSpeakAccess.GetADREventValues();
                    }
                    if (_5minTimeTrigger.Is5Min(1))
                    {
                        Log2.Trace("5Min Trigger Fired: {0}", DateTime.Now.ToShortTimeString());
                     
                    }
                    if (_10minTimeTrigger.Is10Min(0))
                    {
                        Log2.Trace("10Min Trigger Fired: {0}", DateTime.Now.ToShortTimeString());
                        //string cmdFileName = ConfigurationManager.AppSettings["VoiceCommandFile"];
                        //Log2.Trace("Alexa Filename: {0}", cmdFileName);
                       // Voice.SpeakVoiceCommandFile(cmdFileName);
                    }

                    if (_15minTimeTrigger.Is15Min(0))
                    {
                        Log2.Trace("15Min Trigger Fired: {0}", DateTime.Now.ToShortTimeString());
                    }
                    if (_20minTimeTrigger.Is20Min(0))
                    {
                        Log2.Trace("20min Trigger Fired: {0}", DateTime.Now.ToShortTimeString());
                    }
                    if (_30minTimeTrigger.Is30Min(0))
                    {
                        Log2.Trace("30min Trigger Fired: {0}", DateTime.Now.ToShortTimeString());
                    }
                    if (_45minTimeTrigger.Is45Min(0))
                    {
                        Log2.Trace("45min Trigger Fired: {0}", DateTime.Now.ToShortTimeString());
                    }
                    if (_60minTimeTrigger.Is60Min(0))
                    {
                        Log2.Trace("60min Trigger Fired: {0}", DateTime.Now.ToShortTimeString());
                    }

                    _timeLastExecuted = DateTime.Now;

                    //    //````````````````````````````````````````````````````````````````````````````````
                    //    tempStartTime = DateTime.MinValue;
                    //    tempEndTime = DateTime.MinValue;
                    //    DateTime.TryParse("21:10:00", out tempStartTime);
                    //    DateTime.TryParse("21:11:20", out tempEndTime);
                    //    if ((DateTime.Now >= tempStartTime) && (DateTime.Now < tempEndTime))
                    //    {

                    //    }


                }
                catch (Exception Ex)
                {
                    Log2.Error("Exception in OnFire: {0} {1}", _agentName, Ex.ToString());
                }
                Thread.Sleep(_period);
            }
            return true;
        }

        #endregion
      
        #region INativeAgent Members - OnStop

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool OnStop()
        {
            string fileName = MyAppConfig.GetParameter("CacheDataFile");

            string filePath = "logs\\" + fileName;
            //string path = Directory.GetCurrentDirectory();
            //string newpath = path + "\\" + "output\\DataVariableCacheDump.txt";

            //DataVariableCache.Cache2File(filePath);

            try
            {
                string vocalName = MyAppConfig.GetParameter("VocalName");
                string shutdownMessage = MyAppConfig.GetParameter("ShutdownMessage");
                string voice = MyAppConfig.GetParameter("Voice");

                Log2.Trace("Voice = {0}", voice);
               // Voice.SetVoice(voice);
                Voice.Speak("This is " + vocalName + ". " + shutdownMessage);

            }
            catch (Exception ex)
            {
                Log2.Error("Voice Synth Died: ", ex);
            }
          
            Log2.Trace("Upperbay Agent {0} is Sleeping!", _agentName);

            if (_hasEventAssistant) _eventAssistant.Stop();
            if (_hasGameEventAssistant) _gameEventAssistant.Stop();
            if (_hasManualInputAssistant) _manualInputAssistant.Stop();
            if (_hasEtherReadAccessorAssistant) _etherReadAccessorAssistant.Stop();
            if (_hasEtherWriteAccessorAssistant) _etherWriteAccessorAssistant.Stop();
            if (_hasSimulatorAssistant) _simulatorAssistant.Stop();
            if (_hasSmartthingsPublisherAssistant) _smartthingsPublisherAssistant.Stop();
            if (_hasMqttSubscriberAssistant) _mqttSubscriberAssistant.Stop();
            if (_hasMqttPublisherAssistant) _mqttPublisherAssistant.Stop();
            if (_hasCloudMqttPublisherAssistant) _cloudMqttPublisherAssistant.Stop();
            if (_hasCloudMqttSecureSubscriberAssistant) _cloudMqttSecureSubscriberAssistant.Stop();
            if (_hasThingSpeakWriteAccessorAssistant) _thingSpeakWriteAccessorAssistant.Stop();
            if (_hasThingSpeakReadAccessorAssistant) _thingSpeakReadAccessorAssistant.Stop();
            if (_hasWeatherServiceAssistant) _weatherServiceAssistant.Stop();
            if (_hasCacheAssistant) _cacheAssistant.Stop();

            _variableProcessorAssistant.Stop();

            MessageMailer messageMailer = new MessageMailer();
            string smsMessageBody = "Agent is OFFLINE";
            messageMailer.SendSMSText(smsMessageBody);


            Log2.Trace("{0}: Agent OnStop", _agentName);

            if (this._isAlive)
                _isAlive = false;

            Log2.Trace("{0}: Agent Stopped", _agentName);
            return true;
        }
        #endregion
       

        #region Private Helper Methods

        // ---------------------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        private void CacheColonyState()
        {
            // Cache local copy of Host Colony Context data
            // Create and initialize an Agent Pass Port
            this._agentPassPort = new AgentPassPort();

            this._agentPassPort.Celestial = this._hostColony.CelestialName;
            this._agentPassPort.Collective = this._hostColony.CollectiveName;
            this._agentPassPort.Community = this._hostColony.CommunityName;
            this._agentPassPort.Cluster = this._hostColony.ClusterName;
            this._agentPassPort.Colony = this._hostColony.ColonyName;
            this._agentPassPort.AgentRole = this._hostColony.AgentRole;
            this._agentPassPort.ClassName = this._hostColony.ClassName;
            this._agentPassPort.Version = this._hostColony.Version;
            this._agentPassPort.Description = this._hostColony.Description;
            this._agentPassPort.AssemblyPath = this._hostColony.AssemblyPath;
            this._agentPassPort.AssemblyName = this._hostColony.AssemblyName;
            this._agentPassPort.AssemblyDirectory = this._hostColony.AssemblyDirectory;
            this._agentPassPort.Carrier = this._hostColony.CarrierName;
            this._agentPassPort.ServiceName = this._hostColony.ServiceName;
            this._agentPassPort.ColonyServiceName = this._hostColony.ColonyServiceName;
            this._agentPassPort.AgentName = this._hostColony.AgentName;
            this._agentPassPort.LogicalName = this._hostColony.LogicalName;
            this._agentPassPort.AgentNickName = this._hostColony.AgentNickName;

            this._agentPassPort.GenerateFullName();

            ClusterAgent.AgentName = this._agentPassPort.AgentName;
            this._agentName = this._agentPassPort.AgentName;
           
            Log2.Trace("Agent Initialized: {0}", this._agentName);
        }
        

        /// <summary>
        /// 
        /// </summary>
        private void InitializeExternals()
        {
            try
            {
                //// Specify agent config settings at runtime.
                System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.File = "config\\" + _agentName + ".app.config";
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
                ConfigurationManager.GetSection("appSettings");

                //AppConfig appConfig = new AppConfig(_agentName);
                string traceMode = ConfigurationManager.AppSettings["TraceMode"];
             
                Thread thread = Thread.CurrentThread;
                string s = this._hostColony.ServiceName + "." + this._hostColony.AgentName;
                thread.Name = s;

                //Debug, Trace, Information
                Log2.LogInit(_hostColony.ServiceName, _hostColony.AgentName, traceMode);
                Log2.Info("{0} {1}: Logging TraceMode: {2}", _hostColony.ServiceName, _agentName, traceMode);
                //Log2.Info("Agent appSettings File: {0}", config.AppSettings.File);

                MyAppConfig.SetMyAppConfig(_agentName);
            }
            catch (Exception Ex)
            {
                Log2.Error("{0}: Exception in InitializeExternals: {1}", _agentName, Ex.ToString());
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void InitializeAssistants()
        {
            try
            {
                string enableCommunity = MyAppConfig.GetParameter("enableCommunity");
                if (enableCommunity != null) Boolean.TryParse(enableCommunity, out _enableCommunity);
                if (enableCommunity != null)
                {
                    Log2.Trace("{0}: Agent Config Data: _enableCommunity = {1}", _agentName, enableCommunity);
                }

                string enableCluster = MyAppConfig.GetParameter("enableCluster");
                if (enableCluster != null) Boolean.TryParse(enableCluster, out _enableCluster);
                if (enableCluster != null)
                {
                    Log2.Trace("{0}: Agent Config Data: _enableCluster = {1}", _agentName, enableCluster);
                }
                //````````````````````````````````````````````````````````````````````````````````
                // Event Assistant
                string hasEventAssistant = MyAppConfig.GetParameter("hasEventAssistant");
                if (hasEventAssistant != null) Boolean.TryParse(hasEventAssistant, out _hasEventAssistant);
                if (_hasEventAssistant)
                {
                    Log2.Trace("{0}: Agent Config Data: hasEventAssistant = {1}", _agentName, hasEventAssistant);
                    _eventAssistant = new EventReactor();
                    _eventAssistant.AgentPassPort = this._agentPassPort;
                    _eventAssistant.Initialize(this._agentPassPort.ClassName, _agentName, this);
                }
                //````````````````````````````````````````````````````````````````````````````````
                // GameEvent Assistant
                string hasGameEventAssistant = MyAppConfig.GetParameter("hasGameEventAssistant");
                if (hasGameEventAssistant != null) Boolean.TryParse(hasGameEventAssistant, out _hasGameEventAssistant);
                if (_hasGameEventAssistant)
                {
                    Log2.Trace("{0}: Agent Config Data: hasGameEventAssistant = {1}", _agentName, hasGameEventAssistant);
                    _gameEventAssistant = new GameEventReactor();
                    _gameEventAssistant.AgentPassPort = this._agentPassPort;
                    _gameEventAssistant.Initialize(this._agentPassPort.ClassName, _agentName, this);
                }
                //````````````````````````````````````````````````````````````````````````````````
                // Cache Assistant
                string hasCacheAssistant = MyAppConfig.GetParameter("hasCacheAssistant");
                if (hasCacheAssistant != null) Boolean.TryParse(hasCacheAssistant, out _hasCacheAssistant);
                if (_hasCacheAssistant)
                {
                    Log2.Trace("{0}: Agent Config Data: hasCacheAssistant = {1}", _agentName, hasCacheAssistant);
                    _cacheAssistant = new Cache();
                    _cacheAssistant.AgentPassPort = this._agentPassPort;
                    _cacheAssistant.Initialize(this._agentPassPort.ClassName, _agentName, this);
                }
                //````````````````````````````````````````````````````````````````````````````````
                // ManualInput Assistant
                string hasManualInputAssistant = MyAppConfig.GetParameter("hasManualInputAssistant");
                if (hasManualInputAssistant != null) Boolean.TryParse(hasManualInputAssistant, out _hasManualInputAssistant);
                if (_hasManualInputAssistant)
                {
                    Log2.Trace("{0}: Agent Config Data: hasManualInputAssistant = {1}", _agentName, hasManualInputAssistant);
                    _manualInputAssistant = new ManualInput();
                    _manualInputAssistant.AgentPassPort = this._agentPassPort;
                    _manualInputAssistant.Initialize(this._agentPassPort.ClassName, _agentName, this);
                } 
                //````````````````````````````````````````````````````````````````````````````````
                // MqttPublisher Assistant
                string hasMqttPublisherAssistant = MyAppConfig.GetParameter("hasMqttPublisherAssistant");
                if (hasMqttPublisherAssistant != null) Boolean.TryParse(hasMqttPublisherAssistant, out _hasMqttPublisherAssistant);
                if (_hasMqttPublisherAssistant)
                {
                    Log2.Trace("{0}: Agent Config Data: hasMqttPublisherAssistant = {1}", _agentName, hasMqttPublisherAssistant);
                    _mqttPublisherAssistant = new MqttPublisher();
                    _mqttPublisherAssistant.AgentPassPort = this._agentPassPort;
                    _mqttPublisherAssistant.Initialize(this._agentPassPort.ClassName, _agentName, this);
                }
                //````````````````````````````````````````````````````````````````````````````````
                // SmartthingsPublisher Assistant
                string hasSmartthingsPublisherAssistant = MyAppConfig.GetParameter("hasSmartthingsPublisherAssistant");
                if (hasSmartthingsPublisherAssistant != null) Boolean.TryParse(hasSmartthingsPublisherAssistant, out _hasSmartthingsPublisherAssistant);
                if (_hasSmartthingsPublisherAssistant)
                {
                    Log2.Trace("{0}: Agent Config Data: hasSmartthingsPublisherAssistant = {1}", _agentName, hasSmartthingsPublisherAssistant);
                    _smartthingsPublisherAssistant = new SmartthingsPublisher();
                    _smartthingsPublisherAssistant.AgentPassPort = this._agentPassPort;
                    _smartthingsPublisherAssistant.Initialize(this._agentPassPort.ClassName, _agentName, this);
                }
                //````````````````````````````````````````````````````````````````````````````````
                // MqttSubscriber Assistant
                string hasMqttSubscriberAssistant = MyAppConfig.GetParameter("hasMqttSubscriberAssistant");
                if (hasMqttSubscriberAssistant != null) Boolean.TryParse(hasMqttSubscriberAssistant, out _hasMqttSubscriberAssistant);
                if (_hasMqttSubscriberAssistant)
                {
                    Log2.Trace("{0}: Agent Config Data: hasMqttSubscriberAssistant = {1}", _agentName, hasMqttSubscriberAssistant);
                    _mqttSubscriberAssistant = new MqttSubscriber();
                    _mqttSubscriberAssistant.AgentPassPort = this._agentPassPort;
                    _mqttSubscriberAssistant.Initialize(this._agentPassPort.ClassName, _agentName, this);
                }
                //````````````````````````````````````````````````````````````````````````````````
                // CloudMqttPublisher Assistant
                string hasCloudMqttPublisherAssistant = MyAppConfig.GetParameter("hasCloudMqttPublisherAssistant");
                if (hasCloudMqttPublisherAssistant != null) Boolean.TryParse(hasCloudMqttPublisherAssistant, out _hasCloudMqttPublisherAssistant);
                if (_hasCloudMqttPublisherAssistant)
                {
                    Log2.Trace("{0}: Agent Config Data: hasCloudMqttPublisherAssistant = {1}", _agentName, hasCloudMqttPublisherAssistant);
                    _cloudMqttPublisherAssistant = new CloudMqttPublisher();
                    _cloudMqttPublisherAssistant.AgentPassPort = this._agentPassPort;
                    _cloudMqttPublisherAssistant.Initialize(this._agentPassPort.ClassName, _agentName, this);
                }
                //````````````````````````````````````````````````````````````````````````````````
                // CloudMqttSecureSubscriber Assistant
                string hasCloudMqttSecureSubscriberAssistant = MyAppConfig.GetParameter("hasCloudMqttSecureSubscriberAssistant");
                if (hasCloudMqttSecureSubscriberAssistant != null) Boolean.TryParse(hasCloudMqttSecureSubscriberAssistant, out _hasCloudMqttSecureSubscriberAssistant);
                if (_hasCloudMqttSecureSubscriberAssistant)
                {
                    Log2.Trace("{0}: Agent Config Data: _hasCloudMqttSecureSubscriberAssistant = {1}", _agentName, _hasCloudMqttSecureSubscriberAssistant);
                    _cloudMqttSecureSubscriberAssistant = new CloudMqttSecureSubscriber();
                    _cloudMqttSecureSubscriberAssistant.AgentPassPort = this._agentPassPort;
                    _cloudMqttSecureSubscriberAssistant.Initialize(this._agentPassPort.ClassName, _agentName, this);
                }                //````````````````````````````````````````````````````````````````````````````````
                // Simulator Assistant
                string hasSimulatorAssistant = MyAppConfig.GetParameter("hasSimulatorAssistant");
                if (hasSimulatorAssistant != null) Boolean.TryParse(hasSimulatorAssistant, out _hasSimulatorAssistant);
                if (_hasSimulatorAssistant)
                {
                    Log2.Trace("{0}: Agent Config Data: hasSimulatorAssistant = {1}", _agentName, hasSimulatorAssistant);
                    _simulatorAssistant = new Simulator();
                    _simulatorAssistant.AgentPassPort = this._agentPassPort;
                    _simulatorAssistant.Initialize(this._agentPassPort.ClassName, _agentName, this);
                }
                //````````````````````````````````````````````````````````````````````````````````
                // Ether Read Test Access Assistant
                string hasEtherReadAccessorAssistant = MyAppConfig.GetParameter("hasEtherReadAccessorAssistant");
                if (hasEtherReadAccessorAssistant != null) Boolean.TryParse(hasEtherReadAccessorAssistant, out _hasEtherReadAccessorAssistant);
                if (_hasEtherReadAccessorAssistant)
                {
                    Log2.Trace("{0}: Agent Config Data: hasEtherReadAccessorAssistant = {1}", _agentName, hasEtherReadAccessorAssistant);
                    _etherReadAccessorAssistant = new EtherReadAccessor();
                    _etherReadAccessorAssistant.AgentPassPort = this._agentPassPort;
                    _etherReadAccessorAssistant.Initialize(this._agentPassPort.ClassName, _agentName, this);
                }                
                //````````````````````````````````````````````````````````````````````````````````
                // Ether Write Test Access Assistant
                string hasEtherWriteAccessorAssistant = MyAppConfig.GetParameter("hasEtherWriteAccessorAssistant");
                if (hasEtherWriteAccessorAssistant != null) Boolean.TryParse(hasEtherWriteAccessorAssistant, out _hasEtherWriteAccessorAssistant);
                if (_hasEtherWriteAccessorAssistant)
                {
                    Log2.Trace("{0}: Agent Config Data: hasEtherWriteAccessorAssistant = {1}", _agentName, hasEtherWriteAccessorAssistant);
                    _etherWriteAccessorAssistant = new EtherWriteAccessor();
                    _etherWriteAccessorAssistant.AgentPassPort = this._agentPassPort;
                    _etherWriteAccessorAssistant.Initialize(this._agentPassPort.ClassName, _agentName, this);
                }                
                //````````````````````````````````````````````````````````````````````````````````
                // ThingSpeak Read Access Assistant
                string hasThingSpeakReadAccessorAssistant = MyAppConfig.GetParameter("hasThingSpeakReadAccessorAssistant");
                if (hasThingSpeakReadAccessorAssistant != null) Boolean.TryParse(hasThingSpeakReadAccessorAssistant, out _hasThingSpeakReadAccessorAssistant);
                if (_hasThingSpeakReadAccessorAssistant)
                {
                    Log2.Trace("{0}: Agent Config Data: hasThingSpeakReadAccessorAssistant = {1}", _agentName, hasThingSpeakReadAccessorAssistant);
                    _thingSpeakReadAccessorAssistant = new ThingSpeakReadAccessor();
                    _thingSpeakReadAccessorAssistant.AgentPassPort = this._agentPassPort;
                    _thingSpeakReadAccessorAssistant.Initialize(this._agentPassPort.ClassName, _agentName, this);
                }               
                //````````````````````````````````````````````````````````````````````````````````
                // ThingSpeak Write Access Assistant
                string hasThingSpeakWriteAccessorAssistant = MyAppConfig.GetParameter("hasThingSpeakWriteAccessorAssistant");
                if (hasThingSpeakWriteAccessorAssistant != null) Boolean.TryParse(hasThingSpeakWriteAccessorAssistant, out _hasThingSpeakWriteAccessorAssistant);
                if (_hasThingSpeakWriteAccessorAssistant)
                {
                    Log2.Trace("{0}: Agent Config Data: hasThingSpeakWriteAccessorAssistant = {1}", _agentName, hasThingSpeakWriteAccessorAssistant);
                    _thingSpeakWriteAccessorAssistant = new ThingSpeakWriteAccessor();
                    _thingSpeakWriteAccessorAssistant.AgentPassPort = this._agentPassPort;
                    _thingSpeakWriteAccessorAssistant.Initialize(this._agentPassPort.ClassName, _agentName, this);
                }               
              
                //````````````````````````````````````````````````````````````````````````````````
                // WeatherService Assistant
                string hasWeatherServiceAssistant = MyAppConfig.GetParameter("hasWeatherServiceAssistant");
                if (hasWeatherServiceAssistant != null) Boolean.TryParse(hasWeatherServiceAssistant, out _hasWeatherServiceAssistant);
                if (_hasWeatherServiceAssistant)
                {
                    Log2.Trace("{0}: Agent Config Data: WeatherService = {1}", _agentName, hasWeatherServiceAssistant);
                    _weatherServiceAssistant = new WeatherService();
                    _weatherServiceAssistant.AgentPassPort = this._agentPassPort;
                    _weatherServiceAssistant.Initialize(this._agentPassPort.ClassName, _agentName, this);

                    string weatherStationID = MyAppConfig.GetParameter("WeatherStationID");
                    if (weatherStationID != null) _weatherServiceAssistant.WeatherStationID = weatherStationID;
                    string zipCode = MyAppConfig.GetParameter("ZipCode");
                    if (zipCode != null) _weatherServiceAssistant.WeatherZipCode = zipCode;
                    string weatherAlertZone = MyAppConfig.GetParameter("WeatherAlertZone");
                    if (weatherAlertZone != null) _weatherServiceAssistant.WeatherAlertZone = weatherAlertZone;
                }
              
            }
            catch (Exception Ex)
            {
                Log2.Error("{0}: InitializeAssistants FAILED: {1}", _agentName, Ex.ToString());
            }
        }



        #endregion

        #region Private State Members


        // Assistants

        private bool _hasGameEventAssistant = false;
        private Upperbay.Assistant.GameEventReactor _gameEventAssistant = null;

        private bool _hasEventAssistant = false;
        private Upperbay.Assistant.EventReactor _eventAssistant = null;
        
        private bool _hasEtherReadAccessorAssistant = false;
        private Upperbay.Assistant.EtherReadAccessor _etherReadAccessorAssistant = null;

        private bool _hasEtherWriteAccessorAssistant = false;
        private Upperbay.Assistant.EtherWriteAccessor _etherWriteAccessorAssistant = null;

        private bool _hasSimulatorAssistant = false;
        private Upperbay.Assistant.Simulator _simulatorAssistant = null;

        private bool _hasMqttPublisherAssistant = false;
        private Upperbay.Assistant.MqttPublisher _mqttPublisherAssistant = null;

        private bool _hasSmartthingsPublisherAssistant = false;
        private Upperbay.Assistant.SmartthingsPublisher _smartthingsPublisherAssistant = null;

        private bool _hasMqttSubscriberAssistant = false;
        private Upperbay.Assistant.MqttSubscriber _mqttSubscriberAssistant = null;

        private bool _hasCloudMqttPublisherAssistant = false;
        private Upperbay.Assistant.CloudMqttPublisher _cloudMqttPublisherAssistant = null;

        private bool _hasCloudMqttSecureSubscriberAssistant = false;
        private Upperbay.Assistant.CloudMqttSecureSubscriber _cloudMqttSecureSubscriberAssistant = null;

        private bool _hasThingSpeakReadAccessorAssistant = false;
        private Upperbay.Assistant.ThingSpeakReadAccessor _thingSpeakReadAccessorAssistant = null;

        private bool _hasThingSpeakWriteAccessorAssistant = false;
        private Upperbay.Assistant.ThingSpeakWriteAccessor _thingSpeakWriteAccessorAssistant = null;
     
        private bool _hasWeatherServiceAssistant = false;
        private Upperbay.Assistant.WeatherService _weatherServiceAssistant = null;

        private bool _hasManualInputAssistant = false;
        private Upperbay.Assistant.ManualInput _manualInputAssistant = null;

        private bool _hasCacheAssistant = false;
        private Upperbay.Assistant.Cache _cacheAssistant = null;

        private Upperbay.Assistant.VariableProcessor _variableProcessorAssistant = null;
        
        // Cycle Period in milliseconds
        private int _period = 10000;

        // Flag for controlling the running state
        private bool _isAlive = false;

        // ??
        private bool _enableCommunity = false;
        private bool _enableCluster = false;

        private IHostColonyServices _hostColony;

        private AgentPassPort _agentPassPort = null;

        private static string AgentName = null;
        private string _agentName = null; // Shorthand

        private DateTime _timeLastExecuted = DateTime.MinValue;

        private TimeTrigger _hourTimeTrigger = new TimeTrigger();
        private TimeTrigger _5minTimeTrigger = new TimeTrigger();
        private TimeTrigger _10minTimeTrigger = new TimeTrigger();
        private TimeTrigger _15minTimeTrigger = new TimeTrigger();
        private TimeTrigger _20minTimeTrigger = new TimeTrigger();
        private TimeTrigger _30minTimeTrigger = new TimeTrigger();
        private TimeTrigger _45minTimeTrigger = new TimeTrigger();
        private TimeTrigger _60minTimeTrigger = new TimeTrigger();
        private TimeTrigger _240minTimeTrigger = new TimeTrigger();

        private int _lastThirtyMinutes = DateTime.Now.Minute;
        private int _lastTenMinutes = DateTime.Now.Minute;
        private int _lastFiveMinutes = DateTime.Now.Minute;
        private int _lastTwoMinutes = DateTime.Now.Minute;
        private int _lastMinute = DateTime.Now.Minute;

        #endregion
    
    }
    
}
