using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.DDP.Client
{
    public interface IClient
    {
        void AddItem(string jsonItem);
        void Connect(string url);
        void Call(string methodName, string[] args);
        void Subscribe(string methodName, string[] args);
        int GetCurrentRequestId();
    }
}
