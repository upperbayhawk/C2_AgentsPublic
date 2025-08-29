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
    /// NOT IMPLEMENTED - PLACE HOLDER
    /// Static Class
    /// Used as high speed shared memory for threads executing in the agent colony process
    /// </summary>
    public static class CommandCache
	{

        #region Methods
        /// <summary>
        /// Remove an object from the underlying storage
        /// </summary>
        /// <param name="objId">key for the object</param>
        public static void DeleteObject(string objId)
		{
			lock (_writeLock)
			{
				_messageTable.Remove(objId);
				_jsonMessageHashCodeTable.Remove(objId);
			}
		}

		/// <summary>
		/// Retrieve an object from the underlying storage
		/// </summary>
		/// <param name="objId">key for the object</param>
		/// <returns>object</returns>
		public static CommandVariable GetObject(string objId)
		{
			lock (_writeLock)
			{
				return (CommandVariable)_messageTable[objId];
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
				//same datavar, do nothing

				o.UpdateTime = DateTime.Now;
				o.ServerTime = DateTime.Now;
				o.Status = "ONLINE";
				o.Quality = "GOOD";

				if (!_messageTable.ContainsKey(objId))
				{
					Log2.Trace("Add Command Variable");
					_messageTable[objId] = o;
					_jsonMessageHashCodeTable[objId] = hashCode;
				}
				else
				{
					if ((int)_jsonMessageHashCodeTable[objId] != hashCode)
					{
						DataVariable dv = (DataVariable)_messageTable[objId];
						o.LastValue = dv.Value;
						o.LastValueTime = dv.UpdateTime;

						Log2.Trace("Update Command Variable");
						_messageTable[objId] = o;
						_jsonMessageHashCodeTable[objId] = hashCode;
					}
				}
			}
        }

		public static void DumpCache()
		{
			lock (_writeLock)
			{
				foreach (DictionaryEntry de in _messageTable)
				{
					CommandVariable dv = (CommandVariable)de.Value;
					Log2.Trace("CommandCache: {0}, {1}", (string)de.Key, (string)dv.Description);
				}
			}
		}
        public static void Cache2File(string fileName)
        {
            lock (_writeLock)
            {
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    JsonDataVariable json = new JsonDataVariable();
                    string s;
                    foreach (DictionaryEntry de in _messageTable)
                    {
                        DataVariable dv = (DataVariable)de.Value;
                        s = json.DataVariable2Json(dv);
                        Log2.Trace("CommandCache: {0}, {1}", (string)de.Key, (string)dv.Value);
                        writer.WriteLine(s);
                    }
                }
            }
        }
        #endregion

        #region Private State
        /// <summary>
        /// Private State
        /// </summary>
        private static Hashtable _messageTable = new Hashtable();
        private static Hashtable _jsonMessageHashCodeTable = new Hashtable();

        //private static string _name;
        private static object _writeLock = new object();
        #endregion
    }
}
