// Copyright (C) Upperbay Systems, LLC - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and confidential
// Written by Dave Hardin <dave@upperbay.com>, 2001-2020

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using CsvHelper;


namespace ChatterBoxGPT
{

    public class FirstFileRecord_PJMNWS
    {
        public DateTime Time { get; set; }
        public string Load { get; set; }
        public string Price { get; set; }
        public string Temp { get; set; }
        public string Forecast { get; set; }
    }

        public class SecondFileRecord_PJMMID
    {
        public DateTime Time { get; set; }
        public string Load { get; set; }
    }

    public class MergedRecord_PJMNWSMID
    {
        public DateTime Time { get; set; }
        public string Load { get; set; }
        public string Price { get; set; }
        public string Temp { get; set; }
        public string Forecast { get; set; }
        public string LocalLoad { get; set; }
    }



    /// <summary>
    /// 
    /// </summary>
    public class CsvMergePJMNWSMID
    {
        public CsvMergePJMNWSMID()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath1"></param>
        /// <param name="filePath2"></param>
        /// <param name="outputFile"></param>
        /// <returns></returns>
        public int MergeFiles(string firstFilePath, string secondFilePath, string outputFilePath)
        {

            // Reading CSV files
            //var records1 = ReadCsv(filePath1);
            //var records2 = ReadCsv(filePath2);

            //// Merging the data
            //var mergedRecords = from r1 in records1
            //                    join r2 in records2 on r1.Time equals r2.Time into r2s
            //                    from r2 in r2s.DefaultIfEmpty()
            //                    select new DataRecordMapPJMNWS
            //                    {
            //                        Time = r1.Time,
            //                        Data1 = r1.Data1,
            //                        Data2 = r1.Data2,
            //                        Data3 = r2.Data3,
            //                        Data4 = r2?.Data4,
            //                    };

            //// Writing the merged data to a new CSV file
            //WriteCsv(mergedRecords, outputFile);

            List<FirstFileRecord_PJMNWS> firstRecords = ReadCsv<FirstFileRecord_PJMNWS>(firstFilePath);
            Console.WriteLine("first file row count = " + firstRecords.Count);
            // Read the second CSV file
            List<SecondFileRecord_PJMMID> secondRecords = ReadCsv<SecondFileRecord_PJMMID>(secondFilePath);
            Console.WriteLine("second file row count = " + secondRecords.Count);

            var mergedRecords = from first in firstRecords
                                join second in secondRecords
                                on first.Time equals second.Time
                                select new MergedRecord_PJMNWSMID
                                {
                                    Time = first.Time,
                                    Load = first.Load,
                                    Price = first.Price,
                                    Temp = first.Temp,
                                    Forecast = first.Forecast,
                                    LocalLoad = second.Load
                                };

            Console.WriteLine("merged file row count = " + mergedRecords.Count());
            int mergedRowCount = mergedRecords.Count();

            WriteCsv(mergedRecords, outputFilePath);

            Console.WriteLine("The CSV files have been merged successfully.");

            return mergedRowCount;
        }

        List<T> ReadCsv<T>(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csv.GetRecords<T>().ToList();
            }
        }

        void WriteCsv<T>(IEnumerable<T> records, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(records);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        List<DataRecord> ReadCsv(string filePath)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null
            };

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap<DataRecordMap>();
                return csv.GetRecords<DataRecord>().ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="records"></param>
        /// <param name="filePath"></param>
        public void WriteCsv(IEnumerable<DataRecord> records, string filePath)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null
            };

            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.Context.RegisterClassMap<DataRecordMap>();
                csv.WriteRecords(records);
            }
        }
    }
}

