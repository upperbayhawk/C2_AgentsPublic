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
using System.Threading;
using System.Configuration;
using System.IO;
using Upperbay.Core.Logging;
using Upperbay.Core.Library;
using System.Reflection.Emit;

using CSCore;
using CSCore.MediaFoundation;
using CSCore.SoundOut;
using CSCore.Codecs.WAV;


namespace Upperbay.Worker.Voice
{
    public static class Voice
    {

        #region Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="sentence"></param>
        public static void Speak(string sentence)
        {
            string voiceEnabled = MyAppConfig.GetParameter("VoiceEnabled");
            
            if (voiceEnabled != null) Boolean.TryParse(voiceEnabled, out _voiceEnabled);

            string mySentence = sentence.Trim();
            if ((_voiceEnabled) && (mySentence.Length > 4))
            {
                lock (_writeLock)
                {
                    try
                    {
                        Voice.SpeakEasy(mySentence);

                    }
                    catch (Exception ex)
                    {
                        Log2.Error("Speak Exception Aborted = {0}", ex);
                    }
                }
            }
        }




        /// <summary>
        /// 
        /// </summary>
        private static void SpeakEasy(string sentence)
        {
            lock (_writeLock)
            {
                try
                {

                    string voice = MyAppConfig.GetParameter("Voice");
                    if (voice == null)
                    {
                        voice = "Microsoft Hazel Desktop";
                    }
                    string audioDevice = null;
                    int audioDeviceID = 0;

                    audioDevice = MyAppConfig.GetParameter("GameVoiceOutputDevice");
                    if (audioDevice != null)
                    {
                        audioDeviceID = int.Parse(audioDevice);
                    }

                    SpeechSynthesizer synthesizer = new SpeechSynthesizer();
                    synthesizer.SelectVoice(voice);

                    if (true)
                    {
                        synthesizer.SetOutputToDefaultAudioDevice();
                        //synthesizer.SpeakAsync(sentence);
                        synthesizer.Speak(sentence);
                    }
                    else
                    {
                        //MemoryStream stream = new MemoryStream();
                        //synthesizer.SetOutputToWaveStream(stream);
                        //synthesizer.Speak(sentence);

                        //using (var waveOut = new WaveOut { Device = new WaveOutDevice(audioDeviceID) })
                        //using (var waveSource = new MediaFoundationDecoder(stream))
                        //{
                        //    waveOut.Initialize(waveSource);
                        //    waveOut.Play();
                        //    waveOut.WaitForStopped();
                        //}
                    }

                }
                catch (Exception ex)
                {
                    Log2.Error("Voice Exception: {0}:{1}:{2}", ex.StackTrace, ex.Message,ex.InnerException);
                }
            }
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public static void SpeakVoiceCommandFile(string fileName)
        {
            string voiceEnabled = MyAppConfig.GetParameter("VoiceEnabled");
            if (voiceEnabled != null) Boolean.TryParse(voiceEnabled, out _voiceEnabled);
            if (File.Exists(fileName))
            {
                if (_voiceEnabled)
                {
                    lock (_writeLock)
                    {
                        try
                        {
                            _fileName = fileName;
                            Voice.FileSpeaker();
                        }
                        catch (Exception ex)
                        {
                            Log2.Error("SpeakVoiceFile Exception Aborted = {0}", ex);
                        }
                    }
                }
            }
        }




        /// <summary>
        /// 
        /// </summary>
        private static void FileSpeaker()
        {

            int counter = 0;
            string line;
            lock (_writeLock)
            {
                try
                {
                    if (_fileName != null)
                    {
                        // Read the file and display it line by line.  
                        //string fileName = ConfigurationManager.AppSettings["VoiceCommandFile"];
                        Thread.Sleep(4000);
                        System.IO.StreamReader file =
                            new System.IO.StreamReader(_fileName);
                        while ((line = file.ReadLine()) != null)
                        {
                            if ((!line.StartsWith("#") && line.Length > 4))
                            {
                                Voice.Speak(line);
                                Thread.Sleep(10000);
                                counter++;
                            }
                        }
                        file.Close();
                    }
                }
                catch (Exception ex)
                {
                    Log2.Error("Exception: Voice File NOT Found: {0}", ex);
                }
            }
        }

        #endregion

        #region Private State

        private static string _fileName = null;
        private static object _writeLock = new object();
        private static bool _voiceEnabled = false;
        #endregion
    }
}
