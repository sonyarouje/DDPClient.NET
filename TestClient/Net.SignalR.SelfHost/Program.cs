using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using SignalR.Hosting.Self;
using SignalR.Hubs;
namespace Net.SignalR.SelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://localhost:8081/";

            var server = new Server(url);
            server.MapHubs();
            
            server.Start();
            Console.WriteLine("SignalR server started at http://localhost:8081/");

            while (true)
            {
                ConsoleKeyInfo ki = Console.ReadKey(true);
                if (ki.Key == ConsoleKey.X)
                {
                    break;
                }
                if (ki.Key == ConsoleKey.A)
                {
                    CollectionHub.LastInstance().Notify("Static Notification", "Product");
                }
            }
        }

        public class CollectionHub : Hub
        {
            private static int instanceCnt = 0;
            private static CollectionHub _lastInstance;
            public CollectionHub()
            {
                instanceCnt++;
                _lastInstance = this;
            }

            public static CollectionHub LastInstance()
            {
                return _lastInstance;
            }
            public void Subscribe(string collectionName)
            {
                Groups.Add(Context.ConnectionId, collectionName);
                Console.WriteLine("Subscribed to: " + collectionName);
            }

            public Task Unsubscribe(string collectionName)
            {
                return Clients[collectionName].leave(Context.ConnectionId);
            }

            public void Notify(string message, string collection)
            {
                Clients[collection].publish("SignalR Processed: " + message);
                //Clients.publish("SignalR Processed: " + message);
            }

            public void NotifyDynamic(dynamic data)
            {
                string collection = Convert.ToString(data.collection);
                Clients[collection].publish(data);
            }
        }

    }
}
