using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Web;
using System.Net;
using System.Text;
using System.Reflection;
using System.Xml;
using System.Security.Cryptography;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;
using System.Data;

namespace IECS.Library
{
    /// <summary>
    /// 华兴传输数据加解密 Http帮助类
    /// </summary>
    public class HuaXingHelper
    {
        /// <summary>异步请求URL</summary>
        public static String PostUrl = "https://ogws.ghbank.com.cn/extService/ghbExtService.do";
        /// <summary>查询请求URL</summary>
        public static String QueryUrl = "https://ogws.ghbank.com.cn/extService/ghbExtService.do";

        /// <summary>异步请求返回 URL</summary>
        public static String PostReturnUrl_C = "http://www.yixiangdai.com/User/UserCenter.aspx";
        //public static String PostReturnUrl_M = "http://m.yixiangdai.com/User/UC.aspx";
        public static String PostReturnUrl_M = "/User/UC.aspx";

        //public static String WEBHOST = "http://203.86.9.179";
        //public static String WEBHOME = "http://203.86.9.179/index.php";

        /// <summary>商户ID YXD1</summary>
        public static String MERCHANTID = "YXD"; 
        /// <summary>商户名</summary>
        public static String MERCHANTNAME = "P2P易享贷"; 
        /// <summary>平台</summary>
        public static String APPID = "PC"; 
        /// <summary>平台</summary>
        public static String APPIDWX = "WX"; 
        /// <summary>渠道标识</summary>
        public static String channelCode = "P2P033";

        private static Object P_oLock_channelFlow2 = new Object(); //线程锁对象

        /// <summary></summary>
        public static String channelFlow1(string sChannelDate)
        {
            return channelCode + sChannelDate;
        }

        /// <summary></summary>
        public static String channelFlow2(string sChannelTime)
        {
            //防止多线程并发时，系统处理进程不及时，导致获取相同的值
            lock (P_oLock_channelFlow2)
            {
                Random random = new Random();
                return sChannelTime + (new Random()).Next((Int32)(Math.Pow(10, (5 - 1))), (Int32)(Math.Pow(10, 5) - 1));
            }
        }

        /// <summary>
        /// 报文加密处理
        /// </summary>
        /// <param name="sXMLPARA"></param>
        /// <param name="sTRANSCODE"></param>
        /// <returns></returns>
        public static String HxEncrypt(String sXMLPARA, String sTRANSCODE, string sChannelDate = "", string sChannelTime = "", string sChannelFlow1 = "", string sChannelFlow2 = "")
        {

            ///////////////////////////////////////
            //第二步  XMLPARA 内的数据要进行 base64_encode，将字符串以 BASE64 编码。
            ///////////////////////////////////////

            //String XMLPARA_BASE64 = Base64Encode(ClsDES3EncryptAndDecrypt.encrypt(Encoding.Default.GetBytes(sXMLPARA)));
            String XMLPARA_BASE64 = Encrypt(sXMLPARA);

            //dIPpAQaOnYQBkIFuXl41vBEB2dxj0bLYBKEAE/L8Yt/UCWpVyHMjtbjvzI+/H3Wib9M9cFIoBID1CG9C/MuMGlfxL/zzbyjGaZPNIX+9KeSu8VLojKbc1Owp7KdSGvV/wV+HMKn+EJtWuSV2ROPjQywD4CPaOX5TnVq9onKnp0vZwTQBif6vKy3l9AGCiEzl+AsuXHtwiPxklH6Xzozh8PkfK5U+Un1FpKVirueeWM4EZOkm9t7f7D+fKj0HzgXSpAopKQ1kg3r2SZhAd8BMlPDJyAEuHeft0oRl4OQDCV6Ne7CJV0P4NQjL/1mSaNfpzxfRfD8/4hqx336bQZiz903V83H8yY9QPEQJZHsbGNKqvO3Jpb5xaF3QfYo9VUkV3UTuVJ0TCrENdf4gEy66y30wb4eCYTU+rUrrd4V2UQhfMTcLzZ6eSXtHsGS28CaaAxUi550i3SUbOmG8zLwkTY5958NdprXKDC0dH7LmpCNNyPImOSUaaw==

            ///////////////////////////////////////
            //第三步 填充报文内容————替换报文内容的换行符
            ///////////////////////////////////////


            //摘要签名
            String TRANSMIT = "<?xml version='1.0' encoding='UTF-8'?>";
            TRANSMIT += "<Document><header>";
            TRANSMIT += "<channelCode>" + channelCode + "</channelCode>";
            TRANSMIT += "<channelFlow>" + sChannelFlow1 + sTRANSCODE.Substring(sTRANSCODE.Length - 3, 3) + sChannelFlow2 + "</channelFlow>";
            TRANSMIT += "<channelDate>" + sChannelDate + "</channelDate>";
            TRANSMIT += "<channelTime>" + sChannelTime + "</channelTime>";
            TRANSMIT += "<encryptData></encryptData>";
            TRANSMIT += "</header><body><TRANSCODE>" + sTRANSCODE + "</TRANSCODE>";
            TRANSMIT += "<XMLPARA>" + XMLPARA_BASE64 + "</XMLPARA></body></Document>";

            TRANSMIT = TRANSMIT.Replace("\r\n", "").Trim();

            LogHelper.LOG("User_" + DateTime.Now.ToString("yyyyMMdd"), MethodBase.GetCurrentMethod().Name, "trans", "", "", PostUrl, "tran=" + TRANSMIT);
            //<?xml version='1.0' encoding='UTF-8'?><Document><header><channelCode>P2P033</channelCode><channelFlow>P2P0332017092104217000020656</channelFlow><channelDate>20170921</channelDate><channelTime>170000</channelTime><encryptData></encryptData></header><body><TRANSCODE>OGW00042</TRANSCODE><XMLPARA>dIPpAQaOnYQBkIFuXl41vBEB2dxj0bLYBKEAE/L8Yt/UCWpVyHMjtbjvzI+/H3Wib9M9cFIoBID1CG9C/MuMGlfxL/zzbyjGaZPNIX+9KeSu8VLojKbc1Owp7KdSGvV/wV+HMKn+EJtWuSV2ROPjQywD4CPaOX5TnVq9onKnp0vZwTQBif6vKy3l9AGCiEzl+AsuXHtwiPxklH6Xzozh8PkfK5U+Un1FpKVirueeWM4EZOkm9t7f7D+fKj0HzgXSpAopKQ1kg3r2SZhAd8BMlPDJyAEuHeft0oRl4OQDCV6Ne7CJV0P4NQjL/1mSaNfpzxfRfD8/4hqx336bQZiz903V83H8yY9QPEQJZHsbGNKqvO3Jpb5xaF3QfYo9VUkV3UTuVJ0TCrENdf4gEy66y30wb4eCYTU+rUrrd4V2UQhfMTcLzZ6eSXtHsGS28CaaAxUi550i3SUbOmG8zLwkTY5958NdprXKDC0dH7LmpCNNyPImOSUaaw==</XMLPARA></body></Document>

            ///////////////////////////////////////
            //第四步 报文内容MD5摘要————摘要字符串大写转换————签名
            ///////////////////////////////////////

            MD5 _oMD5Hash = MD5.Create();
            String TRANSMIT_MD5 = DESHelper.GetMd5Hash(_oMD5Hash, TRANSMIT).ToUpper();//MD5摘要
            //String TRANSMIT_MD5_SIGN = BitConverter.ToString(ClsCryptRSA.Sign(TRANSMIT_MD5)).Replace("-", "");//签名
            byte[] TRANSMIT_MD5_SIGN = HuaXingHelper.RSAEncryptByPrivateKey(TRANSMIT_MD5);//签名

            //@MD5 【已通过】
            //4D4A0EAE42E622233C82C1C7A827036D

            //@转十六进制
            //108CF1F2CA77BEFA2524751CCB555B559B8BDE6DF70C777698159FFDBE4AED4F8C8C2AD72C8C485438BE763A7ACD8DD5958E01D5CE34C90B8F9300B4D37D635CC48E4558D07A54D756CC807564D9ACFF023588DC4A29E6D0F8CA436C09E214ED4E9EAA24CE497FE5CB6528DFDC852389D4B9BD545B965C410621287D869A9670
            //

            ///////////////////////////////////////
            //第五步 
            //1. 签名转换为16进制字符串——字符串大写转换——得到最终签名
            //2. 计算最终签名格式，并按8位字符串右侧填充
            ///////////////////////////////////////

            //String SIGN = Bin2Hex(TRANSMIT_MD5_SIGN).ToLower();//转为十六进制
            String SIGN = BitConverter.ToString(TRANSMIT_MD5_SIGN).Replace("-", "");//转为十六进制
            //String SIGN_LEN = StrPad(SIGN.Length.ToString(),8,'0',ClsEnum.EnumPadMode.LEFT);
            String SIGN_LEN = SIGN.Length.ToString().PadLeft(8, '0');



            //@填充
            //00000256




            ///////////////////////////////////////
            //第六步 最终报文
            ///////////////////////////////////////

            String result = "001X11          " + SIGN_LEN + SIGN + TRANSMIT;



            //    001X11          
            //00000256108CF1F2CA77BEFA2524751CCB555B559B8BDE6DF70C777698159FFDBE4AED4F8C8C2AD72C8C485438BE763A7ACD8DD5958E01D
            //5CE34C90B8F9300B4D37D635CC48E4558D07A54D756CC807564D9ACFF023588DC4A29E6D0F8CA436C09E214ED4E9EAA24CE497FE5CB6528
            //DFDC852389D4B9BD545B965C410621287D869A9670<?xml version='1.0' encoding='UTF-8'?
            //><Document><header><channelCode>P2P033</channelCode><channelFlow>P2P0332017092104217000020656</channelFlow><cha
            //nnelDate>20170921</channelDate><channelTime>170000</channelTime><encryptData></encryptData></header><body><TRAN
            //SCODE>OGW00042</TRANSCODE><XMLPARA>dIPpAQaOnYQBkIFuXl41vBEB2dxj0bLYBKEAE/L8Yt/UCWpVyHMjtbjvzI
            //+/H3Wib9M9cFIoBID1CG9C/MuMGlfxL/zzbyjGaZPNIX+9KeSu8VLojKbc1Owp7KdSGvV/wV+HMKn
            //+EJtWuSV2ROPjQywD4CPaOX5TnVq9onKnp0vZwTQBif6vKy3l9AGCiEzl+AsuXHtwiPxklH6Xzozh8PkfK5U
            //+Un1FpKVirueeWM4EZOkm9t7f7D
            //+fKj0HzgXSpAopKQ1kg3r2SZhAd8BMlPDJyAEuHeft0oRl4OQDCV6Ne7CJV0P4NQjL/1mSaNfpzxfRfD8/4hqx336bQZiz903V83H8yY9QPEQJZ
            //HsbGNKqvO3Jpb5xaF3QfYo9VUkV3UTuVJ0TCrENdf4gEy66y30wb4eCYTU
            //+rUrrd4V2UQhfMTcLzZ6eSXtHsGS28CaaAxUi550i3SUbOmG8zLwkTY5958NdprXKDC0dH7LmpCNNyPImOSUaaw==</XMLPARA></body></Doc
            //ument>          

            return result;

        }

        /// <summary>
        /// 解密 获取 XMLPARA TRANSCODE channelFlow
        /// </summary>
        /// <param name="resultData"></param>
        /// <param name="TRANSCODE"></param>
        /// <param name="channelFlow"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public static XmlNode HxDecrypt(string resultData, out string TRANSCODE, out string channelFlow, out string errorMsg)
        {
            TRANSCODE = "";
            channelFlow = "";
            errorMsg = "";

            string errorCode = "";
            //string channelCode = "";
            //string channelDate = "";
            //string channelTime = "";
            //string encryptData = "";

            //string MERCHANTID = "";
            //string BANKID = "";

            string XMLPARA = "";

            resultData = resultData.Substring(resultData.IndexOf("<?xml"));

            XmlDocument _xmlDoc = new XmlDocument();
            _xmlDoc.LoadXml(resultData);
            XmlNode _xnDoc = _xmlDoc.SelectSingleNode("Document");

            XmlNode _xnHeader = null;
            XmlNode _xnBody = null;

            foreach (XmlNode xn in _xnDoc.ChildNodes)
            {
                if (xn.Name.Equals("header"))
                {
                    _xnHeader = xn;
                }
                if (xn.Name.Equals("body"))
                {
                    _xnBody = xn;
                }
            }

            foreach (XmlNode xn in _xnHeader.ChildNodes)
            {
                if (xn.Name.Equals("errorCode"))
                {
                    errorCode = xn.InnerText;
                }
                if (xn.Name.Equals("errorMsg"))
                {
                    errorMsg = xn.InnerText;
                }
                if (xn.Name.Equals("channelFlow"))
                {
                    channelFlow = xn.InnerText;
                }
                //if (xn.Name.Equals("channelCode"))
                //{
                //    channelCode = xn.InnerText;
                //}
                //if (xn.Name.Equals("channelDate"))
                //{
                //    channelDate = xn.InnerText;
                //}
                //if (xn.Name.Equals("channelTime"))
                //{
                //    channelTime = xn.InnerText;
                //}
                //if (xn.Name.Equals("encryptData"))
                //{
                //    encryptData = xn.InnerText;
                //}
            }

            //异步执行没有 errorCode 项目，同步执行需要 errorCode == "0" 表示成功
            if (errorCode == "" || errorCode == "0")
            {
                foreach (XmlNode xn in _xnBody.ChildNodes)
                {
                    if (xn.Name.Equals("TRANSCODE"))
                    {
                        TRANSCODE = xn.InnerText;
                    }
                    if (xn.Name.Equals("XMLPARA"))
                    {
                        XMLPARA = xn.InnerText;
                    }
                    //if (xn.Name.Equals("MERCHANTID"))
                    //{
                    //    MERCHANTID = xn.InnerText;
                    //}
                    //if (xn.Name.Equals("BANKID"))
                    //{
                    //    BANKID = xn.InnerText;
                    //}
                }

                String _sXMLPARA_Dec = Decrypt(XMLPARA);
                if (_sXMLPARA_Dec != "")
                {
                    //LogHelper.LOG("Bank_" + DateTime.Now.ToString("yyyyMMdd"), MethodBase.GetCurrentMethod().Name, "HxDecrypt", "", "", mConfig.PostUrl, "XMLPARA=" + XMLPARA_Dec);
                    XmlDocument xmlPARA = new XmlDocument();
                    xmlPARA.LoadXml("<?xml version='1.0' encoding='UTF-8'?><Document>" + _sXMLPARA_Dec + "</Document>");
                    return xmlPARA.SelectSingleNode("Document");
                }
                else
                {
                    LogHelper.LOG(MethodBase.GetCurrentMethod().Name + "_" + DateTime.Now.ToString("yyyyMMdd"), MethodBase.GetCurrentMethod().Name, "", "errorCode", errorCode, "_sXMLPARA_Dec", "NULL");
                }
            }
            else
            {
                LogHelper.LOG(MethodBase.GetCurrentMethod().Name + "_" + DateTime.Now.ToString("yyyyMMdd"), MethodBase.GetCurrentMethod().Name, "", "errorCode", errorCode, "_xnDoc", _xnDoc.OuterXml);
            }

            return null;
        }
        /// <summary>
        /// 解密 获取 XMLPARA TRANSCODE channelFlow
        /// </summary>
        /// <param name="resultData"></param>
        /// <param name="TRANSCODE"></param>
        /// <param name="channelFlow"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public static XmlNode HxDecrypt_Test(string resultData, out string TRANSCODE, out string channelFlow, out string errorMsg)
        {
            TRANSCODE = "";
            channelFlow = "";
            errorMsg = "";

            string errorCode = "";
            //string channelCode = "";
            //string channelDate = "";
            //string channelTime = "";
            //string encryptData = "";

            //string MERCHANTID = "";
            //string BANKID = "";

            string XMLPARA = "";

            resultData = resultData.Substring(resultData.IndexOf("<?xml"));

            XmlDocument _xmlDoc = new XmlDocument();
            _xmlDoc.LoadXml(resultData);
            XmlNode _xnDoc = _xmlDoc.SelectSingleNode("Document");

            XmlNode _xnHeader = null;
            XmlNode _xnBody = null;

            foreach (XmlNode xn in _xnDoc.ChildNodes)
            {
                if (xn.Name.Equals("header"))
                {
                    _xnHeader = xn;
                }
                if (xn.Name.Equals("body"))
                {
                    _xnBody = xn;
                }
            }

            foreach (XmlNode xn in _xnHeader.ChildNodes)
            {
                if (xn.Name.Equals("errorCode"))
                {
                    errorCode = xn.InnerText;
                }
                if (xn.Name.Equals("errorMsg"))
                {
                    errorMsg = xn.InnerText;
                }
                if (xn.Name.Equals("channelFlow"))
                {
                    channelFlow = xn.InnerText;
                }
                //if (xn.Name.Equals("channelCode"))
                //{
                //    channelCode = xn.InnerText;
                //}
                //if (xn.Name.Equals("channelDate"))
                //{
                //    channelDate = xn.InnerText;
                //}
                //if (xn.Name.Equals("channelTime"))
                //{
                //    channelTime = xn.InnerText;
                //}
                //if (xn.Name.Equals("encryptData"))
                //{
                //    encryptData = xn.InnerText;
                //}
            }

            //异步执行没有 errorCode 项目，同步执行需要 errorCode == "0" 表示成功
            if (errorCode == "" || errorCode == "0")
            {
                foreach (XmlNode xn in _xnBody.ChildNodes)
                {
                    if (xn.Name.Equals("TRANSCODE"))
                    {
                        TRANSCODE = xn.InnerText;
                    }
                    if (xn.Name.Equals("XMLPARA"))
                    {
                        XMLPARA = xn.InnerText;
                    }
                    //if (xn.Name.Equals("MERCHANTID"))
                    //{
                    //    MERCHANTID = xn.InnerText;
                    //}
                    //if (xn.Name.Equals("BANKID"))
                    //{
                    //    BANKID = xn.InnerText;
                    //}
                }

                String _sXMLPARA_Dec = Decrypt_Test(XMLPARA);
                if (_sXMLPARA_Dec != "")
                {
                    //LogHelper.LOG("Bank_" + DateTime.Now.ToString("yyyyMMdd"), MethodBase.GetCurrentMethod().Name, "HxDecrypt", "", "", mConfig.PostUrl, "XMLPARA=" + XMLPARA_Dec);
                    XmlDocument xmlPARA = new XmlDocument();
                    xmlPARA.LoadXml("<?xml version='1.0' encoding='UTF-8'?><Document>" + _sXMLPARA_Dec + "</Document>");
                    return xmlPARA.SelectSingleNode("Document");
                }
                else
                {
                    LogHelper.LOG(MethodBase.GetCurrentMethod().Name + "_" + DateTime.Now.ToString("yyyyMMdd"), MethodBase.GetCurrentMethod().Name, "", "errorCode", errorCode, "_sXMLPARA_Dec", "NULL");
                }
            }
            else
            {
                LogHelper.LOG(MethodBase.GetCurrentMethod().Name + "_" + DateTime.Now.ToString("yyyyMMdd"), MethodBase.GetCurrentMethod().Name, "", "errorCode", errorCode, "_xnDoc", _xnDoc.OuterXml);
            }

            return null;
        }

        //32个字符  
        private static string P_Key = "A1B3C3D435F6G1K3I9J1K1L5";
        private static string P_Key_Test = "A1B2C3D4E5F6G7H8I9J0K1L2";

        /// <summary>  
        /// 加密字符串  
        /// </summary>  
        /// <param name="sStr"></param>  
        /// <returns></returns>  
        private static string Encrypt(string sStr)
        {
            string _sEncryptStr = string.Empty;

            SymmetricAlgorithm _oAlgorithm = new TripleDESCryptoServiceProvider();
            //algorithm.Key = Convert.FromBase64String(customKey);
            //algorithm.IV = Convert.FromBase64String(customIV);
            _oAlgorithm.Key = Encoding.UTF8.GetBytes(P_Key);
            //_oAlgorithm.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }; //Encoding.UTF8.GetBytes(customKey.Substring(0,8));
            _oAlgorithm.Mode = CipherMode.ECB;
            _oAlgorithm.Padding = PaddingMode.PKCS7;

            ICryptoTransform _ifTransform = _oAlgorithm.CreateEncryptor();

            byte[] _bData = Encoding.UTF8.GetBytes(sStr);
            using (MemoryStream _oMemoryStream = new MemoryStream())
            {

                using (CryptoStream _oCryptoStream = new CryptoStream(_oMemoryStream, _ifTransform, CryptoStreamMode.Write))
                {

                    _oCryptoStream.Write(_bData, 0, _bData.Length);
                    //_oCryptoStream.FlushFinalBlock();
                    _oCryptoStream.Close();


                    //_sEncryptStr = Encoding.UTF8.GetString(memoryStream.ToArray());
                    _sEncryptStr = Convert.ToBase64String(_oMemoryStream.ToArray());
                    //encryptPassword = Encoding.UTF8.GetString(memoryStream.ToArray());
                }
            }

            return _sEncryptStr;
        }
        /// <summary>
        /// 解密字符串  
        /// </summary>  
        /// <param name="sStr"></param>  
        /// <returns></returns>  
        private static string Decrypt(string sStr)
        {
            string _sDecryptStr = string.Empty;

            SymmetricAlgorithm _oAlgorithm = new TripleDESCryptoServiceProvider();
            //_oAlgorithm.Key = (new System.Text.UTF8Encoding()).GetBytes(P_Key);
            _oAlgorithm.Key = Encoding.UTF8.GetBytes(P_Key);
            //_oAlgorithm.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }; //Encoding.UTF8.GetBytes(customKey.Substring(0,8));
            _oAlgorithm.Mode = CipherMode.ECB;
            _oAlgorithm.Padding = PaddingMode.PKCS7;

            //ICryptoTransform transform = _oAlgorithm.CreateDecryptor(algorithm.Key, algorithm.IV);
            ICryptoTransform _ifTransform = _oAlgorithm.CreateDecryptor();

            byte[] _bData = Convert.FromBase64String(sStr);
            //MemoryStream _oMemoryStream = new MemoryStream(_bData);
            //CryptoStream _oCryptoStream = new CryptoStream(_oMemoryStream, _ifTransform, CryptoStreamMode.Read);
            //StreamReader reader = new StreamReader(_oCryptoStream, Encoding.UTF8);
            //_sDecryptStr = reader.ReadToEnd();

            //reader.Close();
            //_oCryptoStream.Close();
            //_oMemoryStream.Close();
            using (MemoryStream _oMemoryStream = new MemoryStream())
            {

                using (CryptoStream _oCryptoStream = new CryptoStream(_oMemoryStream, _ifTransform, CryptoStreamMode.Write))
                {

                    _oCryptoStream.Write(_bData, 0, _bData.Length);
                    _oCryptoStream.FlushFinalBlock();

                    //_sEncryptStr = Encoding.UTF8.GetString(memoryStream.ToArray());
                    _sDecryptStr = Encoding.UTF8.GetString(_oMemoryStream.ToArray());
                    //encryptPassword = Encoding.UTF8.GetString(memoryStream.ToArray());
                }
            }

            return _sDecryptStr;

        }
        /// <summary>
        /// 解密字符串  
        /// </summary>  
        /// <param name="sStr"></param>  
        /// <returns></returns>  
        private static string Decrypt_Test(string sStr)
        {
            string _sDecryptStr = string.Empty;

            SymmetricAlgorithm _oAlgorithm = new TripleDESCryptoServiceProvider();
            //_oAlgorithm.Key = (new System.Text.UTF8Encoding()).GetBytes(P_Key_Test);
            _oAlgorithm.Key = Encoding.UTF8.GetBytes(P_Key_Test);
            //_oAlgorithm.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }; //Encoding.UTF8.GetBytes(customKey.Substring(0,8));
            _oAlgorithm.Mode = CipherMode.ECB;
            _oAlgorithm.Padding = PaddingMode.PKCS7;

            //ICryptoTransform transform = _oAlgorithm.CreateDecryptor(algorithm.Key, algorithm.IV);
            ICryptoTransform _ifTransform = _oAlgorithm.CreateDecryptor();

            byte[] _bData = Convert.FromBase64String(sStr);
            //MemoryStream _oMemoryStream = new MemoryStream(_bData);
            //CryptoStream _oCryptoStream = new CryptoStream(_oMemoryStream, _ifTransform, CryptoStreamMode.Read);
            //StreamReader reader = new StreamReader(_oCryptoStream, Encoding.UTF8);
            //_sDecryptStr = reader.ReadToEnd();

            //reader.Close();
            //_oCryptoStream.Close();
            //_oMemoryStream.Close();
            using (MemoryStream _oMemoryStream = new MemoryStream())
            {

                using (CryptoStream _oCryptoStream = new CryptoStream(_oMemoryStream, _ifTransform, CryptoStreamMode.Write))
                {

                    _oCryptoStream.Write(_bData, 0, _bData.Length);
                    _oCryptoStream.FlushFinalBlock();

                    //_sEncryptStr = Encoding.UTF8.GetString(memoryStream.ToArray());
                    _sDecryptStr = Encoding.UTF8.GetString(_oMemoryStream.ToArray());
                    //encryptPassword = Encoding.UTF8.GetString(memoryStream.ToArray());
                }
            }

            return _sDecryptStr;

        }

        /// <summary>
        /// RSA私钥格式转换，java->.net
        /// </summary>
        /// <param name="privateKey">java生成的RSA私钥</param>
        /// <returns></returns>
        public static string RSAPrivateKeyJava2DotNet(string privateKey)
        {
            RsaPrivateCrtKeyParameters privateKeyParam = (RsaPrivateCrtKeyParameters)PrivateKeyFactory.CreateKey(Convert.FromBase64String(privateKey));

            return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>",
                Convert.ToBase64String(privateKeyParam.Modulus.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.PublicExponent.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.P.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.Q.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.DP.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.DQ.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.QInv.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.Exponent.ToByteArrayUnsigned()));
        }

        /// <summary>
        /// RSA私钥格式转换，.net->java
        /// </summary>
        /// <param name="privateKey">.net生成的私钥</param>
        /// <returns></returns>
        public static string RSAPrivateKeyDotNet2Java(string privateKey)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(privateKey);
            BigInteger m = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("Modulus")[0].InnerText));
            BigInteger exp = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("Exponent")[0].InnerText));
            BigInteger d = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("D")[0].InnerText));
            BigInteger p = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("P")[0].InnerText));
            BigInteger q = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("Q")[0].InnerText));
            BigInteger dp = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("DP")[0].InnerText));
            BigInteger dq = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("DQ")[0].InnerText));
            BigInteger qinv = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("InverseQ")[0].InnerText));

            RsaPrivateCrtKeyParameters privateKeyParam = new RsaPrivateCrtKeyParameters(m, exp, d, p, q, dp, dq, qinv);

            PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKeyParam);
            byte[] serializedPrivateBytes = privateKeyInfo.ToAsn1Object().GetEncoded();
            return Convert.ToBase64String(serializedPrivateBytes);
        }

        /// <summary>
        /// RSA公钥格式转换，java->.net
        /// </summary>
        /// <param name="publicKey">java生成的公钥</param>
        /// <returns></returns>
        public static string RSAPublicKeyJava2DotNet(string publicKey)
        {
            RsaKeyParameters publicKeyParam = (RsaKeyParameters)PublicKeyFactory.CreateKey(Convert.FromBase64String(publicKey));
            return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent></RSAKeyValue>",
                Convert.ToBase64String(publicKeyParam.Modulus.ToByteArrayUnsigned()),
                Convert.ToBase64String(publicKeyParam.Exponent.ToByteArrayUnsigned()));
        }

        /// <summary>
        /// RSA公钥格式转换，.net->java
        /// </summary>
        /// <param name="publicKey">.net生成的公钥</param>
        /// <returns></returns>
        public static string RSAPublicKeyDotNet2Java(string publicKey)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(publicKey);
            BigInteger m = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("Modulus")[0].InnerText));
            BigInteger p = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("Exponent")[0].InnerText));
            RsaKeyParameters pub = new RsaKeyParameters(false, m, p);

            SubjectPublicKeyInfo publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(pub);
            byte[] serializedPublicBytes = publicKeyInfo.ToAsn1Object().GetDerEncoded();
            return Convert.ToBase64String(serializedPublicBytes);
        }

        /// <summary>用私钥给数据进行RSA加密  
        ///   
        /// </summary>  
        /// <param name="strEncryptString">待加密数据</param>  
        /// <returns>加密后的数据（Base64）</returns>  
        public static byte[] RSAEncryptByPrivateKey(string strEncryptString)
        {
            //privateKey = new StringBuilder();
            //privateKey.Append("-----BEGIN PRIVATE KEY-----\r\n");
            //privateKey.Append("MIICdQIBADANBgkqhkiG9w0BAQEFAASCAl8wggJbAgEAAoGBAJhxga5fgW0GT2vP\r\n");
            //privateKey.Append("ZzhWb69uin5mvJA+OG1Zk9vLRETuYDlKqOC+ioqSaLrQVYwXl/9DJqgtsyz+jHUu\r\n");
            //privateKey.Append("Fpq95O9Zs6P+zMDnwxco6Qmdd3bdA65syAb0xvvwhmAocQ2LtwxokWYhbD8wR8ci\r\n");
            //privateKey.Append("lciED1llHrTmpCHZjt/hTM1RjmWpAgMBAAECgYBdhZU3cuQmDK8AxxbeGzkdeUWB\r\n");
            //privateKey.Append("0mlwXJulikUJnKRfXZauzzzX1E2OQc/6fAFflsHrGjlHB/JacjedVA8OE920eMXQ\r\n");
            //privateKey.Append("SblZsqmS3w0vpvLv/YYE8TaWDFdo+TdNfx1YfuIn8D+7TbCkDj9mHbIia3CqvWTY\r\n");
            //privateKey.Append("VFjlImq9AgV3mhFXmQJBAPe4UStZQm6UzystUcJKKYLRKCznIoCkZCndDrl5l7J2\r\n");
            //privateKey.Append("0WdzhgpbXA324CWkOM/UZCJbpYPttl3lgP7E/U4gp88CQQCdie31qHkPp8H+4QaK\r\n");
            //privateKey.Append("kHjJV+JcyeLRV376ZkrVmcaVWMo5zLjFGEmdb5PC/NfiqnJdAp6NEnryb7M7mtCQ\r\n");
            //privateKey.Append("XgEHAkA9pzMUfd+p1AGHtnzNxVV1iIbePhx5NfDz9l+uy9N0iFvvynHG7BwKIbKB\r\n");
            //privateKey.Append("y9CT8UGGx5Z3MlecIP2s6uw8YKZDAkAbql48ltJzul0qGnBgoxBjI2jgLKAFbV0i\r\n");
            //privateKey.Append("MhaQPkoObiJNVjNYgXRkDnUfd/Gdn5sn1E7trUIkExOhswVPZQK/AkAcFmwB0WcU\r\n");
            //privateKey.Append("kFs2Q3nZYPwTnCfX/q4pFk3YMmbSyA8lQZzj4vfJoG3Jt4ibaTC1HbmnyLeQLp2b\r\n");
            //privateKey.Append("iuFty6OGUCxl\r\n");
            //privateKey.Append("-----END PRIVATE KEY-----");

            StringBuilder _sbPrivateKey = new StringBuilder();
            _sbPrivateKey.Append("MIICdQIBADANBgkqhkiG9w0BAQEFAASCAl8wggJbAgEAAoGBAJhxga5fgW0GT2vP");
            _sbPrivateKey.Append("ZzhWb69uin5mvJA+OG1Zk9vLRETuYDlKqOC+ioqSaLrQVYwXl/9DJqgtsyz+jHUu");
            _sbPrivateKey.Append("Fpq95O9Zs6P+zMDnwxco6Qmdd3bdA65syAb0xvvwhmAocQ2LtwxokWYhbD8wR8ci");
            _sbPrivateKey.Append("lciED1llHrTmpCHZjt/hTM1RjmWpAgMBAAECgYBdhZU3cuQmDK8AxxbeGzkdeUWB");
            _sbPrivateKey.Append("0mlwXJulikUJnKRfXZauzzzX1E2OQc/6fAFflsHrGjlHB/JacjedVA8OE920eMXQ");
            _sbPrivateKey.Append("SblZsqmS3w0vpvLv/YYE8TaWDFdo+TdNfx1YfuIn8D+7TbCkDj9mHbIia3CqvWTY");
            _sbPrivateKey.Append("VFjlImq9AgV3mhFXmQJBAPe4UStZQm6UzystUcJKKYLRKCznIoCkZCndDrl5l7J2");
            _sbPrivateKey.Append("0WdzhgpbXA324CWkOM/UZCJbpYPttl3lgP7E/U4gp88CQQCdie31qHkPp8H+4QaK");
            _sbPrivateKey.Append("kHjJV+JcyeLRV376ZkrVmcaVWMo5zLjFGEmdb5PC/NfiqnJdAp6NEnryb7M7mtCQ");
            _sbPrivateKey.Append("XgEHAkA9pzMUfd+p1AGHtnzNxVV1iIbePhx5NfDz9l+uy9N0iFvvynHG7BwKIbKB");
            _sbPrivateKey.Append("y9CT8UGGx5Z3MlecIP2s6uw8YKZDAkAbql48ltJzul0qGnBgoxBjI2jgLKAFbV0i");
            _sbPrivateKey.Append("MhaQPkoObiJNVjNYgXRkDnUfd/Gdn5sn1E7trUIkExOhswVPZQK/AkAcFmwB0WcU");
            _sbPrivateKey.Append("kFs2Q3nZYPwTnCfX/q4pFk3YMmbSyA8lQZzj4vfJoG3Jt4ibaTC1HbmnyLeQLp2b");
            _sbPrivateKey.Append("iuFty6OGUCxl");


            String _sXmlPrivateKey = RSAPrivateKeyJava2DotNet(_sbPrivateKey.ToString());

            //加载私钥  
            RSACryptoServiceProvider privateRsa = new RSACryptoServiceProvider();
            privateRsa.FromXmlString(_sXmlPrivateKey);

            //转换密钥  
            AsymmetricCipherKeyPair keyPair = DotNetUtilities.GetKeyPair(privateRsa);

            IBufferedCipher c = CipherUtilities.GetCipher("RSA/ECB/PKCS1Padding");// 参数与Java中加密解密的参数一致       
                                                                                  //第一个参数为true表示加密，为false表示解密；第二个参数表示密钥  
            c.Init(true, keyPair.Private);

            byte[] DataToEncrypt = Encoding.UTF8.GetBytes(strEncryptString);
            byte[] outBytes = c.DoFinal(DataToEncrypt);//加密  
            return outBytes;

            //string strBase64 = Convert.ToBase64String(outBytes);
            //return strBase64;
        }

        /// <summary>
        /// 建立日终对账zip文件
        /// </summary>
        /// <param name="dt">要提交的数据</param>
        /// <returns>文件路径</returns>
        public static String CreateTDRZip(DataTable dt)
        {
            String sFileName = "";//GHB_{商户唯一编号}_{业务类型}_{文件类型}_{操作类型}_{文件日期}_{目前上送的文件序号}.
            //try
            //{
            //    FileStream _fs = new FileStream(sFileName, FileMode.Create);

            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        foreach (String drItem in dr.ItemArray)
            //        {
            //            byte[] bItem = Convert.ToByte(drItem);
            //            _fs.Write(Convert.ToByte(drItem), 0, sStream.Length);
            //        }
                    
            //    }      
            //    _fs.Close();
            //}
            //catch (Exception e)
            //{
            //    LogHelper.LOG("File_" + DateTime.Now.ToString("yyyyMMdd"), MethodBase.GetCurrentMethod().Name, " ", "", "", "", e.StackTrace);
            //}

            return sFileName;
        }

        /// <summary>
        /// 读取文件数据流进行Base64编码
        /// </summary>
        /// <param name="sFileName"></param>
        /// <returns></returns>
        public static String FileToStream(String sFileName)
        {
            try
            {
                FileStream _fs = new FileStream(sFileName, FileMode.Open);
                //获取文件大小
                long size = _fs.Length;
                byte[] array = new byte[size];
                _fs.Read(array, 0, array.Length);
                _fs.Close();
                return Convert.ToBase64String(array);
            }
            catch(Exception e)
            {
                LogHelper.LOG("File_" + DateTime.Now.ToString("yyyyMMdd"), MethodBase.GetCurrentMethod().Name, " ", "", "", "", e.StackTrace);
                return String.Empty;
            }
            
        }

        /// <summary>
        /// Base64解码后生成文件
        /// </summary>
        /// <param name="sBase64Str"></param>
        public static void StreamToFile(String sFileName,String sBase64Str)
        {
            try
            {
                byte[] sStream = Convert.FromBase64String(sBase64Str);
                FileStream _fs = new FileStream(sFileName, FileMode.Create);
                _fs.Write(sStream, 0, sStream.Length);
                _fs.Close();
            }
            catch (Exception e)
            {
                LogHelper.LOG("File_" + DateTime.Now.ToString("yyyyMMdd"), MethodBase.GetCurrentMethod().Name, " ", "", "", "", e.StackTrace);
            }
        }


    }
}
