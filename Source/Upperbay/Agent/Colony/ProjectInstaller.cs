//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes: Colonies no longer work!!!! One Service and One Agent in config file.
//  See edit below to name Service based on installation directory
//==================================================================
using System;
using System.Configuration;
using System.ComponentModel;
using System.IO;

using Upperbay.Core.Logging;
using Upperbay.Agent.ConfigurationSettings;


namespace Upperbay.Agent.Colony
{
    /// <summary>
    /// Summary description for ProjectInstaller.
    /// </summary>
    [RunInstaller(true)]
	public class ProjectInstaller : System.Configuration.Install.Installer
	{
        //This overides using service name in app.config
        private bool bUseDirectoryNameForService = true;

        private System.ServiceProcess.ServiceInstaller[] serviceInstallers = null;
        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller = null;

        private ConfigurationHelper configurationHelper = null;
        private int servicesCount = 0;


		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ProjectInstaller()
		{
			// This call is required by the Designer.
            Log2.Trace("ProjectInstaller: Construction");
            InitializeComponent();
            Init();

			// TODO: Add any initialization after the InitializeComponent call
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}


        private void Init()
        {
            try
            {
                // Build ServiceInstaller
                Log2.Trace("ProjectInstaller: Building ServiceInstaller");

                // Get Count of Services
                configurationHelper = new ConfigurationHelper();
                Log2.Trace("ProjectInstaller: Built ServicesConfigurator");

                Log2.Trace("ProjectInstaller: Getting Count");
                servicesCount = configurationHelper.GetServicesCount();
                Log2.Trace("ProjectInstaller: ServicesCount = {0}", servicesCount);

                serviceInstallers = new System.ServiceProcess.ServiceInstaller[servicesCount];
                
                //Get app.config configuration									 
                Log2.Trace("ProjectInstaller: Extracting Configuration");
                Configuration config = ConfigurationManager.OpenExeConfiguration("Upperbay.Agent.Colony.exe");
                ServicesSettings section = (ServicesSettings)config.GetSection("services");

                ColonySettings colony = (ColonySettings)config.GetSection("colony");
                Log2.Trace("ProjectInstaller: Colony Configuration: {0}, {1}, {2}", 
                    colony.ColonyName, 
                    colony.Account, 
                    colony.Username, 
                    colony.Password);

                Log2.Trace("ProjectInstaller: Service in Services:");
                
                int i = 0;
                foreach (ServiceElement service in section.Services)
                {

                    Log2.Debug("ProjectInstaller: Service {0}: ServiceName = {1}", i, service.ServiceName);
                    Log2.Debug("ProjectInstaller: Service {0}: DisplayName = {1}", i, service.DisplayName);
                    Log2.Debug("ProjectInstaller: Service {0}: Description = {1}", i, service.Description);
                    Log2.Debug("ProjectInstaller: Service {0}: StartType = {1}", i, service.StartType);

                    Log2.Trace("ProjectInstaller: Service {0}: ServiceCategory = {1}", i, service.ServiceCategory);
                    Log2.Trace("ProjectInstaller: Service {0}: Type = {1}", i, service.Type);
//                    Log2.Trace("ProjectInstaller: Service {0}: Username = {1}", i, service.Username);
//                    Log2.Trace("ProjectInstaller: Service {0}: Password = {1}", i, service.Password);
                    
                    this.serviceInstallers[i] = new System.ServiceProcess.ServiceInstaller();
                    // 
                    // serviceInstaller1

                    if (bUseDirectoryNameForService)
                    {
                        // Service Name from directory, not from config file!
                        // Needed for cloning Current For Carbon game agents
                        string currentDir = Directory.GetCurrentDirectory();
                        Log2.Debug("Current Dir Name = " + currentDir);
                        DirectoryInfo dir = new DirectoryInfo(currentDir);
                        string dirName = dir.Name;
                        Log2.Debug("Service Dir Name = " + dirName);

                        this.serviceInstallers[i].ServiceName = dirName;
                        this.serviceInstallers[i].DisplayName = dirName;
                        this.serviceInstallers[i].Description = service.Description;
                    }
                    else
                    {
                        // Service Nmae from config file
                        //this.serviceInstallers[i].ServiceName = colony.ColonyName + "." + service.ServiceName;
                        //this.serviceInstallers[i].DisplayName = colony.ColonyName + "." + service.DisplayName;
                        this.serviceInstallers[i].ServiceName = service.ServiceName;
                        this.serviceInstallers[i].DisplayName = service.DisplayName;
                        this.serviceInstallers[i].Description = service.Description;
                    }
                    // this.serviceInstallers[i].ServicesDependedOn = service.Description;

                    if (service.StartType == "manual")
                        this.serviceInstallers[i].StartType =
                            System.ServiceProcess.ServiceStartMode.Manual;
                    else if (service.StartType == "auto")
                        this.serviceInstallers[i].StartType =
                            System.ServiceProcess.ServiceStartMode.Automatic;
                    else
                        this.serviceInstallers[i].StartType =
                            System.ServiceProcess.ServiceStartMode.Automatic;

                    // 
                    // ProjectInstaller
                    // 
                    this.Installers.Add(this.serviceInstallers[i]);
                    i++;
                }


                // Build and Add serviceProcessInstaller1
                Log2.Trace("ProjectInstaller: Building ProcessInstaller");
                this.serviceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();

                if (colony.Account == "User")
                {
                    this.serviceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.User;
                    this.serviceProcessInstaller.Username = colony.Username;
                    this.serviceProcessInstaller.Password = colony.Password;
                    Log2.Trace("ProjectInstaller: Running under User");
                }
                else
                {
                    this.serviceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
                    this.serviceProcessInstaller.Username = "";
                    this.serviceProcessInstaller.Password = "";
                    Log2.Trace("ProjectInstaller: Running under LocalSystem");
                }

                Log2.Trace("ProjectInstaller: Adding ServiceProcessInstaller");
                this.Installers.Add(this.serviceProcessInstaller);

            }// end try
            catch (Exception Ex)
            {
                Log2.Trace("ProjectInstaller Exception: {0}", Ex.ToString());
                throw;
            }

        }



		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion
	

	}
}
