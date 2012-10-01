using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebSocket4Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace Net.DDP.Client
{
    internal class DDPConnector
    {
        private WebSocket _socket;
        private string _url=string.Empty;
        private int _isWait = 0;
        private IClient _client;

        public DDPConnector(IClient client)
        {
            this._client = client;
        }

        public void Connect(string url)
        {
            _url = "ws://" + url + "/websocket";
            _socket = new WebSocket(_url);
            _socket.MessageReceived += new EventHandler<MessageReceivedEventArgs>(socket_MessageReceived);
            _socket.Opened += new EventHandler(_socket_Opened);
            _socket.Open();
            _isWait = 1;
            this.Wait();
        }

        public void Close()
        {
            _socket.Close();
        }

        public void Send(string message)
        {
            _socket.Send(message);
        }

        void _socket_Opened(object sender, EventArgs e)
        {
            this.Send("{\"msg\":\"connect\"}");
            _isWait = 0;
        }

        void socket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            this._client.AddItem(e.Message);
        }

        private void Wait()
        {
            while (_isWait != 0)
            {
                System.Threading.Thread.Sleep(100);
            }
        }

    }
}
