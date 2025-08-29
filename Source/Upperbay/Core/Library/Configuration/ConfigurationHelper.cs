

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Runtime.InteropServices;

using Upperbay.Agent.Logging;




namespace Upperbay.Agent.Library.Configurator
{

    public class ConfigurationHelper
    {

        /// <summary>
        /// 
        /// </summary>
        private Hashtable processNamesHashTable = new Hashtable();
        private int processCount = 0;

        /// <summary>
        /// 
        /// </summary>
        public ConfigurationHelper()
        {
        }
       


        /// <summary>
        /// 
        /// </summary>
        public int ProcessCount
        {
            get { return processCount;}
        }



        /// <summary>
        /// 
        /// </summary>
        public Hashtable ProcessNames
        {
            get { return processNamesHashTable;}
        }


        public void WriteConfig()
        {
            ServicesSettings servicesSection = new ServicesSettings();

            ServicesCollection services = new ServicesCollection();

            ServiceElement service = new ServiceElement();
            ServiceElement service2 = new ServiceElement();

            service.ServiceName = "Service3";
            service.DisplayName = "Display1";
            service.Description = "Desc1";
            service.ServiceCategory = "Prissy1";
            service.Type = "Classy1";
            service.StartType = "Man";

            services.Add(service);

            service2.ServiceName = "Service4";
            service2.DisplayName = "Display2";
            service2.Description = "Desc2";
            service2.ServiceCategory = "Prissy1";
            service2.Type = "Classy2";
            service2.StartType = "Man2";

            services.Add(service2);

            servicesSection.Services = services;

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.Sections.Add("services", servicesSection);
            config.Save();

            Console.WriteLine("Saved File\n");
        }



        public void ReadConfig()
        {
            Configuration config1 = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ServicesSettings section = (ServicesSettings)config1.GetSection("services");

            Console.WriteLine("Service in Services:\n");

            foreach (ServiceElement service1 in section.Services)
            {
                Console.WriteLine(service1.ServiceName);
                Console.WriteLine(service1.DisplayName);
                Console.WriteLine(service1.ServiceCategory);
                Console.WriteLine(service1.Type);
                Console.WriteLine(service1.StartType);
                Console.WriteLine(service1.Description + "\n\n\n");
            }
        }



        public int GetServicesCount()
        {
            try
            {
                if (System.IO.File.Exists("Upperbay.Agent.Colony.exe.config"))
                {
                    //Log.BootLog("ConfigurationHelper.GetServicesCount: File Exists");
                }
                else
                    Log.BootLog("ConfigurationHelper.GetServicesCount: File Does Not Exist");

                Configuration config = ConfigurationManager.OpenExeConfiguration("Upperbay.Agent.Colony.exe");
                Log.BootLog("ConfigurationHelper.GetServicesCount Filepath: {0}", config.FilePath);

                ServicesSettings section = (ServicesSettings)config.GetSection("services");
                if (section == null)
                    Log.BootLog("ConfigurationHelper.GetServicesCount: Did NOT find Sections");
                else
                {
                    //Log.BootLog("ConfigurationHelper.GetServicesCount: Got Sections");
                }

                int i = 0;

                foreach (ServiceElement service in section.Services)
                {
                    //Log.BootLog("ConfigurationHelper.GetServicesCount: Found A Section");
                    i++;
                }
                return i;
            }
            catch //(Exception Ex)
            {
                Log.BootLog("ConfigurationHelper.GetServicesCount: Exception Counting the number of services.");
                return 0;
            }
        }




        public IDictionary ParseTypeString(string typeString)
        {
             //type="Upperbay.Agent.Cell.BaseCell, Upperbay.Agent.Cell.dll, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
            try
            {
                IDictionary props = new Hashtable();

                string delimStr = ",";
                char[] delimiter = delimStr.ToCharArray();
                string[] fields = typeString.Split(delimiter);

                if (fields.Length <= 1)
                {
                    // Insufficient Data
                    throw new Exception ("Type fields are missing.");
                }

                props["type"] = String.Empty;
                props["assembly"] = String.Empty;
                props["version"] = String.Empty;
                props["culture"] = String.Empty;
                props["publickeytoken"] = String.Empty;

                if (fields.Length > 1)
                {
                    props["type"] = fields[0].Trim();
                    props["assembly"] = fields[1].Trim();
                }

                foreach (string nvp in fields)
                {
                    int i;
                    if (nvp.Contains("Version="))
                    {
                        i = nvp.IndexOf("Version=");
                        props["version"]=nvp.Substring(i + 8).Trim();
                    }
                    else if (nvp.Contains("Culture="))
                    {
                        i = nvp.IndexOf("Culture=");
                        props["culture"] = nvp.Substring(i + 8).Trim();
                    }
                    else if (nvp.Contains("PublicKeyToken="))
                    {
                        i = nvp.IndexOf("PublicKeyToken=");
                        props["publickeytoken"] = nvp.Substring(i + 15).Trim();
                    }
                }

                //Log.BootLog("ConfigurationHelper.ParseTypeString: Type={0}, Ass={1}, Version={2}, Culture={3}, PublicKeyToken={4}",
                //    props["type"], props["assembly"], props["version"], props["culture"], props["publickeytoken"]);

                return props;

            }
            catch //(Exception Ex)
            {
                Log.BootLog("ConfigurationHelper.ParseTypeString: Exception Sections");
                return null;
            }
        }


    }

}

