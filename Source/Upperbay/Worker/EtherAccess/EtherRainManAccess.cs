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
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Nethereum.ABI;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.BlockchainProcessing.BlockStorage.Entities;
using Nethereum.Contracts;
using Nethereum.Hex.HexConvertors;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.JsonRpc.WebSocketClient;
using Nethereum.KeyStore;
using Nethereum.Model;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Eth.Transactions;
using Nethereum.RPC.NonceServices;
using Nethereum.RPC.TransactionReceipts;
using Nethereum.Signer;
using Nethereum.Util;
using Nethereum.Web3;


using Nethereum.ABI.Encoders;
using Nethereum.ABI.FunctionEncoding;


//////
using Nethereum.Web3.Accounts;
using Nethereum.Web3.Accounts.Managed;
using Upperbay.Agent.Interfaces;
using Upperbay.Core.Library;
//////




//for UA

// Assemblies needed for Agentness
using Upperbay.Core.Logging;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Account = Nethereum.Web3.Accounts.Account;
using System.Web.UI.WebControls;
using System.Net.WebSockets;

namespace Upperbay.Worker.EtherAccess
{

    //event SoilEvent(
    //    uint indexed slice,
    //    string soilDateTime,
    //    string soilZipcode,
    //    string soilCommunity,
    //    string soilLotNumber,
    //    string soilDeviceName,
    //    string soilTemp,
    //    string soilHumidity

    [Event("SoilEvent")]
    public class SoilEventArgs : IEventDTO
    {
        [Parameter("uint", "slice", 1, true)]
        public uint Slice { get; set; }

        [Parameter("string", "soilDateTime", 2, false)]
        public string SoilDateTime { get; set; }

        [Parameter("string", "soilZipcode", 3, false)]
        public string SoilZipcode { get; set; }

        [Parameter("string", "soilCommunity", 4, false)]
        public string SoilCommunity { get; set; }

        [Parameter("string", "soilLotNumber", 5, false)]
        public string SoilLotNumber { get; set; }

        [Parameter("string", "soilDeviceName", 6, false)]
        public string SoilDeviceName { get; set; }

        [Parameter("string", "soilTemperature", 7, false)]
        public string SoilTemperature { get; set; }

        [Parameter("string", "soilHumidity", 8, false)]
        public string SoilHumidity { get; set; }

    };




    public class EtherRainManAccess
    {

        private int FROM_BLOCK = 100000;


        /// <summary>
        /// 
        /// </summary>
        public EtherRainManAccess()
        {
            _ethereumOracleNameHash = ComputeSha256Hash(_ethereumOracleName);
        }
                
       /// <summary>
       /// 
       /// </summary>
       /// <param name="name"></param>
       /// <param name="url"></param>
       /// <param name="contractAddress"></param>
       /// <param name="externalAddress"></param>
       /// <param name="pkey"></param>
       /// <param name="chainId"></param>
       /// <returns></returns>
        public bool ConfigureAccess(string name, 
                                    string url, 
                                    string contractAddress, 
                                    string externalAddress, 
                                    string pkey,
                                    int chainId)
        {
            // call a method and get event logs
            _ethereumOracleName = name;
            _ethereumNodeURL = url;
            _ethereumContractAddress = contractAddress;
            _ethereumExternalAddress = externalAddress;
            _ethereumExternalPrivateKey = pkey;
            _ethereumChainId = chainId;

            //Make sure args got here correctly
            Log2.Debug("ConfigureAccess Contract URL: " + _ethereumNodeURL);
            Log2.Debug("ConfigureAccess Contract address: " + _ethereumContractAddress);
            Log2.Debug("ConfigureAccess External address: " + _ethereumExternalAddress);
            Log2.Debug("ConfigureAccess External pkey: " + _ethereumExternalPrivateKey);
            Log2.Debug("ConfigureAccess Chain ID: " + _ethereumChainId.ToString());
            //Log2.Trace("Oracle Name Hash: " + _ethereumOracleNameHash);

            _isConfiguredFlag = true;

            return (true);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task AddSoilRecord(
                                    string  soilDateTime,
                                    string  soilZipcode,
                                    string  soilCommunity,
                                    string  soilLot,
                                    string  soilDeviceName,
                                    string  soilTemp,
                                    string  soilHumidity
                                    )
        {

            if (!_isConfiguredFlag)
                return;

            //Convert to Nethereum var types
            var contractAddress = _ethereumContractAddress;
            var externalAddress = _ethereumExternalAddress;
            var privateKey = _ethereumExternalPrivateKey;
            var chainId = _ethereumChainId;

            Web3 web3;

            BigInteger priorityWei = 1_000_000_000;        // 1 μgwei = 1,000,000,000,000 wei
                                                               // (well above the 1-wei floor)
            BigInteger maxFeeWei = 2_000_000_000;

            try
            {
                var account = new Account(privateKey, chainId);

                if (_ethereumNodeURL.StartsWith("wss"))
                {
                    var clientws = new WebSocketClient(_ethereumNodeURL);
                    web3 = new Web3(account, clientws);
                }
                else
                {
                    web3 = new Web3(account, _ethereumNodeURL);
                }


                web3.TransactionManager.UseLegacyAsDefault = true;
                var contract = web3.Eth.GetContract(_contractABI, contractAddress);
                var addSoilRecordFunction = contract.GetFunction("AddSoilRecord");


                Log2.Debug("Begin AddSoilRecord: {0} {1} {2} {3} {4} {5} {6}",
                                        soilDateTime,
                                        soilZipcode,
                                        soilCommunity,
                                        soilLot, 
                                        soilDeviceName,
                                        soilTemp,
                                        soilHumidity);

                HexBigInteger estimate;
                try 
                { 
                    estimate = addSoilRecordFunction.EstimateGasAsync(
                                        from: externalAddress,
                                        gas: null,
                                        value: null,
                                        functionInput: new object[] {
                                            soilDateTime,
                                            soilZipcode,
                                            soilCommunity,
                                            soilLot,
                                            soilDeviceName,
                                            soilTemp,
                                            soilHumidity }).ConfigureAwait(false).GetAwaiter().GetResult();
                }
                catch (Nethereum.JsonRpc.Client.RpcResponseException ex)
                {
                    Log2.Error("EstimateGasAsync RpcResponseException: ", ex.ToString());
                    Log2.Error("Soil data NOT added");
                    return;
                }

                Log2.Debug("Est Gas = {0}", estimate.ToString());

                var est = estimate; // ≈ 273,721 from your log

                // 2) Optionally pad a little (10–30%)
                var padded = new HexBigInteger(est.Value * 12 / 10); // ~+20%

                // 3) Clamp to below the latest block gas limit (with a safety margin)
                var latest = web3.Eth.Blocks
                    .GetBlockWithTransactionsByNumber
                    .SendRequestAsync(BlockParameter.CreateLatest()).ConfigureAwait(false).GetAwaiter().GetResult();

                var blockLimit = latest.GasLimit.Value;               // e.g., 8,000,000
                var safeMax = blockLimit - 100_000;                // margin

                if (padded.Value > safeMax)
                    padded = new HexBigInteger(safeMax);


                Log2.Debug("Calling addSoilEventFunction.SendTransactionAsync: Est Gas = {0}", padded.ToString());
                //gas = new HexBigInteger(new BigInteger(10000000));

                try 
                { 
                    var transactionHash = addSoilRecordFunction.SendTransactionAsync(
                                                                    from: externalAddress,
                                                                    gas: padded,
                                                                    value: null,
                                                                    maxFeePerGas: new HexBigInteger(maxFeeWei),
                                                                    maxPriorityFeePerGas: new HexBigInteger(priorityWei),
                                                                    functionInput: new object[] {
                                                                        soilDateTime,
                                                                        soilZipcode,
                                                                        soilCommunity,
                                                                        soilLot,
                                                                        soilDeviceName,
                                                                        soilTemp,
                                                                        soilHumidity }).ConfigureAwait(false).GetAwaiter().GetResult();
                }
                catch (Nethereum.JsonRpc.Client.RpcResponseException ex)
                {
                    //Log2.Error("SendTransactionAsync RpcResponseException: ", ex.ToString());
                    Log2.Error($"RPC error {ex.RpcError?.Code}: {ex.RpcError?.Message} | data={ex.RpcError?.Data}");
                    Log2.Error("Soil data NOT added");
                    return;
                }

            Log2.Debug("End addSoilEventFunction.SendTransactionAsync Returned");
                
            }
            catch (SmartContractRevertException ex)
            {
                Log2.Error($"Revert: {ex.RevertMessage ?? ex.Message}");
                // optional: inspect ex.RevertMessageEncoded
            }
            catch (RpcResponseException ex)
            {
                Log2.Error($"RPC error {ex.RpcError?.Code}: {ex.RpcError?.Message} | data={ex.RpcError?.Data}");
                // You can branch on common messages here (nonce, underpriced, funds, etc.)
            }
            catch (HttpRequestException ex)
            {
                Log2.Error("HTTP transport error: " + ex.Message);
            }
            catch (WebSocketException ex)
            {
                Log2.Error("WebSocket transport error: " + ex.Message);
            }
            catch (OperationCanceledException)
            {
                Log2.Error("Operation timed out or was canceled");
            }
            catch (FormatException ex)
            {
                Log2.Error("Bad hex/bytes or ABI data format: " + ex.Message);
            }
            catch (OverflowException ex)
            {
                Log2.Error("Numeric overflow vs Solidity type: " + ex.Message);
            }
            catch (ArgumentException ex)
            {
                Log2.Error("Argument/ABI mismatch: " + ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                Log2.Error("Invalid operation/state: " + ex.Message);
            }
            catch (Exception ex)
            {
                Log2.Error("AddSoilRecord Exception: " + ex.ToString());
            }
            Log2.Debug("Finished AddSoilRecord: {0} {1} {2} {3} {4} {5} {6}",
                                                                    soilDateTime,
                                                                    soilZipcode,
                                                                    soilCommunity,
                                                                    soilLot,
                                                                    soilDeviceName,
                                                                    soilTemp,
                                                                    soilHumidity);
        }


      

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task LogSoilEvents(uint sliceNumber)
        {

            if (!_isConfiguredFlag)
                return;

            //Convert to Nethereum var types
            var contractAddress = _ethereumContractAddress;
            var externalAddress = _ethereumExternalAddress;
            var privateKey = _ethereumExternalPrivateKey;
            var chainId = _ethereumChainId;

            int eventCount = 0;

            try
            {
                var account = new Account(privateKey, chainId);
                Web3 web3;
                
                TimeSpan fiveMinutes = TimeSpan.FromMinutes(5);
                TimeSpan tenMinutes = TimeSpan.FromMinutes(10);

                if (_ethereumNodeURL.StartsWith("wss"))
                {
                    WebSocketClient.ConnectionTimeout = fiveMinutes;
                    WebSocketClient.ForceCompleteReadTotalMilliseconds = (int)fiveMinutes.TotalMilliseconds;
                    var clientws = new WebSocketClient(_ethereumNodeURL);
                    web3 = new Web3(account, clientws);
                }
                else
                {
                    ClientBase.ConnectionTimeout = fiveMinutes;
                    var httpHandler = new HttpClientHandler
                    {
                        // optional: tweak TLS / proxy / pooling settings here
                    };

                    var http = new HttpClient(httpHandler)
                    {
                        Timeout = tenMinutes
                    };

                    // ---------- wrap it in an RpcClient ----------
                    var rpcHttp = new RpcClient(new Uri(_ethereumNodeURL), http);

                    web3 = new Web3(account, rpcHttp);
                }
           

                var contract = web3.Eth.GetContract(_contractABI, contractAddress);

                Event soilEvent = contract.GetEvent("SoilEvent");
                Log2.Info("LogSoilEvents for Slice {0}", sliceNumber);

                /*
                //Log2.Trace("Creating filter for all GameEventEvent events");
                var filterAll = await gameEventEvent.CreateFilterAsync(sliceNumber).ConfigureAwait(false);
                Log2.Debug("LogAllEvents-> Created FilterAll: {0} for Slice {1}", filterAll.Value, sliceNumber);
                //Log2.Debug("-> Created FilterAll: {0}", filterAll.Value);
                //Log2.Debug("Get all GameEventEvent events the hard way");
                var log = await gameEventEvent.GetAllChangesAsync<GameEventEventArgs>(filterAll).ConfigureAwait(false);
                */

                //var latestBlock = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
                //var eventAbi = web3.Eth.GetEvent<SoilEventArgs>();

                //var encoder = new IntType("uint256");
                //var encodedSlice = "0x" + encoder.Encode(sliceNumber).ToHex();

                //var filterInput = new NewFilterInput
                //{
                //    FromBlock = new BlockParameter(new HexBigInteger(latestBlock.Value - FROM_BLOCK)),
                //    ToBlock = new BlockParameter(latestBlock),
                //    Address = new[] { contractAddress },
                //    Topics = new object[]
                //    {
                //        eventAbi.EventABI.Sha3Signature, // topic[0] = event signature
                //        encodedSlice                      // topic[1] = indexed sliceNumber
                //    }
                //};
                var playerfilterInput = soilEvent.CreateFilterInput(sliceNumber);  // or build manually

                var logs = await web3.Eth.Filters.GetLogs
                                    .SendRequestAsync(playerfilterInput)
                                    .ConfigureAwait(false);

                var decoder = web3.Eth.GetEvent<SoilEventArgs>();
                var decoded = decoder.DecodeAllEventsForEvent(logs); // returns List<EventLog<GamePlayerEventArgs>>

                // Extract just the event data
                var events = decoded
                    .Where(e => e != null)
                    .Select(e => e.Event)
                    .ToList();

                eventCount = logs.Count();

                Log2.Debug("-> Got SoilEvents {0} for Slice {1}", eventCount.ToString(), sliceNumber);

                //foreach (var evt in log)
                //{
                //    Log2.Debug("-> All GameEvents {0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}",
                
                //}
                //[Parameter("string", "soilDateTime", 2, false)]
                //public string SoilDateTime { get; set; }

                //[Parameter("string", "soilZipcode", 3, false)]
                //public string SoilZipcode { get; set; }

                //[Parameter("string", "soilCommunity", 4, false)]
                //public string SoilCommunity { get; set; }

                //[Parameter("string", "soilLotNumber", 5, false)]
                //public string SoilLotNumber { get; set; }

                //[Parameter("string", "soilDeviceName", 6, false)]
                //public string SoilDeviceName { get; set; }

                //[Parameter("string", "soilTemperature", 7, false)]
                //public string SoilTemperature { get; set; }

                //[Parameter("string", "soilHumidity", 8, false)]
                //public string SoilHumidity { get; set; }

                string filePath = "data\\CloudSoilMoisture" + sliceNumber + ".csv";
                string header =
                    "\"" + "SoilDateTime"  + "\",\"" + "SoilZipcode" + "\",\"" + "SoilCommunity" + "\",\"" + "SoilLotNumber" + "\",\"" + "SoilDeviceName" + "\",\"" + "SoilTemperature" + "\",\"" + "SoilHumidity" + "\"";
                if (eventCount > 0)
                {
                    using (StreamWriter outputFile = new StreamWriter(filePath, false))
                    {
                        outputFile.WriteLine(header);
                        foreach (var evt in events)
                        {
                            string gameEvents =
                                "\"" + evt.SoilDateTime  + "\",\"" + evt.SoilZipcode + "\",\"" + evt.SoilCommunity + "\",\"" + evt.SoilLotNumber + "\",\"" + evt.SoilDeviceName + "\",\"" + evt.SoilTemperature + "\",\"" + evt.SoilHumidity  + "\"";
                            outputFile.WriteLine(gameEvents);
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                Log2.Error("LogAllEvents Exception: " + ex.ToString());
            }
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetSoilCount()
        {
            if (!_isConfiguredFlag)
                return -1;

            //Convert to Nethereum var types
            var contractAddress = _ethereumContractAddress;
            var externalAddress = _ethereumExternalAddress;
            var privateKey = _ethereumExternalPrivateKey;
            var chainId = _ethereumChainId;
            Web3 web3;


            try
            {
                var account = new Account(privateKey, chainId);

                if (_ethereumNodeURL.StartsWith("wss"))
                {
                    var clientws = new WebSocketClient(_ethereumNodeURL);
                    web3 = new Web3(account, clientws);
                }
                else
                {
                    web3 = new Web3(account, _ethereumNodeURL);
                }

                var contract = web3.Eth.GetContract(_contractABI, contractAddress);

                // This matches your public uint variable
                var myUintFunction = contract.GetFunction("SoilCount");

                // CallAsync<T> with type uint (or BigInteger for large values)
                var value = myUintFunction.CallAsync<uint>().ConfigureAwait(false).GetAwaiter().GetResult(); ;

                Log2.Debug($"SoilCount = {value}");

                return ((int)value);

            }
            catch (Exception ex)
            {
                Log2.Error("GetSoilCount Exception: " + ex.ToString());
            }
            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetSliceCount()
        {
            if (!_isConfiguredFlag)
                return -1;

            //Convert to Nethereum var types
            var contractAddress = _ethereumContractAddress;
            var externalAddress = _ethereumExternalAddress;
            var privateKey = _ethereumExternalPrivateKey;
            var chainId = _ethereumChainId;
            Web3 web3;


            try
            {
                var account = new Account(privateKey, chainId);

                if (_ethereumNodeURL.StartsWith("wss"))
                {
                    var clientws = new WebSocketClient(_ethereumNodeURL);
                    web3 = new Web3(account, clientws);
                }
                else
                {
                    web3 = new Web3(account, _ethereumNodeURL);
                }

                var contract = web3.Eth.GetContract(_contractABI, contractAddress);

                // This matches your public uint variable
                var myUintFunction = contract.GetFunction("SoilCount");

                // CallAsync<T> with type uint (or BigInteger for large values)
                var value = myUintFunction.CallAsync<uint>().ConfigureAwait(false).GetAwaiter().GetResult(); ;

                int sliceCount = ((int)value / 9999) + 1;

                Log2.Debug($"SliceCount = {sliceCount}");

                return (sliceCount);

            }
            catch (Exception ex)
            {
                Log2.Error("GetSliceCount Exception: " + ex.ToString());
            }
            return -1;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ba"></param>
        /// <returns></returns>
        private static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        //Properties
        private string _ethereumOracleName = "Upperbay Systems CurrentForCarbon";
        private string _ethereumOracleNameHash = null;
        private string _ethereumNodeURL = null;
        private string _ethereumContractAddress = null;
        private string _ethereumExternalAddress = null;
        private string _ethereumExternalPrivateKey = null;
        private int _ethereumChainId = 3;
        private bool _isConfiguredFlag = false;

        // V1
        //private string _contractABI = @"[{'inputs':[],'stateMutability':'nonpayable','type':'constructor'},{'anonymous':false,'inputs':[{'indexed':true,'internalType':'uint256','name':'slice','type':'uint256'},{'indexed':true,'internalType':'bytes32','name':'gamePlayerIDIdx','type':'bytes32'},{'indexed':false,'internalType':'string','name':'gamePlayerID','type':'string'},{'indexed':false,'internalType':'string','name':'gameEventID','type':'string'},{'indexed':false,'internalType':'string','name':'gameEventStartTime','type':'string'},{'indexed':false,'internalType':'string','name':'gameEventDuration','type':'string'},{'indexed':false,'internalType':'string','name':'pointsPerWatt','type':'string'},{'indexed':false,'internalType':'string','name':'averagePowerInWatts','type':'string'},{'indexed':false,'internalType':'string','name':'baselineAveragePowerInWatts','type':'string'},{'indexed':false,'internalType':'string','name':'deltaAveragePowerInWatts','type':'string'},{'indexed':false,'internalType':'string','name':'wattPoints','type':'string'},{'indexed':false,'internalType':'string','name':'awardValue','type':'string'}],'name':'GameCombinedEventResultEvent','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'internalType':'uint256','name':'slice','type':'uint256'},{'indexed':false,'internalType':'string','name':'gameEventID','type':'string'},{'indexed':false,'internalType':'string','name':'gameEventName','type':'string'},{'indexed':false,'internalType':'string','name':'gameEventType','type':'string'},{'indexed':false,'internalType':'string','name':'gameEventStartTime','type':'string'},{'indexed':false,'internalType':'string','name':'gameEventEndTime','type':'string'},{'indexed':false,'internalType':'string','name':'gameEventDuration','type':'string'},{'indexed':false,'internalType':'string','name':'dollarPerPoint','type':'string'},{'indexed':false,'internalType':'string','name':'pointsPerWatt','type':'string'},{'indexed':false,'internalType':'string','name':'pointsPerPercent','type':'string'}],'name':'GameEventEvent','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'internalType':'uint256','name':'slice','type':'uint256'},{'indexed':false,'internalType':'string','name':'gamePlayerID','type':'string'},{'indexed':false,'internalType':'string','name':'dataConnectionString','type':'string'},{'indexed':false,'internalType':'address','name':'gamePlayerAddress','type':'address'},{'indexed':false,'internalType':'string','name':'status','type':'string'}],'name':'GamePlayerEvent','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'internalType':'uint256','name':'slice','type':'uint256'},{'indexed':true,'internalType':'bytes32','name':'gamePlayerIDIdx','type':'bytes32'},{'indexed':false,'internalType':'string','name':'gameEventStartTime','type':'string'},{'indexed':false,'internalType':'string','name':'gamePlayerID','type':'string'},{'indexed':false,'internalType':'string','name':'gameEventID','type':'string'},{'indexed':false,'internalType':'string','name':'averagePowerInWatts','type':'string'},{'indexed':false,'internalType':'string','name':'baselineAveragePowerInWatts','type':'string'},{'indexed':false,'internalType':'string','name':'deltaAveragePowerInWatts','type':'string'},{'indexed':false,'internalType':'string','name':'percentPoints','type':'string'},{'indexed':false,'internalType':'string','name':'wattPoints','type':'string'},{'indexed':false,'internalType':'string','name':'totalPointsAwarded','type':'string'},{'indexed':false,'internalType':'string','name':'awardValue','type':'string'}],'name':'GameResultEvent','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'internalType':'address','name':'previousOwner','type':'address'},{'indexed':true,'internalType':'address','name':'newOwner','type':'address'}],'name':'OwnershipTransferred','type':'event'},{'inputs':[{'internalType':'string','name':'gameEventID','type':'string'},{'internalType':'string','name':'gameEventName','type':'string'},{'internalType':'string','name':'gameEventType','type':'string'},{'internalType':'string','name':'gameEventStartTime','type':'string'},{'internalType':'string','name':'gameEventEndTime','type':'string'},{'internalType':'string','name':'gameEventDuration','type':'string'},{'internalType':'string','name':'dollarPerPoint','type':'string'},{'internalType':'string','name':'pointsPerWatt','type':'string'},{'internalType':'string','name':'pointsPerPercent','type':'string'}],'name':'AddGameEvent','outputs':[],'stateMutability':'nonpayable','type':'function'},{'inputs':[{'internalType':'string','name':'gamePlayerID','type':'string'},{'internalType':'address','name':'playerAddress','type':'address'},{'internalType':'string','name':'dataConnectionString','type':'string'}],'name':'AddGamePlayer','outputs':[],'stateMutability':'nonpayable','type':'function'},{'inputs':[{'internalType':'string','name':'gamePlayerID','type':'string'},{'internalType':'string','name':'gameEventID','type':'string'},{'internalType':'address','name':'gamePlayerAddress','type':'address'},{'internalType':'string','name':'averagePowerInWatts','type':'string'},{'internalType':'string','name':'baselineAveragePowerInWatts','type':'string'},{'internalType':'string','name':'deltaAveragePowerInWatts','type':'string'},{'internalType':'string','name':'percentPoints','type':'string'},{'internalType':'string','name':'wattPoints','type':'string'},{'internalType':'string','name':'totalPointsAwarded','type':'string'},{'internalType':'string','name':'awardValue','type':'string'}],'name':'AddGameResult','outputs':[],'stateMutability':'nonpayable','type':'function'},{'inputs':[],'name':'EventCount','outputs':[{'internalType':'uint256','name':'','type':'uint256'}],'stateMutability':'view','type':'function'},{'inputs':[{'internalType':'bytes32','name':'_gameEventIDHash','type':'bytes32'}],'name':'GameEventExists','outputs':[{'internalType':'bool','name':'','type':'bool'}],'stateMutability':'view','type':'function'},{'inputs':[{'internalType':'bytes32','name':'_gamePlayerIDHash','type':'bytes32'}],'name':'GamePlayerExists','outputs':[{'internalType':'bool','name':'','type':'bool'}],'stateMutability':'view','type':'function'},{'inputs':[{'internalType':'bytes32','name':'_gameResultIDHash','type':'bytes32'}],'name':'GameResultExists','outputs':[{'internalType':'bool','name':'','type':'bool'}],'stateMutability':'view','type':'function'},{'inputs':[],'name':'HasResultReadyFlag','outputs':[{'internalType':'bool','name':'','type':'bool'}],'stateMutability':'view','type':'function'},{'inputs':[],'name':'PlayerCount','outputs':[{'internalType':'uint256','name':'','type':'uint256'}],'stateMutability':'view','type':'function'},{'inputs':[],'name':'ResultCount','outputs':[{'internalType':'uint256','name':'','type':'uint256'}],'stateMutability':'view','type':'function'},{'inputs':[{'internalType':'string','name':'source','type':'string'}],'name':'StringToBytes32','outputs':[{'internalType':'bytes32','name':'result','type':'bytes32'}],'stateMutability':'pure','type':'function'},{'inputs':[{'internalType':'string','name':'playerID','type':'string'},{'internalType':'string','name':'status','type':'string'}],'name':'UpdatePlayerStatus','outputs':[],'stateMutability':'nonpayable','type':'function'},{'inputs':[{'internalType':'string','name':'resultID','type':'string'},{'internalType':'string','name':'status','type':'string'}],'name':'UpdateResultsStatus','outputs':[],'stateMutability':'nonpayable','type':'function'},{'inputs':[],'name':'_contractName','outputs':[{'internalType':'string','name':'','type':'string'}],'stateMutability':'view','type':'function'},{'inputs':[],'name':'addTestData','outputs':[],'stateMutability':'nonpayable','type':'function'},{'inputs':[],'name':'getAddress','outputs':[{'internalType':'address','name':'','type':'address'}],'stateMutability':'view','type':'function'},{'inputs':[],'name':'kill','outputs':[],'stateMutability':'nonpayable','type':'function'},{'inputs':[],'name':'owner','outputs':[{'internalType':'address','name':'','type':'address'}],'stateMutability':'view','type':'function'},{'inputs':[],'name':'testConnection','outputs':[{'internalType':'bool','name':'','type':'bool'}],'stateMutability':'pure','type':'function'},{'inputs':[{'internalType':'address','name':'newOwner','type':'address'}],'name':'transferOwnership','outputs':[],'stateMutability':'nonpayable','type':'function'}]";
        //private string _contractABI = @"[]";
        // V2
        private string _contractABI = @"[
      {
      ""inputs"": [],
      ""stateMutability"": ""nonpayable"",
      ""type"": ""constructor""
    },
    {
      ""anonymous"": false,
      ""inputs"": [
        {
          ""indexed"": true,
          ""internalType"": ""address"",
          ""name"": ""previousOwner"",
          ""type"": ""address""
        },
        {
          ""indexed"": true,
          ""internalType"": ""address"",
          ""name"": ""newOwner"",
          ""type"": ""address""
        }
      ],
      ""name"": ""OwnershipTransferred"",
      ""type"": ""event""
    },
    {
      ""anonymous"": false,
      ""inputs"": [
        {
          ""indexed"": true,
          ""internalType"": ""uint256"",
          ""name"": ""slice"",
          ""type"": ""uint256""
        },
        {
          ""indexed"": false,
          ""internalType"": ""string"",
          ""name"": ""soilDateTime"",
          ""type"": ""string""
        },
        {
          ""indexed"": false,
          ""internalType"": ""string"",
          ""name"": ""soilZipcode"",
          ""type"": ""string""
        },
        {
          ""indexed"": false,
          ""internalType"": ""string"",
          ""name"": ""soilCommunity"",
          ""type"": ""string""
        },
        {
          ""indexed"": false,
          ""internalType"": ""string"",
          ""name"": ""soilLotNumber"",
          ""type"": ""string""
        },
        {
          ""indexed"": false,
          ""internalType"": ""string"",
          ""name"": ""soilDeviceName"",
          ""type"": ""string""
        },
        {
          ""indexed"": false,
          ""internalType"": ""string"",
          ""name"": ""soilTemperature"",
          ""type"": ""string""
        },
        {
          ""indexed"": false,
          ""internalType"": ""string"",
          ""name"": ""soilHumidity"",
          ""type"": ""string""
        }
      ],
      ""name"": ""SoilEvent"",
      ""type"": ""event""
    },
    {
      ""inputs"": [
        {
          ""internalType"": ""string"",
          ""name"": ""soilDateTime"",
          ""type"": ""string""
        },
        {
          ""internalType"": ""string"",
          ""name"": ""soilZipcode"",
          ""type"": ""string""
        },
        {
          ""internalType"": ""string"",
          ""name"": ""soilCommunity"",
          ""type"": ""string""
        },
        {
          ""internalType"": ""string"",
          ""name"": ""soilLotNumber"",
          ""type"": ""string""
        },
        {
          ""internalType"": ""string"",
          ""name"": ""soilDeviceName"",
          ""type"": ""string""
        },
        {
          ""internalType"": ""string"",
          ""name"": ""soilTemperature"",
          ""type"": ""string""
        },
        {
          ""internalType"": ""string"",
          ""name"": ""soilHumidity"",
          ""type"": ""string""
        }
      ],
      ""name"": ""AddSoilRecord"",
      ""outputs"": [],
      ""stateMutability"": ""nonpayable"",
      ""type"": ""function""
    },
    {
      ""inputs"": [],
      ""name"": ""RunSwitch"",
      ""outputs"": [
        {
          ""internalType"": ""bool"",
          ""name"": """",
          ""type"": ""bool""
        }
      ],
      ""stateMutability"": ""view"",
      ""type"": ""function""
    },
    {
      ""inputs"": [],
      ""name"": ""SoilCount"",
      ""outputs"": [
        {
          ""internalType"": ""uint256"",
          ""name"": """",
          ""type"": ""uint256""
        }
      ],
      ""stateMutability"": ""view"",
      ""type"": ""function""
    },
    {
      ""inputs"": [],
      ""name"": ""_contractName"",
      ""outputs"": [
        {
          ""internalType"": ""string"",
          ""name"": """",
          ""type"": ""string""
        }
      ],
      ""stateMutability"": ""view"",
      ""type"": ""function""
    },
    {
      ""inputs"": [],
      ""name"": ""getAddress"",
      ""outputs"": [
        {
          ""internalType"": ""address"",
          ""name"": """",
          ""type"": ""address""
        }
      ],
      ""stateMutability"": ""view"",
      ""type"": ""function""
    },
    {
      ""inputs"": [],
      ""name"": ""owner"",
      ""outputs"": [
        {
          ""internalType"": ""address"",
          ""name"": """",
          ""type"": ""address""
        }
      ],
      ""stateMutability"": ""view"",
      ""type"": ""function""
    },
    {
      ""inputs"": [],
      ""name"": ""testConnection"",
      ""outputs"": [
        {
          ""internalType"": ""bool"",
          ""name"": """",
          ""type"": ""bool""
        }
      ],
      ""stateMutability"": ""pure"",
      ""type"": ""function""
    },
    {
      ""inputs"": [
        {
          ""internalType"": ""address"",
          ""name"": ""newOwner"",
          ""type"": ""address""
        }
      ],
      ""name"": ""transferOwnership"",
      ""outputs"": [],
      ""stateMutability"": ""nonpayable"",
      ""type"": ""function""
    }
    ]";


    }
}// End Namespace
