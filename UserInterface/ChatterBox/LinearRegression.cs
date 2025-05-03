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
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearRegression;

namespace ChatterBox
{

    public static class LinearRegression
    {


        private class DataPoint
        {
            public DateTime Time { get; set; }
            public double Value { get; set; }
        }

        const int MaxValues = 1000;
     

        public static void Run()
        {
            try
            {

                DateTime[] newTimeData = new DateTime[MaxValues];
                double[] newValueData = new double[MaxValues];


                List<DataPoint> dataPoints = ReadCsv(".\\GptPromptDataReal.txt");
                DataPoint[] dataArray = dataPoints.ToArray();

                for (int i = 0; i < dataPoints.Count; i++)
                {
                    newTimeData[i] = dataArray[i].Time;
                    newValueData[i] = dataArray[i].Value;
                }

                // Convert DateTime to double by calculating the number of days from a baseline
                DateTime baseline = new DateTime(2020, 1, 1);
                double[] xDataAsDouble = Array.ConvertAll(newTimeData, x => (x - baseline).TotalDays);

                // Y values
                //double[] xxData = new double[] { 2.0, 4.0, 6.0, 8.0, 10.0 };
                double[] yData = newValueData;

                // Convert arrays to Math.NET vectors
                //Vector<double> X = Vector<double>.Build.DenseOfArray(xDataAsDouble);
                //Vector<double> Y = Vector<double>.Build.DenseOfArray(yData);

                // Perform the linear regression
                ValueTuple<double, double> p = Fit.Line(xDataAsDouble, yData);

                // The result is a tuple (intercept, slope)
                double a = p.Item2; // slope
                double b = p.Item1; // intercept

                Console.WriteLine($"The best fit line is y = {a}x + {b}");

                Log2.Info("Slope = " + a.ToString());
                Console.WriteLine($"Slope = " + a.ToString());

                //double average = dataPoints.Average(dp => dp.Value);
                //double maximum = dataPoints.Max(dp => dp.Value);
                //double minimum = dataPoints.Min(dp => dp.Value);

                //Log2.Info($"Code Average: {average:F2}");
                //Log2.Info($"Code Maximum: {maximum:F2}");
                //Log2.Info($"Code Minimum: {minimum:F2}");

                //Console.WriteLine($"Code Average: {average:F2}");
                //Console.WriteLine($"Code Maximum: {maximum:F2}");
                //Console.WriteLine($"Code Minimum: {minimum:F2}");

                //List<DataPoint> runningAverages = CalculateRunningAverages(dataPoints, 3);
                //PrintRunningAverages(runningAverages);

                //int hours = AnalyzeSlopes(runningAverages);
                //AnalyzeNewestRunningAverage(runningAverages, hours);
            }
            catch (Exception ex)
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
    }


}

