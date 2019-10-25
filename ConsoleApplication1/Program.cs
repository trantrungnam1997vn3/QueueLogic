using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        public int threadLimit = 0;
        public int availableThread = 0;
        public int QueueLength = 0;



        static void Main(string[] args)
        {

            //var signal = new EventWaitHandle(false, EventResetMode.AutoReset);
            //Stopwatch mywatch = new Stopwatch();

            //Console.WriteLine("Thread Pool Execution");

            //mywatch.Start();
            //ProccessWithThreadPoolMethod();

            ////ProccessWithThreadMethod();
            //mywatch.Stop();

            //Console.WriteLine("Time consumed by ProcessWithThreadPoolMethod is : " + mywatch.ElapsedTicks.ToString());
            //mywatch.Reset();
            //Console.ReadLine();

            
        }


        public void Init()
        {

        }

        public void Produce(Akshay ware)
        {
            ThreadPool.QueueUserWorkItem(
            new WaitCallback(Consume), ware);
            QueueLength++;
        }

        public void Consume(Object obj)
        {
            Console.WriteLine("Thread {0} consumes {1}",
            Thread.CurrentThread.GetHashCode(), //{0}
            ((Akshay)obj).id); //{1}
            Thread.Sleep(100);
            QueueLength--;
        }




        public static void ProccessWithThreadPoolMethod()
        {
            int nWorkers = 2;
            int nIOs = 0;
            ThreadPool.SetMaxThreads(nWorkers, nIOs);
            for (int i = 0; i < 1000; i++)
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
            Thread.Sleep(3000);
            //}
            //);

        }
    }
}
