//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
using System;
using System.Collections;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Upperbay.Core.Logging;


namespace Upperbay.Core.Library
{
    public static class Utilities
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myType"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static ArrayList GetDecoratedProperties(Type myType, string attribute)
        {
            //const int MAX_ARRAY = 100;
            ArrayList foundProperties = new ArrayList();
            bool _debug = false;

            int index = 0;

            if (_debug == true) Log2.Trace("Entering GetDecoratedProperties {0}", myType.FullName);

            //Reflect on my properties and user attributes
            try
            {
                //Log2.Trace("Parsing Classname: {0}", className);
                //Type myType = Type.GetType(className);
                //if (myType == null)
                //    Log2.Trace("ERROR: Type {0} not found!", className);

                if (_debug == true) Log2.Trace("Parsing Classname: {0}", myType.FullName);

                PropertyInfo[] myProperties = myType.GetProperties();

                if (myProperties != null)
                {
                    for (int i = 0; i < myProperties.Length; i++)
                    {
                        if (myProperties[i].CanRead && myProperties[i].CanWrite)
                        {
                            if (_debug == true) Log2.Trace("Found Property {0}", myProperties[i].Name);
                            //FieldInfo field = myType.GetField (myProperties[i].ToString());
                            object[] attributes = myProperties[i].GetCustomAttributes(typeof(UaAttribute), false);
                            
                              for (int j = 0; j < attributes.Length; j++)
                              {
                                
                                if (attributes != null && attributes.Length > 0)
                                {
                                    // Found UA Attribute
                                    UaAttribute propertyFacet = (UaAttribute)attributes[j];
                                    if (_debug == true) Log2.Trace("{0} has attribute {1}", myProperties[i].Name, propertyFacet.PropertyFacet);
                                    if (propertyFacet.PropertyFacet == attribute)
                                    {
                                        foundProperties.Add(myProperties[i].Name);
                                        index++;
                                        if (_debug == true) Log2.Trace("{0} has rw attribute {1}", myProperties[i].Name, propertyFacet.PropertyFacet);
                                    }
                                }
                             }
                        }
                        else if (myProperties[i].CanRead && !myProperties[i].CanWrite)
                        {
                            if (_debug == true) Log2.Trace("{0} is Read Only", myProperties[i].Name);
                            //FieldInfo field = myType.GetField (myProperties[i].ToString());
                            object[] attributes = myProperties[i].GetCustomAttributes(typeof(UaAttribute), false);

                              for (int j = 0; j < attributes.Length; j ++)
                              {

                                    if (attributes != null && attributes.Length > 0)
                                    {
                                        UaAttribute propertyFacet = (UaAttribute)attributes[j];
                                        if (_debug == true) Log2.Trace("{0} has attribute {1}", myProperties[i].Name, propertyFacet.PropertyFacet);
                                        if (propertyFacet.PropertyFacet == attribute)
                                        {
                                            foundProperties.Add(myProperties[i].Name);
                                            index++;
                                            if (_debug == true) Log2.Trace("{0} has ro attribute {1}", myProperties[i].Name, propertyFacet.PropertyFacet);
                                        }
                                    }
                              }
                        }
                    }
                }
                else
                {
                    if (_debug == true) Log2.Trace("GetDecoratedProperties: No Properties");
                }
                return foundProperties;
            }
            catch (Exception Ex)
            {
                Log2.Trace("ERROR:Exception Reading Properties: {0}", Ex.ToString());
                return null;
            }
        }//End GetDecorated

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        public static string ComputeSha256Hash(string rawData)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collective"></param>
        /// <param name="community"></param>
        /// <param name="cluster"></param>
        /// <param name="localAgentName"></param>
        /// <returns></returns>
        //public static string GetGlobalAgentName(    string celestial,
        //                                            string collective,
        //                                            string community,
        //                                            string cluster,
        //                                            string localAgentName)
        //{

        //    string globalAgentName = "/" + celestial + "/" + collective + "/" + community + "/" + cluster + "/" + localAgentName;
        //    return globalAgentName;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="carrier"></param>
        /// <param name="colony"></param>
        /// <param name="service"></param>
        /// <param name="agent"></param>
        /// <returns></returns>
        //public static string GetLocalAgentName( string colony,
        //                                        string agent)
        //{
        //    string localAgentName = colony + "/" + agent;
        //    return localAgentName;
        //}


    }//End Class
}// End Namespace
