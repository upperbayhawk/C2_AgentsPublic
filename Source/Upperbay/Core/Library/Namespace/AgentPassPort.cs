//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
using System;
using System.Text;

namespace Upperbay.Core.Library
{
    /// <summary>
    /// AgentName
    /// </summary>
    [Serializable]
    public class AgentPassPort
    {


        private string _celestial = String.Empty;
        public string Celestial { get { return this._celestial; } set { this._celestial = value; } }

        private string _collective = String.Empty;
        public string Collective { get { return this._collective; } set { this._collective = value; } }

        private string _community = String.Empty;
        public string Community { get { return this._community; } set { this._community = value; } }

        private string _cluster = String.Empty;
        public string Cluster { get { return this._cluster; } set { this._cluster = value; } }

        private string _colony = String.Empty;
        public string Colony { get { return this._colony; } set { this._colony = value; } }
        //------------------------------------------------------------------

        private string _fullName = String.Empty;
        public string FullName { get { return this._fullName; } set { this._fullName = value; } }

        private string _fullNamePrefix = String.Empty;
        public string FullNamePrefix { get { return this._fullNamePrefix; } set { this._fullNamePrefix = value; } }

        //------------------------------------------------------------------

        private string _carrier = String.Empty;
        public string Carrier { get { return this._carrier; } set { this._carrier = value; } }

        private string _cell = String.Empty;
        public string Cell { get { return this._cell; } set { this._cell = value; } }

        //------------------------------------------------------------------

        private string _serviceName = String.Empty;
        public string ServiceName { get { return this._serviceName; } set { this._serviceName = value; } }

        private string _colonyServiceName = String.Empty;
        public string ColonyServiceName { get { return this._colonyServiceName; } set { this._colonyServiceName = value; } }

        private string _className = String.Empty;
        public string ClassName { get { return this._className; } set { this._className = value; } }

        private string _version = String.Empty;
        public string Version { get { return this._version; } set { this._version = value; } }

        private string _description = String.Empty;
        public string Description { get { return this._description; } set { this._description = value; } }

        private string _logicalName = String.Empty;
        public string LogicalName { get { return this._logicalName; } set { this._logicalName = value; } }

        private string _assemblyPath = String.Empty;
        public string AssemblyPath { get { return this._assemblyPath; } set { this._assemblyPath = value; } }

        private string _assemblyName = String.Empty;
        public string AssemblyName { get { return this._assemblyName; } set { this._assemblyName = value; } }

        private string _assemblyDirectory = String.Empty;
        public string AssemblyDirectory { get { return this._assemblyDirectory; } set { this._assemblyDirectory = value; } }

        private string _agentName = String.Empty;
        public string AgentName { get { return this._agentName; } set { this._agentName = value; } }

        private string _agentNickName = String.Empty;
        public string AgentNickName { get { return this._agentNickName; } set { this._agentNickName = value; } }

        private string _agentRole = String.Empty;
        public string AgentRole { get { return this._agentRole; } set { this._agentRole = value; } }

        //private LocalLog _localLog = null;
        //public LocalLog LocalLog { get { return this._localLog; } set { this._localLog = value; } }

        public AgentPassPort()
        {
        }


        
        /// <summary>
        /// Parse Rules:
        ///     Strip off 
        ///         Celestial
        ///         Collective
        ///         Community
        ///         Cluster
        ///         
        ///     Strip off
        ///         .Attribute
        ///         
        ///     CloudName remains
        ///     
        /// 
        /// </summary>
        /// <param name="agentName"></param>
        public bool IsMyName(string anAgentNameWithExtension)
        {
            string anAgentName = ExtractFullName(anAgentNameWithExtension);
            if (anAgentName == this.FullName)
                return true;
            else
                return false;
        }


        /// <summary>
        /// 
        /// </summary>
        public void GenerateFullName()
        {
            StringBuilder sb = new StringBuilder();  
            sb.Append(
                "/" + this.Celestial + '/' + this.Collective + '/' + this.Community + '/' + this.Colony + '/');
            this.FullNamePrefix = sb.ToString();

            sb.Append(this.AgentName);
            this.FullName = sb.ToString();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="anAgentNameWithExtension"></param>
        /// <returns></returns>
        public string ExtractFullName(string anAgentNameWithExtension)
        {
            // Strip off attribute extension
            int i = anAgentNameWithExtension.IndexOf('.');
            if (i > 0)
            {
                return anAgentNameWithExtension.Remove(i);
                //string[] nameFields = anAgentName.Split('/');
            }
            return anAgentNameWithExtension;
        }

        public string[] ParseFullName(string anAgentNameWithExtension)
        {
            // Strip off attribute extension
            int i = anAgentNameWithExtension.IndexOf('.');
            if (i > 0)
            {
                string strNaked = anAgentNameWithExtension.Remove(i);
                string[] nameFields = strNaked.Split('/');
                return nameFields;
            }
            return null;
        }


    }






}
