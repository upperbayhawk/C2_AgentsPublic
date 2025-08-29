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


namespace ChatterBox
{

   class Program
    { 
       static async Task Main(string[] args)
       {
            int sleepMinutes = 0;

            MyAppConfig.SetMyAppConfig("ClusterAgent");
            string traceMode = MyAppConfig.GetParameter("TraceMode");
            Log2.LogInit("ChatterBox", "ClusterAgent", "info");
            Log2.Info("DebugLevel = " + traceMode);


            if (args == null)
            {
                
            }
            else
            {
                // Step 2: print length, and loop over all arguments.
                Log2.Info("args length is " + args.Length);
                
                for (int i = 0; i < args.Length; i++)
                {
                    string argument = args[i];
                    Log2.Info("args index " + i);
                    Log2.Info(" is [" + argument + "]");
                    sleepMinutes = int.Parse(argument);
                }
            }


            while (true)
            {

                //Console.WriteLine("Enter a string of sentences:");
                //string input = Console.ReadLine();

                //string input = "Why is the sky blue?";
                Console.WriteLine("Getting Grid Data"); 
                PJMRealTimeLMP pjmRealTimeLMP = new PJMRealTimeLMP();
                pjmRealTimeLMP.GetPJMRealTimeLMPHistory(".\\GptPromptDataReal.txt");


                string promptText = File.ReadAllText(".\\GptPromptText.Txt");

                string promptData = File.ReadAllText(".\\GptPromptDataReal.Txt");
                string prompt = promptText + " " + promptData;
                Log2.Info(prompt);

                TimeSeriesDataAnalyzer.Run();
                LinearRegression.Run();
                /////////////////////////////////////
                //if (false)
                //{
                //    Thread.Sleep(5000);
                //    Console.WriteLine("Letting GPT analyze the Grid Data");

                //    var chatGptService = new ChatGptService();
                //    string response = await chatGptService.SendMessageToChatGptAsync(prompt);
                //    Log2.Info(response);
                //    Console.WriteLine($"Response from ChatGPT: {response}");
                //}
                ///////////////////////////////////////
                if (sleepMinutes == 0)
                {
                    break;
                }
                else if ((sleepMinutes < 0) || (sleepMinutes > 240))
                {
                    Log2.Error("SleepMinutes = " + sleepMinutes);
                    break;
                }
                else
                {
                    Log2.Info("Sleepytime Minutes = " + sleepMinutes);
                    DateTime dt1 = DateTime.Now;
                    dt1 = dt1.AddMinutes(sleepMinutes);
                    Console.WriteLine("Sleeping for + " + sleepMinutes);
                    Console.WriteLine("Will Wake Up at " + dt1.ToShortTimeString());
                    Thread.Sleep(sleepMinutes * 60 * 1000);
                    Console.WriteLine("Awake");
                }
               
            }
           
       }
   }
}
