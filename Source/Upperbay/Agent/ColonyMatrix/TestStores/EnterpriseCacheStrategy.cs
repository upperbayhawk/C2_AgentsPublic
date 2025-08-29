using System;
using System.Collections;

using Microsoft.Practices.EnterpriseLibrary.Caching;
//using Upperbay.Core.Library;


namespace Upperbay.Agent.ColonyMatrix
{
    /// <summary>
    /// the sample cache strategy implementation which 
    /// shows how you create an pluggable component for SAF.Cache 
    /// to customize the way object is cahced and retrieved.
    /// </summary>
    public class EnterpriseCacheStrategy : ICacheStrategy
    {
        private CacheManager cache = null;
        //private CacheManager metacache = null;
        private string cacheName = null;
        private string cacheMetaName = null;

        
        /// <summary>
        /// constructor to instantiate the internal hashtable.
        /// </summary>
        public EnterpriseCacheStrategy()
        {
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public void Initialize(string cacheName)
        {
            this.cacheName = cacheName;
            this.cacheMetaName = cacheName + "Metabase";
            cache = (CacheManager)CacheFactory.GetCacheManager(cacheName);
            //metacache = CacheFactory.GetCacheManager(cacheMetaName);
        }


        /// <summary>
        /// Add an object to the underlying storage
        /// </summary>
        /// <param name="objId">key for the object</param>
        /// <param name="o">object</param>
        public void AddObject(string objId, object o)
        {
            cache.Add(objId, o);
        }

        /// <summary>
        /// Add an object to the underlying storage
        /// </summary>
        /// <param name="objId">key for the object</param>
        /// <param name="o">object</param>
        public void AddObjectWithMetadata(string objId, object o, object metaobject)
        {
            cache.Add(objId, o);
            //metacache.Add(objId, metaobject);
        }
 

        /// <summary>
        /// Remove an object from the underlying storage
        /// </summary>
        /// <param name="objId">key for the object</param>
        public void RemoveObject(string objId)
        {
            cache.Remove(objId);
        }

        public void RemoveObjectWithMetadata(string objId)
        {
            cache.Remove(objId);
            //metacache.Remove(objId);
        }



        /// <summary>
        /// Retrieve an object from the underlying storage
        /// </summary>
        /// <param name="objId">key for the object</param>
        /// <returns>object</returns>
        public object RetrieveObject(string objId)
        {
            return cache.GetData(objId);
        }

        public object RetrieveObjectWithMetadata(string objId, out object metaobject)
        {
            metaobject = null;
            //metaobject = metacache.GetData(objId);

            return cache.GetData(objId);
        }

        /// <summary>
        /// Update an object to the underlying storage
        /// </summary>
        /// <param name="objId">key for the object</param>
        /// <param name="o">object</param>
        public void UpdateObject(string objId, object o)
        {
            cache.Remove(objId);
            cache.Add(objId, o);
        }

        public void UpdateObjectWithMetadata(string objId, object o, object metaobject)
        {
            cache.Remove(objId);
            cache.Add(objId, o);

            //metacache.Remove(objId);
            //metacache.Add(objId, metaobject);
        }
    }
}
