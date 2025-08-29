
using System;
using System.Configuration;
using System.Text;

using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;



namespace Upperbay.Agent.Library.Configurator
{
	/// <summary>
	/// The class that encapsulates the definition of a Service.
	/// </summary>
    public class ColonySettings : ConfigurationSection
	{

        public ColonySettings()
		{          
		}

        [ConfigurationProperty("colonyName")]
        public string ColonyName
        {
            get { return (string)this["colonyName"]; }
            set { this["colonyName"] = value; }
        }
        
        [ConfigurationProperty("account")]
        public string Account
        {
            get { return (string)this["account"]; }
            set { this["account"] = value; }
        }

        [ConfigurationProperty("username")]
        public string Username
        {
            get { return (string)this["username"]; }
            set { this["username"] = value; }
        }

        [ConfigurationProperty("password")]
        public string Password
        {
            get { return (string)this["password"]; }
            set { this["password"] = value; }
        }

        [ConfigurationProperty("priority")]
        public string Priority
        {
            get { return (string)this["priority"]; }
            set { this["priority"] = value; }
        }

        [ConfigurationProperty("version")]
        public string Version
        {
            get { return (string)this["version"]; }
            set { this["version"] = value; }
        }

		public override string ToString() 
		{
			StringBuilder sb = new StringBuilder();
            sb.AppendFormat("ColonyName = {0}",
                this.ColonyName);
			return sb.ToString();
		}
	}
}
