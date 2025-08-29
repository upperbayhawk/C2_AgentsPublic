//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
using System.Configuration;
using System.ComponentModel;

namespace Upperbay.Agent.ConfigurationSettings
{


    public sealed class ToolboxSettings : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public ToolsCollection Tools
        {
            get
            {
                return (ToolsCollection)base[""];
            }
            set
            {
                base[""] = value;
            }
        }
    }

    public sealed class ToolsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ToolElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ToolElement)element).ToolName;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        public void Add(ToolElement tool)
        {
            if (tool != null)
            {
                //                service.UpdateServiceCollection();
                this.BaseAdd(tool);
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
                return "tool";
            }
        }
    }


    public sealed class ToolElement : ConfigurationElement
    {
        [ConfigurationProperty("toolName", IsKey = true, IsRequired = true)]
        public string ToolName
        {
            get
            {
                return (string)base["toolName"];
            }
            set
            {
                base["toolName"] = value;
            }
        }


        [ConfigurationProperty("interfaceName", IsRequired = true)]
        public string InterfaceName
        {
            get
            {
                return (string)base["interfaceName"];
            }
            set
            {
                base["interfaceName"] = value;
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
