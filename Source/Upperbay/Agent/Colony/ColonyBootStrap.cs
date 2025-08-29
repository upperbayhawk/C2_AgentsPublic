//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
using System;
using System.Collections;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using System.Runtime.Remoting;

using Upperbay.Core.Logging;
using Upperbay.Core.Library;
using Upperbay.Agent.ConfigurationSettings;
using Upperbay.Agent.Interfaces;



namespace Upperbay.Agent.Colony
{


    public class ClaraService : System.ServiceProcess.ServiceBase
    {


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Private Member Variables

        private System.ServiceProcess.ServiceController serviceController1;

        // for logging & debugging
        //private static TraceSwitch colonySwitch = Upperbay.Core.Logging.Log2.AgentSwitch;
        //private static TraceSwitch colonySwitch = new TraceSwitch("Runtime", "Runtime", "Runtime");
        //private static TraceSwitch colonySwitch = new TraceSwitch("Debug", "Debug", "Debug");
        private static TraceSwitch colonySwitch = new TraceSwitch("Trace", "Trace", "Trace");

        //private AppDomain appDomain; 
        private ICell server;
        //TODO: Remove

        private Configuration config = null;
        private ServicesSettings servicesSettings = null;
        private ColonySettings colonySettings = null;
        //private AgentsSettings agentSettings = null;
        //private ToolboxSettings toolboxSettings = null;

        
        private ObjectHandle handle = null;

        private static int serviceCount;


        // State Variables for bootstrapping a Windows Service
        private string colonyName;
        private string fullName;
        private string serviceName;
        private string description;
        private string displayName;
        private string serviceCategory;
        private string startType;
        
        private string serviceType;

        private string serviceTypename;
        private string serviceAssemblyName;
        private string serviceVersion;
        private string serviceCulture;
        private string servicePublicKeyToken;

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        #endregion
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public ClaraService()
        {
            // This call is required by the Windows.Forms Component Designer.
            InitializeComponent();
            // TODO: Add any initialization after the InitComponent call
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Service Main

        // The main entry point for the process
        static void Main()
        {
            Type objType = typeof(ClaraService);

            System.ServiceProcess.ServiceBase[] ServicesToRun;

            // Log Assembly Information
            //Log2.Trace("ColonyBootStrap.Main: COMMENCING!");
            Log2.Trace("ColonyBootStrap.Main: Start ServiceBase");

            // Print the full assembly name
            Log2.Trace("ColonyBootStrap.Main: Full Assembly Name: " + objType.Assembly.FullName.ToString());
            Log2.Trace("ColonyBootStrap.Main: Qual Assembly Name: " + objType.Assembly.GetName().ToString());

//------------------------------------------------------------------------------------------------------

            // Set the current working directory. Services default to Windows\System.
            Process proc = System.Diagnostics.Process.GetCurrentProcess();
            string myName = proc.MainModule.FileName;
            Log2.Trace("ColonyBootStrap.Main: File Name: " + myName);

            string myMod = proc.MainModule.ModuleName;
            Log2.Trace("ColonyBootStrap.Main: Module Name: " + myMod);

            string myDir = myName.Replace(myMod,"");
            Log2.Trace("ColonyBootStrap.Main: Working Dir: " + myDir);
            Environment.CurrentDirectory = myDir;
            
//------------------------------------------------------------------------------------------------------
 

            // Get Count of Services
            ConfigurationHelper configurationHelper = new ConfigurationHelper();
            Log2.Trace("ColonyBootStrap.Main: Reading Service Count from Config");
            ClaraService.serviceCount = configurationHelper.GetServicesCount();
            Log2.Trace("ColonyBootStrap.Main: ServicesCount = " + ClaraService.serviceCount);

            ServicesToRun = new System.ServiceProcess.ServiceBase[ClaraService.serviceCount];

            Log2.Trace("ColonyBootStrap.Main: Opening ServicesSection");

            //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            Configuration config = ConfigurationManager.OpenExeConfiguration("Upperbay.Agent.Colony.exe");
            Log2.Trace("ColonyBootStrap.Main: Opened Config");

            ServicesSettings section = (ServicesSettings)config.GetSection("services");
            if (section == null)
                Log2.Error("ColonyBootStrap.Main: ServicesSection NOT FOUND!!!!");
            else
                Log2.Trace("ColonyBootStrap.Main: Found ServicesSection");

            ColonySettings colonySection = (ColonySettings)config.GetSection("colony");
            Log2.Trace("ColonyBootStrap.Main: " + colonySection.ColonyName + ", " + colonySection.Account + ", " + colonySection.Username + ", " + colonySection.Password);

            Log2.Trace("ColonyBootStrap.Main: " + config.FilePath);

            try
            {
                Log2.Trace("ColonyBootStrap.Main: Setting Process Priority: " + colonySection.Priority);
                ServiceEnvironment.SetPriority(colonySection.Priority);
            }
            catch
            {
                Log2.Error("ColonyBootStrap.Main: Exception, Failed to set process priority: " + colonySection.Priority);
            }


            try
            {
                Log2.Trace("ColonyBootStrap.Main: Service in Services Loop");

                int i = 0;
                foreach (ServiceElement service in section.Services)
                {
                    Log2.Trace("ColonyBootStrap.Main: ServiceName: {0}", service.ServiceName);
                    Log2.Trace("ColonyBootStrap.Main: DisplayName: {0}", service.DisplayName);
                    Log2.Trace("ColonyBootStrap.Main: ServiceCategory: {0}", service.ServiceCategory);
                    Log2.Trace("ColonyBootStrap.Main: Type: {0}", service.Type);
                    Log2.Trace("ColonyBootStrap.Main: StartType: {0}", service.StartType);

                    // Create a Colony bootstrap and set it's properties
                    ClaraService colony = new ClaraService();
                    colony.ColonyName = colonySection.ColonyName;
                    colony.ServiceName = colonySection.ColonyName + "." + service.ServiceName;
                    colony.FullServiceName = colonySection.ColonyName + "." + service.ServiceName;
                    colony.ShortServiceName = service.ServiceName;
                    colony.Description = service.Description;
                    colony.DisplayName = service.DisplayName;
                    colony.ServiceCategory = service.ServiceCategory;
                    colony.serviceType = service.Type;
                    colony.StartType = service.StartType;

                    IDictionary myType = configurationHelper.ParseTypeString(colony.serviceType);

                    colony.serviceTypename = (string)myType["type"];
                    colony.serviceAssemblyName = (string)myType["assembly"];
                    colony.serviceVersion = (string)myType["version"];
                    colony.serviceCulture = (string)myType["culture"];
                    colony.servicePublicKeyToken = (string)myType["publickeytoken"];

                    // Populate array of services from the app.config file
                    Log2.Trace("ColonyBootStrap.Main: Adding Service");
                    ServicesToRun[i] = (System.ServiceProcess.ServiceBase)colony;
                    Log2.Trace("ColonyBootStrap.Main: Added Service");
                    i++;
                }

                AgentsSettings agents = (AgentsSettings)config.GetSection("agents");
                foreach (AgentElement agent in agents.Agents)
                {
                    Log2.Trace("ColonyBootStrap.Main: AgentName: {0}", agent.AgentName);
                    Log2.Trace("ColonyBootStrap.Main: Description: {0}", agent.Description);
                }
            }
            catch (Exception ex)
            {
                Log2.Error("ERROR! ColonyBootStrap.Main: Exception: {0} " + ex.ToString());
            }

            System.ServiceProcess.ServiceBase.Run(ServicesToRun);

            Log2.Trace("ColonyBootStrap.Main: Done");

        }

        #endregion
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Accessor Properties

        // -----Accessor Properties-------

        public string ColonyName { get { return this.colonyName; } set { this.colonyName = value; } }
        public string FullServiceName { get { return this.fullName; } set { this.fullName = value; } }
        public string ShortServiceName { get { return this.serviceName; } set { this.serviceName = value; } }
        public string Description  { get { return this.description; } set { this.description = value; } }
        public string DisplayName  { get { return this.displayName; } set { this.displayName = value; }}
//        public string Password { get { return this.password; } set { this.password = value; } }
        public string ServiceCategory { get { return this.serviceCategory; } set { this.serviceCategory = value; } }
        public string ServiceType { get { return this.serviceType; } set { this.serviceType = value; } }
        public string StartType {  get { return this.startType; }  set { this.startType = value; } }
//        public string Username { get { return this.username; }  set { this.username = value; } }
        #endregion
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Private Housekeeping

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.serviceController1 = new System.ServiceProcess.ServiceController();

        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }



        #endregion
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Service OnStart

        protected override void OnStart(string[] args)
        {

            Log2.Trace("{0}: Colony Service is starting.", this.FullServiceName);

            Log2.Trace("{0}: ServiceName: {1}", this.FullServiceName, this.FullServiceName);
            Log2.Trace("{0}: displayName: {1}", this.FullServiceName, this.displayName);
//            Log2.TraceIf(colonySwitch, "{0}: password: {1}", this.FullServiceName, this.password);
            Log2.Trace("{0}: serviceCategory: {1}", this.FullServiceName, this.serviceCategory);
            Log2.Trace("{0}: serviceType: {1}", this.FullServiceName, this.serviceType);
            Log2.Trace("{0}: startType: {1}", this.FullServiceName, this.startType);
//            Log2.TraceIf(colonySwitch, "{0}: username: {1}", this.FullServiceName, this.username);
            Log2.Trace("{0}: description: {1}", this.FullServiceName, this.description);


            //--------------------------------------------------------------------------
            //////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Initialize
            //////////////////////////////////////////////////////////////////////////////////////////////////////////

            try
            {
                config = ConfigurationManager.OpenExeConfiguration("Upperbay.Agent.Colony.exe");
                servicesSettings = (ServicesSettings)config.GetSection("services");
                colonySettings = (ColonySettings)config.GetSection("colony");

                Log2.Trace("{0}: Colony Service Info: {1}, {2}, {3}, {4}",
                    this.FullServiceName,
                    colonySettings.ColonyName,
                    colonySettings.Account,
                    colonySettings.Username,
                    colonySettings.Password);

                this.colonyName = colonySettings.ColonyName;
            }
            catch (Exception Ex)
            {
                Log2.Error("ERROR! {0}: OnStart Colony Creation Failed: {1}", this.FullServiceName, Ex.ToString());
                throw new Exception("Colony Creation Failed");
            }


            //////////////////////////////////////////////////////////////////////////////////////////////////////////

            try
            {
                // If Standard Agent Then ...
                if (this.serviceCategory == "Agent")
                {

                    Log2.Trace("{0}: Creating Agent App Domain", this.FullServiceName);
                    
                    //appDomain = null;
                    //appDomain = AppDomain.CreateDomain(colonySettings.ColonyName);
 
                    //Log2.TraceIf(colonySwitch, "{0}: Base Dir = {1}", this.FullServiceName, appDomain.BaseDirectory);
                    //cellFactory = new CellFactory();
                    //server = (ICell)cellFactory.CreateCell(this.FullServiceName, this.serviceAssemblyName);
                    

                    //// Spinoff thread
                    ////Create CELL App Domain and instantiate class
                    //Log2.TraceIf(colonySwitch, "{0}: Activating Agent App Domain Class {1} from {2}", this.FullServiceName, this.serviceTypename, this.serviceAssemblyName);
                    //handle = Activator.CreateInstanceFrom(this.serviceAssemblyName, this.serviceTypename);
                    handle = Activator.CreateInstanceFrom(this.serviceAssemblyName, this.serviceTypename);
                    server = (ICell)handle.Unwrap();
                    //Log2.TraceIf(colonySwitch, "{0}: Agent App Domain Loaded: {1}", this.FullServiceName, cellFactory.LocalAppDomain.FriendlyName);


                    // Initialize CELL with Colony Host Services
                    try
                    {
                        Log2.Trace("{0}: Initializing Agent IHostColonyServices", this.FullServiceName);
                        ColonyServices hostServices = new Upperbay.Core.Library.ColonyServices();

                        //TODO
                        hostServices.CelestialName = colonySettings.CelestialName;
                        hostServices.CollectiveName = colonySettings.CollectiveName;
                        hostServices.CommunityName = colonySettings.CommunityName;
                        hostServices.ClusterName = colonySettings.ClusterName;
                        hostServices.CarrierName = System.Environment.MachineName;
                        hostServices.ColonyName = this.ColonyName;
                        hostServices.ServiceName = this.ShortServiceName;
                        hostServices.ClassName = this.serviceTypename;
                        //TODO
                        hostServices.LogicalName = this.serviceTypename;
                        //TODO
                        hostServices.Version = "1.0.0.0";

                        hostServices.Description = this.description;
                        //hostServices.AssemblyDirectory = appDomain.BaseDirectory;
                        hostServices.AssemblyDirectory = AppDomain.CurrentDomain.BaseDirectory;
                        // TODO: Fix
                        hostServices.AssemblyName = this.serviceAssemblyName;
                        hostServices.AssemblyPath = this.serviceAssemblyName;

                        hostServices.Initialize();
                        
                        ICell colonyInterface = (ICell)server;
                       // colonyInterface.OnCellInitialize((IHostColonyServices)hostServices);
                        server.OnCellInitialize((IHostColonyServices)hostServices);
                                            
                        
                    }
                    catch (Exception Ex)
                    {
                        Log2.Error("ERROR! {0}: Initializing Agent ColonyHostServices FAILED: {1}", this.FullServiceName, Ex.ToString());
                    }


                    Log2.Trace("{0}: Initializing Agent Thread", this.FullServiceName);
                    //Spawn Thread for Server
                    Thread thread = new Thread(new ThreadStart(server.OnCellStart));
                    // Start claraServer.OnStart.  On a uniprocessor, the thread does not get 
                    // any processor time until the main thread yields.
                    thread.Start();
                    
                    Log2.Trace("Agent Thread Started", this.FullServiceName);
                }
            }
            catch (Exception Ex)
            {
                Log2.Error("ERROR! {0}: OnStart Colony Creation Failed: {1}", this.FullServiceName, Ex.ToString());
                throw new Exception("Colony Creation Failed");
            }
                
        }

        #endregion
        //////////////////////////////////////////////////////////////////////////////////////////////////////////

        //////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Service OnStop
        //////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// </summary>
        protected override void OnStop()
        {
            if (this.serviceCategory == "Agent")
            {
                // Kill Thread
                server.OnCellStop();// was server
                Log2.Trace("{0}: Colony Service is stopping.", this.FullServiceName);
            }

        }
        #endregion
        //////////////////////////////////////////////////////////////////////////////////////////////////////////

        //////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Service OnShutdown
        //////////////////////////////////////////////////////////////////////////////////////////////////////////

        protected override void OnShutdown()
        {
            // Kill Thread
            server.OnCellStop();

            Log2.Trace("Colony Service {0} is shutting down.", this.FullServiceName);

        }
        #endregion
        //////////////////////////////////////////////////////////////////////////////////////////////////////////

    }
    
}
