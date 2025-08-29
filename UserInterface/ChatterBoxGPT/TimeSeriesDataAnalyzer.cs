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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upperbay.Core.Logging;
using Upperbay.Core.Library;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ChatterBoxGPT
{
    public static class TimeSeriesDataAnalyzer
    {
        private class DataPoint
        {
            public DateTime Time { get; set; }
            public double Value { get; set; }
        }


        /// <summary>
        /// 
        /// </summary>
        public static void Run(string filename)
        {
            try
            {
                List<DataPoint> dataPoints = ReadCsv(filename);

                double average = dataPoints.Average(dp => dp.Value);
                double maximum = dataPoints.Max(dp => dp.Value);
                double minimum = dataPoints.Min(dp => dp.Value);

                Log2.Info($"Time = " + dataPoints.First().Time.ToString());
                Log2.Info($"Code Average: {average:F2}");
                Log2.Info($"Code Maximum: {maximum:F2}");
                Log2.Info($"Code Minimum: {minimum:F2}");

                Console.WriteLine($"Time = " + dataPoints.First().Time.ToString());
                Console.WriteLine($"Code Average: {average:F2}");
                Console.WriteLine($"Code Maximum: {maximum:F2}");
                Console.WriteLine($"Code Minimum: {minimum:F2}");

                PrintValues(dataPoints);

                List<DataPoint> runningAverages = CalculateRunningAverages(dataPoints, 3);
                PrintRunningAverages(runningAverages);

                int hours = AnalyzeSlopes(runningAverages);
                AnalyzeNewestRunningAverage(runningAverages, hours);
            }
            catch(Exception ex) 
            {
                Log2.Error("Response ERROR: {0}", ex.ToString());
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private static List<DataPoint> ReadCsv(string filename)
        {
            List<DataPoint> dataPoints = new List<DataPoint>();

            try
            {
                using (StreamReader sr = new StreamReader(filename))
                {
                    sr.ReadLine(); // Skip header

                    while (!sr.EndOfStream)
                    {
                        string[] tokens = sr.ReadLine().Split(',');

                        string time = Regex.Replace(tokens[0], @"(\[|""|\])", "");
                        string data = Regex.Replace(tokens[1], @"(\[|""|\])", "");

                        Log2.Info("RAW Date = " + tokens[0] + ", " + "RAW Value = " + tokens[1]);

                        dataPoints.Add(new DataPoint
                        {
                            Time = DateTime.ParseExact(time, 
                                                        "M/d/yyyy h:mm:ss tt", 
                                                        new CultureInfo("en-US"), 
                                                        DateTimeStyles.AssumeLocal),
                            Value = double.Parse(data)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Log2.Error("Response ERROR: {0}", ex.ToString());
            }

            return dataPoints;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataPoints"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        private static List<DataPoint> CalculateRunningAverages(List<DataPoint> dataPoints, int interval)
        {
            List<DataPoint> runningAverages = new List<DataPoint>();

            //for (int i = 0; i <= dataPoints.Count - interval; i++)
            //{
            //    double average = dataPoints.Skip(i).Take(interval).Average(dp => dp.Value);

            //    runningAverages.Add(new DataPoint
            //    {
            //        Time = dataPoints[i].Time,
            //        Value = average
            //    });

            //}

            for (int i = 0; i <= dataPoints.Count - interval; i++)
            {
                double average = dataPoints.Skip(i).Take(interval).Average(dp => dp.Value);

                List<DataPoint> rawdog = dataPoints.Skip(i).Take(interval).ToList<DataPoint>();

                for (int j = 0; j < rawdog.Count; j++)
                {
                    Log2.Info($"RAW Time: {rawdog[j].Time}, Value: {rawdog[j].Value:F2}");
                }
               

                runningAverages.Add(new DataPoint
                {
                    Time = dataPoints[i].Time,
                    Value = average
                });
                Log2.Info($"RAW Average = {runningAverages[i].Time}, Value = {runningAverages[i].Value}\n");

                // csv to PjmFiveMinuteRunningAverages.csv
             
                using (TextWriter writer = File.AppendText(("C:\\DATA_ARCHIVE\\PjmFiveMinuteRunningAverages.csv")))
                {
                    writer.WriteLine("\"" + runningAverages[i].Time + "\",\"" + runningAverages[i].Value + "\"");
                }

            }

            return runningAverages;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        private static void PrintValues(List<DataPoint> values)
        {
            Console.WriteLine("\nCode Values:");
            Log2.Info("\nCode Values:");

            foreach (DataPoint dp in values)
            {
                Console.WriteLine($"Code Time: {dp.Time}, Value: {dp.Value:F2}");
                Log2.Info($"Code Time: {dp.Time}, Value: {dp.Value:F2}");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="runningAverages"></param>
        private static void PrintRunningAverages(List<DataPoint> runningAverages)
        {
            Console.WriteLine("\nCode Running Averages:");
            Log2.Info("\nCode Running Averages:");

            foreach (DataPoint dp in runningAverages)
            {
                Console.WriteLine($"Code Time: {dp.Time}, Average: {dp.Value:F2}");
                Log2.Info($"Code Time: {dp.Time}, Average: {dp.Value:F2}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="runningAverages"></param>
        private static void AnalyzeNewestRunningAverage(List<DataPoint> runningAverages, int hours)
        {
            DataPoint newestRunningAverage = runningAverages.First();
            Console.WriteLine("\nCode Analysis:");
            Log2.Info("\nCode Analysis:");

            if (newestRunningAverage.Value > 60.00)
            {
                Console.WriteLine("GAMEON GOLD for " + hours);
                Log2.Info("GAMEON GOLD for " + hours);
            }
            else if (newestRunningAverage.Value > 50.00)
            {
                Console.WriteLine("GAMEON SILVER for " + hours);
                Log2.Info("GAMEON SILVER for " + hours);
            }
            else if (newestRunningAverage.Value > 40.00)
            {
                Console.WriteLine("GAMEON BRONZE for " + hours);
                Log2.Info("GAMEON BRONZE for " + hours);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="runningAverages"></param>
        private static int AnalyzeSlopes(List<DataPoint> runningAverages)
        {
            double slope1 = runningAverages[1].Value - runningAverages[0].Value;
            double slope2 = runningAverages[2].Value - runningAverages[1].Value;

            if (slope1 > 0 && slope2 > 0)
            {
                Console.WriteLine("Code 3 HOURS");
                Log2.Info("Code 3 HOURS");
                return 3;
            }
            else if (slope2 > 0)
            {
                Console.WriteLine("Code 2 HOURS");
                Log2.Info("Code 2 HOURS");
                return 2;
            }
            else
            {
                Console.WriteLine("Code 1 HOUR");
                Log2.Info("Code 1 HOUR");
                return 1;
            }
        }
    }
}
