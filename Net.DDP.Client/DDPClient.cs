using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using WebSocket4Net;

namespace Net.DDP.Client
{
    public class DDPClient:IClient
    {
        private DDPConnector _connector;
        private int _uniqueId;
        private ResultQueue _queueHandler;

        public DDPClient(IDataSubscriber subscriber)
        {
            this._connector = new DDPConnector(this);
            this._queueHandler = new ResultQueue(subscriber);
            _uniqueId = 1;
        }

        public void AddItem(string jsonItem)
        {
            _queueHandler.AddItem(jsonItem);
        }

        public void Connect(string url, bool useSsl = false)
        {
			_connector.Connect(url, useSsl: useSsl);
        }

        public void Call(string methodName, params object[] args)
        {
            _connector.Send(JsonConvert.SerializeObject(new 
                {
                    msg = "method",
                    method = methodName,
                    @params = args,
                    id = this.NextId().ToString()
                }
            ));
        }

        public int Subscribe(string subscribeTo, params object[] args)
        {
            _connector.Send(JsonConvert.SerializeObject(new 
                {
                    msg = "sub",
                    name = subscribeTo,
                    @params = args,
                    id = this.NextId().ToString()
                }
            ));
            return this.GetCurrentRequestId();
        }

        public WebSocketState State
        {
            get { return this._connector.State; }
        }

       
        private int NextId()
        {
            return _uniqueId++;
        }

        public int GetCurrentRequestId()
        {
            return _uniqueId;
        }

    }
}
