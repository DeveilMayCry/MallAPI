using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Redis
{
    public abstract class Redis
    {

        private ConnectionMultiplexer _connection;

        public abstract string RedisConnectStr { get; }


        public ConnectionMultiplexer redisConnection
        {
            get
            {
                if (_connection is null || _connection.IsConnected)
                {
                    _connection =  ConnectionMultiplexer.Connect(RedisConnectStr);
                }
                return _connection;
            }
        }


        public IDatabase Db => redisConnection.GetDatabase();

        public bool SetString(string key, string value, TimeSpan? timeSpan)
        {
            return Db.StringSet(key, value, timeSpan);
        }

        /// <summary>
        /// 将对象序列化后保存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public bool SetString(string key, object value, TimeSpan? timeSpan)
        {
            var jsonStr = JsonConvert.SerializeObject(value);
            return Db.StringSet(key, jsonStr, timeSpan);
        }

        public string GetString(string key)
        {
            return Db.StringGet(key);
        }

        /// <summary>
        /// 从redis取值，若无值则执行func取值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public string GetStringCache(string key, Func<string> func)
        {
            if (Db.KeyExists(key))
            {
                return Db.StringGet(key);
            }
            else
            {
                return func();
            }
        }

        public T GetStringCache<T>(string key,Func<T> func)
        {
            if(Db.KeyExists(key))
            {
                return GetObject<T>(key);
            }else
            {
                return func();
            }
        }

        /// <summary>
        /// 将redis中的字符串值转换为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetObject<T>(string key)
        {
            string value = Db.StringGet(key);
            return JsonConvert.DeserializeObject<T>(key);
        }

    }
}
