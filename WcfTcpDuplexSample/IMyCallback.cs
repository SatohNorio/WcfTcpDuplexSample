using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Gsf.Samples.WCF
{
    public interface IMyCallback
    {
        [OperationContract(IsOneWay=true)]
        void SendData(string name);
    }
}
