using System;
using System.Collections.Generic;
using System.IO

;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace IECS.Library
{
    /// <summary>
    /// 华兴传输数据加解密
    /// </summary>
    public class Crypt_DESede_HX
    {
        //32个字符  
        private static string P_CustomKey = "A1B3C3D435F6G1K3I9J1K1L5";

        /// <summary>  
        /// 加密字符串  
        /// </summary>  
        /// <param name="sStr"></param>  
        /// <returns></returns>  
        public static string Encrypt(string sStr)
        {
            string _sEncryptStr = string.Empty;

            SymmetricAlgorithm _oAlgorithm = new TripleDESCryptoServiceProvider();
            _oAlgorithm.Key = (new System.Text.UTF8Encoding()).GetBytes(P_CustomKey);
            //_oAlgorithm.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
            //_oAlgorithm.Mode = CipherMode.CBC;
            _oAlgorithm.Mode = CipherMode.ECB;
            _oAlgorithm.Padding = PaddingMode.PKCS7;

            ICryptoTransform _ifTransform = _oAlgorithm.CreateEncryptor();

            byte[] _bData = (new UTF8Encoding()).GetBytes(sStr);
            MemoryStream _oMemoryStream = new MemoryStream();
            CryptoStream _oCryptoStream = new CryptoStream(_oMemoryStream, _ifTransform, CryptoStreamMode.Write);

            _oCryptoStream.Write(_bData, 0, _bData.Length);
            _oCryptoStream.FlushFinalBlock();

            //encryptPassword = Encoding.Default.GetString(memoryStream.ToArray());
            _sEncryptStr = Convert.ToBase64String(_oMemoryStream.ToArray());
            //encryptPassword = Encoding.UTF8.GetString(memoryStream.ToArray());

            _oMemoryStream.Close();
            _oCryptoStream.Close();

            return _sEncryptStr;
        }

        /// <summary>  
        /// 解密字符串  
        /// </summary>  
        /// <param name="sStr"></param>  
        /// <returns></returns>  
        public static string Decrypt(string sStr)
        {
            string _sEncryptStr = string.Empty;

            SymmetricAlgorithm _oAlgorithm = new TripleDESCryptoServiceProvider();
            _oAlgorithm.Key = (new System.Text.UTF8Encoding()).GetBytes(P_CustomKey);
            _oAlgorithm.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }; //Encoding.UTF8.GetBytes(customKey.Substring(0,8));
            _oAlgorithm.Mode = CipherMode.CBC;
            _oAlgorithm.Padding = PaddingMode.PKCS7;

            //ICryptoTransform transform = _oAlgorithm.CreateDecryptor(algorithm.Key, algorithm.IV);
            ICryptoTransform _ifTransform = _oAlgorithm.CreateDecryptor();

            byte[] _bBuffer = (new UTF8Encoding()).GetBytes(sStr);
            MemoryStream _oMemoryStream = new MemoryStream(_bBuffer);
            CryptoStream _oCryptoStream = new CryptoStream(_oMemoryStream, _ifTransform, CryptoStreamMode.Read);
            StreamReader reader = new StreamReader(_oCryptoStream, System.Text.Encoding.UTF8);
            _sEncryptStr = reader.ReadToEnd();

            reader.Close();
            _oCryptoStream.Close();
            _oMemoryStream.Close();

            return _sEncryptStr;
        }
    }
}
