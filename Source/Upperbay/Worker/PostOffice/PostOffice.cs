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

using TextmagicRest;

using Upperbay.Core.Logging;
using Upperbay.Core.Library;


namespace Upperbay.Worker.PostOffice
{
    public class MessageMailer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public bool SendSMSText(string body)
        {
            try
            {

                string systemName = MyAppConfig.GetParameter("SystemName");
                string systemBrand = MyAppConfig.GetParameter("SystemBrand");
                string smsMessageBody = systemBrand + " " + systemName + ": " + body;
                Log2.Info(smsMessageBody);

                string smsAccountEnabled = MyAppConfig.GetParameter("SMSAccountEnabled");
                bool enabled = false;
                Boolean.TryParse(smsAccountEnabled, out enabled);
                if (enabled)
                {
                    string cluster = MyAppConfig.GetParameter("ClusterName");
                    string smsAccountName = MyAppConfig.GetClusterParameter(cluster,"SMSAccountName");
                    string smsAccountKey = MyAppConfig.GetClusterParameter(cluster,"SMSAccountKey");
                    string smsTargetPhoneNumber = MyAppConfig.GetParameter("SMSTargetPhoneNumber");

                    //var client = new Client("davidhardin2", "eWymWS3pHCqvewP8NqKdc2DnvCABDE");
                    var client = new Client(smsAccountName, smsAccountKey);

                    var link = client.SendMessage(smsMessageBody, smsTargetPhoneNumber);
                    if (link.Success)
                    {
                        Log2.Info("Message ID {0}, has been successfully sent to {1}: {2}", link.Id, smsTargetPhoneNumber, smsMessageBody);
                        return true;
                    }
                    else
                    {
                        Log2.Error("Message was not sent due to following exception: {0}", link.ClientException.Message);
                        return false;
                    }
                }
                else
                {
                    Log2.Debug("SendSMSText NOT enabled. Set SMSAccountEnabled = true in App.Config");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log2.Error("SendSMSText Exception: {0} {1}", ex.Message, ex);
                return false;
            }
        }


        //public bool SendSMSTextViaEmail(string body)
        //{
        //    try
        //    {
        //        string traceMode = ConfigurationManager.AppSettings["traceMode"];
        //        MailMessage mail = new MailMessage();
        //        SmtpClient SmtpServer = new SmtpClient("smtp.upperbay.com");
        //        mail.To.Add("7745715386@vtext.com");
        //       // mail.Subject = "Test Text";
        //        mail.Body = body;

        //        SmtpServer.Port = 143;
        //        SmtpServer.Credentials = new System.Net.NetworkCredential("dave@upperbay.com", "!LittleRedVette87!");
        //        SmtpServer.EnableSsl = false;
        //        SmtpServer.Send(mail);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log2.Info("{0} {1}", ex.Message, ex);
        //        return false;
        //    }
           
    }
}

