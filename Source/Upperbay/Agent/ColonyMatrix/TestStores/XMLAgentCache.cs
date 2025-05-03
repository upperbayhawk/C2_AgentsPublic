
using System;
using System.Xml;
using System.Configuration;
using System.Collections;
using System.Diagnostics;
//using Upperbay.Core.Library;


namespace Upperbay.Agent.ColonyMatrix
{
    /// <summary>
    /// Agent.Cache is an object caching service that
    /// present the cached object in an hierarchical structure. 
    /// It uses a pluggable object storage mechnism or cache strategy
    /// to storage the objects.
    /// </summary>
    ///  
    [Serializable]
    public class XMLAgentCache
    {
        private XmlElement objectXmlMap = null;
        private ICacheStrategy cs = null;
        private XmlDocument rootXml = new XmlDocument();
        private static XMLAgentCache cache = null;
        private static object lockObj = new Object();
        private string cacheGuid = null;

        private static string name = "UpperbayAgentCache";
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
	
        /// <summary>
        /// Private construtor, required for singleton design pattern.
        /// </summary>
        private XMLAgentCache()
        {
            ////retrieve setting from configuration file
            //ConfigurationManager cm = (ConfigurationManager)ConfigurationSettings.GetConfig("Framework");
            ////load the cache strategy object
            //cs = (ICacheStrategy)cm.CacheConfig.GetCacheStrategy();
     cs = new EnterpriseCacheStrategy();
            //TODO: Add pluggable cache strategy
//     cs = new DefaultCacheStrategy();
//        cs = new MMFCacheStrategy();
            //create an Xml used as a map between  xml expression and object cached in the
            //physical storage.
        }


        private void Initialize()
        {
            objectXmlMap = rootXml.CreateElement(Name);
            //build the internal xml document.
            rootXml.AppendChild(objectXmlMap);
            cs.Initialize(Name);
            cacheGuid = System.Guid.NewGuid().ToString();
        }


        /// <summary>
        /// Singlton method used to return the instance of Cache class
        /// </summary>
        /// <returns></returns>
        public static XMLAgentCache GetAgentCacheService()
        {
            if (cache == null)
            {
                lock (lockObj)
                {
                    if (cache == null)
                    {
                        cache = new XMLAgentCache();
                        cache.Initialize();
                    }
                }
            }
            return cache;
        }

        

        /// <summary>
        /// Add the object to the underlying storage and Xml mapping document
        /// </summary>
        /// <param name="xpath">the hierarchical location of the object in Xml document </param>
        /// <param name="o">the object to be cached</param>
        public virtual void AddObject(string xpath, object o)
        {
            lock (this)
            {
                //Debug.WriteLine(cacheGuid + " Cache Add: " + xpath);
                //clear up the xpath expression
                string newXpath = PrepareXpath(xpath);
                int separator = newXpath.LastIndexOf("/");
                //find the group name
                string group = newXpath.Substring(0, separator);
                //find the item name
                string element = newXpath.Substring(separator + 1);

                XmlNode groupNode = objectXmlMap.SelectSingleNode(group);
                //determin if group is already exist?, if not, create one.
                if (groupNode == null)
                {
                    //lock (this)
                    {
                        //build the xml tree
                        groupNode = CreateNode(group);
                    }
                }
                //get a unique key to identity of object, it is used to map
                //between xml and object key used in the cache strategy
                string objectId = System.Guid.NewGuid().ToString();
                //create an new element and new attribute for this perticular object
                XmlElement objectElement = objectXmlMap.OwnerDocument.CreateElement(element);
                XmlAttribute objectAttribute = objectXmlMap.OwnerDocument.CreateAttribute("objectId");
                objectAttribute.Value = objectId;
                objectElement.Attributes.Append(objectAttribute);
                //Add the object element to the Xml document
                groupNode.AppendChild(objectElement);

                //add the object to the underlying storage through cache strategy
                //Debug.WriteLine(cacheGuid + " Cache Add: " + objectId);
                cs.AddObject(objectId, o);
            }
        }



        /// <summary>
        /// Retrieve the cached object using its hierarchical location
        /// </summary>
        /// <param name="xpath">hierarchical location of the object in xml document</param>
        /// <returns>cached object </returns>
        public virtual object RetrieveObject(string xpath)
        {
            lock (this)
            {
               // Debug.WriteLine(cacheGuid + " Cache Get: " + xpath);
                object o = null;
                XmlNode node = objectXmlMap.SelectSingleNode(PrepareXpath(xpath));
                //if the hierarchical location existed in the xml, retrieve the object
                //otherwise, return the object as null
                if (node != null)
                {
                    string objectId = node.Attributes["objectId"].Value;
                    //retrieve the object through cache strategy
                    //Debug.WriteLine(cacheGuid + " Cache Get: " + objectId);
                    o = cs.RetrieveObject(objectId);
                }
                return o;
            }

        }





        /// <summary>
        /// Remove the object from the storage and clear the Xml assocated with
        /// the object
        /// </summary>
        /// <param name="xpath">hierarchical locatioin of the object</param>
        public virtual void RemoveObject(string xpath)
        {
            lock (this)
            {
                //Debug.WriteLine(cacheGuid + " Cache Remove: " + xpath);
                XmlNode result = objectXmlMap.SelectSingleNode(PrepareXpath(xpath));
                //check if the xpath refers to a group(container) or
                //actual element for cached object
                if (result.HasChildNodes)
                {
                    //remove all the cached object in the hastable
                    //and remove all the child nodes 
                    XmlNodeList objects = result.SelectNodes("*[@objectId]");
                    string objectId = "";
                    foreach (XmlNode node in objects)
                    {
                        objectId = node.Attributes["objectId"].Value;
                        node.ParentNode.RemoveChild(node);
                        //use cache strategy to remove the objects from the 
                        //underlying storage
                        //Debug.WriteLine(cacheGuid + " Cache Remove: " + objectId);
                        cs.RemoveObject(objectId);

                    }

                }
                else
                {
                    //just remove the element node and the object associate with it
                    string objectId = result.Attributes["objectId"].Value;
                    result.ParentNode.RemoveChild(result);
                    //Debug.WriteLine(cacheGuid + " Cache Remove: " + objectId);
                    cs.RemoveObject(objectId);
                }
            }
        }

        /// <summary>
        /// Retrive a list of object under a hierarchical location
        /// </summary>
        /// <param name="xpath">hierarchical location</param>
        /// <returns>an array of objects</returns>
        public virtual object[] RetrieveObjectList(string xpath)
        {

            lock (this)
            {
                XmlNode group = objectXmlMap.SelectSingleNode(PrepareXpath(xpath));
                XmlNodeList results = group.SelectNodes(PrepareXpath(xpath) + "/*[@objectId]");
                ArrayList objects = new ArrayList();
                string objectId = null;
                //loop through each node and link the object in object[]
                //to objects stored via cache strategy
                foreach (XmlNode result in results)
                {
                    objectId = result.Attributes["objectId"].Value;
                    objects.Add(cs.RetrieveObject(objectId));
                }
                //convert the ArrayList to object[]
                return (object[])objects.ToArray(typeof(System.Object));
            }
        }




        /// <summary>
        /// CreateNode is responsible for creating the xml tree that is
        /// specificed in the hierarchical location of the object.
        /// </summary>
        /// <param name="xpath">hierarchical location</param>
        /// <returns></returns>
        private XmlNode CreateNode(string xpath)
        {
            string[] xpathArray = xpath.Split('/');
            string root = "";
            XmlNode parentNode = (XmlNode)objectXmlMap;
            //loop through the array of levels and create the corresponding node for each level
            //skip the root node.
            for (int i = 1; i < xpathArray.Length; i++)
            {
                XmlNode node = objectXmlMap.SelectSingleNode(root + "/" + xpathArray[i]);
                // if the current location doesn't exist, build one
                //otherwise set the current locaiton to the it child location
                if (node == null)
                {
                    XmlElement newElement = objectXmlMap.OwnerDocument.CreateElement(xpathArray[i]);
                    parentNode.AppendChild(newElement);
                }
                //set the new location to one level lower
                root = root + "/" + xpathArray[i];
                parentNode = objectXmlMap.SelectSingleNode(root);
            }
            return parentNode;
        }




        /// <summary>
        /// clean up the xpath so that extra '/' is removed
        /// </summary>
        /// <param name="xpath">hierarchical location</param>
        /// <returns></returns>
        private string PrepareXpath(string xpath)
        {
            string[] xpathArray = xpath.Split('/');
            xpath = "/" + Name;
            foreach (string s in xpathArray)
            {
                if (s != "")
                {
                    xpath = xpath + "/" + s;
                }
            }
            return xpath;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="o"></param>
        public virtual void UpdateObject(string xpath, object o)
        {
            lock (this)
            {
                //Debug.WriteLine(cacheGuid + " Cache Remove: " + xpath);
                XmlNode result = objectXmlMap.SelectSingleNode(PrepareXpath(xpath));
                //check if the xpath refers to a group(container) or
                //actual element for cached object
                //just remove the element node and the object associate with it
                string objectId = result.Attributes["objectId"].Value;
                //Debug.WriteLine(cacheGuid + " Cache Remove: " + objectId);
                cs.RemoveObject(objectId);
                cs.AddObject(objectId, o);
            }
        }
    }



}
