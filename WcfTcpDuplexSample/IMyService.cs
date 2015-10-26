using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Gsf.Samples.WCF
{
    [ServiceContract(
        Namespace="http://Gsf.Samples.WCF", 
        CallbackContract=typeof(IMyCallback))]
    public interface IMyService
    {
        [OperationContract(IsOneWay=true)]
        void Regist(string name);
    }
}
