pragma solidity >=0.4.21 <0.6.0;
contract Datapump
{
    string public _pumpName;
    address public _pumpAddress = 0x6eB314EdF6CD34a20162f89A01b839532572a58F;
    address public _contractAddress = 0x58c71DA8bD4299bdF5A1f9a93dAF0BC4d005c3B1;
    uint public _pumpPingCount = 0;
    uint public _tagCount = 0;
    mapping(uint => Tag) public _tags;
     
    struct Tag{
        uint id;
        string sname;
        uint price;
        address payable owner;
        bool purchased;
    }

    event TagRequestEvent(
    //uint id,
    string indexed tagName
    //address indexed ownerContract,
    //address payable owner
    );

    event PingEvent(
    uint indexed id,
    string indexed pumpName,
    address indexed pumpAddress
    //address indexed contractAddress
    );

    constructor() public{
        _pumpName = "Upperbay Systems DataPump";
    }

 function SendPing() public{
        // Require a valid name
       // require(bytes(pumpAddress).length > 0);
        // Require a valid price
        // Create the product
       // products[productCount] = Product(productCount, _name, _price, msg.sender, false);
        // Trigger an event
        _tagCount++;
        emit PingEvent(_tagCount, _pumpName, _pumpAddress);
    }



    function GetData() public{
        // Require a valid name
        // require(bytes(_name).length > 0);
        // // Require a valid price
        // require(_price > 0);
        // // Increment product count
        // tagCount ++;
        // // Create the product
        // products[productCount] = Product(productCount, _name, _price, msg.sender, false);
        // Trigger an event
        emit TagRequestEvent("FI100");
        _tagCount++;
        emit PingEvent(_tagCount, _pumpName, _pumpAddress);
    }



    function SetTags (string memory tagName) public{
        // Require a valid name
        require(bytes(tagName).length > 0);
        // Require a valid price
        // Increment tag count
        _tagCount++;
        // Create the product
        //products[productCount] = Product(productCount, _name, _price, msg.sender, false);
        // Trigger an event
        emit PingEvent(_tagCount, _pumpName, _pumpAddress);
    }



    // function purchaseProduct(uint _id) public payable {
    //     // Fetch the product
    //     Product memory _product = products[_id];
    //     // Fetch the owner
    //     address payable _seller = _product.owner;
    //     // Make sure the product has a valid id
    //     require(_product.id > 0 && _product.id <= productCount);
    //     // Require that there is enough Ether in the transaction
    //     require(msg.value >= _product.price);
    //     // Require that the product has not been purchased already
    //     require(!_product.purchased);
    //     // Require that the buyer is not the seller
    //     require(_seller != msg.sender);
    //     // Transfer ownership to the buyer
    //     _product.owner = msg.sender;
    //     // Mark as purchased
    //     _product.purchased = true;
    //     // Update the product
    //     products[_id] = _product;
    //     // Pay the seller by sending them Ether
    //     address(_seller).transfer(msg.value);
    //     // Trigger an event
    //     emit ProductPurchased(productCount, _product.name, _product.price, msg.sender, true);
    //     }
}