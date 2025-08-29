//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
using System;
using System.Speech.Synthesis;
namespace KeepTalking
{
    class Program
    {
        static void Main(string[] args)
        {
            var synthesizer = new SpeechSynthesizer();
            foreach (var voice in synthesizer.GetInstalledVoices())
            {
                var info = voice.VoiceInfo;
                Console.WriteLine($"Id: {info.Id} | Name: {info.Name} | Age: { info.Age} | Gender: { info.Gender} | Culture: { info.Culture} ");
            }
            Console.ReadKey();
        }
    }
}