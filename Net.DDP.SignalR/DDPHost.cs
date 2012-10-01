using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Net.DDP.Client;
using SignalR.Hosting.Self;
using SignalR.Hubs;
namespace Net.DDP.SignalR
{
    public class DDPHost
    {
        private string _listenToUrl;
        private string _meteorServerUrl;
        private IDataSubscriber _dataSubscriber;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listenToUrl">will be the url other .net clients will connect and get the notifications</param>
        /// <param name="meteorServerUrl">url where meteor server application is running</param>
        public DDPHost(string listenToUrl, string meteorServerUrl)
        {
            _listenToUrl = listenToUrl;
            _meteorServerUrl = meteorServerUrl;
            _dataSubscriber = new DataSubscriber();
        }

        public void Start()
        {
            var server = new Server(_listenToUrl);
            server.MapHubs();
            server.Start();
            Console.WriteLine("SignalR Server started at " + _listenToUrl);

            SingletonDDPClient.Connect(_meteorServerUrl, _dataSubscriber);
            Console.WriteLine("Connected to Meteor server at " + _meteorServerUrl);
        }
    }
}
