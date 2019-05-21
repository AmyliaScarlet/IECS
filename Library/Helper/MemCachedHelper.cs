using System;
using System.Configuration;
using System.Net;
using System.Reflection;
using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;

namespace IECS.Library
{
    /// <summary>
    /// 
    /// </summary>
    public class MemCachedHelper
    {
        static readonly object P_LockMemClient = new object();

        /// <summary>  </summary>
        public static MemcachedClient MemClient;
        /// <summary>前缀，用以区分不同的应用</summary>
        public static string MemPrefix;

        /// <summary>
        /// 线程安全的单例模式
        /// </summary>
        /// <returns></returns>
        public static MemcachedClient getInstance()
        {
            if (MemClient == null)
            {
                lock (P_LockMemClient)
                {
                    if (MemClient == null)
                    {
                        MemClientInit();
                    }
                }
            }
            return MemClient;
        }

        static void MemClientInit()
        {
            try
            {
                MemPrefix = (ConfigurationManager.AppSettings["MemCachedPrefix"] == null ? "" : ConfigurationManager.AppSettings["MemCachedPrefix"].ToString());
                string _sMemCachedIPPorts = (ConfigurationManager.AppSettings["MemCachedIPPorts"] == null ? "" : ConfigurationManager.AppSettings["MemCachedIPPorts"].ToString());
                string _sMemCachedUserName = (ConfigurationManager.AppSettings["MemCachedUserName"] == null ? "" : ConfigurationManager.AppSettings["MemCachedUserName"].ToString());
                string _sMemCachedPassword = (ConfigurationManager.AppSettings["MemCachedPassword"] == null ? "" : ConfigurationManager.AppSettings["MemCachedPassword"].ToString());

                MemcachedClientConfiguration config = new MemcachedClientConfiguration();//创建配置参数
                string[] _arrMemCachedIPPorts = _sMemCachedIPPorts.Split(';');
                for(int i=0; i<= _arrMemCachedIPPorts.Length; i++)
                {
                    string _sMemCachedIP = _arrMemCachedIPPorts[i].Split(',')[0];
                    int _nMemCachedPort = int.Parse(_arrMemCachedIPPorts[i].Split(',')[1]);
                    config.Servers.Add(new IPEndPoint(IPAddress.Parse(_sMemCachedIP), _nMemCachedPort));//增加服务节点

                }
                config.Protocol = MemcachedProtocol.Text;
                config.Authentication.Type = typeof(PlainTextAuthenticator);//设置验证模式
                config.Authentication.Parameters["userName"] = _sMemCachedUserName;//用户名参数
                config.Authentication.Parameters["password"] = _sMemCachedPassword;//密码参数
                MemClient = new MemcachedClient(config);//创建客户端
            }
            catch (Exception ex)
            {
                LogHelper.LOG("MemClient", MethodBase.GetCurrentMethod().Name, "", "", "", ex.Source.ToString(), ex.Message.ToString());
            }
        }

        /// <summary>
        /// 插入指定值
        /// </summary>
        /// <param name="key">缓存名称 </param>
        /// <param name="value">缓存值</param>
        /// <returns>返回是否成功</returns>
        public bool Insert(string key, string value)
        {
            MemcachedClient mc = getInstance();
            var data = mc.Get(key);

            if (data == null)
                return mc.Store(StoreMode.Add, MemPrefix + key, value);
            else
                return mc.Store(StoreMode.Replace, MemPrefix + key, value);
        }

        /// <summary>
        /// 插入指定值(过期时间)
        /// </summary>
        /// <param name="key">缓存名称 </param>
        /// <param name="value">缓存值</param>
        /// <param name="minutes">过期时间(分钟)，10080 一个礼拜</param>
        /// <returns>返回是否成功</returns>
        public bool Insert(string key, string value, int minutes)
        {
            MemcachedClient mc = getInstance();
            var data = mc.Get(key);

            DateTime dateTime = DateTime.Now.AddMinutes(1);
            if (data == null)
                return mc.Store(StoreMode.Add, MemPrefix + key, value, dateTime);
            else
                return mc.Store(StoreMode.Replace, MemPrefix + key, value, dateTime);
        }

        /// <summary>
        /// 获取缓存值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(string key)
        {
            MemcachedClient mc = getInstance();
            return mc.Get(MemPrefix + key);
        }
        /// <summary>
        /// 是否存在该缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static bool Exists(string key)
        {
            MemcachedClient mc = getInstance();
            return mc.Get(MemPrefix + key) != null;
        }

        /// <summary>
        /// 删除指定缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            MemcachedClient mc = getInstance();
            return mc.Remove(MemPrefix + key);
        }

        /// <summary>
        /// 清空缓存服务器上的缓存
        /// </summary>
        public void FlushCache()
        {
            MemcachedClient mc = getInstance();
            mc.FlushAll();
        }
        
    }


    /*
// 一.如果用官方提供的方法，在web.config里面配置好了各个参数和服务器IP
//<?xml version="1.0"?>
//<configuration>
//  <configSections>   
//     <sectionGroup name="enyim.com">
//      <section name="memcached" type="Enyim.Caching.Configuration.MemcachedClientSection, Enyim.Caching"/>
//    </sectionGroup>
//    <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler,log4net" /> 
//  </configSections>
// <enyim.com> 
//    <memcached protocol="Text">
//      <servers> 
//        <add address="127.0.0.1" port="11211" />
//        <add address="127.0.0.1" port="11212" />
//        <add address="127.0.0.1" port="11213" />
//        <add address="127.0.0.1" port="11214" />
//      </servers>
//      <socketPool minPoolSize="10" maxPoolSize="100" connectionTimeout="00:00:05" deadTimeout="00:02:00" />
//    </memcached>
//  </enyim.com> 
//  <system.web>
//    <compilation debug="true" targetFramework="4.0"/>
//  </system.web>
//</configuration>


/// <summary>
///通用类，组件自动调用web.config里面的配置
/// </summary>
public abstract class MemCachedHelper
{
    public MemCachedHelper()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }

    #region 添加缓存
    /// <summary>
    /// 添加缓存(键不存在则添加，存在则替换)
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    /// <returns></returns>
    public static bool AddCache(string key, object value)
    {
        using (MemcachedClient mc = new MemcachedClient())
        {
            return mc.Store(StoreMode.Set, key, value);
        }
    }
    #endregion

    #region 添加缓存(缓存时间)
    /// <summary>
    /// 添加缓存(键不存在则添加，存在则替换)
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    /// <param name="minutes">缓存时间(分钟)</param>
    /// <returns></returns>
    public static bool AddCache(string key, object value, int minutes)
    {
        using (MemcachedClient mc = new MemcachedClient())
        {
            return mc.Store(StoreMode.Set, key, value, DateTime.Now.AddMinutes(minutes));
        }
    }
    #endregion

    #region 获取缓存
    /// <summary>
    /// 获取缓存
    /// </summary>
    /// <param name="key">键</param>
    /// <returns>返回缓存，没有找到则返回null</returns>
    public static object GetCache(string key)
    {
        using (MemcachedClient mc = new MemcachedClient())
        {
            return mc.Get(key);
        }
    }
    #endregion

    #region 是否存在该缓存
    /// <summary>
    /// 是否存在该缓存
    /// </summary>
    /// <param name="key">键</param>
    /// <returns></returns>
    public static bool IsExists(string key)
    {
        using (MemcachedClient mc = new MemcachedClient())
        {
            return mc.Get(key) != null;
        }
    }
    #endregion

    #region 删除缓存(如果键不存在，则返回false)
    /// <summary>
    /// 删除缓存(如果键不存在，则返回false)
    /// </summary>
    /// <param name="key">键</param>
    /// <returns>成功:true失败:false</returns>
    public static bool DelCache(string key)
    {
        using (MemcachedClient mc = new MemcachedClient())
        {
            return mc.Remove(key);
        }
    }
    #endregion

    #region 清空缓存
    /// <summary>
    /// 清空缓存
    /// </summary>
    public static void FlushCache()
    {
        using (MemcachedClient mc = new MemcachedClient())
        {
            mc.FlushAll();
        }
    }
    #endregion
}


//二.如果不想在web.config配置，那就使用下面的通用类。

/// <summary>
/// MemberHelper 的摘要说明
/// </summary>
public abstract class MemberHelper
{
    public MemberHelper()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }

    #region 创建Memcache客户端
    /// <summary>
    /// 创建Memcache客户端
    /// </summary>
    /// <param name="serverList">服务列表</param>
    /// <returns></returns>
    private static MemcachedClient CreateServer(List<IPEndPoint> serverList)
    {
        MemcachedClientConfiguration config = new MemcachedClientConfiguration();//创建配置参数
        for (int i = 0; i < serverList.Count; i++)
        {
            config.Servers.Add(new System.Net.IPEndPoint(IPAddress.Parse(serverList[i].Address.ToString()), serverList[i].Port));//增加服务节点
        }
        config.Protocol = MemcachedProtocol.Text;
        config.Authentication.Type = typeof(PlainTextAuthenticator);//设置验证模式
        config.Authentication.Parameters["userName"] = "uid";//用户名参数
        config.Authentication.Parameters["password"] = "pwd";//密码参数
        MemcachedClient mac = new MemcachedClient(config);//创建客户端
        return mac;
    }
    #endregion

    #region 添加缓存
    /// <summary>
    /// 添加缓存(键不存在则添加，存在则替换)
    /// </summary>
    /// <param name="serverList">服务器列表</param>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    /// <returns></returns>
    public static bool AddCache(List<IPEndPoint> serverList, string key, object value)
    {
        using (MemcachedClient mc = CreateServer(serverList))
        {
            return mc.Store(StoreMode.Set, key, value);
        }
    }
    #endregion

    #region 添加缓存
    /// <summary>
    /// 添加缓存(键不存在则添加，存在则替换)
    /// </summary>
    /// <param name="serverList">服务器列表</param>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    /// <param name="minutes">缓存时间(分钟)</param>
    /// <returns></returns>
    public static bool AddCache(List<IPEndPoint> serverList, string key, object value, int minutes)
    {
        using (MemcachedClient mc = CreateServer(serverList))
        {
            return mc.Store(StoreMode.Set, key, value, DateTime.Now.AddMinutes(minutes));
        }
    }
    #endregion

    #region 获取缓存
    /// <summary>
    /// 获取缓存
    /// </summary>
    /// <param name="serverList">服务器列表</param>
    /// <param name="key">键</param>
    /// <returns>返回缓存，没有找到则返回null</returns>
    public static object GetCache(List<IPEndPoint> serverList, string key)
    {
        using (MemcachedClient mc = CreateServer(serverList))
        {
            return mc.Get(key);
        }
    }
    #endregion

    #region 是否存在该缓存
    /// <summary>
    /// 是否存在该缓存
    /// </summary>
    /// <param name="serverList">服务器列表</param>
    /// <param name="key">键</param>
    /// <returns></returns>
    public static bool IsExists(List<IPEndPoint> serverList, string key)
    {
        using (MemcachedClient mc = CreateServer(serverList))
        {
            return mc.Get(key) != null;
        }
    }
    #endregion

    #region 删除缓存(如果键不存在，则返回false)
    /// <summary>
    /// 删除缓存(如果键不存在，则返回false)
    /// </summary>
    /// <param name="serverList">服务器列表</param>
    /// <param name="key">键</param>
    /// <returns>成功:true失败:false</returns>
    public static bool DelCache(List<IPEndPoint> serverList, string key)
    {
        using (MemcachedClient mc = CreateServer(serverList))
        {
            return mc.Remove(key);
        }
    }
    #endregion

    #region 清空缓存
    /// <summary>
    /// 清空缓存
    /// </summary>
    /// <param name="serverList">服务器列表</param>
    public static void FlushCache(List<IPEndPoint> serverList)
    {
        using (MemcachedClient mc = CreateServer(serverList))
        {
            mc.FlushAll();
        }
    }
    #endregion
}
*/

}
