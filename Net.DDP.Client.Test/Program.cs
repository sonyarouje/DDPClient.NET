using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Net.DDP.Client;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using Net.DDP.SignalR;
namespace Net.DDP.Client.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            DDPHost host = new DDPHost("http://localhost:8081/", "localhost:3000");
            host.Start();
            //IDataSubscriber subscriber = new Subscriber();
            //DDPClient client = new DDPClient(subscriber);

            //client.Connect("localhost:3000");
            //client.Subscribe("allproducts");
            //client.Call("addProduct", "code", "Passed product");
            Console.ReadLine();
        }

    }

    public class Subscriber:IDataSubscriber
    {
        private IList<String> _ids = new List<String>();

        public void DataReceived(dynamic data)
        {
            try
            {
                if (data.type == "sub")
                {
                    Console.WriteLine(data.prodCode + ": " + data.prodName + ": collection: " + data.collection);
                    _ids.Add((string)data.id);
                }
                else if (data.type == "unset")
                {
                    foreach (string item in _ids)
                    {
                        if (item == data.id)
                            Console.WriteLine("deleleted item with id: " + data.id);
                    }
                }
                else if (data.type == "method")
                    Console.WriteLine(data.result);
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
