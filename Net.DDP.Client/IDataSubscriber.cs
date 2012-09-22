using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.DDP.Client
{
    public interface IDataSubscriber
    {
        void DataReceived(dynamic data);
    }
}
