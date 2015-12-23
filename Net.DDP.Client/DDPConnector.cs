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
        private string _url = string.Empty;
        private int _isWait = 0;
        private IClient _client;

        private bool _keepAlive;

        public DDPConnector(IClient client)
        {
            this._client = client;
        }

		public void Connect(string url, bool keepAlive = true, bool useSsl = false)
		{
			_keepAlive = keepAlive;
			if (useSsl) {
				_url = "wss://" + url + "/websocket";
			} else {
				_url = "ws://" + url + "/websocket";
			}
			_socket = new WebSocket(_url);
			_socket.MessageReceived += new EventHandler<MessageReceivedEventArgs>(_socket_MessageReceived);
			_socket.Opened += new EventHandler(_socket_Opened);
			_socket.Open();
			_isWait = 1;
			this._wait();
		}

        public WebSocketState State
        {
            get { return this._socket == null ? WebSocketState.None : _socket.State; }
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
            _socket.Send("{\"msg\": \"connect\",\"version\":\"1\",\"support\":[\"1\", \"pre1\"]}");
            _isWait = 0;
        }
        
        void _socket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            if (! _handle_Ping(e.Message))
            {
                this._client.AddItem(e.Message);
            }
        }

        bool _handle_Ping(string message)
        {
            if (_keepAlive && message.Equals("{\"msg\":\"ping\"}"))
            {
                _socket.Send("{\"msg\":\"pong\"}");
                return true;
            }
            return false;
        }
        
        private void _wait()
        {
            while (_isWait != 0)
            {
                System.Threading.Thread.Sleep(100);
            }
        }

    }
}
