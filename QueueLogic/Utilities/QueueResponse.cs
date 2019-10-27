using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QueueLogic.Utilities
{
    public class QueueResponse
    {
        private static ConcurrentDictionary<int, string> queueResp = new ConcurrentDictionary<int, string>();


        public static void AddMssResponseToQueue(int id, string mss)
        {
            queueResp.TryAdd(id, "Hello" + mss);
        }

        public static void GetResultById(int id, out string result)
        {
            //Newtonsoft.Json message = { };
            result = "";
            queueResp.TryGetValue(id, out result);
        }
    }
}