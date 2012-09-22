using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Net.DDP.Client
{
    internal class ResultQueue
    {
        private static ManualResetEvent _enqueuedEvent;
        private static Thread _workerThread;
        private Queue<string> _jsonItemsQueue;
        private string _currentJsongItem;
        private JsonDeserializeHelper _serializeHelper;

        public ResultQueue(IDataSubscriber subscriber)
        {
            this._jsonItemsQueue = new Queue<string>();
            this._serializeHelper = new JsonDeserializeHelper(subscriber);

            _enqueuedEvent = new ManualResetEvent(false);
            _workerThread = new Thread(new ThreadStart(PerformDeserilization));
            _workerThread.Start();
        }

        public void AddItem(string jsonItem)
        {
            lock (_jsonItemsQueue)
            {
                _jsonItemsQueue.Enqueue(jsonItem);
                _enqueuedEvent.Set();
            }
            RestartThread();
        }


        private bool Dequeue()
        {
            lock (_jsonItemsQueue)
            {
                if (_jsonItemsQueue.Count > 0)
                {
                    _enqueuedEvent.Reset();
                    _currentJsongItem = _jsonItemsQueue.Dequeue();
                }
                else
                {
                    return false;
                }

                return true;
            }
        }

        public void RestartThread()
        {
            if (_workerThread.ThreadState == ThreadState.Stopped)
            {
                _workerThread.Abort();
                _workerThread = new Thread(new ThreadStart(PerformDeserilization));
                _workerThread.Start();
            }
        }

        private void PerformDeserilization()
        {
            while (Dequeue())
            {
                _serializeHelper.Deserialize(_currentJsongItem);
            }
        }
    }
}
