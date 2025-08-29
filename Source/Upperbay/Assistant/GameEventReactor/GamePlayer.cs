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
using System.Configuration;
using System.Xml;
using System.IO;
using System.Runtime;
using System.Windows.Forms;
using System.Security.Cryptography;

using Upperbay.Core.Library;

namespace Upperbay.Assistant
{
    class GamePlayer
    {
        #region Methods
        public GamePlayer()
        { 
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string CreatePlayerSignature()
        {
            string currentForCarbonWatts = MyAppConfig.GetParameter("CurrentForCarbonWatts");
            string ethereumExternalAddress = MyAppConfig.GetParameter("EthereumExternalAddress");
            string gamePlayerID = MyAppConfig.GetParameter("GamePlayerID");

            string signatureData = currentForCarbonWatts + ethereumExternalAddress + gamePlayerID;
            signatureData.Replace(" ", String.Empty);

            string gamePlayerSignature = ComputeSha256Hash(signatureData);

            var xmlDoc = new XmlDocument();
            string fileName = Path.Combine(Application.StartupPath, "config\\ClusterAgent.app.config");
            xmlDoc.Load(fileName);
            xmlDoc.SelectSingleNode("//appSettings/add[@key='GamePlayerSignature']").Attributes["value"].Value = gamePlayerSignature;
            xmlDoc.Save(fileName);
            ConfigurationManager.RefreshSection("appSettings");
            //string gamePlayerSignature = ConfigurationManager.AppSettings["GamePlayerSignature"];

            return gamePlayerSignature;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        private string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
    #endregion
}
