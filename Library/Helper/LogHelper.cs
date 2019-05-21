using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace IECS.Library
{
    /// <summary>
    /// 日志处理队列类
    /// </summary>
    public class LogQueueModel
    {
        /// <summary>日志类型</summary>
        public string sLogType { get; set; }
        /// <summary>来源</summary>
        public string sSource { get; set; }
        /// <summary>错误号</summary>
        public string sErrorNo { get; set; }
        /// <summary>所属页</summary>
        public string sPage { get; set; }
        /// <summary>模块</summary>
        public string sModule { get; set; }
        /// <summary>标题</summary>
        public string sTitle { get; set; }
        /// <summary>消息</summary>
        public string sMessage { get; set; }
    }

    /// <summary>
    /// 日志处理类
    /// </summary>
    public class LogHelper
    {
        #region 新日志

        private static String FLDR_PATH = AppDomain.CurrentDomain.BaseDirectory.ToString() + "..\\LOG\\"; //日志路径
        private static String FILE_NAME = "LOG_{0}.log"; //日志名称
        private static String FILE_BACKUP_NAME = "LOG_{0}_{1}.log"; //日志备份名称
        private static Object ThreadLock = new Object(); //线程是否已开始
        private static Object QueueLock = new Object(); //线程是否已开始
        private static bool IsThreadStart = false; //线程是否已开始
        private static Queue<LogQueueModel> QueueList = new Queue<LogQueueModel>(); //日志队列

        /// <summary>
        /// 写日志到文件
        /// </summary>
        /// <param name="sLogType">日志类型 DB,PG,US,WK,WW</param>
        /// <param name="sSource">来源</param>
        /// <param name="sErrorNo">错误号</param>
        /// <param name="sPage">所属页</param>
        /// <param name="sModule">模块</param>
        /// <param name="sTitle">标题</param>
        /// <param name="sMessage">消息</param>
        public static void LOG(String sLogType, String sSource, String sErrorNo, String sPage, String sModule, String sTitle, String sMessage)
        {
            LogQueueModel _oQueueModel = new LogQueueModel();
            _oQueueModel.sLogType = sLogType;
            _oQueueModel.sSource = sSource;
            _oQueueModel.sErrorNo = sErrorNo;
            _oQueueModel.sPage = sPage;
            _oQueueModel.sModule = sModule;
            _oQueueModel.sTitle = sTitle;
            _oQueueModel.sMessage = sMessage;

            lock (QueueLock)
            {
                //增加到日志队列
                QueueList.Enqueue(_oQueueModel);
            }

            //启动新线程处理日志队列
            ThreadStart();
        }

        /// <summary>队列的处理进程</summary>
        private static void QueueProcess()
        {
            while (QueueList.Count > 0)
            {
                try
                {
                    LogQueueModel _oLogQueueModel;
                    lock (QueueLock)
                    {
                        //从队列中取出的 LogQueueModel
                        _oLogQueueModel = QueueList.Dequeue();
                    }

                    //判断文件夹是否存在, 不存在则创建文件夹 
                    if (!Directory.Exists(FLDR_PATH)) Directory.CreateDirectory(FLDR_PATH);

                    String _sFilePath = String.Format(FLDR_PATH + FILE_NAME, _oLogQueueModel.sLogType); //获得路径
                    if (!File.Exists(_sFilePath))
                    {
                        //日志文件不存在，需要建立
                        StreamWriter _srTemp = File.CreateText(_sFilePath);
                        _srTemp.Close();
                    }
                    else
                    {
                        //文件大小超过 5M 需要备份
                        FileInfo objFI = new FileInfo(_sFilePath);
                        if (objFI.Length > 5120000)
                        {
                            String _sFileBackupPath = String.Format(FLDR_PATH + FILE_BACKUP_NAME, _oLogQueueModel.sLogType, DateTime.Now.ToString("yyyyMMddHHmmss")); //获得备份路径
                            objFI.MoveTo(_sFileBackupPath);
                            StreamWriter sr = File.CreateText(_sFilePath);
                            sr.Close();
                        }
                    }

                    //写入日志
                    StreamWriter srW = File.AppendText(_sFilePath);
                    srW.WriteLine("时间:" + DateTime.Now.ToString());
                    srW.WriteLine("来源:" + _oLogQueueModel.sSource + "，错号:" + _oLogQueueModel.sErrorNo);
                    srW.WriteLine("页号:" + _oLogQueueModel.sPage + "，模块:" + _oLogQueueModel.sModule);
                    srW.WriteLine("标题:" + _oLogQueueModel.sTitle);
                    srW.WriteLine("信息:" + _oLogQueueModel.sMessage);
                    srW.WriteLine("");
                    //srW.Flush();
                    srW.Close();
                }
                catch 
                {
                    
                }
            }
        }

        /// <summary>启动新线程</summary>
        private static void ThreadStart()
        {
            lock (ThreadLock)
            {
                if (IsThreadStart == false)
                {
                    Thread thread = new Thread(ThreadProcess);
                    thread.IsBackground = true;
                    thread.Start();
                    IsThreadStart = true; //标注线程已开始状态
                }
            }
        }

        /// <summary>线程的处理进程</summary>
        private static void ThreadProcess()
        {
            while (true)
            {
                if (QueueList.Count > 0)
                {
                    //队列的处理进程
                    QueueProcess();
                }
                else
                {
                    //没有任务，休息3秒钟  
                    Thread.Sleep(3000);
                }
            }
        }

        #endregion


        //private static String FILE_NAME_DB = AppDomain.CurrentDomain.BaseDirectory.ToString() + "..\\ErrorLOG_DB.log"; //数据库错误
        //private static String FILE_NAME_PG = AppDomain.CurrentDomain.BaseDirectory.ToString() + "..\\ErrorLOG_PG.log"; //程序错误
        //private static String FILE_NAME_US = AppDomain.CurrentDomain.BaseDirectory.ToString() + "..\\ErrorLOG_US.log"; //用户错误
        //private static String FILE_NAME_WK = AppDomain.CurrentDomain.BaseDirectory.ToString() + "..\\ErrorLOG_WK.log"; //工作错误
        //private static String FILE_NAME_WWW = AppDomain.CurrentDomain.BaseDirectory.ToString() + "..\\ErrorLOG_WWW.log"; //网站错误

        ///// <summary>
        ///// 写错误日志到文件 ErrorLOG_PG.log
        ///// </summary>
        ///// <param name="sLogName"></param>
        ///// <param name="sSource"></param>
        ///// <param name="sTitle"></param>
        ///// <param name="sMessage"></param>
        //public static void WriterLOG(String sLogName, String sSource, String sTitle, String sMessage)
        //{

        //  String _sFilePath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "..\\LOG_"+ sLogName + ".log";

        //    //格式化错误信息
        //    if (!File.Exists(_sFilePath))
        //    {
        //        StreamWriter sr = File.CreateText(_sFilePath);
        //        sr.Close();
        //    }
        //    else
        //    {
        //        FileInfo objFI = new FileInfo(_sFilePath);
        //        if (objFI.Length > 5120000)
        //        {
        //            objFI.MoveTo(_sFilePath + "." + DateTime.Now.ToString("yyyyMMdd"));
        //            StreamWriter sr = File.CreateText(_sFilePath);
        //            sr.Close();
        //        }
        //    }

        //    StreamWriter srW = File.AppendText(_sFilePath);
        //    srW.WriteLine("时间:" + DateTime.Now.ToString());
        //    srW.WriteLine("来源:" + sSource);
        //    srW.WriteLine("标题:" + sTitle);
        //    srW.WriteLine("信息:" + sMessage);
        //    srW.WriteLine("");
        //    srW.Close();
        //}



        ///// <summary>
        ///// 写错误日志到文件 ErrorLOG_DB.log
        ///// </summary>
        ///// <param name="sErrorID"></param>
        ///// <param name="sModuleID"></param>
        ///// <param name="sMessage"></param>
        //public static void WriterDBLOG(String sErrorID, String sModuleID, String sMessage)
        //{
        //    //格式化错误信息
        //    //String strErr = DateTime.Now.ToString() + "   错误号:" + vErrorID + "   模块号:" + vModuleID + "   错误描述:" + vMessage;
        //    if (!File.Exists(FILE_NAME_DB))
        //    {
        //        StreamWriter sr = File.CreateText(FILE_NAME_DB);
        //        sr.Close();
        //    }
        //    else
        //    {
        //        FileInfo objFI = new FileInfo(FILE_NAME_DB);
        //        if (objFI.Length > 5120000)
        //        {
        //            objFI.MoveTo(FILE_NAME_DB + "." + DateTime.Now.ToString("yyyyMMdd"));
        //            StreamWriter sr = File.CreateText(FILE_NAME_DB);
        //            sr.Close();
        //        }
        //    }
            
        //    StreamWriter srW = File.AppendText(FILE_NAME_DB);
        //    srW.WriteLine("发生时间:" + DateTime.Now.ToString());
        //    srW.WriteLine("错 误 号:" + sErrorID);
        //    srW.WriteLine("模 块 号:" + sModuleID);
        //    srW.WriteLine("错误描述:" + sMessage);
        //    srW.WriteLine("");
        //    //srW.WriteLine(strErr);
        //    srW.Close();
        //}

        ///// <summary>
        ///// 写错误日志到文件 ErrorLOG_PG.log
        ///// </summary>
        ///// <param name="sErrorPage"></param>
        ///// <param name="sErrorMessage"></param>
        ///// <param name="sErrorSource"></param>
        //public static void WriterPageLOG(String sErrorPage, String sErrorMessage, String sErrorSource)
        //{
        //    //格式化错误信息
        //    if (!File.Exists(FILE_NAME_PG))
        //    {
        //        StreamWriter sr = File.CreateText(FILE_NAME_PG);
        //        sr.Close();
        //    }
        //    else
        //    {
        //        FileInfo objFI = new FileInfo(FILE_NAME_PG);
        //        if (objFI.Length > 5120000)
        //        {
        //            objFI.MoveTo(FILE_NAME_PG + "." + DateTime.Now.ToString("yyyyMMdd"));
        //            StreamWriter sr = File.CreateText(FILE_NAME_PG);
        //            sr.Close();
        //        }
        //    }

        //    StreamWriter srW = File.AppendText(FILE_NAME_PG);
        //    srW.WriteLine("发生时间:" + DateTime.Now.ToString());
        //    srW.WriteLine("异 常 页:" + sErrorPage);
        //    srW.WriteLine("异常信息:" + sErrorMessage);
        //    srW.WriteLine("异 常 源:" + sErrorSource);
        //    srW.WriteLine("");
        //    srW.Close();
        //}

        ///// <summary>
        ///// 写错误日志到文件 ErrorLOG_US.log
        ///// </summary>
        ///// <param name="sErrorMessage"></param>
        ///// <param name="sErrorSource"></param>
        //public static void WriterCheckUserLOG(String sErrorMessage, String sErrorSource)
        //{
        //    //格式化错误信息
        //    if (!File.Exists(FILE_NAME_US))
        //    {
        //        StreamWriter sr = File.CreateText(FILE_NAME_US);
        //        sr.Close();
        //    }
        //    else
        //    {
        //        FileInfo objFI = new FileInfo(FILE_NAME_US);
        //        if (objFI.Length > 5120000)
        //        {
        //            objFI.MoveTo(FILE_NAME_US + "." + DateTime.Now.ToString("yyyyMMdd"));
        //            StreamWriter sr = File.CreateText(FILE_NAME_US);
        //            sr.Close();
        //        }
        //    }

        //    StreamWriter srW = File.AppendText(FILE_NAME_US);
        //    srW.WriteLine("发生时间:" + DateTime.Now.ToString());
        //    srW.WriteLine("异常信息:" + sErrorMessage);
        //    srW.WriteLine("异 常 源:" + sErrorSource);
        //    srW.WriteLine("");
        //    srW.Close();
        //}

        ///// <summary>
        ///// 写错误日志到文件 ErrorLOG_US.log
        ///// </summary>
        ///// <param name="sErrorMessage"></param>
        ///// <param name="sErrorSource"></param>
        //public static void WriterWorkLOG(String sErrorMessage, String sErrorSource)
        //{
        //    //格式化错误信息
        //    if (!File.Exists(FILE_NAME_WK))
        //    {
        //        StreamWriter sr = File.CreateText(FILE_NAME_WK);
        //        sr.Close();
        //    }
        //    else
        //    {
        //        FileInfo objFI = new FileInfo(FILE_NAME_WK);
        //        if (objFI.Length > 5120000)
        //        {
        //            objFI.MoveTo(FILE_NAME_WK + "." + DateTime.Now.ToString("yyyyMMdd"));
        //            StreamWriter sr = File.CreateText(FILE_NAME_WK);
        //            sr.Close();
        //        }
        //    }

        //    StreamWriter srW = File.AppendText(FILE_NAME_WK);
        //    srW.WriteLine("发生时间:" + DateTime.Now.ToString());
        //    srW.WriteLine("异常信息:" + sErrorMessage);
        //    srW.WriteLine("异 常 源:" + sErrorSource);
        //    srW.WriteLine("");
        //    srW.Close();
        //}

        ///// <summary>
        ///// 写错误日志到文件 ErrorLOG_WWW.log
        ///// </summary>
        ///// <param name="sErrorPage"></param>
        ///// <param name="sErrorMessage"></param>
        ///// <param name="sErrorSource"></param>
        //public static void WriterWWWLOG(String sErrorPage, String sErrorMessage, String sErrorSource)
        //{
        //    //格式化错误信息
        //    if (!File.Exists(FILE_NAME_WWW))
        //    {
        //        StreamWriter sr = File.CreateText(FILE_NAME_WWW);
        //        sr.Close();
        //    }
        //    else
        //    {
        //        FileInfo objFI = new FileInfo(FILE_NAME_WWW);
        //        if (objFI.Length > 5120000)
        //        {
        //            objFI.MoveTo(FILE_NAME_WWW + "." + DateTime.Now.ToString("yyyyMMdd"));
        //            StreamWriter sr = File.CreateText(FILE_NAME_WWW);
        //            sr.Close();
        //        }
        //    }

        //    StreamWriter srW = File.AppendText(FILE_NAME_WWW);
        //    srW.WriteLine("发生时间:" + DateTime.Now.ToString());
        //    srW.WriteLine("异 常 页:" + sErrorPage);
        //    srW.WriteLine("异常信息:" + sErrorMessage);
        //    srW.WriteLine("异 常 源:" + sErrorSource);
        //    srW.WriteLine("");
        //    srW.Close();
        //}
    }
}
