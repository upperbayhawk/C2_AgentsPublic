//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

using Newtonsoft.Json;

using Upperbay.Core.Logging;
using Upperbay.Core.Library;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Web.UI;


namespace Upperbay.Worker.LMP
{
    public class PJMHourLoadPrelim
    {
        public class Rootobject
        {
            public Link[] links { get; set; }
            public Item[] items { get; set; }
            public Searchspecification searchSpecification { get; set; }
            public int totalRows { get; set; }
        }

        public class Searchspecification
        {
            public int rowCount { get; set; }
            public string sort { get; set; }
            public string order { get; set; }
            public int startRow { get; set; }
            public bool isActiveMetadata { get; set; }
            public string[] fields { get; set; }
            public Filter[] filters { get; set; }
        }

        public class Filter
        {
            public string datetime_beginning_ept { get; set; }
        }

        public class Link
        {
            public string rel { get; set; }
            public string href { get; set; }
        }

        public class Item
        {
            public DateTime datetime_beginning_utc { get; set; }
            public DateTime datetime_beginning_ept { get; set; }
            public DateTime datetime_ending_utc { get; set; }
            public DateTime datetime_ending_ept { get; set; }
            public string load_area { get; set; }
            public float prelim_load_avg_hourly { get; set; }
        }
        public PJMHourLoadPrelim()
        {

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="forecastArea"></param>
        /// <param name="rowsRequested"></param>
        /// <returns></returns>
        public string GetJson(string forecastArea, int rowsRequested)
        {
            var jsonResponse = "";
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            var baseUri = "https://api.pjm.com/api/v1/hrl_load_prelim?";

            string cluster = MyAppConfig.GetParameter("ClusterName");
            string subscriptionKey = MyAppConfig.GetClusterParameter(cluster, "LMPKey");

            //https://api.pjm.com/api/v1/hrl_load_prelim
            //[?download]
            //[&rowCount]
            //[&sort]
            //[&order]
            //[&startRow]
            //[&isActiveMetadata]
            //[&fields]
            //[&datetime_beginning_utc]
            //[&datetime_beginning_ept]
            //[&datetime_ending_utc]
            //[&datetime_ending_ept]
            //[&load_area]

            // load_area: AE, AECO, AEPAPT, AEPIMP, AEPKPT, AEPOPT, AP, BC, CE, DAY,
            // DEOK, DOM, DPLCO, DUQ, EASTON, EKPC, JC, ME, OE, PAPWR, PE, PEPCO, PLCO,
            // PN, PS, RECO, RTO, SMECO, UGI, OVEC.

            // Request headers
            //httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "3122xxxxxxxxxxxxxxxxxxxxxxxxxxf8");
            //httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            // Request parameters
            queryString["load_area"] = forecastArea;
            //queryString["download"] = "{boolean}";
            queryString["rowCount"] = rowsRequested.ToString();
            //queryString["sort"] = "{string}";
            //queryString["order"] = "{string}";
            queryString["startRow"] = "1";
            //queryString["isActiveMetadata"] = "{boolean}";
            //queryString["fields"] = "{string}";
            //queryString["datetime_beginning_utc"] = "{string}";
            queryString["datetime_beginning_ept"] = "Today";

            var uri = baseUri + queryString;

            try
            {
                //Log2.Debug("Calling httpClient.GetAsync");
                System.Net.WebRequest webRequest = System.Net.WebRequest.Create(uri);
                if (webRequest != null)
                {
                    webRequest.Method = "GET";
                    webRequest.Timeout = 80000;
                    webRequest.ContentType = "application/json";
                    webRequest.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                    using (System.IO.Stream s = webRequest.GetResponse().GetResponseStream())
                    {
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                        {
                            jsonResponse = sr.ReadToEnd();
                            //Log2.Debug("PJM Response Raw: {0}", jsonResponse); 
                        }
                    }
                }

                //Console.WriteLine(contentStream);
                Log2.Debug("PJM Response: {0}", jsonResponse);
            }
            catch (Exception ex)
            {
                Log2.Error("PJM Response ERROR: {0}", ex.ToString());
                return null;
            }

            return jsonResponse;

        }
       


       /// <summary>
       /// 
       /// </summary>
       /// <param name="jsonResponse"></param>
       /// <returns></returns>
        public double GetLastValue(string jsonResponse)
        {
            double lastValue = 0; 
            
            try 
            { 
                Rootobject myRootObject = JsonConvert.DeserializeObject<Rootobject>(jsonResponse);

                int numOfRows = myRootObject.items.Length;
                Log2.Debug("PJM Response Rows: {0}", numOfRows.ToString());
                if (numOfRows > 0)
                {
                    //Console.WriteLine(myRootObject.items[0].datetime_beginning_ept.ToString());
                    //Console.WriteLine(myRootObject.items[0].total_lmp_rt.ToString());
                    Log2.Debug("PJM Response Start Time: {0}", myRootObject.items[0].datetime_ending_ept.ToString());
                    Log2.Debug("PJM Response Start Value: {0}", myRootObject.items[0].prelim_load_avg_hourly.ToString());
                    lastValue = myRootObject.items[numOfRows-1].prelim_load_avg_hourly;
                    Log2.Debug("PJM Response Last Value: {0}", lastValue.ToString());
                }
                else
                    Log2.Debug("PJM Response: Zero Rows Returned");
            }
            catch (Exception ex)
            {
                Log2.Error("PJM Response ERROR: {0}", ex.ToString());
            }
            return lastValue;
        }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="jsonString"></param>
      /// <param name="filename"></param>
      /// <returns></returns>
        public bool WriteJsonToFile(string jsonString, string filename)
        {
            try
            {
                if (filename != null)
                {
                    using (TextWriter writer1 = File.CreateText((filename)))
                    {
                        writer1.Write(jsonString);
                    }
                }
            }
            catch (Exception ex)
            {
                Log2.Error("WriteJsonToFile ERROR: {0}", ex.ToString());
                return false;
            }
            return true;
        }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="jsonString"></param>
      /// <param name="filename"></param>
      /// <returns></returns>
        public bool WriteCsvToFile(string jsonString, string filename)
        {
            try
            {
                Rootobject myRootObject = JsonConvert.DeserializeObject<Rootobject>(jsonString);

                Log2.Info("PJM TotalRows Response Rows: {0}", myRootObject.totalRows.ToString());
                Log2.Info("PJM ItemsRows Response Rows: {0}", myRootObject.items.Length.ToString());
                int numOfRows = myRootObject.items.Length;

                using (TextWriter writer = File.AppendText(("C:\\DATA_ARCHIVE\\PjmFiveMinuteLoadArchive.csv")))
                {
                    string time = myRootObject.items[0].datetime_beginning_ept.ToString();
                    string price = myRootObject.items[0].prelim_load_avg_hourly.ToString();
                    //writer.WriteLine("\"" + time + "\",\"" + price + "\"");

                    using (TextWriter writer1 = File.CreateText((filename)))
                    {
                        writer1.WriteLine("time,load");
                        for (int i = 0; i < numOfRows; i++)
                        {
                            string time1 = myRootObject.items[i].datetime_beginning_ept.ToString();
                            string load1 = myRootObject.items[i].prelim_load_avg_hourly.ToString();
                            writer1.WriteLine("\"" + time1 + "\",\"" + load1 + "\"");
                            writer.WriteLine("\"" + time1 + "\",\"" + load1 + "\"");
                            Log2.Info(time1 + "," + load1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log2.Error("WriteCsvToFile ERROR: {0}", ex.ToString());
                return false;
            }
            return true;
        }
    }
}
