//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
using System;

using Upperbay.Agent.Interfaces;

namespace Upperbay.Core.Library
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ColonyServices : IHostColonyServices
    {


        public ColonyServices()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Initialize()
        {
            //get the cache object 
            //cache = XMLAgentCache.GetAgentCacheService();
            return true;
        }

	

        #region IColonyHostServices Members

        public string CelestialName { get { return this._celestialName; } set { this._celestialName = value; } }
        public string CollectiveName { get { return this._collectiveName; } set { this._collectiveName = value; } }
        public string CommunityName { get { return this._communityName; } set { this._communityName = value; } }
        public string ClusterName { get { return this._clusterName; } set { this._clusterName = value; } }
        public string CarrierName { get { return this._carrierName; } set { this._carrierName = value; } }
        public string ColonyName { get { return this._colonyName; } set { this._colonyName = value; } }
        public string ServiceName { get { return this._serviceName; } set { this._serviceName = value; } }
        public string ColonyServiceName { get { return this._colonyServiceName; } set { this._colonyServiceName = value; } }
        public string ClassName { get { return this._className; } set { this._className = value; } }
        public string Version { get { return this._version; } set { this._version = value; } }
        public string Description{  get { return this._description; }  set { this._description = value; } }
        public string LogicalName{ get { return this._logicalName; } set { this._logicalName = value; } }
        public string AssemblyPath { get { return this._assemblyPath; } set { this._assemblyPath = value; }}
        public string AssemblyName { get { return this._assemblyName; } set { this._assemblyName = value; } }
        public string AssemblyDirectory { get { return this._assemblyDirectory; } set { this._assemblyDirectory = value; } }

        public string AgentName { get { return this._agentName; } set { this._agentName = value; } }
        public string AgentNickName { get { return this._agentNickName; } set { this._agentNickName = value; } }
        public string AgentRole { get { return this._agentRole; } set { this._agentRole = value; } }


        public bool UpdateAgentData()
        {
            throw new Exception("The method or operation is not implemented.");
        }

      


        #endregion

        #region Private State Members

        //private XMLAgentCache cache = null;

        private string _celestialName = String.Empty;
        private string _collectiveName = String.Empty;
        private string _communityName = String.Empty;
        private string _clusterName = String.Empty;
        private string _carrierName = String.Empty;
        private string _colonyName = String.Empty;
        private string _serviceName = String.Empty;
        private string _colonyServiceName = String.Empty;
        private string _className = String.Empty;
        private string _agentRole = String.Empty;
        private string _version = String.Empty;
        private string _description = String.Empty;
        private string _logicalName = String.Empty;
        private string _assemblyPath = String.Empty;
        private string _assemblyName = String.Empty;
        private string _assemblyDirectory = String.Empty;

        private string _agentName = String.Empty;
        private string _agentNickName = String.Empty;


        #endregion
    }
}
