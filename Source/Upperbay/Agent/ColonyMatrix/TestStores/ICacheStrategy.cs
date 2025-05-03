using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Upperbay.Agent.ColonyMatrix
{
    /// <summary>
    /// the interface for cache strategy.
    /// each class that is pluggable to the SAF.Cache must 
    /// implement this interface.
    /// </summary>
    public interface ICacheStrategy
    {
        void Initialize(string name);
        void AddObject(string objId, object o);
        void RemoveObject(string objId);
        object RetrieveObject(string objId);
        void UpdateObject(string objId, object o);
    }
}
