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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Speech.Synthesis;
using System.Management;
using CSCore;
using CSCore.MediaFoundation;
using CSCore.SoundOut;

namespace WaveOutTest
{
    class Program
    {
        static void Main()
        {
          //  ManagementObjectSearcher objSearcher = new ManagementObjectSearcher(
          //"SELECT * FROM Win32_SoundDevice");

          //  ManagementObjectCollection objCollection = objSearcher.Get();

          //  foreach (ManagementObject obj in objCollection)
          //  {
          //      foreach (PropertyData property in obj.Properties)
          //      {
          //          Console.Out.WriteLine(String.Format("{0}:{1}", property.Name, property.Value));
          //      }
          //  }


            using (var stream = new MemoryStream())
            using (var speechEngine = new SpeechSynthesizer())
            {
                Console.WriteLine("Available devices:");
                foreach (var device in WaveOutDevice.EnumerateDevices())
                {
                    Console.WriteLine("{0}: {1}", device.DeviceId, device.Name);
                }
                Console.WriteLine("\nEnter device for speech output:");
                var deviceId = (int)char.GetNumericValue(Console.ReadKey().KeyChar);

                speechEngine.SetOutputToWaveStream(stream);
                speechEngine.Speak("Testing 1 2 3");

                using (var waveOut = new WaveOut { Device = new WaveOutDevice(deviceId) })
                using (var waveSource = new MediaFoundationDecoder(stream))
                {
                    waveOut.Initialize(waveSource);
                    waveOut.Play();
                    waveOut.WaitForStopped();
                }
            }
            Console.ReadLine();
        }
    }
}

