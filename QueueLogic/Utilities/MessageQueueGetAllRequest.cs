using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace QueueLogic.Utilities
{
    public class MessageQueueGetAllRequest<T>
    {
        private BlockingCollection<T> _msgQueue;
        private static Action<T> _processMsg;
        private Task _runUpdateDynamicData;
        private static CancellationTokenSource _tokenSource;
        private static CancellationToken _ct;

        private MessageQueueGetAllRequest<Object> messageQueueGetAllRequest;
        public static MessageQueueGetAllRequest<Object> Instance
        {
            get
            {
                if (Instance == null)
                {
                    return new MessageQueueGetAllRequest<Object>();
                }
                return Instance;
            }
        }

        public MessageQueueGetAllRequest()
        {
            //_processMsg = processMsg;
            //_msgQueue = new Queue<T>();
            _tokenSource = new CancellationTokenSource();
            _ct = _tokenSource.Token;
            _runUpdateDynamicData = new Task(ProcessMsgInQueue, _ct);
            _runUpdateDynamicData.Start();

        }

        public MessageQueueGetAllRequest(Action<T> processMsg)
        {
            //_processMsg = processMsg;
            _tokenSource = new CancellationTokenSource();
            _ct = _tokenSource.Token;
            _runUpdateDynamicData = new Task(ProcessMsgInQueue, _ct);
            _runUpdateDynamicData.Start();
        }

        private static readonly object _token = new object();
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