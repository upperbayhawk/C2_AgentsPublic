using System;
using System.Collections;
using Upperbay.Core.Library;
using Upperbay.Core.Logging;
using Upperbay.Agent.Interfaces;
using System.IO;
using Upperbay.Worker.JSON;

namespace Upperbay.Agent.ColonyMatrix
{
	/// <summary>
	/// the sample cache strategy implementation which 
	/// shows how you create an pluggable component for SAF.Cache 
	/// to customize the way object is cahced and retrieved.
	/// </summary>
	public static class MessageCache
	{
		// tagname/DataVariable
		private static Hashtable _meassageTable = new Hashtable();
		//tagname/Json hashcode
		private static Hashtable _jsonMessageHashCodeTable = new Hashtable();

		//private static string _name;
		private static object _writeLock = new object();


        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        //public void Initialize(string name)
        //{
        //    _dataVariableTable = new Hashtable();
        //    _jsonHashCodeTable = new Hashtable();
        //    _name = name;
        //}

        /// <summary>
        /// Add an object to the underlying storage
        /// </summary>
        /// <param name="objId">key for the object</param>
        /// <param name="o">object</param>
  //      public static void AddObject(string objId, DataVariable o, int hashCode)
		//{
		//	lock (_writeLock)
		//	{
		//		if (_dataVariableTable[objId] != null)
		//		{
		//			_dataVariableTable[objId] = o;
		//			_jsonHashCodeTable[objId] = hashCode;
		//		}
		//		else
		//		{
		//			_dataVariableTable.Add(objId, o);
		//			_jsonHashCodeTable.Add(objId, hashCode);
		//		}
		//	}
		//}

		/// <summary>
		/// Remove an object from the underlying storage
		/// </summary>
		/// <param name="objId">key for the object</param>
		public static void DeleteObject(string objId)
		{
			lock (_writeLock)
			{
				_meassageTable.Remove(objId);
				_jsonMessageHashCodeTable.Remove(objId);
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
				return (DataVariable)_meassageTable[objId];
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

				if (!_meassageTable.ContainsKey(objId))
				{
					Log2.Trace("Add DataVar");
					_meassageTable[objId] = o;
					_jsonMessageHashCodeTable[objId] = hashCode;
				}
				else
				{
					if ((int)_jsonMessageHashCodeTable[objId] != hashCode)
					{
						DataVariable dv = (DataVariable)_meassageTable[objId];
						o.LastValue = dv.Value;
						o.LastValueTime = dv.UpdateTime;

						Log2.Trace("Update DataVar");
						_meassageTable[objId] = o;
						_jsonMessageHashCodeTable[objId] = hashCode;
					}
				}
			}
        }

		public static void DumpCache()
		{
			lock (_writeLock)
			{
				foreach (DictionaryEntry de in _meassageTable)
				{
					DataVariable dv = (DataVariable)de.Value;
					Log2.Trace("DATACACHE: {0}, {1}", (string)de.Key, (string)dv.Value);
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
					foreach (DictionaryEntry de in _meassageTable)
					{
						DataVariable dv = (DataVariable)de.Value;
						s = json.DataVariable2Json(dv);
						Log2.Trace("DATAFILE: {0}, {1}", (string)de.Key, (string)dv.Value);
						writer.WriteLine(s);
					}
				}
			}
		}
	}
}
