using System;
using System.Data;
using System.Globalization;
using System.Text;
using System.Xml;
//using Microsoft.Practices.EnterpriseLibrary.Data;
//using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using System.Diagnostics;

using Upperbay.Core.Logging;
using Upperbay.Core.Library;



namespace Upperbay.Agent.ColonyMatrix
{
    /// <summary>
    /// Encapulsates access to the database.
    /// </summary>
    public class AgentData
    {

        //-----------------------------------------------------------------------------------------------------
        // Public Properties

        //private string _celestial = null;
        //public string Celestial { get { return this._celestial; } set { this._celestial = value; } }

        //private string _collective = null;
        //public string Collective { get { return this._collective; } set { this._collective = value; } }

        //private string _community = null;
        //public string Community { get { return this._community; } set { this._community = value; } }

        //private string _cluster = null;
        //public string Cluster { get { return this._cluster; } set { this._cluster = value; } }

        //private string _carrier = null;
        //public string Carrier { get { return this._carrier; } set { this._carrier = value; } }

        //private string _colony = null;
        //public string Colony { get { return this._colony; } set { this._colony = value; } }

        //private string _service = null;
        //public string Service { get { return this._service; } set { this._service = value; } }

        //private static TraceSwitch _agentSwitch = Upperbay.Core.Logging.Log2.AgentSwitch;

        private bool _online = true;
        public bool Online { get { return this._online; } set { this._online = value; } }

        private AgentPassPort _agentPassPort = null;
        public AgentPassPort AgentPassPort { get { return this._agentPassPort; } set { this._agentPassPort = value; } }

        //TODO: Configuration parameter
        private string _databaseName = "UpperbayAgents";
        public string DatabaseName { get { return this._databaseName; } set { this._databaseName = value; } }


        // Private members
      //  private Database _database = null; replace!
        //private string _qualifiedPrefix = null;

        public AgentData()
        {
        }


        //-----------------------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="agentName"></param>
        /// <param name="description"></param>
        /// <param name="datatype"></param>
        public void AddAgent(string agentName, string agentNickName, string description, string datatype)
        {
            return;

            //string qualifiedAgentName;

            ////~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //if ((!_online) || (_agentPassPort == null))
            //{
            //    if (!_online)
            //        Log2.Error("AddAgent Offline");
            //    else
            //        Log2.Error("AddAgent Failed: Null _agentPassPort");
            //    return;
            //}
            ////~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            ////if (_qualifiedPrefix == null)
            ////    _qualifiedPrefix = _agentPassPort.FullNamePrefix;

            //qualifiedAgentName = _agentPassPort.FullName;
            //agentNickName = _agentPassPort.AgentNickName;

            ////~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            //try
            //{
            //    if (_database == null)
            //    {
            //        _database = DatabaseFactory.CreateDatabase(_databaseName);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    //ToDO: Proper error handling
            //    Log2.Error("AddAgent Failed: Createdatabase {0}", ex.ToString());
            //}

            //int agentStorageKey = qualifiedAgentName.GetHashCode();
            //DateTime dataTime = DateTime.Now;

            //// Remove old agent if one exists

            //try
            //{

            //    DbCommand removeCommand = _database.GetStoredProcCommand("RemoveAgent");
            //    _database.AddInParameter(removeCommand, "@agentStorageKey", DbType.Int32, agentStorageKey);
            //    _database.AddInParameter(removeCommand, "@qualifiedAgentName", DbType.String, qualifiedAgentName);

            //    _database.ExecuteNonQuery(removeCommand);

            //}
            //catch (Exception ex)
            //{
            //    //TODO: error message
            //    Log2.Error("Virgin Agent: {0}", ex.ToString());
            //    // _online = false;
            //}


            //// Just Do It

            //try
            //{

            //    DbCommand insertCommand = _database.GetStoredProcCommand("AddAgent");
            //    _database.AddInParameter(insertCommand, "@agentStorageKey", DbType.Int32, agentStorageKey);
            //    _database.AddInParameter(insertCommand, "@qualifiedAgentName", DbType.String, qualifiedAgentName);
            //    _database.AddInParameter(insertCommand, "@agentNickName", DbType.String, agentNickName);
            //    _database.AddInParameter(insertCommand, "@celestial", DbType.String, _agentPassPort.Celestial);
            //    _database.AddInParameter(insertCommand, "@collective", DbType.String, _agentPassPort.Collective);
            //    _database.AddInParameter(insertCommand, "@community", DbType.String, _agentPassPort.Community);
            //    _database.AddInParameter(insertCommand, "@cluster", DbType.String, _agentPassPort.Cluster);
            //    _database.AddInParameter(insertCommand, "@carrier", DbType.String, _agentPassPort.Carrier);
            //    _database.AddInParameter(insertCommand, "@colony", DbType.String, _agentPassPort.Colony);
            //    _database.AddInParameter(insertCommand, "@service", DbType.String, _agentPassPort.ServiceName);
            //    _database.AddInParameter(insertCommand, "@name", DbType.String, agentName);
            //    _database.AddInParameter(insertCommand, "@description", DbType.String, description);
            //    _database.AddInParameter(insertCommand, "@datatype", DbType.String, datatype);
            //    _database.AddInParameter(insertCommand, "@updateTime", DbType.DateTime, dataTime);

            //    _database.ExecuteNonQuery(insertCommand);

            //}
            //catch (Exception ex)
            //{
            //    //TODO: error message
            //    Log2.Error("AddAgent SPC Failed: {0}", ex.ToString());
            //   // _online = false;
            //}
        }
        //-----------------------------------------------------------------------------------------------------
        public void RemoveAgent(string agentName)
        {
            return;

            //string qualifiedAgentName;

            ////~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //if ((!_online) || (_agentPassPort == null))
            //{
            //    if (!_online)
            //        Log2.Error("RemoveAgent Offline");
            //    else
            //        Log2.Error("AddAgent Failed: Null _agentPassPort");
            //    return;
            //}
            ////~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            ////if (_qualifiedPrefix == null)
            ////    _qualifiedPrefix = "/" + _celestial + "/" + _collective + "/" + _community + "/" + _cluster + "/" + _colony;

            ////qualifiedAgentName = _qualifiedPrefix + "/" + agentName;
            //qualifiedAgentName = _agentPassPort.FullName;
            ////~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


            //int agentStorageKey = qualifiedAgentName.GetHashCode();
            //DateTime dataTime = DateTime.Now;

            //// Remove old agent if one exists

            //try
            //{

            //    DbCommand removeCommand = _database.GetStoredProcCommand("RemoveAgent");
            //    _database.AddInParameter(removeCommand, "@agentStorageKey", DbType.Int32, agentStorageKey);
            //    _database.AddInParameter(removeCommand, "@qualifiedAgentName", DbType.String, qualifiedAgentName);

            //    _database.ExecuteNonQuery(removeCommand);

            //}
            //catch (Exception ex)
            //{
            //    //TODO: error message
            //    Log2.Error("Virgin Agent: {0}", ex.ToString());
            //    // _online = false;
            //}
            
        }
        //-----------------------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public void LoadAgents()
        {


            //try
            //{
            //    if (_database == null)
            //    {
            //        _database = DatabaseFactory.CreateDatabase(_databaseName);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    //ToDO: Proper error handling
            //    Log2.Error("LoadAgents Failed: Createdatabase {0}", ex.ToString());
            //}


            //try
            //{

            //    DbCommand insertCommand = _database.GetStoredProcCommand("LoadAgents");
            //    _database.ExecuteNonQuery(insertCommand);

            //}
            //catch (Exception ex)
            //{
            //    //TODO: error message
            //    Log2.Error("LoadAgents Failed:  LoadAgents SPC: {0}", ex.ToString());
            //    // _online = false;
            //}
        }
        
        
        //-----------------------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="agentName"></param>
        /// <param name="propertyName"></param>
        public void AddAgentProperty(string agentName, string propertyName)
        {
            return;

            //string qualifiedAgentName;
            //string qualifiedPropertyName;
            //string localPropertyName;
            
            ////~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //if ((!_online) || (_agentPassPort == null))
            //{
            //    if (!_online)
            //        Log2.Error("RemoveAgent Offline");
               
            //    return;
            //}

            ////~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            ////if (_qualifiedPrefix == null)
            ////    _qualifiedPrefix = "/" + _celestial + "/" + _collective + "/" + _community + "/" + _cluster + "/" + _colony ;

            ////qualifiedAgentName = _qualifiedPrefix + "/" + agentName;


            //qualifiedAgentName = _agentPassPort.FullName;
            //qualifiedPropertyName = qualifiedAgentName + "." + propertyName;
            //localPropertyName = _agentPassPort.AgentNickName + "." + propertyName;

            ////~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            //try
            //{
            //    if (_database == null)
            //    {
            //        _database = DatabaseFactory.CreateDatabase(_databaseName);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    //ToDO: Proper error handling
            //    Log2.Error("AddAgentProperty Failed: Createdatabase {0}", ex.ToString());
            //}

            ////~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            //int qualifiedPropertyStorageKey = qualifiedPropertyName.GetHashCode();
            //int agentStorageKey = qualifiedAgentName.GetHashCode();
            //int localStorageKey = localPropertyName.GetHashCode();
            //DateTime dataTime = DateTime.Now;


            //try
            //{

            //    DbCommand removeCommand = _database.GetStoredProcCommand("RemoveValue");
            //    _database.AddInParameter(removeCommand, "@qualifiedPropertyStorageKey", DbType.Int32, qualifiedPropertyStorageKey);
            //    _database.AddInParameter(removeCommand, "@qualifiedPropertyName", DbType.String, qualifiedPropertyName);

            //    _database.ExecuteNonQuery(removeCommand);

            //}
            //catch (Exception ex)
            //{
            //    //TODO: error message
            //    Log2.Error("Virgin Property: {0}", ex.ToString());
            //    // _online = false;
            //}



            //try
            //{

            //    DbCommand insertCommand = _database.GetStoredProcCommand("AddValue");
            //    _database.AddInParameter(insertCommand, "@agentStorageKey", DbType.Int32, agentStorageKey);
            //    _database.AddInParameter(insertCommand, "@qualifiedPropertyStorageKey", DbType.Int32, qualifiedPropertyStorageKey);
            //    _database.AddInParameter(insertCommand, "@localPropertyStorageKey", DbType.Int32, localStorageKey);
            //    _database.AddInParameter(insertCommand, "@qualifiedAgentName", DbType.String, qualifiedAgentName);
            //    _database.AddInParameter(insertCommand, "@qualifiedPropertyName", DbType.String, qualifiedPropertyName);
            //    _database.AddInParameter(insertCommand, "@localPropertyName", DbType.String, localPropertyName);
            //    _database.AddInParameter(insertCommand, "@propertyName", DbType.String, propertyName);
            //    _database.AddInParameter(insertCommand, "@updateTime", DbType.DateTime, dataTime);

            //    _database.ExecuteNonQuery(insertCommand);

            //}
            //catch (Exception ex)
            //{
            //    //TODO: error message
            //    Log2.Error("AddAgentProperty Failed: AddValue SPC {0}", ex.ToString());
            //    //_online = false;

            //}
        }



        //-----------------------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="agentName"></param>
        /// <param name="propertyName"></param>
        public void RemoveAgentProperty(string agentName, string propertyName)
        {
            return;

            //string qualifiedAgentName;
            //string qualifiedPropertyName;

            ////~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //if ((!_online) || (_agentPassPort == null))
            //{
            //    if (!_online)
            //        Log2.Error("RemoveAgent Offline");
            //    return;
            //}

            ////~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            ////if (_qualifiedPrefix == null)
            ////    _qualifiedPrefix = "/" + _celestial + "/" + _collective + "/" + _community + "/" + _cluster + "/" + _colony;

            ////qualifiedAgentName = _qualifiedPrefix + "/" + agentName;


            //qualifiedAgentName = _agentPassPort.FullName;
            //qualifiedPropertyName = qualifiedAgentName + "." + propertyName;

            ////qualifiedPropertyName = _qualifiedPrefix + "/" + agentName + "." + propertyName;

            ////~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            
            //int qualifiedPropertyStorageKey = qualifiedPropertyName.GetHashCode();
            //int agentStorageKey = qualifiedAgentName.GetHashCode();
            //DateTime dataTime = DateTime.Now;
            
            //try
            //{

            //    DbCommand removeCommand = _database.GetStoredProcCommand("RemoveValue");
            //    _database.AddInParameter(removeCommand, "@qualifiedPropertyStorageKey", DbType.Int32, qualifiedPropertyStorageKey);
            //    _database.AddInParameter(removeCommand, "@qualifiedPropertyName", DbType.String, qualifiedPropertyName);

            //    _database.ExecuteNonQuery(removeCommand);

            //}
            //catch (Exception ex)
            //{
            //    //TODO: error message
            //    Log2.Error("Exception Removing Property: {0}", ex.ToString());
            //    // _online = false;
            //}

        }


        //-----------------------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void UpdatePropertyValue(string agentName, string propertyName, string value, string qual, DateTime time)
        {
            return;


            ////string qualifiedAgentName;
            //string qualifiedPropertyName;

            ////~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //if ((!_online) || (_agentPassPort == null))
            //{
            //    if (!_online)
            //        Log2.Error("UpdatePropertyValue Offline");
               
            //    return;
            //}
            ////~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            //try
            //{
            //    if (_database == null)
            //    {
            //        _database = DatabaseFactory.CreateDatabase(_databaseName);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    //ToDO: Proper error handling
            //    Log2.Error("UpdatePropertyValue Failed: CreateDatabase {0}", ex.ToString());
            //}

            ////~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            ////if (_qualifiedPrefix == null)
            ////    _qualifiedPrefix = "/" + _celestial + "/" + _collective + "/" + _community + "/" + _cluster + "/" + _colony;
            //////qualifiedAgentName = _qualifiedPrefix + "/" + agentName;
            ////qualifiedPropertyName = _qualifiedPrefix + "/" + agentName + "." + propertyName;


            //qualifiedPropertyName = _agentPassPort.FullName + "." + propertyName;


            ////int agentStorageKey = qualifiedAgentName.GetHashCode();
            //int qualifiedPropertyStorageKey = qualifiedPropertyName.GetHashCode();

            //DateTime dataTime = time;

            ////TODO: Handle quality properly
            ////string _quality = "GOOD";
            //string quality = qual;

            //try
            //{

            //    DbCommand insertCommand = _database.GetStoredProcCommand("UpdateValue");
            //    _database.AddInParameter(insertCommand, "@qualifiedPropertyStorageKey", DbType.Int32, qualifiedPropertyStorageKey);
            //    _database.AddInParameter(insertCommand, "@qualifiedPropertyName", DbType.String, qualifiedPropertyName);
            //    _database.AddInParameter(insertCommand, "@value", DbType.String, value);
            //    _database.AddInParameter(insertCommand, "@quality", DbType.String, quality);
            //    _database.AddInParameter(insertCommand, "@updateTime", DbType.DateTime, dataTime);
            //    _database.ExecuteNonQuery(insertCommand);

            //}
            //catch (Exception ex)
            //{
            //    //TODO: error message
            //    Log2.Trace("UpdatePropertyValue Failed: UpdateValue SPC {0}", ex.ToString());
            //   // _online = false;

            //}
        }

        //-----------------------------------------------------------------------------------------------------
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="name"></param>
        ///// <param name="value"></param>
        //public string GetPropertyValue(string agentName, string propertyName)
        //{
        //    //TODO: remove name from argumnet

        //    string qualifiedPropertyName = null; ;
        //    string localPropertyName = null; ;

        //    //if (_qualifiedPrefix == null)
        //    //    _qualifiedPrefix = "/" + _celestial + "/" + _collective + "/" + _community + "/" + _cluster + "/" + _colony;
        //    //qualifiedPropertyName = _qualifiedPrefix + "/" + agentName + "." + propertyName;

        //    qualifiedPropertyName = _agentPassPort.FullName + "." + propertyName;
        //    localPropertyName = _agentPassPort.AgentNickName + "." + propertyName;

        //    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //    //Log2.Trace("Qauled Name: {0}", qualifiedPropertyName);
        //    //if (qualifiedPropertyName.StartsWith("/"))
        //    //{
        //    //    qualifiedPropertyName = qualifiedPropertyName.TrimStart('/');
        //    //    Log2.Trace("Trimmed Qauled {0}", qualifiedPropertyName);
        //    //}
        //    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        //    try
        //    {
        //        if (_database == null)
        //        {
        //            _database = DatabaseFactory.CreateDatabase(_databaseName);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //ToDO: Proper error handling
        //        Log2.Trace("UpdatePropertyValue Failed: CreateDatabase {0}", ex.ToString());
        //    }

        //    int propertyStorageKey = qualifiedPropertyName.GetHashCode();
        //    DateTime dataTime = DateTime.Now;
  
        //    try
        //    {

        //        DbCommand selectCommand = _database.GetStoredProcCommand("GetValue");
        //        _database.AddInParameter(selectCommand, "@propertyStorageKey", DbType.Int32, propertyStorageKey);
        //        _database.AddInParameter(selectCommand, "@qualifiedPropertyName", DbType.String, qualifiedPropertyName);
        //        _database.AddOutParameter(selectCommand, "@itemValue", DbType.String, 48);
        //        _database.ExecuteNonQuery(selectCommand);
                    
        //        string str = (string)_database.GetParameterValue(selectCommand,"@itemValue");
                
        //        return str;

        //    }
        //    catch (Exception ex)
        //    {
        //        //TODO: error message
        //        Log2.Trace("GetPropertyValue Failed: GetValue SPC {0}", ex.ToString());
        //        // _online = false;

        //    }
        //    return null;
        //}

        //-----------------------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public string GetQualifiedPropertyValue(string qualifiedPropertyName, out string quality, out DateTime time)
        {
            //TODO: remove name from argumnet
        

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            Log2.Trace("Qauled Name: {0}", qualifiedPropertyName);
            //if (!qualifiedPropertyName.StartsWith("/"))
            //{
            //    qualifiedPropertyName = "/" + qualifiedPropertyName;
            //    Log2.Trace("New Qauled {0}", qualifiedPropertyName);
            //}
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            //try
            //{
            //    if (_database == null)
            //    {
            //        _database = DatabaseFactory.CreateDatabase(_databaseName);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    //ToDO: Proper error handling
            //    Log2.Error("UpdatePropertyValue Failed: CreateDatabase {0}", ex.ToString());
            //}

            int propertyStorageKey = qualifiedPropertyName.GetHashCode();
            DateTime dataTime = DateTime.Now;

            //try
            //{

            //    DbCommand selectCommand = _database.GetStoredProcCommand("GetValue");
            //    _database.AddInParameter(selectCommand, "@propertyStorageKey", DbType.Int32, propertyStorageKey);
            //    _database.AddInParameter(selectCommand, "@propertyName", DbType.String, qualifiedPropertyName);
            //    _database.AddOutParameter(selectCommand, "@itemValue", DbType.String, 48);
            //    _database.AddOutParameter(selectCommand, "@itemQuality", DbType.String, 48);
            //    _database.AddOutParameter(selectCommand, "@itemTime", DbType.DateTime, 48);
            //    _database.ExecuteNonQuery(selectCommand);

            //    string valueString = (string)_database.GetParameterValue(selectCommand, "@itemValue");
            //    quality = (string)_database.GetParameterValue(selectCommand, "@itemQuality");
            //    time = (DateTime)_database.GetParameterValue(selectCommand, "@itemTime");

            //    return valueString;

            //}
            //catch (Exception ex)
            //{
            //    //TODO: error message
            //    Log2.Error("GetQualifiedPropertyValue Failed: GetValue SPC {0}", ex.ToString());
            //    // _online = false;

            //}
            quality = null;
            time = DateTime.Now;
            return null;
        }
   
        //-----------------------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void FlushAgents()
        {

            //try
            //{
            //    if (_database == null)
            //    {
            //        _database = DatabaseFactory.CreateDatabase(_databaseName);
            //    }

            //    DbCommand insertCommand = _database.GetStoredProcCommand("FlushAgents");
            //    _database.ExecuteNonQuery(insertCommand);

            //}
            //catch (Exception ex)
            //{
            //    //TODO: error message
            //    Log2.Error("UpdateAgent Failed: FlushAgents {0}", ex.ToString());
            //    // _online = false;

            //}

        }
        //-----------------------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public void FlushHistory()
        {

            //try
            //{
            //    if (_database == null)
            //    {
            //        _database = DatabaseFactory.CreateDatabase(_databaseName);
            //    }

            //    DbCommand insertCommand = _database.GetStoredProcCommand("FlushHistory");
            //    _database.ExecuteNonQuery(insertCommand);

            //}
            //catch (Exception ex)
            //{
            //    //TODO: error message
            //    Log2.Error("UpdateAgent Failed: FlushHistory SPC: {0}", ex.ToString());
            //    // _online = false;

            //}

        }
        //-----------------------------------------------------------------------------------------------------
        public void AddHistory(string agentName, string propertyName, string value, string qual, DateTime time)
        {
            string qualifiedAgentName;
            string qualifiedPropertyName;

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //if ((!_online) || (_agentPassPort == null))
            //{
            //    if (!_online)
            //        Log2.Error("AddHistory Offline");
                
            //    return;
            //}

            ////~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            //try
            //{
            //    if (_database == null)
            //    {
            //        _database = DatabaseFactory.CreateDatabase(_databaseName);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    //TODO: error message
            //    Log2.Error("AddHistory Failed: CreateDatabase: {0}", ex.ToString());
            //    // _online = false;

            //}

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            //if (_qualifiedPrefix == null)
            //    _qualifiedPrefix = "/" + _celestial + "/" + _collective + "/" + _community + "/" + _cluster + "/" + _colony;

            //qualifiedAgentName = _qualifiedPrefix + "/" + agentName;
            //qualifiedPropertyName = _qualifiedPrefix + "/" + agentName + "." + propertyName;

            qualifiedAgentName = _agentPassPort.FullName;
            qualifiedPropertyName = qualifiedAgentName + "." + propertyName;
            
            int agentStorageKey = qualifiedAgentName.GetHashCode();
            int qualifiedPropertyStorageKey = qualifiedPropertyName.GetHashCode();

            DateTime dataTime = time;
            string quality = qual;

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            //try
            //{

            //    DbCommand insertCommand = _database.GetStoredProcCommand("AddHistory");
            //    _database.AddInParameter(insertCommand, "@agentStorageKey", DbType.Int32, agentStorageKey);
            //    _database.AddInParameter(insertCommand, "@qualifiedPropertyStorageKey", DbType.Int32, qualifiedPropertyStorageKey);
            //    _database.AddInParameter(insertCommand, "@qualifiedAgentName", DbType.String, qualifiedAgentName);
            //    _database.AddInParameter(insertCommand, "@qualifiedPropertyName", DbType.String, qualifiedPropertyName);
            //    _database.AddInParameter(insertCommand, "@propertyName", DbType.String, propertyName);
            //    _database.AddInParameter(insertCommand, "@value", DbType.String, value);
            //    _database.AddInParameter(insertCommand, "@quality", DbType.String, quality);
            //    _database.AddInParameter(insertCommand, "@updateTime", DbType.DateTime, dataTime);

            //    _database.ExecuteNonQuery(insertCommand);

            //}
            //catch (Exception ex)
            //{
            //    //TODO: error message
            //    Log2.Error("AddHistory Failed: AddHistory SPC: {0}", ex.ToString());
            //   // _online = false;

            //}
        }
       }
    }
