using System;
using System.Configuration;
using System.Threading;
using System.Web;

namespace IECS.Library
{
    /// <summary>
    /// 缓存层 单向获取 如没有会从调用处执行获取数据的方法（编写获取数据的方法体在Data层进行） 取得数据并缓存 
    /// </summary>
    class Cache
    {
        /// <summary>标记缓存时间</summary>
        private static readonly int SignCacheTimeDefault = int.Parse(ConfigurationManager.AppSettings["SignCacheTimeDefault"].ToString());
        /// <summary>数据额外缓存时间（s）</summary>
        private static readonly int CacheTimeMore = 60;

        /// <summary>
        /// 获取带标记的缓存成员数据
        /// 标记比数据提前过期 过期后脏读数据 且异步更新数据缓存
        /// </summary>
        /// <param name="GetData"></param>
        /// <param name="sCacheKey"></param>
        /// <param name="nCacheTime"></param>
        /// <returns></returns>
        public static object GetSignCacheMemberData(Func<object> GetData, String sCacheKey, int nCacheTime = 0)
        {
            object oCacheValue = null;

            if (nCacheTime == 0)
                nCacheTime = SignCacheTimeDefault;

            //缓存标记。
            string sCacheSign = sCacheKey + "_Sign";
            var sign = Get(sCacheSign);

            //获取缓存值
            oCacheValue = Get(sCacheKey);
            if (sign != null)
                return oCacheValue; //未过期，直接返回。

            #region 锁标记
            //锁标记 返回脏数据 并更新数据 （高并发请求下的作用：队列无需一个个等待 标记一过期会直接返回脏数据并会更新数据 之后拿到的就是新数据 队列都无需等待拿数据的过程）
            #endregion
            lock (sCacheSign)
            {
                sign = Get(sCacheSign);
                if (sign != null)
                    return oCacheValue;

                Insert(sCacheSign, "1", nCacheTime);

                if (GetData != null)
                {
                    ThreadPool.QueueUserWorkItem((arg) =>
                    {
                        oCacheValue = GetData();
                        Insert(sCacheKey, oCacheValue, nCacheTime + CacheTimeMore); //数据缓存时间要设得比标记缓存时间更长 用于脏读 多的时间要确保足以完成获取数据的操作                  
                    });
                }
            }

            return oCacheValue;
        }
        

        private static object Get(String sKey)
        {
            return HttpRuntime.Cache[sKey];
        }

        private static void Insert(String sKey, object oValue,Int64 nCacheTime)
        {
            if (nCacheTime > 0)
                HttpRuntime.Cache.Insert(sKey, oValue, null, DateTime.Now.AddSeconds(nCacheTime), System.Web.Caching.Cache.NoSlidingExpiration);
            else
                HttpRuntime.Cache[sKey] = oValue;
        }


    }


}
