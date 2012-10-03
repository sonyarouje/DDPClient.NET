using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Net.DDP.Client;
namespace Net.DDP.SignalR
{
    public class DataSubscriber:IDataSubscriber
    {
        public void DataReceived(dynamic data)
        {
            DDPStream.GetLastInstance().Publish(data);
        }
    }
}
