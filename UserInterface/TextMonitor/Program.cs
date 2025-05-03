//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
using System;
using System.IO;
using TextMagicClient.Api;
using TextMagicClient.Client;
using TextMagicClient.Model;

using System.Threading;

using Upperbay.Core.Logging;
using Upperbay.Core.Library;
using System.Linq.Expressions;
using System.Runtime.Remoting.Lifetime;
using System.Collections.Generic;
using Newtonsoft.Json;


using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;



//using System.Collections.Generic;
using System.Collections;
namespace TextMonitor
{
    class Program
    {

    
        static void Main(string[] args)
        {
            int sleepMinutes = 0;

            MyAppConfig.SetMyAppConfig("ClusterAgent");
            string traceMode = MyAppConfig.GetParameter("TraceMode");
            Log2.LogInit("TextMonitor", "ClusterAgent", "debug");
            //Log2.Info("DebugLevel = " + traceMode);

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

            StringTank tank = new StringTank();
            string path = ".\\data\\TextingTank.txt";
            if (File.Exists(path))
            {
                tank.DeserializeFromFile(path); 
            }


            while (true)
            {

                // put your Username and API Key from https://my.textmagic.com/online/api/rest-api/keys page.

                string cluster = MyAppConfig.GetParameter("ClusterName");
                string smsAccountName = MyAppConfig.GetClusterParameter(cluster, "SMSAccountName");
                string smsAccountKey = MyAppConfig.GetClusterParameter(cluster, "SMSAccountKey");
                string smsTargetPhoneNumber = MyAppConfig.GetParameter("SMSTargetPhoneNumber");

                //var client = new Client("davidhardin2", "eWymWS3pHCqvewP8NqKdc2DnvCABDE");

                Configuration.Default.Username = smsAccountName;
                Configuration.Default.Password = smsAccountKey;

                var apiInstance = new TextMagicApi();

                // Optional, you can pass them as null values to getAllInboundMessages method below, default values will be used
                int page = 1;
                int limit = 10;
                string orderBy = "id";
                string direction = "desc";

                try
                {
                    // GetAllInboundMessagesPaginatedResponse class object
                    var result = apiInstance.GetAllInboundMessages(page, limit, orderBy, direction);
                    // ..

                    //{
                    //    "page": 1,
                    //      "pageCount": 10,
                    //      "limit": 10,
                    //      "resources": [
                    //        {
                    //                                "id": 1782832,
                    //          "sender": "447860021130",
                    //          "receiver": "447624800500",
                    //          "messageTime": "2012-11-28T18:38:28+0000",
                    //          "text": "I Love TextMagic!",
                    //          "contactId": 1,
                    //          "firstName": "Charles",
                    //          "lastName": "Conway",
                    //          "avatar": "avatars/dummy_avatar.png",
                    //          "email": "charles@example.com"
                    //        }
                    //      ]
                    //    }

                    //Console.WriteLine("Result = " + result);

                    //result.Resources.ForEach(x => Console.WriteLine(x));

                    if (result.Resources.Count > 0)
                    { 

                        foreach (var item in result.Resources)
                        {

                            Console.WriteLine(item.Id.ToString());
                            Console.WriteLine(item.Sender);
                            Console.WriteLine(item.FirstName);
                            Console.WriteLine(item.LastName);
                            Console.WriteLine(item.MessageTime);
                            Console.WriteLine(item.Text);
                            Console.WriteLine(item.Email);

                            if (tank.Contains(item.Id.ToString()) == false)
                            {

                                if (item.Text == "HELP")
                                {
                                    Console.WriteLine("HELP from " + item.LastName);

                                    var obj = new SendMessageInputObject();

                                    // Required parameters
                                    obj.Text = "Upperbay Systems LLC: For support, contact us at dave@upperbay.com or call 17745715386";
                                    obj.Phones = item.Sender;

                                    try
                                    {
                                        // SendMessageResponse class object
                                        var sendResult = apiInstance.SendMessage(obj);
                                        // ...
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("Exception when calling sendMessage: " + e.Message);
                                    }

                                }
                                else if (item.Text == "STOP")
                                {
                                    Console.WriteLine("STOP from " + item.LastName);

                                    var obj = new SendMessageInputObject();

                                    // Required parameters
                                    obj.Text = "Upperbay Systems LLC: You have successfully unsubscribed & will no longer receive any additional messages. Text HELP for assistance or JOIN to rejoin.";
                                    obj.Phones = item.Sender;

                                    try
                                    {
                                        // SendMessageResponse class object
                                        var sendResult = apiInstance.SendMessage(obj);
                                        // ...
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("Exception when calling sendMessage: " + e.Message);
                                    }

                                }
                                else if (item.Text == "JOIN")
                                {
                                    Console.WriteLine("JOIN" + item.LastName);

                                    var obj = new SendMessageInputObject();

                                    // Required parameters
                                    obj.Text = "Upperbay Systems LLC: Hello, welcome to Current For Carbon. Msg freq varies. Msg&data rates may apply. Text STOP to stop, HELP for help.";
                                    obj.Phones = item.Sender;

                                    // Optional parameters, you can skip these setters calls
                                    //obj.TemplateId = 1;
                                    //obj.SendingTime = 1565606455;
                                    //obj.SendingDateTime = "2020-05-27 13:02:33";
                                    //obj.SendingTimezone = "America/Buenos_Aires";
                                    //obj.Contacts = "1,2,3,4";
                                    //obj.Lists = "1,2,3,4";
                                    //obj.CutExtra = true;
                                    //obj.PartsCount = 6;
                                    //obj.ReferenceId = 1;
                                    //obj.From = "Test sender id";
                                    //obj.Rrule = "FREQ=YEARLY;BYMONTH=1;BYMONTHDAY=1;COUNT=1";
                                    //obj.CreateChat = false;
                                    //obj.Tts = false;
                                    //obj.Local = false;
                                    //obj.LocalCountry = "US";
                                    //obj.Destination = "mms";
                                    //obj.Resources = "tmauKcSmwflB77kLQ15904023426649.jpg";

                                    try
                                    {
                                        // SendMessageResponse class object
                                        var sendResult = apiInstance.SendMessage(obj);
                                        // ...
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("Exception when calling sendMessage: " + e.Message);
                                    }
                                }

                                tank.Add(item.Id.ToString(), item.Sender);
                                tank.SerializeToFile(path);
                            }
                        }

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception when calling getAllInboundMessages: " + e.Message);
                }



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
                    //Log2.Info("Sleepytime Minutes = " + sleepMinutes);
                    DateTime dt1 = DateTime.Now;
                    dt1 = dt1.AddMinutes(sleepMinutes);
                    Console.WriteLine("Sleeping for + " + sleepMinutes);
                    Console.WriteLine("Will Wake Up at " + dt1.ToShortTimeString());
                    Thread.Sleep(sleepMinutes * 60 * 1000);
                    //Console.WriteLine("Awake");
                }
            }
        }
    }
 
 
    /// <summary>
    /// 
    /// </summary>
    public class StringTank
    {
        private Dictionary<string, string> _data = new Dictionary<string, string>();

        public void Add(string key, string value)
        {
            _data[key] = value; // This will add or update the value for the given key.
        }

        public bool Contains(string key)
        {
            return _data.ContainsKey(key);
        }

        public void SerializeToFile(string filePath)
        {
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                formatter.Serialize(stream, _data);
            }
        }

        public void DeserializeFromFile(string filePath)
        {
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                _data = (Dictionary<string, string>)formatter.Deserialize(stream);
            }
        }

        public override string ToString()
        {
            return string.Join(", ", _data);
        }
    }


}
