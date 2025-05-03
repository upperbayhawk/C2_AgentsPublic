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
    public class JsonEventVariable
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datavar"></param>
        /// <returns></returns>
        public string EventVariable2Json(EventVariable datavar)
        {
            string output = JsonConvert.SerializeObject(datavar);
            Log2.Trace("EventVariable2Json: {0}", output);
            return output;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public EventVariable Json2EventVariable(string jsonString)
        {
            EventVariable deserializedData = JsonConvert.DeserializeObject<EventVariable>(jsonString);
            Log2.Trace("Json2EventVariable: {0} {1}", 
                deserializedData.EventName,
                deserializedData.EventType); 
            return deserializedData;
        }
    }
}// End Namespace
