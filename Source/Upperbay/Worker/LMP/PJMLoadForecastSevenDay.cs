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
    public class PJMLoadForecastSevenDay
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
            public DateTime evaluated_at_datetime_utc { get; set; }
            public DateTime evaluated_at_datetime_ept { get; set; }
            public DateTime forecast_datetime_beginning_utc { get; set; }
            public DateTime forecast_datetime_beginning_ept { get; set; }
            public DateTime forecast_datetime_ending_utc { get; set; }
            public DateTime forecast_datetime_ending_ept { get; set; }
            public string forecast_area { get; set; }
            public float forecast_load_mw { get; set; }
        }

    
        public PJMLoadForecastSevenDay()
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
            var baseUri = "https://api.pjm.com/api/v1/load_frcstd_7_day?";

            string cluster = MyAppConfig.GetParameter("ClusterName");
            string subscriptionKey = MyAppConfig.GetClusterParameter(cluster, "LMPKey");

            //https://api.pjm.com/api/v1/load_frcstd_7_day
            //[?download]
            //[&rowCount]
            //[&sort]
            //[&order]
            //[&startRow]
            //[&isActiveMetadata]
            //[&fields]
            //[&forecast_datetime_beginning_utc]
            //[&forecast_datetime_beginning_ept]
            //[&forecast_datetime_ending_utc]
            //[&forecast_datetime_ending_ept]
            //[&forecast_area]


            //RTO_COMBINED: Summation of all Zones; MID_ATLANTIC_REGION: AE + BGE + DPL + JCPL + METED + PECO +
            //PENELEC + PEPCO + PPL + PSEG + RECO + UGI; SOUTHERN_REGION: DOM;
            //WESTERN_REGION: AEP + AP + CE + DAY + DUQ + DEOK + FE + EKPC",

       
            // Request headers
            //httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "312249xxxxxxxxxxxxxxxxxxxx43eef8");
            //httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            // Request parameters
            queryString["forecast_area"] = forecastArea;
            //queryString["download"] = "{boolean}";
            queryString["rowCount"] = rowsRequested.ToString();
            //queryString["sort"] = "{string}";
            //queryString["order"] = "{string}";
            queryString["startRow"] = "1";
            //queryString["isActiveMetadata"] = "{boolean}";
            //queryString["fields"] = "{string}";
            //queryString["datetime_beginning_utc"] = "{string}";
            //queryString["forecast_datetime_beginning_ept"] = "Today";
            //queryString["forecast_datetime_ending_ept"] = "NextWeek";

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
                    Log2.Debug("PJM Response Start Time: {0}", myRootObject.items[0].forecast_datetime_beginning_ept.ToString());
                    Log2.Debug("PJM Response Start Value: {0}", myRootObject.items[0].forecast_load_mw.ToString());
                    lastValue = myRootObject.items[numOfRows-1].forecast_load_mw;
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
                    Log2.Debug("PJM Response Start Time: {0}", myRootObject.items[0].forecast_datetime_beginning_ept.ToString());
                    Log2.Debug("PJM Response Start Value: {0}", myRootObject.items[0].forecast_load_mw.ToString());
                    firstValue = myRootObject.items[0].forecast_load_mw;
                    lastValue = myRootObject.items[numOfRows - 1].forecast_load_mw;
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
        public bool WriteJsonToCsv(string jsonString, string filename)
        {
            try
            {

                string archiveDirectory = "C:\\LMP_ARCHIVE";
                string archivePath = "C:\\LMP_ARCHIVE\\PjmLoadForecastSevenDayArchive.csv";

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

               

                using (TextWriter writer = File.AppendText((archivePath)))
                {
                    string time = myRootObject.items[0].forecast_datetime_beginning_ept.ToString();
                    string price = myRootObject.items[0].forecast_load_mw.ToString();
                    //writer.WriteLine("\"" + time + "\",\"" + price + "\"");

                    using (TextWriter writer1 = File.CreateText((filename)))
                    {
                        writer1.WriteLine("Time,Load");
                        for (int i = 0; i < numOfRows; i++)
                        {
                            string time1 = myRootObject.items[i].forecast_datetime_beginning_ept.ToString();
                            string load1 = myRootObject.items[i].forecast_load_mw.ToString();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonString"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool WriteCurrentDayLoadJsonToCsv(string jsonString, string filename)
        {
            try
            {

                string archiveDirectory = "C:\\LMP_ARCHIVE";
                string archivePath = "C:\\LMP_ARCHIVE\\PjmCurrentDayLoadArchive.csv";

                

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
                    string time = myRootObject.items[0].forecast_datetime_beginning_ept.ToString();
                    string price = myRootObject.items[0].forecast_load_mw.ToString();
                    //writer.WriteLine("\"" + time + "\",\"" + price + "\"");

                    using (TextWriter writer1 = File.CreateText((filename)))
                    {
                        writer1.WriteLine("Time,Load");
                        for (int i = 0; i < numOfRows; i++)
                        {
                            DateTime pjmDateTime = myRootObject.items[i].forecast_datetime_beginning_ept;
                            //Console.WriteLine("pjm day: " + pjmDateTime.Day.ToString());
                            //Console.WriteLine("my day: " + myDateTime.Day.ToString());

                            if (pjmDateTime.Day == myDateTime.Day)
                            {
                                if (pjmDateTime >= myDateTime)
                                {
                                    string time1 = myRootObject.items[i].forecast_datetime_beginning_ept.ToString();
                                    string load1 = myRootObject.items[i].forecast_load_mw.ToString();
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

    }
}
