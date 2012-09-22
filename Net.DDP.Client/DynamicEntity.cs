using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
namespace Net.DDP.Client
{
    public class DynamicEntity:DynamicObject
    {
        Dictionary<string, object> _dictionary = new Dictionary<string, object>();

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            string name = binder.Name;
            return _dictionary.TryGetValue(name, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _dictionary[binder.Name] = value;
            return true;
        }

        internal void AddData(string key, string value )
        {
            if (this._dictionary.ContainsKey(key) == false)
                _dictionary.Add(key, value);
            else
                _dictionary[key] = value;
        }
    }
}
