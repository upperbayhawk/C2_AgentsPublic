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
    public class PJMDayAheadHourlyLMP
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


        // "datetime_beginning_utc": "2024-02-06T05:00:00",
        //"datetime_beginning_ept": "2024-02-06T00:00:00",
        //"pnode_id": 1,
        //"pnode_name": "PJM-RTO",
        //"voltage": null,
        //"equipment": null,
        //"type": "ZONE",
        //"zone": null,
        //"system_energy_price_da": 25.71,
        //"total_lmp_da": 26.080456,
        //"congestion_price_da": 0.184252,
        //"marginal_loss_price_da": 0.186204,
        //"row_is_current": true,
        //"version_nbr": 1


        public class Item
        {
            public DateTime datetime_beginning_utc { get; set; }
            public DateTime datetime_beginning_ept { get; set; }
            public Int64 pnode_id { get; set; }
            public string pnode_name { get; set; }
            public string voltage { get; set; }
            public string equipment { get; set; }
            public string type { get; set; }
            public string zone { get; set; }
            public string system_energy_price_da { get; set; }
            public float total_lmp_da { get; set; }
            public float congestion_price_da { get; set; }
            public float marginal_loss_price_da { get; set; }
            public bool row_is_current { get; set; }
            public bool version_nbr { get; set; }
        }

        public PJMDayAheadHourlyLMP()
        {

        }

        //public async Task<double> GetPJMRealTimeLMPAsync()
        //{
        //    double lmp = 0.0;
        //    var httpClient = new HttpClient();
        //    var queryString = HttpUtility.ParseQueryString(string.Empty);

        //    //get subscription key
        //    //get pnodeID
        //    string cluster = MyAppConfig.GetParameter("ClusterName");
        //    string subscriptionKey = MyAppConfig.GetClusterParameter(cluster, "LMPKey");
        //    string pnode = MyAppConfig.GetClusterParameter(cluster, "LMPNode");

        //    // Request headers
        //    //httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "312249d38ae64xxxxxxxxxxxxxxxeef8");
        //    httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

        //    // Request parameters
        //    //queryString["pnode_id"] = "2156113753";
        //    queryString["pnode_id"] = pnode;
        //    //queryString["download"] = "{boolean}";
        //    queryString["rowCount"] = "1";
        //    //queryString["sort"] = "{string}";
        //    //queryString["order"] = "{string}";
        //    queryString["startRow"] = "1";
        //    //queryString["isActiveMetadata"] = "{boolean}";
        //    //queryString["fields"] = "{string}";
        //    //queryString["case_approval_datetime_utc"] = "{string}";
        //    //queryString["case_approval_datetime_ept"] = "{string}";
        //    //queryString["datetime_beginning_utc"] = "{string}";
        //    queryString["datetime_beginning_ept"] = "5MinutesAgo";
        //    var uri = "https://api.pjm.com/api/v1/rt_unverified_fivemin_lmps?" + queryString;

        //    try
        //    {
        //        //Log2.Debug("Calling httpClient.GetAsync");
        //        HttpResponseMessage httpResponse = await httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
        //        httpResponse.EnsureSuccessStatusCode(); // throws if not 200-299
        //        var contentStream = await httpResponse.Content.ReadAsStringAsync();

        //        //Console.WriteLine(contentStream);
        //        Log2.Debug("PJM Response: {0}", contentStream);

        //        Rootobject myRootObject = JsonConvert.DeserializeObject<Rootobject>(contentStream);

        //        Log2.Debug("PJM Response Rows: {0}", myRootObject.totalRows.ToString());
        //        int numOfRows = myRootObject.totalRows;
        //        if (numOfRows > 0)
        //        {
        //            //Console.WriteLine(myRootObject.items[0].datetime_beginning_ept.ToString());
        //            //Console.WriteLine(myRootObject.items[0].total_lmp_rt.ToString());
        //            Log2.Debug("PJM Response LMP Time: {0}", myRootObject.items[0].datetime_beginning_ept.ToString());
        //            Log2.Debug("PJM Response LMP Value: {0}", myRootObject.items[0].total_lmp_rt.ToString());
        //            lmp = myRootObject.items[0].total_lmp_rt;
        //        }
        //        else
        //            Log2.Debug("PJM Response: Zero Rows Returned");
        //    }
        //    catch (Exception ex)
        //    {
        //        Log2.Error("PJM Response ERROR: {0}", ex.ToString());
        //    }
        //    return lmp;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="forecastArea"></param>
        /// <param name="rowsRequested"></param>
        /// <returns></returns>
        public string GetJson(string pricing_node, int rowsRequested)
        {
            var jsonResponse = "";
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            var baseUri = "https://api.pjm.com/api/v1/da_hrl_lmps?";

            string cluster = MyAppConfig.GetParameter("ClusterName");
            string subscriptionKey = MyAppConfig.GetClusterParameter(cluster, "LMPKey");

           
           
            // Request headers
            //httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "312249d38xxxxxxxxxxxxxxxx343eef8");
            //httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            // Request parameters
            
            //queryString["download"] = "{boolean}";
            queryString["rowCount"] = rowsRequested.ToString();
            //queryString["sort"] = "{string}";
            //queryString["order"] = "{string}";
            queryString["startRow"] = "1";
            //queryString["isActiveMetadata"] = "{boolean}";
            //queryString["fields"] = "{string}";
            //queryString["datetime_beginning_utc"] = "{string}";
            queryString["datetime_beginning_ept"] = "Today";
            //queryString["forecast_datetime_ending_ept"] = "NextWeek";
            //queryString["row_is_current"] = "true";
            queryString["pnode_id"] = pricing_node;

            var uri = baseUri + queryString;
            Log2.Debug("PJM Url: " + uri);

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
        public bool WriteCurrentDayAheadHourlyLMPToCsv(string jsonString, string filename)
        {
            try
            {

                string archiveDirectory = "C:\\LMP_ARCHIVE";
                string archivePath = "C:\\LMP_ARCHIVE\\PjmCurrentDayAheadHourlyLMPArchive.csv";



                if (!Directory.Exists(archiveDirectory))
                {
                    Directory.CreateDirectory(archiveDirectory);
                    Log2.Info("Directory created successfully: " + archiveDirectory);
                }

                if (!File.Exists(archivePath))
                    File.Create(archivePath).Close();

                Rootobject myRootObject = JsonConvert.DeserializeObject<Rootobject>(jsonString);

                Log2.Info("PJM TotalRows Response Rows: {0}", myRootObject.totalRows.ToString());
                Log2.Info("PJM ItemsRows Response Rows: {0}", myRootObject.items.Length.ToString());
                int numOfRows = myRootObject.items.Length;


                DateTime myDateTime = DateTime.Now;

                using (TextWriter writer = File.AppendText((archivePath)))
                {
                    string time = myRootObject.items[0].datetime_beginning_ept.ToString();
                    string price = myRootObject.items[0].total_lmp_da.ToString();
                    //writer.WriteLine("\"" + time + "\",\"" + price + "\"");

                    using (TextWriter writer1 = File.CreateText((filename)))
                    {
                        writer1.WriteLine("Time,Price");
                        for (int i = 0; i < numOfRows; i++)
                        {
                            DateTime pjmDateTime = myRootObject.items[i].datetime_beginning_ept;
                            //Console.WriteLine("pjm day: " + pjmDateTime.Day.ToString());
                            //Console.WriteLine("my day: " + myDateTime.Day.ToString());

                            if (pjmDateTime.Day == myDateTime.Day)
                            {
                                if (pjmDateTime >= myDateTime)
                                {
                                    string time1 = myRootObject.items[i].datetime_beginning_ept.ToString();
                                    string load1 = myRootObject.items[i].total_lmp_da.ToString();
                                    writer1.WriteLine("\"" + time1 + "\",\"" + load1 + "\"");
                                    writer.WriteLine("\"" + time1 + "\",\"" + load1 + "\"");
                                    Log2.Info(time1 + "," + load1);
                                }
                            }
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



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //public double GetPJmDayAheadHourlyLMPs()
        //{
        //    double lmp = 0.0;
        //    var queryString = HttpUtility.ParseQueryString(string.Empty);

        //    //get subscription key
        //    //get pnodeID
        //    string cluster = MyAppConfig.GetParameter("ClusterName");
        //    string subscriptionKey = MyAppConfig.GetClusterParameter(cluster, "LMPKey");
        //    string pnode = MyAppConfig.GetClusterParameter(cluster, "LMPNode");

        //    var jsonResponse = "";

        //    // Request headers
        //    //httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "3122xxxxxxxxxxxxxxxxxx6f8343eef8");
        //    //httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

        //    // Request parameters
        //    //queryString["pnode_id"] = "2156113753";
        //    queryString["pnode_id"] = "1";
        //    //queryString["pnode_id"] = pnode;
        //    //queryString["download"] = "{boolean}";
        //    queryString["rowCount"] = "24";
        //    //queryString["sort"] = "{string}";
        //    //queryString["order"] = "{string}";
        //    queryString["startRow"] = "1";
        //    //queryString["isActiveMetadata"] = "{boolean}";
        //    //queryString["fields"] = "{string}";
        //    //queryString["case_approval_datetime_utc"] = "{string}";
        //    //queryString["case_approval_datetime_ept"] = "{string}";
        //    //queryString["datetime_beginning_utc"] = "{string}";
        //    queryString["datetime_beginning_ept"] = "Today";
        //    var uri = "https://api.pjm.com/api/v1/da_hrl_lmps?" + queryString;

        //    try
        //    {
        //        //Log2.Debug("Calling httpClient.GetAsync");
        //        System.Net.WebRequest webRequest = System.Net.WebRequest.Create(uri);
        //        if (webRequest != null)
        //        {
        //            webRequest.Method = "GET";
        //            webRequest.Timeout = 20000;
        //            webRequest.ContentType = "application/json";
        //            webRequest.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
        //            using (System.IO.Stream s = webRequest.GetResponse().GetResponseStream())
        //            {
        //                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
        //                {
        //                    jsonResponse = sr.ReadToEnd();
        //                    //Log2.Debug("PJM Response Raw: {0}", jsonResponse); 
        //                }
        //            }
        //        }

        //        //Console.WriteLine(contentStream);
        //        Log2.Debug("PJM Response: {0}", jsonResponse);

        //        Rootobject myRootObject = JsonConvert.DeserializeObject<Rootobject>(jsonResponse);

        //        Log2.Debug("PJM Response Rows: {0}", myRootObject.totalRows.ToString());
        //        int numOfRows = myRootObject.totalRows;
        //        if (numOfRows > 0)
        //        {
        //            //Console.WriteLine(myRootObject.items[0].datetime_beginning_ept.ToString());
        //            //Console.WriteLine(myRootObject.items[0].total_lmp_rt.ToString());
        //            Log2.Debug("PJM Response LMP Time: {0}", myRootObject.items[0].datetime_beginning_ept.ToString());
        //            Log2.Debug("PJM Response LMP Value: {0}", myRootObject.items[0].total_lmp_da.ToString());
        //            lmp = myRootObject.items[0].total_lmp_da;
        //        }
        //        else
        //            Log2.Debug("PJM Response: Zero Rows Returned");
        //    }
        //    catch (Exception ex)
        //    {
        //        Log2.Error("PJM Response ERROR: {0}", ex.ToString());
        //    }
        //    return lmp;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonResponse"></param>
        /// <returns></returns>
        public double GetFirstValue(string jsonResponse)
        {
            double lastValue = 0;
            double firstValue = 0;

            try
            {
                Rootobject myRootObject = JsonConvert.DeserializeObject<Rootobject>(jsonResponse);

                int numOfRows = myRootObject.items.Length;
                Log2.Debug("PJM Response Rows: {0}", numOfRows.ToString());
                if (numOfRows > 0)
                {
                    //Console.WriteLine(myRootObject.items[0].datetime_beginning_ept.ToString());
                    //Console.WriteLine(myRootObject.items[0].total_lmp_rt.ToString());
                    Log2.Debug("PJM Response Start Time: {0}", myRootObject.items[0].datetime_beginning_ept.ToString());
                    Log2.Debug("PJM Response Start Value: {0}", myRootObject.items[0].total_lmp_da.ToString());
                    firstValue = myRootObject.items[0].total_lmp_da;
                    lastValue = myRootObject.items[numOfRows - 1].total_lmp_da;
                    Log2.Debug("PJM Response First Value: {0}", firstValue.ToString());
                    Log2.Debug("PJM Response Last Value: {0}", lastValue.ToString());
                }
                else
                    Log2.Debug("PJM Response: Zero Rows Returned");
            }
            catch (Exception ex)
            {
                Log2.Error("PJM Response ERROR: {0}", ex.ToString());
            }
            return firstValue;
        }


        

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public bool GetPJMDayAheadHourlyLMPHistory(string fileName)
        //{
        //    //double lmp = 0.0;
        //    var queryString = HttpUtility.ParseQueryString(string.Empty);

        //    //get subscription key
        //    //get pnodeID
        //    string cluster = MyAppConfig.GetParameter("ClusterName");
        //    string subscriptionKey = MyAppConfig.GetClusterParameter(cluster, "LMPKey");
        //    string pnode = MyAppConfig.GetClusterParameter(cluster, "LMPNode");
        //    int rowsRequested = 12;

        //    var jsonResponse = "";

        //    // Request headers
        //    //httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "312249d3xxxxxxxxxxxxxxxx8343eef8");
        //    //httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

        //    // Request parameters
        //    //queryString["pnode_id"] = "2156113753";
        //    queryString["pnode_id"] = "1"; //HACK
        //    //            queryString["pnode_id"] = pnode;
        //    //queryString["download"] = "{boolean}";
        //    queryString["rowCount"] = rowsRequested.ToString();
        //    //queryString["sort"] = "{string}";
        //    //queryString["order"] = "{string}";
        //    queryString["startRow"] = "1";
        //    //queryString["isActiveMetadata"] = "{boolean}";
        //    //queryString["fields"] = "{string}";
        //    //queryString["case_approval_datetime_utc"] = "{string}";
        //    //queryString["case_approval_datetime_ept"] = "{string}";
        //    //queryString["datetime_beginning_utc"] = "{string}";
        //    queryString["datetime_beginning_ept"] = "Today";
        //    var uri = "https://api.pjm.com/api/v1/da_hrl_lmps?" + queryString;

        //    try
        //    {
        //        //Log2.Debug("Calling httpClient.GetAsync");
        //        System.Net.WebRequest webRequest = System.Net.WebRequest.Create(uri);
        //        if (webRequest != null)
        //        {
        //            webRequest.Method = "GET";
        //            webRequest.Timeout = 80000;
        //            webRequest.ContentType = "application/json";
        //            webRequest.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
        //            using (System.IO.Stream s = webRequest.GetResponse().GetResponseStream())
        //            {
        //                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
        //                {
        //                    jsonResponse = sr.ReadToEnd();
        //                    //Log2.Debug("PJM Response Raw: {0}", jsonResponse); 
        //                }
        //            }
        //        }

        //        //Console.WriteLine(contentStream);
        //        Log2.Debug("PJM Response: {0}", jsonResponse);

        //        Rootobject myRootObject = JsonConvert.DeserializeObject<Rootobject>(jsonResponse);

        //        Log2.Info("PJM TotalRows Response Rows: {0}", myRootObject.totalRows.ToString());
        //        Log2.Info("PJM ItemsRows Response Rows: {0}", myRootObject.items.Length.ToString());
        //        int numOfRows = myRootObject.items.Length;

        //        try
        //        {
        //            //int numOfRows = rowsRequested;
        //            if (numOfRows > 0)
        //            {
        //                //Console.WriteLine(myRootObject.items[0].datetime_beginning_ept.ToString());
        //                //Console.WriteLine(myRootObject.items[0].total_lmp_rt.ToString());
        //                Log2.Debug("PJM Response LMP Time: {0}", myRootObject.items[0].datetime_beginning_ept.ToString());
        //                Log2.Debug("PJM Response LMP Value: {0}", myRootObject.items[0].total_lmp_da.ToString());
        //                using (TextWriter writer = File.AppendText(("C:\\DATA_ARCHIVE\\PjmDayAheadHourlyLMPArchive.csv")))
        //                {

        //                    string time = myRootObject.items[0].datetime_beginning_ept.ToString();
        //                    string price = myRootObject.items[0].total_lmp_da.ToString();
        //                    //writer.WriteLine("\"" + time + "\",\"" + price + "\"");

        //                    using (TextWriter writer1 = File.CreateText((fileName)))
        //                    {
        //                        writer1.WriteLine("time,price");
        //                        for (int i = 0; i < numOfRows; i++)
        //                        {
        //                            string time1 = myRootObject.items[i].datetime_beginning_ept.ToString();
        //                            string price1 = myRootObject.items[i].total_lmp_da.ToString();
        //                            writer1.WriteLine("\"" + time1 + "\",\"" + price1 + "\"");
        //                            writer.WriteLine("\"" + time1 + "\",\"" + price1 + "\"");
        //                            Log2.Info(time1 + "," + price1);
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //                Log2.Debug("PJM Response: Zero Rows Returned");
        //        }
        //        catch (Exception ex)
        //        {
        //            Log2.Error("PJM Response ERROR: {0}", ex.ToString());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log2.Error("PJM Response ERROR: {0}", ex.ToString());
        //        return false;
        //    }

        //    return true;

        //}
      
    }
}




