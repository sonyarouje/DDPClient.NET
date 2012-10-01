using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Net.DDP.Client;
namespace Net.DDP.SignalR
{
    internal static class SingletonDDPClient
    {
        private static DDP.Client.DDPClient _client;


        public static void Connect(string url, IDataSubscriber dataSubscriber)
        {
            if (_client == null)
                _client = new DDPClient(dataSubscriber);

            _client.Connect(url);
        }

        public static void Subscribe(string subscriberTo, params string[] args)
        {
            _client.Subscribe(subscriberTo, args);
        }

        public static void Call(string methodName, params string[] args)
        {
            _client.Call(methodName, args);
        }
    }
}
