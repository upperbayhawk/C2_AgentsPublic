//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Threading;

using Upperbay.Core.Logging;
using Upperbay.Core.Library;
using Upperbay.Worker.LMP;
using Upperbay.Worker.NWS;
using System.Net.NetworkInformation;
using Newtonsoft.Json.Linq;
using System.Security.Claims;


namespace ChatterBoxGPT
{

   class Program
    { 
       static async Task Main(string[] args)
       {
            MyAppConfig.SetMyAppConfig("ClusterAgent");
            //string traceMode = MyAppConfig.GetParameter("TraceMode");
            string traceMode = "trace";
            Log2.LogInit("ChatterBoxGPT", "ClusterAgent", traceMode);
            Log2.Info("DebugLevel = " + traceMode);

            // --------------------------------------
            int sleepMinutes = 0;
            string timeString = null;

            // Check if there are any command-line arguments
            if (args.Length == 0)
            {
     
            }

            // Loop through each command-line argument
            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];

                // You can perform different actions based on the argument value
                if (arg.StartsWith("--"))
                {
                    // Handle long options (e.g., --help, --version)
                    if (arg == "--help")
                    {
                        Console.WriteLine("Displaying help information: TBD");
                        // Add your help logic here
                        return;
                    }
                    else if (arg == "--version")
                    {
                        Console.WriteLine("Version 0.6.9");
                        return;
                        // Add your version logic here
                    }
                    // Add more options as needed
                    else
                    {
                        Console.WriteLine($"Unknown option: {arg}");
                    }
                }
                else if (arg.StartsWith("-"))
                {
                    // Handle short options (e.g., -f, -v)
                    // Add your short option logic here
                    if (arg == "-m")
                    {
                        sleepMinutes = int.Parse(args[i + 1]);
                        Console.WriteLine("SleepMinutes = " + sleepMinutes);
                    }
                    else if (arg == "-h")
                    {
                        int sleepHours = int.Parse(args[i + 1]);
                        sleepMinutes = sleepHours * 60;
                        Console.WriteLine("SleepMinutes = " + sleepMinutes);
                    }
                    else if (arg == "-t")
                    {
                        timeString = args[i + 1];
                        string[] timeComponents = timeString.Split(':');
                        if (timeComponents.Length != 2 || !int.TryParse(timeComponents[0], out int hour) || !int.TryParse(timeComponents[1], out int minute))
                        {
                            Console.WriteLine("Invalid time string format: " + timeString);
                            return;
                        }
                        Console.WriteLine("WakeTime = " + timeString);
                    }
                    // Add more options as needed
                    else
                    {
                        Console.WriteLine($"Unknown option: {arg}");
                    }
                }
                else
                {
                    // Handle positional arguments (non-option arguments)
                    Console.WriteLine($"Positional argument {i + 1}: {arg}");
                    // Add your positional argument logic here
                }
            }

            //---------------------------------------
           

            MQTTPipe.MqttInitializeAsync("192.168.0.111",
                                             "",
                                             "",
                                             1883);
            MQTTPipe.MqttSubscribeAsync();

            bool exit = false;
            string userInput = null;
                       
            // Console.Clear();
            Console.WriteLine("Select PJM dataset to be fed to GPT");
            Console.WriteLine("1. PJMLoad_Lmp_Temp");
            Console.WriteLine("2. PJMLoadForecastSevenDay");
            Console.WriteLine("3. PJMDayAheadHourlyLmp");
            Console.WriteLine("4. PJMFiveMinLoadForecast");
            Console.WriteLine("5. PJMUnverifiedFiveMinLmp");
            Console.WriteLine("6. PJMHourLoadMetered");
            Console.WriteLine("7. PJMHourLoadPrelim");
            Console.WriteLine("8. PJMInstLoad");
            Console.WriteLine("9. PJMOperationsSummary");
            Console.WriteLine("10. Exit");

            Console.Write("Enter your choice (1-10): ");

            userInput = Console.ReadLine();

            switch (userInput)
            {
                case "1":
                    // Code for Option 3
                    Console.WriteLine("PJMLoad_Lmp_Temp selected.");
                    break;
                case "2":
                    // Code for Option 3
                    Console.WriteLine("PJMLoadForecastSevenDay selected.");
                    break;
                case "3":
                    // Code for Option 1
                    Console.WriteLine("PJMDayAheadHourlyLMP selected.");
                    break;
                case "4":
                    // Code for Option 1
                    Console.WriteLine("PJMFiveMinLoadForecast selected.");
                    break;
                case "5":
                    // Code for Option 3
                    Console.WriteLine("PJMUnverifiedFiveMinLmp selected.");
                    break;
                case "6":
                    // Code for Option 3
                    Console.WriteLine("PJMHourLoadMetered selected.");
                    break;
                case "7":
                    // Code for Option 3
                    Console.WriteLine("PJMHourLoadPrelim selected.");
                    break;
                case "8":
                    // Code for Option 3
                    Console.WriteLine("PJMInstLoad selected.");
                    break;
                case "9":
                    // Code for Option 3
                    Console.WriteLine("PJMOperationsSummary selected.");
                    break;
               
                case "10":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice, bye, bye.");
                    exit = true;
                    break;
            }
           
            if (exit) return;

            while (true)
            {
                DateTime anchorDateTime = DateTime.Now;

                Console.WriteLine("Getting Grid Data");

                if (userInput == "1")
                {

                    //PJMLoadAndLmp

                    // DOM = 34964545
                    // PJM = 1
                    // pricing node, 93154, DPL_ODEC
                    // GridPayloadCSV Header Format
                    // Time, PJMLoad, PJMLmp, LocalLoad, LocalLmp, LocalTemp, LocalForecast, 

                    //PJMLoadForecastSevenDay pJMLoadForecastSevenDay = new PJMLoadForecastSevenDay();
                    //string jsonLoadDOM = pJMLoadForecastSevenDay.GetJson("DOMINION", 200);
                    //if (jsonLoadDOM != null)
                    //{
                    //    pJMLoadForecastSevenDay.WriteJsonToFile(jsonLoadDOM, ".\\data\\PJMLoadForeCastSevenDayDOM.json");
                    //    pJMLoadForecastSevenDay.WriteJsonToCsv(jsonLoadDOM, ".\\data\\PJMLoadForecastSevenDayDOM.csv");
                    //}

                    //string jsonLoadDPL_ODEC = pJMLoadForecastSevenDay.GetJson("DPL_ODEC", 200);
                    //if (jsonLoadDPL_ODEC != null)
                    //{
                    //    pJMLoadForecastSevenDay.WriteJsonToFile(jsonLoadDPL_ODEC, ".\\data\\PJMLoadForeCastSevenDayDPL_ODEC.json");
                    //    pJMLoadForecastSevenDay.WriteJsonToCsv(jsonLoadDPL_ODEC, ".\\data\\PJMLoadForecastSevenDayDPL_ODEC.csv");
                    //}

                    //ODEC
                    //PJMLoadForecastSevenDay pJMLoadForecastSevenDay = new PJMLoadForecastSevenDay();
                    //PJMDayAheadHourlyLMP pJMDayAheadHourlyLMP = new PJMDayAheadHourlyLMP();
                    //string jsonLMPODEC = pJMDayAheadHourlyLMP.GetJson("93154", 24);
                    //if (jsonLMPODEC != null)
                    //{
                    //    pJMDayAheadHourlyLMP.WriteJsonToFile(jsonLMPODEC, ".\\data\\PJMDayAheadHourlyLMPODEC.json");
                    //    pJMDayAheadHourlyLMP.WriteCurrentDayAheadHourlyLMPToCsv(jsonLMPODEC, ".\\data\\PJMDayAheadHourlyLMPODEC.csv");
                    //}


                    bool badData = false;

                    PJMDayAheadHourlyLMP pJMDayAheadHourlyLMP = new PJMDayAheadHourlyLMP();
                    PJMLoadForecastSevenDay pJMLoadForecastSevenDay = new PJMLoadForecastSevenDay();

                    string jsonLoad = pJMLoadForecastSevenDay.GetJson("RTO_COMBINED", 200);
                    if (jsonLoad != null)
                    {
                        pJMLoadForecastSevenDay.WriteJsonToFile(jsonLoad, ".\\data\\PJMLoadForeCastSevenDay.json");
                        double dfirstVal = pJMLoadForecastSevenDay.GetFirstValue(jsonLoad);
                        Console.WriteLine("First value: " + dfirstVal.ToString());
                        double dlastVal = pJMLoadForecastSevenDay.GetLastValue(jsonLoad);
                        Console.WriteLine("Last value: " + dlastVal.ToString());
                        pJMLoadForecastSevenDay.WriteJsonToCsv(jsonLoad, ".\\data\\PJMLoadForecastSevenDay.csv");
                        pJMLoadForecastSevenDay.WriteCurrentDayLoadJsonToCsv(jsonLoad, ".\\data\\PJMLoadForecastToday.csv");

                        //PJMDayAheadHourlyLMP

                        //PJMDayAheadHourlyLMP pJMDayAheadHourlyLMP = new PJMDayAheadHourlyLMP();
                        string jsonLMP = pJMDayAheadHourlyLMP.GetJson("1", 24);
                        if (jsonLMP != null)
                        {
                            pJMDayAheadHourlyLMP.WriteJsonToFile(jsonLMP, ".\\data\\PJMDayAheadHourlyLMP.json");
                            double dFirstVal = pJMDayAheadHourlyLMP.GetFirstValue(jsonLMP);
                            Console.WriteLine("First value: " + dFirstVal.ToString());
                            pJMDayAheadHourlyLMP.WriteCurrentDayAheadHourlyLMPToCsv(jsonLMP, ".\\data\\PJMDayAheadHourlyLMP.csv");

                            CsvMergePJMLoadLmp csvMergePJMLoadLmp = new CsvMergePJMLoadLmp();
                            int csvMergePJMLoadLmpRowCount = csvMergePJMLoadLmp.MergeFiles(".\\data\\PJMLoadForecastToday.csv",
                                                ".\\data\\PJMDayAheadHourlyLMP.csv",
                                                ".\\data\\PJMLoadAndLMP.csv");
                            if (csvMergePJMLoadLmpRowCount == 0)
                            {
                                //bad data
                                Log2.Error("Bad Data: csvMergePJMLoadLmpRowCount == 0");
                                Console.WriteLine("Bad Data: csvMergePJMLoadLmpRowCount == 0");
                                badData = true;
                            }

                            //TimeSeriesDataAnalyzer.Run(".\\data\\PJMDayAheadHourlyLMP.csv");
                            //LinearRegression.Run(".\\data\\PJMDayAheadHourlyLMP.csv");

                            //Coords for Lewes, DE
                            string LAT = "39.7810";
                            string LON = "-75.1571";
                            NWSWeatherForecast weatherForecast = new NWSWeatherForecast();
                            string weather = weatherForecast.GetWeatherForecastInJson(LAT, LON);
                            Console.WriteLine("Weather: " + weather);
                            weatherForecast.WriteTodaysWeatherForecastToCsv(weather, ".\\data\\NWSWeatherToday.csv");
                            weatherForecast.WriteWeatherForecastToCsv(weather, ".\\data\\NWSWeatherSevenDay.csv");

                            CsvMergePJMwNWS csvMergePJMwNWS = new CsvMergePJMwNWS();
                            int csvMergePJMwNWSRowCount =  csvMergePJMwNWS.MergeFiles(".\\data\\PJMLoadAndLmp.csv",
                                                ".\\data\\NWSWeatherToday.csv",
                                                ".\\data\\GridWeatherPayloadPJMNWS.csv");
                            if (csvMergePJMwNWSRowCount <= 1)
                            {
                                //bad data
                                Log2.Error("Bad Data: csvMergePJMwNWSRowCount == 0");
                                Console.WriteLine("Bad Data: csvMergePJMwNWSRowCount == 0");
                                badData = true;
                            }


                            string jsonLoadDPL_MIDATL = pJMLoadForecastSevenDay.GetJson("DP&L/MIDATL", 200);
                            if (jsonLoadDPL_MIDATL != null)
                            {
                                pJMLoadForecastSevenDay.WriteJsonToFile(jsonLoadDPL_MIDATL, ".\\data\\PJMLoadForeCastSevenDayDPL_MIDATL.json");
                                pJMLoadForecastSevenDay.WriteJsonToCsv(jsonLoadDPL_MIDATL, ".\\data\\PJMLoadForecastSevenDayDPL_MIDATL.csv");
                                pJMLoadForecastSevenDay.WriteCurrentDayLoadJsonToCsv(jsonLoadDPL_MIDATL, ".\\data\\PJMLoadForecastTodayDPL_MIDATL.csv");

                                // merge with loadandlmp
                                CsvMergePJMNWSMID csvMergePJMNWSMID = new CsvMergePJMNWSMID();
                                int csvMergePJMNWSMIDRowCount = csvMergePJMNWSMID.MergeFiles(".\\data\\GridWeatherPayloadPJMNWS.csv",
                                                    ".\\data\\PJMLoadForecastTodayDPL_MIDATL.csv",
                                                    ".\\data\\GridWeatherPayloadPJMNWSMID.csv");
                                if (csvMergePJMNWSMIDRowCount <= 1)
                                {
                                    //bad data
                                    Log2.Error("Bad Data: csvMergePJMNWSMIDRowCount == 0");
                                    Console.WriteLine("Bad Data: csvMergePJMNWSMIDRowCount == 0");
                                    badData = true;
                                }
                            }

                            //PJMLoadForecastSevenDay pJMLoadForecastSevenDay = new PJMLoadForecastSevenDay();
                            //PJMDayAheadHourlyLMP pJMDayAheadHourlyLMP = new PJMDayAheadHourlyLMP();

                            string jsonLMPODEC = pJMDayAheadHourlyLMP.GetJson("93154", 24);
                            if (jsonLMPODEC != null)
                            {
                                pJMDayAheadHourlyLMP.WriteJsonToFile(jsonLMPODEC, ".\\data\\PJMDayAheadHourlyLMPODEC.json");
                                pJMDayAheadHourlyLMP.WriteCurrentDayAheadHourlyLMPToCsv(jsonLMPODEC, ".\\data\\PJMDayAheadHourlyLMPODEC.csv");
                                CsvMergePJMNWSMIDLOC csvMergePJMNWSMIDLOC = new CsvMergePJMNWSMIDLOC();
                                int csvMergePJMNWSMIDLOCRowCount = csvMergePJMNWSMIDLOC.MergeFiles(".\\data\\GridWeatherPayloadPJMNWSMID.csv",
                                                    ".\\data\\PJMDayAheadHourlyLMPODEC.csv",
                                                    ".\\data\\GridWeatherPayload.csv");
                                if (csvMergePJMNWSMIDLOCRowCount <= 1)
                                {
                                    //bad data
                                    Log2.Error("Bad Data: csvMergePJMNWSMIDLOCRowCount == 0");
                                    Console.WriteLine("Bad Data: csvMergePJMNWSMIDLOCRowCount == 0");
                                    badData = true;
                                }
                            }

                            //string jsonLMPMILFORD = pJMDayAheadHourlyLMP.GetJson("49966", 24);
                            //if (jsonLMPMILFORD != null)
                            //{
                            //    pJMDayAheadHourlyLMP.WriteJsonToFile(jsonLMPMILFORD, ".\\data\\PJMDayAheadHourlyLMPMILFORD.json");
                            //    pJMDayAheadHourlyLMP.WriteCurrentDayAheadHourlyLMPToCsv(jsonLMPMILFORD, ".\\data\\PJMDayAheadHourlyLMPMILFORD.csv");
                            //    // merge with loadandlmp
                            //    CsvMergePJMNWSMIDLOC csvMergePJMNWSMIDLOC = new CsvMergePJMNWSMIDLOC();
                            //    csvMergePJMNWSMIDLOC.MergeFiles(".\\data\\GridWeatherPayloadPJMNWSMID.csv",
                            //                        ".\\data\\PJMDayAheadHourlyLMPMILFORD.csv",
                            //                        ".\\data\\GridWeatherPayload.csv");
                            //}
                            




                            //Coords for Lewes, DE
                            string location = "Lewes, Delaware";
                            VisualCrossingWeatherData weatherData = new VisualCrossingWeatherData();
                            string weatherJson = weatherData.GetWeatherSevenDayDataInJson(location);
                            Console.WriteLine("Weather Data: " + weather);
                            weatherData.WriteJsonToFile(weatherJson, ".\\data\\VisualCrossingWeatherData.json");
                            weatherData.WriteWeatherDataToCsv(weatherJson, ".\\data\\VisualCrossingWeatherData.csv");

                            PJMOperationsSummary pJMOperationsSummary = new PJMOperationsSummary();
                            //string json = pJMOperationsSummary.GetJson("RTO_COMBINED", 1);
                            
                            string json = pJMOperationsSummary.GetJson("MIDATL", 1); // overridden for RTO
                            pJMOperationsSummary.WriteJsonToFile(json, ".\\data\\PJMOperationsSummary.json");

                            if (badData == false)
                            {
                                string promptText = File.ReadAllText(".\\prompts\\PromptPJMLoadAndLMP.Txt");
                                string promptData = File.ReadAllText(".\\data\\GridWeatherPayload.csv");
                                //string promptWeatherData = File.ReadAllText(".\\data\\VisualCrossingWeatherData.csv");
                                string promptOpsSummary = File.ReadAllText(".\\data\\PJMOperationsSummary.json");
                                string prompt = promptText + " " + promptData;
                                //string prompt = promptText + " " + promptData + "\nOperations Summary in JSON follows: \n" + promptOpsSummary;
                                Log2.Info(prompt);
                                File.WriteAllText(".\\data\\FullPromptToAgent.txt", prompt);


                                Console.WriteLine("Letting GPT analyze the Grid Data");

                                DateTime startGPTDateTime = DateTime.Now;
                                string customFormat = startGPTDateTime.ToString("yyyy-MM-dd HH:mm:ss");

                                Log2.Info("To Brain: " + customFormat + ": " + prompt);
                                Console.WriteLine("To Brain: " + customFormat + ": " + prompt);
                                Console.WriteLine("WAITING...");
                                MQTTPipe.PublishMessage(prompt);
                                string response = MQTTPipe.ReadMessage();
                                Log2.Info("From Brain: " + customFormat + ": " + response);
                                Console.WriteLine("From Brain: " + customFormat + ": " + response);
                            }
                            else
                            {
                                Log2.Error("FAILED due to Bad Data");
                                Console.WriteLine("FAILED due to Bad Data");

                            }
                        }
                    }
                }

                if (userInput == "2")
                {
                    //PJMLoadForecastSevenDay

                    PJMLoadForecastSevenDay pJMLoadForecastSevenDay = new PJMLoadForecastSevenDay();
                    string json = pJMLoadForecastSevenDay.GetJson("RTO_COMBINED", 200);
                    if (json != null)
                    {
                        pJMLoadForecastSevenDay.WriteJsonToFile(json, ".\\data\\PJMLoadForeCastSevenDay.json");
                        double dfirstVal = pJMLoadForecastSevenDay.GetFirstValue(json);
                        Console.WriteLine("First value: " + dfirstVal.ToString());
                        double dlastVal = pJMLoadForecastSevenDay.GetLastValue(json);
                        Console.WriteLine("Last value: " + dlastVal.ToString());
                        //pJMLoadForecastSevenDay.WriteJsonToCsv(json, ".\\data\\GptPromptDataCSV.txt");
                        pJMLoadForecastSevenDay.WriteCurrentDayLoadJsonToCsv(json, ".\\data\\PJMLoadForecastSevenDay.csv");

                        string promptText = File.ReadAllText(".\\prompts\\PromptPJMLoadForecastSevenDay.Txt");
                        string promptData = File.ReadAllText(".\\data\\PJMLoadForecastSevenDay.csv");
                        string prompt = promptText + " " + promptData;
                        Log2.Info(prompt);

                        TimeSeriesDataAnalyzer.Run(".\\data\\PJMLoadForecastSevenDay.csv");
                        LinearRegression.Run(".\\data\\PJMLoadForecastSevenDay.csv");

                        Console.WriteLine("Letting GPT analyze the Grid Data");

                        DateTime startGPTDateTime = DateTime.Now;
                        string customFormat = startGPTDateTime.ToString("yyyy-MM-dd HH:mm:ss");

                        Log2.Info("To Brain: " + customFormat + ": " + prompt);
                        Console.WriteLine("To Brain: " + customFormat + ": " + prompt);
                        Console.WriteLine("WAITING...");
                        MQTTPipe.PublishMessage(prompt);
                        string response = MQTTPipe.ReadMessage();
                        Log2.Info("From Brain: " + customFormat + ": " + response);
                        Console.WriteLine("From Brain: " + customFormat + ": " + response);
                    }
                }

                if (userInput == "3")
                {
                    //PJMDayAheadHourlyLMP

                    PJMDayAheadHourlyLMP pJMDayAheadHourlyLMP = new PJMDayAheadHourlyLMP();
                    string json = pJMDayAheadHourlyLMP.GetJson("1", 24);
                    if (json != null)
                    {
                        pJMDayAheadHourlyLMP.WriteJsonToFile(json, ".\\data\\PJMDayAheadHourlyLMP.json");
                        double dFirstVal = pJMDayAheadHourlyLMP.GetFirstValue(json);
                        Console.WriteLine("First value: " + dFirstVal.ToString());
                        pJMDayAheadHourlyLMP.WriteCurrentDayAheadHourlyLMPToCsv(json, ".\\data\\PJMDayAheadHourlyLMP.csv");

                        string promptText = File.ReadAllText(".\\prompts\\PromptPJMDayAheadHourlyLMP.Txt");
                        string promptData = File.ReadAllText(".\\data\\PJMDayAheadHourlyLMP.csv");
                        string prompt = promptText + " " + promptData;
                        Log2.Info(prompt);

                        TimeSeriesDataAnalyzer.Run(".\\data\\PJMDayAheadHourlyLMP.csv");
                        LinearRegression.Run(".\\data\\PJMDayAheadHourlyLMP.csv");

                        Console.WriteLine("Letting GPT analyze the Grid Data");

                        DateTime startGPTDateTime = DateTime.Now;
                        string customFormat = startGPTDateTime.ToString("yyyy-MM-dd HH:mm:ss");

                        Log2.Info("To Brain: " + customFormat + ": " + prompt);
                        Console.WriteLine("To Brain: " + customFormat + ": " + prompt);
                        Console.WriteLine("WAITING...");
                        MQTTPipe.PublishMessage(prompt);
                        string response = MQTTPipe.ReadMessage();
                        Log2.Info("From Brain: " + customFormat + ": " + response);
                        Console.WriteLine("From Brain: " + customFormat + ": " + response);
                    }
                }


                if (userInput == "4")
                {
                    PJMFiveMinLoadForecast pJMFiveMinLoadForecast = new PJMFiveMinLoadForecast();
                    string json = pJMFiveMinLoadForecast.GetJson("MID_ATLANTIC_REGION", 100);
                    pJMFiveMinLoadForecast.WriteJsonToFile(json, ".\\data\\PJMFiveMinLoadForecast.json");
                    double dlastVal = pJMFiveMinLoadForecast.GetLastValue(json);
                    Console.WriteLine("Last value: " + dlastVal.ToString());
                    //pJMFiveMinLoadForecast.WriteCsvToFile(json, ".\\data\\PJMFiveMinLoadForcast.csv");
                }

                if (userInput == "5")
                {

                    PJMUnverifiedFiveMinLmp pJMUnverifiedFiveMinLmp = new PJMUnverifiedFiveMinLmp();
                    string json = pJMUnverifiedFiveMinLmp.GetJson("49955", 100);
                    pJMUnverifiedFiveMinLmp.WriteJsonToFile(json, ".\\data\\PJMUnverifiedFiveMinLmp.json");
                    double dlastVal = pJMUnverifiedFiveMinLmp.GetLastValue(json);
                    Console.WriteLine("Last value: " + dlastVal.ToString());
                    //pJMUnverifiedFiveMinLmp.WriteCsvToFile(json, ".\\data\\PJMUnverifiesFiveMinLmp.csv");
                }

                if (userInput == "6")
                {

                    PJMHourLoadMetered pJMHourLoadMetered = new PJMHourLoadMetered();
                    string json = pJMHourLoadMetered.GetJson("DPLCO", 100);
                    pJMHourLoadMetered.WriteJsonToFile(json, ".\\data\\PJMHourLoadMetered.json");
                    double dlastVal = pJMHourLoadMetered.GetLastValue(json);
                    Console.WriteLine("Last value: " + dlastVal.ToString());
                    //pJMHourLoadMetered.WriteCsvToFile(json, ".\\data\\PJMHourLoadMetered.csv");
                }

                if (userInput == "7")
                {

                    PJMHourLoadPrelim pJMHourLoadPrelim = new PJMHourLoadPrelim();
                    string json = pJMHourLoadPrelim.GetJson("DPLCO", 100);
                    pJMHourLoadPrelim.WriteJsonToFile(json, ".\\data\\PJMHourLoadPrelim.json");
                    double dlastVal = pJMHourLoadPrelim.GetLastValue(json);
                    Console.WriteLine("Last value: " + dlastVal.ToString());
                    //pJMHourLoadPrelim.WriteCsvToFile(json, ".\\data\\PJMHourLoadPrelim.csv");
                }

                if (userInput == "8")
                {

                    PJMInstLoad pJMInstLoad = new PJMInstLoad();
                    string json = pJMInstLoad.GetJson("DPL", 100);
                    pJMInstLoad.WriteJsonToFile(json, ".\\data\\PJMInstLoad.json");
                    double dlastVal = pJMInstLoad.GetLastValue(json);
                    Console.WriteLine("Last value: " + dlastVal.ToString());
                    //pJMInstLoad.WriteCsvToFile(json, ".\\data\\PJMInstLoad.csv");
                }

                if (userInput == "9")
                {

                    PJMOperationsSummary pJMOperationsSummary = new PJMOperationsSummary();
                    string json = pJMOperationsSummary.GetJson("MIDATL", 1);
                    pJMOperationsSummary.WriteJsonToFile(json, ".\\data\\PJMOperationsSummary.json");
                    double dlastVal = pJMOperationsSummary.GetLastValue(json);
                    Console.WriteLine("Last value: " + dlastVal.ToString());
                    //pJMOperationsSummary.WriteCsvToFile(json, ".\\data\\PJMOperationsSummary.csv");
                }

                

                ///////////////////////////////////////
                if ((sleepMinutes == 0) && (timeString == null))
                {
                    break;
                }
                else if (timeString != null)
                {
                    DateTime endTime = DateTime.Now;
                    TimeSpan executionTime = endTime - anchorDateTime;
                    Log2.Info("Execution Time = " + executionTime.ToString());
                    Console.WriteLine("Execution Time = " + executionTime.ToString());

                    DateTime tomorrow = DateTime.Today.AddDays(1);
                    string[] timeComponents = timeString.Split(':');
                    if (timeComponents.Length != 2 || !int.TryParse(timeComponents[0], out int hour) || !int.TryParse(timeComponents[1], out int minute))
                    {
                        Console.WriteLine("Invalid time string format.");
                        return;
                    }
                    DateTime tomorrowAtTime = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, hour, minute, 0);
                    TimeSpan timeSpan = tomorrowAtTime - DateTime.Now;

                    int timeToSleep = (int)Math.Ceiling(timeSpan.TotalMilliseconds);
                    DateTime alarm = DateTime.Now.AddMilliseconds(timeSpan.TotalMilliseconds);
                    Console.WriteLine("Sleeping, will wake up at " + alarm.ToLongDateString() + " " + alarm.ToLongTimeString());
                    Thread.Sleep(timeToSleep);
                    Console.WriteLine("Awake at " + alarm.ToLongDateString() + " " + alarm.ToLongTimeString());
                }
                else
                {
                    DateTime endTime = DateTime.Now;
                    TimeSpan executionTime = endTime - anchorDateTime;
                    Log2.Info("Execution Time = " + executionTime.ToString());
                    Console.WriteLine("Execution Time = " + executionTime.ToString());

                    Log2.Info("Sleepytime Minutes = " + sleepMinutes);
                    Console.WriteLine("Cycling every " + sleepMinutes + " Mins");

                    int periodMilliseconds = sleepMinutes * 60 * 1000;
                    double millisecondsTillAlarm = periodMilliseconds - executionTime.TotalMilliseconds;

                    int timeToSleep = (int)Math.Ceiling(millisecondsTillAlarm);
                    DateTime alarm = DateTime.Now.AddMilliseconds(timeToSleep);

                    Console.WriteLine("Sleeping for " + timeToSleep + " Mils. Will Wake Up at " + alarm.ToLongDateString() + " " + alarm.ToLongTimeString());
                    Thread.Sleep(timeToSleep);
                    Console.WriteLine("Awake at "+ alarm.ToLongDateString() + " " + alarm.ToLongTimeString());
                } //end else
               
            }// end while
           
       }
   }
}
