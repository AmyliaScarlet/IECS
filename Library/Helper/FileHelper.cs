using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Web;
using System.Net;
using System.Text;

namespace IECS.Library
{
    /// <summary>
    /// 
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>string</returns>
        public static string Read(string path)
        {
            string result = null;
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                StreamReader reader = new StreamReader(fs, Encoding.UTF8);
                result = reader.ReadToEnd();
            }
            return result;
        }
    }

}
