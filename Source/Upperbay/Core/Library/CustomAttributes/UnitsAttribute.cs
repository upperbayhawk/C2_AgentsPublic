using System;
using System.Collections.Generic;
using System.Text;

namespace Upperbay.Agent.Library
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class UnitsAttribute : System.Attribute
    {
        public UnitsAttribute(string unitString)
        {
            this.UnitString = unitString;
        }
        public readonly string UnitString;
    }
}





//Different Flavors of Custom Attributes
//While the syntax is always the same, attributes are compiled into a wide variety of representations. The following sections will explore the three flavors of attributes: user attributes, run time attributes, and compile time attributes. 
//User Attributes
//User attributes are the simplest form of attributes. Their meaning depends entirely on the intent of the user. Consider the following code snippet: 


//class Programmer
//{
//    [Currency ("Japanese Yen")]
//    public int Salary = 1000000;
//}
//class CurrencyAttribute : System.Attribute
//{
//    public CurrencyAttribute(string currencyName)
//    {
//        Name = currencyName;
//    }
//    public readonly string Name;
//}

//Given the exchange rate between dollar and yen, it's clear that this attribute is crucial. Remove it, and the Programmer's salary may be ambiguous. The class CurrencyAttribute defines the currency attribute itself. A nice thing about custom attributes is that they have all the power of classes. For example, a real-life attribute class might have an exchange rate. 
//But who would possibly know what this attribute means? Neither the compiler nor the runtime have any concept of currency. It's your job to discover the attributes through the GetCustomAttribute method and do something meaningful with them. The following code first gets the FieldInfo metadata element and then queries its attributes through GetCustomAttributes: 


//FieldInfo field = typeof (Programmer).GetField ("Salary");
//object [] attributes = field.GetCustomAttributes (
//    typeof (CurrencyAttribute), false);
//if (attributes != null && attributes.Length > 0)
//{
//    CurrencyAttribute currency = (CurrencyAttribute)attributes[0];
//    Console.WriteLine ("The currency for Programmer is " + currency.Name);
//}


