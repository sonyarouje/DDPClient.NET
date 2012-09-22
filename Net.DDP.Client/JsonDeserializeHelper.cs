using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
namespace Net.DDP.Client
{
    public class JsonDeserializeHelper
    {
        private IDataSubscriber _subscriber;

        public JsonDeserializeHelper(IDataSubscriber subscriber)
        {
            this._subscriber = subscriber;
        }

        internal void Deserialize(string jsonItem)
        {
            JObject jObj = JObject.Parse(jsonItem);
            if (jObj["set"]!=null)
            {
                DynamicEntity d= this.GetData(jObj);
                d.AddData("type", "sub");
                this._subscriber.DataReceived(d);
            }
            else if (jObj["unset"]!=null)
            {
                DynamicEntity entity = new DynamicEntity();
                entity.AddData("type", "unset");
                entity.AddData("id", jObj["id"].ToString());
                this._subscriber.DataReceived(entity);
            }
            else if (jObj["result"]!=null)
            {
                DynamicEntity entity = new DynamicEntity();
                entity.AddData("type", "method");
                entity.AddData("requestingId", jObj["id"].ToString());
                entity.AddData("result", jObj["result"].ToString());
                this._subscriber.DataReceived(entity);
            }
        }

        private DynamicEntity GetData(JObject json)
        {
            DynamicEntity entity = new DynamicEntity();
            entity.AddData("id",json["id"].ToString());
            entity.AddData("collection", json["collection"].ToString());
            JObject tmp = (JObject)json["set"];
            foreach (var item in  tmp)
                entity.AddData(item.Key, item.Value.ToString());

            return entity;
        }
    }
}
