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


namespace Upperbay.Worker.LMP
{
    public class PJMRealTimeRTLoad
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
            public DateTime evaluated_at_utc { get; set; }
            public DateTime evaluated_at_ept { get; set; }
            public DateTime forecast_datetime_beginning_utc { get; set; }
            public DateTime forecast_datetime_beginning_ept { get; set; }
            public DateTime forecast_datetime_ending_utc { get; set; }
            public DateTime forecast_datetime_ending_ept { get; set; }
            public string forecast_area { get; set; }
            public float forecast_load_mw { get; set; }
        }

        public PJMRealTimeRTLoad()
        {

        }

        public async Task<double> GetPJMRealTimeLMPAsync()
        {
            double lmp = 0.0;
            var httpClient = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            //get subscription key
            //get pnodeID
            string cluster = MyAppConfig.GetParameter("ClusterName");
            string subscriptionKey = MyAppConfig.GetClusterParameter(cluster, "LMPKey");
            string pnode = MyAppConfig.GetClusterParameter(cluster, "LMPNode");

            // Request headers
            //httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "312xxxxxxxxxxxxxxxxxxxxxxxxxxef8");
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            // Request parameters
            //queryString["pnode_id"] = "2156113753";
            queryString["pnode_id"] = pnode;
            //queryString["download"] = "{boolean}";
            queryString["rowCount"] = "1";
            //queryString["sort"] = "{string}";
            //queryString["order"] = "{string}";
            queryString["startRow"] = "1";
            //queryString["isActiveMetadata"] = "{boolean}";
            //queryString["fields"] = "{string}";
            //queryString["case_approval_datetime_utc"] = "{string}";
            //queryString["case_approval_datetime_ept"] = "{string}";
            //queryString["datetime_beginning_utc"] = "{string}";
            queryString["datetime_beginning_ept"] = "5MinutesAgo";
            var uri = "https://api.pjm.com/api/v1/rt_unverified_fivemin_lmps?" + queryString;

            try
            {
                //Log2.Debug("Calling httpClient.GetAsync");
                HttpResponseMessage httpResponse = await httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
                httpResponse.EnsureSuccessStatusCode(); // throws if not 200-299
                var contentStream = await httpResponse.Content.ReadAsStringAsync();

                //Console.WriteLine(contentStream);
                Log2.Debug("PJM Response: {0}", contentStream);

                Rootobject myRootObject = JsonConvert.DeserializeObject<Rootobject>(contentStream);

                Log2.Debug("PJM Response Rows: {0}", myRootObject.totalRows.ToString());
                int numOfRows = myRootObject.totalRows;
                if (numOfRows > 0)
                {
                    //Console.WriteLine(myRootObject.items[0].datetime_beginning_ept.ToString());
                    //Console.WriteLine(myRootObject.items[0].total_lmp_rt.ToString());
                    Log2.Debug("PJM Response LMP Time: {0}", myRootObject.items[0].forecast_datetime_beginning_ept.ToString());
                    Log2.Debug("PJM Response LMP Value: {0}", myRootObject.items[0].forecast_load_mw.ToString());
                    lmp = myRootObject.items[0].forecast_load_mw;
                }
                else
                    Log2.Debug("PJM Response: Zero Rows Returned");
            }
            catch (Exception ex)
            {
                Log2.Error("PJM Response ERROR: {0}", ex.ToString());
            }
            return lmp;
        }


        /// <summary>
        ///TODO
        /// </summary>
        /// <returns></returns>
        public double GetPJMRealTimeLoadForecast()
        {
            double lmp = 0.0;
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            //get subscription key
            //get pnodeID
            string cluster = MyAppConfig.GetParameter("ClusterName");
            string subscriptionKey = MyAppConfig.GetClusterParameter(cluster, "LMPKey");
            string pnode = MyAppConfig.GetClusterParameter(cluster, "LoadNode");

            var jsonResponse = "";

            // Request headers
            //httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "312249d38xxxxxxxxxxxxxxxx343eef8");
            //httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            // Request parameters
            //queryString["pnode_id"] = "2156113753";
            queryString["pnode_id"] = pnode;
            //queryString["download"] = "{boolean}";
            queryString["rowCount"] = "1";
            //queryString["sort"] = "{string}";
            //queryString["order"] = "{string}";
            queryString["startRow"] = "1";
            //queryString["isActiveMetadata"] = "{boolean}";
            //queryString["fields"] = "{string}";
            //queryString["case_approval_datetime_utc"] = "{string}";
            //queryString["case_approval_datetime_ept"] = "{string}";
            //queryString["datetime_beginning_utc"] = "{string}";
            queryString["datetime_beginning_ept"] = "5MinutesAgo";
            var uri = "https://api.pjm.com/api/v1/rt_unverified_fivemin_lmps?" + queryString;

            try
            {
                //Log2.Debug("Calling httpClient.GetAsync");
                System.Net.WebRequest webRequest = System.Net.WebRequest.Create(uri);
                if (webRequest != null)
                {
                    webRequest.Method = "GET";
                    webRequest.Timeout = 20000;
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

                Rootobject myRootObject = JsonConvert.DeserializeObject<Rootobject>(jsonResponse);

                Log2.Debug("PJM Response Rows: {0}", myRootObject.totalRows.ToString());
                int numOfRows = myRootObject.totalRows;
                if (numOfRows > 0)
                {
                    //Console.WriteLine(myRootObject.items[0].datetime_beginning_ept.ToString());
                    //Console.WriteLine(myRootObject.items[0].total_lmp_rt.ToString());
                    Log2.Debug("PJM Response LMP Time: {0}", myRootObject.items[0].forecast_datetime_beginning_ept.ToString());
                    Log2.Debug("PJM Response LMP Value: {0}", myRootObject.items[0].forecast_load_mw.ToString());
                    lmp = myRootObject.items[0].forecast_load_mw;
                }
                else
                    Log2.Debug("PJM Response: Zero Rows Returned");
            }
            catch (Exception ex)
            {
                Log2.Error("PJM Response ERROR: {0}", ex.ToString());
            }
            return lmp;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool GetPJMRealTimeLoadForecastHistory(string fileName)
        {
            //double lmp = 0.0;
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            //get subscription key
            //get pnodeID
            string cluster = MyAppConfig.GetParameter("ClusterName");
            string subscriptionKey = MyAppConfig.GetClusterParameter(cluster, "LMPKey");
            string loadnode = MyAppConfig.GetClusterParameter(cluster, "LoadNode");
            int rowsRequested = 12;

            var jsonResponse = "";

            //https://api.pjm.com/api/v1/very_short_load_frcst[?download][&rowCount][&sort][&order][&startRow][&isActiveMetadata][&fields][&evaluated_at_utc][&evaluated_at_ept][&forecast_datetime_beginning_utc][&forecast_datetime_beginning_ept][&forecast_datetime_ending_utc][&forecast_datetime_ending_ept][&forecast_area]


            // Request headers
            //httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "3122xxxxxxxxxxxxxxxxxxxxxx43eef8");
            //httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            // Request parameters
            //queryString["pnode_id"] = "2156113753";
            //queryString["pnode_id"] = "49955"; //HACK
            queryString["forecast_area"] = loadnode;
            //            queryString["pnode_id"] = pnode;
            //queryString["download"] = "{boolean}";
            queryString["rowCount"] = rowsRequested.ToString();
            //queryString["sort"] = "{string}";
            //queryString["order"] = "{string}";
            queryString["startRow"] = "1";
            //queryString["isActiveMetadata"] = "{boolean}";
            //queryString["fields"] = "{string}";
            //queryString["case_approval_datetime_utc"] = "{string}";
            //queryString["case_approval_datetime_ept"] = "{string}";
            //queryString["datetime_beginning_utc"] = "{string}";
            queryString["forecast_datetime_beginning_ept"] = "Today";
            //queryString["datetime_beginning_ept"] = "Today";
            //var uri = "https://api.pjm.com/api/v1/rt_unverified_fivemin_lmps?" + queryString;
            var uri = "https://api.pjm.com/api/v1/very_short_load_frcst?" + queryString;

            // load query string
            //[?download]
            //[&rowCount]
            //[&sort]
            //[&order]
            //[&startRow]
            //[&isActiveMetadata]
            //[&fields]
            //[&evaluated_at_utc]
            //[&evaluated_at_ept]
            //[&forecast_datetime_beginning_utc]
            //[&forecast_datetime_beginning_ept]
            //[&forecast_datetime_ending_utc]
            //[&forecast_datetime_ending_ept]
            //[&forecast_area]





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

                Rootobject myRootObject = JsonConvert.DeserializeObject<Rootobject>(jsonResponse);

                Log2.Info("PJM TotalRows Response Rows: {0}", myRootObject.totalRows.ToString());
                Log2.Info("PJM ItemsRows Response Rows: {0}", myRootObject.items.Length.ToString());
                int numOfRows = myRootObject.items.Length;

                using (TextWriter writer1 = File.CreateText((fileName)))
                {
                    writer1.Write(jsonResponse);

                }


                //try
                //{
                //    //int numOfRows = rowsRequested;
                //    if (numOfRows > 0)
                //    {
                //        //Console.WriteLine(myRootObject.items[0].datetime_beginning_ept.ToString());
                //        //Console.WriteLine(myRootObject.items[0].total_lmp_rt.ToString());
                //        Log2.Debug("PJM Response LMP Time: {0}", myRootObject.items[0].datetime_beginning_ept.ToString());
                //        Log2.Debug("PJM Response LMP Value: {0}", myRootObject.items[0].total_lmp_rt.ToString());
                //        using (TextWriter writer = File.AppendText(("C:\\DATA_ARCHIVE\\PjmFiveMinuteLoadArchive.csv")))
                //        {

                //            string time = myRootObject.items[0].datetime_beginning_ept.ToString();
                //            string price = myRootObject.items[0].total_lmp_rt.ToString();
                //            //writer.WriteLine("\"" + time + "\",\"" + price + "\"");

                //            using (TextWriter writer1 = File.CreateText((fileName)))
                //            {
                //                writer1.WriteLine("time,price");
                //                for (int i = 0; i < numOfRows; i++)
                //                {
                //                    string time1 = myRootObject.items[i].datetime_beginning_ept.ToString();
                //                    string price1 = myRootObject.items[i].total_lmp_rt.ToString();
                //                    writer1.WriteLine("\"" + time1 + "\",\"" + price1 + "\"");
                //                    writer.WriteLine("\"" + time1 + "\",\"" + price1 + "\"");
                //                    Log2.Info(time1 + "," + price1);
                //                }
                //            }
                //        }
                //    }
                //    else
                //        Log2.Debug("PJM Response: Zero Rows Returned");
                //}
                //catch (Exception ex)
                //{
                //    Log2.Error("PJM Response ERROR: {0}", ex.ToString());
                //}
            }
            catch (Exception ex)
            {
                Log2.Error("PJM Response ERROR: {0}", ex.ToString());
                return false;
            }

            return true;

        }
    }
}



