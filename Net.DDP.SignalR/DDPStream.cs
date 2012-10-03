using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using System.Threading.Tasks;
using SignalR.Hubs;
using SignalR.Hosting;
namespace Net.DDP.SignalR
{
    public class DDPStream:Hub
    {
        private static DDPStream _lastListenerInstance;
        public DDPStream()
        {
            _lastListenerInstance = this;
        }

        internal static DDPStream GetLastInstance()
        {
            return _lastListenerInstance;
        }

        /// <summary>
        /// Call this function from the client if both the Meteor.Collection name and the publish name is same.
        /// For eg. Products =  new Meteor.Collection("product");
        ///     Meteor.publish("product", function(){
        ///            return Products.find();
        ///        });
        /// In the above scenario collection name is product also the publish name is also product.
        /// </summary>
        /// <param name="subscriptionName"></param>
        public void Subscribe(string subscriptionName)
        {
            Groups.Add(Context.ConnectionId, subscriptionName);
            SingletonDDPClient.Subscribe(subscriptionName);
            Console.WriteLine("Subscribed to " + subscriptionName);
        }

        /// <summary>
        /// Call this function from the client if both the Meteor.Collection name and the publish name is different.
        /// For eg. Products =  new Meteor.Collection("product");
        ///     Meteor.publish("allProducts", function(){
        ///            return Products.find();
        ///        });
        /// In the above scenario collection name is product and the publish name is allProducts.
        /// </summary>
        /// <param name="subscriptionName"></param>
        /// <param name="collectionName"></param>
        public void Subscribe(string subscriptionName, string collectionName)
        {
            Groups.Add(Context.ConnectionId, collectionName);
            SingletonDDPClient.Subscribe(subscriptionName);
            Console.WriteLine("Subscribed to " + subscriptionName);
        }

        public Task Unsubscribe(string collectionName)
        {
            return Clients[collectionName].leave(Context.ConnectionId);
        }

        public void Publish(dynamic data)
        {
            if (data.type == "sub")
            {
                string collection = Convert.ToString(data.collection);
                Clients[collection].flush(data);
            }
        }
    }
}
