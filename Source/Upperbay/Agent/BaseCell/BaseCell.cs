﻿//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.Remoting;

using Upperbay.Agent.Interfaces;
using Upperbay.Core.Logging;
using Upperbay.Agent.ConfigurationSettings;



namespace Upperbay.Agent.Cell
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
	public class BaseCell : ICell
	{
       
		public BaseCell()
		{
        }

        #region ICell Members

        /// <summary>
        /// 
        /// </summary>
        public void OnCellStart()
		{

            // initialize, if not already done
            if (!this._isInitialized)
                return;

            // hook for derived classes to add custom Start code
            OnStart();

            this._isRunning = true;

            OnCellRun();

        }
        
        /// <summary>
        /// Initialize Host Services.
        /// </summary>
        /// <param name="hostServices"></param>
        /// <returns></returns>
        public bool OnCellInitialize(IHostColonyServices hostServices)
        {

            if (this._isInitialized)
            {
                Log2.Trace("Already Initialized!: {0} {1}",
                    this._myHost.ColonyName, this._myHost.ServiceName);
                return true;
            }

            _myAgents = new List<INativeAgent>();
            _myAgents.Clear();

            try
            {
                this._myHost = hostServices;
                Log2.Trace("ColonyCell Communicating Thru Host Services: {0} {1}",
                    this._myHost.ColonyName, this._myHost.ServiceName);

                Configuration config = ConfigurationManager.OpenExeConfiguration("Upperbay.Agent.Colony.exe");
                AgentsSettings agents = (AgentsSettings)config.GetSection("agents");

                ConfigurationHelper helper = new ConfigurationHelper();

                foreach (AgentElement agent in agents.Agents)
                {
                    //Log2.Trace("Is this Agent for my Service? {0} == {1}", this._myHost.ServiceName, agent.Servicename);

                    if (hostServices.ServiceName == agent.Servicename)
                    {
                        Log2.Trace("AgentName: {0} {1} {2}", this._myHost.ColonyName, this._myHost.ServiceName, agent.AgentName);
                        Log2.Trace("Description: {0} {1} {2}", this._myHost.ColonyName, this._myHost.ServiceName, agent.Description);
                        Log2.Trace("Type: {0} {1} {2}", this._myHost.ColonyName, this._myHost.ServiceName, agent.Type);

                        IDictionary dic = helper.ParseTypeString(agent.Type);

                        Log2.Trace("Activating Agent Class {0} {1} from {2}",
                            this._myHost.ServiceName, (string)dic["type"], (string)dic["assembly"]);
                        ObjectHandle handle = Activator.CreateInstanceFrom((string)dic["assembly"], (string)dic["type"]);
                        INativeAgent server = (INativeAgent)handle.Unwrap();

                        Log2.Trace("{0}: Agent Class Loaded", this._myHost.ServiceName);

                        // Initialize CELL with Colony Host Services
                        Log2.Trace("Initializing Agent IHostColonyServices {0}", this._myHost.ServiceName);
                        IHostColonyServices agentServices = (IHostColonyServices)_myHost;

                        //Maybe Later
                        //agentServices.CollectiveName = String.Empty;
                        //agentServices.ColonyName = this.myHost.ColonyName;
                        //agentServices.CellName = this.myHost.CellName;
                        //agentServices.LogicalName = String.Empty;
                        //agentServices.Version = String.Empty;
                        //agentServices.Description = this.myHost.Description;
                        agentServices.ClassName = (string)dic["type"];
                        agentServices.AgentName = agent.AgentName;
                        agentServices.AgentRole = agent.AgentName;
                        agentServices.AgentNickName = agent.AgentNickName;
                        Log2.Trace("Initializing Agent IHostColonyServices: {0} (NickName = {1})",
                            this._myHost.ServiceName, agentServices.AgentNickName);

                        INativeAgent agentInterface = (INativeAgent)server;
                        agentInterface.OnInitialize((IHostColonyServices)agentServices);

                        Log2.Trace("Initializing Agent Thread {0}", this._myHost.ServiceName);
                        //Spawn Thread for Server
                        Thread thread = new Thread(new ThreadStart(agentInterface.OnStart));
                        // Start OnStart.  On a uniprocessor, the thread does not get 
                        // any processor time until the main thread yields.
                        thread.Start();
                        Log2.Trace("Agent Thread Started {0}", this._myHost.ServiceName);

                        _myAgents.Add(server);
                    }

                }// End Foreach

                // hook for derived classes to add custom Initialize code
                OnInit();

                this._isInitialized = true;

                return true;
            }
            catch (Exception Ex)
            {
                Log2.Trace("{0}: Agent Exception: {1}", this._myHost.ServiceName, Ex.ToString());
                return false;
            }

         }


        public void OnCellRun()
        {
            while (!_bAbort)
            {
                this.OnFire();
                Thread.Sleep(2000);
            }
        }

        /// <summary>
        /// Execution Trigger
        /// </summary>
        /// <returns></returns>
        private bool OnFire()
        {
            try
            {
                _iCounter++;
                if (_iCounter > 1000)
                    _iCounter = 0;

                return true;
            }
            catch(Exception Ex)
            {
                Log2.Trace(Ex.ToString());
                return false;
            }
            
        }
        /// <summary>
        /// 
        /// </summary>
        public void OnCellStop()
        {
            try
            {
                Log2.Trace("Calling Agent OnStop Methods");
                foreach (INativeAgent agent in _myAgents)
                {
                    agent.OnStop();
                }
                _myAgents.Clear();

                this._isRunning = false;
            }
            catch (Exception Ex)
            {
                Log2.Trace("BaseCell Exception OnStop: {0}", Ex.ToString());
            }
            _bAbort = true;

            Log2.Trace("ColonyService is Dead!\n");
        }

        #endregion

        /// <summary>
        /// Provides a hook implement custom initialization code.
        /// </summary>
        protected virtual void OnInit() { }

        /// <summary>
        /// Provides a hook implement custom start code.
        /// </summary>
        protected virtual void OnStart() { }

        /// <summary>
        /// Provides a hook implement custom stop code.
        /// </summary>
        protected virtual void OnStop() { }

        #region Private State

        /// <summary>
        /// Private State
        /// </summary>

        private bool _bAbort = false;
        private bool _isInitialized = false;
        public bool _isRunning = false;

        private IHostColonyServices _myHost = null;
        private List<INativeAgent> _myAgents = null;

        Int32 _iCounter = 0;
        #endregion
    }
}
