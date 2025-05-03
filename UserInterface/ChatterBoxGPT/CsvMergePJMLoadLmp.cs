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
    public class DataRecord
    {
        public string Time { get; set; }
        public string Data1 { get; set; }
        public string Data2 { get; set; }
    }

    public sealed class DataRecordMap : ClassMap<DataRecord>
    {
        public DataRecordMap()
        {
            Map(m => m.Time).Name("Time");
            Map(m => m.Data1).Name("Load");
            Map(m => m.Data2).Name("Price");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CsvMergePJMLoadLmp
    {
        public CsvMergePJMLoadLmp()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath1"></param>
        /// <param name="filePath2"></param>
        /// <param name="outputFile"></param>
        /// <returns></returns>
        public int MergeFiles(string filePath1, string filePath2, string outputFile)
        {

            // Reading CSV files
            var records1 = ReadCsv(filePath1);
            var records2 = ReadCsv(filePath2);

            // Merging the data
            var mergedRecords = from r1 in records1
                                join r2 in records2 on r1.Time equals r2.Time into r2s
                                from r2 in r2s.DefaultIfEmpty()
                                select new DataRecord
                                {
                                    Time = r1.Time,
                                    Data1 = r1.Data1,
                                    Data2 = r2?.Data2
                                };

            // Writing the merged data to a new CSV file
            WriteCsv(mergedRecords, outputFile);
            int mergedRowCount = mergedRecords.Count();

            Console.WriteLine("The CSV files have been merged successfully.");

            return mergedRowCount;
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

