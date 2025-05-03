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
using System.Reflection;
using System.Globalization;


namespace Upperbay.Worker.NWS
{
    public class NWSWeatherForecast
    {

        public NWSWeatherForecast()
        {

        }

        public string GetHourlyForecastInJson(string hourlyUrl)
        {
            var jsonResponse = "";
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            try
            {
                //Log2.Debug("Calling httpClient.GetAsync");
                System.Net.WebRequest webRequest = System.Net.WebRequest.Create(hourlyUrl);
                HttpWebRequest httpRequest = webRequest as HttpWebRequest;
                if (httpRequest != null)
                {
                    httpRequest.Method = "GET";
                    httpRequest.Timeout = 80000;
                    httpRequest.ContentType = "application/json";
                    httpRequest.UserAgent = "MyApp (dave@upperbay.com)";
                    //webRequest.Headers.Add("User-Agent", "MyApp (dave@upperbay.com)");
                    using (System.IO.Stream s = httpRequest.GetResponse().GetResponseStream())
                    {
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                        {
                            jsonResponse = sr.ReadToEnd();
                            Log2.Debug("NWS Response Raw: {0}", jsonResponse);
                        }
                    }
                }

                //Console.WriteLine(contentStream);
                Log2.Debug("NWS Response: {0}", jsonResponse);
            }
            catch (Exception ex)
            {
                Log2.Error("NWS Response ERROR: {0}", ex.ToString());
                return null;
            }

            return jsonResponse;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <returns></returns>
        public string GetWeatherForecastInJson(string lat, string lon)
        {
            var jsonResponse = "";
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            var baseUrl = "https://api.weather.gov";

            //Get lat lon from config
            //string cluster = MyAppConfig.GetParameter("ClusterName");
            //string latitude = MyAppConfig.GetClusterParameter(cluster, "Latitude");
            //string logitude = MyAppConfig.GetClusterParameter(cluster, "Longitude");

            string url = baseUrl + "/points/" + lat + "," + lon;
            
            try
            {
                //Log2.Debug("Calling httpClient.GetAsync");
                System.Net.WebRequest webRequest = System.Net.WebRequest.Create(url);
                HttpWebRequest httpRequest = webRequest as HttpWebRequest;
                if (httpRequest != null)
                {
                    httpRequest.Method = "GET";
                    httpRequest.Timeout = 80000;
                    httpRequest.ContentType = "application/json";
                    httpRequest.UserAgent = "MyApp (dave@upperbay.com)";
                    //webRequest.Headers.Add("User-Agent", "MyApp (dave@upperbay.com)");
                    using (System.IO.Stream s = httpRequest.GetResponse().GetResponseStream())
                    {
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                        {
                            jsonResponse = sr.ReadToEnd();
                            Log2.Debug("NWS Response Raw: {0}", jsonResponse); 
                        }
                    }
                }

                //Console.WriteLine(contentStream);
                Log2.Debug("NWS Response: {0}", jsonResponse);
                JObject json = JObject.Parse(jsonResponse);
                string hourlyUrl = (string)json["properties"]["forecastHourly"];

                string hourlyResponse = GetHourlyForecastInJson(hourlyUrl);

                return hourlyResponse;
            }
            catch (Exception ex)
            {
                Log2.Error("NWS Response ERROR: {0}", ex.ToString());
                return null;
            }

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
        public bool WriteWeatherForecastToCsv(string jsonString, string filename)
        {
            try
            {

                string archiveDirectory = "C:\\LMP_ARCHIVE";
                string archivePath = "C:\\LMP_ARCHIVE\\NWSWeather.csv";



                if (!Directory.Exists(archiveDirectory))
                {
                    Directory.CreateDirectory(archiveDirectory);
                    Log2.Info("Directory created successfully: " + archiveDirectory);
                }

                if (!File.Exists(archivePath))
                    File.Create(archivePath).Close();

                JObject json = JObject.Parse(jsonString);

                // Access the 'employees' array
                JArray periods = (JArray)json["properties"]["periods"];

                // Iterate through each employee in the array
                foreach (JObject period in periods)
                {
                    string startTime = (string)period["startTime"];
                    string temperature = (string)period["temperature"];
                    string temperatureUnit = (string)period["temperatureUnit"];
                    string shortForecast = (string)period["shortForecast"];
                    Log2.Debug ($"Time: {startTime}, Temp: {temperature}, Forecast: {shortForecast}");
                }

                DateTime myDateTime = DateTime.Now;

                using (TextWriter writer1 = File.CreateText((filename)))
                {
                    writer1.WriteLine("Time,Temp,Forecast");
                    foreach (JObject period in periods)
                    {
                        string startTime = (string)period["startTime"];
                        string temperature = (string)period["temperature"];
                        string temperatureUnit = (string)period["temperatureUnit"];
                        string shortForecast = (string)period["shortForecast"];
                        writer1.WriteLine("\"" + startTime + "\",\"" + temperature + "\",\"" + shortForecast + "\"");
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

        public bool WriteTodaysWeatherForecastToCsv(string jsonString, string filename)
        {
            try
            {

                string archiveDirectory = "C:\\LMP_ARCHIVE";
                string archivePath = "C:\\LMP_ARCHIVE\\NWSWeather.csv";



                if (!Directory.Exists(archiveDirectory))
                {
                    Directory.CreateDirectory(archiveDirectory);
                    Log2.Info("Directory created successfully: " + archiveDirectory);
                }

                if (!File.Exists(archivePath))
                    File.Create(archivePath).Close();

                JObject json = JObject.Parse(jsonString);

                // Access the 'employees' array
                JArray periods = (JArray)json["properties"]["periods"];

                // Iterate through each employee in the array
                foreach (JObject period in periods)
                {
                    string startTime = (string)period["startTime"];
                    string temperature = (string)period["temperature"];
                    string temperatureUnit = (string)period["temperatureUnit"];
                    string shortForecast = (string)period["shortForecast"];
                    Log2.Debug($"Time: {startTime}, Temp: {temperature}, Forecast: {shortForecast}");
                }

                DateTime myDateTime = DateTime.Now;

                using (TextWriter writer1 = File.CreateText((filename)))
                {
                    writer1.WriteLine("Time,Temp,Forecast");
                    foreach (JObject period in periods)
                    {
                        string startTime = (string)period["startTime"];
                        DateTime parsedDateTime;
                        bool isValidDate = DateTime.TryParse(startTime, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDateTime);
                        if (isValidDate)
                        {
                            // Compare parsed date with today's date
                            if (parsedDateTime.Date == DateTime.Today)
                            {
                                //Console.WriteLine("The date is today.");

                                string temperature = (string)period["temperature"];
                                string temperatureUnit = (string)period["temperatureUnit"];
                                string shortForecast = (string)period["shortForecast"];
                                writer1.WriteLine("\"" + startTime + "\",\"" + temperature + "\",\"" + shortForecast + "\"");
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
        /// <param name="jsonResponse"></param>
        /// <returns></returns>
        //public double GetFirstValue(string jsonResponse)
        //{
        //    double lastValue = 0;
        //    double firstValue = 0;

        //    try
        //    {
        //        Rootobject myRootObject = JsonConvert.DeserializeObject<Rootobject>(jsonResponse);

        //        int numOfRows = myRootObject.items.Length;
        //        Log2.Debug("NWS Response Rows: {0}", numOfRows.ToString());
        //        if (numOfRows > 0)
        //        {
        //            //Console.WriteLine(myRootObject.items[0].datetime_beginning_ept.ToString());
        //            //Console.WriteLine(myRootObject.items[0].total_lmp_rt.ToString());
        //            Log2.Debug("NWS Response Start Time: {0}", myRootObject.items[0].datetime_beginning_ept.ToString());
        //            Log2.Debug("NWS Response Start Value: {0}", myRootObject.items[0].total_lmp_da.ToString());
        //            firstValue = myRootObject.items[0].total_lmp_da;
        //            lastValue = myRootObject.items[numOfRows - 1].total_lmp_da;
        //            Log2.Debug("NWS Response First Value: {0}", firstValue.ToString());
        //            Log2.Debug("NWS Response Last Value: {0}", lastValue.ToString());
        //        }
        //        else
        //            Log2.Debug("NWS Response: Zero Rows Returned");
        //    }
        //    catch (Exception ex)
        //    {
        //        Log2.Error("NWS Response ERROR: {0}", ex.ToString());
        //    }
        //    return firstValue;
        //}
    }
}




