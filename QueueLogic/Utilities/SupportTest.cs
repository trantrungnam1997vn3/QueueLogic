using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace QueueLogic.Utilities
{
    public class SupportTest
    {
        private void LongRunningMethod(object monitorSync)
        {
            lock(monitorSync)
            {
                Thread.Sleep(3000);
                Monitor.Pulse(monitorSync);
            }
        }

        public bool ImpatientMethod()
        {
            Action<object> longMethod = LongRunningMethod;
            object monitorSync = new object();
            bool timeOut;
            lock(monitorSync)
            {
                longMethod.BeginInvoke(monitorSync, null, null);
                timeOut = !Monitor.Wait(monitorSync, 30);
            }
            if(timeOut)
            {
                return false;
            }
            return true;
        }
    }
}