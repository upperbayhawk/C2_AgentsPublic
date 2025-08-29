/*
* **************************************************************************
* 
* FILE NAME: DBAdapter.cs
*
* Author: Dave Hardin
*
* COMPONENT HISTORY
* 
*   Date       Name    Reason for Change
* ----------  ------  ------------------------------------------------------
* 02/08/05     DBH		Initial version
* **************************************************************************
*/
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Data;
using System.Data.SqlClient;

using Upperbay.Core.Logging;
//using Upperbay.Core.Library;



namespace Upperbay.Agent.ColonyMatrix
{

	public class DBAdapter
	{

		// State Variables

		private string ConnectionString ="Integrated Security=SSPI;" +
			"Initial Catalog=mydb;" +
			"Data Source=localhost;";

		//private SqlDataReader reader = null; 
		private SqlConnection conn = null; 
		private SqlConnection connSQL = null; 
		private SqlCommand cmd = null;
		private string sql = null;

		/// <summary>
		/// 
		/// </summary>
		public DBAdapter()
		{
		}


		public bool ConnectDatabase()
		{
			try
			{
				ConnectionString = "Integrated Security=SSPI;" +
					"Initial Catalog=mydb;" +
					"Data Source=localhost;" +
					"Pooling=false;";

				conn = new SqlConnection(ConnectionString); 

				if( conn.State != ConnectionState.Open)
					conn.Open();  

				return (true);
			}
			catch(Exception e)
			{
                Log2.Error ("Exception: {0}",e.ToString());
				return(false);
			}
		}



		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool BuildDatabase()
		{
			try
			{
				ConnectionString = "Integrated Security=SSPI;" +
					"Initial Catalog=;" +
					"Data Source=localhost;" +
					"Pooling=false;";

				connSQL = new SqlConnection(ConnectionString); 

				if( connSQL.State != ConnectionState.Open)
					connSQL.Open();  

				string sql = "CREATE DATABASE mydb ON PRIMARY"
					+"(Name=test_data, filename = 'C:\\mysql\\mydb_data.mdf', size=3,"
					+"maxsize=5, filegrowth=10%)log on"
					+"(name=mydbb_log, filename='C:\\mysql\\mydb_log.ldf',size=3,"
					+"maxsize=20,filegrowth=1)" ;  

				Log2.Trace("Trying to Exec SQL: " + sql,Priority.Med);

				cmd = new SqlCommand(sql, connSQL);
			}
			catch (Exception e)
			{
				Log2.Trace(e.Message.ToString(),Priority.Med);
			}

			try
			{
				Log2.Trace("Executing SQL: " + sql,Priority.Med);
				cmd.ExecuteNonQuery();
			}
			catch(SqlException ae)
			{
				Log2.Trace(ae.Message.ToString(),Priority.Med);
			}
			finally
			{
				connSQL.Close();
			}

			return (true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool BuildTable()
		{
			// Open the connection

			try
			{
				sql = "CREATE TABLE myTable"+
					"(myId INTEGER CONSTRAINT PKeyMyId PRIMARY KEY,"+
					"myName CHAR(50), myAddress CHAR(255), myBalance FLOAT)" ;  

				cmd = new SqlCommand(sql, conn);
			}
			catch (Exception e)
			{
				Log2.Trace(e.Message.ToString(),Priority.Med);
			}



			try
			{
				cmd.ExecuteNonQuery();      
				// Adding records the table
				sql = "INSERT INTO myTable(myId, myName, myAddress, myBalance) "+
					"VALUES (1001, 'Puneet Nehra', 'A 449 Sect 19, DELHI', 23.98 ) " ;
				cmd = new SqlCommand(sql, conn);
				cmd.ExecuteNonQuery();  
				sql = "INSERT INTO myTable(myId, myName, myAddress, myBalance) "+
					"VALUES (1002, 'Anoop Singh', 'Lodi Road, DELHI', 353.64) " ;
				cmd = new SqlCommand(sql, conn);
				cmd.ExecuteNonQuery();    

				sql = "INSERT INTO myTable(myId, myName, myAddress, myBalance) "+
					"VALUES (1003, 'Rakesh M', 'Nag Chowk, Jabalpur M.P.', 43.43) " ;
				cmd = new SqlCommand(sql, conn);
				cmd.ExecuteNonQuery();    

				sql = "INSERT INTO myTable(myId, myName, myAddress, myBalance) "+
					"VALUES (1004, 'Madan Kesh', '4th Street, Lane 3, DELHI', 23.00) " ;
				cmd = new SqlCommand(sql, conn);
				cmd.ExecuteNonQuery();    

			}
			catch(SqlException ae)
			{
				Log2.Trace(ae.Message.ToString(),Priority.Med);
			}

			return (true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool DropDatabase()
		{
			try
			{
				ConnectionString = "Integrated Security=SSPI;" +
					"Initial Catalog=;" +
					"Data Source=localhost;" +
					"Pooling=false;";

				connSQL = new SqlConnection(ConnectionString); 

				if( connSQL.State != ConnectionState.Open)
					connSQL.Open();  


				//			string sql = "DROP TABLE myTable";  

				sql = "DROP DATABASE mydb";  
				Log2.Trace(sql,Priority.Med);

				cmd = new SqlCommand(sql, connSQL);

			}
			catch (Exception e)
			{
				Log2.Trace(e.Message.ToString(),Priority.Med);
			}


			try
			{
				Log2.Trace("Executing SQL: " + sql,Priority.Med);
				cmd.ExecuteNonQuery();
			}
			catch(SqlException ae)
			{
				Log2.Trace(ae.Message.ToString(),Priority.Med);
			}
			finally
			{
				connSQL.Close();
			}
			return(true);
		}

		public bool CloseDatabase()
		{
			try
			{
				conn.Close();
				return(true);
			}
			catch (Exception e)
			{
				Log2.Trace(e.Message.ToString(),Priority.Med);
			}
			return(false);
		}


	}// End Class
}

