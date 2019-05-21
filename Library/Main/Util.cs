
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;


using Newtonsoft.Json;
using System.Globalization;
using System.Security.Cryptography;
using System.Configuration;

namespace IECS.Library
{

    /// <summary>
    /// UtilFunc 的摘要说明
    /// </summary>
    public class Util
    {
        public Util()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        ///// <summary>
        ///// 判断字符串是否为纯数字
        ///// </summary>
        ///// <param name="str"></param>
        ///// <returns></returns>
        //public static bool IsNumber(string str)
        //{
        //    if (str == null || str.Length == 0)
        //        return false;
        //    ASCIIEncoding ascii = new ASCIIEncoding();
        //    byte[] bytestr = ascii.GetBytes(str);         //把string类型的参数保存到数组里  

        //    foreach (byte c in bytestr)                   //遍历这个数组里的内容  
        //    {
        //        if (c < 48 || c > 57)                          //判断是否为数字  
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        ///// <summary>
        ///// 获取当前日期时间 yyyy-MM-dd | yyyyMM | yyyy
        ///// </summary>
        ///// <param name="format">yyyy-MM-dd | yyyyMM | yyyy</param>
        ///// <returns></returns>
        //public static string GetDateNow(string format = "")
        //{
        //    if (format.Equals("yyyyMM") || format.Equals("yyyy"))
        //    {
        //        return DateTime.Now.Date.ToString(format);
        //    }
        //    else
        //    {
        //        return DateTime.Now.ToString(format);
        //    }
        //}

        /// <summary>
        /// Bool转Int
        /// </summary>
        /// <param name="bVal"></param>
        /// <returns></returns>
        public static int BoolToInt(bool bVal)
        {
            return bVal ? 1 : 0;          
        }

        /// <summary>
        /// 日期格式的字符串转格式化DateTime 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static T StringToDateTime<T>(String str,string format= "yyyy-MM-dd HH:mm:ss")
        {
            if (str == "")
            {
                return default(T);
            }

            if (typeof(T).ToString().ToLower().Contains("datetime"))
            {
                return (T)(object)DateTime.Parse(DateTime.Parse(str).ToString(format, new DateTimeFormatInfo() { ShortDatePattern = format }));

            }
            else if (typeof(T).ToString().ToLower().Contains("string"))
            {
                return (T)(object)DateTime.Parse(str).ToString(format, new DateTimeFormatInfo() { ShortDatePattern = format });
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// 遍历DataSet所有行 写入HashList
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static HashList DataSetToHashList(DataSet ds)
        {
            HashList _hl = new HashList();
            try
            {
                if (ds.Tables.Count > 0)
                {
                    foreach (DataTable dt in ds.Tables)
                    {
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (dr != null && dr.ItemArray.Length > 0)
                                {
                                    for (int i = 0; i < dr.ItemArray.Length; i++)
                                    {
                                        _hl[dr.Table.Columns[i].ColumnName] = dr.ItemArray[i];
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.LOG(MethodBase.GetCurrentMethod().Name + "_" + DateTime.Now.ToString("yyyyMMdd"), MethodBase.GetCurrentMethod().DeclaringType.FullName, "", "", "", "", e.StackTrace);
            }
            return _hl;
        }

        ///// <summary>
        ///// 获取当前时间戳
        ///// </summary>
        ///// <returns></returns>
        //public static int GetTimeStamp()
        //{
        //    TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        //    return int.Parse((long.Parse(Convert.ToInt64(ts.TotalMilliseconds).ToString()) / 1000).ToString());
        //}

        /// <summary>
        /// 时间戳转时间
        /// </summary>
        /// <param name="unixTimeStamp">unix时间戳</param>
        /// <param name="format">yyyy/MM/dd HH:mm:ss</param>
        /// <returns></returns>
        public static T TimeStampToDataTime<T>(int unixTimeStamp, String format = "yyyy-MM-dd HH:mm:ss")
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)); // 当地时区

            DateTime dt = startTime.AddSeconds(unixTimeStamp);

            if (typeof(T).ToString().ToLower().Contains("string"))
            {
                return (T)(object)dt.ToString(format);
            }
            else if (typeof(T).ToString().ToLower().Contains("datetime"))
            {
                return (T)(object)dt;
            }
            else
            {
                throw new ArgumentException("传入数据格式错误！");
            }

        }

        /// <summary>
        /// 字符串转Unicode
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <returns>Unicode编码后的字符串</returns>
        public static string String2Unicode(string source)
        {
            var bytes = Encoding.Unicode.GetBytes(source);
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < bytes.Length; i += 2)
            {
                stringBuilder.AppendFormat("\\u{0}{1}", bytes[i + 1].ToString("x").PadLeft(2, '0'), bytes[i].ToString("x").PadLeft(2, '0'));
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Unicode转字符串
        /// </summary>
        /// <param name="source">经过Unicode编码的字符串</param>
        /// <returns>正常字符串</returns>
        public static string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(source, x => Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)).ToString());
        }


        /// <summary>
        /// 判断是否是有效的格利高里日期
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static bool CheckDate(int year, int month, int day)
        {
            if (year < 1970 || year > 32767)
            {
                return false;
            }
            if (month < 1 || month > 12)
            {
                return false;
            }
            if (day < 1 || day > 31)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// DateTime转时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long DataTimeToTimeStamp(DateTime dateTime)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (dateTime.Ticks - startTime.Ticks) / 10000 /1000;
            return t;
        }

        /// <summary>  
        /// 获取指定日期的所在月份第一天  
        /// </summary>  
        /// <param name="dateTime"></param>  
        /// <returns></returns>  
        public static DateTime GetDateTimeMonthFirstDay(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        /// <summary>  
        /// 获取指定日期所在周的第一天，星期一为第一天 （默认星期天为第一天）
        /// </summary>  
        /// <param name="dateTime"></param>  
        /// <returns></returns>  
        public static DateTime GetDateTimeWeekFirstDayMon(DateTime dateTime)
        {
            DateTime firstWeekDay = DateTime.Now;

            try
            {
                int weeknow = Convert.ToInt32(dateTime.DayOfWeek);
                weeknow = (weeknow == 0 ? (7 - 1) : (weeknow - 1));

                int daydiff = (-1) * weeknow;

                firstWeekDay = dateTime.AddDays(daydiff);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return firstWeekDay;
        }
        /// <summary>
        /// 将数组的某一索引位置的元素移动到指定索引位置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="fromIndex">要移动的元素所在的索引位</param>
        /// <param name="toIndex">要移动到的索引位</param>
        public static void ArrayMove<T>(ref T[] array, int fromIndex, int toIndex)
        {
            if (fromIndex> array.Length-1 || toIndex > array.Length - 1 || fromIndex == toIndex) return;

            T[] tempArray = new T[array.Length];
            if (fromIndex > toIndex)
            {
                for (int i=0;i< array.Length;i++)
                {
                    if (i == toIndex)
                    {
                        tempArray[i] = array[fromIndex];
                    }
                    else
                    {
                        if (i > fromIndex || i < toIndex)
                        {
                            tempArray[i] = array[i];
                        }
                        else
                        {
                            tempArray[i] = array[i - 1];
                        }
                    }
                }
            }
            else if(fromIndex < toIndex)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    if (i == toIndex)
                    {
                        tempArray[i] = array[fromIndex];
                    }
                    else
                    {
                        if (i < fromIndex || i > toIndex)
                        {
                            tempArray[i] = array[i];
                        }
                        else
                        {
                            tempArray[i] = array[i + 1];
                        }
                    }
                }
            }
            array = tempArray; 
        }


        /// <summary>  
        /// 判断输入的字符串是否是一个合法的手机号  
        /// </summary>  
        /// <param name="input"></param>  
        /// <returns></returns>  
        public static bool IsMobile(string input)
        {
            if (Array.IndexOf(new int[] { 11, 12 }, input.Length) == -1) return false;

            string _sPattern = @"^1[345678]\d{9}$";
            Regex _regexMobile = new Regex(_sPattern);
            if (!_regexMobile.IsMatch(input)) return false;
            return true;

            //string dianxin = @"^1[3578][01379]\d{8}$";
            //Regex regexDX = new Regex(dianxin);

            //string liantong = @"^1[34578][01256]\d{8}";
            //Regex regexLT = new Regex(liantong);

            //string yidong = @"^(1[012345678]\d{8}|1[345678][012356789]\d{8})$";
            //Regex regexYD = new Regex(yidong);

            //string xianggang = @"^([569]\d{7})$";
            //Regex regexXG = new Regex(xianggang);
            //if (regexDX.IsMatch(input) || regexLT.IsMatch(input) || regexYD.IsMatch(input) || regexXG.IsMatch(input))
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }


        /// <summary>  
        /// 判断输入的字符串是否是身份证 
        /// </summary>  
        /// <param name="input"></param>  
        /// <returns></returns>  
        public static bool IsIDCard(string input)
        {

            string ChkNum18 = @"^\d{18}$";
            Regex regexChkNum18 = new Regex(ChkNum18);
            string ChkNum17 = @"^\d{17}[\d|X|x]$";
            Regex regexChkNum17 = new Regex(ChkNum17);
            string ChkNum15 = @"^\d{15}$";
            Regex regexChkNum15 = new Regex(ChkNum15);

            if (regexChkNum18.IsMatch(input) || regexChkNum17.IsMatch(input))
            {
                if (CheckIDCard18(input))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (regexChkNum15.IsMatch(input))
            {
                if (CheckIDCard15(input))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 检查18位身份证号有效性
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool CheckIDCard18(string Id)

        {
            long n = 0;
            if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证
            }

            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";

            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = Id.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
            {
                return false;//校验码验证
            }
            return true;//符合GB11643-1999标准

        }
        /// <summary>
        /// 检查15位身份证号有效性
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool CheckIDCard15(string Id)
        {
            long n = 0;
            if (long.TryParse(Id, out n) == false || n < Math.Pow(10, 14))
            {
                return false;//数字验证
            }

            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";

            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//省份验证
            }

            string birth = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time = new DateTime();

            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }

            return true;//符合15位身份证标准

        }



        public static String GetClientIP()
        {
            String _sUserIP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (_sUserIP == "" || _sUserIP == null) _sUserIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            return _sUserIP;
        }

        /// <summary>
        /// 验证访问IP的有效性
        /// </summary>
        /// <param name="_sUserIP">ip地址</param>
        /// <param name="usePage">访问页面</param>
        /// <param name="timeSpan">时间间隔</param>
        /// <param name="id">数据ID</param>
        /// <returns></returns>
        public static bool CheckIPValid(String _sUserIP, String usePage, int timeSpan = 0, int id = 0)
        {

            if (HttpContext.Current.Session[usePage + "_" + id + "_ip"] == null)
            {
                HttpContext.Current.Session[usePage + "_" + id + "_ip"] = new String[] { _sUserIP, TimeHelper.GetTimeStamp2(DateTime.Now).ToString() };
                return true;
            }
            else
            {
                String[] IPSession = (String[])HttpContext.Current.Session[usePage + "_" + id + "_ip"];
                if (_sUserIP.Equals(IPSession[0]))
                {
                    if (TimeHelper.GetTimeStamp2(DateTime.Now) - int.Parse(IPSession[1]) < timeSpan)
                    {
                        return false;
                    }
                    else
                    {
                        HttpContext.Current.Session[usePage + "_" + id + "_ip"] = new String[] { _sUserIP, TimeHelper.GetTimeStamp2(DateTime.Now).ToString() };
                        return true;
                    }

                }
                else
                {
                    HttpContext.Current.Session[usePage + "_" + id + "_ip"] = new String[] { _sUserIP, TimeHelper.GetTimeStamp2(DateTime.Now).ToString() };
                    return true;
                }
            }

        }

        /// <summary>
        /// GB2312转换成UTF8
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string gb2312_utf8(string text)
        {
            //声明字符集   
            Encoding utf8, gb2312;
            //gb2312   
            gb2312 = Encoding.GetEncoding("gb2312");
            //utf8   
            utf8 = Encoding.GetEncoding("utf-8");
            byte[] gb;
            gb = gb2312.GetBytes(text);
            gb = Encoding.Convert(gb2312, utf8, gb);
            //返回转换后的字符   
            return utf8.GetString(gb);
        }

        /// <summary>
        /// UTF8转换成GB2312
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string utf8_gb2312(string text)
        {
            //声明字符集   
            Encoding utf8, gb2312;
            //utf8   
            utf8 = Encoding.GetEncoding("utf-8");
            //gb2312   
            gb2312 = Encoding.GetEncoding("gb2312");
            byte[] utf;
            utf = utf8.GetBytes(text);
            utf = Encoding.Convert(utf8, gb2312, utf);
            //返回转换后的字符   
            return gb2312.GetString(utf);
        }

        /// <summary>
        /// 将数组[]转成字符串
        /// </summary>
        /// <param name="glue">分隔符</param>
        /// <param name="pieces">数组</param>
        public static string Implode<T>(string glue, T[] pieces)
        {
            string result = String.Empty;
            int count = pieces.Length;
            for (int i = 0; i < count; i++)
            {
                if (i == 0)
                {
                    result = pieces[i].ToString();
                }
                else
                {
                    result = result + glue.ToString() + pieces[i];
                }
            }
            return result;
        }

        /// <summary>
        /// 合并DataRow (不能有同名元素 有同名元素的 addDrData2的值将会覆盖addDrData1的值)
        /// </summary>
        /// <param name="addDrData1"></param>
        /// <param name="addDrData2"></param>
        public static DataRow DataRowMerge(DataRow addDrData1, DataRow addDrData2)
        {
            DataTable _dt = new DataTable();
            _dt = addDrData1.Table.Copy();

            foreach (DataColumn Columns in addDrData2.Table.Columns)
            {
                if (!_dt.Columns.Contains(Columns.ColumnName))
                {
                    _dt.Columns.Add(Columns.ColumnName);
                }

                _dt.Rows[0][Columns.ColumnName] = addDrData2[Columns.ColumnName];
            }

            return _dt.Rows[0];
        }

        /// <summary>
        /// DataRow转Dictionary
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static Dictionary<string, T> DataRowToDictionary<T>(DataRow dr)
        {
            Dictionary<string, T> dic = new Dictionary<string, T>();
            foreach (DataColumn Columns in dr.Table.Columns)
            {
                dic[Columns.ColumnName] = (T)(object)(dr[Columns.ColumnName].ToString());
            }

            return dic;
        }

        /// <summary>
        /// Dictionary转DataRow
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static DataRow DictionaryToDataRow<T>(Dictionary<string, T> dic)
        {
            DataTable _dt = new DataTable();
            _dt.Rows.Add();
            foreach (string KeyName in dic.Keys)
            {
                _dt.Columns.Add(KeyName);
                _dt.Rows[0][KeyName] = dic[KeyName];
            }

            return _dt.Rows[0];
        }

        /// <summary>
        /// 从数组的第start个元素开始取出，并返回数组中的len个元素
        /// </summary>
        /// <param name="array"></param>
        /// <param name="start"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static object[] ArraySlice(object[] array, int start, int len)
        {
            object[] obj = new object[len];
            int o = 0;
            for (int i = start; i < len; i++)
            {
                obj[o] = array[i];
                o++;
            }

            return obj;
        }

        /// <summary>
        /// 返回在预定义字符之前添加反斜杠的字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string addslashes(string str)
        {
            string result = Regex.Replace(str, @"\\", @"\\");
            return Regex.Replace(result, "'", "\\'");
        }

        /// <summary>
        /// 遮挡用户名称
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string CutNameStr(string str)
        {
            char[] c = str.ToCharArray();
            if (str.Length > 9)
            {
                if (c[0] >= 0x4e00 && c[0] <= 0x9fbb)
                {
                    return str.Substring(0, 1) + "****";
                }
                else
                {
                    return str.Substring(0, 5) + "****" + str.Substring(str.Length - 4, 4);
                }
            }
            else
            {
                if (c[0] >= 0x4e00 && c[0] <= 0x9fbb)
                {
                    return str.Substring(0, 1) + "****";
                }
                else
                {
                    if (str.Length > 3)
                    {
                        return str.Substring(0, 3) + "****";

                    }
                    else
                    {
                        return str.Substring(0, 1) + "****";
                    }
                }
            }

        }
        /// <summary>
        /// 遮挡手机号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string CutMobileStr(string str)
        {
            if (str.Length - 2 >= 0)
            {
                return str.Substring(0, 3) + "*******" + str.Substring(str.Length - 2, 2);
            }
            else
            {
                return str;
            }
            
        }
        /// <summary>
        /// 银行卡尾号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string CutBankStr(string str)
        {
            if (str.Length - 4 >= 0)
            {
                return str.Substring(str.Length - 4, 4);
            }
            else
            {
                return str;
            }
            
        }

        /// <summary>
        /// Hashtable转DataRow
        /// </summary>
        /// <param name="hashtable"></param>
        /// <returns></returns>
        public static DataRow HashtableToDataRow(Hashtable hashtable)
        {
            DataTable dt = new DataTable();
            dt.Rows.Add();
            foreach (object item in hashtable.Keys)
            {
                string name = item.ToString();
                if (!dt.Columns.Contains(name))
                {
                    dt.Columns.Add(name);
                }

                object value = hashtable[item];
                if (value != null)
                {
                    dt.Rows[0][name] = value;
                }
            }

            return dt.Rows[0];
        }

        /// <summary>
        /// Entity转DataRow
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="usData"></param>
        /// <returns></returns>
        public static DataRow EntityToDataRow<T>(T usData)
        {
            DataTable dt = new DataTable();
            dt.Rows.Add();
            PropertyInfo[] properties = usData.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            foreach (PropertyInfo item in properties)
            {
                string name = item.Name;
                if (!dt.Columns.Contains(name))
                {
                    dt.Columns.Add(name);
                }

                object value = item.GetValue(usData, null);
                if (value != null)
                {
                    dt.Rows[0][name] = value;
                }
            }

            return dt.Rows[0];
        }

        /// <summary>
        /// DataRow转Entity 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static T DataRowToEntity<T>(DataRow dr)
        {
            if (dr == null)
            {
                return default(T);
            }

            T model = (T)Activator.CreateInstance(typeof(T));

            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                PropertyInfo propertyInfo = model.GetType().GetProperty(dr.Table.Columns[i].ColumnName);

                if (propertyInfo != null && dr[i] != DBNull.Value)
                {
                    propertyInfo.SetValue(model, dr[i], null);
                    break;
                }
            }

            //System.Reflection.PropertyInfo[] properties = mc.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            //foreach (DataColums cName in dr.Table.Columns)
            //{
            //    foreach (System.Reflection.PropertyInfo item in properties)
            //    {
            //        string name = item.Name;
            //        object value = item.GetValue(mc, null);
            //        if (name.Equals(cName.))
            //        {
            //            item.SetValue(name, dr[cName.]);
            //            break;
            //        }
            //    }
            //}

            return model;
        }

        /// <summary>
        /// List转DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ListToDataTable<T>(List<T> list)
        {
            DataTable dt = new DataTable();
            DataRow dr = dt.NewRow();
            for (int i = 0; i < list.Count; i++)
            {    
                PropertyInfo[] properties = list[i].GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                foreach (PropertyInfo item in properties)
                {
                    string name = item.Name;
                    if (!dr.Table.Columns.Contains(name))
                    {
                        dr.Table.Columns.Add(name);
                    }

                    object value = item.GetValue(list[i], null);
                    if (value != null)
                    {
                        dr[name] = value;
                    }
                    
                }
                dt.Rows.Add(dr.ItemArray);
            }
            
            //dt.AcceptChanges();
            return dt;
        }

        /// <summary>
        /// Dictionary转DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static DataTable DictionaryToDataTable<T>(Dictionary<T, T> dic)
        {
            DataTable _dt = new DataTable();
            _dt.Columns.Add("Idx");
            _dt.Columns.Add("Key");
            _dt.Columns.Add("Value");

            int i = 0;
            foreach (T k in dic.Keys)
            {
                DataRow _dr = _dt.NewRow();

                _dr["Idx"] = i;
                _dr["Key"] = k;
                _dr["Value"] = dic[k];

                _dt.Rows.Add(_dr);
            }

            return _dt;
        }

        /// <summary>
        /// Entity[]转DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="EntityList"></param>
        /// <returns></returns>
        public static DataTable EntityToDataTable<T>(T[] EntityList)
        {
            DataTable dt = new DataTable();

            for (int i = 0; i < EntityList.Length; i++)
            {
                dt.Rows.Add();
                PropertyInfo[] properties = EntityList[i].GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                foreach (PropertyInfo item in properties)
                {
                    string name = item.Name;
                    if (!dt.Columns.Contains(name))
                    {
                        dt.Columns.Add(name);
                    }

                    object value = item.GetValue(EntityList[i], null);
                    if (value != null)
                    {
                        dt.Rows[i][name] = value;
                    }
                }
            }
            return dt;
        }




        /// <summary>
        /// 两个string[]数组 取 交集 或 差集
        /// </summary>
        /// <param name="arr1"></param>
        /// <param name="arr2"></param>
        /// <param name="isIntersected">true取交集 false取差集</param>
        /// <returns></returns>
        public static String[] StringScreen(String[] arr1, String[] arr2, bool isIntersected)
        {

            int len = arr1.Length > arr2.Length ? arr1.Length : arr2.Length;
            int o = 0;


            String[] result = new String[len];
            Dictionary<String, Boolean> map = new Dictionary<String, Boolean>();

            foreach (String str1 in arr1)
            {
                if (!map.ContainsKey(str1))
                {
                    map[str1] = false;
                }
            }

            foreach (String str2 in arr2)
            {
                if (str2 != null && !str2.Equals(""))
                {
                    if (map.ContainsKey(str2))
                    {
                        map[str2] = true;
                    }
                    else
                    {
                        map[str2] = false;
                    }
                }
            }

            foreach (String entry in map.Keys)
            {
                if (map[entry] == isIntersected)
                {
                    result[o] = entry;
                    o++;
                }
            }


            String[] res = new string[o];
            for (int r = 0; r < o; r++)
            {
                res[r] = result[r];
            }

            return res;
        }

        /// <summary>
        /// 根据DataRow执行SQL插入或更新 
        /// </summary>
        /// <param name="tableName">指定表 表名</param>
        /// <param name="fieldValues">传入的待筛选列的行数据</param>
        /// <param name="mode"></param>
        /// <param name="where"></param>
        /// <returns>INSERT返回插入ID  UPDATE返回影响行数</returns>
        //public static int SQLAutoExecute(String tableName, DataRow fieldValues, ClsEnum.EnumSQLMode mode = ClsEnum.EnumSQLMode.INSERT, String where = "")
        //{
        //int re = -1; //用于接收 插入ID（INSERT时）或 影响行数（UPDATE时）
        //string sql = ""; //sql语句
        //string keysStr = ""; //列名
        //string valuesStr = ""; //列值
        //String pKeyColumnsName = SQLGetpKey(tableName);//获取指定表的主键列名

        //DataTable dtColumns = DB.ExecSQL_GetTable("Select Name FROM SysColumns Where id=Object_Id('" + tableName + "')"); //获取指定表的所有列名

        ////将指定表的所有列名转为用string[]存储
        //string[] tableColumnsName = new string[dtColumns.Rows.Count];
        //for (int c = 0; c < dtColumns.Rows.Count; c++)
        //{
        //    tableColumnsName[c] = dtColumns.Rows[c]["Name"].ToString();
        //}
        ////将待筛选列列名转为用string[]存储
        //string[] drColumnsName = new string[fieldValues.Table.Columns.Count];
        //for (int d = 0; d < fieldValues.Table.Columns.Count; d++)
        //{
        //    drColumnsName[d] = fieldValues.Table.Columns[d].ColumnName;
        //}

        ////取以上两个string[]的交集 排除掉不属于指定表的多余字段  string[] columns即为不多不少刚好要作用到的列名
        //string[] columns = StringScreen(tableColumnsName, drColumnsName, true);


        //if (mode == ClsEnum.EnumSQLMode.INSERT)
        //{
        //    foreach (DataColumn Columns in fieldValues.Table.Columns)
        //    {
        //        //遍历查找符合的列 条件为存在于string[] columns中 且不是主键
        //        if (Array.IndexOf(columns, Columns.ColumnName) != -1 && pKeyColumnsName!= Columns.ColumnName)
        //        {
        //            //对没有设值的 数字型字段给0 其他给''
        //            if (Columns.DataType.Name.ToLower().Contains("int")  //Double Byte Boolean Decimal Single SByte   float )
        //            {
        //                if (fieldValues[Columns.ColumnName] == null)
        //                {
        //                    fieldValues[Columns.ColumnName] = 0;
        //                }
        //            }
        //            else
        //            {
        //                if (fieldValues[Columns.ColumnName] == null)
        //                {
        //                    fieldValues[Columns.ColumnName] = "''";
        //                }
        //                else
        //                {
        //                    fieldValues[Columns.ColumnName] = "'"+ fieldValues[Columns.ColumnName].ToString() + "'";
        //                }

        //            }

        //            //拼接列名和值
        //            keysStr += "," + Columns.ColumnName;
        //            valuesStr += "," + fieldValues[Columns.ColumnName].ToString();
        //        }
        //    }

        //    //去掉句首的逗号
        //    keysStr = keysStr.Substring(1, keysStr.Length - 1);
        //    valuesStr = valuesStr.Substring(1, valuesStr.Length - 1);

        //    //组建SQL语句
        //    if (fieldValues != null)
        //    {
        //        sql = "INSERT INTO " + tableName + " (" + keysStr + ") VALUES (" + valuesStr + ")";
        //    }

        //    //运行SQL语句，返回插入ID
        //    if (sql != "")
        //    {
        //        re = DB.ExecSQL_GetInsertId(sql);
        //    }
        //}
        //else if (mode == ClsEnum.EnumSQLMode.UPDATE)
        //{
        //    String setStr = "";

        //    foreach (DataColumn Columns in fieldValues.Table.Columns)
        //    {
        //        //遍历查找符合的列 条件为存在于string[] columns中 且不是主键
        //        if (Array.IndexOf(columns, Columns.ColumnName) != -1 && pKeyColumnsName != Columns.ColumnName)
        //        {
        //            //拼接列名和值 未设值的不更新
        //            if (fieldValues[Columns.ColumnName] != null)
        //            {
        //                if (Columns.DataType.Name.ToLower().Contains("int"))
        //                {
        //                    setStr += "," + Columns.ColumnName + "=" + fieldValues[Columns.ColumnName].ToString();
        //                }
        //                else
        //                {
        //                    setStr += "," + Columns.ColumnName + "='" + fieldValues[Columns.ColumnName].ToString()+"'";
        //                }
        //            }
        //        }
        //    }

        //    //去掉句首的逗号
        //    setStr = setStr.Substring(1, setStr.Length - 1);

        //    //组建SQL语句
        //    if (fieldValues != null)
        //    {
        //        sql = "UPDATE " + tableName + " SET " + setStr + " WHERE " + where;
        //    }

        //    //运行SQL语句，返回影响行数
        //    if (sql != "")
        //    {
        //        re = DB.ExecSQL_NonQuery(sql);
        //    }
        //}

        //return re;
        //}



        /// <summary>
        /// 获取指定表的主键列名
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static string SQLGetpKey(string tableName)
        {
            return SQLHelper.ExecSQL_GetValue("Declare @objectid int"+
            " Set @objectid = object_id('"+ tableName + "')"+
            " Select col_name(@objectid, colid)  '主键字段' From  sysobjects as o"+
            " Inner Join sysindexes as i On i.name = o.name"+
            " Inner Join sysindexkeys as k On k.indid = i.indid"+
            " Where o.xtype = 'PK' and parent_obj = @objectid and k.id = @objectid");
        }



        /// <summary>
        /// 根据原始数组返回指定数量的随机元素组成的数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr">原始数组</param>
        /// <param name="arrCount">指定数量</param>
        /// <returns></returns>
        public static T[] ArrayRand<T>(T[] arr, int arrCount = 1)
        {
            if (arrCount > arr.Length)
            {
                arrCount = arr.Length;
            }
            T[] re = new T[arrCount];
            Random ran = new Random();
            for (int i = 0; i < arrCount; i++)
            {
                int r = ran.Next(1, arr.Length);
                re[i] = arr[r - 1];
            }

            return re;
        }

        /// <summary>
        /// 同PHP file_get_contents
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string file_get_contents(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            WebResponse response = request.GetResponse();
            using (MemoryStream ms = new MemoryStream())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    int readc;
                    byte[] buffer = new byte[1024];
                    while ((readc = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, readc);
                    }
                }
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }


        ///// <summary>
        ///// 获取指定日期是星期几 int=>7 string=>星期天
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="timeSpan"></param>
        ///// <returns></returns>
        //public static T GetDataTimeWeek<T>(int timeSpan)
        //{
        //    string day = TimeStampToDataTime<DateTime>(timeSpan).DayOfWeek.ToString();
        //    int[] Idays = new int[] { 1, 2, 3, 4, 5, 6, 7 };
        //    string[] Cdays = new string[] { "星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期天" };
        //    string[] Edays = new string[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        //    for (int i = 0; i < Edays.Length; i++)
        //    {
        //        if (Edays[i] == day)
        //        {
        //            if (typeof(T).ToString().ToLower().Contains("string"))
        //            {
        //                return (T)(object)Cdays[i];
        //            }
        //            else if (typeof(T).ToString().ToLower().Contains("int"))
        //            {
        //                return (T)(object)Idays[i];
        //            }
        //            else
        //            {
        //                return (T)(object)Edays[i];
        //            }
        //        }
        //    }
        //    return default(T);
        //}

        /// <summary>
        /// 获取字符串后几位
        /// </summary>
        /// <param name="str">要截取的字符串</param>
        /// <param name="num">返回的具体位数</param>
        /// <returns>返回结果的字符串</returns>
        public static string GetLastStr(string str, int num)
        {
            int count = 0;
            if (str.Length > num)
            {
                count = str.Length - num;
                str = str.Substring(count, num);
            }
            return str;
        }



        /// <summary>
        /// 将DataTable数据转为StringWriter  (用于导出Excel)
        /// </summary>
        /// <param name="dtSource">源数据</param>
        /// <param name="columnsFiled">字段映射表</param>
        /// <param name="ch">分割符 (若导出Excel则为\t)</param>
        /// <returns></returns>
        public static StringWriter DataTableToStringWriter(DataTable dtSource, HashList columnsFiled, string ch = "\t")
        {
            
            StringWriter sw = new StringWriter();
            string columns = "";

            foreach (string cValue in columnsFiled.Values)
            {
                columns += cValue + ch;
            }

            columns = columns.Substring(0, columns.Length - ch.Length);
            sw.WriteLine(columns);
    
            foreach (DataRow dr in dtSource.Rows)
            {        
                string line = "";
                foreach (string cKey in columnsFiled.Keys)
                {
                    line += dr[cKey].ToString() + ch;
                }
                line = line.Substring(0, line.Length - ch.Length);
                sw.WriteLine(line);
            }
            sw.Close();

            return sw;

        }

        /// <summary>
        /// DataTable导出Excel
        /// </summary>
        /// <param name="Response"></param>
        /// <param name="dt"></param>
        /// <param name="columnsName"></param>
        public static void ExportToExcel(HttpResponse Response, DataTable dt, HashList columnsName)
        {
            String fileName = (new StackTrace()).GetFrame(1).GetMethod().ReflectedType.Name;
            StringWriter sw = DataTableToStringWriter(dt, columnsName);
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName.Substring(fileName.IndexOf('_')+1) + ".xls");
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = Encoding.GetEncoding("GB2312");
            Response.Write(sw);
            Response.End();
        }

        /// <summary>
        /// 计算两个日期的差 (支持int时间戳 string日期字符串 DateTime日期)
        /// </summary>
        /// <typeparam name="T">int string DateTime</typeparam>
        /// <param name="time1">string格式为yyyy-MM-dd</param>
        /// <param name="time2">string格式为yyyy-MM-dd</param>
        /// <param name="d">差的单位 y M d H m s </param>
        /// <returns></returns>
        public static string HowMuchDateTime<T>(T time1,T time2,string d ="M")
        {
            if (time1.GetType() != time2.GetType())
            {
                throw new ArgumentException("传入数据格式错误！");
            }

            TimeSpan ts = new TimeSpan();

            if (typeof(T).ToString().ToLower().Contains("int"))
            {
                ts = (TimeStampToDataTime<DateTime>((int)(object)time1) - TimeStampToDataTime<DateTime>((int)(object)time2)).Duration();
            }
            else if (typeof(T).ToString().ToLower().Contains("string"))
            {
                ts = (DateTime.Parse(time1.ToString()) - DateTime.Parse(time2.ToString())).Duration();
            }
            else if (typeof(T).ToString().ToLower().Contains("datetime"))
            {
                ts = ((DateTime)(object)time1 - (DateTime)(object)time2).Duration();
            }
            else
            {
                throw new ArgumentException("传入数据格式错误！");
            }

            if (d.Equals("y"))
            {
                return (ts.TotalDays / 365).ToString("0.00");
            }
            else if (d.Equals("M"))
            {
                return (ts.TotalDays / 30).ToString("0.00");
            }
            else if (d.Equals("d"))
            {
                return ts.TotalDays.ToString();
            }
            else if (d.Equals("H"))
            {
                return ts.TotalHours.ToString();
            }
            else if (d.Equals("m"))
            {
                return ts.TotalMinutes.ToString();
            }
            else if (d.Equals("s"))
            {
                return ts.TotalSeconds.ToString();
            }
            else
            {
                throw new ArgumentException("传入数据格式错误！");
            }
        }

        /// <summary>
        /// 根据表名创建一个空内容的DataRow
        /// </summary>
        /// <param name="sTableName">表名</param>
        /// <param name="isStrict">是否严格约束数据类型</param>
        /// <returns></returns>
        public static DataRow CreateDataRow(string sTableName, bool isStrict = false)
        {
            DataTable dt = new DataTable();
            
            if (isStrict)
            {
                dt = SQLHelper.ExecSQL_GetDataRow("SELECT TOP 1 * FROM " + sTableName + "").Table.Clone();
            }
            else
            {
                DataTable dtColumns = SQLHelper.ExecSQL_GetTable("Select Name FROM SysColumns Where id=Object_Id('" + sTableName + "')");
                for (int c = 0; c < dtColumns.Rows.Count; c++)
                {
                    dt.Columns.Add(dtColumns.Rows[c]["Name"].ToString(),typeof(object));
                }
            }

            return dt.NewRow();
        }

        public static DataTable CreateBtnBoxSub(List<string[]> list)
        {
            DataTable _dt = new DataTable();
            _dt.Columns.Add("ID");
            _dt.Columns.Add("subText");
            _dt.Columns.Add("subUrl");
            _dt.Columns.Add("imgUrl");
            DataRow _dr;
            for (int i = 0; i < list.Count; i++)
            {
                _dr = _dt.NewRow();
                _dr["ID"] = i;
                _dr["subText"] = list[i][0];
                _dr["subUrl"] = list[i][1];
                _dr["imgUrl"] = list[i][2];
                _dt.Rows.Add(_dr);
            }
            return _dt;
        }

        /// <summary>
        /// http请求传递json数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public static String RequestJson(String url, String json)
        {
            String result = "";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = "POST";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(json);
            }

            var response = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            return result;
        }
        /// <summary>
        /// Json 字符串 转换为 DataTable数据集合
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static DataTable JsonToDataTable(string json)
        {
            DataTable dataTable = new DataTable();  //实例化
            DataTable result;
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
                ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
                if (arrayList.Count > 0)
                {
                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {
                        if (dictionary.Keys.Count == 0)
                        {
                            result = dataTable;
                            return result;
                        }
                        if (dataTable.Columns.Count == 0)
                        {
                            foreach (string current in dictionary.Keys)
                            {
                                dataTable.Columns.Add(current, dictionary[current].GetType());
                            }
                        }
                        DataRow dataRow = dataTable.NewRow();
                        foreach (string current in dictionary.Keys)
                        {
                            dataRow[current] = dictionary[current];
                        }

                        dataTable.Rows.Add(dataRow); //循环添加行到DataTable中
                    }
                }
            }
            catch
            {
                //SystemError.WriterLOG("UtilFunc_" + DateTime.Now.ToString("yyyyMMdd"), "JsonToDataTable", "", "json:\n\r" + json);
                LogHelper.LOG("UtilFunc_" + DateTime.Now.ToString("yyyyMMdd"), MethodBase.GetCurrentMethod().Name, "", "", "", "", "json:\n\r" + json);
            }
            result = dataTable;
            return result;
        }

        /// <summary>
        /// 把对象转换为JSON字符串
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns>JSON字符串</returns>
        public static string ObjToJSON(object o)
        {
            string sJson = "";
            try
            {
                sJson = JsonConvert.SerializeObject(o);
            }
            catch
            {
                //SystemError.WriterLOG("UtilFunc_" + DateTime.Now.ToString("yyyyMMdd"), "ObjToJSON", "", "json:\n\r" + sJson);
                LogHelper.LOG("UtilFunc_" + DateTime.Now.ToString("yyyyMMdd"), MethodBase.GetCurrentMethod().Name, "", "", "", "", "json:\n\r" + sJson);
            }

            return sJson;
        }

        /// <summary>
        /// 把JSON字符串转换为对象
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns>JSON字符串</returns>
        public static T JsonStrToObj<T>(string jStr)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(jStr);
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }


        public static string HashtableToJson(Hashtable hr, int readcount = 0)
        {
            string json = "{";
            foreach (DictionaryEntry row in hr)
            {
                try
                {
                    string key = "\"" + row.Key + "\":";
                    if (row.Value is Hashtable)
                    {
                        Hashtable t = (Hashtable)row.Value;
                        if (t.Count > 0)
                        {
                            json += key + HashtableToJson(t, readcount++) + ",";
                        }
                        else { json += key + "{},"; }
                    }
                    else
                    {
                        string value = "\"" + row.Value.ToString() + "\",";
                        json += key + value;
                    }
                }
                catch {
                    //SystemError.WriterLOG("UtilFunc_" + DateTime.Now.ToString("yyyyMMdd"), "HashtableToJson", "", "json:\n\r" + json + "}");
                    LogHelper.LOG("UtilFunc_" + DateTime.Now.ToString("yyyyMMdd"), MethodBase.GetCurrentMethod().Name, "", "", "", "", "json:\n\r" + json);
                }
            }

            json = json + "}";
            return json;
        }


        public static String[] Hash()
        {
            DateTime dt = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now, TimeZoneInfo.Local);
            //DateTime dtGMT = TimeZoneInfo.ConvertTimeFromUtc(dt, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));//参数对应国家或者时区 
            String time = dt.ToString() + ":" + DateTime.Now.Millisecond.ToString();
            String seed = time + ConfigurationManager.AppSettings["ApplicationHash"];
            return new String[]{ time, Md5(seed)};  
        }

        /// <summary>
        /// 32位MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string Md5(string input)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }


    }

    /// <summary>
    /// UTF8字符串转换为汉字用的类
    /// 转换如"\\u8d35"之类的字符串为对应的汉字
    /// </summary>
    public class UTF8String
    {
        string m_strContent = "";
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="content">要转换的字符串</param>
        public UTF8String(string content)
        {
            m_strContent = content;
        }
        public string getContent()
        {
            return m_strContent;
        }
        /// <summary>
        /// 转换函数
        /// </summary>
        /// <returns>返回转换好的字符串</returns>
        public string ToString()
        {
            string reString = null;
            char[] content = m_strContent.ToCharArray(); //把字符串变为字符数组，以进行处理
            for (int i = 0; i < content.Length; i++) //遍历所有字符
            {
                if (content[i] == '\\') //判断是否转义字符 \ 
                {
                    switch (content[i + 1]) //判断转义字符的下一个字符是什么
                    {
                        case 'u': //转换的是汉字
                        case 'U':
                            reString += HexArrayToChar(content, i + 2); //获取对应的汉字
                            i = i + 5;
                            break;
                        case '/': //转换的是 /
                        case '\\': //转换的是 \
                        case '"':
                            break;
                        default: //其它
                            reString += EscapeCharacter(content[i + 1]); //转为其它类型字符
                            i = i + 1;
                            break;
                    }
                }
                else
                    reString += content[i]; //非转义字符则直接加入
            }
            return reString;
        }

        /// <summary>
        /// 字符数组转对应汉字字符
        /// </summary>
        /// <param name="content">要转换的数字</param>
        /// <param name="startIndex">起始位置</param>
        /// <returns>对应的汉字</returns>
        private char HexArrayToChar(char[] content, int startIndex)
        {
            char[] ac = new char[4];
            for (int i = 0; i < 4; i++) //获取要转换的部分
                ac[i] = content[startIndex + i];
            string num = new string(ac); //字符数组转为字符串
            return HexStringToChar(num);
        }

        /// <summary>
        /// 转义字符转换函数
        /// 转换字符为对应的转义字符
        /// </summary>
        /// <param name="c">要转的字符</param>
        /// <returns>对应的转义字符</returns>
        private char EscapeCharacter(char c)
        {
            char rc;
            switch (c)
            {
                case 't':
                    c = '\t';
                    break;
                case 'n':
                    c = '\n';
                    break;
                case 'r':
                    c = '\r';
                    break;
                case '\'':
                    c = '\'';
                    break;
                case '0':
                    c = '\0';
                    break;
            }
            return c;
        }

        /// <summary>
        /// 字符串转对应汉字字符
        /// 只能处理如"8d34"之类的数字字符为对应的汉字
        /// 例子："9648" 转为 '陈'
        /// </summary>
        /// <param name="content">转换的字符串</param>
        /// <returns>对应的汉字</returns>
        public static char HexStringToChar(string content)
        {
            int num = Convert.ToInt32(content, 16);
            return (char)num;
        }

        /// <summary>
        /// 把string转为UTF8String类型
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static UTF8String ValueOf(string content)
        {
            string reString = null;
            char[] ac = content.ToCharArray();
            int num;
            foreach (char c in ac)
            {
                num = (int)c;
                string n = num.ToString("X2");
                if (n.Length == 4)
                    reString += "\\u" + n;
                else
                    reString += c;
            }
            return new UTF8String(reString);
        }

    }

    /// <summary>
    /// 具备ArrayList排序特性的Hashtable
    /// </summary>
    public class HashList : Hashtable
    {
        private ArrayList alKey = new ArrayList();
        private ArrayList alValue = new ArrayList();

        public override void Add(object key, object value)
        {
            alKey.Add(key);
            alValue.Add(value);
            base.Add(key, value);
        }
        public override void Clear()
        {
            alKey.Clear();
            alValue.Clear();
            base.Clear();
        }
        public override void Remove(object key)
        {  
            alKey.Remove(key);
            alValue.Remove(base[key]);
            base.Remove(key);
        }
        public override ICollection Keys
        {
            get
            {
                return alKey;
            } 
        }
        public override ICollection Values
        {
            get
            {
                return alValue;
            }
        }
        public void Sort()
        {
            alKey.Sort();
            alValue.Sort();
        }
        public void SortKey()
        {
            alKey.Sort();
        }
        public void SortValue()
        {
            alValue.Sort();
        }
    }

}