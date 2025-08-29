using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts;
using System.Threading;
using Upperbay.Assistant.EtherReadTestAccessor.Datapump.ContractDefinition;

namespace Upperbay.Assistant.EtherReadTestAccessor.Datapump
{
    public partial class DatapumpService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, DatapumpDeployment datapumpDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<DatapumpDeployment>().SendRequestAndWaitForReceiptAsync(datapumpDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, DatapumpDeployment datapumpDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<DatapumpDeployment>().SendRequestAsync(datapumpDeployment);
        }

        public static async Task<DatapumpService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, DatapumpDeployment datapumpDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, datapumpDeployment, cancellationTokenSource);
            return new DatapumpService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public DatapumpService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<string> PumpAddressQueryAsync(PumpAddressFunction pumpAddressFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PumpAddressFunction, string>(pumpAddressFunction, blockParameter);
        }

        
        public Task<string> PumpAddressQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PumpAddressFunction, string>(null, blockParameter);
        }

        public Task<string> ContractAddressQueryAsync(ContractAddressFunction contractAddressFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<ContractAddressFunction, string>(contractAddressFunction, blockParameter);
        }

        
        public Task<string> ContractAddressQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<ContractAddressFunction, string>(null, blockParameter);
        }

        public Task<BigInteger> PumpPingCountQueryAsync(PumpPingCountFunction pumpPingCountFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PumpPingCountFunction, BigInteger>(pumpPingCountFunction, blockParameter);
        }

        
        public Task<BigInteger> PumpPingCountQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PumpPingCountFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> GetDataRequestAsync(GetDataFunction getDataFunction)
        {
             return ContractHandler.SendRequestAsync(getDataFunction);
        }

        public Task<string> GetDataRequestAsync()
        {
             return ContractHandler.SendRequestAsync<GetDataFunction>();
        }

        public Task<TransactionReceipt> GetDataRequestAndWaitForReceiptAsync(GetDataFunction getDataFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(getDataFunction, cancellationToken);
        }

        public Task<TransactionReceipt> GetDataRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<GetDataFunction>(null, cancellationToken);
        }

        public Task<string> SendPingRequestAsync(SendPingFunction sendPingFunction)
        {
             return ContractHandler.SendRequestAsync(sendPingFunction);
        }

        public Task<TransactionReceipt> SendPingRequestAndWaitForReceiptAsync(SendPingFunction sendPingFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(sendPingFunction, cancellationToken);
        }

        public Task<string> SendPingRequestAsync(BigInteger number)
        {
            var sendPingFunction = new SendPingFunction();
                sendPingFunction.Number = number;
            
             return ContractHandler.SendRequestAsync(sendPingFunction);
        }

        public Task<TransactionReceipt> SendPingRequestAndWaitForReceiptAsync(BigInteger number, CancellationTokenSource cancellationToken = null)
        {
            var sendPingFunction = new SendPingFunction();
                sendPingFunction.Number = number;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(sendPingFunction, cancellationToken);
        }

        public Task<string> SetTagsRequestAsync(SetTagsFunction setTagsFunction)
        {
             return ContractHandler.SendRequestAsync(setTagsFunction);
        }

        public Task<TransactionReceipt> SetTagsRequestAndWaitForReceiptAsync(SetTagsFunction setTagsFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setTagsFunction, cancellationToken);
        }

        public Task<string> SetTagsRequestAsync(string tagName)
        {
            var setTagsFunction = new SetTagsFunction();
                setTagsFunction.TagName = tagName;
            
             return ContractHandler.SendRequestAsync(setTagsFunction);
        }

        public Task<TransactionReceipt> SetTagsRequestAndWaitForReceiptAsync(string tagName, CancellationTokenSource cancellationToken = null)
        {
            var setTagsFunction = new SetTagsFunction();
                setTagsFunction.TagName = tagName;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setTagsFunction, cancellationToken);
        }

        public Task<BigInteger> TagCountQueryAsync(TagCountFunction tagCountFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TagCountFunction, BigInteger>(tagCountFunction, blockParameter);
        }

        
        public Task<BigInteger> TagCountQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TagCountFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> PumpNameQueryAsync(PumpNameFunction pumpNameFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PumpNameFunction, string>(pumpNameFunction, blockParameter);
        }

        
        public Task<string> PumpNameQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PumpNameFunction, string>(null, blockParameter);
        }

        public Task<TagsOutputDTO> TagsQueryAsync(TagsFunction tagsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<TagsFunction, TagsOutputDTO>(tagsFunction, blockParameter);
        }

        public Task<TagsOutputDTO> TagsQueryAsync(BigInteger returnValue1, BlockParameter blockParameter = null)
        {
            var tagsFunction = new TagsFunction();
                tagsFunction.ReturnValue1 = returnValue1;
            
            return ContractHandler.QueryDeserializingToObjectAsync<TagsFunction, TagsOutputDTO>(tagsFunction, blockParameter);
        }
    }
}
