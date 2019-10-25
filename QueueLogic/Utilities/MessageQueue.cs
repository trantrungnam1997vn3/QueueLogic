using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace QueueLogic.Utilities
{
    public class MessageQueue<T>
    {
        private Queue<T> _msgQueue;
        private Action<T> _processMsg;
        private Task _runUpdateDynamicData;
        private CancellationTokenSource _tokenSource;
        private CancellationToken _ct;

        public MessageQueue(Action<T> processMsg)
        {
            _processMsg = processMsg;
            _msgQueue = new Queue<T>();
            _tokenSource = new CancellationTokenSource();
            _ct = _tokenSource.Token;
            _runUpdateDynamicData = new Task(ProcessMsgInQueue, _ct);
            _runUpdateDynamicData.Start();
        }

        private readonly object _token = new object();
        public void EnqueueAMessage(T msg)
        {
            lock (_token)
            {
                _msgQueue.Enqueue(msg);
                Monitor.Pulse(_token);
            }
        }

        private void ProcessMsgInQueue()
        {
            while (true)
            {
                T msg;
                lock (_token)
                {
                    if (_msgQueue.Count == 0) Monitor.Wait(_token);
                    if (_ct.IsCancellationRequested)
                        _ct.ThrowIfCancellationRequested();
                    msg = _msgQueue.Dequeue();
                }

                _processMsg(msg);
            }
        }

        public void TerminateQueue()
        {
            Monitor.PulseAll(_token);
            _tokenSource.Cancel();
        }
    }
}