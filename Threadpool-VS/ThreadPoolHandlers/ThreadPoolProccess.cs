using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;

namespace Threadpool_VS.ThreadPoolHandlers
{
    public class ThreadPoolProccess
    {
        public static void ProccessWithThreadPoolMethod()
        {
            for (int i = 0; i < 10; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(Proccess), null);
            }
        }

        public static void ProccessWithThreadMethod()
        {
            for (int i = 0; i < 10; i++)
            {
                Thread t = new Thread(Proccess);
                t.Start();
            }
        }

        public static void Proccess(Object stateInfo)
        {
            //return new ThreadStart(() =>
            //{
            Console.WriteLine(String.Format("Run at {0}", Thread.CurrentThread.ManagedThreadId));
            //}
            //);
        }


    }


}