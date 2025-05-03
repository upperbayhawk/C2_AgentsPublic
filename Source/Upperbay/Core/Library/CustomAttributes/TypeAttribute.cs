//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
using System;

namespace Upperbay.Core.Library
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class TypeAttribute : System.Attribute
    {
        public TypeAttribute(string typeString)
        {
            this.TypeString = typeString;
        }
        public readonly string TypeString;
    }
}

