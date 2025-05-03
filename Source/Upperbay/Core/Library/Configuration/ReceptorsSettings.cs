using System;
using System.Configuration;
using System.ComponentModel;
using System.Globalization;

namespace Upperbay.Agent.Library.Configurator
{


    public sealed class ReceptorsSettings : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public ReceptorsCollection Receptors
        {
            get
            {
                return (ReceptorsCollection)base[""];
            }
            set
            {
                base[""] = value;
            }
        }
    }

    public sealed class ReceptorsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ReceptorElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ReceptorElement)element).ReceptorName;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        public void Add(ReceptorElement receptor)
        {
            if (receptor != null)
            {
                //                service.UpdateServiceCollection();
                this.BaseAdd(receptor);
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
                return "receptor";
            }
        }
    }


    public sealed class ReceptorElement : ConfigurationElement
    {
        [ConfigurationProperty("receptorName", IsKey = true, IsRequired = true)]
        public string ReceptorName
        {
            get
            {
                return (string)base["receptorName"];
            }
            set
            {
                base["receptorName"] = value;
            }
        }

        [ConfigurationProperty("serviceName", IsRequired = true)]
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

        [ConfigurationProperty("version", IsRequired = false)]
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

    //public class YesNoToBooleanConverter : ConfigurationConverterBase
    //{
    //    public override object ConvertTo(ITypeDescriptorContext ctx, CultureInfo ci, object value, Type type)
    //    {
    //        bool bValue = (bool)value;
    //        if (bValue)
    //            return "yes";

    //        return "no";
    //    }

    //    public override object ConvertFrom(ITypeDescriptorContext ctx, CultureInfo ci, object data)
    //    {
    //        bool result = false;

    //        string value = (string)data;
    //        if (string.IsNullOrEmpty(value))
    //            throw new ConfigurationErrorsException();

    //        if (value == "yes")
    //            result = true;
    //        else if (value == "no")
    //            result = false;
    //        else
    //            throw new ConfigurationErrorsException();
    //        return result;
    //    }
    //}
}
