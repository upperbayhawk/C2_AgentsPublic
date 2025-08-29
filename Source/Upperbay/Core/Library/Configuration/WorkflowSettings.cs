using System;
using System.Configuration;
using System.ComponentModel;
using System.Globalization;

namespace Upperbay.Agent.Library.Configurator
{


    public sealed class WorkflowSettings : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public WorkflowsCollection Workflows
        {
            get
            {
                return (WorkflowsCollection)base[""];
            }
            set
            {
                base[""] = value;
            }
        }
    }

    public sealed class WorkflowsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new WorkflowElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((WorkflowElement)element).WorkflowName;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        public void Add(WorkflowElement workflow)
        {
            if (workflow != null)
            {
                //                service.UpdateServiceCollection();
                this.BaseAdd(workflow);
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
                return "workflow";
            }
        }
    }


    public sealed class WorkflowElement : ConfigurationElement
    {
        [ConfigurationProperty("workflowName", IsKey = true, IsRequired = true)]
        public string WorkflowName
        {
            get
            {
                return (string)base["workflowName"];
            }
            set
            {
                base["workflowName"] = value;
            }
        }


        [ConfigurationProperty("agentName", IsRequired = true)]
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
