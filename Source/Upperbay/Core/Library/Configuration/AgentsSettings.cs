using System;
using System.Configuration;
using System.ComponentModel;
using System.Globalization;

namespace Upperbay.Agent.Library.Configurator
{


    public sealed class AgentsSettings : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public AgentsCollection Agents
        {
            get
            {
                return (AgentsCollection)base[""];
            }
            set
            {
                base[""] = value;
            }
        }
    }

    public sealed class AgentsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new AgentElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AgentElement)element).AgentName;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        public void Add(AgentElement agent)
        {
            if (agent != null)
            {
                //                service.UpdateServiceCollection();
                this.BaseAdd(agent);
            }
        }

        public void Clear()
        {
            base.BaseClear();
        }

        public void Remove(string name)
        {
            base.BaseRemove(name);
        }

        protected override string ElementName
        {
            get
            {
                return "agent";
            }
        }
    }


    public sealed class AgentElement : ConfigurationElement
    {
        [ConfigurationProperty("agentName", IsKey = true, IsRequired = true)]
        public string AgentName
        {
            get
            {
                return (string)base["agentName"];
            }
            set
            {
                base["agentName"] = value;
            }
        }


        [ConfigurationProperty("serviceName", IsRequired = true)]
        public string Servicename
        {
            get
            {
                return (string)base["serviceName"];
            }
            set
            {
                base["serviceName"] = value;
            }
        }



        [ConfigurationProperty("description", IsRequired = false)]
        public string Description
        {
            get
            {
                return (string)base["description"];
            }
            set
            {
                base["description"] = value;
            }
        }

        [ConfigurationProperty("type", IsRequired = true)]
        public string Type
        {
            get
            {
                return (string)base["type"];
            }
            set
            {
                base["type"] = value;
            }
        }



        [ConfigurationProperty("default")]
        [TypeConverter(typeof(YesNoToBooleanConverter))]
        public bool Default
        {
            get
            {
                return (bool)base["default"];
            }
            set
            {
                base["default"] = value;
            }
        }
    }
}
