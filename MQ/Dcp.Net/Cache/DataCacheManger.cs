﻿using System;
using System.Collections;

namespace Dcp.Net.Cache
{
    /// <summary>
    ///     表达式树委托Set缓存
    /// </summary>
    public class DataCacheManger<TValue> : AbsCacheManger<string, TValue> where TValue:class
    {
        /// <summary>
        ///     线程锁
        /// </summary>
        private static readonly object LockObject = new object();

        internal DataCacheManger(string key, Func<TValue> initCache) : base(key)
        {
            this._initCache = initCache;
        }

        // 不存在数据时，初始化操作
        private readonly Func<TValue> _initCache;

        /// <summary>
        ///     当缓存不存在时，上锁加入缓存
        /// </summary>
        protected override TValue SetCacheLock()
        {
            if (_initCache == null) { return default(TValue); }
            lock (LockObject) { if (!CacheList.ContainsKey(Key)) { CacheList.Add(Key, _initCache()); } }
            return CacheList[Key];
        }
    }

    /// <summary>
    /// 表达式树委托Set缓存
    /// </summary>
    public class DataCacheManger
    {
        /// <summary>
        /// 清除缓存
        /// </summary>
        public static void Clear()
        {
            DataCacheManger<IList>.Clear();
        }

        /// <summary>
        ///     获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="initCache">不存在数据时，初始化操作</param>
        public static TValue Cache<TValue>(string key, Func<TValue> initCache = null) where TValue : class
        {
            return new DataCacheManger<TValue>(key, initCache).GetValue();
        }
    }
}