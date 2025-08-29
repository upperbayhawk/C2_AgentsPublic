using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


namespace Upperbay.Core.Library
{
    /// <summary>
    /// 
    /// </summary>
    public class AppConfigXXX
    {

        public AppConfigXXX(string agentName)
        {
            //// Specify agent config settings at runtime.
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.File = "config\\" + agentName + ".app.config";
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            ConfigurationManager.GetSection("appSettings");
        }

        /// <summary>
        /// AppConfig overrides for hardcoding app.config file parameters.
        /// Defaults to App.Config file
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public string GetAppConfigParameter(string parameter)
        {
            string parmValue;

            switch (parameter)
            {
                case "hello":
                    parmValue = "helloyou";
                    break;

                default:
                    // from app.config file
                    ConfigurationManager.GetSection("appSettings");
                    parmValue = ConfigurationManager.AppSettings[parameter];
                    break;
            }

            return parmValue;
        }
    }
}
