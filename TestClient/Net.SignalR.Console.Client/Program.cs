using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using SignalR.Client;
using SignalR.Client.Hubs;
namespace Net.SignalR.Console.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var hubConnection = new HubConnection("http://localhost:8081/");

            var tempHub = hubConnection.CreateProxy("DDPStreamWriter");
            tempHub.On("flush", message => System.Console.WriteLine(message.prodName));
            hubConnection.Start().Wait();
            tempHub.Invoke("Subscribe", "allproducts","product");
            System.Console.Read();
        }
    }
}
