//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
using Newtonsoft.Json;

// Assemblies needed for Agentness
using Upperbay.Core.Logging;
using Upperbay.Agent.Interfaces;


namespace Upperbay.Worker.JSON
{
    public class JsonDataVariable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="datavar"></param>
        /// <returns></returns>
        public string DataVariable2Json(DataVariable datavar)
        {
            string output = JsonConvert.SerializeObject(datavar);
            Log2.Trace("DataVariable2Json: {0}", output);
            return output;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public DataVariable Json2DataVariable(string jsonString)
        {
            DataVariable deserializedData = JsonConvert.DeserializeObject<DataVariable>(jsonString);
            Log2.Trace("Json2DataVariable: {0} {1}", 
                deserializedData.ExternalName,
                deserializedData.Value); 
            return deserializedData;
        }
    }
}// End Namespace
