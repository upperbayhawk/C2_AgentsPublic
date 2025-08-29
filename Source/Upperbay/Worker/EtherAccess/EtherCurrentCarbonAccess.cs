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
using System.Net.WebSockets;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Xml.Linq;
using Nethereum.ABI;
using Nethereum.ABI.Encoders;
using Nethereum.ABI.FunctionEncoding;
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

namespace Upperbay.Worker.EtherAccess
{
      
    [Event("GamePlayerEvent")]
    public class GamePlayerEventArgs : IEventDTO
    {
        [Parameter("uint", "slice", 1, true)]
        public uint Slice { get; set; }

        [Parameter("string", "gamePlayerID", 2, false)]
        public string GamePlayerID { get; set; }

        [Parameter("string", "dataConnectionString", 3, false)]
        public string DataConnectionString { get; set; }

        [Parameter("address", "gamePlayerAddress", 4, false)]
        public string GamePlayerAddress { get; set; }

        [Parameter("string", "status", 5, false)]
        public string Status { get; set; }
    };

    [Event("GameEventEvent")]
    public class GameEventEventArgs : IEventDTO
    {
        [Parameter("uint", "slice", 1, true)]
        public uint Slice { get; set; }

        [Parameter("string", "gameEventID", 2, false)]
        public string GameEventID { get; set; }

        [Parameter("string", "gameEventName", 3, false)]
        public string GameEventName { get; set; }

        [Parameter("string", "gameEventType", 4, false)]
        public string GameEventType { get; set; }

        [Parameter("string", "gameEventStartTime", 5, false)]
        public string GameEventStartTime { get; set; }

        [Parameter("string", "gameEventEndTime", 6, false)]
        public string GameEventEndTime { get; set; }

        [Parameter("string", "gameEventDuration", 7, false)]
        public string GameEventDuration { get; set; }

        [Parameter("string", "dollarPerPoint", 8, false)]
        public string DollarPerPoint { get; set; }

        [Parameter("string", "pointsPerWatt", 9, false)]
        public string PointsPerWatt { get; set; }

        [Parameter("string", "pointsPerPercent", 10, false)]
        public string PointsPerPercent { get; set; }
    };

    [Event("GameResultEvent")]
    public class GameResultEventArgs : IEventDTO
    {

        [Parameter("uint", "slice", 1, true)]
        public uint Slice { get; set; }

        [Parameter("bytes32", "gamePlayerIDIdx", 2, true)]
        public byte[] GamePlayerIDIdx { get; set; }

        [Parameter("string", "gamePlayerID", 3, false)]
        public string GamePlayerID { get; set; }

        [Parameter("string", "gameEventID", 4, false)]
        public string GameEventID { get; set; }

        [Parameter("string", "gameEventStartTime", 5, false)]
        public string GameEventStartTime { get; set; }

        [Parameter("string", "averagePowerInWatts", 6, false)]
        public string AveragePowerInWatts { get; set; }

        [Parameter("string", "baselineAveragePowerInWatts", 7, false)]
        public string BaselineAveragePowerInWatts { get; set; }

        [Parameter("string", "deltaAveragePowerInWatts", 8, false)]
        public string DeltaAveragePowerInWatts { get; set; }

        [Parameter("string", "percentPoints", 9, false)]
        public string PercentPoints { get; set; }

        [Parameter("string", "wattPoints", 10, false)]
        public string WattPoints { get; set; }

        [Parameter("string", "totalPointsAwarded", 11, false)]
        public string TotalPointsAwarded { get; set; }

        [Parameter("string", "awardValue", 12, false)]
        public string AwardValue { get; set; }

    };

    [Event("GameCombinedEventResultEvent")]
    public class GameCombinedEventResultEventArgs : IEventDTO
    {
        [Parameter("uint", "slice", 1, true)]
        public uint Slice { get; set; }

        [Parameter("bytes32", "gamePlayerIDIdx", 2, true)]
        public byte[] GamePlayerIDIdx { get; set; }

        [Parameter("string", "gamePlayerID", 3, false)]
        public string GamePlayerID { get; set; }

        [Parameter("string", "gameEventID", 4, false)]
        public string GameEventID { get; set; }

        [Parameter("string", "gameEventStartTime", 5, false)]
        public string GameEventStartTime { get; set; }

        [Parameter("string", "gameEventDuration", 6, false)]
        public string GameEventDuration { get; set; }

        [Parameter("string", "pointsPerWatt", 7, false)]
        public string PointsPerWatt { get; set; }

        [Parameter("string", "averagePowerInWatts", 8, false)]
        public string AveragePowerInWatts { get; set; }

        [Parameter("string", "baselineAveragePowerInWatts", 9, false)]
        public string BaselineAveragePowerInWatts { get; set; }

        [Parameter("string", "deltaAveragePowerInWatts", 10, false)]
        public string DeltaAveragePowerInWatts { get; set; }

        [Parameter("string", "wattPoints", 11, false)]
        public string WattPoints { get; set; }
               
        [Parameter("string", "awardValue", 12, false)]
        public string AwardValue { get; set; }

    };



    public class EtherCurrentCarbonAccess
    {

        private int FROM_BLOCK = 100000;


        /// <summary>
        /// 
        /// </summary>
        public EtherCurrentCarbonAccess()
        {
            _ethereumOracleNameHash = ComputeSha256Hash(_ethereumOracleName);
        }


       
        //public async Task SendEtherToContract()
        //{

        //    if (!_isConfiguredFlag) 
        //        return;

        //    var privateKey = _ethereumExternalPrivateKey;
        //    var account = new Account(privateKey);
        //    var toAddress = _ethereumContractAddress;
        //    //var toAddress = "0x53eFDf11B302ed090D09690937E4Cd0EB9779E67";
        //    //var toAddress = _testAccount;
        //    var web3 = new Web3(account, _ethereumNodeURL);
        //    Log2.Trace("Sending Transfer ether");

        //    var balanceOriginal = await web3.Eth.GetBalance.SendRequestAsync(toAddress);
        //    var balanceOriginalEther = Web3.Convert.FromWei(balanceOriginal.Value);
        //    Log2.Trace("Original Balance = " + balanceOriginalEther.ToString());

        //    var transferService = web3.Eth.GetEtherTransferService();
        //    var estimate = await transferService.EstimateGasAsync(toAddress, 1.11m);
        //    Log2.Trace("Estimate = " + estimate.ToString());

        //    var receipt = await web3.Eth.GetEtherTransferService()
        //                    .TransferEtherAndWaitForReceiptAsync(toAddress, 1.11m, 2, estimate);
        //    Log2.Trace("Received Transfer ether");

        //    var balance = await web3.Eth.GetBalance.SendRequestAsync(toAddress);
        //    Log2.Trace($"Balance in Wei: " + balance.Value.ToString());

        //    var etherAmount = Web3.Convert.FromWei(balance.Value);
        //    Log2.Trace($"Balance in Ether: " + etherAmount.ToString());

        //}
                
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
        /// <param name="gamePlayerID"></param>
        /// <param name="dataConnectionString"></param>
        /// <returns></returns>
        public async Task AddPlayer(string gamePlayerID,
                                    string dataConnectionString)
        {
            
            if (!_isConfiguredFlag)
                return;

            //Convert to Nethereum var types
            var contractAddress = _ethereumContractAddress;
            var externalAddress = _ethereumExternalAddress;
            var privateKey = _ethereumExternalPrivateKey;
            var chainId = _ethereumChainId;
            Web3 web3;


            BigInteger priorityWei = 1_000_000;        // 1 μgwei = 1,000,000,000,000 wei
                                                               // (well above the 1-wei floor)
            BigInteger maxFeeWei = 2_000_000;

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

                var addGamePlayerFunction = contract.GetFunction("AddGamePlayer");

                Log2.Debug("AddPlayer: {0} {1} {2}", gamePlayerID,
                                                     externalAddress,
                                                     dataConnectionString);

                HexBigInteger estimate;
                try
                {

                    estimate = addGamePlayerFunction.EstimateGasAsync(externalAddress, null, null,
                                                                                gamePlayerID,
                                                                                externalAddress,
                                                                                dataConnectionString).ConfigureAwait(false).GetAwaiter().GetResult();

                }
                catch (SmartContractRevertException ex)
                {
                    Log2.Error($"Revert: {ex.RevertMessage ?? ex.Message}");
                    // optional: inspect ex.RevertMessageEncoded
                    Log2.Error("Player NOT added");
                    return;
                }
                catch (RpcResponseException ex)
                {
                    Log2.Error($"RPC error {ex.RpcError?.Code}: {ex.RpcError?.Message} | data={ex.RpcError?.Data}");
                    // You can branch on common messages here (nonce, underpriced, funds, etc.)
                    Log2.Error("Player NOT added");
                    return;
                }
                catch (HttpRequestException ex)
                {
                    Log2.Error("HTTP transport error: " + ex.Message);
                    Log2.Error("Player NOT added");
                    return;
                }
                catch (WebSocketException ex)
                {
                    Log2.Error("WebSocket transport error: " + ex.Message);
                    Log2.Error("Player NOT added");
                    return;
                }
                catch (OperationCanceledException)
                {
                    Log2.Error("Operation timed out or was canceled");
                    Log2.Error("Player NOT added");
                    return;
                }
                catch (FormatException ex)
                {
                    Log2.Error("Bad hex/bytes or ABI data format: " + ex.Message);
                    Log2.Error("Player NOT added");
                    return;
                }
                catch (OverflowException ex)
                {
                    Log2.Error("Numeric overflow vs Solidity type: " + ex.Message);
                    Log2.Error("Player NOT added");
                    return;
                }
                catch (ArgumentException ex)
                {
                    Log2.Error("Argument/ABI mismatch: " + ex.Message);
                    Log2.Error("Player NOT added");
                    return;
                }
                catch (InvalidOperationException ex)
                {
                    Log2.Error("Invalid operation/state: " + ex.Message);
                    Log2.Error("Player NOT added");
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

                //gas = new HexBigInteger(new BigInteger(10000000));
                Log2.Debug("Calling addGamePlayerFunction.SendTransactionAsync: Est. Gas {0}", padded.ToString());
                //gas = new HexBigInteger(new BigInteger(1000000));

                try
                {
                    var transactionHash = addGamePlayerFunction.SendTransactionAsync(
                                                                    from: externalAddress,
                                                                    gas: padded,
                                                                    value: null,
                                                                    maxFeePerGas: new HexBigInteger(maxFeeWei),
                                                                    maxPriorityFeePerGas: new HexBigInteger(priorityWei),
                                                                    functionInput: new object[] {
                                                                        gamePlayerID,
                                                                        externalAddress,
                                                                        dataConnectionString
                                                                    }).ConfigureAwait(false).GetAwaiter().GetResult();
                }
                catch (SmartContractRevertException ex)
                {
                    Log2.Error($"Revert: {ex.RevertMessage ?? ex.Message}");
                    // optional: inspect ex.RevertMessageEncoded
                    Log2.Error("Player NOT added");
                    return;
                }
                catch (RpcResponseException ex)
                {
                    Log2.Error($"RPC error {ex.RpcError?.Code}: {ex.RpcError?.Message} | data={ex.RpcError?.Data}");
                    // You can branch on common messages here (nonce, underpriced, funds, etc.)
                    Log2.Error("Player NOT added");
                    return;
                }
                catch (HttpRequestException ex)
                {
                    Log2.Error("HTTP transport error: " + ex.Message);
                    Log2.Error("Player NOT added");
                    return;
                }
                catch (WebSocketException ex)
                {
                    Log2.Error("WebSocket transport error: " + ex.Message);
                    Log2.Error("Player NOT added");
                    return;
                }
                catch (OperationCanceledException)
                {
                    Log2.Error("Operation timed out or was canceled");
                    Log2.Error("Player NOT added");
                    return;
                }
                catch (FormatException ex)
                {
                    Log2.Error("Bad hex/bytes or ABI data format: " + ex.Message);
                    Log2.Error("Player NOT added");
                    return;
                }
                catch (OverflowException ex)
                {
                    Log2.Error("Numeric overflow vs Solidity type: " + ex.Message);
                    Log2.Error("Player NOT added");
                    return;
                }
                catch (ArgumentException ex)
                {
                    Log2.Error("Argument/ABI mismatch: " + ex.Message);
                    Log2.Error("Player NOT added");
                    return;
                }
                catch (InvalidOperationException ex)
                {
                    Log2.Error("Invalid operation/state: " + ex.Message);
                    Log2.Error("Player NOT added");
                    return;
                }
                
                Log2.Trace("addGamePlayerFunction.SendTransactionAsync Returned");

 
            }
            catch (Exception ex)
            {
                Log2.Error("PlayerAdded Exception: " + ex.ToString());
                Log2.Error("Player NOT added");
                return;
            }
            Log2.Info("PlayerAdded:{0} {1} {2}", gamePlayerID,
                                                 externalAddress,
                                                 dataConnectionString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gpcList"></param>
        /// <returns></returns>
        public async Task AddPlayerFromConfidential(List<GamePlayerConfidential> gpcList)
        {

            if (!_isConfiguredFlag)
                return;

            //string _playerSignature = Utilities.ComputeSha256Hash(gpc.DataConnectString + gpc.EthereumExternalAddress + gpc.GamePlayerId);
            //Log2.Debug("AddPlayerFromConfidential:_playerSignature: {0}", _playerSignature);

            //Convert to Nethereum var types
            var contractAddress = _ethereumContractAddress;
            var externalAddress = _ethereumExternalAddress;
            var privateKey = _ethereumExternalPrivateKey;
            var chainId = _ethereumChainId;
            Web3 web3;

            BigInteger priorityWei = 1_000_000;        // 1 μgwei = 1,000,000,000,000 wei
                                                               // (well above the 1-wei floor)
            BigInteger maxFeeWei = 2_000_000;

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

                var addGamePlayerFunction = contract.GetFunction("AddGamePlayer");
                
                foreach (GamePlayerConfidential gpc in gpcList)
                {
                    try
                    {
                        Log2.Debug("AddPlayerFromConfidential:AddPlayer: {0} {1} {2}", gpc.GamePlayerId,
                                                            gpc.EthereumExternalAddress,
                                                            gpc.DataConnectString);
                        HexBigInteger estimate;
                        try 
                        { 
                            estimate = addGamePlayerFunction.EstimateGasAsync(externalAddress, null, null,
                                                                                            gpc.GamePlayerId,
                                                                                            gpc.EthereumExternalAddress,
                                                                                            //gpc.DataConnectString);
                                                                                            gpc.DataConnectString).ConfigureAwait(false).GetAwaiter().GetResult();
                        }
                        catch (SmartContractRevertException ex)
                        {
                            Log2.Error($"Revert: {ex.RevertMessage ?? ex.Message}");
                            // optional: inspect ex.RevertMessageEncoded
                            Log2.Error("Player NOT added");
                            return;
                        }
                        catch (RpcResponseException ex)
                        {
                            Log2.Error($"RPC error {ex.RpcError?.Code}: {ex.RpcError?.Message} | data={ex.RpcError?.Data}");
                            // You can branch on common messages here (nonce, underpriced, funds, etc.)
                            Log2.Error("Player NOT added");
                            return;
                        }
                        catch (HttpRequestException ex)
                        {
                            Log2.Error("HTTP transport error: " + ex.Message);
                            Log2.Error("Player NOT added");
                            return;
                        }
                        catch (WebSocketException ex)
                        {
                            Log2.Error("WebSocket transport error: " + ex.Message);
                            Log2.Error("Player NOT added");
                            return;
                        }
                        catch (OperationCanceledException)
                        {
                            Log2.Error("Operation timed out or was canceled");
                            Log2.Error("Player NOT added");
                            return;
                        }
                        catch (FormatException ex)
                        {
                            Log2.Error("Bad hex/bytes or ABI data format: " + ex.Message);
                            Log2.Error("Player NOT added");
                            return;
                        }
                        catch (OverflowException ex)
                        {
                            Log2.Error("Numeric overflow vs Solidity type: " + ex.Message);
                            Log2.Error("Player NOT added");
                            return;
                        }
                        catch (ArgumentException ex)
                        {
                            Log2.Error("Argument/ABI mismatch: " + ex.Message);
                            Log2.Error("Player NOT added");
                            return;
                        }
                        catch (InvalidOperationException ex)
                        {
                            Log2.Error("Invalid operation/state: " + ex.Message);
                            Log2.Error("Player NOT added");
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


                        Log2.Debug("AddPlayerFromConfidential:Calling addGamePlayerFunction.SendTransactionAsync: Est. Gas {0}", padded.ToString());
                        //var newgas = new HexBigInteger(new BigInteger(1000000));

                       
                        Log2.Debug("AddPlayerFromConfidential:AddPlayer: {0} {1} {2}", gpc.GamePlayerId,
                                                                gpc.EthereumExternalAddress,
                                                                gpc.DataConnectString);
                        try 
                        { 
                            var transactionHash = addGamePlayerFunction.SendTransactionAsync(
                                                                        from: externalAddress,
                                                                        gas: padded,
                                                                        value: null,
                                                                        maxFeePerGas: new HexBigInteger(maxFeeWei),
                                                                        maxPriorityFeePerGas: new HexBigInteger(priorityWei),
                                                                        functionInput: new object[] {
                                                                            gpc.GamePlayerId,
                                                                            gpc.EthereumExternalAddress,
                                                                            gpc.DataConnectString
                                                                        }).ConfigureAwait(false).GetAwaiter().GetResult();
                            //gpc.GamePlayerId,
                            //gpc.EthereumExternalAddress,
                            //gpc.DataConnectString
                            //);
                        }
                        catch (SmartContractRevertException ex)
                        {
                            Log2.Error($"Revert: {ex.RevertMessage ?? ex.Message}");
                            // optional: inspect ex.RevertMessageEncoded
                            Log2.Error("Player NOT added");
                            return;
                        }
                        catch (RpcResponseException ex)
                        {
                            Log2.Error($"RPC error {ex.RpcError?.Code}: {ex.RpcError?.Message} | data={ex.RpcError?.Data}");
                            // You can branch on common messages here (nonce, underpriced, funds, etc.)
                            Log2.Error("Player NOT added");
                            return;
                        }
                        catch (HttpRequestException ex)
                        {
                            Log2.Error("HTTP transport error: " + ex.Message);
                            Log2.Error("Player NOT added");
                            return;
                        }
                        catch (WebSocketException ex)
                        {
                            Log2.Error("WebSocket transport error: " + ex.Message);
                            Log2.Error("Player NOT added");
                            return;
                        }
                        catch (OperationCanceledException)
                        {
                            Log2.Error("Operation timed out or was canceled");
                            Log2.Error("Player NOT added");
                            return;
                        }
                        catch (FormatException ex)
                        {
                            Log2.Error("Bad hex/bytes or ABI data format: " + ex.Message);
                            Log2.Error("Player NOT added");
                            return;
                        }
                        catch (OverflowException ex)
                        {
                            Log2.Error("Numeric overflow vs Solidity type: " + ex.Message);
                            Log2.Error("Player NOT added");
                            return;
                        }
                        catch (ArgumentException ex)
                        {
                            Log2.Error("Argument/ABI mismatch: " + ex.Message);
                            Log2.Error("Player NOT added");
                            return;
                        }
                        catch (InvalidOperationException ex)
                        {
                            Log2.Error("Invalid operation/state: " + ex.Message);
                            Log2.Error("Player NOT added");
                            return;
                        }


                        Log2.Trace("AddPlayerFromConfidential:addGamePlayerFunction.SendTransactionAsync Returned");
                        Thread.Sleep(1000);
                    }
                    catch(Exception ex)
                    {
                        Log2.Error("AddPlayerFromConfidential:PlayerAdded Exception: " + ex.ToString());
                    }
                }
                
            }
            catch (Exception ex)
            {
                Log2.Error("AddPlayerFromConfidential: Exception: " + ex.ToString());
            }
           
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameEventID"></param>
        /// <param name="gameEventName"></param>
        /// <param name="gameEventType"></param>
        /// <param name="gameEventStartTime"></param>
        /// <param name="gameEventEndTime"></param>
        /// <param name="gameEventDuration"></param>
        /// <param name="dollarPerPoint"></param>
        /// <param name="pointsPerWatt"></param>
        /// <param name="pointsPerPercent"></param>
        /// <returns></returns>
        public async Task AddEvent(
                                    string  gameEventID,
                                    string  gameEventName,
                                    string  gameEventType,
                                    string  gameEventStartTime,
                                    string  gameEventEndTime,
                                    string  gameEventDuration,
                                    string  dollarPerPoint,
                                    string  pointsPerWatt,
                                    string  pointsPerPercent
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

            BigInteger priorityWei = 1_000_000;        // 1 μgwei = 1,000,000,000,000 wei
                                                               // (well above the 1-wei floor)
            BigInteger maxFeeWei = 2_000_000;

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
                var addGameEventFunction = contract.GetFunction("AddGameEvent");

                Log2.Debug("AddEvent: {0} {1} {2} {3} {4} {5} {6} {7} {8}", gameEventID,
                                                                gameEventName,
                                                                gameEventType,
                                                                gameEventStartTime,
                                                                gameEventEndTime,
                                                                gameEventDuration,
                                                                dollarPerPoint,
                                                                pointsPerWatt,
                                                                pointsPerPercent);

                HexBigInteger estimate;
                try
                {
                    estimate = addGameEventFunction.EstimateGasAsync(externalAddress, null, null,
                                                                                    gameEventID,
                                                                                    gameEventName,
                                                                                    gameEventType,
                                                                                    gameEventStartTime,
                                                                                    gameEventEndTime,
                                                                                    gameEventDuration,
                                                                                    dollarPerPoint,
                                                                                    pointsPerWatt,
                                                                                    pointsPerPercent).ConfigureAwait(false).GetAwaiter().GetResult();
                }
                catch (SmartContractRevertException ex)
                {
                    Log2.Error($"Revert: {ex.RevertMessage ?? ex.Message}");
                    // optional: inspect ex.RevertMessageEncoded
                    Log2.Error("Event NOT added");
                    return;
                }
                catch (RpcResponseException ex)
                {
                    Log2.Error($"RPC error {ex.RpcError?.Code}: {ex.RpcError?.Message} | data={ex.RpcError?.Data}");
                    // You can branch on common messages here (nonce, underpriced, funds, etc.)
                    Log2.Error("Event NOT added");
                    return;
                }
                catch (HttpRequestException ex)
                {
                    Log2.Error("HTTP transport error: " + ex.Message);
                    Log2.Error("Event NOT added");
                    return;
                }
                catch (WebSocketException ex)
                {
                    Log2.Error("WebSocket transport error: " + ex.Message);
                    Log2.Error("Event NOT added");
                    return;
                }
                catch (OperationCanceledException)
                {
                    Log2.Error("Operation timed out or was canceled");
                    Log2.Error("Event NOT added");
                    return;
                }
                catch (FormatException ex)
                {
                    Log2.Error("Bad hex/bytes or ABI data format: " + ex.Message);
                    Log2.Error("Event NOT added");
                    return;
                }
                catch (OverflowException ex)
                {
                    Log2.Error("Numeric overflow vs Solidity type: " + ex.Message);
                    Log2.Error("Event NOT added");
                    return;
                }
                catch (ArgumentException ex)
                {
                    Log2.Error("Argument/ABI mismatch: " + ex.Message);
                    Log2.Error("Event NOT added");
                    return;
                }
                catch (InvalidOperationException ex)
                {
                    Log2.Error("Invalid operation/state: " + ex.Message);
                    Log2.Error("Event NOT added");
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
                Log2.Debug("Calling addGameEventFunction.SendTransactionAsync: Est Gas = {0}", padded.ToString());
                //gas = new HexBigInteger(new BigInteger(1000000));

                try
                {

                    var transactionHash = addGameEventFunction.SendTransactionAsync(
                                                                    from: externalAddress,
                                                                    gas: padded,
                                                                    value: null,
                                                                    maxFeePerGas: new HexBigInteger(maxFeeWei),
                                                                    maxPriorityFeePerGas: new HexBigInteger(priorityWei),
                                                                    functionInput: new object[] {
                                                                        gameEventID,
                                                                        gameEventName,
                                                                        gameEventType,
                                                                        gameEventStartTime,
                                                                        gameEventEndTime,
                                                                        gameEventDuration,
                                                                        dollarPerPoint,
                                                                        pointsPerWatt,
                                                                        pointsPerPercent }).ConfigureAwait(false).GetAwaiter().GetResult();
                }
                catch (SmartContractRevertException ex)
                {
                    Log2.Error($"Revert: {ex.RevertMessage ?? ex.Message}");
                    // optional: inspect ex.RevertMessageEncoded
                    Log2.Error("Event NOT added");
                    return;
                }
                catch (RpcResponseException ex)
                {
                    Log2.Error($"RPC error {ex.RpcError?.Code}: {ex.RpcError?.Message} | data={ex.RpcError?.Data}");
                    // You can branch on common messages here (nonce, underpriced, funds, etc.)
                    Log2.Error("Event NOT added");
                    return;
                }
                catch (HttpRequestException ex)
                {
                    Log2.Error("HTTP transport error: " + ex.Message);
                    Log2.Error("Event NOT added");
                    return;
                }
                catch (WebSocketException ex)
                {
                    Log2.Error("WebSocket transport error: " + ex.Message);
                    Log2.Error("Event NOT added");
                    return;
                }
                catch (OperationCanceledException)
                {
                    Log2.Error("Operation timed out or was canceled");
                    Log2.Error("Event NOT added");
                    return;
                }
                catch (FormatException ex)
                {
                    Log2.Error("Bad hex/bytes or ABI data format: " + ex.Message);
                    Log2.Error("Event NOT added");
                    return;
                }
                catch (OverflowException ex)
                {
                    Log2.Error("Numeric overflow vs Solidity type: " + ex.Message);
                    Log2.Error("Event NOT added");
                    return;
                }
                catch (ArgumentException ex)
                {
                    Log2.Error("Argument/ABI mismatch: " + ex.Message);
                    Log2.Error("Event NOT added");
                    return;
                }
                catch (InvalidOperationException ex)
                {
                    Log2.Error("Invalid operation/state: " + ex.Message);
                    Log2.Error("Event NOT added");
                    return;
                }


                Log2.Trace("addGameEventFunction.SendTransactionAsync Returned");
                
            }
            catch (Exception ex)
            {
                Log2.Error("EventAdded Exception: " + ex.ToString());
            }
            Log2.Debug("EventAdded: {0} {1} {2} {3} {4} {5} {6} {7} {8}", gameEventID,
                                                                gameEventName,
                                                                gameEventType,
                                                                gameEventStartTime,
                                                                gameEventEndTime,
                                                                gameEventDuration,
                                                                dollarPerPoint,
                                                                pointsPerWatt,
                                                                pointsPerPercent);
        }



       /// <summary>
       /// 
       /// </summary>
       /// <param name="gamePlayerID"></param>
       /// <param name="gameEventID"></param>
       /// <param name="averagePowerInWatts"></param>
       /// <param name="baselineAveragePowerInWatts"></param>
       /// <param name="deltaAveragePowerInWatts"></param>
       /// <param name="percentPoints"></param>
       /// <param name="wattPoints"></param>
       /// <param name="totalPointsAwarded"></param>
       /// <param name="awardValue"></param>
       /// <returns></returns>
        public async Task AddResult(
                                    string gamePlayerID,
                                    string gameEventID,
                                    string averagePowerInWatts,
                                    string baselineAveragePowerInWatts,
                                    string deltaAveragePowerInWatts,
                                    string percentPoints,
                                    string wattPoints,
                                    string totalPointsAwarded,
                                    string awardValue
                                   )
        {

            if (!_isConfiguredFlag)
                return;

            //Convert to Nethereum var types
            var contractAddress = _ethereumContractAddress;
            var externalAddress = _ethereumExternalAddress;
            var privateKey = _ethereumExternalPrivateKey;
            var chainId = _ethereumChainId;

            BigInteger priorityWei = 1_000_000;        // 1 μgwei = 1,000,000,000,000 wei
                                                               // (well above the 1-wei floor)
            BigInteger maxFeeWei = 2_000_000;

            try
            {
                Log2.Debug("AddResult Opening Ethereum Account: {0} {1}", privateKey, chainId.ToString());
                var account = new Account(privateKey, chainId);

                Web3 web3;
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
                var addGameResultFunction = contract.GetFunction("AddGameResult");


                Log2.Debug("AddResult: {0} {1} {2} {3} {4} {5} {6} {7} {8} {9}", gamePlayerID,
                                                                            gameEventID,
                                                                            externalAddress,
                                                                            averagePowerInWatts,
                                                                            baselineAveragePowerInWatts,
                                                                            deltaAveragePowerInWatts,
                                                                            percentPoints,
                                                                            wattPoints,
                                                                            totalPointsAwarded,
                                                                            awardValue);
              
                HexBigInteger estimate;

                try
                {
                    estimate = addGameResultFunction.EstimateGasAsync(externalAddress, null, null,
                                                             gamePlayerID,
                                                             gameEventID,
                                                             externalAddress,
                                                             averagePowerInWatts,
                                                             baselineAveragePowerInWatts,
                                                             deltaAveragePowerInWatts,
                                                             percentPoints,
                                                             wattPoints,
                                                             totalPointsAwarded,
                                                             awardValue).ConfigureAwait(false).GetAwaiter().GetResult();

                }
                catch (SmartContractRevertException ex)
                {
                    Log2.Error($"Revert: {ex.RevertMessage ?? ex.Message}");
                    // optional: inspect ex.RevertMessageEncoded
                    Log2.Error("Result NOT added");
                    return;
                }
                catch (RpcResponseException ex)
                {
                    Log2.Error($"RPC error {ex.RpcError?.Code}: {ex.RpcError?.Message} | data={ex.RpcError?.Data}");
                    // You can branch on common messages here (nonce, underpriced, funds, etc.)
                    Log2.Error("Result NOT added");
                    return;
                }
                catch (HttpRequestException ex)
                {
                    Log2.Error("HTTP transport error: " + ex.Message);
                    Log2.Error("Result NOT added");
                    return;
                }
                catch (WebSocketException ex)
                {
                    Log2.Error("WebSocket transport error: " + ex.Message);
                    Log2.Error("Result NOT added");
                    return;
                }
                catch (OperationCanceledException)
                {
                    Log2.Error("Operation timed out or was canceled");
                    Log2.Error("Result NOT added");
                    return;
                }
                catch (FormatException ex)
                {
                    Log2.Error("Bad hex/bytes or ABI data format: " + ex.Message);
                    Log2.Error("Result NOT added");
                    return;
                }
                catch (OverflowException ex)
                {
                    Log2.Error("Numeric overflow vs Solidity type: " + ex.Message);
                    Log2.Error("Result NOT added");
                    return;
                }
                catch (ArgumentException ex)
                {
                    Log2.Error("Argument/ABI mismatch: " + ex.Message);
                    Log2.Error("Result NOT added");
                    return;
                }
                catch (InvalidOperationException ex)
                {
                    Log2.Error("Invalid operation/state: " + ex.Message);
                    Log2.Error("Result NOT added");
                    return;
                }


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

                Log2.Debug("Calling addGameResultFunction.SendTransactionAsync: Est Padded Gas  = {0}", padded.ToString());
                //gas = new HexBigInteger(new BigInteger(1000000));

                try
                {
                    var trasnactionHash = addGameResultFunction.SendTransactionAsync(
                                                                                        from: externalAddress,
                                                                                        gas: padded,
                                                                                        value: null,
                                                                                        maxFeePerGas: new HexBigInteger(maxFeeWei),
                                                                                        maxPriorityFeePerGas: new HexBigInteger(priorityWei),
                                                                                        functionInput: new object[] {
                                                                                            gamePlayerID,
                                                                                            gameEventID,
                                                                                            externalAddress,
                                                                                            averagePowerInWatts,
                                                                                            baselineAveragePowerInWatts,
                                                                                            deltaAveragePowerInWatts,
                                                                                            percentPoints,
                                                                                            wattPoints,
                                                                                            totalPointsAwarded,
                                                                                            awardValue }).ConfigureAwait(false).GetAwaiter().GetResult();

                }
                catch (SmartContractRevertException ex)
                {
                    Log2.Error($"Revert: {ex.RevertMessage ?? ex.Message}");
                    // optional: inspect ex.RevertMessageEncoded
                    Log2.Error("Result NOT added");
                    return;
                }
                catch (RpcResponseException ex)
                {
                    Log2.Error($"RPC error {ex.RpcError?.Code}: {ex.RpcError?.Message} | data={ex.RpcError?.Data}");
                    // You can branch on common messages here (nonce, underpriced, funds, etc.)
                    Log2.Error("Result NOT added");
                    return;
                }
                catch (HttpRequestException ex)
                {
                    Log2.Error("HTTP transport error: " + ex.Message);
                    Log2.Error("Result NOT added");
                    return;
                }
                catch (WebSocketException ex)
                {
                    Log2.Error("WebSocket transport error: " + ex.Message);
                    Log2.Error("Result NOT added");
                    return;
                }
                catch (OperationCanceledException)
                {
                    Log2.Error("Operation timed out or was canceled");
                    Log2.Error("Result NOT added");
                    return;
                }
                catch (FormatException ex)
                {
                    Log2.Error("Bad hex/bytes or ABI data format: " + ex.Message);
                    Log2.Error("Result NOT added");
                    return;
                }
                catch (OverflowException ex)
                {
                    Log2.Error("Numeric overflow vs Solidity type: " + ex.Message);
                    Log2.Error("Result NOT added");
                    return;
                }
                catch (ArgumentException ex)
                {
                    Log2.Error("Argument/ABI mismatch: " + ex.Message);
                    Log2.Error("Result NOT added");
                    return;
                }
                catch (InvalidOperationException ex)
                {
                    Log2.Error("Invalid operation/state: " + ex.Message);
                    Log2.Error("Result NOT added");
                    return;
                }


                Log2.Trace("GameResultFunction.SendTransactionAsync Returned");

            }
            catch (Exception ex)
            {
                Log2.Error("ResultAdded Exception: " + ex.ToString());
                return;
            }
            Log2.Debug("ResultAdded: {0} {1} {2} {3} {4} {5} {6} {7} {8} {9}", gamePlayerID,
                                                                            gameEventID,
                                                                            externalAddress,
                                                                            averagePowerInWatts,
                                                                            baselineAveragePowerInWatts,
                                                                            deltaAveragePowerInWatts,
                                                                            percentPoints,
                                                                            wattPoints,
                                                                            totalPointsAwarded,
                                                                            awardValue); 
        }

        /// <summary>
        /// DEPRECATED
        /// </summary>
        /// <param name="gamePlayerIDFilter"></param>
        /// <returns></returns>
        //public async Task LogPlayer(string gamePlayerIDFilter)
        //{

        //    if (!_isConfiguredFlag)
        //        return;

        //    //Convert to Nethereum var types
        //    var contractAddress = _ethereumContractAddress;
        //    var externalAddress = _ethereumExternalAddress;
        //    var privateKey = _ethereumExternalPrivateKey;
        //    var chainId = _ethereumChainId;

        //    try
        //    {
        //        var account = new Account(privateKey, chainId);
        //        Web3 web3;
        //        if (_ethereumNodeURL.StartsWith("wss"))
        //        {
        //            var clientws = new WebSocketClient(_ethereumNodeURL);
        //            web3 = new Web3(account, clientws);
        //        }
        //        else
        //        {
        //            web3 = new Web3(account, _ethereumNodeURL);
        //        }

        //        // web3 = new Web3(account, _ethereumNodeURL);
        //        var contract = web3.Eth.GetContract(_contractABI, contractAddress);
        //        Event gamePlayerEvent = contract.GetEvent("GamePlayerEvent");

        //        Log2.Trace("Creating filter for GamePlayerEvent events for: {0}", gamePlayerIDFilter);
        //        var filterAll = await gamePlayerEvent.CreateFilterAsync <string,string>(null,gamePlayerIDFilter);
        //        Log2.Debug("-> Found {0}", filterAll.Value);
        //        var log = await gamePlayerEvent.GetAllChanges<GamePlayerEventArgs>(filterAll).ConfigureAwait(false);

        //        Log2.Debug("-> Got GamePlayerEvents {0}", log.Count.ToString());
        //        foreach (var evt in log)
        //        {
        //            Log2.Debug("-> GamePlayer {0} -> {1}: {2} : {3} : {4} : {5}",
        //                gamePlayerIDFilter,
        //                evt.Event.GamePlayerID,
        //                evt.Event.DataConnectionString,
        //                evt.Event.GamePlayerAddress.ToString(),
        //                evt.Event.Status,
        //                evt.Event.Slice
        //                );
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log2.Error("LogPlayers Exception: " + ex.ToString());
        //    }
        //    return;
        //}

        /// <summary>
        /// DEPRECATED
        /// </summary>
        /// <param name="gameEventIDFilter"></param>
        /// <returns></returns>
        //public async Task LogEvent(string gameEventIDFilter)
        //{
        //    if (!_isConfiguredFlag)
        //        return;

        //    //Convert to Nethereum var types
        //    var contractAddress = _ethereumContractAddress;
        //    var externalAddress = _ethereumExternalAddress;
        //    var privateKey = _ethereumExternalPrivateKey;
        //    var chainId = _ethereumChainId;

        //    try
        //    {
        //        var account = new Account(privateKey, chainId);
        //        Web3 web3;
        //        if (_ethereumNodeURL.StartsWith("wss"))
        //        {
        //            var clientws = new WebSocketClient(_ethereumNodeURL);
        //            web3 = new Web3(account, clientws);
        //        }
        //        else
        //        {
        //            web3 = new Web3(account, _ethereumNodeURL);
        //        }

        //        // web3 = new Web3(account, _ethereumNodeURL);
        //        var contract = web3.Eth.GetContract(_contractABI, contractAddress);

        //        Event gameEventEvent = contract.GetEvent("GameEventEvent");

        //        Log2.Trace("Creating GameEventEvent filter for: {0}", gameEventIDFilter);
        //        var filterAll = await gameEventEvent.CreateFilterAsync<object, string>(null, gameEventIDFilter);
        //        //var filterAll = await gameEventEvent.CreateFilterAsync(gameEventIDFilter);
        //        Log2.Debug("-> Created FilterAll: {0}", filterAll.Value);
        //        var log = await gameEventEvent.GetAllChanges<GameEventEventArgs>(filterAll).ConfigureAwait(false);

        //        Log2.Debug("-> Got GameEventEvents {0}", log.Count.ToString());
        //        foreach (var evt in log)
        //        {
        //            Log2.Debug("-> GameEvent For {0} -> {1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}",
        //               gameEventIDFilter,
        //               evt.Event.GameEventID,
        //               evt.Event.GameEventName,
        //               evt.Event.GameEventType,
        //               evt.Event.GameEventStartTime,
        //               evt.Event.GameEventEndTime,
        //               evt.Event.GameEventDuration,
        //               evt.Event.DollarPerPoint,
        //               evt.Event.PointsPerWatt,
        //               evt.Event.PointsPerPercent);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log2.Error("LogEvent Exception: " + ex.ToString());
        //    }
        //    return;
        //}


        /// <summary>
        /// DEPRECATED
        /// </summary>
        /// <param name="gameEventIDFilter"></param>
        /// <returns></returns>
        //public async Task LogResultsForEvent(string gameEventIDFilter)
        //{
        //    if (!_isConfiguredFlag)
        //        return;

        //    //Convert to Nethereum var types
        //    var contractAddress = _ethereumContractAddress;
        //    var externalAddress = _ethereumExternalAddress;
        //    var privateKey = _ethereumExternalPrivateKey;
        //    var chainId = _ethereumChainId;

        //    try
        //    {
        //        var account = new Account(privateKey, chainId);
        //        Web3 web3;
        //        if (_ethereumNodeURL.StartsWith("wss"))
        //        {
        //            var clientws = new WebSocketClient(_ethereumNodeURL);
        //            web3 = new Web3(account, clientws);
        //        }
        //        else
        //        {
        //            web3 = new Web3(account, _ethereumNodeURL);
        //        }

        //        // web3 = new Web3(account, _ethereumNodeURL);
        //        var contract = web3.Eth.GetContract(_contractABI, contractAddress);
        //        Event gameResultEvent = contract.GetEvent("GameResultEvent");

        //        Log2.Trace("Creating filter for all GameResultEvent events");
        //        var filterAll = await gameResultEvent.CreateFilterAsync<object, object, string>(null, null, gameEventIDFilter);
        //        Log2.Debug("-> Created FilterAll: {0}", filterAll.Value);

        //        var log = await gameResultEvent.GetAllChanges<GameResultEventArgs>(filterAll).ConfigureAwait(false);

        //        Log2.Debug("-> Got GameResultEvents {0}", log.Count.ToString());
        //        foreach (var evt in log)
        //        {
        //            Log2.Debug("-> GameResultsForEvent {0} -> {1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}",
        //                gameEventIDFilter,
        //                evt.Event.GamePlayerID,
        //                evt.Event.GameEventID,
        //                evt.Event.AveragePowerInWatts,
        //                evt.Event.BaselineAveragePowerInWatts,
        //                evt.Event.DeltaAveragePowerInWatts,
        //                evt.Event.PercentPoints,
        //                evt.Event.WattPoints,
        //                evt.Event.TotalPointsAwarded,
        //                evt.Event.AwardValue);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log2.Error("GetResultsForEvent Exception: " + ex.ToString());
        //    }
        //    return;
        //}


        /// <summary>
        /// DEPRECATED
        /// </summary>
        /// <param name="gamePlayerIDFilter"></param>
        /// <returns></returns>
        //public async Task LogResultsForPlayer(string gamePlayerIDFilter)
        //{
        //    if (!_isConfiguredFlag)
        //        return;

        //    //Convert to Nethereum var types
        //    var contractAddress = _ethereumContractAddress;
        //    var externalAddress = _ethereumExternalAddress;
        //    var privateKey = _ethereumExternalPrivateKey;
        //    var chainId = _ethereumChainId;

        //    try
        //    {
        //        var account = new Account(privateKey, chainId);
        //        Web3 web3;
        //        if (_ethereumNodeURL.StartsWith("wss"))
        //        {
        //            var clientws = new WebSocketClient(_ethereumNodeURL);
        //            web3 = new Web3(account, clientws);
        //        }
        //        else
        //        {
        //            web3 = new Web3(account, _ethereumNodeURL);
        //        }

        //        // web3 = new Web3(account, _ethereumNodeURL);
        //        var contract = web3.Eth.GetContract(_contractABI, contractAddress);
        //        Event gameResultEvent = contract.GetEvent("GameResultEvent");

        //        Log2.Trace("Creating filter for all GameResultEvent events");
               
        //        var filterAll = await gameResultEvent.CreateFilterAsync<object,string>(null,gamePlayerIDFilter);
        //        Log2.Debug("-> Created FilterAll: {0}", filterAll.Value);

        //        var log = await gameResultEvent.GetAllChanges<GameResultEventArgs>(filterAll).ConfigureAwait(false);

        //        Log2.Debug("-> Got GameResultEvents {0}", log.Count.ToString());
        //        foreach (var evt in log)
        //        {
        //            Log2.Debug("-> GameResultsForPlayer {0} -> {1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}",
        //                gamePlayerIDFilter,
        //                evt.Event.GamePlayerID,
        //                evt.Event.GameEventID,
        //                evt.Event.AveragePowerInWatts,
        //                evt.Event.BaselineAveragePowerInWatts,
        //                evt.Event.DeltaAveragePowerInWatts,
        //                evt.Event.PercentPoints,
        //                evt.Event.WattPoints,
        //                evt.Event.TotalPointsAwarded,
        //                evt.Event.AwardValue);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log2.Error("GetResultsForEvent Exception: " + ex.ToString());
        //    }
        //    return;
        //}



        /// <summary>
        /// DEPRECATED
        /// </summary>
        /// <param name="gamePlayerIDFilter"></param>
        /// <param name="gameEventIDFilter"></param>
        /// <returns></returns>
        //public async Task LogResultsForPlayerEvent(string gamePlayerIDFilter, string gameEventIDFilter)
        //{
        //    if (!_isConfiguredFlag)
        //        return;

        //    //Convert to Nethereum var types
        //    var contractAddress = _ethereumContractAddress;
        //    var externalAddress = _ethereumExternalAddress;
        //    var privateKey = _ethereumExternalPrivateKey;
        //    var chainId = _ethereumChainId;

        //    try
        //    {
        //        var account = new Account(privateKey, chainId);
        //        Web3 web3;
        //        if (_ethereumNodeURL.StartsWith("wss"))
        //        {
        //            var clientws = new WebSocketClient(_ethereumNodeURL);
        //            web3 = new Web3(account, clientws);
        //        }
        //        else
        //        {
        //            web3 = new Web3(account, _ethereumNodeURL);
        //        }

        //        // web3 = new Web3(account, _ethereumNodeURL);
        //        var contract = web3.Eth.GetContract(_contractABI, contractAddress);
        //        Event gameResultEvent = contract.GetEvent("GameResultEvent");

        //        Log2.Trace("Creating filter for a Player Result");

        //        var filterAll = await gameResultEvent.CreateFilterAsync<object,string, string>(null, gamePlayerIDFilter, gameEventIDFilter);
        //        Log2.Debug("-> Created FilterAll: {0}", filterAll.Value);

        //        var log = await gameResultEvent.GetAllChanges<GameResultEventArgs>(filterAll).ConfigureAwait(false);

        //        Log2.Debug("-> Got Player Result {0}", log.Count.ToString());
        //        foreach (var evt in log)
        //        {
        //            Log2.Debug("-> GameResultsForPlayer {0} {1} -> {2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}",
        //                gamePlayerIDFilter,
        //                gameEventIDFilter,
        //                evt.Event.GamePlayerID,
        //                evt.Event.GameEventID,
        //                evt.Event.AveragePowerInWatts,
        //                evt.Event.BaselineAveragePowerInWatts,
        //                evt.Event.DeltaAveragePowerInWatts,
        //                evt.Event.PercentPoints,
        //                evt.Event.WattPoints,
        //                evt.Event.TotalPointsAwarded,
        //                evt.Event.AwardValue);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log2.Error("GetResultsForPlayerEvent Exception: " + ex.ToString());
        //    }
        //    return;
        //}





        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task LogAllPlayers(uint sliceNumber)
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

                //FILTER FOR GamePlayerEvent EVENTS
                //Get Events from Contract
                Event gamePlayerEvent = contract.GetEvent("GamePlayerEvent");

                Log2.Info("LogAllPlayers for Slice {0}", sliceNumber);

                /*
                Log2.Trace("Creating filter for all GamePlayerEvent events");
                var logAllPlayersFilterAll = await gamePlayerEvent.CreateFilterAsync(sliceNumber).ConfigureAwait(false);
                Log2.Debug("LogAllPlayers-> Created FilterAll: {0} for Slice {1}", logAllPlayersFilterAll.Value, sliceNumber);
                var log = await gamePlayerEvent.GetAllChangesAsync<GamePlayerEventArgs>(logAllPlayersFilterAll).ConfigureAwait(false);
                Log2.Debug("-> Got GamePlayers {0} for Slice {1}", log.Count.ToString(), sliceNumber);
                */

                //var latestBlock = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
                //var eventAbi = web3.Eth.GetEvent<GamePlayerEventArgs>();

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
                var playerfilterInput = gamePlayerEvent.CreateFilterInput(sliceNumber);  // or build manually
                var logs = await web3.Eth.Filters.GetLogs
                                    .SendRequestAsync(playerfilterInput)
                                    .ConfigureAwait(false);

                //var logs = await web3.Eth.Filters.GetLogs
                //                    .SendRequestAsync(filterInput)
                //                    .ConfigureAwait(false);

                var decoder = web3.Eth.GetEvent<GamePlayerEventArgs>();
                var decoded = decoder.DecodeAllEventsForEvent(logs); // returns List<EventLog<GamePlayerEventArgs>>

                // Extract just the event data
                var events = decoded
                    .Where(e => e != null)
                    .Select(e => e.Event)
                    .ToList();

                eventCount = events.Count();
                //eventCount = decoded.Count;
                Log2.Debug("-> Got GamePlayers {0} for Slice {1}", eventCount.ToString(), sliceNumber);
                ///

                //foreach (var evt in log)
                //{
                //    Log2.Debug("-> All GamePlayers {0} : {1}: {2} : {3} : {4}",
                //        evt.GamePlayerID,
                //        evt.DataConnectionString,
                //        evt.GamePlayerAddress.ToString(),
                //        evt.Status,
                //        evt.Slice
                //        ); ;
                //}


                string filePath = "logs\\AllGamePlayers" + sliceNumber + ".csv";
                string header =
                    "\"" + "GamePlayerID" + "\",\"" + "DataConnectionString" + "\",\"" + "GamePlayerAddress" + "\",\"" + "PlayerStatus" + "\",\"" + "Slice" + "\"";
                if (eventCount > 0)
                {
                    using (StreamWriter outputFile = new StreamWriter(filePath, false))
                    {
                        outputFile.WriteLine(header);
                        foreach (var evt in events)
                        {
                            string players =
                                "\"" + evt.GamePlayerID + "\",\"" + evt.DataConnectionString + "\",\"" + evt.GamePlayerAddress.ToString() + "\",\"" + evt.Status + "\",\"" + evt.Slice + "\"";
                            outputFile.WriteLine(players);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log2.Error("LogAllPlayers Exception: " + ex.ToString());
            }
            return;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task LogAllEvents(uint sliceNumber)
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

                Event gameEventEvent = contract.GetEvent("GameEventEvent");
                Log2.Info("LogAllEvents for Slice {0}", sliceNumber);

                /*
                //Log2.Trace("Creating filter for all GameEventEvent events");
                var filterAll = await gameEventEvent.CreateFilterAsync(sliceNumber).ConfigureAwait(false);
                Log2.Debug("LogAllEvents-> Created FilterAll: {0} for Slice {1}", filterAll.Value, sliceNumber);
                //Log2.Debug("-> Created FilterAll: {0}", filterAll.Value);
                //Log2.Debug("Get all GameEventEvent events the hard way");
                var log = await gameEventEvent.GetAllChangesAsync<GameEventEventArgs>(filterAll).ConfigureAwait(false);
                */

                //var latestBlock = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
                //var eventAbi = web3.Eth.GetEvent<GameEventEventArgs>();

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
                var playerfilterInput = gameEventEvent.CreateFilterInput(sliceNumber);  // or build manually

                var logs = await web3.Eth.Filters.GetLogs
                                    .SendRequestAsync(playerfilterInput)
                                    .ConfigureAwait(false);

                var decoder = web3.Eth.GetEvent<GameEventEventArgs>();
                var decoded = decoder.DecodeAllEventsForEvent(logs); // returns List<EventLog<GamePlayerEventArgs>>

                // Extract just the event data
                var events = decoded
                    .Where(e => e != null)
                    .Select(e => e.Event)
                    .ToList();

                eventCount = logs.Count();

                Log2.Debug("-> Got GameEventEvents {0} for Slice {1}", eventCount.ToString(), sliceNumber);

                //foreach (var evt in log)
                //{
                //    Log2.Debug("-> All GameEvents {0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}",
                //       evt.GameEventID,
                //       evt.GameEventName,
                //       evt.GameEventType,
                //       evt.GameEventStartTime,
                //       evt.GameEventEndTime,
                //       evt.GameEventDuration,
                //       evt.DollarPerPoint,
                //       evt.PointsPerWatt,
                //       evt.PointsPerPercent);
                //}

                string filePath = "logs\\AllGameEvents" + sliceNumber + ".csv";
                string header =
                    "\"" + "GameEventID" + "\",\"" + "GameEventName" + "\",\"" + "GameEventType" + "\",\"" + "GameEventStartTime" + "\",\"" + "GameEventEndTime" + "\",\"" + "GameEventDuration" + "\",\"" + "DollarPerPoint" + "\",\"" + "PointsPerWatt" + "\",\"" + "PointsPerPercent" + "\",\"" + "Slice" + "\"";
                if (eventCount > 0)
                {
                    using (StreamWriter outputFile = new StreamWriter(filePath, false))
                    {
                        outputFile.WriteLine(header);
                        foreach (var evt in events)
                        {
                            string gameEvents =
                                "\"" + evt.GameEventID + "\",\"" + evt.GameEventName + "\",\"" + evt.GameEventType + "\",\"" + evt.GameEventStartTime + "\",\"" + evt.GameEventEndTime + "\",\"" + evt.GameEventDuration + "\",\"" + evt.DollarPerPoint + "\",\"" + evt.PointsPerWatt + "\",\"" + evt.PointsPerPercent + "\",\"" + evt.Slice + "\"";
                            outputFile.WriteLine(gameEvents);
                        }
                    }
                }
                //else
                //{
                //    using (StreamWriter outputFile = new StreamWriter(filePath, true))
                //    {
                //        foreach (var evt in log)
                //        {
                //            string events =
                //                "\"" + evt.GameEventID + "\",\"" + evt.GameEventName + "\",\"" + evt.GameEventType + "\",\"" + evt.GameEventStartTime + "\",\"" + evt.GameEventEndTime + "\",\"" + evt.GameEventDuration + "\",\"" + evt.DollarPerPoint + "\",\"" + evt.PointsPerWatt + "\",\"" + evt.PointsPerPercent + "\",\"" + evt.Slice + "\"";
                //            outputFile.WriteLine(events);
                //        }
                //    }
                //}
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
        public async Task LogAllResults(uint sliceNumber)
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

                //FILTER FOR GameResultEvent EVENTS
                //Get Events from Contract
                Event gameResultEvent = contract.GetEvent("GameResultEvent");
                Log2.Info("LogAllResults for Slice {0}", sliceNumber);

                /*
                //Log2.Trace("Creating filter for all GameResultEvent events");
                var filterAll = await gameResultEvent.CreateFilterAsync(sliceNumber).ConfigureAwait(false);
                Log2.Debug("LogAllResults-> Created FilterAll: {0} for Slice {1}", filterAll.Value, sliceNumber);
                //Log2.Debug("-> Created FilterAll: {0}", filterAll.Value);
                //Log2.Debug("Get all GameResultEvent events the hard way");
                var log = await gameResultEvent.GetAllChangesAsync<GameResultEventArgs>(filterAll).ConfigureAwait(false);
                */

                //var latestBlock = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
                //var eventAbi = web3.Eth.GetEvent<GameResultEventArgs>();

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
                var playerfilterInput = gameResultEvent.CreateFilterInput(sliceNumber);  // or build manually

                var logs = await web3.Eth.Filters.GetLogs
                                    .SendRequestAsync(playerfilterInput)
                                    .ConfigureAwait(false);

                var decoder = web3.Eth.GetEvent<GameResultEventArgs>();
                var decoded = decoder.DecodeAllEventsForEvent(logs); // returns List<EventLog<GamePlayerEventArgs>>

                // Extract just the event data
                var events = decoded
                    .Where(e => e != null)
                    .Select(e => e.Event)
                    .ToList();

                eventCount = events.Count();
                               
                Log2.Debug("LogAllResults -> Got GameResultEvents {0}", eventCount.ToString());

                foreach (var evt in events)
                {
                    //Log2.Debug("-> All GameResults {0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}",
                    //    evt.GamePlayerID,
                    //    evt.GameEventID,
                    //    evt.GameEventStartTime,
                    //    evt.AveragePowerInWatts,
                    //    evt.BaselineAveragePowerInWatts,
                    //    evt.DeltaAveragePowerInWatts,
                    //    evt.PercentPoints,
                    //    evt.WattPoints,
                    //    evt.TotalPointsAwarded,
                    //    evt.AwardValue,
                    //    evt.Slice);

                    byte[] playerHashBytes = evt.GamePlayerIDIdx;
                    string playerHash = ByteArrayToString(playerHashBytes);
                    //Log2.Debug("LogAllResults playerID,playerHash = {0} -> {1}", evt.Event.GamePlayerID, playerHash);

                    //Accumulate year to date game data for each player
                    try
                    {
                        string dateTime = evt.GameEventStartTime.ToString();
                        char[] delimiterChars = { ' ', '/', ',', '.', ':', '\t' };
                        string[] dateArray = evt.GameEventStartTime.ToString().Split(delimiterChars);
                        string year = dateArray[2];

                        Log2.Debug("LogAllResults:Adding Points: {0}, {1}, {2}, {3}, {4}",
                                                                    evt.GamePlayerID.ToString(),
                                                                    year,
                                                                    evt.AwardValue.ToString(),
                                                                    dateTime,
                                                                    evt.Slice
                                                                    );

                        YearToDatePlayerAwards yearToDatePlayerAwards = null;
                        if (GameAllStatistics.AllYTDAwards.TryGetValue(evt.GamePlayerID.ToString(),
                                                                        out yearToDatePlayerAwards))
                        {
                            yearToDatePlayerAwards.AddPointsAndValue(year,
                                                                Double.Parse(evt.WattPoints),
                                                                Double.Parse(evt.PercentPoints),
                                                                Double.Parse(evt.AwardValue),
                                                                Double.Parse(evt.DeltaAveragePowerInWatts),
                                                                dateTime);
                        }
                        else
                        {
                            yearToDatePlayerAwards = new YearToDatePlayerAwards(evt.GamePlayerID.ToString());
                            yearToDatePlayerAwards.AddPointsAndValue(year,
                                                                Double.Parse(evt.WattPoints),
                                                                Double.Parse(evt.PercentPoints),
                                                                Double.Parse(evt.AwardValue),
                                                                Double.Parse(evt.DeltaAveragePowerInWatts),
                                                                dateTime);
                            GameAllStatistics.AllYTDAwards.TryAdd(evt.GamePlayerID.ToString(),
                                                               yearToDatePlayerAwards);
                        }

                        GameAllStatistics.gameAwards.AddPointsAndValue(evt.GameEventID,
                                                                Double.Parse(evt.WattPoints),
                                                                Double.Parse(evt.PercentPoints),
                                                                Double.Parse(evt.AwardValue),
                                                                Double.Parse(evt.DeltaAveragePowerInWatts)
                                                                );

                    }
                    catch (Exception ex)
                    {
                        Log2.Error("{0}", ex.Message);
                    }
                }


                //Write data to file
                string filePath = "logs\\AllGameResults" + sliceNumber + ".csv";
                string header =
                       "\"" + "GamePlayerID" + "\",\"" + "GameEventID" + "\",\"" + "AveragePowerInWatts" + "\",\"" + "BaselineAveragePowerInWatts" + "\",\"" + "DeltaAveragePowerInWatts" + "\",\"" + "PercentPoints" + "\",\"" + "WattPoints" + "\",\"" + "TotalPointsAwarded" + "\",\"" + "AwardValue" + "\",\"" + "Slice" + "\"";
                if (eventCount > 0)
                {
                    using (StreamWriter outputFile = new StreamWriter(filePath, false))
                    {
                        outputFile.WriteLine(header);
                        foreach (var evt in events)
                        {
                            string gameEvents =
                                "\"" + evt.GamePlayerID + "\",\"" + evt.GameEventID + "\",\"" + evt.AveragePowerInWatts + "\",\"" + evt.BaselineAveragePowerInWatts + "\",\"" + evt.DeltaAveragePowerInWatts + "\",\"" + evt.PercentPoints + "\",\"" + evt.WattPoints + "\",\"" + evt.TotalPointsAwarded + "\",\"" + evt.AwardValue + "\",\"" + evt.Slice + "\"";
                            outputFile.WriteLine(gameEvents);
                        }
                    }
                }
                //else
                //{
                //    using (StreamWriter outputFile = new StreamWriter(filePath, true))
                //    {
                //        foreach (var evt in log)
                //        {
                //            string events =
                //                "\"" + evt.GamePlayerID + "\",\"" + evt.GameEventID + "\",\"" + evt.AveragePowerInWatts + "\",\"" + evt.BaselineAveragePowerInWatts + "\",\"" + evt.DeltaAveragePowerInWatts + "\",\"" + evt.PercentPoints + "\",\"" + evt.WattPoints + "\",\"" + evt.TotalPointsAwarded + "\",\"" + evt.AwardValue + "\",\"" + evt.Slice + "\"";
                //            outputFile.WriteLine(events);
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                Log2.Error("LogAllResults Exception: " + ex.ToString());
            }
            
            return;
        }


      
         ///
         /// <summary>
         /// 
         /// </summary>
         /// <returns></returns>
         public async Task LogAllCombinedEventResults(uint sliceNumber)
         {
            if (!_isConfiguredFlag)
                return;

            //Convert to Nethereum var types
            var contractAddress = _ethereumContractAddress;
            var externalAddress = _ethereumExternalAddress;
            var privateKey = _ethereumExternalPrivateKey;
            var chainId = _ethereumChainId;

            int eventCount = 0;

            TimeSpan fiveMinutes = TimeSpan.FromMinutes(5);
            TimeSpan tenMinutes = TimeSpan.FromMinutes(10);

            try
            {
                var account = new Account(privateKey, chainId);
                Web3 web3;

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

                //FILTER FOR GameResultEvent EVENTS

                //Get Events from Contract
                Event gameCombinedEventResultEvent = contract.GetEvent("GameCombinedEventResultEvent");
                Log2.Info("LogAllCombinedResults for Slice {0}", sliceNumber);
                //Log2.Trace("Creating filter for all GameCombinedEventResultEvent events");

                /*
                var filterAll = await gameCombinedEventResultEvent.CreateFilterAsync(sliceNumber).ConfigureAwait(false);
                //Log2.Debug("-> Created FilterAll: {0}", filterAll.Value);
                //Log2.Debug("Get all GameCombinedEventResultEvent events the hard way");
                Log2.Debug("LogAllCombinedResults-> Created FilterAll: {0} for Slice {1}", filterAll.Value, sliceNumber);
                var log = await gameCombinedEventResultEvent.GetAllChangesAsync<GameCombinedEventResultEventArgs>(filterAll).ConfigureAwait(false);
                */

                //var latestBlock = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
                //var eventAbi = web3.Eth.GetEvent<GameCombinedEventResultEventArgs>();

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
                var playerfilterInput = gameCombinedEventResultEvent.CreateFilterInput(sliceNumber);  // or build manually

                var logs = await web3.Eth.Filters.GetLogs
                                    .SendRequestAsync(playerfilterInput)
                                    .ConfigureAwait(false);

                var decoder = web3.Eth.GetEvent<GameCombinedEventResultEventArgs>();
                var decoded = decoder.DecodeAllEventsForEvent(logs); // returns List<EventLog<GamePlayerEventArgs>>

                // Extract just the event data
                var events = decoded
                    .Where(e => e != null)
                    .Select(e => e.Event)
                    .ToList();

                eventCount = events.Count();

                //eventCount = log.Count;
                Log2.Debug("-> Got gameCombinedEventResultEvents {0}", eventCount.ToString());

                //foreach (var evt in log)
                //{
                //    //Log2.Debug("-> All GameCombinedEventResults {0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}",
                //    //    evt.GamePlayerID,
                //    //    evt.GameEventID,
                //    //    evt.GameEventStartTime,
                //    //    evt.GameEventDuration,
                //    //    evt.PointsPerWatt,
                //    //    evt.AveragePowerInWatts,
                //    //    evt.BaselineAveragePowerInWatts,
                //    //    evt.DeltaAveragePowerInWatts,
                //    //    evt.WattPoints,
                //    //    evt.AwardValue);

                //    byte[] playerHashBytes = evt.Event.GamePlayerIDIdx;
                //    string playerHash = ByteArrayToString(playerHashBytes);
                //    //Log2.Debug("playerID,playerHash = {0} -> {1}", evt.Event.GamePlayerID,playerHash);

                //}


                //Write data to file
                string filePath = "logs\\ALL_GAME_EVENTRESULTS" + sliceNumber + ".csv";
                string header =
                   "\"" + "GamePlayerID" + "\",\"" + "GameEventID" + "\",\"" + "EventStartTime" + "\",\"" + "EventDuration" + "\",\"" + "PointsPerWatt" + "\",\"" + "AveragePowerInWatts" + "\",\"" + "BaselineAveragePowerInWatts" + "\",\"" + "DeltaAveragePowerInWatts" + "\",\"" + "WattPoints" + "\",\"" + "AwardValue" + "\",\"" + "EventEnergyInKWHR" + "\",\"" + "BaselineEnergyInKWHR" + "\",\"" + "Slice" + "\"";

                if (eventCount > 0)
                {
                    using (StreamWriter outputFile = new StreamWriter(filePath, false))
                    {
                        outputFile.WriteLine(header);
                        foreach (var evt in events)
                        {
                            double durationMinutes = Double.Parse(evt.GameEventDuration);
                            double eventWatts = Double.Parse(evt.AveragePowerInWatts);
                            double eventKwattHours = (eventWatts / 1000) * (durationMinutes / 60);
                            double baselineWatts = Double.Parse(evt.BaselineAveragePowerInWatts);
                            double baselineKwattHours = (baselineWatts / 1000) * (durationMinutes / 60);

                            string gameevents =
                                "\"" + evt.GamePlayerID + "\",\"" + evt.GameEventID + "\",\"" + evt.GameEventStartTime + "\",\"" + evt.GameEventDuration + "\",\"" + evt.PointsPerWatt + "\",\"" + evt.AveragePowerInWatts + "\",\"" + evt.BaselineAveragePowerInWatts + "\",\"" + evt.DeltaAveragePowerInWatts + "\",\"" + evt.WattPoints + "\",\"" + evt.AwardValue + "\",\"" + eventKwattHours.ToString("0.######") + "\",\"" + baselineKwattHours.ToString("0.######") + "\",\"" + evt.Slice + "\"";
                            outputFile.WriteLine(gameevents);
                        }
                    }
                }
                //else
                //{
                //    using (StreamWriter outputFile = new StreamWriter(filePath, true))
                //    {
                //        foreach (var evt in log)
                //        {
                //            double durationMinutes = Double.Parse(evt.Event.GameEventDuration);
                //            double eventWatts = Double.Parse(evt.Event.AveragePowerInWatts);
                //            double eventKwattHours = (eventWatts / 1000) * (durationMinutes / 60);
                //            double baselineWatts = Double.Parse(evt.Event.BaselineAveragePowerInWatts);
                //            double baselineKwattHours = (baselineWatts / 1000) * (durationMinutes / 60);

                //            string events =
                //                "\"" + evt.GamePlayerID + "\",\"" + evt.GameEventID + "\",\"" + evt.GameEventStartTime + "\",\"" + evt.GameEventDuration + "\",\"" + evt.PointsPerWatt + "\",\"" + evt.AveragePowerInWatts + "\",\"" + evt.BaselineAveragePowerInWatts + "\",\"" + evt.DeltaAveragePowerInWatts + "\",\"" + evt.WattPoints + "\",\"" + evt.AwardValue + "\",\"" + eventKwattHours.ToString("0.######") + "\",\"" + baselineKwattHours.ToString("0.######") + "\",\"" + evt.Slice + "\"";
                //            outputFile.WriteLine(events);
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                Log2.Error("LogAllCombinedEventResults Exception: " + ex.ToString());
            }
            return;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task LogMyResults(uint sliceNumber,
                                        string gamePlayerId)
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

                //FILTER FOR GameResultEvent EVENTS

                //Get Events from Contract
                Event gameResultEvent = contract.GetEvent("GameResultEvent");
                Log2.Info("LogMyResults for Slice {0}", sliceNumber);
                //hash playerid string
                byte[] playerIDHash;
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    // ComputeHash - returns byte array  
                    playerIDHash = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(gamePlayerId));
                    string playerHash = ByteArrayToString(playerIDHash);
                    //Log2.Debug("playerID,playerHash = {0} -> {1}", gamePlayerId, playerHash);
                }

                //Log2.Trace("Creating filter for all GameResultEvent events");

                //NewFilterInput input = gameResultEvent.CreateFilterInput<uint, byte[]>(null, playerIDHash);

                /*
                var filterAll = await gameResultEvent.CreateFilterAsync(sliceNumber,playerIDHash).ConfigureAwait(false);
                //Log2.Debug("-> Created FilterAll: {0}", filterAll.Value);
                //Log2.Debug("Get all GameResultEvent events the hard way");
                Log2.Debug("LogMyResults-> Created FilterAll: {0} for Slice {1} and Player {2}", filterAll.Value, sliceNumber, playerIDHash);
                var log = await gameResultEvent.GetAllChangesAsync<GameResultEventArgs>(filterAll).ConfigureAwait(false);
                */


                var latestBlock = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
                var eventAbi = web3.Eth.GetEvent<GameResultEventArgs>();

                var encoder = new IntType("uint256");
                var encodedSlice = "0x" + encoder.Encode(sliceNumber).ToHex();
                var encodedPlayerIDHash = "0x" + encoder.Encode(sliceNumber).ToHex();

                var filterInput = new NewFilterInput
                {
                    FromBlock = new BlockParameter(new HexBigInteger(latestBlock.Value - FROM_BLOCK)),
                    ToBlock = new BlockParameter(latestBlock),
                    Address = new[] { contractAddress },
                    Topics = new object[]
                    {
                        eventAbi.EventABI.Sha3Signature, // topic[0] = event signature
                        encodedSlice,                      // topic[1] = indexed sliceNumber
                        playerIDHash
                    }
                };
                // var playerfilterInput = gamePlayerEvent.CreateFilterInput(sliceNumber);  // or build manually

                var logs = await web3.Eth.Filters.GetLogs
                                    .SendRequestAsync(filterInput)
                                    .ConfigureAwait(false);

                var decoder = web3.Eth.GetEvent<GameResultEventArgs>();
                var decoded = decoder.DecodeAllEventsForEvent(logs); // returns List<EventLog<GamePlayerEventArgs>>

                // Extract just the event data
                var events = decoded
                    .Where(e => e != null)
                    .Select(e => e.Event)
                    .ToList();

                eventCount = events.Count();

                Log2.Debug("LogMyResults:Got MyGameResultEvents {0}", eventCount.ToString());

                foreach (var evt in events)
                {
                    bool result = gamePlayerId.Equals(evt.GamePlayerID);
                    if (result)
                    {
                        //Log2.Debug("-> MY GameResults {0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}",
                        //    evt.GamePlayerID,
                        //    evt.GameEventID,
                        //    evt.AveragePowerInWatts,
                        //    evt.BaselineAveragePowerInWatts,
                        //    evt.DeltaAveragePowerInWatts,
                        //    evt.PercentPoints,
                        //    evt.WattPoints,
                        //    evt.TotalPointsAwarded,
                        //    evt.AwardValue);


                        try
                        {
                            string dateTime = evt.GameEventStartTime;
                            char[] delimiterChars = { ' ', '/', ',', '.', ':', '\t' };
                            string[] dateArray = evt.GameEventStartTime.ToString().Split(delimiterChars);
                            string year = dateArray[2];
                            Log2.Debug("LogMyResults:Adding Points: {0}, {1}, {2}, {3}", 
                                                                year, 
                                                                evt.WattPoints.ToString(), 
                                                                evt.AwardValue.ToString(),
                                                                evt.Slice);

                            GamePlayerStatistics.yearToDateC2CAwards.AddPointsAndValue(year,
                                                                Double.Parse(evt.WattPoints),
                                                                Double.Parse(evt.PercentPoints),
                                                                Double.Parse(evt.AwardValue),
                                                                Double.Parse(evt.DeltaAveragePowerInWatts),
                                                                dateTime);
                        }
                        catch (Exception ex)
                        {
                            Log2.Error("{0}", ex.Message);
                        }
                    }
                }

                string filePath = "logs\\MyGameResults" + sliceNumber + ".csv";
                string header =
                        "\"" + "GamePlayerID" + "\",\"" + "GameEventID" + "\",\"" + "AveragePowerInWatts" + "\",\"" + "BaselineAveragePowerInWatts" + "\",\"" + "DeltaAveragePowerInWatts" + "\",\"" + "PercentPoints" + "\",\"" + "WattPoints" + "\",\"" + "TotalPointsAwarded" + "\",\"" + "AwardValue" + "\",\"" + "Slice" + "\"";
                if (eventCount > 0)
                {
                    using (StreamWriter outputFile = new StreamWriter(filePath, false))
                    {
                        outputFile.WriteLine(header);
                        foreach (var evt in events)
                        {
                            bool result = gamePlayerId.Equals(evt.GamePlayerID);
                            if (result)
                            {
                                string gameEvents =
                                    "\"" + evt.GamePlayerID + "\",\"" + evt.GameEventID + "\",\"" + evt.AveragePowerInWatts + "\",\"" + evt.BaselineAveragePowerInWatts + "\",\"" + evt.DeltaAveragePowerInWatts + "\",\"" + evt.PercentPoints + "\",\"" + evt.WattPoints + "\",\"" + evt.TotalPointsAwarded + "\",\"" + evt.AwardValue + "\",\"" + evt.Slice + "\"";
                                outputFile.WriteLine(gameEvents);
                            }
                        }
                    }
                }
                //else
                //{
                //    using (StreamWriter outputFile = new StreamWriter(filePath, true))
                //    {
                //        foreach (var evt in log)
                //        {
                //            bool result = gamePlayerId.Equals(evt.Event.GamePlayerID);
                //            if (result)
                //            {
                //                string events =
                //                    "\"" + evt.Event.GamePlayerID + "\",\"" + evt.Event.GameEventID + "\",\"" + evt.Event.AveragePowerInWatts + "\",\"" + evt.Event.BaselineAveragePowerInWatts + "\",\"" + evt.Event.DeltaAveragePowerInWatts + "\",\"" + evt.Event.PercentPoints + "\",\"" + evt.Event.WattPoints + "\",\"" + evt.Event.TotalPointsAwarded + "\",\"" + evt.Event.AwardValue + "\"";
                //                outputFile.WriteLine(events);
                //            }
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                Log2.Error("LogAllResults Exception: " + ex.ToString());
            }

           
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task LogMyCombinedEventResults(uint sliceNumber,
                                                    string gamePlayerId)
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

                //FILTER FOR GameResultEvent EVENTS

                //Get Events from Contract
                Event gameCombinedEventResultEvent = contract.GetEvent("GameCombinedEventResultEvent");
                Log2.Info("LogMyCombinedEventResults for Slice {0}", sliceNumber);
                //hash playerid string
                byte[] playerIDHash;
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    // ComputeHash - returns byte array  
                    playerIDHash = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(gamePlayerId));
                    string playerHash = ByteArrayToString(playerIDHash);
                    //Log2.Debug("playerID,playerHash = {0} -> {1}", gamePlayerId, playerHash);
                }

                /*
                //Log2.Trace("Creating filter for all GameCombinedEventResultEvent events");
                var filterAll = await gameCombinedEventResultEvent.CreateFilterAsync(sliceNumber, playerIDHash).ConfigureAwait(false);
                //Log2.Debug("-> Created FilterAll: {0}", filterAll.Value);
                //Log2.Debug("Get all GameCombinedEventResultEvent events the hard way");
                Log2.Debug("LogMyCombinedEventResults-> Created FilterAll: {0} for Slice {1} and Player {2}", filterAll.Value, sliceNumber, playerIDHash);
                var log = await gameCombinedEventResultEvent.GetAllChangesAsync<GameCombinedEventResultEventArgs>(filterAll).ConfigureAwait(false);
                */

                var latestBlock = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
                var eventAbi = web3.Eth.GetEvent<GameCombinedEventResultEventArgs>();

                var encoder = new IntType("uint256");
                var encodedSlice = "0x" + encoder.Encode(sliceNumber).ToHex();

                var filterInput = new NewFilterInput
                {
                    FromBlock = new BlockParameter(new HexBigInteger(latestBlock.Value - FROM_BLOCK)),
                    ToBlock = new BlockParameter(latestBlock),
                    Address = new[] { contractAddress },
                    Topics = new object[]
                    {
                        eventAbi.EventABI.Sha3Signature, // topic[0] = event signature
                        encodedSlice,                      // topic[1] = indexed sliceNumber
                        playerIDHash

                    }
                };
                // var playerfilterInput = gamePlayerEvent.CreateFilterInput(sliceNumber);  // or build manually

                var logs = await web3.Eth.Filters.GetLogs
                                    .SendRequestAsync(filterInput)
                                    .ConfigureAwait(false);

                var decoder = web3.Eth.GetEvent<GameCombinedEventResultEventArgs>();
                var decoded = decoder.DecodeAllEventsForEvent(logs); // returns List<EventLog<GamePlayerEventArgs>>

                // Extract just the event data
                var events = decoded
                    .Where(e => e != null)
                    .Select(e => e.Event)
                    .ToList();

                eventCount = events.Count();

                //eventCount = log.Count;
                Log2.Debug("LogMyCombinedEventResults-> Got MyGameCombinedEventResultEvents {0}", eventCount.ToString());

                //foreach (var evt in log)
                //{
                //    bool result = gamePlayerId.Equals(evt.Event.GamePlayerID);
                //    if (result)
                //    {
                //        Log2.Debug("-> My GameCombinedEventResults {0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}",
                //        evt.Event.GamePlayerID,
                //        evt.Event.GameEventID,
                //        evt.Event.GameEventStartTime,
                //        evt.Event.GameEventDuration,
                //        evt.Event.PointsPerWatt,
                //        evt.Event.AveragePowerInWatts,
                //        evt.Event.BaselineAveragePowerInWatts,
                //        evt.Event.DeltaAveragePowerInWatts,
                //        evt.Event.WattPoints,
                //        evt.Event.AwardValue);
                //    }
                //}
                

                string filePath = "logs\\MY_GAME_COMBINEDEVENTRESULTS" + sliceNumber + ".csv";
                string header =
                   "\"" + "GamePlayerID" + "\",\"" + "GameEventID" + "\",\"" + "EventStartTime" + "\",\"" + "EventDuration" + "\",\"" + "PointsPerWatt" + "\",\"" + "AveragePowerInWatts" + "\",\"" + "BaselineAveragePowerInWatts" + "\",\"" + "DeltaAveragePowerInWatts" + "\",\"" + "WattPoints" + "\",\"" + "AwardValue" + "\",\"" + "Slice" + "\"";
                if (eventCount > 0)
                {
                    using (StreamWriter outputFile = new StreamWriter(filePath, false))
                    {
                        outputFile.WriteLine(header);
                        foreach (var evt in events)
                        {
                            bool result = gamePlayerId.Equals(evt.GamePlayerID);
                            if (result)
                            {
                                string gameEvents =
                                "\"" + evt.GamePlayerID + "\",\"" + evt.GameEventID + "\",\"" + evt.GameEventStartTime + "\",\"" + evt.GameEventDuration + "\",\"" + evt.PointsPerWatt + "\",\"" + evt.AveragePowerInWatts + "\",\"" + evt.BaselineAveragePowerInWatts + "\",\"" + evt.DeltaAveragePowerInWatts + "\",\"" + evt.WattPoints + "\",\"" + evt.AwardValue + "\",\"" + evt.Slice + "\"";
                                outputFile.WriteLine(gameEvents);
                            }
                        }
                    }
                }
                //else
                //{
                //    using (StreamWriter outputFile = new StreamWriter(filePath, true))
                //    {
                //        foreach (var evt in log)
                //        {
                //            bool result = gamePlayerId.Equals(evt.Event.GamePlayerID);
                //            if (result)
                //            {
                //                string events =
                //                "\"" + evt.Event.GamePlayerID + "\",\"" + evt.Event.GameEventID + "\",\"" + evt.Event.GameEventStartTime + "\",\"" + evt.Event.GameEventDuration + "\",\"" + evt.Event.PointsPerWatt + "\",\"" + evt.Event.AveragePowerInWatts + "\",\"" + evt.Event.BaselineAveragePowerInWatts + "\",\"" + evt.Event.DeltaAveragePowerInWatts + "\",\"" + evt.Event.WattPoints + "\",\"" + evt.Event.AwardValue + "\",\"" + evt.Event.Slice + "\"";
                //                outputFile.WriteLine(events);
                //            }
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                Log2.Error("LogMyCombinedEventResults Exception: " + ex.ToString());
            }
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetPlayerCount()
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
                var myUintFunction = contract.GetFunction("PlayerCount");

                // CallAsync<T> with type uint (or BigInteger for large values)
                var value = myUintFunction.CallAsync<uint>().ConfigureAwait(false).GetAwaiter().GetResult(); ;

                Log2.Debug($"PlayerCount = {value}");

                return ((int)value);

            }
            catch (Exception ex)
            {
                Log2.Error("GetPlayerCount Exception: " + ex.ToString());
            }
            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetEventCount()
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
                var myUintFunction = contract.GetFunction("EventCount");

                // CallAsync<T> with type uint (or BigInteger for large values)
                var value = myUintFunction.CallAsync<uint>().ConfigureAwait(false).GetAwaiter().GetResult(); ;

                Log2.Debug($"EventCount = {value}");

                return ((int)value);

            }
            catch (Exception ex)
            {
                Log2.Error("GetEventCount Exception: " + ex.ToString());
            }
            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetResultCount()
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
                var myUintFunction = contract.GetFunction("ResultCount");

                // CallAsync<T> with type uint (or BigInteger for large values)
                var value = myUintFunction.CallAsync<uint>().ConfigureAwait(false).GetAwaiter().GetResult(); ;

                Log2.Debug($"ResultCount = {value}");

                return ((int)value);

            }
            catch (Exception ex)
            {
                Log2.Error("GetResultCount Exception: " + ex.ToString());
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
                var myUintFunction = contract.GetFunction("ResultCount");

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
        /// <returns></returns>
        public async Task SendEther(string name, string fromPrivateKey, string toAddress, string ethToTransfer)
        {
            //TODO: Manage nonces

            var myPrivateKey = fromPrivateKey;
            var myAccount = new Account(myPrivateKey, _ethereumChainId);
            var myToAddress = toAddress;
            var myWeb3 = new Web3(myAccount, _ethereumNodeURL);
            decimal myEthToTransfer;
            if (!Decimal.TryParse(ethToTransfer, out myEthToTransfer))
            {
                Log2.Error("Invalid ETH To Transfer = " + ethToTransfer);
                return;
            }

            try
            {
                Log2.Info("Sending " + ethToTransfer + " ETH to " + toAddress);
                //1.11m
                var balanceOriginal = await myWeb3.Eth.GetBalance.SendRequestAsync(myToAddress);
                var balanceOriginalEther = Web3.Convert.FromWei(balanceOriginal.Value);
                //Log2.Info("Original Balance = " + balanceOriginalEther.ToString());
                Log2.Info("Start  Balance for {0}, Account: {1}, Balance = {2}", name, toAddress, balanceOriginalEther.ToString());

                //var transferService = myWeb3.Eth.GetEtherTransferService();
                //var estimate = await transferService.EstimateGasAsync(toAddress, myEthToTransfer);
                //Log2.Trace("Estimate = " + estimate.ToString());
                //var receipt = await myWeb3.Eth.GetEtherTransferService().TransferEtherAndWaitForReceiptAsync(myToAddress, myEthToTransfer, 2, estimate);

                var receipt = await myWeb3.Eth.GetEtherTransferService().TransferEtherAndWaitForReceiptAsync(myToAddress, myEthToTransfer);
                Log2.Trace("Senting " + ethToTransfer + " ETH to " + toAddress);

                var balance = await myWeb3.Eth.GetBalance.SendRequestAsync(myToAddress);
                Log2.Trace($"Ending Balance in Wei: " + balance.Value.ToString());

                var etherAmount = Web3.Convert.FromWei(balance.Value);
                ////Log2.Info($"Ending Balance in Ether: " + etherAmount.ToString());
                Log2.Info("Ending Balance for {0}, Account: {1}, Balance = {2}", name, toAddress, etherAmount.ToString());
            }
            catch(Exception ex)
            {
                Log2.Error("SendEther Exception: {0}", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fromPrivateKey"></param>
        /// <param name="toAddress"></param>
        /// <returns></returns>
        public async Task GetEther(string name, string fromPrivateKey, string toAddress)
        {

            var myPrivateKey = fromPrivateKey;
            var myAccount = new Account(myPrivateKey, _ethereumChainId);
            var myToAddress = toAddress;
            var myWeb3 = new Web3(myAccount, _ethereumNodeURL);


            try 
            { 
            //1.11m
            var balanceOriginal = await myWeb3.Eth.GetBalance.SendRequestAsync(myToAddress);
            var balanceOriginalEther = Web3.Convert.FromWei(balanceOriginal.Value);
            Log2.Info("       Balance for {0}, Account: {1}, Balance = {2}", name, toAddress, balanceOriginalEther.ToString());

            //var transferService = myWeb3.Eth.GetEtherTransferService();
            //var estimate = await transferService.EstimateGasAsync(toAddress, myEthToTransfer);
            //Log2.Trace("Estimate = " + estimate.ToString());
            //var receipt = await myWeb3.Eth.GetEtherTransferService().TransferEtherAndWaitForReceiptAsync(myToAddress, myEthToTransfer, 2, estimate);
            }
            catch (Exception ex)
            {
                Log2.Error("GetEther Exception: {0}", ex);
            }


        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fromPrivateKey"></param>
        /// <param name="toAddress"></param>
        /// <param name="ethToTransfer"></param>
        /// <returns></returns>
        public async Task TransferEther(string name, string fromPrivateKey, string toAddress, string ethToTransfer)
        {

            if (!_isConfiguredFlag)
                return;

            //Convert to Nethereum var types
            var privateKey = fromPrivateKey;
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
                Log2.Debug("TransferEther: {0} {1} {2}", name, fromPrivateKey, toAddress, ethToTransfer);
                var wei = Web3.Convert.ToWei(ethToTransfer);

                //To(the address where you sending the transaction, in this case, the contract address which is automatically set)
                //From(the address from)
                //Gas(the total amount of gas you want to spend, or gas limit)
                //Gas Price(gas price)
                //Value(the amount of ether(in Wei) you want to send, this can be to an account or a contract, in your scenario, you will be sending it to a contract.Your function in solidity should be able to access it using msg.value)
                //Data(in your scenario, this is the function and parameters encoded)


               var transaction = await web3.TransactionManager.SendTransactionAsync(
                                                        account.Address,
                                                        toAddress,
                                                        new HexBigInteger(wei));

                //var transactionReceipt = await web3.TransactionManager.TransactionReceiptService.SendRequestAndWaitForReceiptAsync(
                //                        new TransactionInput() 
                //                        { 
                //                            From = account.Address, 
                //                            To = toAddress, 
                //                            Value = new HexBigInteger(wei) 
                //                        }, 
                //                        null);
            }
            catch (Exception ex)
            {
                Log2.Error("TransferEther Exception: " + ex.ToString());
            }
            
            Log2.Info("TransferEther: {0} {1} {2}", name, fromPrivateKey, toAddress, ethToTransfer);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gpcList"></param>
        /// <returns></returns>
        public async Task TransferEtherFromConfidential(List<GamePlayerConfidential> gpcList, string ether)
        {

            if (!_isConfiguredFlag)
                return;

 
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

                
                foreach (GamePlayerConfidential gpc in gpcList)
                {
                    try
                    {
 
                        Log2.Debug("TransferEther: {0}, {1}, {2}", 
                                            gpc.GamePlayerName, 
                                            gpc.EthereumExternalAddress, 
                                            ether);
                        var wei = Web3.Convert.ToWei(ether);

                        //To(the address where you sending the transaction, in this case, the contract address which is automatically set)
                        //From(the address from)
                        //Gas(the total amount of gas you want to spend, or gas limit)
                        //Gas Price(gas price)
                        //Value(the amount of ether(in Wei) you want to send, this can be to an account or a contract, in your scenario, you will be sending it to a contract.Your function in solidity should be able to access it using msg.value)
                        //Data(in your scenario, this is the function and parameters encoded)


                        var transaction = await web3.TransactionManager.SendTransactionAsync(
                                                                 account.Address,
                                                                 gpc.EthereumExternalAddress,
                                                                 new HexBigInteger(wei));


                        Log2.Trace("TransferEtherFromConfidential:SendTransactionAsync Returned");
                    }
                    catch (Exception ex)
                    {
                        Log2.Error("TransferEtherFromConfidential Exception: " + ex.ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                Log2.Error("TransferEtherFromConfidential Exception: " + ex.ToString());
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task KillContract()
        {

            if (!_isConfiguredFlag)
                return;

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
                var kill = contract.GetFunction("kill");
              

                var gas = await kill.EstimateGasAsync(externalAddress, null, null);

                Log2.Trace("Calling kill.SendTransactionAsync: Est Gas = {0}", gas.ToString());
                gas = new HexBigInteger(new BigInteger(1000000));
                var transactionHash = await kill.SendTransactionAsync( externalAddress,
                                                                       gas,
                                                                       null);

                Log2.Debug("KillContract Succeeded");

            }
            catch (Exception ex)
            {
                Log2.Error("KillContract Exception: " + ex.ToString());
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="confirmed"></param>
        /// <returns></returns>
        public bool SetResultConfirmation(bool confirmed)
        {
            return true;
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
          ""internalType"": ""uint256"",
          ""name"": ""slice"",
          ""type"": ""uint256""
        },
        {
          ""indexed"": true,
          ""internalType"": ""bytes32"",
          ""name"": ""gamePlayerIDIdx"",
          ""type"": ""bytes32""
        },
        {
          ""indexed"": false,
          ""internalType"": ""string"",
          ""name"": ""gamePlayerID"",
          ""type"": ""string""
        },
        {
          ""indexed"": false,
          ""internalType"": ""string"",
          ""name"": ""gameEventID"",
          ""type"": ""string""
        },
        {
          ""indexed"": false,
          ""internalType"": ""string"",
          ""name"": ""gameEventStartTime"",
          ""type"": ""string""
        },
        {
          ""indexed"": false,
          ""internalType"": ""string"",
          ""name"": ""gameEventDuration"",
          ""type"": ""string""
        },
        {
          ""indexed"": false,
          ""internalType"": ""string"",
          ""name"": ""pointsPerWatt"",
          ""type"": ""string""
        },
        {
          ""indexed"": false,
          ""internalType"": ""string"",
          ""name"": ""averagePowerInWatts"",
          ""type"": ""string""
        },
        {
          ""indexed"": false,
          ""internalType"": ""string"",
          ""name"": ""baselineAveragePowerInWatts"",
          ""type"": ""string""
        },
        {
          ""indexed"": false,
          ""internalType"": ""string"",
          ""name"": ""deltaAveragePowerInWatts"",
          ""type"": ""string""
        },
        {
          ""indexed"": false,
          ""internalType"": ""string"",
          ""name"": ""wattPoints"",
          ""type"": ""string""
        },
        {
          ""indexed"": false,
          ""internalType"": ""string"",
          ""name"": ""awardValue"",
          ""type"": ""string""
        }
      ],
      ""name"": ""GameCombinedEventResultEvent"",
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
          ""name"": ""gameEventID"",
          ""type"": ""string""
        },
        {
          ""indexed"": false,
          ""internalType"": ""string"",
          ""name"": ""gameEventName"",
          ""type"": ""string""
        },
        {
          ""indexed"": false,
          ""internalType"": ""string"",
          ""name"": ""gameEventType"",
          ""type"": ""string""
        },
        {
          ""indexed"": false,
          ""internalType"": ""string"",
          ""name"": ""gameEventStartTime"",
          ""type"": ""string""
        },
        {
          ""indexed"": false,
          ""internalType"": ""string"",
          ""name"": ""gameEventEndTime"",
          ""type"": ""string""
        },
        {
          ""indexed"": false,
          ""internalType"": ""string"",
          ""name"": ""gameEventDuration"",
          ""type"": ""string""
        },
        {
          ""indexed"": false,
          ""internalType"": ""string"",
          ""name"": ""dollarPerPoint"",
          ""type"": ""string""
        },
        {
          ""indexed"": false,
          ""internalType"": ""string"",
          ""name"": ""pointsPerWatt"",
          ""type"": ""string""
        },
        {
          ""indexed"": false,
          ""internalType"": ""string"",
          ""name"": ""pointsPerPercent"",
          ""type"": ""string""
        }
      ],
      ""name"": ""GameEventEvent"",
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
          ""name"": ""gamePlayerID"",
          ""type"": ""string""
        },
        {
          ""indexed"": false,
          ""internalType"": ""string"",
          ""name"": ""dataConnectionString"",
          ""type"": ""string""
        },
        {
          ""indexed"": false,
          ""internalType"": ""address"",
          ""name"": ""gamePlayerAddress"",
          ""type"": ""address""
        },
        {
          ""indexed"": false,
          ""internalType"": ""string"",
          ""name"": ""status"",
          ""type"": ""string""
        }
      ],
      ""name"": ""GamePlayerEvent"",
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
          ""indexed"": true,
          ""internalType"": ""bytes32"",
          ""name"": ""gamePlayerIDIdx"",
          ""type"": ""bytes32""
        },
        {
          ""indexed"": false,
          ""internalType"": ""string"",
          ""name"": ""gameEventStartTime"",
          ""type"": ""string""
        },
        {
          ""indexed"": false,
          ""internalType"": ""string"",
          ""name"": ""gamePlayerID"",
          ""type"": ""string""
        },
        {
          ""indexed"": false,
          ""internalType"": ""string"",
          ""name"": ""gameEventID"",
          ""type"": ""string""
        },
        {
          ""indexed"": false,
          ""internalType"": ""string"",
          ""name"": ""averagePowerInWatts"",
          ""type"": ""string""
        },
        {
          ""indexed"": false,
          ""internalType"": ""string"",
          ""name"": ""baselineAveragePowerInWatts"",
          ""type"": ""string""
        },
        {
          ""indexed"": false,
          ""internalType"": ""string"",
          ""name"": ""deltaAveragePowerInWatts"",
          ""type"": ""string""
        },
        {
          ""indexed"": false,
          ""internalType"": ""string"",
          ""name"": ""percentPoints"",
          ""type"": ""string""
        },
        {
          ""indexed"": false,
          ""internalType"": ""string"",
          ""name"": ""wattPoints"",
          ""type"": ""string""
        },
        {
          ""indexed"": false,
          ""internalType"": ""string"",
          ""name"": ""totalPointsAwarded"",
          ""type"": ""string""
        },
        {
          ""indexed"": false,
          ""internalType"": ""string"",
          ""name"": ""awardValue"",
          ""type"": ""string""
        }
      ],
      ""name"": ""GameResultEvent"",
      ""type"": ""event""
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
      ""inputs"": [
        {
          ""internalType"": ""string"",
          ""name"": ""gameEventID"",
          ""type"": ""string""
        },
        {
          ""internalType"": ""string"",
          ""name"": ""gameEventName"",
          ""type"": ""string""
        },
        {
          ""internalType"": ""string"",
          ""name"": ""gameEventType"",
          ""type"": ""string""
        },
        {
          ""internalType"": ""string"",
          ""name"": ""gameEventStartTime"",
          ""type"": ""string""
        },
        {
          ""internalType"": ""string"",
          ""name"": ""gameEventEndTime"",
          ""type"": ""string""
        },
        {
          ""internalType"": ""string"",
          ""name"": ""gameEventDuration"",
          ""type"": ""string""
        },
        {
          ""internalType"": ""string"",
          ""name"": ""dollarPerPoint"",
          ""type"": ""string""
        },
        {
          ""internalType"": ""string"",
          ""name"": ""pointsPerWatt"",
          ""type"": ""string""
        },
        {
          ""internalType"": ""string"",
          ""name"": ""pointsPerPercent"",
          ""type"": ""string""
        }
      ],
      ""name"": ""AddGameEvent"",
      ""outputs"": [],
      ""stateMutability"": ""nonpayable"",
      ""type"": ""function""
    },
    {
      ""inputs"": [
        {
          ""internalType"": ""string"",
          ""name"": ""gamePlayerID"",
          ""type"": ""string""
        },
        {
          ""internalType"": ""address"",
          ""name"": ""playerAddress"",
          ""type"": ""address""
        },
        {
          ""internalType"": ""string"",
          ""name"": ""dataConnectionString"",
          ""type"": ""string""
        }
      ],
      ""name"": ""AddGamePlayer"",
      ""outputs"": [],
      ""stateMutability"": ""nonpayable"",
      ""type"": ""function""
    },
    {
      ""inputs"": [
        {
          ""internalType"": ""string"",
          ""name"": ""gamePlayerID"",
          ""type"": ""string""
        },
        {
          ""internalType"": ""string"",
          ""name"": ""gameEventID"",
          ""type"": ""string""
        },
        {
          ""internalType"": ""address"",
          ""name"": ""gamePlayerAddress"",
          ""type"": ""address""
        },
        {
          ""internalType"": ""string"",
          ""name"": ""averagePowerInWatts"",
          ""type"": ""string""
        },
        {
          ""internalType"": ""string"",
          ""name"": ""baselineAveragePowerInWatts"",
          ""type"": ""string""
        },
        {
          ""internalType"": ""string"",
          ""name"": ""deltaAveragePowerInWatts"",
          ""type"": ""string""
        },
        {
          ""internalType"": ""string"",
          ""name"": ""percentPoints"",
          ""type"": ""string""
        },
        {
          ""internalType"": ""string"",
          ""name"": ""wattPoints"",
          ""type"": ""string""
        },
        {
          ""internalType"": ""string"",
          ""name"": ""totalPointsAwarded"",
          ""type"": ""string""
        },
        {
          ""internalType"": ""string"",
          ""name"": ""awardValue"",
          ""type"": ""string""
        }
      ],
      ""name"": ""AddGameResult"",
      ""outputs"": [],
      ""stateMutability"": ""nonpayable"",
      ""type"": ""function""
    },
    {
      ""inputs"": [],
      ""name"": ""EventCount"",
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
      ""name"": ""PlayerCount"",
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
      ""name"": ""ResultCount"",
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
      ""name"": ""ResultSliceCount"",
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
      ""inputs"": [
        {
          ""internalType"": ""string"",
          ""name"": ""playerID"",
          ""type"": ""string""
        },
        {
          ""internalType"": ""string"",
          ""name"": ""status"",
          ""type"": ""string""
        }
      ],
      ""name"": ""UpdatePlayerStatus"",
      ""outputs"": [],
      ""stateMutability"": ""nonpayable"",
      ""type"": ""function""
    },
    {
      ""inputs"": [
        {
          ""internalType"": ""string"",
          ""name"": ""resultID"",
          ""type"": ""string""
        },
        {
          ""internalType"": ""string"",
          ""name"": ""status"",
          ""type"": ""string""
        }
      ],
      ""name"": ""UpdateResultsStatus"",
      ""outputs"": [],
      ""stateMutability"": ""nonpayable"",
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
      ""name"": ""kill"",
      ""outputs"": [],
      ""stateMutability"": ""nonpayable"",
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
