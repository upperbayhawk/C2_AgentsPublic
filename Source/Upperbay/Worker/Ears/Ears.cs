//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
using System;
using System.Configuration;
using System.Speech.Recognition;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

using Upperbay.Core.Logging;


namespace Upperbay.Worker.Ears
{
    public static class Listen2Master
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        static bool Listening()
        {
            // Create an in-process speech recognizer for the en-US locale.  
            using (
            SpeechRecognitionEngine recognizer =
              new SpeechRecognitionEngine(
                new System.Globalization.CultureInfo("en-US")))
            {

                // Create and load a dictation grammar.  
                recognizer.LoadGrammar(new DictationGrammar());

                // Add a handler for the speech recognized event.  
                recognizer.SpeechRecognized +=
                  new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);

                // Configure input to the speech recognizer.  
                recognizer.SetInputToDefaultAudioDevice();

                // Start asynchronous, continuous speech recognition.  
                recognizer.RecognizeAsync(RecognizeMode.Multiple);

                // Keep the console window open.  
                while (true)
                {
                   // string smsAccountName = ConfigurationManager.AppSettings["SMSAccountName"];
                    Console.ReadLine();
                }
            }
        }

        /// <summary>
        /// Handle the SpeechRecognized event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
           Log2.Info("Recognized text: " + e.Result.Text);
        }
    }
}

