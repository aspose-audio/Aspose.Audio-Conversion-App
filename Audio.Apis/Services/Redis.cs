using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AudioVedio.Apis.Services
{
    public class Redis : ICache
    {
        int DEFAULT_TMEOUT = 600;

        JsonSerializerSettings jsonConfig = new JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore, NullValueHandling = NullValueHandling.Ignore };

        ConnectionMultiplexer connectionMultiplexer;
        IDatabase database;

        class CacheObject<T>
        {
            public int ExpireTime { get; set; }
            public bool ForceOutofDate { get; set; }
            public T Value { get; set; }
        }

        public Redis()
        {
            connectionMultiplexer = RedisConnectionHelp.Instance;
            database = connectionMultiplexer.GetDatabase();
        }

        public int TimeOut
        {
            get
            {
                return DEFAULT_TMEOUT;
            }
            set
            {
                DEFAULT_TMEOUT = value;
            }
        }

        public object Get(string key)
        {
            return Get<object>(key);
        }

        public T Get<T>(string key)
        {

            var cacheValue = database.StringGet(key);
            var value = default(T);
            if (!cacheValue.IsNull)
            {
                var cacheObject = JsonConvert.DeserializeObject<CacheObject<T>>(cacheValue, jsonConfig);
                
                value = cacheObject.Value;
            }

            return value;

        }

        public void Insert(string key, object data)
        {
            var currentTime = DateTime.Now;

            var jsonData = GetJsonData(data, TimeOut, false);

            database.StringSet(key, jsonData);
        }

        public void Insert(string key, object data, int cacheTime)
        {
            var currentTime = DateTime.Now;
            var timeSpan = TimeSpan.FromSeconds(cacheTime);

            var jsonData = GetJsonData(data, TimeOut, true);

            database.StringSet(key, jsonData, timeSpan);

        }

        public void Insert(string key, object data, DateTime cacheTime)
        {
            var currentTime = DateTime.Now;
            var timeSpan = cacheTime - DateTime.Now;

            var jsonData = GetJsonData(data, TimeOut, true);

            database.StringSet(key, jsonData, timeSpan);

        }

        public void Insert<T>(string key, T data)
        {

            var jsonData = GetJsonData<T>(data, TimeOut, false);

            database.StringSet(key, jsonData);

        }

        public void Insert<T>(string key, T data, int cacheTime)
        {
            var timeSpan = TimeSpan.FromSeconds(cacheTime);
            var jsonData = GetJsonData<T>(data, TimeOut, true);
            database.StringSet(key, jsonData, timeSpan);
        }

        public void Insert<T>(string key, T data, DateTime cacheTime)
        {
            var timeSpan = cacheTime - DateTime.Now;
            var jsonData = GetJsonData<T>(data, TimeOut, true);
            database.StringSet(key, jsonData, timeSpan);
        }

        public void Remove(string key)
        {
            database.KeyDelete(key);
        }

        public bool Exists(string key)
        {
            return database.KeyExists(key);
        }

        string GetJsonData(object data, int cacheTime, bool forceOutOfDate)
        {
            var cacheObject = new CacheObject<object>() { Value = data, ExpireTime = cacheTime, ForceOutofDate = forceOutOfDate };
            return JsonConvert.SerializeObject(cacheObject, jsonConfig);
        }
        string GetJsonData<T>(T data, int cacheTime, bool forceOutOfDate)
        {
            var cacheObject = new CacheObject<T>() { Value = data, ExpireTime = cacheTime, ForceOutofDate = forceOutOfDate };
            return JsonConvert.SerializeObject(cacheObject, jsonConfig);
        }
    }
}
