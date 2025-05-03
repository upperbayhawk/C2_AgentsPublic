using System;
using System.Configuration;
using System.ComponentModel;
using System.Globalization;

namespace Upperbay.Agent.Library.Configurator
{


    public sealed class DatastoreSettings : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public DatastoresCollection Datastores
        {
            get
            {
                return (DatastoresCollection)base[""];
            }
            set
            {
                base[""] = value;
            }
        }
    }

    public sealed class DatastoresCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new DatastoreElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DatastoreElement)element).DatastoreName;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        public void Add(DatastoreElement datastore)
        {
            if (datastore != null)
            {
                //                service.UpdateServiceCollection();
                this.BaseAdd(datastore);
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
                return "datastore";
            }
        }
    }


    public sealed class DatastoreElement : ConfigurationElement
    {
        [ConfigurationProperty("datastoreName", IsKey = true, IsRequired = true)]
        public string DatastoreName
        {
            get
            {
                return (string)base["datastoreName"];
            }
            set
            {
                base["datastoreName"] = value;
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
