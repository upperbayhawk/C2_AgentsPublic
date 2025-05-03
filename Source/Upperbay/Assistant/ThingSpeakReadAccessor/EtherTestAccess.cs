using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Threading;
using System.Text;

using System.Threading.Tasks;

using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.Web3.Accounts.Managed;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts.CQS;
using Nethereum.Util;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Contracts;
using Nethereum.Contracts.Extensions;
using Nethereum.RPC.Eth.DTOs;


//for UA
using System.Security.Cryptography.X509Certificates;

// Assemblies needed for Agentness
using Upperbay.Core.Logging;
using Upperbay.Agent.Interfaces;
using Nethereum.Model;
using Org.BouncyCastle.Security;
using Account = Nethereum.Web3.Accounts.Account;

namespace Upperbay.Assistant
{

    [Event("PumpEvent")]
    public class PumpEventArgs: IEventDTO
    {
        [Parameter("string", "pumpname_idx", 1, true)]
        public string PumpnameIdx { get; set; }

        [Parameter("string", "pumpname", 2, false)]
        public string Pumpname { get; set; }

        [Parameter("uint256", "seqNumber", 3, false)]
        public BigInteger Seqnumber { get; set; }

        [Parameter("address", "contractAddress", 4, false)]
        public string ContractAddress { get; set; }
    };

    [Event("OracleDockEvent")]
    public class OracleDockEventArgs : IEventDTO
    {
        [Parameter("string", "oraclename_idx", 1, true)]
        public string OracleNameIdx { get; set; }

        [Parameter("string", "oraclename", 2, false)]
        public string OracleName { get; set; }

        [Parameter("uint256", "seqnumber", 3, false)]
        public BigInteger SeqNumber { get; set; }

        [Parameter("address", "oracle_address", 4, false)]
        public string OracleAddress { get; set; }

        [Parameter("string", "time", 5, false)]
        public string Time{ get; set; }
    };

    [Event("OracleUndockEvent")]
    public class OracleUndockEventArgs : IEventDTO
    {
        [Parameter("string", "oraclename_idx", 1, true)]
        public string OracleNameIdx { get; set; }

        [Parameter("string", "oraclename", 2, false)]
        public string OracleName { get; set; }

        [Parameter("uint256", "seqnumber", 3, false)]
        public BigInteger SeqNumber { get; set; }

        [Parameter("address", "oracle_address", 4, false)]
        public string OracleAddress { get; set; }

        [Parameter("string", "time", 5, false)]
        public string Time { get; set; }
    };

    [Event("OraclePingEvent")]
    public class OraclePingEventArgs : IEventDTO
    {
        [Parameter("string", "oraclename_idx", 1, true)]
        public string OracleNameIdx { get; set; }

        [Parameter("string", "oraclename", 2, false)]
        public string OracleName { get; set; }

        [Parameter("uint256", "seqnumber", 3, false)]
        public BigInteger SeqNumber { get; set; }

        [Parameter("address", "oracle_address", 4, false)]
        public string OracleAddress { get; set; }

        [Parameter("string", "time", 5, false)]
        public string Time { get; set; }
    };



    public class EtherTestAccess
    {
        //Properties
        private string ContractABI = @"[{'inputs':[],'stateMutability':'nonpayable','type':'constructor'},{'anonymous':false,'inputs':[{'indexed':true,'internalType':'string','name':'oracleNameIdx','type':'string'},{'indexed':false,'internalType':'string','name':'oracleName','type':'string'},{'indexed':false,'internalType':'uint256','name':'seqNumber','type':'uint256'},{'indexed':false,'internalType':'address','name':'oracleAddress','type':'address'},{'indexed':false,'internalType':'string','name':'time','type':'string'}],'name':'OracleDockEvent','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'internalType':'string','name':'oracleNameIdx','type':'string'},{'indexed':false,'internalType':'string','name':'oracleName','type':'string'},{'indexed':false,'internalType':'uint256','name':'seqNumber','type':'uint256'},{'indexed':false,'internalType':'address','name':'oracleAddress','type':'address'},{'indexed':false,'internalType':'string','name':'time','type':'string'}],'name':'OracleGetDataEvent','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'internalType':'string','name':'oracleNameIdx','type':'string'},{'indexed':false,'internalType':'string','name':'oracleName','type':'string'},{'indexed':false,'internalType':'uint256','name':'seqNumber','type':'uint256'},{'indexed':false,'internalType':'address','name':'oracleAddress','type':'address'},{'indexed':false,'internalType':'string','name':'time','type':'string'}],'name':'OraclePingEvent','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'internalType':'string','name':'oracleNameIdx','type':'string'},{'indexed':false,'internalType':'string','name':'oracleName','type':'string'},{'indexed':false,'internalType':'uint256','name':'seqNumber','type':'uint256'},{'indexed':false,'internalType':'address','name':'oracleAddress','type':'address'},{'indexed':false,'internalType':'string','name':'time','type':'string'}],'name':'OracleUndockEvent','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'internalType':'address','name':'previousOwner','type':'address'},{'indexed':true,'internalType':'address','name':'newOwner','type':'address'}],'name':'OwnershipTransferred','type':'event'},{'inputs':[],'name':'HasDataReadyFlag','outputs':[{'internalType':'bool','name':'','type':'bool'}],'stateMutability':'view','type':'function'},{'inputs':[],'name':'HasDockedOracleFlag','outputs':[{'internalType':'bool','name':'','type':'bool'}],'stateMutability':'view','type':'function'},{'inputs':[{'internalType':'string','name':'_name','type':'string'},{'internalType':'string','name':'_description','type':'string'},{'internalType':'string','name':'_accessPath','type':'string'},{'internalType':'string','name':'_creationTime','type':'string'}],'name':'addDataItem','outputs':[{'internalType':'bytes32','name':'','type':'bytes32'}],'stateMutability':'nonpayable','type':'function'},{'inputs':[],'name':'addTestData','outputs':[],'stateMutability':'nonpayable','type':'function'},{'inputs':[{'internalType':'bytes32','name':'_dataItemId','type':'bytes32'}],'name':'dataItemExists','outputs':[{'internalType':'bool','name':'','type':'bool'}],'stateMutability':'view','type':'function'},{'inputs':[{'internalType':'string','name':'_oracleName','type':'string'},{'internalType':'string','name':'_oracleNameHash','type':'string'},{'internalType':'uint256','name':'_seqNumber','type':'uint256'},{'internalType':'address','name':'_oracleAddress','type':'address'}],'name':'dockOracle','outputs':[{'internalType':'string','name':'','type':'string'}],'stateMutability':'payable','type':'function'},{'inputs':[],'name':'getAddress','outputs':[{'internalType':'address','name':'','type':'address'}],'stateMutability':'view','type':'function'},{'inputs':[],'name':'getAllDataItems','outputs':[{'internalType':'bytes32[]','name':'','type':'bytes32[]'}],'stateMutability':'view','type':'function'},{'inputs':[{'internalType':'bytes32','name':'_dataItemId','type':'bytes32'}],'name':'getDataItem','outputs':[{'components':[{'internalType':'bytes32','name':'id','type':'bytes32'},{'internalType':'string','name':'name','type':'string'},{'internalType':'string','name':'description','type':'string'},{'internalType':'string','name':'externalName','type':'string'},{'internalType':'string','name':'value','type':'string'},{'internalType':'string','name':'externalValue','type':'string'},{'internalType':'string','name':'quality','type':'string'},{'internalType':'string','name':'updateTime','type':'string'},{'internalType':'string','name':'serverTime','type':'string'},{'internalType':'string','name':'creationTime','type':'string'},{'internalType':'string','name':'accessPath','type':'string'},{'internalType':'enum DataPumpOracle.DataItemOutcome','name':'outcome','type':'uint8'}],'internalType':'struct DataPumpOracle.DataItem','name':'','type':'tuple'}],'stateMutability':'view','type':'function'},{'inputs':[{'internalType':'bytes32','name':'_dataItemId','type':'bytes32'}],'name':'getDataItemValue','outputs':[{'internalType':'string','name':'','type':'string'}],'stateMutability':'view','type':'function'},{'inputs':[{'internalType':'bool','name':'_pending','type':'bool'}],'name':'getMostRecentDataItem','outputs':[{'components':[{'internalType':'bytes32','name':'id','type':'bytes32'},{'internalType':'string','name':'name','type':'string'},{'internalType':'string','name':'description','type':'string'},{'internalType':'string','name':'externalName','type':'string'},{'internalType':'string','name':'value','type':'string'},{'internalType':'string','name':'externalValue','type':'string'},{'internalType':'string','name':'quality','type':'string'},{'internalType':'string','name':'updateTime','type':'string'},{'internalType':'string','name':'serverTime','type':'string'},{'internalType':'string','name':'creationTime','type':'string'},{'internalType':'string','name':'accessPath','type':'string'},{'internalType':'enum DataPumpOracle.DataItemOutcome','name':'outcome','type':'uint8'}],'internalType':'struct DataPumpOracle.DataItem','name':'','type':'tuple'}],'stateMutability':'view','type':'function'},{'inputs':[],'name':'getPendingDataItems','outputs':[{'internalType':'bytes32[]','name':'','type':'bytes32[]'}],'stateMutability':'view','type':'function'},{'inputs':[],'name':'kill','outputs':[],'stateMutability':'nonpayable','type':'function'},{'inputs':[],'name':'owner','outputs':[{'internalType':'address','name':'','type':'address'}],'stateMutability':'view','type':'function'},{'inputs':[{'internalType':'string','name':'_oracleName','type':'string'},{'internalType':'uint256','name':'_seqNumber','type':'uint256'},{'internalType':'address','name':'_oracleAddress','type':'address'}],'name':'pingOracle','outputs':[{'internalType':'string','name':'','type':'string'}],'stateMutability':'payable','type':'function'},{'inputs':[{'internalType':'bytes32','name':'_dataItemId','type':'bytes32'},{'internalType':'string','name':'_value','type':'string'}],'name':'setDataItemValueById','outputs':[{'internalType':'string','name':'','type':'string'}],'stateMutability':'payable','type':'function'},{'inputs':[{'internalType':'string','name':'_name','type':'string'},{'internalType':'string','name':'_value','type':'string'}],'name':'setDataItemValueByName','outputs':[{'internalType':'string','name':'','type':'string'}],'stateMutability':'payable','type':'function'},{'inputs':[],'name':'testConnection','outputs':[{'internalType':'bool','name':'','type':'bool'}],'stateMutability':'pure','type':'function'},{'inputs':[{'internalType':'address','name':'newOwner','type':'address'}],'name':'transferOwnership','outputs':[],'stateMutability':'nonpayable','type':'function'},{'inputs':[{'internalType':'string','name':'_oracleName','type':'string'},{'internalType':'uint256','name':'_seqNumber','type':'uint256'},{'internalType':'address','name':'_oracleAddress','type':'address'}],'name':'undockOracle','outputs':[{'internalType':'string','name':'','type':'string'}],'stateMutability':'payable','type':'function'}]";
        private string EthereumOracleName = "Upperbay Systems DataPump Oracle";
        private string EthereumOracleNameHash = null;
        private string EthereumNodeURL = null;
        private string EthereumContractAddress = null;
        private string EthereumExternalAddress = null;
        private string EthereumExternalPrivateKey = null;
        private bool IsConfiguredFlag = false;
        private bool HasDockedOracleFlag = false;
        private uint EthereumOracleSequenceNumber = 0;

        private string TestAccount = "0xaf964a5f61Bb13d2Ca06eBDAF1b27e259899E168";

        private class TagConfig
        {
            public string channelID;
            public string fieldID;
            public string key;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EtherTestAccess()
        {
            EthereumOracleNameHash = ComputeSha256Hash(EthereumOracleName);
        }

        /// <summary>
        /// SendEtherToContract
        /// </summary>
        /// <param name="url"></param>
        /// <param name="address"></param>
        /// <param name="pkey"></param>
        /// <returns></returns>


        public async Task SendEtherToContract()
        {

            if ((!IsConfiguredFlag) || (!HasDockedOracleFlag))
                return;

            var privateKey = EthereumExternalPrivateKey;
            var account = new Account(privateKey);
            //            var toAddress = contract_address;
            //var toAddress = "0x53eFDf11B302ed090D09690937E4Cd0EB9779E67";
            var toAddress = TestAccount;
            var web3 = new Web3(account, EthereumNodeURL);
            Log2.Trace("Sending Transfer ether");

            var balanceOriginal = await web3.Eth.GetBalance.SendRequestAsync(toAddress);
            var balanceOriginalEther = Web3.Convert.FromWei(balanceOriginal.Value);
            Log2.Trace("Original Balance = " + balanceOriginalEther.ToString());

            var transferService = web3.Eth.GetEtherTransferService();
            var estimate = await transferService.EstimateGasAsync(toAddress, 1.11m);
            Log2.Trace("Estimate = " + estimate.ToString());

            var receipt = await web3.Eth.GetEtherTransferService()
                            .TransferEtherAndWaitForReceiptAsync(toAddress, 1.11m, 2, estimate);
            Log2.Trace("Received Transfer ether");

            var balance = await web3.Eth.GetBalance.SendRequestAsync(toAddress);
            Log2.Trace($"Balance in Wei: " + balance.Value.ToString());

            var etherAmount = Web3.Convert.FromWei(balance.Value);
            Log2.Trace($"Balance in Ether: " + etherAmount.ToString());

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_url"></param>
        /// <param name="_contractAddress"></param>
        /// <param name="_externalAddress"></param>
        /// <param name="_pkey"></param>
        /// <returns></returns>
        public bool ConfigureOracle(string _name, string _url, string _contractAddress, string _externalAddress, string _pkey)
        {
            // call a method and get event logs
            EthereumOracleName = _name;
            EthereumNodeURL = _url;
            EthereumContractAddress = _contractAddress;
            EthereumExternalAddress = _externalAddress;
            EthereumExternalPrivateKey = _pkey;

            //Make sure args got here correctly
            Log2.Trace("Contract URL: " + EthereumNodeURL);
            Log2.Trace("Contract address: " + EthereumContractAddress);
            Log2.Trace("External address: " + EthereumExternalAddress);
            Log2.Trace("External pkey: " + EthereumExternalPrivateKey);
            Log2.Trace("Oracle Name Hash: " + EthereumOracleNameHash);

            
            IsConfiguredFlag = true;
            HasDockedOracleFlag = false;
            EthereumOracleSequenceNumber = 0;

            return (true);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="contract_address"></param>
        /// <param name="external_address"></param>
        /// <param name="pkey"></param>
        /// <returns></returns>
        public async Task DockOracle()
        {

//            if ((!IsConfiguredFlag) || (HasDockedOracleFlag))
//                return;
            if (!IsConfiguredFlag)
                return;


            //Convert to Nethereum var types
            var contractAddress = EthereumContractAddress;
            var externalAddress = EthereumExternalAddress;
            var privateKey = EthereumExternalPrivateKey;

            try
            {
                var account = new Account(privateKey);
                var web3 = new Web3(account, EthereumNodeURL);
                var contract = web3.Eth.GetContract(ContractABI, contractAddress);

                EthereumOracleSequenceNumber++;
                // Get contract properties
                Task<bool> hasDockedOracle = contract.GetFunction("HasDockedOracleFlag").CallAsync<bool>();
                hasDockedOracle.Wait();
                bool myhasDockedOracle = (bool)hasDockedOracle.Result;
                Log2.Trace("HasDockedDataPump = " + myhasDockedOracle);

                if (myhasDockedOracle)
                {
                    Log2.Trace("WARNING: Already HasDockedOracleFlag = " + myhasDockedOracle);
                   // return;
                }

                //Task <BigInteger> seqNumber = contract.GetFunction("_seqNumber").CallAsync<BigInteger>();
                //seqNumber.Wait();
                //string mySeqNumber = seqNumber.Result.ToString();
                //Log2.Trace("Ping: seqNumber = " + mySeqNumber);

                // Call a contract function
                //ADD RETURN VALUE
                EthereumOracleSequenceNumber++;
                uint myseq = EthereumOracleSequenceNumber;
                HexBigInteger gas = new HexBigInteger(new BigInteger(400000));
                HexBigInteger value = new HexBigInteger(new BigInteger(89));
                Task<string> dockFunction = contract.GetFunction("dockOracle").SendTransactionAsync(externalAddress, gas, value, EthereumOracleName, EthereumOracleNameHash, myseq);
                Log2.Trace("Docked Oracle");

                //FILTER FOR DockOracle EVENTS

                //Get Events from Contract
                Event oracleDockEvent = contract.GetEvent("OracleDockEvent");

                Log2.Trace($"Creating filter for all events");
                var filterAll = await oracleDockEvent.CreateFilterAsync().ConfigureAwait(false);
                Log2.Trace($"-> Created {filterAll.Value}");

                Log2.Trace("Get all events the hard way");
                var log = await oracleDockEvent.GetFilterChanges<OracleDockEventArgs>(filterAll).ConfigureAwait(false);
                Log2.Trace($"-> Got {log.Count}");
                foreach (var evt in log)
                {
                    Log2.Trace("-> OracleDockEvent {0} : {1} : {2} : {3}",
                        evt.Event.OracleNameIdx.ToString(),
                        evt.Event.OracleName.ToString(),
                        evt.Event.SeqNumber.ToString(),
                        evt.Event.OracleAddress);
                }

                var pumpEventHandler = web3.Eth.GetEvent<OracleDockEventArgs>(contractAddress);
                var filterAllPumpEventsForContract = pumpEventHandler.CreateFilterInput();
                var allPumpEventsForContract2 = await pumpEventHandler.GetAllChanges(filterAllPumpEventsForContract);

                //var filterTransferEventsForContractAllReceiverAddresses = transferEventHandler.CreateFilterInput(null, new[] { receiverAddress2, receiverAddress });
                //var filterIdPumpEventsForContractAllReceiverAddress2 = await pumpEventHandler.CreateFilterAsync(filterTransferEventsForContractAllReceiverAddress2);
                //var allPumpEventsForContract2 = await pumpEventHandler.GetFilterChanges(filterAllPumpEventsForContract);

                //var filterTransferEventsForContractReceiverAddress2 = transferEventHandler.CreateFilterInput(account.Address, receiverAddress2);
                //var transferEventsForContractReceiverAddress2 = await transferEventHandler.GetAllChanges(filterTransferEventsForContractReceiverAddress2);

                Log2.Trace("Number of Events: {0}" + allPumpEventsForContract2.Count.ToString());

                foreach (var evt in allPumpEventsForContract2)
                {
                    Log2.Trace("-> OracleDockEvent {0} : {1} : {2} : {3}",
                        evt.Event.OracleNameIdx.ToString(),
                        evt.Event.OracleName.ToString(),
                        evt.Event.SeqNumber.ToString(),
                        evt.Event.OracleAddress);
                }
            }
            catch (Exception ex)
            {
                Log2.Trace("Exception: " + ex.ToString());

            }
            Log2.Trace("Dock Event completed");

            HasDockedOracleFlag = true;
        }




        /// <summary>
        /// UndockOracle
        /// </summary>
        /// <param name="url"></param>
        /// <param name="contract_address"></param>
        /// <param name="external_address"></param>
        /// <param name="pkey"></param>
        /// <returns></returns>
        public async Task UndockOracle()
        {
            //if ((!IsConfiguredFlag) || (!HasDockedOracleFlag))
            //    return;
            if (!IsConfiguredFlag)
                return;

            //Convert to Nethereum var types
            var contractAddress = EthereumContractAddress;
            var externalAddress = EthereumExternalAddress;
            var privateKey = EthereumExternalPrivateKey;

            try
            {
                var account = new Account(privateKey);
                var web3 = new Web3(account, EthereumNodeURL);
                var contract = web3.Eth.GetContract(ContractABI, contractAddress);

                EthereumOracleSequenceNumber++;
                // Get contract properties
                Task<bool> hasDockedOracle = contract.GetFunction("HasDockedOracleFlag").CallAsync<bool>();
                hasDockedOracle.Wait();
                bool myhasDockedOracle = (bool)hasDockedOracle.Result;
                Log2.Trace("HasDockedDataPump = " + myhasDockedOracle);

                if (!myhasDockedOracle)
                {
                    Log2.Trace("Warning: Already Undocked: Flag =  " + myhasDockedOracle);
                   // return;
                }

                // Call a contract function
                //ADD RETURN VALUE
                EthereumOracleSequenceNumber++;
                uint seq = EthereumOracleSequenceNumber;
                HexBigInteger gas = new HexBigInteger(new BigInteger(400000));
                HexBigInteger value = new HexBigInteger(new BigInteger(89));
                Task<string> dockFunction = contract.GetFunction("undockOracle").SendTransactionAsync(externalAddress, gas, value, EthereumOracleName, seq);
                Log2.Trace("Undocked Oracle");

                //FILTER FOR DockOracle EVENTS

                //Get Events from Contract
                Event oracleUndockEvent = contract.GetEvent("OracleUndockEvent");

                Log2.Trace($"Creating filter for all events");
                var filterAll = await oracleUndockEvent.CreateFilterAsync().ConfigureAwait(false);
                Log2.Trace($"-> Created {filterAll.Value}");

                Log2.Trace("Get all events the hard way");
                var log = await oracleUndockEvent.GetFilterChanges<OracleUndockEventArgs>(filterAll).ConfigureAwait(false);
                Log2.Trace($"-> Got {log.Count}");
                foreach (var evt in log)
                {
                    Log2.Trace("-> UndockEvent {0} : {1} : {2} : {3}",
                        evt.Event.OracleNameIdx.ToString(),
                        evt.Event.OracleName.ToString(),
                        evt.Event.SeqNumber.ToString(),
                        evt.Event.OracleAddress);
                }

                var pumpEventHandler = web3.Eth.GetEvent<OracleUndockEventArgs>(contractAddress);
                var filterAllPumpEventsForContract = pumpEventHandler.CreateFilterInput();
                var allPumpEventsForContract2 = await pumpEventHandler.GetAllChanges(filterAllPumpEventsForContract);

                //var filterTransferEventsForContractAllReceiverAddresses = transferEventHandler.CreateFilterInput(null, new[] { receiverAddress2, receiverAddress });
                //var filterIdPumpEventsForContractAllReceiverAddress2 = await pumpEventHandler.CreateFilterAsync(filterTransferEventsForContractAllReceiverAddress2);
                //var allPumpEventsForContract2 = await pumpEventHandler.GetFilterChanges(filterAllPumpEventsForContract);

                //var filterTransferEventsForContractReceiverAddress2 = transferEventHandler.CreateFilterInput(account.Address, receiverAddress2);
                //var transferEventsForContractReceiverAddress2 = await transferEventHandler.GetAllChanges(filterTransferEventsForContractReceiverAddress2);

                Log2.Trace("Number of Events: {0}" + allPumpEventsForContract2.Count.ToString());

                foreach (var evt in allPumpEventsForContract2)
                {
                    Log2.Trace("-> UndockEvent {0} : {1} : {2} : {3}",
                        evt.Event.OracleNameIdx.ToString(),
                        evt.Event.OracleName.ToString(),
                        evt.Event.SeqNumber.ToString(),
                        evt.Event.OracleAddress);
                }
            }
            catch (Exception ex)
            {
                Log2.Trace("Exception: " + ex.ToString());

            }
            Log2.Trace("Undock Event completed");


            HasDockedOracleFlag = false;
        }




        public async Task PingOracle()
        {
            //if ((!IsConfiguredFlag) || (!HasDockedOracleFlag))
            //    return;
            if (!IsConfiguredFlag)
                return;

            //Convert to Nethereum var types
            var contractAddress = EthereumContractAddress;
            var externalAddress = EthereumExternalAddress;
            var privateKey = EthereumExternalPrivateKey;

            try
            {
                var account = new Account(privateKey);
                var web3 = new Web3(account, EthereumNodeURL);
                var contract = web3.Eth.GetContract(ContractABI, contractAddress);

                EthereumOracleSequenceNumber++;
                // Get contract properties
                Task<bool> hasDockedOracle = contract.GetFunction("HasDockedOracleFlag").CallAsync<bool>();
                hasDockedOracle.Wait();
                bool myhasDockedOracle = (bool)hasDockedOracle.Result;
                Log2.Trace("HasDockedDataPump = " + myhasDockedOracle);

                if (!myhasDockedOracle)
                {
                    Log2.Trace("Warning Doesn't have Oracle. HasDockedOracleFlag = " + myhasDockedOracle);
                   // return;
                }

                //Task <BigInteger> seqNumber = contract.GetFunction("_seqNumber").CallAsync<BigInteger>();
                //seqNumber.Wait();
                //string mySeqNumber = seqNumber.Result.ToString();
                //Log2.Trace("Ping: seqNumber = " + mySeqNumber);

                // Call a contract function
                //ADD RETURN VALUE
                EthereumOracleSequenceNumber++;
                uint myseq = EthereumOracleSequenceNumber;
                HexBigInteger gas = new HexBigInteger(new BigInteger(400000));
                HexBigInteger value = new HexBigInteger(new BigInteger(89));
                Task<string> dockFunction = contract.GetFunction("pingOracle").SendTransactionAsync(externalAddress, gas, value, EthereumOracleName, myseq);
                Log2.Trace("Pinged Oracle");

                //FILTER FOR DockOracle EVENTS

                //Get Events from Contract
                Event oraclePingEvent = contract.GetEvent("OraclePingEvent");

                Log2.Trace($"Creating filter for all events");
                var filterAll = await oraclePingEvent.CreateFilterAsync().ConfigureAwait(false);
                Log2.Trace($"-> Created {0}", filterAll.Value);

                Log2.Trace("Get all events the hard way");
                var log = await oraclePingEvent.GetFilterChanges<OraclePingEventArgs>(filterAll).ConfigureAwait(false);
                Log2.Trace($"-> Got {0}", log.Count);
                foreach (var evt in log)
                {
                    Log2.Trace("-> OraclePingEvent {0} : {1} : {2} : {3}",
                        evt.Event.OracleNameIdx.ToString(),
                        evt.Event.OracleName.ToString(),
                        evt.Event.SeqNumber.ToString(),
                        evt.Event.OracleAddress);
                }

                var pumpEventHandler = web3.Eth.GetEvent<OraclePingEventArgs>(contractAddress);
                var filterAllPumpEventsForContract = pumpEventHandler.CreateFilterInput();
                var allPumpEventsForContract2 = await pumpEventHandler.GetAllChanges(filterAllPumpEventsForContract);

                //var filterTransferEventsForContractAllReceiverAddresses = transferEventHandler.CreateFilterInput(null, new[] { receiverAddress2, receiverAddress });
                //var filterIdPumpEventsForContractAllReceiverAddress2 = await pumpEventHandler.CreateFilterAsync(filterTransferEventsForContractAllReceiverAddress2);
                //var allPumpEventsForContract2 = await pumpEventHandler.GetFilterChanges(filterAllPumpEventsForContract);

                //var filterTransferEventsForContractReceiverAddress2 = transferEventHandler.CreateFilterInput(account.Address, receiverAddress2);
                //var transferEventsForContractReceiverAddress2 = await transferEventHandler.GetAllChanges(filterTransferEventsForContractReceiverAddress2);

                Log2.Trace("Number of Events: {0}" + allPumpEventsForContract2.Count.ToString());

                foreach (var evt in allPumpEventsForContract2)
                {
                    Log2.Trace("-> OraclePingEvent {0} : {1} : {2} : {3}",
                        evt.Event.OracleNameIdx.ToString(),
                        evt.Event.OracleName.ToString(),
                        evt.Event.SeqNumber.ToString(),
                        evt.Event.OracleAddress);
                }
            }
            catch (Exception ex)
            {
                Log2.Trace("Exception: " + ex.ToString());

            }
            Log2.Trace("Ping Event completed");

        }



        /// <summary>
        /// GetEvents
        /// </summary>
        /// <returns></returns>

        public async Task GetEvents()
        {
            //if ((!IsConfiguredFlag) || (!HasDockedOracleFlag))
            //    return;
            if (!IsConfiguredFlag)
                return;

            //Convert to Nethereum var types
            var contractAddress = EthereumContractAddress;
            var externalAddress = EthereumExternalAddress;
            var privateKey = EthereumExternalPrivateKey;

            try
            {
                var account = new Account(privateKey);
                var web3 = new Web3(account, EthereumNodeURL);
                var contract = web3.Eth.GetContract(ContractABI, contractAddress);

                // Get contract properties
                Task<string> pumpAddress = contract.GetFunction("_pumpAddress").CallAsync<string>();
                pumpAddress.Wait();
                string mypumpAddress = (string)pumpAddress.Result;
                Log2.Trace("Ping: PumpAddress = " + mypumpAddress);

                Task<string> pumpName = contract.GetFunction("_pumpName").CallAsync<string>();
                pumpName.Wait();
                string mypumpName = (string)pumpName.Result;
                Log2.Trace("Ping: PumpName = " + mypumpName);

                Task<BigInteger> seqNumber = contract.GetFunction("_seqNumber").CallAsync<BigInteger>();
                seqNumber.Wait();
                string mySeqNumber = seqNumber.Result.ToString();
                Log2.Trace("Ping: seqNumber = " + mySeqNumber);

                // Call a contract function
                uint myuint = UInt32.Parse(mySeqNumber);
                HexBigInteger gas = new HexBigInteger(new BigInteger(400000));
                HexBigInteger value = new HexBigInteger(new BigInteger(89));
                Task<string> pingFunction = contract.GetFunction("SendPing").SendTransactionAsync(externalAddress, gas, value, myuint);

                Log2.Trace("Pinged contract");

                //Get Events from Contract
                Event pumpevents = contract.GetEvent("PumpEvent");

                Log2.Trace($"Creating filter for all events");
                var filterAll = await pumpevents.CreateFilterAsync().ConfigureAwait(false);
                Log2.Trace($"-> Created {filterAll.Value}");

                Log2.Trace("Get all events the hard way");
                var log = await pumpevents.GetFilterChanges<PumpEventArgs>(filterAll).ConfigureAwait(false);
                Log2.Trace($"-> Got {log.Count}");
                foreach (var evt in log)
                {
                    Log2.Trace("-> PumpEvent {0} : {1} : {2} : {3}",
                        evt.Event.PumpnameIdx.ToString(),
                        evt.Event.Pumpname.ToString(),
                        evt.Event.Seqnumber.ToString(),
                        evt.Event.ContractAddress);
                }

                var pumpEventHandler = web3.Eth.GetEvent<PumpEventArgs>(contractAddress);
                var filterAllPumpEventsForContract = pumpEventHandler.CreateFilterInput();
                var allPumpEventsForContract2 = await pumpEventHandler.GetAllChanges(filterAllPumpEventsForContract);

                //var filterTransferEventsForContractAllReceiverAddresses = transferEventHandler.CreateFilterInput(null, new[] { receiverAddress2, receiverAddress });
                //var filterIdPumpEventsForContractAllReceiverAddress2 = await pumpEventHandler.CreateFilterAsync(filterTransferEventsForContractAllReceiverAddress2);
                //var allPumpEventsForContract2 = await pumpEventHandler.GetFilterChanges(filterAllPumpEventsForContract);

                //var filterTransferEventsForContractReceiverAddress2 = transferEventHandler.CreateFilterInput(account.Address, receiverAddress2);
                //var transferEventsForContractReceiverAddress2 = await transferEventHandler.GetAllChanges(filterTransferEventsForContractReceiverAddress2);

                Log2.Trace("Number of Events: {0}" + allPumpEventsForContract2.Count.ToString());

                foreach (var evt in allPumpEventsForContract2)
                {
                    Log2.Trace("-> PumpEventHandler {0} : {1} : {2} : {3}",
                        evt.Event.PumpnameIdx.ToString(),
                        evt.Event.Pumpname.ToString(),
                        evt.Event.Seqnumber.ToString(),
                        evt.Event.ContractAddress);
                }
            }
            catch (Exception ex)
            {
                Log2.Trace("Exception: " + ex.ToString());

            }
            Log2.Trace("Pinged Event completed");
        }



        //static void Main(string[] args)
        //{
        //    //The URL endpoint for the blockchain network.
        //    string url = "HTTP://localhost:7545";

        //    //The contract address.
        //    string address = "0x345cA3e014Aaf5dcA488057592ee47305D9B3e10";

        //    //The ABI for the contract.
        //    string ABI = @"[{'constant':true,'inputs':[],'name':'candidate1','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'candidate','type':'uint256'}],'name':'castVote','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[],'name':'candidate2','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[{'name':'','type':'address'}],'name':'voted','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'view','type':'function'}]";

        //    //Creates the connecto to the network and gets an instance of the contract.
        //    Web3 web3 = new Web3(url);
        //    Contract voteContract = web3.Eth.GetContract(ABI, address);

        //    //Reads the vote count for Candidate 1 and Candidate 2
        //    Task<BigInteger> candidate1Function = voteContract.GetFunction("candidate1").CallAsync<BigInteger>();
        //    candidate1Function.Wait();
        //    int candidate1 = (int)candidate1Function.Result;
        //    Task<BigInteger> candidate2Function = voteContract.GetFunction("candidate2").CallAsync<BigInteger>();
        //    candidate2Function.Wait();
        //    int candidate2 = (int)candidate2Function.Result;
        //    Console.WriteLine("Candidate 1 votes: {0}", candidate1);
        //    Console.WriteLine("Candidate 2 votes: {0}", candidate2);

        //            //Prompts for the account address.
        //            Console.Write("Enter the address of your account: ");
        //            string accountAddress = Console.ReadLine();

        //            //Prompts for the users vote.
        //            int vote = 0;
        //            Console.Write("Press 1 to vote for candidate 1, Press 2 to vote for candidate 2: ");
        //            Int32.TryParse(Convert.ToChar(Console.Read()).ToString(), out vote);
        //            Console.WriteLine("You pressed {0}", vote);

        //            //Executes the vote on the contract.
        //            try
        //            {
        //                HexBigInteger gas = new HexBigInteger(new BigInteger(400000));
        //                HexBigInteger value = new HexBigInteger(new BigInteger(0));
        //                Task<string> castVoteFunction = voteContract.GetFunction("castVote").SendTransactionAsync(accountAddress, gas, value, vote);
        //                castVoteFunction.Wait();
        //                Console.WriteLine("Vote Cast!");
        //            }
        //            catch (Exception e)
        //            {
        //                Console.WriteLine("Error: {0}", e.Message);
        //            }
        //        }
        //    }
        //}



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
}// End Namespace
