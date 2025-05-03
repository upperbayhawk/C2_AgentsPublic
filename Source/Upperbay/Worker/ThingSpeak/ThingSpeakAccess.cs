//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

using Upperbay.Core.Logging;
using Upperbay.Agent.Interfaces;


namespace Upperbay.Worker.ThingSpeak
{


    //    JSON
    //channel
    //id : 317026
    //name : "watts@campdavid"
    //latitude : "0.0"
    //longitude : "0.0"
    //field1 : "powermeter.w_l1_l2"
    //created_at : "2017-08-14t13:43:36z"
    //updated_at : "2018-09-12t14:21:57z"
    //last_entry_id : 2169693
    //feeds
    //0
    //created_at : "2020-04-24t20:42:31z"
    //entry_id : 2169692
    //field1 : "698"
    //1
    //created_at : "2020-04-24t20:43:01z"
    //entry_id : 2169693
    //field1 : "701"
    //    ////
    //{"channel":{"id":317026,"name":"watts@campdavid","latitude":"0.0","longitude":"0.0"}}
    //       "field1":"powermeter.w_l1_l2",
    //      "created_at":"2017-08-14t13:43:36z",
    //      "updated_at":"2018-09-12t14:21:57z",
    //      "last_entry_id":2169693},
    //      "feeds":[
    //               {"created_at":"2020-04-24t20:42:31z","entry_id":2169692,"field1":"698"},
    //               {"created_at":"2020-04-24t20:43:01z","entry_id":2169693,"field1":"701"}
    //              ]
    //    }
    //}

    public class Channel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string field1 { get; set; }
        public string field2 { get; set; }
        public string field3 { get; set; }
        public string field4 { get; set; }
        public string field5 { get; set; }
        public string field6 { get; set; }
        public string field7 { get; set; }
        public string field8 { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public int last_entry_id { get; set; }
    }
    public class Feed
    {
        public string created_at { get; set; }
        public int entry_id { get; set; }
        public string field1 { get; set; }
        public string field2 { get; set; }
        public string field3 { get; set; }
        public string field4 { get; set; }
        public string field5 { get; set; }
        public string field6 { get; set; }
        public string field7 { get; set; }
        public string field8 { get; set; }
    }

    public class RootObject
    {
        public Channel channel { get; set; }
        public List<Feed> feeds { get; set; }
    }

    public class ThingSpeakAccess
    {
        /// <summary>
        /// The URL of the server.
        /// </summary>
        public const string DefaultServerUrl = "https://api.thingspeak.com/channels/";

        #region Private State

        private string _serverUrl = DefaultServerUrl;
        public string ServerUrl { get { return this._serverUrl; } set { this._serverUrl = value; } }
        private class TagConfig
        {
            public string channelID;
            public string fieldID;
            public string key;
        }
        #endregion

        #region Methods


        //        public string GetValue()
        //        {
        //            //ReadKey
        //            //Channel
        //            //Field
        //            //            const string WRITEKEY = "GJYxxxxxxxxxxV5X";
        //            string sResult = null;
        //            string sReadURI = null;
        //            string responseFromServer;
        //            try
        //            {
        //                const string READKEY = "O8T9QTxxxxxxxxJ8";
        //                string sReadBaseURI = "https://api.thingspeak.com/channels/";
        //                string sChannelID = "3xxxx6";
        ////                string sfield = "PowerMeter.W_L1_L2";
        //                string sfield = "1.json";
        //               // string sFormat = ".csv";
        //                //sReadURI = sReadBaseURI + sChannelID + "/fields/" + sfield + "?api_key=";
        //                //sReadURI = sReadURI + READKEY;
        //                sReadURI = sReadBaseURI + sChannelID + "/fields/" + sfield + "?api_key=" + READKEY + "&results=2";

        //                Log2.Trace(sReadURI);

        //                HttpWebRequest ThingsSpeakReq;
        //                HttpWebResponse ThingsSpeakResp;
        //                ThingsSpeakReq = (HttpWebRequest)WebRequest.Create(sReadURI);

        //                ThingsSpeakResp = (HttpWebResponse)ThingsSpeakReq.GetResponse();

        //                if (!(string.Equals(ThingsSpeakResp.StatusDescription, "OK")))
        //                {
        //                    Exception exData = new Exception(ThingsSpeakResp.StatusDescription);
        //                    throw exData;

        //                }
        //                else
        //                {
        //                    Log2.Trace("SUCCESS");
        //                    // Get the stream containing content returned by the server.
        //                    // The using block ensures the stream is automatically closed.
        //                    using (Stream dataStream = ThingsSpeakResp.GetResponseStream())
        //                    {
        //                        // Open the stream using a StreamReader for easy access.
        //                        StreamReader reader = new StreamReader(dataStream);
        //                        // Read the content.
        //                        responseFromServer = reader.ReadToEnd();
        //                        // Display the content.
        //                        Log2.Trace(responseFromServer);
        //                    }
        //                }

        //                channel myChannel = JsonConvert.DeserializeObject<channel>(responseFromServer);
        //                string myString = myChannel.feeds[1].field1;
        //                Log2.Trace("LATEST VALUE = " + myString);
        //                sResult = myString;
        //                // Close the response.
        //                ThingsSpeakResp.Close();

        //            }
        //            catch (Exception ex)
        //            {
        //                Log2.Trace(ex.Message);
        //                throw;
        //            }

        //            return (sResult);
        //        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelID"></param>
        /// <param name="fieldID"></param>
        /// <param name="readKey"></param>
        /// <returns></returns>
        public DataVariable GetValue(string channelID, string fieldID, string readKey)
        {
            //string sResult;
            string sReadURI;
            string responseFromServer;
            DataVariable dataVariable = new DataVariable();

            //Log2.Trace("traceswitch {0}", _agentSwitch.Level.ToString());
            try
            {
                //string sReadBaseURI = "https://api.thingspeak.com/channels/";
                //string sChannelID = "317026";
                //                string sfield = "PowerMeter.W_L1_L2";
                //string sfield = "1.json";
                // string sFormat = ".csv";
                //sReadURI = sReadBaseURI + sChannelID + "/fields/" + sfield + "?api_key=";
                //sReadURI = sReadURI + READKEY;
                sReadURI = _serverUrl + channelID + "/fields/" + fieldID + ".json?api_key=" + readKey + "&results=20";
//                sReadURI = _serverUrl + channelID + "/fields/last.json?api_key=" + readKey + "&results=2";

                Log2.Trace(sReadURI);

                HttpWebRequest ThingsSpeakReq;
                HttpWebResponse ThingsSpeakResp;
                ThingsSpeakReq = (HttpWebRequest)WebRequest.Create(sReadURI);

                ThingsSpeakResp = (HttpWebResponse)ThingsSpeakReq.GetResponse();

                if (!(string.Equals(ThingsSpeakResp.StatusDescription, "OK")))
                {
                    Exception exData = new Exception(ThingsSpeakResp.StatusDescription);
                    throw exData;
                }
                else
                {
                    Log2.Trace("SUCCESS");
                    // Get the stream containing content returned by the server.
                    // The using block ensures the stream is automatically closed.
                    using (Stream dataStream = ThingsSpeakResp.GetResponseStream())
                    {
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        responseFromServer = reader.ReadToEnd();
                        //Log2.Trace(responseFromServer);
                    }
                    // Display the content.

                    Log2.Trace("Response = {0}", responseFromServer);
                    string myString;
                    RootObject myRootObject = JsonConvert.DeserializeObject<RootObject>(responseFromServer);

                    myString = myRootObject.channel.id.ToString();
                    Log2.Trace("LATEST ID = {0}", myString);

                    myString = myRootObject.channel.latitude;
                    Log2.Trace("LATEST LAT = {0}", myString);


                    myString = myRootObject.channel.longitude;
                    Log2.Trace("LATEST LONG = {0}", myString);

                    myString = myRootObject.channel.updated_at;
                    Log2.Trace("LATEST Upd At = {0}", myString);

                    myString = myRootObject.channel.last_entry_id.ToString();
                    Log2.Trace("LATEST LAST ENTRY ID = {0}", myString);

                    myString = myRootObject.channel.created_at;
                    Log2.Trace("LATEST CREATED AT = {0}", myString);

                    myString = myRootObject.channel.field1;
                    Log2.Trace("LATEST FIELD1 = {0}", myString);

                    myString = myRootObject.channel.name;
                    Log2.Trace("LATEST NAME = {0}", myString);

                   // Find latest value
                        string s;
                        for (int i = 19; i >= 0; i--)
                        {
                            //get the last good value from the return feed
                            s = myRootObject.feeds[i].field1;
                            if (s != null)
                            {
                                dataVariable.Value = myRootObject.feeds[i].field1;
                               
                                Log2.Trace("FOUND LATEST VALUE = {0}", dataVariable.Value);

                                try
                                {
                                    myString = myRootObject.feeds[i].created_at;
                                    Log2.Trace("LATEST JSON TIME = " + myString);
                                    DateTime time = DateTime.Parse(myString);
                                    Log2.Trace("PARSED TIME = {0}", time.ToString());

                                    dataVariable.UpdateTime = time;
                                    dataVariable.ServerTime = DateTime.Now;
                                    dataVariable.Status = "GOOD";
                                    dataVariable.Quality = "GOOD";
                                    dataVariable.ExternalName = myRootObject.channel.field1;
                            }
                            catch (Exception ex)
                                {
                                    Log2.Error("DateTime Parse Exception = {0}", ex.Message);
                                }
                            break;
                            }
                        }
                  
                    // Close the response.
                    ThingsSpeakResp.Close();
                    return (dataVariable);
                }
            }
            catch (Exception ex)
            {
                Log2.Trace(ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public DataVariable GetValue(string configString)
        {

            // example string -> "channelID=525xxxx,fieldID=1,readKey=76xxx7634"
            //ReadKey
            //Channel
            //Field
            //parse config string and getvalue
 
            TagConfig tagConfig = new TagConfig();
        
            //string value = null;
            string phrase = null;
            //string props = null;
            DataVariable dataVariable;
            
            phrase = configString;
            Log2.Trace("Reading ThingSpeak ConfigString: {0}", phrase);
            string[] words = phrase.Split(',');
            foreach (var word in words)
            {
                Log2.Trace(word);
                string[] parms = word.Split('=');
                foreach (var prop in parms)
                {
                    if (String.Equals(parms[0],"channelID"))
                    {
                        tagConfig.channelID = parms[1];
                    }
                    else if (String.Equals(parms[0], "fieldID"))
                    {
                        tagConfig.fieldID = parms[1];
                    } else if (String.Equals(parms[0], "readKey"))
                    {
                        tagConfig.key= parms[1];
                    }
                    //System.Console.WriteLine($"<{word}>");
                }
                //System.Console.WriteLine($"<{word}>");
            }

            Log2.Trace("Reading ThingSpeak Tag: {0}, {1}, {2}", tagConfig.channelID, tagConfig.fieldID, tagConfig.key);

            dataVariable = GetValue(tagConfig.channelID, tagConfig.fieldID, tagConfig.key);

            Log2.Trace("Reading ThingSpeak Value: {0}", dataVariable.Value);

            return (dataVariable);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="configString"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="averageIntervalInMinutes"></param>
        /// <returns></returns>
        public string GetAverageValue(string configString, 
                                    DateTime beginTime, 
                                    DateTime endTime, 
                                    int averageIntervalInMinutes,
                                    string timeZone)
        {
            string sReadURI;
            string responseFromServer;
            string average = "0.0";

            HttpWebRequest ThingsSpeakReq;
            HttpWebResponse ThingsSpeakResp;

            try
            {
                // Calculate Time Strings for Query
                string beginTimeString = beginTime.ToString("yyyy-MM-dd\\%20HH:mm:ss");
                string endTimeString = endTime.ToString("yyyy-MM-dd\\%20HH:mm:ss");

                Log2.Debug("Begin: {0}", beginTimeString);
                Log2.Debug("End: {0}", endTimeString);

                TagConfig tagConfig = new TagConfig();
                string phrase = null;

                phrase = configString;
                Log2.Trace("Reading ThingSpeak ConfigString: {0}", phrase);
                string[] words = phrase.Split(',');
                foreach (var word in words)
                {
                    Log2.Trace(word);
                    string[] parms = word.Split('=');
                    foreach (var prop in parms)
                    {
                        if (String.Equals(parms[0], "channelID"))
                        {
                            tagConfig.channelID = parms[1];
                        }
                        else if (String.Equals(parms[0], "fieldID"))
                        {
                            tagConfig.fieldID = parms[1];
                        }
                        else if (String.Equals(parms[0], "readKey"))
                        {
                            tagConfig.key = parms[1];
                        }
                        //System.Console.WriteLine($"<{word}>");
                    }
                    //System.Console.WriteLine($"<{word}>");
                }

                Log2.Trace("Reading ThingSpeak Tag: {0}, {1}, {2}", 
                                        tagConfig.channelID, tagConfig.fieldID, tagConfig.key);

                string url = _serverUrl;
                string chan = tagConfig.channelID;
                string fld = tagConfig.fieldID;
                string key = tagConfig.key;
                string avg = averageIntervalInMinutes.ToString();
                if (averageIntervalInMinutes == 60) // 1Hours
                    avg = "60";
                else if (averageIntervalInMinutes == 120) // 2Hours
                    avg = "60";
                else if (averageIntervalInMinutes == 180) // 3Hours
                    avg = "60";
                else if (averageIntervalInMinutes == 240) // 4Hours
                    avg = "240";
                else if (averageIntervalInMinutes == 300) // 5 hours
                    avg = "60";
                //+"&results=" + sam

                //string timeZone = "America/New_York";

                sReadURI =
                _serverUrl + chan + "/fields/" + fld + ".json?api_key=" + key + "&start=" + beginTimeString + "&end=" + endTimeString + "&average=" + avg + "&timezone=" + timeZone;

                //sReadURI =
                ////    "https://api.thingspeak.com/channels/31xxx6/fields/1.json?api_key=O8xxxxxxxxWUKTJ8&results=3000";
                //"https://api.thingspeak.com/channels/3xxx26/fields/1.json?api_key=O8T9QxxxxxxxxTJ8&start=2020-07-21%2010:00:00&end=2020-07-21%2011:00:00&results=3000";
                //sReadURI =
                //"https://api.thingspeak.com/channels/1xxx974/fields/1.json?api_key=USI884xxxxxx0HBX&start=2020-08-25%2012:00:00&end=2020-08-25%2012:10:00&results=1200&average=10";

                Log2.Debug(sReadURI);

                ThingsSpeakReq = (HttpWebRequest)WebRequest.Create(sReadURI);

                ThingsSpeakResp = (HttpWebResponse)ThingsSpeakReq.GetResponse();

                if (!(string.Equals(ThingsSpeakResp.StatusDescription, "OK")))
                {
                    Exception exData = new Exception(ThingsSpeakResp.StatusDescription);
                    Log2.Error("GetAverageValues Exception: {0}", exData);
                    throw exData;
                }
                else
                {
                    Log2.Trace("GetAverageValue: SUCCESS");
                    // Get the stream containing content returned by the server.
                    // The using block ensures the stream is automatically closed.
                    using (Stream dataStream = ThingsSpeakResp.GetResponseStream())
                    {
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        responseFromServer = reader.ReadToEnd();
                        Log2.Debug("ThingSpeak Response: {0}", responseFromServer);
                    }

                    // Display the content.

                    RootObject myRootObject = JsonConvert.DeserializeObject<RootObject>(responseFromServer);

                    //myString = myRootObject.channel.id.ToString();
                    //Log2.Trace("LATEST ID = {0}", myString);

                    //myString = myRootObject.channel.latitude;
                    //Log2.Trace("LATEST LAT = {0}", myString);


                    //myString = myRootObject.channel.longitude;
                    //Log2.Trace("LATEST LONG = {0}", myString);

                    //myString = myRootObject.channel.updated_at;
                    //Log2.Trace("LATEST Upd At = {0}", myString);

                    //myString = myRootObject.channel.last_entry_id.ToString();
                    //Log2.Trace("LATEST LAST ENTRY ID = {0}", myString);

                    //myString = myRootObject.channel.created_at;
                    //Log2.Trace("LATEST CREATED AT = {0}", myString);

                    //myString = myRootObject.channel.field1;
                    //Log2.Trace("LATEST FIELD1 = {0}", myString);

                    //myString = myRootObject.channel.name;
                    //Log2.Trace("LATEST NAME = {0}", myString);
                                        
                    int lastFieldIndex = myRootObject.feeds.Count - 1;
                    Log2.Debug("THINGSPEAK Last Feed Index = {0}", lastFieldIndex);
                    if (lastFieldIndex >= 0)
                    {

                        if (averageIntervalInMinutes == 60) // 1 Hour
                        {
                            average = myRootObject.feeds[lastFieldIndex].field1;
                        }
                        else if (averageIntervalInMinutes == 240) // 4 Hours
                        {
                            average = myRootObject.feeds[lastFieldIndex].field1;
                        }
                        else if (averageIntervalInMinutes == 120) // 2Hours
                        {
                            if (lastFieldIndex > 0)
                            {
                                string average1 = myRootObject.feeds[lastFieldIndex - 1].field1;
                                double avg1 = Double.Parse(average1);
                                string average2 = myRootObject.feeds[lastFieldIndex].field1;
                                double avg2 = Double.Parse(average2);
                                average = ((avg1 + avg2) / 2).ToString();
                            }
                            else
                            {
                                Log2.Debug("THINGSPEAK ERROR 2 hour had {0} fields, needs 2 fields", lastFieldIndex + 1);
                            }
                        }
                        else if (averageIntervalInMinutes == 180) // 3Hours
                        {
                            if (lastFieldIndex > 2)
                            {
                                string average1 = myRootObject.feeds[lastFieldIndex - 2].field1;
                                double avg1 = Double.Parse(average1);
                                string average2 = myRootObject.feeds[lastFieldIndex - 1].field1;
                                double avg2 = Double.Parse(average2);
                                string average3 = myRootObject.feeds[lastFieldIndex].field1;
                                double avg3 = Double.Parse(average3);
                                average = ((avg1 + avg2 + avg3) / 3).ToString();
                            }
                            else
                            {
                                Log2.Debug("THINGSPEAK ERROR 3 hour had {0} fields, needs 3 fields", lastFieldIndex + 1);
                            }
                        }
                        else if (averageIntervalInMinutes == 300) // 5 hours
                        {
                            if (lastFieldIndex > 4)
                            {
                                string average1 = myRootObject.feeds[lastFieldIndex - 4].field1;
                                double avg1 = Double.Parse(average1);
                                string average2 = myRootObject.feeds[lastFieldIndex - 3].field1;
                                double avg2 = Double.Parse(average2);
                                string average3 = myRootObject.feeds[lastFieldIndex - 2].field1;
                                double avg3 = Double.Parse(average3);
                                string average4 = myRootObject.feeds[lastFieldIndex - 1].field1;
                                double avg4 = Double.Parse(average4);
                                string average5 = myRootObject.feeds[lastFieldIndex].field1;
                                double avg5 = Double.Parse(average5);
                                average = ((avg1 + avg2 + avg3 + avg4 + avg5) / 5).ToString();
                            }
                            else
                            {
                                Log2.Debug("THINGSPEAK ERROR 5 hour had {0} fields, needs 5 fields", lastFieldIndex + 1);
                            }
                        }
                        else if (averageIntervalInMinutes == 360) // 6 hours
                        {
                            if (lastFieldIndex > 5)
                            {
                                string average0 = myRootObject.feeds[lastFieldIndex - 5].field1;
                                double avg0 = Double.Parse(average0);
                                string average1 = myRootObject.feeds[lastFieldIndex - 4].field1;
                                double avg1 = Double.Parse(average1);
                                string average2 = myRootObject.feeds[lastFieldIndex - 3].field1;
                                double avg2 = Double.Parse(average2);
                                string average3 = myRootObject.feeds[lastFieldIndex - 2].field1;
                                double avg3 = Double.Parse(average3);
                                string average4 = myRootObject.feeds[lastFieldIndex - 1].field1;
                                double avg4 = Double.Parse(average4);
                                string average5 = myRootObject.feeds[lastFieldIndex].field1;
                                double avg5 = Double.Parse(average5);
                                average = ((avg1 + avg2 + avg3 + avg4 + avg5 + avg0) / 6).ToString();
                            }
                            else
                            {
                                Log2.Debug("THINGSPEAK ERROR 6 hour had {0} fields, needs 6 fields", lastFieldIndex + 1);
                            }
                        }
                        else
                        {
                            average = myRootObject.feeds[lastFieldIndex].field1;
                        }

                        Log2.Trace("FOUND AVERAGE VALUE = {0}", average);
                    }
                    else 
                    {
                        Log2.Error("ERROR: BAD INDEX {0}", lastFieldIndex);
                        average = "0.0";
                    }
                    // Close the response.
                    ThingsSpeakResp.Close();
                    
                    return average;
                }
            }
            catch (Exception ex)
            {
                Log2.Error("ERROR GetAverageValue: {0}", ex.Message);
                throw;
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configString"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public string GetRawValues(string configString,
                                   DateTime beginTime,
                                   DateTime endTime,
                                   string timeZone
                                   )
        {
            string sReadURI;
            string responseFromServer;
            
            HttpWebRequest ThingsSpeakReq;
            HttpWebResponse ThingsSpeakResp;

            try
            {
                // Calculate Time Strings for Query
                string beginTimeString = beginTime.ToString("yyyy-MM-dd\\%20HH:mm:ss");
                string endTimeString = endTime.ToString("yyyy-MM-dd\\%20HH:mm:ss");

                Log2.Debug("Begin: {0}", beginTimeString);
                Log2.Debug("End: {0}", endTimeString);

                TagConfig tagConfig = new TagConfig();
                string phrase = null;

                phrase = configString;
                Log2.Trace("Reading ThingSpeak ConfigString: {0}", phrase);
                string[] words = phrase.Split(',');
                foreach (var word in words)
                {
                    Log2.Trace(word);
                    string[] parms = word.Split('=');
                    foreach (var prop in parms)
                    {
                        if (String.Equals(parms[0], "channelID"))
                        {
                            tagConfig.channelID = parms[1];
                        }
                        else if (String.Equals(parms[0], "fieldID"))
                        {
                            tagConfig.fieldID = parms[1];
                        }
                        else if (String.Equals(parms[0], "readKey"))
                        {
                            tagConfig.key = parms[1];
                        }
                        //System.Console.WriteLine($"<{word}>");
                    }
                    //System.Console.WriteLine($"<{word}>");
                }

                Log2.Trace("Reading ThingSpeak Tag: {0}, {1}, {2}",
                                        tagConfig.channelID, tagConfig.fieldID, tagConfig.key);

                string url = _serverUrl;
                string chan = tagConfig.channelID;
                string fld = tagConfig.fieldID;
                string key = tagConfig.key;

                //string timeZone = "America/New_York";

                sReadURI =
                _serverUrl + chan + "/fields/" + fld + ".json?api_key=" + key + "&start=" + beginTimeString + "&end=" + endTimeString + "&timezone=" + timeZone;

                //sReadURI =
                ////    "https://api.thingspeak.com/channels/3xxx26/fields/1.json?api_key=O8xxxxxxxxxxxTJ8&results=3000";
                //"https://api.thingspeak.com/channels/3xxx26/fields/1.json?api_key=O8xxxxxxxxxxKTJ8&start=2020-07-21%2010:00:00&end=2020-07-21%2011:00:00&results=3000";
                //sReadURI =
                //"https://api.thingspeak.com/channels/11xxxx4/fields/1.json?api_key=USI884xxxxxx0HBX&start=2020-08-25%2012:00:00&end=2020-08-25%2012:10:00&results=1200&average=10";

                Log2.Debug(sReadURI);

                ThingsSpeakReq = (HttpWebRequest)WebRequest.Create(sReadURI);

                ThingsSpeakResp = (HttpWebResponse)ThingsSpeakReq.GetResponse();

                if (!(string.Equals(ThingsSpeakResp.StatusDescription, "OK")))
                {
                    Exception exData = new Exception(ThingsSpeakResp.StatusDescription);
                    Log2.Error("GetRawValues Exception: {0}", exData);
                    throw exData;
                }
                else
                {
                    Log2.Trace("GetRawValues: SUCCESS");
                    // Get the stream containing content returned by the server.
                    // The using block ensures the stream is automatically closed.
                    using (Stream dataStream = ThingsSpeakResp.GetResponseStream())
                    {
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        responseFromServer = reader.ReadToEnd();
                        Log2.Debug("ThingSpeak Response: {0}", responseFromServer);

                        string filePath = "logs\\PowerMeterRawValues.json";
                        using (StreamWriter outputFile = new StreamWriter(filePath, true))
                        {
                            outputFile.WriteLine(responseFromServer);
                        }
                    }

                    // Display the content.

                    RootObject myRootObject = JsonConvert.DeserializeObject<RootObject>(responseFromServer);

                    //myString = myRootObject.channel.id.ToString();
                    //Log2.Trace("LATEST ID = {0}", myString);

                    //myString = myRootObject.channel.latitude;
                    //Log2.Trace("LATEST LAT = {0}", myString);

                    //myString = myRootObject.channel.longitude;
                    //Log2.Trace("LATEST LONG = {0}", myString);

                    //myString = myRootObject.channel.updated_at;
                    //Log2.Trace("LATEST Upd At = {0}", myString);

                    //myString = myRootObject.channel.last_entry_id.ToString();
                    //Log2.Trace("LATEST LAST ENTRY ID = {0}", myString);

                    //myString = myRootObject.channel.created_at;
                    //Log2.Trace("LATEST CREATED AT = {0}", myString);

                    //myString = myRootObject.channel.field1;
                    //Log2.Trace("LATEST FIELD1 = {0}", myString);

                    //myString = myRootObject.channel.name;
                    //Log2.Trace("LATEST NAME = {0}", myString);

                    //INSERT INTO ODBC DATABASE


                    // Close the response.
                    ThingsSpeakResp.Close();

                    return responseFromServer;
                }
            }
            catch (Exception ex)
            {
                Log2.Error("ERROR GetRawValues: {0}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configString"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public string GetAverageValueFromRawData(string tag,
                                    string playerID,
                                    string eventID,
                                    string eventName,
                                    string configString,
                                    DateTime beginTime,
                                    DateTime endTime,
                                    int durationInMinutes,
                                    string timeZone,
                                    bool saveRawData
                                    )
        {
            string sReadURI;
            string responseFromServer;

            HttpWebRequest ThingsSpeakReq;
            HttpWebResponse ThingsSpeakResp;

            try
            {
                // Calculate Time Strings for Query
                string beginTimeString = beginTime.ToString("yyyy-MM-dd\\%20HH:mm:ss");
                string endTimeString = endTime.ToString("yyyy-MM-dd\\%20HH:mm:ss");

                Log2.Debug("SaveRawValues Begin: {0}", beginTimeString);
                Log2.Debug("SaveRawValues End: {0}", endTimeString);

                TagConfig tagConfig = new TagConfig();
                string phrase = null;

                phrase = configString;
                Log2.Trace("Reading ThingSpeak ConfigString: {0}", phrase);
                string[] words = phrase.Split(',');
                foreach (var word in words)
                {
                    Log2.Trace(word);
                    string[] parms = word.Split('=');
                    foreach (var prop in parms)
                    {
                        if (String.Equals(parms[0], "channelID"))
                        {
                            tagConfig.channelID = parms[1];
                        }
                        else if (String.Equals(parms[0], "fieldID"))
                        {
                            tagConfig.fieldID = parms[1];
                        }
                        else if (String.Equals(parms[0], "readKey"))
                        {
                            tagConfig.key = parms[1];
                        }
                        //System.Console.WriteLine($"<{word}>");
                    }
                    //System.Console.WriteLine($"<{word}>");
                }

                Log2.Trace("SaveRawValues Reading ThingSpeak Tag: {0}, {1}, {2}",
                                        tagConfig.channelID, tagConfig.fieldID, tagConfig.key);

                string url = _serverUrl;
                string chan = tagConfig.channelID;
                string fld = tagConfig.fieldID;
                string key = tagConfig.key;

                //string timeZone = "America/New_York";

                sReadURI =
                _serverUrl + chan + "/fields/" + fld + ".json?api_key=" + key + "&start=" + beginTimeString + "&end=" + endTimeString + "&timezone=" + timeZone;

                //sReadURI =
                ////    "https://api.thingspeak.com/channels/317xxx/fields/1.json?api_key=O8T9xxxxxxxxKTJ8&results=3000";
                //"https://api.thingspeak.com/channels/3xxx26/fields/1.json?api_key=O8T9xxxxxxxxxTJ8&start=2020-07-21%2010:00:00&end=2020-07-21%2011:00:00&results=3000";
                //sReadURI =
                //"https://api.thingspeak.com/channels/11xxxx4/fields/1.json?api_key=USI8xxxxxxxx0HBX&start=2020-08-25%2012:00:00&end=2020-08-25%2012:10:00&results=1200&average=10";

                Log2.Debug("SaveRawValues: {0}", sReadURI);

                ThingsSpeakReq = (HttpWebRequest)WebRequest.Create(sReadURI);

                ThingsSpeakResp = (HttpWebResponse)ThingsSpeakReq.GetResponse();

                if (!(string.Equals(ThingsSpeakResp.StatusDescription, "OK")))
                {
                    Exception exData = new Exception(ThingsSpeakResp.StatusDescription);
                    Log2.Error("SaveRawValues Exception: {0}", exData);
                    throw exData;
                }
                else
                {
                    Log2.Trace("GetAverageValueFromRawData: SUCCESS");
                    // Get the stream containing content returned by the server.
                    // The using block ensures the stream is automatically closed.
                    using (Stream dataStream = ThingsSpeakResp.GetResponseStream())
                    {
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        responseFromServer = reader.ReadToEnd();

                        if (saveRawData)
                        {
                            Log2.Debug("SaveRawValues ThingSpeak Response: {0}", responseFromServer);
                            string filePath = "logs\\PowerMeterRawValues.json";
                            using (StreamWriter outputFile = new StreamWriter(filePath, true))
                            {
                                outputFile.WriteLine(responseFromServer);
                            }
                        }
                    }

                    // Display the content.

                    RootObject myRootObject = JsonConvert.DeserializeObject<RootObject>(responseFromServer);

                    //myString = myRootObject.channel.id.ToString();
                    //Log2.Trace("LATEST ID = {0}", myString);

                    //myString = myRootObject.channel.latitude;
                    //Log2.Trace("LATEST LAT = {0}", myString);

                    //myString = myRootObject.channel.longitude;
                    //Log2.Trace("LATEST LONG = {0}", myString);

                    //myString = myRootObject.channel.updated_at;
                    //Log2.Trace("LATEST Upd At = {0}", myString);

                    //myString = myRootObject.channel.last_entry_id.ToString();
                    //Log2.Trace("LATEST LAST ENTRY ID = {0}", myString);

                    //myString = myRootObject.channel.created_at;
                    //Log2.Trace("LATEST CREATED AT = {0}", myString);

                    //myString = myRootObject.channel.field1;
                    //Log2.Trace("LATEST FIELD1 = {0}", myString);

                    //myString = myRootObject.channel.name;
                    //Log2.Trace("LATEST NAME = {0}", myString);

                    //INSERT INTO FILE

                    string myName = myRootObject.channel.name;
                    string filePathCsv = "data\\" + playerID + "-meterdata.csv";
                    double avgWatts = 0.0;
                    double kwattHours = 0.0;

                    try
                    {
                        int totalCount = 0;
                        double totalWatts = 0.0;
                        DateTime lastTime = DateTime.Now;
                        string currentDir = Directory.GetCurrentDirectory();
                        System.IO.Directory.CreateDirectory(currentDir + "\\data");

                        using (StreamWriter outputFile = new StreamWriter(filePathCsv, true))
                        {
                            foreach (var element in myRootObject.feeds)
                            {
                                string rawValue = element.field1;
                                string rawTime = element.created_at;
                                DateTime time;
                                if (DateTime.TryParse(rawTime, out time))
                                {
                                    if (saveRawData)
                                    {
                                        //Log2.Trace("SaveRawValues: {0}:{1}", time, rawValue);
                                        string events = "\"" + tag + "\",\"" + playerID + "\",\"" + eventID + "\",\"" + myName + "\",\"" + configString + "\",\"" + time.ToString() + "\",\"" + rawValue + "\"";
                                        outputFile.WriteLine(events);
                                    }
                                    lastTime = time;
                                    totalCount++;
                                    double watts = double.Parse(rawValue);
                                    totalWatts = totalWatts + watts;
                                }
                                else
                                {
                                    Log2.Error("ERROR DateTime parsing failed, raw data not written: {0}", rawTime);
                                }
                            }
                            avgWatts = totalWatts / totalCount;
                            kwattHours = ((avgWatts / 1000) * ((double)durationInMinutes/60));
                            string avg = "\"" + tag + "\",\"" + playerID + "\",\"" + eventID + "\",\"" + myName + "\",\"" + configString + "\",\"" + lastTime.ToString() + "\",\"" + avgWatts.ToString("0.######") + "\",\"" + "Average Power in Watts" + "\"";
                            outputFile.WriteLine(avg);
                            string kwhr ="\"" + tag + "\",\"" + playerID + "\",\"" + eventID + "\",\"" + myName + "\",\"" + configString + "\",\"" + lastTime.ToString() + "\",\"" + kwattHours.ToString("0.######") + "\",\"" + "Energy in KiloWatt-Hrs" + "\"";
                            outputFile.WriteLine(kwhr);
                            outputFile.WriteLine();
                        }
                    }
                    catch (Exception ex)
                    {
                        Log2.Error("ERROR SaveRawValues Writing to data file: {0} {1}", filePathCsv, ex.Message);
                    }
                    // Close the response.
                    ThingsSpeakResp.Close();

                    return avgWatts.ToString();
                }
            }
            catch (Exception ex)
            {
                Log2.Error("ERROR SaveRawValues: {0}", ex.Message);
                throw;
            }
        }




        /// <summary>
        /// PLAY METHOD FOR TESTING ONLY
        /// </summary>
//        public DataVariable GetADREventValues(string channelID, string fieldID, string readKey)
        public bool GetADREventValues()
        {
            //string sResult;
            string sReadURI;
            string responseFromServer;
            DataVariable dataVariable = new DataVariable();

            //Log2.Trace("traceswitch {0}", _agentSwitch.Level.ToString());
            try
            {
                //string sReadBaseURI = "https://api.thingspeak.com/channels/";
                //string sChannelID = "317026";
                //                string sfield = "PowerMeter.W_L1_L2";
                //string sfield = "1.json";
                // string sFormat = ".csv";
                //sReadURI = sReadBaseURI + sChannelID + "/fields/" + sfield + "?api_key=";
                //sReadURI = sReadURI + READKEY;
                //                sReadURI = _serverUrl + channelID + "/fields/last.json?api_key=" + readKey + "&results=2";

                //                sReadURI = _serverUrl + channelID + "/fields/" + fieldID + ".json?api_key=" + readKey + "&results=100";


                //sReadURI = _serverUrl + channelID + "/fields/" + fieldID + ".json?api_key=" + readKey + "&results=100";

                ///////////////////////////////////////////////////////////////////////

                HttpWebRequest ThingsSpeakReq;
                HttpWebResponse ThingsSpeakResp;

                Log2.Debug("8000 values");
                sReadURI =
   "https://api.thingspeak.com/channels/31xx26/fields/1.json?api_key=O8T9xxxxx4WUKTJ8&results=1000&average=60";
//"https://api.thingspeak.com/channels/31xx26/fields/1.json?api_key=O8TxxxxxY4WUKTJ8&start=2020-07-21%2010:00:00&end=2020-07-21%2011:00:00&results=3000";

                Log2.Debug(sReadURI);

                //HttpWebRequest ThingsSpeakReq;
                //HttpWebResponse ThingsSpeakResp;
                ThingsSpeakReq = (HttpWebRequest)WebRequest.Create(sReadURI);

                ThingsSpeakResp = (HttpWebResponse)ThingsSpeakReq.GetResponse();

                if (!(string.Equals(ThingsSpeakResp.StatusDescription, "OK")))
                {
                    Exception exData = new Exception(ThingsSpeakResp.StatusDescription);
                    Log2.Error("GetADREventValues Exception: {0}", exData);
                    throw exData;

                }
                else
                {
                    Log2.Trace("SUCCESS");
                    // Get the stream containing content returned by the server.
                    // The using block ensures the stream is automatically closed.
                    using (Stream dataStream = ThingsSpeakResp.GetResponseStream())
                    {
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        responseFromServer = reader.ReadToEnd();
                        Log2.Debug(responseFromServer);
                    }
                }

                ///////////////////////////////////////////////////////////////////////

                DateTime endTime = DateTime.Now;
                TimeSpan interval = new TimeSpan(1, 0, 0);
                DateTime beginTime = new DateTime();
                beginTime = endTime.Subtract(interval);
         
                string beginTimeString = beginTime.ToString("yyyy-MM-dd\\%20HH:mm:ss");
                string endTimeString = endTime.ToString("yyyy-MM-dd\\%20HH:mm:ss");

                Log2.Debug("Begin: {0}", beginTimeString);
                Log2.Debug("End: {0}", endTimeString);

                sReadURI =
                //    "https://api.thingspeak.com/channels/317026/fields/1.json?api_key=O8T9xxxxx4WUKTxx&results=3000";
                "https://api.thingspeak.com/channels/31xx26/fields/1.json?api_key=O8xxxxxxx4WUKTJ8&start=2020-07-21%2010:00:00&end=2020-07-21%2011:00:00&results=3000";

                Log2.Debug(sReadURI);

                //HttpWebRequest ThingsSpeakReq;
                //HttpWebResponse ThingsSpeakResp;
                ThingsSpeakReq = (HttpWebRequest)WebRequest.Create(sReadURI);

                ThingsSpeakResp = (HttpWebResponse)ThingsSpeakReq.GetResponse();

                if (!(string.Equals(ThingsSpeakResp.StatusDescription, "OK")))
                {
                    Exception exData = new Exception(ThingsSpeakResp.StatusDescription);
                    Log2.Error("GetADREventValues Exception: {0}", exData);
                    throw exData;
                }
                else
                {
                    Log2.Trace("SUCCESS");
                    // Get the stream containing content returned by the server.
                    // The using block ensures the stream is automatically closed.
                    using (Stream dataStream = ThingsSpeakResp.GetResponseStream())
                    {
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        responseFromServer = reader.ReadToEnd();
                        Log2.Debug("ThingSpeak Response: {0}", responseFromServer);
                    }

//////////////////////////////////////////////////////////

                    // Display the content.

                    //Log2.Trace("Response = {0}", responseFromServer);
                    //string myString;
                    //RootObject myRootObject = JsonConvert.DeserializeObject<RootObject>(responseFromServer);

                    //myString = myRootObject.channel.id.ToString();
                    //Log2.Trace("LATEST ID = {0}", myString);

                    //myString = myRootObject.channel.latitude;
                    //Log2.Trace("LATEST LAT = {0}", myString);


                    //myString = myRootObject.channel.longitude;
                    //Log2.Trace("LATEST LONG = {0}", myString);

                    //myString = myRootObject.channel.updated_at;
                    //Log2.Trace("LATEST Upd At = {0}", myString);

                    //myString = myRootObject.channel.last_entry_id.ToString();
                    //Log2.Trace("LATEST LAST ENTRY ID = {0}", myString);

                    //myString = myRootObject.channel.created_at;
                    //Log2.Trace("LATEST CREATED AT = {0}", myString);

                    //myString = myRootObject.channel.field1;
                    //Log2.Trace("LATEST FIELD1 = {0}", myString);

                    //myString = myRootObject.channel.name;
                    //Log2.Trace("LATEST NAME = {0}", myString);

                    //// Find latest value
                    //string s;
                    //for (int i = 99; i >= 0; i--)
                    //{
                    //    //get the last good value from the return feed
                    //    s = myRootObject.feeds[i].field1;
                    //    if (s != null)
                    //    {
                    //        dataVariable.Value = myRootObject.feeds[i].field1;

                    //        Log2.Trace("FOUND LATEST VALUE = {0}", dataVariable.Value);

                    //        try
                    //        {

                    //            myString = myRootObject.feeds[i].created_at;
                    //            Log2.Trace("LATEST JSON TIME = " + myString);
                    //            DateTime time = DateTime.Parse(myString);
                    //            Log2.Trace("PARSED TIME = {0}", time.ToString());

                    //            dataVariable.UpdateTime = time;
                    //            dataVariable.ServerTime = DateTime.Now;
                    //            dataVariable.Status = "GOOD";
                    //            dataVariable.Quality = "GOOD";
                    //            dataVariable.ExternalName = myRootObject.channel.field1;
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            Log2.Error("DateTime Parse Exception = {0}", ex.Message);
                    //        }
                    //        break;
                    //    }
                    //}

                    // Close the response.
                    ThingsSpeakResp.Close();
                    //return (dataVariable);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log2.Trace(ex.Message);
                throw;
            }
        }



        /// <summary>
        /// SetValue
        /// </summary>
        /// <param name="fieldID"></param>
        /// <param name="writeKey"></param>
        /// <param name="dataVar"></param>
        /// <returns></returns>
        public bool SetValue(string fieldID, string writeKey, DataVariable dataVar)
        {
                // Sample Code

                //const string WRITEKEY = "GJY2ZFxxxxxxxV5X";
                //string strUpdateBase = "http://api.thingspeak.com/update";
                //string strUpdateURI = strUpdateBase + "?key=" + WRITEKEY;
                //string strField1 = "18";
                //string strField2 = "42";
                //HttpWebRequest ThingsSpeakReq;
                //HttpWebResponse ThingsSpeakResp;

                //strUpdateURI += "&field1=" + strField1;
                //strUpdateURI += "&field2=" + strField2;

                //ThingsSpeakReq = (HttpWebRequest)WebRequest.Create(strUpdateURI);

                //ThingsSpeakResp = (HttpWebResponse)ThingsSpeakReq.GetResponse();

                //if (!(string.Equals(ThingsSpeakResp.StatusDescription, "OK")))
                //{
                //    Exception exData = new Exception(ThingsSpeakResp.StatusDescription);
                //    throw exData;
                //}
                      
            string sWriteURI;
            string responseFromServer;
            DataVariable dataVariable = new DataVariable();

            try
            {
                string writeBaseURI = "https://api.thingspeak.com/update";
                //string sChannelID = "317026";
                //                string sfield = "PowerMeter.W_L1_L2";
                //string sfield = "1.json";
                // string sFormat = ".csv";
                //sReadURI = sReadBaseURI + sChannelID + "/fields/" + sfield + "?api_key=";
                //sReadURI = sReadURI + READKEY;
                sWriteURI = writeBaseURI + "?api_key=" + writeKey + "&field" + fieldID + "=" + dataVar.Value;

                Log2.Trace(sWriteURI);

                HttpWebRequest ThingsSpeakReq;
                HttpWebResponse ThingsSpeakResp;
                ThingsSpeakReq = (HttpWebRequest)WebRequest.Create(sWriteURI);

                ThingsSpeakResp = (HttpWebResponse)ThingsSpeakReq.GetResponse();

                if (!(string.Equals(ThingsSpeakResp.StatusDescription, "OK")))
                {
                    Exception exData = new Exception(ThingsSpeakResp.StatusDescription);
                    throw exData;
                }
                else
                {
                    //Log2.Debug("SUCCESS");
                    // Get the stream containing content returned by the server.
                    // The using block ensures the stream is automatically closed.
                    using (Stream dataStream = ThingsSpeakResp.GetResponseStream())
                    {
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        responseFromServer = reader.ReadToEnd();
                        //Log2.Trace(responseFromServer);
                    }
                    // Display the content.

                    Log2.Trace("SetValue Response = {0}", responseFromServer);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log2.Error(ex.Message);
                throw;
            }
        }// end Run


        /// <summary>
        /// SetValue
        /// </summary>
        /// <param name="configString"></param>
        /// <param name="var"></param>
        /// <returns></returns>
        public bool SetValue(string configString, DataVariable var)
        {

            // example string -> "channelID=525xxx5,fieldID=1,readKey=76xxxxx34"
            //ReadKey
            //Channel
            //Field
            //parse config string and getvalue

            TagConfig tagConfig = new TagConfig();
            bool bReturn = true;
            string phrase = null;

            phrase = configString;
            Log2.Trace("Writing ThingSpeak ConfigString: {0}", phrase);
            string[] words = phrase.Split(',');
            foreach (var word in words)
            {
                Log2.Trace(word);
                string[] parms = word.Split('=');
                foreach (var prop in parms)
                {
                    if (String.Equals(parms[0], "channelID"))
                    {
                        tagConfig.channelID = parms[1];
                    }
                    else if (String.Equals(parms[0], "fieldID"))
                    {
                        tagConfig.fieldID = parms[1];
                    }
                    else if (String.Equals(parms[0], "writeKey"))
                    {
                        tagConfig.key = parms[1];
                    }
                }
            }

            Log2.Trace("Writing ThingSpeak Tag: {0}, {1}, {2}", tagConfig.fieldID, tagConfig.key, var.Value);

            bReturn = SetValue(tagConfig.fieldID, tagConfig.key, var);

            Log2.Trace("Write Succeeded: {0}", bReturn.ToString());

            return (true);
        }
        #endregion
    }
}// End Namespace
