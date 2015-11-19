using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Diagnostics;

namespace Gsf.Samples.WCF
{
    public class MyService : IMyService
    {
        public void Regist(string name)
        {
            Trace.Write("[CALL] Regist() => ");

            var context  = OperationContext.Current;
            var callback = context.GetCallbackChannel<IMyCallback>();

            Trace.WriteLine("[CALLBACK] SendData();");
            callback.SendData(string.Format("{0}_Callback", name));
        }
    }
}
