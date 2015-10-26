using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Gsf.Samples.WCF
{
    public class MyService : IMyService
    {
        public void Regist(string name)
        {
            Console.Write("[CALL] Regist() => ");

            var context  = OperationContext.Current;
            var callback = context.GetCallbackChannel<IMyCallback>();

            Console.WriteLine("[CALLBACK] SendData();");
            callback.SendData(string.Format("{0}_Callback", name));
        }
    }
}
