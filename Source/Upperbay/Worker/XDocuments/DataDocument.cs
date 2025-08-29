//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
using System.Linq;
using System.IO;


using System.Xml.Linq;

namespace Upperbay.Core.Network
{
    public class DataDocument
    {

        // Public
        private string _xmlDocument = null;
        public string XmlDocument { get { return this._xmlDocument; } set { this._xmlDocument = value; } }

        //private string _xmlFile =  @"P2DML-V0001-ElectricityPricing.xml";
        private string _xmlFile = @"c:\c4\externaltools\p2dml\bin\debug\P2DML-V0001-ElectricityPricing.xml";
        public string XmlFile { get { return this._xmlFile; } set { this._xmlFile = value; } }

        // Private
        private XDocument _xDocument = null;

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Constructor
        /// </summary>
        public DataDocument()
        {
        }
             
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool InitializeFromFile()
        {
            // Load into a string
            _xmlDocument = File.ReadAllText(_xmlFile); 
            
            // Load string into an XDocument
            _xDocument = new XDocument();
            _xDocument = XDocument.Parse(_xmlDocument, LoadOptions.SetBaseUri);

            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool InitializeFromText()
        {
            
            // Load string into an XDocument
            _xDocument = new XDocument();
            _xDocument = XDocument.Parse(_xmlDocument, LoadOptions.SetBaseUri);

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool SaveToFile()
        {
            _xDocument.Save(_xmlFile);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool SaveToString()
        {
            _xmlDocument = _xDocument.ToString();
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pricingZone"></param>
        /// <returns></returns>
        public string GetRealTimePrice(string pricingZone)
        {
            string priceString = null;

           // XDocument loaded = XDocument.Parse(_xmlDocument, LoadOptions.SetBaseUri);

            // Query the data and write out a subset of prices

            var query = from c in _xDocument.Descendants("{http://www.upperbay.com/xml/p2dml-v0001}Price")
                        where (string)c.Element("{http://www.upperbay.com/xml/p2dml-v0001}Zone") == pricingZone
                        select (string)c.Element("{http://www.upperbay.com/xml/p2dml-v0001}ValueString");


            foreach (string val in query)
            {
                priceString = val.ToString();
            }

            return priceString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pricingZone"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public bool PutRealTimePrice(string pricingZone, string price)
        {

            //XDocument loaded = XDocument.Parse(_xmlDocument, LoadOptions.SetBaseUri);

            // Query the data and write out a subset of prices

            var query = from c in _xDocument.Descendants("{http://www.upperbay.com/xml/p2dml-v0001}Price")
                        where (string)c.Element("{http://www.upperbay.com/xml/p2dml-v0001}Zone") == pricingZone
                        select c.Element("{http://www.upperbay.com/xml/p2dml-v0001}ValueString");


            foreach (var val in query)
            {
                val.SetValue(price);
                return true;
            }

            return false;
        }
    }
}
