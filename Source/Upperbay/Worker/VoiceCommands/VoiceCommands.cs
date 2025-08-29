using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

using Upperbay.Core.Library;
using Upperbay.Core.Logging;
using Upperbay.Agent.Interfaces;
using Upperbay.Worker.JSON;
using Upperbay.Worker.Voice;

namespace Upperbay.Worker.VoiceCommands
{
    public class VoiceCommands
    {
        public bool SpeakVoiceFile(string fileName)
        {

            int counter = 0;
            string line;

            // Read the file and display it line by line.  
            System.IO.StreamReader file =
                new System.IO.StreamReader(fileName);
            while ((line = file.ReadLine()) != null)
            {
                Voice.Speak("This is " + vocalName + ". " + startupMessage);
                counter++;
            }

            file.Close();




            return true;
        }



    }
}
