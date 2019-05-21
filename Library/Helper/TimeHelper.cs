using System;


namespace IECS.Library
{

    /// <summary>
    /// 时间戳
    /// </summary>
    public class TimeHelper
    {
        /// <summary> 缺省时间 </summary>
        public static readonly DateTime DATETIME_DEFAULT = new DateTime(1900, 1, 1);
        /// <summary> 最小时间 </summary>
        public static readonly DateTime DATETIME_MINIMUM = new DateTime(1900, 1, 1);
        /// <summary> 最大时间 </summary>
        public static readonly DateTime DATETIME_MAXIMUM = new DateTime(2899, 12, 31);

        /// <summary>
        /// 获取时间戳(字符)
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp(DateTime datTime, long lUTC = 28800)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            long _lTS = (datTime.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            _lTS = _lTS - lUTC;
            if (_lTS >= 1000)
            {
                return _lTS.ToString().Substring(0, _lTS.ToString().Length - 3);
            }
            else
            {
                return _lTS.ToString();
            }
        }
        /// <summary>
        /// 获取时间戳(数字)
        /// </summary>
        /// <returns></returns>
        public static int GetTimeStamp2(DateTime datTime, long lUTC = 28800)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            long _lTS = (datTime.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            _lTS = _lTS - lUTC;
            if (_lTS >= 1000)
            {
                return Int32.Parse(_lTS.ToString().Substring(0, _lTS.ToString().Length - 3));
            }
            else
            {
                return Int32.Parse(_lTS.ToString());
            }
        }

        ///// <summary>        
        ///// 时间戳转为C#格式时间        
        ///// </summary>        
        ///// <param name=”timeStamp”></param>        
        ///// <returns></returns>        
        //public static DateTime GetDateTime(string timeStamp)
        //{
        //    DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        //    long lTime = long.Parse(timeStamp + "0000");
        //    TimeSpan toNow = new TimeSpan(lTime);
        //    return dtStart.Add(toNow);
        //}

        /// <summary>
        /// 时间戳转为C#格式时间10位
        /// </summary>
        /// <param name="lTimeStamp">Unix时间戳格式</param>
        /// <param name="lUTC">Unix时间偏移</param>
        /// <returns>C#格式时间</returns>
        public static DateTime GetDateTime(long lTimeStamp, long lUTC = 28800)
        {
            DateTime dtStart = new DateTime(1970, 1, 1);
            //DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return dtStart.AddSeconds(lTimeStamp + lUTC);
        }

        /// <summary>
        /// 时间戳转为C#格式时间10位
        /// </summary>
        /// <param name="lTimeStamp">Unix时间戳格式</param>
        /// <param name="sFormat"></param>
        /// <param name="lUTC"></param>
        /// <returns>C#格式时间</returns>
        public static string GetDateTime(long lTimeStamp, string sFormat, long lUTC = 28800)
        {
            DateTime dtStart = new DateTime(1970, 1, 1);
            //DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return dtStart.AddSeconds(lTimeStamp + lUTC).ToString(sFormat);
        }

        /// <summary>计算两个时间的差值</summary>
        public static TimeSpan GetTimeDiff(DateTime startTime, DateTime endTime)
        {
            TimeSpan ts = endTime - startTime;
            return ts;
        }

        /// <summary>计算两个时间的差值</summary>
        public static string GetTimeDiff_Format(DateTime startTime, DateTime endTime)
        {
            string timeSpan = string.Empty;
            TimeSpan ts = endTime - startTime;
            if (ts.Days > 0)
                timeSpan += ts.Days.ToString() + "天";
            if (ts.Hours > 0)
                timeSpan += ts.Hours.ToString() + "小时";
            if (ts.Minutes > 0)
                timeSpan += ts.Minutes.ToString() + "分钟";
            if (ts.Seconds > 0)
                timeSpan += ts.Seconds.ToString() + "秒";
            return timeSpan;
        }

        ///// <summary>
        ///// 验证时间戳
        ///// </summary>
        ///// <param name="time"></param>
        ///// <param name="interval">差值（分钟）</param>
        ///// <returns></returns>
        //public static bool IsTime(long time, double interval)
        //{
        //    DateTime dt = GetDateTime(time);
        //    //取现在时间
        //    DateTime dt1 = DateTime.Now.AddMinutes(interval);
        //    DateTime dt2 = DateTime.Now.AddMinutes(interval * -1);
        //    if (dt > dt2 && dt < dt1)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        ///// <summary>
        ///// 判断时间戳是否正确（验证前8位）
        ///// </summary>
        ///// <param name="time"></param>
        ///// <returns></returns>
        //public static bool IsTime(string time)
        //{
        //    string str = GetTimeStamp(DateTime.Now);
        //    if (str.Equals(time))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
    }

}
