//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================

// Services/ChatGptService.cs
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Upperbay.Core.Logging;

namespace ChatterBox
{
    public class ChatGptService
    {

        private readonly HttpClient _httpClient;
        private string _url = "https://api.openai.com/v1/chat/completions";
        private string _apiKey = "Bearer " + "sk-y2s4DiRE6R1Sxxxxxxxxil90X";

        public ChatGptService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> SendMessageToChatGptAsync(string userMessage)
        {
            try
            {
                var gptMessages = new List<dynamic>
                {
                new {role = "system",
                    content = "You are ChatGPT, a large language " +
                                    "model trained by OpenAI. " +
                                    "Answer as concisely as possible. " } };
            
                //new
                //{
                //    role = "assistant",
                //    content = "How can I help you?" }
                //};

                gptMessages.Add(new { role = "user", content = userMessage });

                var chatGptRequest = new ChatGptRequest
                {
                    messages = gptMessages,
                    //model = "gpt-4"
                    model = "gpt-3.5-turbo"
                   // max_tokens = 300,
                };

                _httpClient.DefaultRequestHeaders.Add("Authorization", _apiKey);
                var requestJson = JsonConvert.SerializeObject(chatGptRequest);
                Console.WriteLine("JSON REQUEST: " + requestJson.ToString());

                var requestContent = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json");
                
                var httpResponseMessage = await _httpClient.PostAsync(_url, requestContent);
                httpResponseMessage.EnsureSuccessStatusCode();

                var jsonString = await httpResponseMessage.Content.ReadAsStringAsync();

                var responseObject = JsonConvert.DeserializeAnonymousType(jsonString, new
                {
                    choices = new[] { new { message = new { role = string.Empty, content = string.Empty } } },
                    error = new { message = string.Empty }
                });

                var messageObject = responseObject?.choices[0].message;
                //messages.Add(messageObject);

                Console.WriteLine("JSON RESPONSE: " + jsonString.ToString());
                                
                string val = messageObject.content.Trim();
                Log2.Info(val);

                string returnValue;
                //extract first floating point number from the return string
                var match = Regex.Match(val, @"([-+]?[0-9]*\.?[0-9]+)");
                if (match.Success)
                    //returnValue = Convert.ToSingle(match.Groups[1].Value);
                    returnValue = (string)match.Groups[1].Value;
                else
                    returnValue = val;
                return returnValue;
            }
            catch(Exception ex) 
            { 
                Console.WriteLine(ex.Message);
                return ("ERROR");
            }
        }

    }

    public class ChatGptRequest
    {
        public string model { get; set; }
        public List<dynamic> messages { get; set; }
        //public int max_tokens { get; set; }
        
    }


//ns
}
