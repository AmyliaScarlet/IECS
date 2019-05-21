using System;
using System.Data;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Reflection;
//using IECS.Library;

namespace IECS.Library
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class OleDbHelper : LogHelper
    {
        /// <summary>
        /// OleDbHelper
        /// </summary>
        public OleDbHelper()
        {

        }

        /// <summary>
        /// 选择连接的数据库连接字符
        /// </summary>
        private static String GetConnExcel8String(String sFilePath)
        {
            String _sConn = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                "Data Source=" + sFilePath + ";" +
                "Extended Properties='Excel 8.0;HDR=YES;IMEX=1;'";
            return _sConn;
        }
        /// <summary>
        /// 释放数据库连接资源
        /// </summary>
        /// <param name="Conn"></param>
        public static void Dispose(OleDbDataAdapter Conn)
        {
            if (Conn != null)
            {
                Conn.Dispose();
            }
            GC.Collect();
        }

        /// <summary>
        /// 导出Execl数据为 DataSet
        /// </summary>
        public static void GetDataSetForExcel8(string sFilePath, string sTableName, ref DataSet Ds)
        {
            String _sConn = GetConnExcel8String(sFilePath);
            String _sSQL = "SELECT * FROM [" + sTableName + "$]";
            OleDbDataAdapter _ExcelDa = new OleDbDataAdapter(_sSQL, _sConn);

            DataSet ExcelDs = new DataSet();
            try
            {
                _ExcelDa.Fill(Ds, sTableName);

            }
            catch (Exception ex)
            {
                LogHelper.LOG("DB", MethodBase.GetCurrentMethod().Name, ex.Source.ToString(), "", _sSQL, "", ex.Message.ToString());
            }
            finally
            {
                Dispose(_ExcelDa);
            }
        }
    }
}
