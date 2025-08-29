using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

using Upperbay.Core.Logging;
//using Upperbay.Core.Library;


namespace Upperbay.Agent.ColonyMatrix
{
	/// <summary>
    /// Summary description for AgentCache.
	/// </summary>
	public class AgentDataSet
	{
        private static DataSet agentDataSet;
        private static DataTable agentDataTable;
        private static DataTable agentVariables;
        private static DataRelation agentRelations;
 
		private static object StaticLock = new object();

        public AgentDataSet()
		{
		}


                //Agents
                //    AgentID
                //    AgentName
                //    LogicalName
                //    BrowseName(UA)
                //    DisplayName(UA)
                //    NodeID(UA)
                //    Description
                //    MachineName
                //    ColonyName 
                //    CellName 
                //    ClassName
                //    NodeClass(UA)
                //    AssemblyPath
                //    SpeciesClassifier
                //    Role (Sensor, Actuator, Controller)
                //    Version
                //    TriggerType (Periodic, Event) 


        //    VariableID
        //    AgentID – Foreign Key
        //    Name
        //    DataType
        //    Quality
        //    Value
        //    SourceDatetime
        //    ServerDatetime
        //    ArraySize
        //    AccessLevel
        //    UserAccessLevel
        //    MinimumSamplingInterval


        /// <summary>
        /// 
        /// </summary>
		public void Initialize()
		{
			lock(StaticLock)
			{


                // Build the parent table

				try
				{
                    agentDataTable = new DataTable("AgentData");

					{
                        //Create AgentID and Add the DataTable
						DataColumn myDataColumn = new DataColumn();
						myDataColumn.DataType = Type.GetType("System.Int32");
						myDataColumn.ColumnName = "AgentID";
						myDataColumn.ReadOnly = true;
						myDataColumn.AllowDBNull = false;
						myDataColumn.Unique = true;

						myDataColumn.AutoIncrement = true;
						myDataColumn.AutoIncrementSeed = 1000;
						myDataColumn.AutoIncrementStep = 10;

                        agentDataTable.Columns.Add(myDataColumn);
					}

					{
						//Create AgentName Column and Add the DataTable

						DataColumn myDataColumn = new DataColumn();
						myDataColumn.DataType = Type.GetType("System.String");
						myDataColumn.ColumnName = "AgentName";
						myDataColumn.ReadOnly = false;
						myDataColumn.AllowDBNull = false;
						myDataColumn.Unique = true;

                        agentDataTable.Columns.Add(myDataColumn);
					}

					{
						//Create LogicalName Column and Add the DataTable
						DataColumn myDataColumn = new DataColumn();
						myDataColumn.DataType = Type.GetType("System.String");
						myDataColumn.ColumnName = "LogicalName";
						myDataColumn.ReadOnly = false;
						myDataColumn.AllowDBNull = false;
						myDataColumn.Unique = false;

                        agentDataTable.Columns.Add(myDataColumn);
					}

					{
						//Create BrowseName Column and Add the DataTable
						DataColumn myDataColumn = new DataColumn();
						myDataColumn.DataType = Type.GetType("System.String");
						myDataColumn.ColumnName = "BrowseName";
						myDataColumn.ReadOnly = false;
						myDataColumn.AllowDBNull = false;
						myDataColumn.Unique = false;

                        agentDataTable.Columns.Add(myDataColumn);
					}

                    {
						//Create DisplayName Column and Add the DataTable
						DataColumn myDataColumn = new DataColumn();
						myDataColumn.DataType = Type.GetType("System.String");
						myDataColumn.ColumnName = "DisplayName";
						myDataColumn.ReadOnly = false;
						myDataColumn.AllowDBNull = false;
						myDataColumn.Unique = false;

                        agentDataTable.Columns.Add(myDataColumn);
					}

					{
						//Create NodeID Column and Add the DataTable
						DataColumn myDataColumn = new DataColumn();
						myDataColumn.DataType = Type.GetType("System.Int32");
						myDataColumn.ColumnName = "NodeID";
						myDataColumn.ReadOnly = false;
						myDataColumn.AllowDBNull = false;
						myDataColumn.Unique = false;

                        agentDataTable.Columns.Add(myDataColumn);
					}
					// AckState
					{
						//Create Description Column and Add the DataTable
						DataColumn myDataColumn = new DataColumn();
						myDataColumn.DataType = Type.GetType("System.String");
						myDataColumn.ColumnName = "Description";
						myDataColumn.ReadOnly = false;
						myDataColumn.AllowDBNull = false;
						myDataColumn.Unique = false;

                        agentDataTable.Columns.Add(myDataColumn);
					}

					{
						//Create MachineName Column and Add the DataTable
						DataColumn myDataColumn = new DataColumn();
						myDataColumn.DataType = Type.GetType("System.String");
						myDataColumn.ColumnName = "MachineName";
						myDataColumn.ReadOnly = false;
						myDataColumn.AllowDBNull = false;
						myDataColumn.Unique = false;

                        agentDataTable.Columns.Add(myDataColumn);
					}

					{
						//Create ColonyName Column and Add the DataTable
						DataColumn myDataColumn = new DataColumn();
						myDataColumn.DataType = Type.GetType("System.String");
						myDataColumn.ColumnName = "ColonyName";
						myDataColumn.ReadOnly = false;
						myDataColumn.AllowDBNull = false;
						myDataColumn.Unique = false;

                        agentDataTable.Columns.Add(myDataColumn);
					}

					{
						//Create CellName Column and Add the DataTable
						DataColumn myDataColumn = new DataColumn();
						myDataColumn.DataType = Type.GetType("System.String");
						myDataColumn.ColumnName = "CellName";
						myDataColumn.ReadOnly = false;
						myDataColumn.AllowDBNull = false;
						myDataColumn.Unique = false;

                        agentDataTable.Columns.Add(myDataColumn);
					}

					{
						//Create ClassName Column and Add the DataTable
						DataColumn myDataColumn = new DataColumn();
						myDataColumn.DataType = Type.GetType("System.String");
						myDataColumn.ColumnName = "ClassName";
						myDataColumn.ReadOnly = false;
						myDataColumn.AllowDBNull = false;
						myDataColumn.Unique = false;

                        agentDataTable.Columns.Add(myDataColumn);
					}
					{
						//Create NodeClass Column and Add the DataTable
						DataColumn myDataColumn = new DataColumn();
						myDataColumn.DataType = Type.GetType("System.String");
						myDataColumn.ColumnName = "NodeClass";
						myDataColumn.ReadOnly = false;
						myDataColumn.AllowDBNull = false;
						myDataColumn.Unique = false;

                        agentDataTable.Columns.Add(myDataColumn);
					}
					{
						//Create AssemblyPath Column and Add the DataTable
						DataColumn myDataColumn = new DataColumn();
						myDataColumn.DataType = Type.GetType("System.String");
						myDataColumn.ColumnName = "AssemblyPath";
						myDataColumn.ReadOnly = false;
						myDataColumn.AllowDBNull = false;
						myDataColumn.Unique = false;

                        agentDataTable.Columns.Add(myDataColumn);
					}
					{
						//Create SpeciesClassifier Column and Add the DataTable
						DataColumn myDataColumn = new DataColumn();
						myDataColumn.DataType = Type.GetType("System.String");
						myDataColumn.ColumnName = "SpeciesClassifier";
						myDataColumn.ReadOnly = false;
						myDataColumn.AllowDBNull = false;
						myDataColumn.Unique = false;

                        agentDataTable.Columns.Add(myDataColumn);
					}

                    {
                        //Create Version Column and Add the DataTable
                        DataColumn myDataColumn = new DataColumn();
                        myDataColumn.DataType = Type.GetType("System.String");
                        myDataColumn.ColumnName = "Role";
                        myDataColumn.ReadOnly = false;
                        myDataColumn.AllowDBNull = false;
                        myDataColumn.Unique = false;

                        agentDataTable.Columns.Add(myDataColumn);
                    }

                    {
						//Create Version Column and Add the DataTable
						DataColumn myDataColumn = new DataColumn();
						myDataColumn.DataType = Type.GetType("System.String");
						myDataColumn.ColumnName = "Version";
						myDataColumn.ReadOnly = false;
						myDataColumn.AllowDBNull = false;
						myDataColumn.Unique = false;

                        agentDataTable.Columns.Add(myDataColumn);
					}

					{
						//Create TriggerType Column and Add the DataTable
						DataColumn myDataColumn = new DataColumn();
						myDataColumn.DataType = Type.GetType("System.String");
						myDataColumn.ColumnName = "TriggerType";
						myDataColumn.ReadOnly = false;
						myDataColumn.AllowDBNull = false;
						myDataColumn.Unique = false;

                        agentDataTable.Columns.Add(myDataColumn);
					}
                    
					DataColumn[] PK = new DataColumn[1];
                    PK[0] = agentDataTable.Columns["AgentID"];
                    agentDataTable.PrimaryKey = PK;
				}
				catch( Exception e)
				{
                    Log2.Error("Exception: {0}", e.ToString());
                    //m_Logger.LogError("Exception in Initialize: " + e.ToString());
				}

                //Build the child table

                //AgentVariables
                //    VariableID
                //    AgentID – Foreign Key
                //    Name
                //    DataType
                //    Quality
                //    Value
                //    SourceDatetime
                //    ServerDatetime
                //    ArraySize
                //    AccessLevel
                //    UserAccessLevel
                //    MinimumSamplingInterval

                try
                {
                    agentVariables = new DataTable("AgentVariables");

                    {
                        //Create VariableID and Add the DataTable
                        DataColumn myDataColumn = new DataColumn();
                        myDataColumn.DataType = Type.GetType("System.Int32");
                        myDataColumn.ColumnName = "VariableID";
                        myDataColumn.ReadOnly = true;
                        myDataColumn.AllowDBNull = false;
                        myDataColumn.Unique = true;

                        myDataColumn.AutoIncrement = true;
                        myDataColumn.AutoIncrementSeed = 1000;
                        myDataColumn.AutoIncrementStep = 10;

                        agentVariables.Columns.Add(myDataColumn);
                    }

                    {
                        //Create AgentID and Add the DataTable
                        DataColumn myDataColumn = new DataColumn();
                        myDataColumn.DataType = Type.GetType("System.Int32");
                        myDataColumn.ColumnName = "AgentID";
                        myDataColumn.ReadOnly = false;
                        myDataColumn.AllowDBNull = true;
                        myDataColumn.Unique = false;
                        
                        agentVariables.Columns.Add(myDataColumn);
                    }


                    {
                        //Create PropertyName Column and Add the DataTable

                        DataColumn myDataColumn = new DataColumn();
                        myDataColumn.DataType = Type.GetType("System.String");
                        myDataColumn.ColumnName = "Name";
                        myDataColumn.ReadOnly = false;
                        myDataColumn.AllowDBNull = true;
                        myDataColumn.Unique = false;

                        agentVariables.Columns.Add(myDataColumn);
                    }

                    {
                        //Create PropertyDataType Column and Add the DataTable

                        DataColumn myDataColumn = new DataColumn();
                        myDataColumn.DataType = Type.GetType("System.String");
                        myDataColumn.ColumnName = "DataType";
                        myDataColumn.ReadOnly = false;
                        myDataColumn.AllowDBNull = true;
                        myDataColumn.Unique = false;

                        agentVariables.Columns.Add(myDataColumn);
                    }
                    {
                        //Create PropertyQuality Column and Add the DataTable

                        DataColumn myDataColumn = new DataColumn();
                        myDataColumn.DataType = Type.GetType("System.String");
                        myDataColumn.ColumnName = "Quality";
                        myDataColumn.ReadOnly = false;
                        myDataColumn.AllowDBNull = true;
                        myDataColumn.Unique = false;

                        agentVariables.Columns.Add(myDataColumn);
                    }
                    {
                        //Create PropertyValue Column and Add the DataTable

                        DataColumn myDataColumn = new DataColumn();
                        myDataColumn.DataType = Type.GetType("System.Object");
                        myDataColumn.ColumnName = "Value";
                        myDataColumn.ReadOnly = false;
                        myDataColumn.AllowDBNull = true;
                        myDataColumn.Unique = false;

                        agentVariables.Columns.Add(myDataColumn);
                    }
                    {
                        //Create PropertySourceDatetime Column and Add the DataTable

                        DataColumn myDataColumn = new DataColumn();
                        myDataColumn.DataType = Type.GetType("System.String");
                        myDataColumn.ColumnName = "SourceDatetime";
                        myDataColumn.ReadOnly = false;
                        myDataColumn.AllowDBNull = true;
                        myDataColumn.Unique = false;

                        agentVariables.Columns.Add(myDataColumn);
                    }
                    {
                        //Create PropertyServerDatetime Column and Add the DataTable

                        DataColumn myDataColumn = new DataColumn();
                        myDataColumn.DataType = Type.GetType("System.String");
                        myDataColumn.ColumnName = "ServerDatetime";
                        myDataColumn.ReadOnly = false;
                        myDataColumn.AllowDBNull = true;
                        myDataColumn.Unique = false;

                        agentVariables.Columns.Add(myDataColumn);
                    }
                    {
                        //Create PropertyArraySize Column and Add the DataTable

                        DataColumn myDataColumn = new DataColumn();
                        myDataColumn.DataType = Type.GetType("System.String");
                        myDataColumn.ColumnName = "ArraySize";
                        myDataColumn.ReadOnly = false;
                        myDataColumn.AllowDBNull = true;
                        myDataColumn.Unique = false;

                        agentVariables.Columns.Add(myDataColumn);
                    }
                    {
                        //Create PropertyAccessLevel Column and Add the DataTable

                        DataColumn myDataColumn = new DataColumn();
                        myDataColumn.DataType = Type.GetType("System.String");
                        myDataColumn.ColumnName = "AccessLevel";
                        myDataColumn.ReadOnly = false;
                        myDataColumn.AllowDBNull = true;
                        myDataColumn.Unique = false;

                        agentVariables.Columns.Add(myDataColumn);
                    }
                    {
                        //Create PropertyUserAccessLevel Column and Add the DataTable

                        DataColumn myDataColumn = new DataColumn();
                        myDataColumn.DataType = Type.GetType("System.String");
                        myDataColumn.ColumnName = "UserAccessLevel";
                        myDataColumn.ReadOnly = false;
                        myDataColumn.AllowDBNull = true;
                        myDataColumn.Unique = false;

                        agentVariables.Columns.Add(myDataColumn);
                    }
                    {
                        //Create PropertyMinimumSamplingInterval Column and Add the DataTable

                        DataColumn myDataColumn = new DataColumn();
                        myDataColumn.DataType = Type.GetType("System.String");
                        myDataColumn.ColumnName = "MinimumSamplingInterval";
                        myDataColumn.ReadOnly = false;
                        myDataColumn.AllowDBNull = true;
                        myDataColumn.Unique = false;

                        agentVariables.Columns.Add(myDataColumn);
                    }
                    
                    DataColumn[] PK = new DataColumn[1];
                    PK[0] = agentVariables.Columns["VariableID"];
                    agentVariables.PrimaryKey = PK;

                }
                catch (Exception e)
                {
                    Log2.Error("Exception in Initialize: " + e.ToString());
                }

                /// Construct Dataset
                agentDataSet = new DataSet("AgentDataSet");

                agentDataSet.Tables.Add(agentDataTable);
                agentDataSet.Tables.Add(agentVariables);
                
       			//pkTerritories = dtTerritories.Constraints.Add("pkTerritories", dtTerritories.Columns["TerritoryID"], true);
			    //fkTerritories = dtTerritories.Constraints.Add("fkRegionID", dtRegion.Columns["RegionID"], dtTerritories.Columns["RegionID"]);
                //In our example database the two tables are related on field CustID. CustID is foreign key in Orders table. Add following code that establishes foreign key constraints between them. 

                //ForeignKeyConstraint fk;

                //fk = new ForeignKeyConstraint("fk", agentDataSet.Tables[0].Columns["AgentID"], agentDataSet.Tables[1].Columns["AgentID"]); 
                //fk.DeleteRule = Rule.Cascade;
                //fk.UpdateRule = Rule.Cascade;
                //agentDataSet.Tables(1).Constraints.Add(fk);
                //agentDataSet.EnforceConstraints = True;


                agentRelations = new DataRelation("agentDataSet", 
                    agentDataSet.Tables[0].Columns["AgentID"],
                    agentDataSet.Tables[1].Columns["AgentID"]);
                agentRelations.Nested = true;
                
                agentDataSet.Relations.Add(agentRelations);

                Log2.Trace("Initialize Completed");


			}
		}



//        public static string getalarmstatename(string sofoxtag, int ialarmtype)
//        {
//            string soalarmname;
//            tagtranslator tagtranslator = new tagtranslator();
//            soalarmname = sofoxtag + "." + tagtranslator.getfoxalarmtypestring(ialarmtype);
//            return soalarmname;
//        }



        public bool AddAgent(String agentName)
        {
            lock (StaticLock)
            {
                try
                {

                    // Add a new row to the database, initialize state and return the keyed index.
                    DataRow newRow = agentDataTable.NewRow();


                    //if ((QueryAlarmStateID(soAlarmName)) == -1)
                    {
                        newRow["AgentName"] = agentName;
                        newRow["LogicalName"] = agentName;
                        newRow["BrowseName"] = agentName;
                        newRow["DisplayName"] = agentName;
                        newRow["NodeID"] = 0;
                        newRow["Description"] = String.Empty;
                        newRow["MachineName"] = String.Empty;
                        newRow["ColonyName"] = String.Empty;
                        newRow["CellName"] = String.Empty;
                        newRow["ClassName"] = String.Empty;
                        newRow["NodeClass"] = String.Empty;
                        newRow["AssemblyPath"] = String.Empty;
                        newRow["SpeciesClassifier"] = String.Empty;
                        newRow["Role"] = String.Empty;
                        newRow["Version"] = String.Empty;
                        newRow["TriggerType"] = String.Empty;
                        agentDataTable.Rows.Add(newRow);
                        //m_Logger.LogDebug("AddAlarmState: " + soAlarmName);
                        Log2.Trace("Added " + agentName);
                        return true;
                    }
                    //else
                    //{
                    //    return false;
                    //}
                }
                catch (Exception e)
                {
                    Log2.Trace("Exception " + e.ToString());
                    return false;
                }
            }
        }

        //    VariableID
        //    AgentID – Foreign Key
        //    Name
        //    DataType
        //    Quality
        //    Value
        //    SourceDatetime
        //    ServerDatetime
        //    ArraySize
        //    AccessLevel
        //    UserAccessLevel
        //    MinimumSamplingInterval

        public bool AddAgentVariables(String agentName, String var)
        {
            DataRow agentDatum = null;
            Int32 myKey = 0;
            
            lock (StaticLock)
            {

                try
                {
                    String soFilter = "AgentName='" + agentName + "'";
                    DataRow[] agentData = agentDataTable.Select(soFilter);

                    if (agentData.Length > 0)
                    {
                        agentDatum = agentData[0];
                        myKey = (Int32)agentDatum["AgentID"];
                        Log2.Trace("AgentKey= " + agentDatum["AgentID"]);
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    Log2.Trace("Exception in DumpAgent: " + e.ToString());
                }

                
                try
                {
                    // Add a new row to the database, initialize state and return the keyed index.
                    DataRow newRow = agentVariables.NewRow();

                    //if ((QueryAlarmStateID(soAlarmName)) == -1)
                    {
                        newRow["AgentID"] = myKey;
                        newRow["Name"] = var;
                        newRow["DataType"] = "float";
                        newRow["Quality"] = "good";
                        newRow["Value"] = (Object)0.0000;
                        newRow["SourceDatetime"] = "now";
                        newRow["ServerDatetime"] = "now";
                        newRow["ArraySize"] = String.Empty;
                        newRow["AccessLevel"] = String.Empty;
                        newRow["UserAccessLevel"] = String.Empty;
                        newRow["MinimumSamplingInterval"] = String.Empty;
                        agentVariables.Rows.Add(newRow);
                        //m_Logger.LogDebug("AddAlarmState: " + soAlarmName);
                        Log2.Trace("Added " + agentName +"." + var);
                        
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Log2.Trace("Exception " + e.ToString());
                    return false;
                }
            }
        }


        public bool UpdateAgentVariable(String agentName, String var, Object val)
        {
            DataRow agentDatum = null;
            Int32 myKey = 0;

            lock (StaticLock)
            {

                try
                {
                    String soFilter = "AgentName='" + agentName + "'";
                    DataRow[] agentData = agentDataTable.Select(soFilter);

                    if (agentData.Length > 0)
                    {
                        agentDatum = agentData[0];
                        myKey = (Int32)agentDatum["AgentID"];
                        //Log2.Trace("AgentKey= " + agentDatum["AgentID"]);
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    Log2.Trace("Exception in UpdateAgentVariable: " + e.ToString());
                    return false;
                }


                try
                {
                    // Add a new row to the database, initialize state and return the keyed index.
                    DataRow[] newchildRows = agentDatum.GetChildRows(agentRelations);

                    for (int i = 0; i < newchildRows.Length; i++)
                    {
                        //— Grab the values for each child row of the first customer
                        DataRow oRow = newchildRows[i];
                        if (oRow["Name"].ToString() == var)
                        {
                            oRow["Value"] = (Object)val;
                           // Log2.Trace("UpdateAgentVariable: " + oRow["Value"].ToString()); 
                            return true;
                        }
                    }
                    return false;
                }
                catch (Exception e)
                {
                    Log2.Trace("Exception in UpdateAgentVariable: " + e.ToString());
                    return false;
                }
            }
        }


//        /// <summary>
//        /// Remove an existing row from the CurrentAlarmTable based on AlarmStateName
//        /// </summary>
//        /// <param name="soAlarmName"></param>
//        /// <returns></returns>
//        public static bool RemoveAlarmState( String soAlarmName )
//        {
//            lock(StaticLock)
//            {
				
//                try
//                {
//                    int iIndex = 0;
//                    String soFilter = "AlarmStateName='" + soAlarmName + "'";
//                    DataRow[] alarmData = currentAlarmTable.Select(soFilter);
//                    if (alarmData.Length > 0)
//                    {
//                        DataRow alarmDatum = alarmData[0];
//                        iIndex = (int)alarmDatum["AlarmID"];
//                        alarmDatum.Delete();
//                        currentAlarmTable.AcceptChanges();
//                        return true;
//                    }
//                    else
//                    {
//                        return false;
//                    }
//                }
//                catch ( Exception e)
//                {
//                    m_Logger.LogError("Exception in RemoveAlarmState: " + e.ToString());
//                    return false;
//                }
//            }
//        }


        public void RegenerateAgents()
        {
            Log2.Trace("Clearing Agents.");
            agentVariables.Clear();
            agentDataTable.Clear();

            Log2.Trace("Regenerating Agents.");
            agentDataTable.ReadXml("C:\\AGENTXML.XML");

        }





        public void DumpAgents()
        {


            agentDataTable.WriteXml("C:\\AGENTXML.XML");


            lock (StaticLock)
            {
                try
                {
                    DataRow[] currRows = agentDataTable.Select(null, null, DataViewRowState.CurrentRows);
                   

                    if (currRows.Length < 1)
                        Log2.Trace("No Current Rows Found");
                    else
                    {
                        foreach (DataRow myRow in currRows)
                        {
                            foreach (DataColumn myCol in agentDataTable.Columns)
                                Log2.Trace("\t" + myRow[myCol]);

                            Log2.Trace("\t" + myRow.RowState);

                            Log2.Trace("Getting Child Rows");

                            // Loop thru all the variables
                            DataRow[] oRows = myRow.GetChildRows(agentRelations);
                            //— Retrieve the child rows for the first agent
                            string sMsg = "The variables for the agent are: \n";
                            //— Loop through the child rows for the first agent
                            for (int i = 0; i < oRows.Length; i++)
                            {
                                //— Grab the values for each child row of the first customer
                                DataRow oRow = oRows[i];
                                sMsg += "\t" + oRow["Name"].ToString() +
                                        " " + oRow["Value"].ToString() +
                                         "\n";
                            }
                            //— Display the values of the child rows
                            Log2.Trace(sMsg);
                        }
                    }
                }
                catch (Exception e)
                {
                    Log2.Trace("Exception in DumpAgent: " + e.ToString());
                }
            }

        }
            





        //    lock (StaticLock)
        //    {
                
        //        try
        //        {
        //            String soFilter = "AgentName='" + agentName + "'";
        //            DataRow[] agentData = agentDataTable.Select(soFilter);

        //            if (agentData.Length > 0)
        //            {
        //                DataRow agentDatum = agentData[0];
        //                Log2.Trace("AgentDump= " + agentDatum["AgentName"]);
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            Log2.Trace("Exception in DumpAgent: " + e.ToString());
        //        }
        //    }
        //}

//        public static int QueryAlarmStateID( String soAlarmName )
//        {
//            lock(StaticLock)
//            {
//                int iStateID = -1;
//                try
//                {
//                    String soFilter = "AlarmStateName='" + soAlarmName + "'";
//                    DataRow[] alarmData = currentAlarmTable.Select(soFilter);
//                    if (alarmData.Length > 0)
//                    {
//                        DataRow alarmDatum = alarmData[0];
//                        iStateID = (int)alarmDatum["AlarmID"];
//                        return iStateID;
//                    }
//                    else
//                    {
//                        return -1;
//                    }
//                }
//                catch ( Exception e)
//                {
//                    m_Logger.LogError("Exception in QueryAlarmStateID: " + e.ToString());
//                    return -1;
//                }
//            }
//        }

//        public static int QueryAlarmStateWWInstanceID( String soAlarmName )
//        {
//            lock(StaticLock)
//            {
//                int iWWInstanceID = -1;
//                try
//                {
//                    String soFilter = "AlarmStateName='" + soAlarmName + "'";
//                    DataRow[] alarmData = currentAlarmTable.Select(soFilter);
//                    if (alarmData.Length > 0)
//                    {
//                        DataRow alarmDatum = alarmData[0];
//                        iWWInstanceID = (int)alarmDatum["WWInstanceID"];
//                        return iWWInstanceID;
//                    }
//                    else
//                    {
//                        return -1;
//                    }
//                }
//                catch ( Exception e)
//                {
//                    m_Logger.LogError("Exception in QueryAlarmStateID: " + e.ToString());
//                    return -1;
//                }
//            }
//        }



//        public static String QueryAlarmStateCompoundBlock( int iAlarmID )
//        {
//            lock(StaticLock)
//            {
//                String soCompoundBlock;
//                try
//                {
//                    String soFilter = "AlarmID='" + iAlarmID + "'";
//                    DataRow[] alarmData = currentAlarmTable.Select(soFilter);
//                    if (alarmData.Length > 0)
//                    {
//                        DataRow alarmDatum = alarmData[0];
//                        soCompoundBlock = (String)alarmDatum["Compound"] + ":" + alarmDatum["Block"];
//                        return soCompoundBlock;
//                    }
//                    else
//                    {
//                        return String.Empty;
//                    }
//                }
//                catch ( Exception e)
//                {
//                    m_Logger.LogError("Exception in QueryAlarmStateCompoundBlock: " + e.ToString());
//                    return String.Empty;
//                }
//            }
//        }


//        public static bool QueryAlarmStateACK( String soAlarmName)
//        {
//            lock(StaticLock)
//            {
//                bool bAckState = false;
//                try
//                {
//                    String soFilter = "AlarmStateName='" + soAlarmName + "'";
//                    DataRow[] alarmData = currentAlarmTable.Select(soFilter);
//                    if (alarmData.Length > 0)
//                    {
//                        DataRow alarmDatum = alarmData[0];
//                        bAckState = (bool)alarmDatum["AckState"];
//                        return bAckState;
//                    }
//                    else
//                    {
//                        return false;
//                    }
//                }
//                catch ( Exception e)
//                {
//                    m_Logger.LogError("Exception in QueryAlarmStateACK: " + e.ToString());
//                    return false;
//                }
//            }
//        }

//        public static bool QueryAlarmStateLocalAck( String soAlarmName, 
//                                                    out String soAckOprName,
//                                                    out String soAckNode,
//                                                    out String soAckComment)
//        {
//            lock(StaticLock)
//            {
//                bool bLocalAck = false;
//                try
//                {
//                    String soFilter = "AlarmStateName='" + soAlarmName + "'";
//                    DataRow[] alarmData = currentAlarmTable.Select(soFilter);
//                    if (alarmData.Length > 0)
//                    {
//                        DataRow alarmDatum = alarmData[0];
//                        bLocalAck = (bool)alarmDatum["LocalAck"];
//                        soAckOprName = (String)alarmDatum["AckOprName"];
//                        soAckNode = (String)alarmDatum["AckNode"];
//                        soAckComment = (String)alarmDatum["AckComment"];
//                        return bLocalAck;
//                    }
//                    else
//                    {
//                        soAckOprName = String.Empty;
//                        soAckNode = String.Empty;
//                        soAckComment = String.Empty;
//                        return false;
//                    }
//                }
//                catch ( Exception e)
//                {
//                    soAckOprName = String.Empty;
//                    soAckNode = String.Empty;
//                    soAckComment = String.Empty;
//                    m_Logger.LogError("Exception in QueryAlarmStateLocalAck: " + e.ToString());
//                    return false;
//                }
//            }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="iWWInstanceID"></param>
//        /// <param name="soAckOprName"></param>
//        /// <param name="soAckNode"></param>
//        /// <param name="soAckComment"></param>
//        /// <returns></returns>
//        public static bool QueryAlarmStateLocalAckByID( int iWWInstanceID, 
//            out String soAckOprName,
//            out String soAckNode,
//            out String soAckComment)
//        {
//            lock(StaticLock)
//            {
//                bool bLocalAck = false;
//                try
//                {
//                    String soFilter = "WWInstanceID='" + iWWInstanceID + "'";
//                    DataRow[] alarmData = currentAlarmTable.Select(soFilter);
//                    if (alarmData.Length > 0)
//                    {
//                        DataRow alarmDatum = alarmData[0];
//                        bLocalAck = (bool)alarmDatum["LocalAck"];
//                        soAckOprName = (String)alarmDatum["AckOprName"];
//                        soAckNode = (String)alarmDatum["AckNode"];
//                        soAckComment = (String)alarmDatum["AckComment"];
//                        return bLocalAck;
//                    }
//                    else
//                    {
//                        soAckOprName = String.Empty;
//                        soAckNode = String.Empty;
//                        soAckComment = String.Empty;
//                        return false;
//                    }
//                }
//                catch ( Exception e)
//                {
//                    soAckOprName = String.Empty;
//                    soAckNode = String.Empty;
//                    soAckComment = String.Empty;
//                    m_Logger.LogError("Exception in QueryAlarmStateLocalAck: " + e.ToString());
//                    return false;
//                }
//            }
//        }

//        /// <summary>
//        /// Get the BLOCK name associated with a WWInstanceID
//        /// </summary>
//        /// <param name="iWWInstanceID"></param>
//        /// <param name="sBlock"></param>
//        /// <returns></returns>
//        public static bool QueryAlarmStateBlock( int iWWInstanceID, 
//            out String sBlock)
//        {
//            lock(StaticLock)
//            {
//                try
//                {
//                    String soFilter = "WWInstanceID='" + iWWInstanceID + "'";
//                    DataRow[] alarmData = currentAlarmTable.Select(soFilter);
//                    if (alarmData.Length > 0)
//                    {
//                        DataRow alarmDatum = alarmData[0];
//                        sBlock = (String)alarmDatum["Block"];
//                        return true;
//                    }
//                    else
//                    {
//                        sBlock = String.Empty;
//                        return false;
//                    }
//                }
//                catch ( Exception e)
//                {
//                    sBlock = String.Empty;
//                    m_Logger.LogError("Exception in QueryAlarmStateBlock: " + e.ToString());
//                    return false;
//                }
//            }
//        }




//        public static bool QueryAlarmStateRTN( String soAlarmName )
//        {
//            lock(StaticLock)
//            {
//                bool bRtnState = false;
//                try
//            {
//                    String soFilter = "AlarmStateName='" + soAlarmName + "'";
//                    DataRow[] alarmData = currentAlarmTable.Select(soFilter);
//                    if (alarmData.Length > 0)
//                    {
//                        DataRow alarmDatum = alarmData[0];
//                        bRtnState = (bool)alarmDatum["RtnState"];
//                        return bRtnState;
//                    }
//                    else
//                    {
//                        return false;
//                    }
//                }
//                catch ( Exception e)
//                {
//                    m_Logger.LogError("Exception in QueryAlarmStateRTN: " + e.ToString());
//                    return false;
//                }
//            }
//        }


//        public static bool QueryAlarmStateDIS( String soAlarmName )
//        {
//            lock(StaticLock)
//            {
//                bool bDisableState = false;
//                try
//                {
//                    String soFilter = "AlarmStateName='" + soAlarmName + "'";
//                    DataRow[] alarmData = currentAlarmTable.Select(soFilter);
//                    if (alarmData.Length > 0)
//                    {
//                        DataRow alarmDatum = alarmData[0];
//                        bDisableState = (bool)alarmDatum["DisableState"];
//                        return bDisableState;
//                    }
//                    else
//                    {
//                        return false;
//                    }
//                }
//                catch ( Exception e)
//                {
//                    m_Logger.LogError("Exception in QueryAlarmState: " + e.ToString());
//                    return false;
//                }
//            }
//        }


//        public static bool UpdateAlarmStateWWInstanceID(String soAlarmName, int iWWInstanceID)
//        {
//            lock(StaticLock)
//            {

//                try
//                {
//                    String soFilter = "AlarmStateName='" + soAlarmName + "'";
//                    DataRow[] alarmData = currentAlarmTable.Select(soFilter);
//                    if (alarmData.Length > 0)
//                    {
//                        DataRow alarmDatum = alarmData[0];
//                        alarmDatum["WWInstanceID"] = iWWInstanceID;
//                        return true;
//                    }
//                    else
//                    {
//                        return false;
//                    }
//                }
//                catch ( Exception e)
//                {
//                    m_Logger.LogError("Exception in UpdateAlarmStateWWInstanceID: " + e.ToString());
//                    return false;
//                }
//            }

//        }

//        public static bool UpdateAlarmStateWWInstanceID(int iAlarmID, int iWWInstanceID)
//        {
//            lock(StaticLock)
//            {

//                try
//                {
//                    String soFilter = "AlarmID='" + iAlarmID + "'";
//                    DataRow[] alarmData = currentAlarmTable.Select(soFilter);
//                    if (alarmData.Length > 0)
//                    {
//                        DataRow alarmDatum = alarmData[0];
//                        alarmDatum["WWInstanceID"] = iWWInstanceID;
//                        return true;
//                    }
//                    else
//                    {
//                        return false;
//                    }
//                }
//                catch ( Exception e)
//                {
//                    m_Logger.LogError("Exception in UpdateAlarmStateWWInstanceID: " + e.ToString());
//                    return false;
//                }
//            }

//        }


//        public static bool UpdateAlarmStateNewWWInstanceID(int iOldWWInstanceID, int iNewWWInstanceID)
//        {
//            lock(StaticLock)
//            {

//                try
//                {
//                    String soFilter = "WWInstanceID='" + iOldWWInstanceID + "'";
//                    DataRow[] alarmData = currentAlarmTable.Select(soFilter);
//                    if (alarmData.Length > 0)
//                    {
//                        DataRow alarmDatum = alarmData[0];
//                        alarmDatum["WWInstanceID"] = iNewWWInstanceID;
//                        return true;
//                    }
//                    else
//                    {
//                        return false;
//                    }
//                }
//                catch ( Exception e)
//                {
//                    m_Logger.LogError("Exception in UpdateAlarmStateNewWWInstanceID: " + e.ToString());
//                    return false;
//                }
//            }

//        }

		


//        public static bool UpdateAlarmStateACK(String soAlarmName, bool bAckState)
//        {
//            lock(StaticLock)
//            {

//                try
//                {
//                    String soFilter = "AlarmStateName='" + soAlarmName + "'";
//                    DataRow[] alarmData = currentAlarmTable.Select(soFilter);
//                    if (alarmData.Length > 0)
//                    {
//                        DataRow alarmDatum = alarmData[0];
//                        alarmDatum["AckState"] = bAckState;
//                        return true;
//                    }
//                    else
//                    {
//                        return false;
//                    }
//                }
//                catch ( Exception e)
//                {
//                    m_Logger.LogError("Exception in UpdateAlarmStateACK: " + e.ToString());
//                    return false;
//                }
//            }

//        }


//        public static bool UpdateAlarmStateACK(int iWWInstanceID, bool bAckState)
//        {
//            lock(StaticLock)
//            {
//                try
//                {
//                    String soFilter = "WWInstanceID='" + iWWInstanceID + "'";
//                    DataRow[] alarmData = currentAlarmTable.Select(soFilter);
//                    if (alarmData.Length > 0)
//                    {
//                        DataRow alarmDatum = alarmData[0];
//                        alarmDatum["AckState"] = bAckState;
//                        return true;
//                    }
//                    else
//                    {
//                        return false;
//                    }
//                }
//                catch ( Exception e)
//                {
//                    m_Logger.LogError("Exception in UpdateAlarmStateACK: " + e.ToString());
//                    return false;
//                }
//            }

//        }


//        public static bool UpdateAlarmStateLocalAck(int iWWInstanceID, 
//                                                    bool bLocalAck,
//                                                    String sAckOprName,
//                                                    String sAckNode,
//                                                    String sAckComment)
//        {
//            lock(StaticLock)
//            {
//                try
//                {
//                    String soFilter = "WWInstanceID='" + iWWInstanceID + "'";
//                    DataRow[] alarmData = currentAlarmTable.Select(soFilter);
//                    if (alarmData.Length > 0)
//                    {
//                        DataRow alarmDatum = alarmData[0];
//                        alarmDatum["LocalAck"] = bLocalAck;
//                        alarmDatum["AckOprName"] = sAckOprName;
//                        alarmDatum["AckNode"] = sAckNode;
//                        alarmDatum["AckComment"] = sAckComment;
//                        return true;
//                    }
//                    else
//                    {
//                        return false;
//                    }
//                }
//                catch ( Exception e)
//                {
//                    m_Logger.LogError("Exception in UpdateAlarmStateLocalAck: " + e.ToString());
//                    return false;
//                }
//            }

//        }




//        public static bool UpdateAlarmStateRTN(String soAlarmName, bool bRtnState)
//        {
//            lock(StaticLock)
//            {
//                try
//                {
//                    String soFilter = "AlarmStateName='" + soAlarmName + "'";
//                    DataRow[] alarmData = currentAlarmTable.Select(soFilter);
//                    if (alarmData.Length > 0)
//                    {
//                        DataRow alarmDatum = alarmData[0];
//                        alarmDatum["RtnState"] = bRtnState;
//                        return true;
//                    }
//                    else
//                    {
//                        return false;
//                    }

//                }
//                catch ( Exception e)
//                {
//                    m_Logger.LogError("Exception in UpdateAlarmStateRTN: " + e.ToString());
//                    return false;
//                }
//            }
//        }

//        public static bool UpdateAlarmStateRTN(int iAlarmID, bool bRtnState)
//        {
//            lock(StaticLock)
//            {
//                try
//                {
//                    String soFilter = "WWInstanceID='" + iAlarmID + "'";
//                    DataRow[] alarmData = currentAlarmTable.Select(soFilter);
//                    if (alarmData.Length > 0)
//                    {
//                        DataRow alarmDatum = alarmData[0];
//                        alarmDatum["RtnState"] = bRtnState;
//                        return true;
//                    }
//                    else
//                    {
//                        return false;
//                    }

//                }
//                catch ( Exception e)
//                {
//                    m_Logger.LogError("Exception in UpdateAlarmStateRTN: " + e.ToString());
//                    return false;
//                }
//            }
//        }



//        public static bool UpdateAlarmStateDIS(String soAlarmName, bool bDisableState)
//        {
//            lock(StaticLock)
//            {
//                try
//                {
//                    String soFilter = "AlarmStateName='" + soAlarmName + "'";
//                    DataRow[] alarmData = currentAlarmTable.Select(soFilter);
//                    if (alarmData.Length > 0)
//                    {
//                        DataRow alarmDatum = alarmData[0];
//                        alarmDatum["DisableState"] = bDisableState;
//                        return true;
//                    }
//                    else
//                    {
//                        return false;
//                    }
//                }
//                catch ( Exception e)
//                {
//                    m_Logger.LogError("Exception in UpdateAlarmStateDIS: " + e.ToString());
//                    return false;
//                }
//            }

//        }

//        public static bool UpdateAlarmStateDIS(int iWWInstanceID, bool bDisableState)
//        {
//            lock(StaticLock)
//            {
//                try
//                {
//                    String soFilter = "WWInstanceID='" + iWWInstanceID + "'";
//                    DataRow[] alarmData = currentAlarmTable.Select(soFilter);
//                    if (alarmData.Length > 0)
//                    {
//                        DataRow alarmDatum = alarmData[0];
//                        alarmDatum["DisableState"] = bDisableState;
//                        return true;
//                    }
//                    else
//                    {
//                        return false;
//                    }
//                }
//                catch ( Exception e)
//                {
//                    m_Logger.LogError("Exception in UpdateAlarmStateDIS: " + e.ToString());
//                    return false;
//                }
//            }

//        }

//        public static bool PurgeAlarmStates()
//        {
//            lock(StaticLock)
//            {
//                //Remove an existing row from the database based on AlarmStateName
//                try
//                {
//                    String soFilter = "AlarmID>0";
//                    DataRow[] alarmData = currentAlarmTable.Select(soFilter);
//                    if (alarmData.Length > 0)
//                    {
//                        for (int i = 0 ; i < alarmData.Length; i++)
//                        {
//                            DataRow alarmDatum = alarmData[i];
//                            if ( ((bool)(alarmDatum["RtnState"]) == true) &&
//                                 ((bool)(alarmDatum["AckState"]) == true) &&
//                                 ((bool)(alarmDatum["DisableState"]) == false) )
//                                alarmDatum.Delete();
//                        }
//                        currentAlarmTable.AcceptChanges();
//                        return true;
//                    }
//                    else
//                    {
//                        return false;
//                    }
//                }
//                catch ( Exception e)
//                {
//                    m_Logger.LogError("Exception in RemoveAlarmState: " + e.ToString());
//                    return false;
//                }
//            }
//        }

//        public static bool EmptyAlarmStates()
//        {
//            lock(StaticLock)
//            {
//                //Remove an existing row from the database based on AlarmStateName
//                try
//                {
//                    String soFilter = "AlarmID>0";
//                    DataRow[] alarmData = currentAlarmTable.Select(soFilter);
//                    if (alarmData.Length > 0)
//                    {
//                        for (int i = 0 ; i < alarmData.Length; i++)
//                        {
//                            DataRow alarmDatum = alarmData[i];
//                            alarmDatum.Delete();
//                        }
//                        currentAlarmTable.AcceptChanges();
//                        return true;
//                    }
//                    else
//                    {
//                        return false;
//                    }
//                }
//                catch ( Exception e)
//                {
//                    m_Logger.LogError("Exception in RemoveAlarmState: " + e.ToString());
//                    return false;
//                }
//            }
//        }


//        public static void GetAlarmStateTableCompounds(String soCompound, 
//            out String[] saBlocks, 
//            out int[] iInstanceIDs)
//        {
//            lock(StaticLock)
//            {
//                try
//                {
//                    String soFilter = "Compound='" + soCompound + "'";
//                    DataRow[] alarmData = currentAlarmTable.Select(soFilter);
//                    String[] saMyBlocks = new String[alarmData.Length];
//                    int[] iMyInstanceIds = new int[alarmData.Length];
//                    if (alarmData.Length < 1)
//                    {
//                        m_Logger.LogDebug("GetAlarmStateTableCompounds: No Current Rows Found");
//                        saBlocks = saMyBlocks;
//                        iInstanceIDs = iMyInstanceIds;
//                        return;
//                    }
//                    else
//                    {
//                        TagTranslator tagTranslator = new TagTranslator();
//                        for ( int i = 0 ; i < alarmData.Length ; i++)
//                        {
//                            DataRow alarmDatum = alarmData[i];
//                            String soMyFoxBlock = 
//                                alarmDatum["Compound"] + ":" + alarmDatum["Block"] + "." + tagTranslator.GetFoxAlarmTypeString((int)alarmDatum["AlarmType"]);
//                            saMyBlocks[i] = soMyFoxBlock;
//                            iMyInstanceIds[i] = (int)alarmDatum["WWInstanceID"];
//                        }
//                        saBlocks = saMyBlocks;
//                        iInstanceIDs = iMyInstanceIds;
//                        return;
//                    }
//                }
//                catch ( Exception e)
//                {
//                    m_Logger.LogError("Exception in DumpAlarmStateTable: " + e.ToString());
//                    String[] saNullBlocks = new String[0];
//                    int[] iNullInstanceIds = new int[0];
//                    saBlocks = saNullBlocks; iInstanceIDs = iNullInstanceIds;
//                    return;
//                }
//            }// End Lock
//        }//End


//        public static void GetAlarmStateTableBlocks(String soBlock, 
//            out String[] saBlocks, 
//            out int[] iInstanceIDs)
//        {
//            lock(StaticLock)
//            {
//                try
//                {
//                    String soFilter = "Block='" + soBlock + "'";
//                    DataRow[] alarmData = currentAlarmTable.Select(soFilter);
//                    String[] saMyBlocks = new String[alarmData.Length];
//                    int[] iMyInstanceIds = new int[alarmData.Length];
//                    if (alarmData.Length < 1)
//                    {
//                        m_Logger.LogDebug("GetAlarmStateTableBlocks: No Current Rows Found");
//                        saBlocks = saMyBlocks;
//                        iInstanceIDs = iMyInstanceIds;
//                        return;
//                    }
//                    else
//                    {
//                        TagTranslator tagTranslator = new TagTranslator();
//                        for ( int i = 0 ; i < alarmData.Length ; i++)
//                        {
//                            DataRow alarmDatum = alarmData[i];
//                            String soMyFoxBlock = 
//                                alarmDatum["Compound"] + ":" + alarmDatum["Block"] + "." + tagTranslator.GetFoxAlarmTypeString((int)alarmDatum["AlarmType"]);
//                            saMyBlocks[i] = soMyFoxBlock;
//                            iMyInstanceIds[i] = (int)alarmDatum["WWInstanceID"];
//                        }
//                        saBlocks = saMyBlocks;
//                        iInstanceIDs = iMyInstanceIds;
//                        return;
//                    }
//                }
//                catch ( Exception e)
//                {
//                    m_Logger.LogError("Exception in DumpAlarmStateTable: " + e.ToString());
//                    String[] saNullBlocks = new String[0];
//                    int[] iNullInstanceIds = new int[0];
//                    saBlocks = saNullBlocks; iInstanceIDs = iNullInstanceIds;
//                    return;
//                }
//            }// End Lock
//        }//End



//        public static void GetAlarmStateTableBlocks(int iAckInstanceID, 
//            out String[] saBlocks, 
//            out int[] iInstanceIDs)
//        {
//            lock(StaticLock)
//            {
//                try
//                {
//                    String soFilter = "WWInstanceID='" + iAckInstanceID + "'";
//                    DataRow[] alarmData = currentAlarmTable.Select(soFilter);
//                    String[] saMyBlocks = new String[alarmData.Length];
//                    int[] iMyInstanceIds = new int[alarmData.Length];
//                    if (alarmData.Length < 1)
//                    {
//                        m_Logger.LogDebug("GetAlarmStateTableBlocks: No Current Rows Found");
//                        saBlocks = saMyBlocks;
//                        iInstanceIDs = iMyInstanceIds;
//                        return;
//                    }
//                    else
//                    {
//                        TagTranslator tagTranslator = new TagTranslator();
//                        for ( int i = 0 ; i < alarmData.Length ; i++)
//                        {
//                            DataRow alarmDatum = alarmData[i];
//                            String soMyFoxBlock = 
//                                alarmDatum["Compound"] + ":" + alarmDatum["Block"] + "." + tagTranslator.GetFoxAlarmTypeString((int)alarmDatum["AlarmType"]);
//                            saMyBlocks[i] = soMyFoxBlock;
//                            iMyInstanceIds[i] = (int)alarmDatum["WWInstanceID"];
//                            m_Logger.LogDebug("soMyFoxBlock: " + saMyBlocks[i] + " (" + iMyInstanceIds[i] + ")");
//                        }
//                        saBlocks = saMyBlocks;
//                        iInstanceIDs = iMyInstanceIds;
//                        return;
//                    }
//                }
//                catch ( Exception e)
//                {
//                    m_Logger.LogError("Exception in DumpAlarmStateTable: " + e.ToString());
//                    String[] saNullBlocks = new String[0];
//                    int[] iNullInstanceIds = new int[0];
//                    saBlocks = saNullBlocks; iInstanceIDs = iNullInstanceIds;
//                    return;
//                }
//            }// End Lock
//        }//End





//        public static void DumpAlarmStateTable()
//        {
//            lock(StaticLock)
//            {
//                try
//                {
//                    DataRow[] currRows = currentAlarmTable.Select(null, null, DataViewRowState.CurrentRows);

//                    if (currRows.Length < 1 )
//                        m_Logger.LogDebug("No Current Rows Found");
//                    else
//                    {

////						foreach (DataColumn myCol in currentAlarmTable.Columns)
////							m_Logger.LogDebug("\t" + myCol.ColumnName);
////						m_Logger.LogDebug("\tRowState");

//                        foreach (DataRow myRow in currRows)
//                        {
//                            foreach (DataColumn myCol in currentAlarmTable.Columns)
//                                m_Logger.LogDebug("\t" + myRow[myCol]);

//                            m_Logger.LogDebug("\t" + myRow.RowState);
//                        }
//                    }			
//                }
//                catch ( Exception e)
//                {
//                    m_Logger.LogError("Exception in DumpAlarmStateTable: " + e.ToString());
//                }
//            }

//        }

//        /// <summary>
//        /// Test Method
//        /// </summary>
//        public static void PopulateAlarmStateTable()
//        {
//                AddAlarmState("CMP1:BLK1", 1 );
//                AddAlarmState("CMP2:BLK2", 2 );
//                AddAlarmState("CMP3:BLK3", 3 );
//                AddAlarmState("CMP4:BLK4", 4 );
//                AddAlarmState("CMP5:BLK5", 5 );

//        }

//        /// <summary>
//        /// Test Method
//        /// </summary>
//        public static void UpdateAlarmStateTable()
//        {

//            UpdateAlarmStateACK(GetAlarmStateName( "CMP1:BLK1", 1 ), true);
//            UpdateAlarmStateRTN(GetAlarmStateName( "CMP1:BLK1", 1 ), true);
//            UpdateAlarmStateDIS(GetAlarmStateName( "CMP1:BLK1", 1 ), true);

//            UpdateAlarmStateACK(GetAlarmStateName( "CMP2:BLK2", 2 ), true);
//            UpdateAlarmStateRTN(GetAlarmStateName( "CMP2:BLK2", 2 ), true);
//            UpdateAlarmStateDIS(GetAlarmStateName( "CMP2:BLK2", 2 ), true);

//            UpdateAlarmStateACK(GetAlarmStateName( "CMP3:BLK3", 3 ), true);
//            UpdateAlarmStateRTN(GetAlarmStateName( "CMP3:BLK3", 3 ), true);
//            UpdateAlarmStateDIS(GetAlarmStateName( "CMP3:BLK3", 3 ), true);

//            UpdateAlarmStateACK(GetAlarmStateName( "CMP4:BLK4", 4 ), true);
//            UpdateAlarmStateRTN(GetAlarmStateName( "CMP4:BLK4", 4 ), true);
//            UpdateAlarmStateDIS(GetAlarmStateName( "CMP4:BLK4", 4 ), true);

//            UpdateAlarmStateACK(GetAlarmStateName( "CMP5:BLK5", 5 ), true);
//            UpdateAlarmStateRTN(GetAlarmStateName( "CMP5:BLK5", 5 ), true);
//            UpdateAlarmStateDIS(GetAlarmStateName( "CMP5:BLK5", 5 ), true);

//        }

//        /// <summary>
//        /// Test Method
//        /// </summary>
//        public static void RemoveSomeAlarmStates()
//        {
//            RemoveAlarmState(GetAlarmStateName( "CMP1:BLK1", 1 ));
//            RemoveAlarmState(GetAlarmStateName( "CMP2:BLK2", 2 ));
//        }

//        /// <summary>
//        /// Test Method
//        /// </summary>
//        public static void RemoveAlarmStateTable()
//        {
//            RemoveAlarmState(GetAlarmStateName( "CMP1:BLK1", 1 ));
//            RemoveAlarmState(GetAlarmStateName( "CMP2:BLK2", 2 ));
//            RemoveAlarmState(GetAlarmStateName( "CMP3:BLK3", 3 ));
//            RemoveAlarmState(GetAlarmStateName( "CMP4:BLK4", 4 ));
//            RemoveAlarmState(GetAlarmStateName( "CMP5:BLK5", 5 ));
//        }

	}
}
