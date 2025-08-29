//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
using System;
using System.Configuration;
using System.ComponentModel;
using System.Globalization;

namespace Upperbay.Agent.ConfigurationSettings
{


    public sealed class ServicesSettings : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public ServicesCollection Services
        {
            get
            {
                return (ServicesCollection)base[""];
            }
            set
            {
                base[""] = value;
            }
        }
    }

    public sealed class ServicesCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ServiceElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ServiceElement)element).ServiceName;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        public void Add(ServiceElement service)
        {
            if (service != null)
            {
                //                service.UpdateServiceCollection();
                this.BaseAdd(service);
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
                return "service";
            }
        }
    }


    public sealed class ServiceElement : ConfigurationElement
    {
        [ConfigurationProperty("serviceName", IsKey = true, IsRequired = true)]
        public string ServiceName
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

        [ConfigurationProperty("displayName", IsRequired = true)]
        public string DisplayName
        {
            get
            {
                return (string)base["displayName"];
            }
            set
            {
                base["displayName"] = value;
            }
        }

        [ConfigurationProperty("description", IsRequired = true)]
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


        [ConfigurationProperty("serviceCategory", IsRequired = true)]
        public string ServiceCategory
        {
            get
            {
                return (string)base["serviceCategory"];
            }
            set
            {
                base["serviceCategory"] = value;
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
        [ConfigurationProperty("startType", IsRequired = true)]
        public string StartType
        {
            get
            {
                return (string)base["startType"];
            }
            set
            {
                base["startType"] = value;
            }
        }


        [ConfigurationProperty("version", IsRequired = true)]
        public string Version
        {
            get
            {
                return (string)base["version"];
            }
            set
            {
                base["version"] = value;
            }
        }


        [ConfigurationProperty("port", IsRequired = false)]
        public string Port
        {
            get
            {
                return (string)base["port"];
            }
            set
            {
                base["port"] = value;
            }
        }

        [ConfigurationProperty("uri", IsRequired = false)]
        public string Uri
        {
            get
            {
                return (string)base["uri"];
            }
            set
            {
                base["uri"] = value;
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

    public class YesNoToBooleanConverter : ConfigurationConverterBase
    {
        public override object ConvertTo(ITypeDescriptorContext ctx, CultureInfo ci, object value, Type type)
        {
            bool bValue = (bool)value;
            if (bValue)
                return "yes";

            return "no";
        }

        public override object ConvertFrom(ITypeDescriptorContext ctx, CultureInfo ci, object data)
        {
            bool result = false;

            string value = (string)data;
            if (string.IsNullOrEmpty(value))
                throw new ConfigurationErrorsException();

            if (value == "yes")
                result = true;
            else if (value == "no")
                result = false;
            else
                throw new ConfigurationErrorsException();
            return result;
        }
    }
}
