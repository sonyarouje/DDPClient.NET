using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

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

        public void Connect(string url)
        {
            _connector.Connect(url);
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

        private string CreateJSonArray(params string[] args)
        {
            if (args == null)
                return string.Empty;

            StringBuilder argumentBuilder = new StringBuilder();
            string delimiter=string.Empty;
            for (int i = 0; i < args.Length; i++)
            {
                argumentBuilder.Append(delimiter);
                argumentBuilder.Append(string.Format("\"{0}\"",args[i]));
                delimiter = ",";
            }

            return argumentBuilder.ToString();
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
