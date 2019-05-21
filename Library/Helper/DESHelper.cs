using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace IECS.Library
{
    /// <summary>
    /// 应用程序错误处理类
    /// </summary>
    public class DESHelper
    {
        /// <summary>
        /// 标准的DES加密
        /// </summary>
        /// <param name="value"></param>
        /// <param name="arrKEY_64"></param>
        /// <param name="arrIV_64"></param>
        /// <returns></returns>
        public static String GetDESEncrypt(String value, byte[] arrKEY_64, byte[] arrIV_64)
        {
            if (value != "")
            {
                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, cryptoProvider.CreateEncryptor(arrKEY_64, arrIV_64), CryptoStreamMode.Write);
                StreamWriter sw = new StreamWriter(cs);
                sw.Write(value);
                sw.Flush();
                cs.FlushFinalBlock();
                ms.Flush();

                return Convert.ToBase64String(ms.GetBuffer(), 0, Convert.ToInt16(ms.Length));
            }
            return "-1";
        }

        /// <summary>
        /// 标准的DES解密
        /// </summary>
        /// <param name="value"></param>
        /// <param name="arrKEY_64"></param>
        /// <param name="arrIV_64"></param>
        /// <returns></returns>
        public static String GetDESDecrypt(String value, byte[] arrKEY_64, byte[] arrIV_64)
        {
            try
            {

                if (value != "")
                {
                    DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();

                    //从字符串转换为字节组

                    byte[] buffer = Convert.FromBase64String(value);

                    MemoryStream ms = new MemoryStream(buffer);

                    CryptoStream cs = new CryptoStream(ms, cryptoProvider.CreateDecryptor(arrKEY_64, arrIV_64), CryptoStreamMode.Read);

                    StreamReader sr = new StreamReader(cs);

                    return sr.ReadToEnd();

                }

            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }

            return "-1";

        }

        /// <summary>
        /// 另一种加密算法（字符串可以是汉字，但密码不可以是汉字）
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="texti"></param>
        /// <returns></returns>
        public String GetenCrypt(String strKey, String texti)
        {
            try
            {
                String Crypted = "";
                Int32 i, G, intKey;
                Int32 Sana = 0;
                Int32 X1 = 0;

                for (i = 0; i <= strKey.Length - 1; i++)
                {
                    Sana = GetASC(strKey.Substring(i, 1));
                    X1 = X1 + Sana;
                }

                X1 = Convert.ToInt32((X1 * 0.1) / 6);
                intKey = X1;
                G = 0;

                for (i = 0; i <= texti.Length - 1; i++)
                {
                    Sana = GetASC(texti.Substring(i, 1));
                    G = G + 1;

                    if (G == 6) { G = 0; }
                    X1 = 0;

                    if (G == 0) { X1 = Sana - (intKey - 2); }
                    if (G == 1) { X1 = Sana + (intKey - 5); }
                    if (G == 2) { X1 = Sana - (intKey - 4); }
                    if (G == 3) { X1 = Sana + (intKey - 2); }
                    if (G == 4) { X1 = Sana - (intKey - 3); }
                    if (G == 5) { X1 = Sana + (intKey - 5); }
                    X1 = X1 + G;
                    Crypted = Crypted + Convert.ToChar(X1);
                }

                return Crypted;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        /// <summary>
        /// 另一种解密算法（字符串可以是汉字，但密码不可以是汉字）
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="texti"></param>
        /// <returns></returns>
        public String GetdeCrypt(String strKey, String texti)
        {
            try
            {
                String deCrypted = "";
                Int32 i, G, intKey;
                Int32 Sana = 0;
                Int32 X1 = 0;

                for (i = 0; i <= strKey.Length - 1; i++)
                {
                    Sana = GetASC(strKey.Substring(i, 1));
                    X1 = X1 + Sana;
                }

                X1 = Convert.ToInt32((X1 * 0.1) / 6);
                intKey = X1;
                G = 0;

                for (i = 0; i <= texti.Length - 1; i++)
                {
                    Sana = GetASC(texti.Substring(i, 1));
                    G = G + 1;

                    if (G == 6) { G = 0; }
                    X1 = 0;

                    if (G == 0) { X1 = Sana + (intKey - 2); }
                    if (G == 1) { X1 = Sana - (intKey - 5); }
                    if (G == 2) { X1 = Sana + (intKey - 4); }
                    if (G == 3) { X1 = Sana - (intKey - 2); }
                    if (G == 4) { X1 = Sana + (intKey - 3); }
                    if (G == 5) { X1 = Sana - (intKey - 5); }
                    X1 = X1 - G;
                    deCrypted = deCrypted + Convert.ToChar(X1);

                }

                return deCrypted;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        /// <summary>
        /// 获取ASC码 
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        private int GetASC(String Data)
        {
            byte[] b = System.Text.Encoding.Default.GetBytes(Data);
            int p = 0;

            if (b.Length == 1)   //如果为英文字符直接返回   
                return (int)b[0];
            for (int i = 0; i < b.Length; i += 2)
            {
                p = (int)b[i];
                p = p * 256 + b[i + 1] - 65536;
            }
            return p;
        }

        /// <summary>
        /// MD5
        /// </summary>
        /// <param name="md5Hash"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

    }
}
