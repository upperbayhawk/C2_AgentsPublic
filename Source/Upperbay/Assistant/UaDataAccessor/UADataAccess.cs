using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

using System.Text.RegularExpressions;


//for UA
using System.Security.Cryptography.X509Certificates;

// Assemblies needed for Agentness
using Upperbay.Core.Logging;

//for UA
using Opc.Ua;
using Opc.Ua.Client;

namespace Upperbay.Assistant
{
    class UADataAccess
    {

        // for logging & debugging
        //private static TraceSwitch _agentSwitch = Upperbay.Core.Logging.Log2.AgentSwitch;
        //private static TraceSwitch _agentSwitch = null;

        /// <summary>
        /// The URL of the server.
        /// </summary>
        //public const string DefaultServerUrl = "http://localhost:6000/UA/SampleClient";
        // public const string DefaultServerUrl = "opc.tcp://localhost:5001/UA/SampleServer";
        public const string DefaultServerUrl = "http://localhost:5000/UA/SampleServer";

        private string serverUrl = DefaultServerUrl;
        public string ServerUrl { get { return this.serverUrl; } set { this.serverUrl = value; } }


        /// <summary>
        /// Stores information about a item.
        /// </summary>
        private class NodeOfInterest
        {
            public NodeId NodeId;
            public LocalizedText DisplayName;
            public DataValue Value;
            public NodeId DataType;
            public int ValueRank;
            public MonitoredItem MonitoredItem;
            public string NodeUrl;
            public string NodeProperty;
        }

        private static string[] _nameSpaceArray = null;
        private static NamespaceTable _nameSpaceTable = null;


        private Session session;

        private static Dictionary<string, DataValue> _dataCache = new Dictionary<string, DataValue>();


        /// <summary>
        /// Used to synchronize access to data via multiple threads.
        /// </summary>
        private static object m_lock = new object();


        private int _maxUaIds = 100;
        public int MaxUaIds { get { return this._maxUaIds; } set { this._maxUaIds = value; } }

        List<NodeOfInterest> nodes = null;
        string[] nodeNames = null;

        string[] nodeUrls = null;
        string[] nodeProps = null;

        int currentNodeIdIndex = 0;
        int currentNodeUrlIndex = 0;


        public UADataAccess()
        {
            nodeNames = new string[_maxUaIds];
            nodeUrls = new string[_maxUaIds];
            nodeProps = new string[_maxUaIds];
        }


        /// <summary>
        /// 
        /// </summary>
        public void Configure()
        {

            try
            {
                // the application configuration can be loaded from any file.
                // ApplicationConfiguration.Load method loads configuration by looking up a file path in the app.config.
                ApplicationConfiguration configuration = new ApplicationConfiguration();

                configuration.ApplicationName = "My Friendly Application Name";
                configuration.ApplicationType = ApplicationType.Client;
                configuration.ApplicationUri = "http://localhost/VendorId/ApplicationId/InstanceId";

                configuration.SecurityConfiguration = new SecurityConfiguration();
                configuration.SecurityConfiguration.ApplicationCertificate = new CertificateIdentifier();

                // specify the location of the certificate as part of the configuration.
                // LocalMachine - use the machine wide certificate store.
                // Personal - use the store for individual certificates.
                // ApplicationName - use the application name as a search key.                
                configuration.SecurityConfiguration.ApplicationCertificate = new CertificateIdentifier();
                configuration.SecurityConfiguration.ApplicationCertificate.StoreLocation = StoreLocations.LocalMachine;
                configuration.SecurityConfiguration.ApplicationCertificate.StoreName = StoreNames.Personal;
                configuration.SecurityConfiguration.ApplicationCertificate.SubjectName = configuration.ApplicationName;

                // find the certificate in the store.
                X509Certificate2 clientCertificate = configuration.SecurityConfiguration.ApplicationCertificate.Find(true);

                // create a new certificate if one not found.
                if (clientCertificate == null)
                {
                    // this code would normally be called as part of the installer - called here to illustrate.
                    Opc.Ua.Security.CertificateFactory factory = new Opc.Ua.Security.CertificateFactory();

                    // create a new certificate an place it in the LocalMachine/Personal store.
                    clientCertificate = factory.CreateCertificate(
                        StoreLocation.LocalMachine,
                        configuration.SecurityConfiguration.ApplicationCertificate.StoreName,
                        configuration.ApplicationName,
                        configuration.ApplicationUri,
                        new string[] { "localhost" },
                        "Upperbay Systems LLC",
                        1024,
                        1);

                    Log2.Trace("Created client certificate: {0}", clientCertificate.Subject);
                }

                // it possible to add your own bindings by subtype Opc.Ua.Bindings.BaseBinding
                configuration.TransportConfigurations.Add(new TransportConfiguration(Utils.UriSchemeOpcTcp, typeof(Opc.Ua.Bindings.UaTcpBinding)));
                configuration.TransportConfigurations.Add(new TransportConfiguration(Utils.UriSchemeHttp, typeof(Opc.Ua.Bindings.UaSoapXmlBinding)));

                configuration.TransportQuotas = new TransportQuotas();
                configuration.TransportQuotas.OperationTimeout = 60000;

                configuration.ClientConfiguration = new ClientConfiguration();
                configuration.ClientConfiguration.DefaultSessionTimeout = 30000;

                // trace files capture detailed error informate produced by the SDK and/or stack.
                configuration.TraceConfiguration = new TraceConfiguration();
                configuration.TraceConfiguration.OutputFilePath = "trace.txt";
                configuration.TraceConfiguration.DeleteOnLoad = true;

                // must be called before it can be used (called automatically if Load() is called.
                configuration.Validate(ApplicationType.Client);

                // create the binding factory.
                BindingFactory bindingFactory = BindingFactory.Create(configuration);

                ConfiguredEndpointCollection endpoints = new ConfiguredEndpointCollection(configuration);
                ConfiguredEndpoint endpoint = endpoints.Create(serverUrl);

                // UpdateFromServer selects the first endpoint description with SignAndEncrypt enabled.
                endpoint.Description.SecurityMode = MessageSecurityMode.SignAndEncrypt;

                // update endpoint description using the discovery endpoint.
                if (endpoint.UpdateBeforeConnect)
                {
                    endpoint.UpdateFromServer(bindingFactory);
                    Log2.Trace("Updated endpoint description for url: {0}", serverUrl);
                }

                // validate the server certificate.
                byte[] certificateData = endpoint.Description.ServerCertificate;
                X509Certificate2 serverCertificate = new X509Certificate2(certificateData);

                try
                {
                    configuration.CertificateValidator.Validate(serverCertificate);
                }
                catch (ServiceResultException e)
                {
                    if (e.StatusCode != StatusCodes.BadCertificateUntrusted)
                    {
                        throw new ServiceResultException(e, StatusCodes.BadCertificateInvalid);
                    }

                    // automatically trust the certificate if it is untrusted.
                    configuration.SecurityConfiguration.AddTrustedPeer(certificateData);
                    configuration.CertificateValidator.Update(configuration);
                }

                Log2.Trace("Validated server certificate: {0}", serverCertificate.Subject);

                // Initialize the channel which will be created with the server.
                SessionChannel channel = SessionChannel.Create(
                    configuration,
                    endpoint.Description,
                    endpoint.Configuration,
                    bindingFactory,
                    clientCertificate,
                    null);

                // Wrap the channel with the session object.
                // This call will fail if the server does not trust the client certificate.
                session = new Session(channel, configuration, endpoint);
                session.ReturnDiagnostics = DiagnosticsMasks.All;

                // register keep alive callback.
                session.KeepAlive += new KeepAliveEventHandler(Session_KeepAlive);

                // passing null for the user identity will create an anonymous session.
                UserIdentity identity = null;

                // check if the endpoint supports user/password identity.
                //foreach (UserTokenPolicy tokenPolicy in endpoint.Description.UserIdentityTokens)
                //{
                //    if (tokenPolicy.TokenType == UserTokenType.UserName)
                //    {
                //        //identity = new UserIdentity("iamuser", "password");
                //        identity = new UserIdentity("", "");
                //        break;
                //    }
                //}

                // create the session. This actually connects to the server.
                session.Open("My Session Name", identity);


            }
            catch (Exception e)
            {
                Log2.Error("Unexpected exception: {0}.\nYou Are Screwed.", e.Message);
            }
        }// End Config


        public void AddNodeId(string str)
        {
            try
            {
                nodeNames.SetValue(str, currentNodeIdIndex);
                currentNodeIdIndex++;

            }
            catch (Exception ex)
            {
                Log2.Error("Unexpected exception: {0}.\nYou Are Screwed.", ex.Message);
            }

        }// end AddNOdeID

        public void AddNodeUrl(string prop, string str)
        {
            try
            {
                nodeUrls.SetValue(str, currentNodeUrlIndex);
                nodeProps.SetValue(prop, currentNodeUrlIndex);
                currentNodeUrlIndex++;

            }
            catch (Exception ex)
            {
                Log2.Error("Unexpected exception: {0}.\nYou Are Screwed.", ex.Message);
            }

        }// end AddNOdeID


        public string GetDataValue(string str)
        {
            try
            {
                return UADataAccess._dataCache[str].Value.ToString();

            }
            catch (Exception ex)
            {
                Log2.Error("GetDataValue exception: {0}", ex.Message);
                return null;
            }


        }// end GetDataValue

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string GetDataTime(string str)
        {
            try
            {
                return UADataAccess._dataCache[str].SourceTimestamp.ToString();

            }
            catch (Exception ex)
            {
                Log2.Error("GetDataTime exception: {0}", ex.Message);
                return null;
            }
        }


        public string GetDataStatus(string str)
        {
            try
            {
                return UADataAccess._dataCache[str].StatusCode.ToString();

            }
            catch (Exception ex)
            {
                Log2.Error("GetDataStatus exception: {0}", ex.Message);
                return null;
            }
        }

        public string GetDataQuality(string str)
        {
            try
            {
                if (StatusCode.IsGood(UADataAccess._dataCache[str].StatusCode))
                    return "Good";
                else
                    return "Bad";

            }
            catch (Exception ex)
            {
                Log2.Error("GetDataQuality exception: {0}", ex.Message);
                return null;
            }
        }
        
        
        /// <summary>
        /// 
        /// </summary>
        public void Run()
        {
            //NamespaceTable wellKnownNamespaceUris = new NamespaceTable();
            //wellKnownNamespaceUris.Append("opc.com://localhost/OPCSample.OpcDaServer");
            //wellKnownNamespaceUris.Append("4b01bc8f-c72a-4f4d-a10c-b6c4aa6e2519");
            //wellKnownNamespaceUris.Append("http://opcfoundation.org/UA/ComInterop/");
            //wellKnownNamespaceUris.Append("http://opcfoundation.org/UA/Test/");
            //wellKnownNamespaceUris.Append("http://opcfoundation.org/UA/Sample/");

            try
            {


                nodes = GetNodeIdsFromNodeUrls(
                        session,
                        Objects.ObjectsFolder,
                        nodeUrls.ToArray(),
                        nodeProps.ToArray());

                
                
                //nodes = GetNodeIds(
                //     session,
                //     Objects.ObjectsFolder,
                //     wellKnownNamespaceUris,
                //     nodeNames.ToArray());

                // Nodes can be identified by a path relative to a starting node.
                // This path is constructed from the namespace qualified names for each node in the path.
                // The namespaces have a URI but are identified as a index in the relative path.
                // The wellKnownNamespaceUris stores the URIs that match the indexes used.

                //nodes = GetNodeIds(
                //    session,
                //    Objects.ObjectsFolder,
                //    wellKnownNamespaceUris,
                //    "1:Data/1:Static/1:Scalar/1:Int32Value",
                //    "1:Data/1:Static/1:Scalar/1:FloatValue");

                // read the current values for the nodes. 
                ReadNodes(session, nodes);

                // create a subscription.
                Subscribe(session, nodes);

                // start a timer that updates the values.
                BeginWrite(session, nodes);

            }
            catch (Exception ex)
            {
                Log2.Error("Unexpected exception: {0}.\nYou Are Screwed.", ex.Message);
            }

        }// end Run



        public void Close()
        {
            try
            {
                session.Close(5000);

            }
            catch (Exception ex)
            {
                Log2.Error("Unexpected exception: {0}.\nYou Are Screwed.", ex.Message);
            }

        }// end Run






        /// <summary>
        /// Raised when a keep alive response is returned from the server.
        /// </summary>
        static void Session_KeepAlive(Session session, KeepAliveEventArgs e)
        {
            Log2.Trace("===>>> Session KeepAlive: {0} ServerTime: {1:HH:MM:ss}", e.CurrentState, e.CurrentTime.ToLocalTime());
        }

        /// <summary>
        /// Raised when a publish response arrives from the server.
        /// </summary>
        static void Session_Notification(Session session, NotificationEventArgs e)
        {
            NotificationMessage message = e.NotificationMessage;

            // check for keep alive.
            if (message.NotificationData.Count == 0)
            {
                Log2.Trace(
                    "===>>> Subscription KeepAlive: SubscriptionId={0} MessageId={1} Time={2:HH:mm:ss.fff}",
                    e.Subscription.Id,
                    message.SequenceNumber,
                    message.PublishTime.ToLocalTime());

                return;
            }

            // get the data changes (oldest to newest).
            foreach (MonitoredItemNotification datachange in message.GetDataChanges(false))
            {
                // lookup the monitored item.
                MonitoredItem monitoredItem = e.Subscription.FindItemByClientHandle(datachange.ClientHandle);

                if (monitoredItem == null)
                {
                    Log2.Trace("MonitoredItem ClientHandle not known: {0}", datachange.ClientHandle);
                    continue;
                }

                // this is called on another thread so we need to synchronize before accessing the node.
                lock (m_lock)
                {
                    NodeOfInterest node = monitoredItem.Handle as NodeOfInterest;

                    Log2.Trace(
                        "Update for {0}: {1} Status={2} Timestamp={3:HH:mm:ss.fff}",
                        node.DisplayName,
                        datachange.Value.WrappedValue,
                        datachange.Value.StatusCode,
                        datachange.Value.SourceTimestamp.ToLocalTime());


                    try
                    {
                        UADataAccess._dataCache[node.NodeProperty] = datachange.Value;
                        Log2.Trace("InjectedData: {0}", node.NodeProperty);
                    }
                    catch (ArgumentException ax)
                    {
                        Log2.Error("Exception: {0}", ax.ToString());
                        UADataAccess._dataCache.Add(node.NodeProperty, datachange.Value);
                        Log2.Trace("AddedData: {0}", node.NodeProperty);

                    }
                }
            }
        }

        /// <summary>
        /// Returns the node ids for a set of relative paths.
        /// </summary>
        /// <param name="session">An open session with the server to use.</param>
        /// <param name="startNodeId">The starting node for the relative paths.</param>
        /// <param name="namespacesUris">The namespace URIs referenced by the relative paths.</param>
        /// <param name="relativePaths">The relative paths.</param>
        /// <returns>A collection of local nodes.</returns>
        static List<NodeOfInterest> GetNodeIds(
            Session session,
            NodeId startNodeId,
            NamespaceTable namespacesUris,
            params string[] relativePaths)
        {

            Dictionary<int, string> relativePathCache = new Dictionary<int, string>();

            // 
            if (ReadServerNamespaceArrayNodes(session) == false)
            {
                Log2.Error("ReadServerNamespaceArrayNodes FAILED!");
            }



            // build the list of browse paths to follow by parsing the relative paths.
            BrowsePathCollection browsePaths = new BrowsePathCollection();

            if (relativePaths != null)
            {
                for (int ii = 0; ii < relativePaths.Length; ii++)
                {
                    BrowsePath browsePath = new BrowsePath();

                    // The relative paths used indexes in the namespacesUris table. These must be 
                    // converted to indexes used by the server. An error occurs if the relative path
                    // refers to a namespaceUri that the server does not recognize.

                    // The relative paths may refer to ReferenceType by their BrowseName. The TypeTree object
                    // allows the parser to look up the server's NodeId for the ReferenceType.

                    browsePath.RelativePath = RelativePath.Parse(
                        relativePaths[ii],
                        session.TypeTree,
                        namespacesUris,
                        session.NamespaceUris);

                    browsePath.StartingNode = startNodeId;

                    browsePaths.Add(browsePath);


                }
            }

            // make the call to the server.
            BrowsePathResultCollection results;
            DiagnosticInfoCollection diagnosticInfos;

            ResponseHeader responseHeader = session.TranslateBrowsePathsToNodeIds(
                null,
                browsePaths,
                out results,
                out diagnosticInfos);

            // ensure that the server returned valid results.
            Session.ValidateResponse(results, browsePaths);
            Session.ValidateDiagnosticInfos(diagnosticInfos, browsePaths);

            Log2.Trace("Translated {0} browse paths.", relativePaths.Length);

            // collect the list of node ids found.
            List<NodeOfInterest> nodes = new List<NodeOfInterest>();

            for (int ii = 0; ii < results.Count; ii++)
            {
                // check if the start node actually exists.
                if (StatusCode.IsBad(results[ii].StatusCode))
                {
                    ServiceResult error = new ServiceResult(
                        results[ii].StatusCode,
                        diagnosticInfos[ii],
                        responseHeader.StringTable);

                    Log2.Trace("Path '{0}' is not valid. Error = {1}", relativePaths[ii], error);
                    continue;
                }

                // an empty list is returned if no node was found.
                if (results[ii].Targets.Count == 0)
                {
                    Log2.Trace("Path '{0}' does not exist.", relativePaths[ii]);
                    continue;
                }

                // Multiple matches are possible, however, the node that matches the type model is the
                // one we are interested in here. The rest can be ignored.
                BrowsePathTarget target = results[ii].Targets[0];

                if (target.RemainingPathIndex != UInt32.MaxValue)
                {
                    Log2.Trace("Path '{0}' refers to a node in another server.", relativePaths[ii]);
                    continue;
                }

                // The targetId is an ExpandedNodeId because it could be node in another server. 
                // The ToNodeId function is used to convert a local NodeId stored in a ExpandedNodeId to a NodeId.

                NodeOfInterest node = new NodeOfInterest();
                node.NodeId = ExpandedNodeId.ToNodeId(target.TargetId, session.NamespaceUris);

                nodes.Add(node);
            }

            Log2.Trace("Translate found {0} local nodes.", nodes.Count);

            // return whatever was found.
            return nodes;
        }
        //-----------------------------------------------------------------------------------------

        /// <summary>
        /// Returns the node ids for a set of relative paths.
        /// </summary>
        /// <param name="session">An open session with the server to use.</param>
        /// <param name="startNodeId">The starting node for the relative paths.</param>
        /// <param name="namespacesUris">The namespace URIs referenced by the relative paths.</param>
        /// <param name="relativePaths">The relative paths.</param>
        /// <returns>A collection of local nodes.</returns>
        static List<NodeOfInterest> GetNodeIdsFromNodeUrls(
            Session session,
            NodeId startNodeId,
           // NamespaceTable namespacesUris,
            string[] urlPaths,
            params string[] nodeProps)
        {

            Dictionary<int, string> relativePathCache = new Dictionary<int, string>();
            string[] relativePaths = new string[100];


            // 
            if (ReadServerNamespaceArrayNodes(session) == false)
            {
                Log2.Error("ReadServerNamespaceArrayNodes FAILED!");
                return null;
            }


            for (int iii = 0 ; iii < urlPaths.Length ; iii++)
            {
                if (urlPaths[iii] != null)
                {
                    relativePaths[iii] = ParseNodeUrl(urlPaths[iii]);
                }
            }



            // build the list of browse paths to follow by parsing the relative paths.
            BrowsePathCollection browsePaths = new BrowsePathCollection();

            if (relativePaths != null)
            {
                for (int ii = 0; ii < relativePaths.Length; ii++)
                {
                    BrowsePath browsePath = new BrowsePath();

                    // The relative paths used indexes in the namespacesUris table. These must be 
                    // converted to indexes used by the server. An error occurs if the relative path
                    // refers to a namespaceUri that the server does not recognize.

                    // The relative paths may refer to ReferenceType by their BrowseName. The TypeTree object
                    // allows the parser to look up the server's NodeId for the ReferenceType.

                    browsePath.RelativePath = RelativePath.Parse(
                        relativePaths[ii],
                        session.TypeTree,
                        UADataAccess._nameSpaceTable,
                        session.NamespaceUris);

                    browsePath.StartingNode = startNodeId;

                    browsePaths.Add(browsePath);


                }
            }

            // make the call to the server.
            BrowsePathResultCollection results;
            DiagnosticInfoCollection diagnosticInfos;

            ResponseHeader responseHeader = session.TranslateBrowsePathsToNodeIds(
                null,
                browsePaths,
                out results,
                out diagnosticInfos);

            // ensure that the server returned valid results.
            Session.ValidateResponse(results, browsePaths);
            Session.ValidateDiagnosticInfos(diagnosticInfos, browsePaths);

            Log2.Trace("Translated {0} browse paths.", relativePaths.Length);

            // collect the list of node ids found.
            List<NodeOfInterest> nodes = new List<NodeOfInterest>();

            for (int ii = 0; ii < results.Count; ii++)
            {
                // check if the start node actually exists.
                if (StatusCode.IsBad(results[ii].StatusCode))
                {
                    ServiceResult error = new ServiceResult(
                        results[ii].StatusCode,
                        diagnosticInfos[ii],
                        responseHeader.StringTable);

                    Log2.Trace("Path '{0}' is not valid. Error = {1}", relativePaths[ii], error);
                    continue;
                }

                // an empty list is returned if no node was found.
                if (results[ii].Targets.Count == 0)
                {
                    Log2.Trace("Path '{0}' does not exist.", relativePaths[ii]);
                    continue;
                }

                // Multiple matches are possible, however, the node that matches the type model is the
                // one we are interested in here. The rest can be ignored.
                BrowsePathTarget target = results[ii].Targets[0];

                if (target.RemainingPathIndex != UInt32.MaxValue)
                {
                    Log2.Trace("Path '{0}' refers to a node in another server.", relativePaths[ii]);
                    continue;
                }

                // The targetId is an ExpandedNodeId because it could be node in another server. 
                // The ToNodeId function is used to convert a local NodeId stored in a ExpandedNodeId to a NodeId.

                NodeOfInterest node = new NodeOfInterest();
                node.NodeId = ExpandedNodeId.ToNodeId(target.TargetId, session.NamespaceUris);
                node.NodeUrl = urlPaths[ii];
                node.NodeProperty = nodeProps[ii];
                nodes.Add(node);
            }

            Log2.Trace("Translate found {0} local nodes.", nodes.Count);

            // return whatever was found.
            return nodes;
        }

        //-----------------------------------------------------------------------------------------

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeUrl"></param>
        /// <returns>RelativePath</returns>
        public static string ParseNodeUrl(string nodeUrl)
        {

            string relativePath = null;

            try
            {
                //string url =
                //    //"http://www.cambiaresearch.com/Cambia3/snippets/csharp/regex/uri_regex.aspx?0=H#authority,#frag1#frag2";
                //"uanode://UAServerLogicalName/2:FC1001/0:Input?http://opcfoundation.org/UA/Test&http://opcfoundation.org/UA/Sample&http://opcfoundation.org/UA/Junk";

                string regexPattern = @"^(?<s1>(?<s0>[^:/\?#]+):)?(?<a1>"
                   + @"//(?<a0>[^/\?#]*))?(?<p0>[^\?#]*)"
                   + @"(?<f1>#(?<f0>.*))?"
                   + @"(?<q1>\?(?<q0>[^#]*))?"
                   + @"(?<f1>#(?<f0>.*))?";

                Regex re = new Regex(regexPattern, RegexOptions.ExplicitCapture);
                Match m = re.Match(nodeUrl);

                Log2.Trace("ParseNodeUrl <b>URL: " + nodeUrl + "</b><p>");

                string str = m.Groups["s0"].Value;
                if (str != "uanode")
                    Log2.Error("ParseNodeUrl Invalid NodeURL -> {0}", str);

                //str = m.Groups["s1"].Value + "  (Scheme with colon)<br>";
                //Log2.Trace(str);

                //str = m.Groups["a0"].Value + "  (Authority without //)<br>";
                //Log2.Trace(str);
                //str = m.Groups["a1"].Value + "  (Authority with //)<br>";
                //Log2.Trace(str);

                
                relativePath = m.Groups["p0"].Value;
                relativePath = relativePath.TrimStart('/');
                Log2.Trace(relativePath);

                //  "  (Query without ?)<br>"
                //str = m.Groups["q0"].Value;
                //Log2.Trace(str);
                // "  (Query with ?)<br>"
                //str = m.Groups["q1"].Value;
                //Log2.Trace(str);

                //str = m.Groups["f0"].Value + "  (Fragment without #)<br>";
                //Log2.Trace(str);
                //str = m.Groups["f1"].Value + "  (Fragment with #)<br>";

                str = m.Groups["q0"].Value;
                string[] newstr = str.Split('&');
                Log2.Trace("ParseNodeUrl {0}", newstr.Length.ToString());
                //Log2.Trace(newstr[0]);
                //Log2.Trace(newstr[1]);
                //Log2.Trace(newstr[2]);

                for (int ii = 0; ii < newstr.Count(); ii++)
                {
                    if (newstr[ii] != null)
                    {
                        string[] nameSpace = newstr[ii].Split('=');
                        int index;
                        if (Int32.TryParse(nameSpace[0], out index))
                        {
                            int newIndex = 0;
                            if (((newIndex = UADataAccess.GetNamespaceIndex(nameSpace[1]))) == index)
                            {
                                // Good NameSpace Index
                                Log2.Trace("ParseNodeUrl Good Namespace {0}", relativePath);
                            }
                            else
                            {

                                // Bad NameSpace Index
                                Log2.Error("ParseNodeUrl BAD Namespace {0}", relativePath);
                                // Build new relativePath with new namespace indexes
                                string oldString = index.ToString() + ':';
                                Log2.Trace("oldString = {0}", oldString);
                                string newString = newIndex.ToString() + ':';
                                Log2.Trace("newString = {0}", newString);
                                Log2.Trace("Old relativePath = {0}", relativePath);
                                string myNewPath = relativePath.Replace(oldString, newString);
                                Log2.Trace("New relativePath = {0}", myNewPath);
                                relativePath = myNewPath;
                            }
                        }
                    }
                }



            }
            catch (Exception Ex)
            {
                Log2.Error("ParseNodeUrl Exception: {0}", Ex.ToString());
            }
            return relativePath;
        }

        //-----------------------------------------------------------------------------------------

        public static int GetNamespaceIndex( string nameSpaceString)
        {

            try
            {

                for (int ii = 0; ii < UADataAccess._nameSpaceTable.Count;ii++)
                {
                    if (nameSpaceString == UADataAccess._nameSpaceArray[ii])
                        return ii;
                }

                Log2.Error("GetNamespace NOT Found in Server Exception!!!");

            }
            catch (Exception Ex)
            {
                Log2.Error("GetNamespaceIndex Exception: {0}", Ex.ToString());
            }

            return 0;
        }



        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Reads the value, datatype and value rank for the specified nodes.
        /// </summary>
        /// <param name="session">An open session with the server to use</param>
        /// <param name="nodeIds">The node ids for the variables to read.</param>
        static void ReadNodes(
            Session session,
            List<NodeOfInterest> nodes)
        {
            // build list of attributes to read.
            ReadValueIdCollection nodesToRead = new ReadValueIdCollection();

            for (int ii = 0; ii < nodes.Count; ii++)
            {
                // read the browse name attribute.
                ReadValueId attributeToRead = new ReadValueId();

                attributeToRead.NodeId = nodes[ii].NodeId;
                attributeToRead.AttributeId = Attributes.DisplayName;

                // the handle is a local property that can be used whatever purpose the client likes.
                // in this example the handle is going to be used to associate the results with the input node.
                attributeToRead.Handle = nodes[ii];

                nodesToRead.Add(attributeToRead);

                // read the value attribute.
                attributeToRead = new ReadValueId();

                attributeToRead.NodeId = nodes[ii].NodeId;
                attributeToRead.AttributeId = Attributes.Value;
                attributeToRead.Handle = nodes[ii];

                nodesToRead.Add(attributeToRead);

                // read the data type attribute.
                attributeToRead = new ReadValueId();

                attributeToRead.NodeId = nodes[ii].NodeId;
                attributeToRead.AttributeId = Attributes.DataType;
                attributeToRead.Handle = nodes[ii];

                nodesToRead.Add(attributeToRead);

                // read the value rank attribute.
                attributeToRead = new ReadValueId();

                attributeToRead.NodeId = nodes[ii].NodeId;
                attributeToRead.AttributeId = Attributes.ValueRank;
                attributeToRead.Handle = nodes[ii];

                nodesToRead.Add(attributeToRead);
            }

            // make the call to the server.
            DataValueCollection results;
            DiagnosticInfoCollection diagnosticInfos;

            ResponseHeader responseHeader = session.Read(
                null,
                0,
                TimestampsToReturn.Both,
                nodesToRead,
                out results,
                out diagnosticInfos);

            // ensure that the server returned valid results.
            Session.ValidateResponse(results, nodesToRead);
            Session.ValidateDiagnosticInfos(diagnosticInfos, nodesToRead);

            Log2.Trace("Read {0} attribute values.", nodesToRead.Count);

            // write values found.
            for (int ii = 0; ii < results.Count; ii++)
            {
                string attributeName = Attributes.GetBrowseName(nodesToRead[ii].AttributeId);

                // check an error occurred.
                if (StatusCode.IsBad(results[ii].StatusCode))
                {
                    ServiceResult error = new ServiceResult(
                        results[ii].StatusCode,
                        diagnosticInfos[ii],
                        responseHeader.StringTable);

                    Log2.Trace("{0}: Error={1}", attributeName, error);
                    continue;
                }

                NodeOfInterest node = nodesToRead[ii].Handle as NodeOfInterest;

                // write value attribute.
                if (nodesToRead[ii].AttributeId == Attributes.Value)
                {
                    Log2.Trace(
                        "{0}: {1} Status={2} Timestamp={3:HH:mm:ss.fff}",
                        attributeName,
                        results[ii].WrappedValue,
                        results[ii].StatusCode,
                        results[ii].SourceTimestamp.ToLocalTime());

                    node.Value = results[ii];
                    continue;
                }

                // save the values for the different attributes.
                switch (nodesToRead[ii].AttributeId)
                {
                    case Attributes.DisplayName:
                        {
                            node.DisplayName = (LocalizedText)results[ii].GetValue(typeof(LocalizedText));
                            break;
                        }

                    case Attributes.DataType:
                        {
                            node.DataType = (NodeId)results[ii].GetValue(typeof(NodeId));
                            break;
                        }

                    case Attributes.ValueRank:
                        {
                            node.ValueRank = (int)results[ii].GetValue(typeof(int));
                            break;
                        }

                }

                // write other attribute.
                Log2.Trace("{0}: {1}", attributeName, results[ii].WrappedValue);
            }
        }


        //-----------------------------------------------------------------------------------------


        static Boolean ReadServerNamespaceArrayNodes(Session session)
        {
            // build list of attributes to read.
            ReadValueIdCollection nodesToRead = new ReadValueIdCollection();

            // read the browse name attribute.
            ReadValueId attributeToRead = new ReadValueId();

            attributeToRead.NodeId = 2255;
            attributeToRead.AttributeId = Attributes.Value;

            // the handle is a local property that can be used whatever purpose the client likes.
            // in this example the handle is going to be used to associate the results with the input node.
            attributeToRead.Handle = 2255;

            nodesToRead.Add(attributeToRead);


            // make the call to the server.
            DataValueCollection results;
            DiagnosticInfoCollection diagnosticInfos;

            ResponseHeader responseHeader = session.Read(
                null,
                0,
                TimestampsToReturn.Both,
                nodesToRead,
                out results,
                out diagnosticInfos);

            // ensure that the server returned valid results.
            Session.ValidateResponse(results, nodesToRead);
            Session.ValidateDiagnosticInfos(diagnosticInfos, nodesToRead);

            Log2.Trace("Read NameSpaceArray {0} attribute values.", nodesToRead.Count);

            // write values found.
            //for (int ii = 0; ii < results.Count; ii++)
            //{
            //string attributeName = Attributes.GetBrowseName(nodesToRead[0].AttributeId);

            //// check an error occurred.
            //if (StatusCode.IsBad(results[ii].StatusCode))
            //{
            //    ServiceResult error = new ServiceResult(
            //        results[ii].StatusCode,
            //        diagnosticInfos[ii],
            //        responseHeader.StringTable);

            //    Log2.Trace("{0}: NameSpaceArray Error={1}", attributeName, error);
            //    continue;
            //}

            //NodeOfInterest node = nodesToRead[0].Handle as NodeOfInterest;

            // write value attribute.
            if (nodesToRead[0].AttributeId == Attributes.Value)
            {
                Log2.Trace(
                    "NameSpaceArray : {0} Status={1} Timestamp={2:HH:mm:ss.fff}",
                    //attributeName,
                    results[0].WrappedValue,
                    results[0].StatusCode,
                    results[0].SourceTimestamp.ToLocalTime());

                //  node.Value = results[0];
                //continue;
            }

            _nameSpaceTable = new NamespaceTable();

            try
            {
                Variant var = results[0].WrappedValue;
                string[] str = (string[])var.Value;

                _nameSpaceArray = (string[])var.Value;

                for (int ii = 1; ii < str.Count(); ii++)
                {
                    Log2.Trace(
                            "NameString : {0}",
                            _nameSpaceArray[ii]);
                    _nameSpaceTable.Append(_nameSpaceArray[ii]);
                }

            }
            catch (Exception Ex)
            {

                Log2.Error("Exception: {0}", Ex.ToString());
                return false;

            }

            //// save the values for the different attributes.
            //switch (nodesToRead[ii].AttributeId)
            //{
            //    case Attributes.DisplayName:
            //        {
            //            node.DisplayName = (LocalizedText)results[ii].GetValue(typeof(LocalizedText));
            //            break;
            //        }

            //    case Attributes.DataType:
            //        {
            //            node.DataType = (NodeId)results[ii].GetValue(typeof(NodeId));
            //            break;
            //        }

            //    case Attributes.ValueRank:
            //        {
            //            node.ValueRank = (int)results[ii].GetValue(typeof(int));
            //            break;
            //        }

            //}

            // write other attribute.
            //Log2.Trace("NameSpaceArray {0}: {1}", attributeName, results[ii].WrappedValue);
            //}

            return true;
        }

        //-----------------------------------------------------------------------------------------

        /// <summary>
        /// Creates a subscription and monitored items for each of the specified nodes.
        /// </summary>
        /// <param name="session">An open session with the server to use</param>
        /// <param name="nodeIds">The node ids for the variables to subscribe to.</param>
        static void Subscribe(
            Session session,
            List<NodeOfInterest> nodes)
        {
            // Subscriptions have 5 parameters of interest.

            // PublishingEnabled          - whether notifications are returned (keep alives are always returned).
            // PublishingInterval         - how frequently to return notifications (in milliseconds).
            // KeepAliveCount             - how frequently to return empty keep alive messages (multiple of the PublishingInterval).
            // LifetimeCount              - how long the subscription should live if no publishes arrives (multiple of the PublishingInterval).
            // MaxNotificationsPerPublish - the maximum number of notifications to return in a single publish.

            Subscription subscription = new Subscription(session.DefaultSubscription);

            subscription.DisplayName = "My Subscription Name";
            subscription.PublishingEnabled = true;
            subscription.PublishingInterval = 1000; // in milliseconds.
            subscription.KeepAliveCount = 10;   // 10*1000  = 10s
            subscription.LifetimeCount = 100;  // 1000*100 = 100s;
            subscription.MaxNotificationsPerPublish = 10;

            // associate the subscription with the session.            
            session.AddSubscription(subscription);

            // call the server and create the subscription.
            subscription.Create();

            // at this point the subscription is sending publish requests at the keep alive rate.
            // use the Notification event the session to receive updates when a publish completes.
            session.Notification += new NotificationEventHandler(Session_Notification);

            // create the monitored items.

            // MonitoringMode   - whether changes to the item are being reported.
            // SamplingInterval - how frequently the item value is sampled.
            // QueueSize        - the number of data changes to buffer between publishes.
            // DiscardOldest    - whether the 

            for (int ii = 0; ii < nodes.Count; ii++)
            {
                MonitoredItem monitoredItem = new MonitoredItem(subscription.DefaultItem);

                monitoredItem.StartNodeId = nodes[ii].NodeId;
                monitoredItem.AttributeId = Attributes.Value;
                monitoredItem.MonitoringMode = MonitoringMode.Reporting;
                monitoredItem.SamplingInterval = 500;
                monitoredItem.QueueSize = 2;
                monitoredItem.DiscardOldest = false;
                monitoredItem.Handle = nodes[ii];

                subscription.AddItem(monitoredItem);
                nodes[ii].MonitoredItem = monitoredItem;
            }

            // call the server and apply any changes to the state of the subscription or monitored items.
            subscription.ApplyChanges();
        }

        /// <summary>
        /// Stores the state associated with a periodic write operation.
        /// </summary>
        private class WriteState
        {
            public Session Session;
            public WriteValueCollection NodesToWrite;
            public Opc.Ua.Test.DataGenerator Generator;
        }

        /// <summary>
        /// Reads the value, datatype and value rank for the specified nodes.
        /// </summary>
        /// <param name="session">An open session with the server to use</param>
        /// <param name="nodeIds">The node ids for the variables to read.</param>
        static void BeginWrite(
            Session session,
            List<NodeOfInterest> nodes)
        {
            // build list of values to write.
            WriteValueCollection nodesToWrite = new WriteValueCollection();

            // create a state object that can be passed into the async event handler.
            WriteState state = new WriteState();

            state.Session = session;
            state.NodesToWrite = nodesToWrite;

            // this is a utility class that generates random data suitable for testing.
            state.Generator = new Opc.Ua.Test.DataGenerator(null);

            lock (m_lock)
            {
                for (int ii = 0; ii < nodes.Count; ii++)
                {
                    WriteValue valueToWrite = new WriteValue();

                    valueToWrite.NodeId = nodes[ii].NodeId;
                    valueToWrite.AttributeId = Attributes.Value;
                    valueToWrite.Handle = nodes[ii];

                    nodesToWrite.Add(valueToWrite);
                }
            }

            // start the first write.
            NextWrite(state);
        }

        //-----------------------------------------------------------------------------------------

        /// <summary>
        /// Starts the next write operation.
        /// </summary>
        private static void NextWrite(WriteState state)
        {
            try
            {
                // assign some random values.
                lock (m_lock)
                {
                    for (int ii = 0; ii < state.NodesToWrite.Count; ii++)
                    {
                        WriteValue valueToWrite = state.NodesToWrite[ii];
                        valueToWrite.Value.Value = GetRandomValue(state, (NodeOfInterest)valueToWrite.Handle);
                    }
                }

                // make the call to the server.
                state.Session.BeginWrite(
                    null,
                    state.NodesToWrite,
                    OnWriteComplete,
                    state);
            }
            catch (Exception e)
            {
                Utils.Trace(e, "Unexpected exception starting write operation.");
            }
        }

        //-----------------------------------------------------------------------------------------

        /// <summary>
        /// Called when an asynchronous write operation completes.
        /// </summary>
        public static void OnWriteComplete(IAsyncResult result)
        {
            try
            {
                WriteState state = (WriteState)result.AsyncState;

                // complete write operation.
                StatusCodeCollection results;
                DiagnosticInfoCollection diagnosticInfos;

                ResponseHeader responseHeader = state.Session.EndWrite(
                    result,
                    out results,
                    out diagnosticInfos);

                // ensure that the server returned valid results.
                Session.ValidateResponse(results, state.NodesToWrite);
                Session.ValidateDiagnosticInfos(diagnosticInfos, state.NodesToWrite);

                Log2.Trace("Wrote {0} values.", state.NodesToWrite.Count);

                // verify return status.
                for (int ii = 0; ii < results.Count; ii++)
                {
                    lock (m_lock)
                    {
                        NodeOfInterest node = (NodeOfInterest)state.NodesToWrite[ii].Handle;

                        // check if an error occurred.
                        if (StatusCode.IsBad(results[ii]))
                        {
                            ServiceResult error = new ServiceResult(
                                results[ii],
                                diagnosticInfos[ii],
                                responseHeader.StringTable);

                            Log2.Trace("Write Failed for {0}: Error={1}", node.DisplayName, error);
                        }
                    }
                }

                // wait a little.
                Thread.Sleep(500);

                // send another write if still connected.
                if (state.Session.Connected)
                {
                    NextWrite(state);
                }
            }
            catch (Exception e)
            {
                Utils.Trace(e, "Unexpected exception during write operation.");
            }
        }


        //-----------------------------------------------------------------------------------------

        /// <summary>
        /// Returns a random value for the node.
        /// </summary>
        private static object GetRandomValue(WriteState state, NodeOfInterest node)
        {
            BuiltInType builtInType = DataTypes.GetBuiltInType(node.DataType, state.Session.TypeTree);

            if (node.ValueRank < 0)
            {
                switch (builtInType)
                {
                    case BuiltInType.Int32:
                        {
                            return state.Generator.GetRandom<int>(false);
                        }

                    case BuiltInType.Float:
                        {
                            return state.Generator.GetRandom<float>(false);
                        }
                    case BuiltInType.Double:
                        {
                            return state.Generator.GetRandom<double>(false);
                        }
                }
            }
            else
            {
                switch (builtInType)
                {
                    case BuiltInType.Int32:
                        {
                            return state.Generator.GetRandomArray<int>(false, 10, false);
                        }

                    case BuiltInType.Float:
                        {
                            return state.Generator.GetRandomArray<float>(false, 10, false);
                        }
                    case BuiltInType.Double:
                        {
                            return state.Generator.GetRandomArray<double>(false, 10, false);
                        }
                }
            }

            return node.Value;
        }


        //-----------------------------------------------------------------------------------------
    }
}// End Namespace
