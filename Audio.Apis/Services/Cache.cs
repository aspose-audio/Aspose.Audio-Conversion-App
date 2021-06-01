using System;
using System.Collections.Generic;
using System.Text;

namespace AudioVedio.Apis.Services
{
    public static class Cache
    {
        private static object cacheLocker = new object();
        private static ICache cache = null;
        static Cache()
        {
            Load();
        }

        private static void Load()
        {
            try
            {
                cache = new Redis();
            }
            catch (Exception)
            {
                //Log.Error(ex.Message);
            }
        }
        public static ICache GetCache()
        {
            return cache;
        }
        public static int TimeOut
        {
            get
            {
                return cache.TimeOut;
            }
            set
            {
                lock (cacheLocker)
                {
                    cache.TimeOut = value;
                }
            }
        }
        public static object Get(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return null;
            return cache.Get(key);
        }
        public static T Get<T>(string key)
        {
            return cache.Get<T>(key);
        }
        public static void Insert(string key, object data)
        {
            if (string.IsNullOrWhiteSpace(key) || data == null)
                return;
            //lock (cacheLocker)
            {
                cache.Insert(key, data);
            }
        }
        public static void Insert<T>(string key, T data)
        {
            if (string.IsNullOrWhiteSpace(key) || data == null)
                return;
            //lock (cacheLocker)
            {
                cache.Insert<T>(key, data);
            }
        }
        public static void Insert(string key, object data, int cacheTime)
        {
            if (!string.IsNullOrWhiteSpace(key) && data != null)
            {
                //lock (cacheLocker)
                {
                    cache.Insert(key, data, cacheTime);
                }
            }
        }
        public static void Insert<T>(string key, T data, int cacheTime)
        {
            if (!string.IsNullOrWhiteSpace(key) && data != null)
            {
                //lock (cacheLocker)
                {
                    cache.Insert<T>(key, data, cacheTime);
                }
            }
        }
        public static void Insert(string key, object data, DateTime cacheTime)
        {
            if (!string.IsNullOrWhiteSpace(key) && data != null)
            {
                //lock (cacheLocker)
                {
                    cache.Insert(key, data, cacheTime);
                }
            }
        }
        public static void Insert<T>(string key, T data, DateTime cacheTime)
        {
            if (!string.IsNullOrWhiteSpace(key) && data != null)
            {
                //lock (cacheLocker)
                {
                    cache.Insert<T>(key, data, cacheTime);
                }
            }
        }
        public static void Remove(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return;
            lock (cacheLocker)
            {
                cache.Remove(key);
            }
        }
        public static bool Exists(string key)
        {
            return cache.Exists(key);
        }
    }
}
