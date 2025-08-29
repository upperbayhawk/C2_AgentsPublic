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
    public class JsonGameResultsVariable
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resultvar"></param>
        /// <returns></returns>
        public string GameResultsVariable2Json(GameResultsVariable resultvar)
        {
            string output = JsonConvert.SerializeObject(resultvar);
            Log2.Trace("GameResultsVariable2Json: {0}", output);
            return output;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public GameResultsVariable Json2GameResultsVariable(string jsonString)
        {
            GameResultsVariable deserializedData = JsonConvert.DeserializeObject<GameResultsVariable>(jsonString);
            Log2.Trace("Json2GameResultsVariable: {0} {1}", 
                deserializedData.GameName,
                deserializedData.GameId); 
            return deserializedData;
        }
    }
}// End Namespace
