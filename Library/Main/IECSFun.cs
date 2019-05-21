using System;
using System.Reflection;


namespace IECS.Library
{
    public class IECSFun
    {
        /// <summary>
        /// 获取数据 该处不表明获取数据的操作而在Data层表明 直接使用Cache.GetSignCacheMemberData可在调用处表明
        /// </summary>
        /// <param name="sKey"></param>
        /// <param name="nCacheTime"></param>
        /// <returns></returns>
        public object GetData(String sKey, int nCacheTime = 0)
        {
            String sFunName = sKey;

            if (ExistFun(new Data(), sFunName))
                return Cache.GetSignCacheMemberData(() => { return CallFun(new Data(), sFunName); }, sKey, nCacheTime);
            else
                return null;
        }

        /// <summary>
        /// 指定位置的函数是否存在
        /// </summary>
        /// <param name="oMethodWhere">通常是函数所在类的实例</param>
        /// <param name="funName"></param>
        /// <returns></returns>
        public Boolean ExistFun(object oMethodWhere, string funName)
        {
            MethodInfo mi = null;
            mi = oMethodWhere.GetType().GetMethod(funName);
            if (mi == null)
                return false;
            else
                return true;
        }
        /// <summary>
        /// 执行指定位置的函数
        /// </summary>
        /// <param name="oMethodWhere">通常是函数所在类的实例</param>
        /// <param name="funName"></param>
        /// <param name="paramseters">需要执行的函数的参数</param>
        /// <returns></returns>
        public object CallFun(object oMethodWhere, string funName, object[] paramseters = null)
        {
            MethodInfo mi = oMethodWhere.GetType().GetMethod(funName);
            return mi.Invoke(oMethodWhere, paramseters);
        }


    }

}
