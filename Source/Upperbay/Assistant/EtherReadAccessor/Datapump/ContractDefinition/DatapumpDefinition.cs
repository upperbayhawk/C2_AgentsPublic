using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts;
using System.Threading;

namespace Upperbay.Assistant.EtherReadTestAccessor.Datapump.ContractDefinition
{


    public partial class DatapumpDeployment : DatapumpDeploymentBase
    {
        public DatapumpDeployment() : base(BYTECODE) { }
        public DatapumpDeployment(string byteCode) : base(byteCode) { }
    }

    public class DatapumpDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "";
        public DatapumpDeploymentBase() : base(BYTECODE) { }
        public DatapumpDeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class PumpAddressFunction : PumpAddressFunctionBase { }

    [Function("_pumpAddress", "address")]
    public class PumpAddressFunctionBase : FunctionMessage
    {

    }

    public partial class ContractAddressFunction : ContractAddressFunctionBase { }

    [Function("_contractAddress", "address")]
    public class ContractAddressFunctionBase : FunctionMessage
    {

    }

    public partial class PumpPingCountFunction : PumpPingCountFunctionBase { }

    [Function("_pumpPingCount", "uint256")]
    public class PumpPingCountFunctionBase : FunctionMessage
    {

    }

    public partial class GetDataFunction : GetDataFunctionBase { }

    [Function("GetData")]
    public class GetDataFunctionBase : FunctionMessage
    {

    }

    public partial class SendPingFunction : SendPingFunctionBase { }

    [Function("SendPing", "uint256")]
    public class SendPingFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "number", 1)]
        public virtual BigInteger Number { get; set; }
    }

    public partial class SetTagsFunction : SetTagsFunctionBase { }

    [Function("SetTags")]
    public class SetTagsFunctionBase : FunctionMessage
    {
        [Parameter("string", "tagName", 1)]
        public virtual string TagName { get; set; }
    }

    public partial class TagCountFunction : TagCountFunctionBase { }

    [Function("_tagCount", "uint256")]
    public class TagCountFunctionBase : FunctionMessage
    {

    }

    public partial class PumpNameFunction : PumpNameFunctionBase { }

    [Function("_pumpName", "string")]
    public class PumpNameFunctionBase : FunctionMessage
    {

    }

    public partial class TagsFunction : TagsFunctionBase { }

    [Function("_tags", typeof(TagsOutputDTO))]
    public class TagsFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class TagRequestEventEventDTO : TagRequestEventEventDTOBase { }

    [Event("TagRequestEvent")]
    public class TagRequestEventEventDTOBase : IEventDTO
    {
        [Parameter("string", "tagName", 1, true )]
        public virtual string TagName { get; set; }
    }

    public partial class PingEventEventDTO : PingEventEventDTOBase { }

    [Event("PingEvent")]
    public class PingEventEventDTOBase : IEventDTO
    {
        [Parameter("uint256", "id", 1, true )]
        public virtual BigInteger Id { get; set; }
        [Parameter("string", "pumpName", 2, true )]
        public virtual string PumpName { get; set; }
        [Parameter("address", "pumpAddress", 3, true )]
        public virtual string PumpAddress { get; set; }
    }

    public partial class PumpAddressOutputDTO : PumpAddressOutputDTOBase { }

    [FunctionOutput]
    public class PumpAddressOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class ContractAddressOutputDTO : ContractAddressOutputDTOBase { }

    [FunctionOutput]
    public class ContractAddressOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class PumpPingCountOutputDTO : PumpPingCountOutputDTOBase { }

    [FunctionOutput]
    public class PumpPingCountOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }







    public partial class TagCountOutputDTO : TagCountOutputDTOBase { }

    [FunctionOutput]
    public class TagCountOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class PumpNameOutputDTO : PumpNameOutputDTOBase { }

    [FunctionOutput]
    public class PumpNameOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("string", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class TagsOutputDTO : TagsOutputDTOBase { }

    [FunctionOutput]
    public class TagsOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "id", 1)]
        public virtual BigInteger Id { get; set; }
        [Parameter("string", "sname", 2)]
        public virtual string Sname { get; set; }
        [Parameter("uint256", "price", 3)]
        public virtual BigInteger Price { get; set; }
        [Parameter("address", "owner", 4)]
        public virtual string Owner { get; set; }
        [Parameter("bool", "purchased", 5)]
        public virtual bool Purchased { get; set; }
    }
}
