using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Web;
using System.Net;
using System.Text;
using System.Reflection;

namespace IECS.Library
{
    /// <summary>
    /// Http帮助类
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        /// 发送 POST 请求
        /// </summary>
        /// <param name="url">Url地址</param>
        /// <param name="data">数据</param>
        /// <param name="contentType">数据</param>
        public static string SendPostHttpRequest(string url, string data, string contentType = "application/x-www-form-urlencoded")
        {
            return mSendPostRequest(url, contentType, data); 
        }
        /// <summary>
        /// 发送 GET 请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="contentType">数据</param>
        /// <returns></returns>
        public static string SendGetHttpRequest(string url, string contentType = "application/x-www-form-urlencoded")
        {
            return mSendGetRequest(url, contentType);
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="url">Url地址</param>
        /// <param name="contentType">数据类型</param>
        /// <param name="requestData">数据</param>
        private static string mSendPostRequest(string url, string contentType, string requestData)
        {
            WebRequest request = (WebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            byte[] postBytes = null;
            request.ContentType = contentType;
            postBytes = Encoding.UTF8.GetBytes(requestData);
            request.ContentLength = postBytes.Length;
            using (Stream outstream = request.GetRequestStream())
            {
                outstream.Write(postBytes, 0, postBytes.Length);
            }
            string result = string.Empty;
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    if (response != null)
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                            {
                                result = reader.ReadToEnd();
                            }
                        }

                    }
                }
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    //Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        result = reader.ReadToEnd();
                        LogHelper.LOG("HttpHelper",MethodBase.GetCurrentMethod().Name, e.Source, e.Message, url, requestData, result);
                        result = "";
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="url">Url地址</param>
        /// <param name="contentType">数据</param>
        private static string mSendGetRequest(string url, string contentType)
        {
            WebRequest request = (WebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = contentType;
            string result = string.Empty;
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    if (response != null)
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                            {
                                result = reader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    //Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        result = reader.ReadToEnd();
                        LogHelper.LOG("HttpHelper", MethodBase.GetCurrentMethod().Name, "", e.Source, e.Message, url, result);
                        result = "";
                    }
                }
            }
            return result;
        }


        ///// <summary>
        ///// 需要WebService支持Post调用
        ///// </summary>
        //public static string PostWebService(String URL, String MethodName, string requestData)
        //{
        //    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(URL + "/" + MethodName);
        //    request.Method = "POST";
        //    request.ContentType = "application/json;charset=utf-8";
        //    request.Credentials = CredentialCache.DefaultCredentials;
        //    request.Timeout = 10000;
        //    byte[] postBytes = null;
        //    postBytes = Encoding.UTF8.GetBytes(requestData);
        //    request.ContentLength = postBytes.Length;
        //    Stream writer = request.GetRequestStream();
        //    writer.Write(postBytes, 0, postBytes.Length);
        //    writer.Close();

        //    StreamReader sr = new StreamReader(request.GetResponse().GetResponseStream(), Encoding.UTF8);
        //    String retXml = sr.ReadToEnd();
        //    sr.Close();
        //    return retXml;
        //}
    }

}
