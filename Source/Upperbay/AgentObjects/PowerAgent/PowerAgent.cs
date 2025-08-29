//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading;
using Upperbay.Agent.ColonyMatrix;
using Upperbay.Agent.Interfaces;
using Upperbay.Assistant;
// Assemblies needed for Agentness
using Upperbay.Core.Library;
using Upperbay.Core.Logging;
using Upperbay.Worker.EtherAccess;
using Upperbay.Worker.JSON;
using Upperbay.Worker.PostOffice;
using Upperbay.Worker.SmartThings;
using Upperbay.Worker.Timers;
using Upperbay.Worker.Voice;
using static System.Net.WebRequestMethods;
using System.Diagnostics;


namespace Upperbay.AgentObject
{
    /// <summary>
    /// Summary description for DataAgent.
    /// </summary>
    ///     

    //    [InstrumentationClass(InstrumentationType.Instance)]
    //    [Serializable]
    public class PowerAgent : INativeAgent
    {
        public PowerAgent()
        {
        }

        #region Agent Properties

        private DataVariable _agentValue = new DataVariable("AgentValue", "AgentValue", "A Test Variable", "Feet");
        [Ua("publish"), Ua("simulated")]
        public DataVariable AgentValue { get { return this._agentValue; } set { this._agentValue = value; } }

        private bool _bRainManVirgin = true; // run on startup
        private RunOnceHelper _runHelper = new RunOnceHelper();

        private CancellationToken onStopCancellationToken;

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
            //InitializeAssistants();

            //````````````````````````````````````````````````````````````````````````````````
            // Read DLL App.config file settings


            string period = MyAppConfig.GetParameter("Period");

            if (period != null) _period = Int32.Parse(period);
            if (period != null) Log2.Debug("Agent Config Data: {0} period = {1}", _agentName, period);

            //string location = MyAppConfig.GetParameter("Location");
            //if (location != null)
            //{
            //    this.Location.Value = location;
            //    this.Location.Quality = "Good";
            //    this.Location.UpdateTime = DateTime.Now;
            //    Log2.Trace("Agent Config Data: {0} Location = {1}", _agentName, location);
            //}

            string clusterName = MyAppConfig.GetParameter("ClusterName");
            string clusterVersion = MyAppConfig.GetParameter("ClusterVersion");
            if (clusterName != null)
            {
                Log2.Info("ClusterName: {0}, ClusterVersion: {1}", clusterName, clusterVersion);
            }

           
            string powerManEnabled = MyAppConfig.GetParameter("PowerManEnabled");
            if (powerManEnabled == "true")
            {
                Log2.Info("Service Running in PowerMan Mode");
            }
            //string aiGameEnable = MyAppConfig.GetParameter("AIGameEnable");
            //if (aiGameEnable == "true")
            //{
            //    Log2.Info("Service Running in AI GAME ENABLE!");
            //}

            //_variableProcessorAssistant = new VariableProcessor();
            //_variableProcessorAssistant.Initialize(this._agentPassPort.ClassName, _agentName, this);

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
        public void OnStart(CancellationToken token)
        {
            onStopCancellationToken = token;
            Log2.Info("onStopCancellationToken Received");

            //Set Current Thread to agent name for logging
            Thread thread = Thread.CurrentThread;
            string s = this._agentPassPort.ServiceName + "." + this._agentPassPort.AgentName;
            thread.Name = s;


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
           
            while (this._isAlive)
            {
                
                try
                {
                    if (onStopCancellationToken.IsCancellationRequested)
                    {
                        this._isAlive = false;
                        Log2.Info("onStopCancellationToken.IsCancellationRequested");
                    }
                        

                    //ConfigurationManager.RefreshSection("appSettings");
                    //ConfigurationManager.GetSection("appSettings");

                    //AppConfig appConfig = new AppConfig(_agentName);
                    //string traceMode = appConfig.GetAppConfigParameter("TraceMode");
                    string traceMode = MyAppConfig.GetParameter("TraceMode");

                    Log2.SetLoggingLevel(traceMode);

                    //Log2.Debug("{0}: Agent OnFire", _agentName);


                    ////////////////////////////
                    //SmartThings RainMan Scan
                    try
                    {
                        string powerManEnabled = MyAppConfig.GetParameter("PowerManEnabled");
                        string powerManPeriodSecs = MyAppConfig.GetParameter("PowerManPeriodSecs");
                        double secs;
                        Double.TryParse(powerManPeriodSecs, out secs);
                        if (powerManEnabled == "true")
                        {
                            var now = DateTime.Now;


                            //// Every 5 minutes, ±10s tolerance
                            //if (_runHelper.ShouldRunOnceAtBoundary("every-5-min", now, TimeSpan.FromMinutes(5), 10)
                            //    || (_bRainManVirgin))

                            // Every 6 hours, ±10s tolerance
                            if (_runHelper.ShouldRunOnceAtBoundary(powerManPeriodSecs, now, TimeSpan.FromSeconds(secs), 10)
                            || (_bRainManVirgin))
                            {
                                if (true)
                                {
                                    Log2.Debug("START SCANNING SMARTTHINGS");
                                    _bRainManVirgin = false;
                                    //var scanner = new SmartThingsAccess();
                                    var st = new Upperbay.Worker.SmartThings.SmartThingsAccess();
                                    // Require switch state and temperature (from main component)
                                    st.InitCli(
                                        cliPath: "C:\\Program Files (x86)\\SmartThings\\smartthings.exe", // or full path to smartthings.exe
                                        requiredAttributes: new[] {
                                            "main.powerMeter.power"
                                        },
                                        locationId: null,        // auto-discover first location
                                        hubId: null,
                                        httpTimeoutSeconds: 120,
                                        profile: "default"       // optional CLI profile name
                                    );

                                   
                                    var sw = Stopwatch.StartNew();
                                    // Find devices with XT or IT in their name/label
                                    var devices = st.Fetch("XT", "XX").GetAwaiter().GetResult();
                                    sw.Stop();
                                    var duration = sw.ElapsedMilliseconds;
                                    Log2.Debug($"END SCANNING SMARTTHINGS: {duration} ms.)");

                                    int deviceCount = devices.Count;
                                    Log2.Debug($"Devices: {deviceCount}");

                                    // Ensure logs directory exists
                                    string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
                                    Directory.CreateDirectory(logDir);

                                    // Use a date-based CSV log file (one per day)
                                    //string csvPath = Path.Combine(logDir, $"RainMan_Devices_{DateTime.Now:yyyy-MM-dd}.csv");
                                    string csvPath = Path.Combine(logDir, $"PowerMan_Readings.csv");

                                    // Add header if file is new
                                    if (!System.IO.File.Exists(csvPath))
                                    {
                                        System.IO.File.AppendAllText(csvPath, "Timestamp,DeviceName,Power (Watts)" + Environment.NewLine);
                                    }

                                    if (deviceCount == 1)
                                    {
                                        foreach (var d in devices)
                                        {
                                            Log2.Debug($"{d.Label ?? d.Name} [{d.DeviceId}]");

                                            // Safely get temperature & humidity
                                            string power = d.Attributes.TryGetValue("main.powerMeter.power", out var t) ? t?.ToString() ?? "na" : "na";

                                            // Debug log
                                            Log2.Debug($"  power = {power}");

                                            // Build CSV-safe line
                                            string csvLine = string.Format("\"{0}\",\"{1}\",\"{2}\"",
                                                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                                (d.Label ?? d.Name).Replace("\"", "\"\""),
                                                power.Replace("\"", "\"\""));

                                            System.IO.File.AppendAllText(csvPath, csvLine + Environment.NewLine);

                                            string powerManThingspeakEnable = MyAppConfig.GetParameter("PowerManThingspeakEnable");
                                            if (powerManThingspeakEnable == "true")
                                            {

                                                try
                                                {
                                                    string powerManThingspeakField = MyAppConfig.GetParameter("PowerManThingspeakField");
                                                    string powerManThingspeakKey = MyAppConfig.GetParameter("PowerManThingspeakKey");
                                                    string value = power.Replace("\"", "\"\"");

                                                    var form = new FormUrlEncodedContent(new Dictionary<string, string>
                                                    {
                                                        ["api_key"] = powerManThingspeakKey,
                                                        ["field1"] = value.ToString()
                                                    });

                                                    var http = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
                                                    var resp = http.PostAsync($"https://api.thingspeak.com/update", form)
                                                                .ConfigureAwait(false)
                                                                .GetAwaiter()
                                                                .GetResult();
                                                    string body = resp.Content.ReadAsStringAsync()
                                                                              .ConfigureAwait(false)
                                                                              .GetAwaiter()
                                                                              .GetResult();   // body = entry_id (e.g., "12345") or "0" on failure
                                                    if (!resp.IsSuccessStatusCode || body == "0")
                                                        throw new HttpRequestException($"ThingSpeak update failed (HTTP {(int)resp.StatusCode}): {body}");


                                                }
                                                catch (HttpRequestException ex)
                                                {
                                                    Log2.Error($"ERROR: {ex}");
                                                }
                                                catch (Exception ex)
                                                {
                                                    Log2.Error($"ERROR: {ex}");
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Log2.Error($"ERROR: ONLY ONE POWER METER!");

                                    }
                                }

                            }
                  
                        }
                    }
                    catch (Exception ex)
                    {
                        Log2.Error("Exception: " + ex.ToString());
                    }

                    _timeLastExecuted = DateTime.Now;

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
            //string fileName = MyAppConfig.GetParameter("CacheDataFile");

            //string filePath = "logs\\" + fileName;
            ////string path = Directory.GetCurrentDirectory();
            //string newpath = path + "\\" + "output\\DataVariableCacheDump.txt";

            //DataVariableCache.Cache2File(filePath);

            //try
            //{
            //    string vocalName = MyAppConfig.GetParameter("VocalName");
            //    string shutdownMessage = MyAppConfig.GetParameter("ShutdownMessage");
            //    string voice = MyAppConfig.GetParameter("Voice");

            //    Log2.Trace("Voice = {0}", voice);
            //    // Voice.SetVoice(voice);
            //    Voice.Speak("This is " + vocalName + ". " + shutdownMessage);

            //}
            //catch (Exception ex)
            //{
            //    Log2.Error("Voice Synth Died: ", ex);
            //}

            //Log2.Trace("Upperbay Agent {0} is Sleeping!", _agentName);

            //if (_hasEventAssistant) _eventAssistant.Stop();
            //if (_hasGameEventAssistant) _gameEventAssistant.Stop();
            //if (_hasManualInputAssistant) _manualInputAssistant.Stop();
            //if (_hasEtherReadAccessorAssistant) _etherReadAccessorAssistant.Stop();
            //if (_hasEtherWriteAccessorAssistant) _etherWriteAccessorAssistant.Stop();
            //if (_hasSimulatorAssistant) _simulatorAssistant.Stop();
            //if (_hasSmartthingsPublisherAssistant) _smartthingsPublisherAssistant.Stop();
            //if (_hasMqttSubscriberAssistant) _mqttSubscriberAssistant.Stop();
            //if (_hasMqttPublisherAssistant) _mqttPublisherAssistant.Stop();
            //if (_hasCloudMqttPublisherAssistant) _cloudMqttPublisherAssistant.Stop();
            //if (_hasCloudMqttSecureSubscriberAssistant) _cloudMqttSecureSubscriberAssistant.Stop();
            //if (_hasThingSpeakWriteAccessorAssistant) _thingSpeakWriteAccessorAssistant.Stop();
            //if (_hasThingSpeakReadAccessorAssistant) _thingSpeakReadAccessorAssistant.Stop();
            //if (_hasWeatherServiceAssistant) _weatherServiceAssistant.Stop();
            //if (_hasCacheAssistant) _cacheAssistant.Stop();

            //_variableProcessorAssistant.Stop();

            //MessageMailer messageMailer = new MessageMailer();
            //string smsMessageBody = "Agent is OFFLINE";
            //messageMailer.SendSMSText(smsMessageBody);


            Log2.Debug("{0}: Agent OnStop", _agentName);

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

            PowerAgent.AgentName = this._agentPassPort.AgentName;
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
                Log2.Info("Agent Log File: {0}", config.AppSettings.File);
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


    public class RunOnceHelper
    {
        private readonly object _gate = new object();
        private readonly Dictionary<string, DateTime> _lastBucketRun = new Dictionary<string, DateTime>();
        private readonly HashSet<string> _virginSchedules = new HashSet<string>();

        public RunOnceHelper()
        {
        }

        /// <summary>
        /// Returns true exactly once at the start of each cadence bucket (e.g., every 5 minutes,
        /// or every 6 hours), allowing a ±toleranceSeconds window around the boundary.
        /// - scheduleName: unique name for this schedule (e.g., "every-5-min", "six-hour").
        /// - now: current time (use DateTime.Now or DateTime.UtcNow consistently).
        /// - cadence: e.g., TimeSpan.FromMinutes(5) or TimeSpan.FromHours(6).
        /// - toleranceSeconds: window around the boundary (e.g., 10).
        /// - fireVirginImmediately: if true, fires once immediately on first call for each scheduleName.
        /// </summary>
        public bool ShouldRunOnceAtBoundary(
            string scheduleName,
            DateTime now,
            TimeSpan cadence,
            int toleranceSeconds,
            bool fireVirginImmediately = true)
        {
            if (string.IsNullOrEmpty(scheduleName))
                throw new ArgumentException("Schedule name is required.", "scheduleName");

            if (cadence <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException("cadence", "Cadence must be positive.");

            // Snap 'now' down to the start of its cadence bucket: floor(now, cadence)
            long ticksPerBucket = cadence.Ticks;
            long flooredTicks = (now.Ticks / ticksPerBucket) * ticksPerBucket;
            var bucketStart = new DateTime(flooredTicks, now.Kind);

            // Only fire near the bucket boundary
            double secondsFromBoundary = Math.Abs((now - bucketStart).TotalSeconds);
            bool withinTolerance = secondsFromBoundary <= toleranceSeconds;

            if (!withinTolerance)
                return false;

            lock (_gate)
            {
                // First run for this schedule?
                if (fireVirginImmediately && !_virginSchedules.Contains(scheduleName))
                {
                    _virginSchedules.Add(scheduleName);
                    _lastBucketRun[scheduleName] = bucketStart;
                    return true;
                }

                DateTime last;
                if (_lastBucketRun.TryGetValue(scheduleName, out last))
                {
                    if (last == bucketStart)
                        return false; // Already fired for this bucket
                }

                _lastBucketRun[scheduleName] = bucketStart;
                return true;
            }
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
