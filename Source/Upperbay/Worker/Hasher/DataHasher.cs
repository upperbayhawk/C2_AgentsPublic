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
using Upperbay.Worker.JSON;

namespace Upperbay.Worker.Hasher
{
    public class DataHasher
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataVar"></param>
        /// <returns></returns>
        public int HashDataVar(DataVariable dataVar)
        {
            JsonDataVariable jData = new JsonDataVariable();
            string data = jData.DataVariable2Json(dataVar);
            Int32 hash = data.GetHashCode();
            return hash;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public int HashDataJson(string json)
        {
            string data = json;
            Int32 hash = data.GetHashCode();
            return hash;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataVar"></param>
        /// <param name="dataHash"></param>
        /// <returns></returns>
        public bool IsSameDataVar(DataVariable dataVar, int dataHash)
        {
            JsonDataVariable jData = new JsonDataVariable();
            string data = jData.DataVariable2Json(dataVar);
            Int32 hash = data.GetHashCode();
            if (hash == dataHash)
                return true;
            else 
                return false;
            
        }
    }
}
