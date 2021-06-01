using System;
using System.Collections.Generic;
using System.Text;

namespace AudioVedio.Apis.Services
{
    public interface ICache
    {
        int TimeOut { set; get; }
        object Get(string key);
        T Get<T>(string key);
        void Remove(string key);
        void Insert(string key, object data);
        void Insert<T>(string key, T data);
        void Insert(string key, object data, int cacheTime);
        void Insert<T>(string key, T data, int cacheTime);
        void Insert(string key, object data, DateTime cacheTime);
        void Insert<T>(string key, T data, DateTime cacheTime);
        bool Exists(string key);

    }
}
