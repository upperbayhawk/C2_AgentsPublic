//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
using System.Configuration;
using System.Text;



namespace Upperbay.Agent.ConfigurationSettings
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

        [ConfigurationProperty("celestialName")]
        public string CelestialName
        {
            get { return (string)this["celestialName"]; }
            set { this["celestialName"] = value; }
        }

        [ConfigurationProperty("collectiveName")]
        public string CollectiveName
        {
            get { return (string)this["collectiveName"]; }
            set { this["collectiveName"] = value; }
        }

        [ConfigurationProperty("communityName")]
        public string CommunityName
        {
            get { return (string)this["communityName"]; }
            set { this["communityName"] = value; }
        }

        [ConfigurationProperty("clusterName")]
        public string ClusterName
        {
            get { return (string)this["clusterName"]; }
            set { this["clusterName"] = value; }
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
