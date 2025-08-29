//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
using System;
using System.Collections;
using Upperbay.Core.Logging;
using Upperbay.Agent.Interfaces;
using System.IO;
using Upperbay.Worker.JSON;

namespace Upperbay.Agent.ColonyMatrix
{

    /// <summary>
    /// Static Class
    /// Used as high speed shared memory for threads executing in the agent colony process
    /// </summary>
    public static class DataVariableCache
	{

        //TODO: Convert to ConcurrentDictionary

        #region Methods

        /// <summary>
        /// Remove an object from the underlying storage
        /// </summary>
        /// <param name="objId">key for the object</param>
        public static void DeleteObject(string objId)
		{
			lock (_writeLock)
			{
				_dataVariableTable.Remove(objId);
				_jsonHashCodeTable.Remove(objId);
			}
		}

		/// <summary>
		/// Retrieve an object from the underlying storage
		/// </summary>
		/// <param name="objId">key for the object</param>
		/// <returns>object</returns>
		public static DataVariable GetObject(string objId)
		{
			lock (_writeLock)
			{
				return (DataVariable)_dataVariableTable[objId];
			}
		}
        /// <summary>
        /// Update an object to the underlying storage
        /// </summary>
        /// <param name="objId">key for the object</param>
        /// <param name="o">object</param>
        public static void PutObject(string objId, DataVariable o, int hashCode)
        {
			lock (_writeLock)
			{

				if (o.ChangeFlag)
				{
					o.ChangeFlag = false;
					// Change Happened
					Log2.Trace("DataVar Change Detected: {0}, {1}", o.TagName, o.Value);
				}
				if (!_dataVariableTable.ContainsKey(objId))
				{
					Log2.Trace("Add DataVar");
					_dataVariableTable[objId] = o;
					_jsonHashCodeTable[objId] = hashCode;
				}
				else
				{
					if ((int)_jsonHashCodeTable[objId] != hashCode)
					{
						DataVariable dv = (DataVariable)_dataVariableTable[objId];

						Log2.Trace("Update DataVar");
						_dataVariableTable[objId] = o;
						_jsonHashCodeTable[objId] = hashCode;
					}
				}
			}
        }

        /// <summary>
        /// 
        /// </summary>
		public static void DumpCache()
		{
			lock (_writeLock)
			{
				foreach (DictionaryEntry de in _dataVariableTable)
				{
					DataVariable dv = (DataVariable)de.Value;
					Log2.Trace("DATACACHE: {0}, {1}", (string)de.Key, (string)dv.Value);
				}
			}
		}
	
        /// <summary>
        /// 
        /// </summary>
		public static void ProcessCache()
		{
			lock (_writeLock)
			{
				try
				{
					if (_dataVariableTable.Count > 0)
					{
						
						foreach (DictionaryEntry de in _dataVariableTable)
						{
							DataVariable dv = (DataVariable)de.Value;
							//Do Something Useful
						}
					}
				}
				catch (Exception ex)
				{
					Log2.Error("CACHE Exception: {0}", ex);
				}
			}
		}


        /// <summary>
        /// TODO!
        /// </summary>
        /// <param name="fileName"></param>
		//public static void LoadCache(string filename)
		//{
		//	lock (_writeLock)
		//	{
  //              try
  //              {

  //                  using (StreamReader writer = new StreamReader(filename))
  //                  {
  //                      JsonDataVariable json = new JsonDataVariable();
  //                      string s;
  //                      foreach (DictionaryEntry de in _dataVariableTable)
  //                      {
  //                          DataVariable dv = (DataVariable)de.Value;
  //                          s = json.DataVariable2Json(dv);
  //                          Log2.Trace("DATAFILE: {0}, {1}", (string)de.Key, (string)dv.Value);
  //                          writer.WriteLine(s);
  //                      }
  //                  }
  //              }
  //              catch (Exception ex)
  //              {

  //                  Log2.Trace("DataVariableCache.Cache2File: File Not Found: {0}", filename);
  //              }


  //              foreach (DictionaryEntry de in _dataVariableTable)
  //              {
  //                  DataVariable dv = (DataVariable)de.Value;
  //                  Log2.Trace("DATACACHE: {0}, {1}", (string)de.Key, (string)dv.Value);
  //              }
  //          }
		//}



        
		public static void Cache2File(string fileName)
		{
			lock (_writeLock)
			{
				try
				{
					using (StreamWriter writer = new StreamWriter(fileName))
					{
						JsonDataVariable json = new JsonDataVariable();
						string s;
						foreach (DictionaryEntry de in _dataVariableTable)
						{
							DataVariable dv = (DataVariable)de.Value;
							s = json.DataVariable2Json(dv);
							Log2.Trace("DATAFILE: {0}, {1}", (string)de.Key, (string)dv.Value);
							writer.WriteLine(s);
						}
					}
				}
				catch(Exception ex)
                {
					Log2.Trace("DataVariableCache.Cache2File: File Not Found: {0} {1}", fileName, ex);
				}
			}
		}

        #endregion

        #region Private State
        /// <summary>
        /// Private State
        /// </summary>
        private static Hashtable _dataVariableTable = new Hashtable();
        private static Hashtable _jsonHashCodeTable = new Hashtable();
        private static object _writeLock = new object();
        #endregion

    }
}
