using System;
using System.Data;
using System.Reflection;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
//using IECS.Library;
using System.Collections;

namespace IECS.Library
{
    /// <summary>
    /// ���ݿ������ ֻ�������ݿ�����߼� �ò㲻����SQL�������ô�����
    /// </summary>
    public abstract class SQLHelper : LogHelper
    {
        private static byte[] KEY_64 = { 19, 77, 12, 02, 19, 76, 06, 03 };
        private static byte[] IV_64 = { 19, 76, 06, 03, 19, 77, 12, 02 };

        /// <summary>����Ӧ�ó���������ݿ�</summary>
        private static readonly string P_ConnectionString = DESHelper.GetDESDecrypt((ConfigurationManager.AppSettings["DBConnection"] == null ? "" : ConfigurationManager.AppSettings["DBConnection"].ToString()), KEY_64, IV_64).Replace("<SERVERNAME>", System.Net.Dns.GetHostName());

        /// <summary>
        /// 
        /// </summary>
        public SQLHelper()
        {

        }

        /// <summary>
        /// ѡ�����ӵ����ݿ������ַ�
        /// </summary>
        private static String GetConnString()
        {
            return P_ConnectionString;
        }

        /// <summary>
        /// �ͷ����ݿ�������Դ
        /// </summary>
        /// <param name="Conn"></param>
        public static void Dispose(SqlConnection Conn)
        {
            if (Conn != null)
            {
                Conn.Close();
                Conn.Dispose();
            }
            GC.Collect();
        }

        #region ��ѯ����ʵ����

        /// <summary>
        /// ��ȡ����������¼ʵ����
        /// </summary>
        /// <param name="sSQL"></param>
        /// <param name="objModel"></param>
        /// <returns></returns>
        public static void ExecSQL_GetModel(String sSQL, ref object objModel)
        {
            SqlConnection _Conn = new SqlConnection(GetConnString());
            SqlDataReader _rdr;
            try
            {
                _Conn.Open();
                SqlCommand _Cmd = new SqlCommand(sSQL, _Conn);
                _rdr = _Cmd.ExecuteReader(CommandBehavior.SingleRow);
                if (_rdr.Read())
                {
                    for (int i = 0; i < _rdr.FieldCount; i++)
                    {
                        PropertyInfo _PI = objModel.GetType().GetProperty(_rdr.GetName(i));
                        if (_PI != null)
                        {
                            if (_rdr.GetValue(i) != DBNull.Value)
                            {
                                if (_PI.PropertyType.IsEnum)
                                    _PI.SetValue(objModel, Enum.ToObject(_PI.PropertyType, _rdr.GetValue(i)), null);
                                else
                                    _PI.SetValue(objModel, _rdr.GetValue(i), null);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                LogHelper.LOG("DB", MethodBase.GetCurrentMethod().Name, ex.Number.ToString(), "", sSQL, "", ex.Message.ToString());
            }
        }

        /// <summary>
        /// ��ȡ����������¼ʵ����
        /// </summary>
        /// <param name="sSQL"></param>
        /// <param name="objModel"></param>
        /// <param name="objModel2"></param>
        /// <returns></returns>
        public static void ExecSQL_GetModel(String sSQL, ref object objModel, ref object objModel2)
        {
            SqlConnection _Conn = new SqlConnection(GetConnString());
            SqlDataReader _rdr;
            try
            {
                _Conn.Open();
                SqlCommand _Cmd = new SqlCommand(sSQL, _Conn);
                _rdr = _Cmd.ExecuteReader(CommandBehavior.SingleRow);
                if (_rdr.Read())
                {
                    for (int i = 0; i < _rdr.FieldCount; i++)
                    {
                        PropertyInfo _PI = objModel.GetType().GetProperty(_rdr.GetName(i));
                        if (_PI != null)
                        {
                            if (_rdr.GetValue(i) != DBNull.Value)
                            {
                                if (_PI.PropertyType.IsEnum)
                                    _PI.SetValue(objModel, Enum.ToObject(_PI.PropertyType, _rdr.GetValue(i)), null);
                                else
                                    _PI.SetValue(objModel, _rdr.GetValue(i), null);
                            }
                        }
                        PropertyInfo _PI2 = objModel2.GetType().GetProperty(_rdr.GetName(i));
                        if (_PI2 != null)
                        {
                            if (_rdr.GetValue(i) != DBNull.Value)
                            {
                                if (_PI2.PropertyType.IsEnum)
                                    _PI2.SetValue(objModel2, Enum.ToObject(_PI2.PropertyType, _rdr.GetValue(i)), null);
                                else
                                    _PI2.SetValue(objModel2, _rdr.GetValue(i), null);
                            }
                        }

                    }
                }
            }
            catch (SqlException ex)
            {
                LogHelper.LOG("DB", MethodBase.GetCurrentMethod().Name, ex.Number.ToString(), "", sSQL, "", ex.Message.ToString());
            }
        }

        /// <summary>
        /// ��ȡ����������¼ʵ����
        /// </summary>
        /// <param name="sSQL"></param>
        /// <param name="objModel"></param>
        /// <param name="objModel2"></param>
        /// <param name="objModel3"></param>
        /// <returns></returns>
        public static void ExecSQL_GetModel(String sSQL, ref object objModel, ref object objModel2, ref object objModel3)
        {
            SqlConnection _Conn = new SqlConnection(GetConnString());
            SqlDataReader _rdr;
            try
            {
                _Conn.Open();
                SqlCommand _Cmd = new SqlCommand(sSQL, _Conn);
                _rdr = _Cmd.ExecuteReader(CommandBehavior.SingleRow);
                if (_rdr.Read())
                {
                    for (int i = 0; i < _rdr.FieldCount; i++)
                    {
                        PropertyInfo _PI = objModel.GetType().GetProperty(_rdr.GetName(i));
                        if (_PI != null)
                        {
                            if (_rdr.GetValue(i) != DBNull.Value)
                            {
                                if (_PI.PropertyType.IsEnum)
                                    _PI.SetValue(objModel, Enum.ToObject(_PI.PropertyType, _rdr.GetValue(i)), null);
                                else
                                    _PI.SetValue(objModel, _rdr.GetValue(i), null);
                            }
                        }
                        PropertyInfo _PI2 = objModel2.GetType().GetProperty(_rdr.GetName(i));
                        if (_PI2 != null)
                        {
                            if (_rdr.GetValue(i) != DBNull.Value)
                            {
                                if (_PI2.PropertyType.IsEnum)
                                    _PI2.SetValue(objModel2, Enum.ToObject(_PI2.PropertyType, _rdr.GetValue(i)), null);
                                else
                                    _PI2.SetValue(objModel2, _rdr.GetValue(i), null);
                            }
                        }
                        PropertyInfo _PI3 = objModel3.GetType().GetProperty(_rdr.GetName(i));
                        if (_PI3 != null)
                        {
                            if (_rdr.GetValue(i) != DBNull.Value)
                            {
                                if (_PI3.PropertyType.IsEnum)
                                    _PI3.SetValue(objModel3, Enum.ToObject(_PI3.PropertyType, _rdr.GetValue(i)), null);
                                else
                                    _PI3.SetValue(objModel3, _rdr.GetValue(i), null);
                            }
                        }

                    }
                }
            }
            catch (SqlException ex)
            {
                LogHelper.LOG("DB", MethodBase.GetCurrentMethod().Name, ex.Number.ToString(), "", sSQL, "", ex.Message.ToString());
            }
        }

        /// <summary>
        /// ��ȡ����������¼�� List ʵ���� 
        /// </summary>
        /// <param name="sSQL"></param>
        /// <returns></returns>
        public static List<T> ExecSQL_GetModelList<T>(String sSQL)
        {
            SqlConnection _Conn = new SqlConnection(GetConnString());
            SqlDataReader _rdr;
            List<T> lstModelList = new List<T>();
            try
            {
                _Conn.Open();
                SqlCommand _Cmd = new SqlCommand(sSQL, _Conn);
                _rdr = _Cmd.ExecuteReader();
                while (_rdr.Read())
                {
                    T _oModel = (T)(Assembly.GetAssembly(typeof(T))).CreateInstance(typeof(T).FullName);
                    for (int i = 0; i < _rdr.FieldCount; i++)
                    {
                        PropertyInfo _PI = _oModel.GetType().GetProperty(_rdr.GetName(i));
                        if (_PI != null)
                        {
                            if (_rdr.GetValue(i) != DBNull.Value)
                            {
                                if (_PI.PropertyType.IsEnum)
                                    _PI.SetValue(_oModel, Enum.ToObject(_PI.PropertyType, _rdr.GetValue(i)), null);
                                else
                                    _PI.SetValue(_oModel, _rdr.GetValue(i), null);
                            }
                        }
                    }
                    lstModelList.Add(_oModel);
                }
            }
            catch (SqlException ex)
            {
                LogHelper.LOG("DB", MethodBase.GetCurrentMethod().Name, ex.Number.ToString(), "", sSQL, "", ex.Message.ToString());
            }
            return lstModelList;
        }

        /// <summary>
        /// ��ȡ����������¼�� List ʵ���� (��ҳ������ʹ��)
        /// </summary>
        /// <param name="sSQL"></param>
        /// <param name="nRowCount"></param>
        /// <param name="nPageCount"></param>
        /// <param name="nCurrentPage"></param>
        /// <returns></returns>
        public static List<T> ExecSQL_GetModelList<T>(String sSQL, ref int nRowCount, ref int nPageCount, ref int nCurrentPage)
        {
            SqlConnection _Conn = new SqlConnection(GetConnString());
            SqlDataReader _rdr;
            List<T> lstModelList = new List<T>();
            try
            {
                _Conn.Open();
                SqlCommand _Cmd = new SqlCommand(sSQL, _Conn);
                _rdr = _Cmd.ExecuteReader();
                if (_rdr.Read())
                {
                    nRowCount = Int32.Parse(_rdr["lngRowCount"].ToString());
                    nPageCount = Int32.Parse(_rdr["lngPageCount"].ToString());
                    nCurrentPage = Int32.Parse(_rdr["lngCurrentPage"].ToString());
                }
                _rdr.NextResult();
                while (_rdr.Read())
                {
                    T _oModel = (T)(Assembly.GetAssembly(typeof(T))).CreateInstance(typeof(T).FullName);
                    for (int i = 0; i < _rdr.FieldCount; i++)
                    {
                        PropertyInfo _PI = _oModel.GetType().GetProperty(_rdr.GetName(i));
                        if (_PI != null)
                        {
                            if (_rdr.GetValue(i) != DBNull.Value)
                            {
                                if (_PI.PropertyType.IsEnum)
                                    _PI.SetValue(_oModel, Enum.ToObject(_PI.PropertyType, _rdr.GetValue(i)), null);
                                else
                                    _PI.SetValue(_oModel, _rdr.GetValue(i), null);
                            }
                        }
                    }
                    lstModelList.Add(_oModel);
                }
            }
            catch (SqlException ex)
            {
                LogHelper.LOG("DB", MethodBase.GetCurrentMethod().Name, ex.Number.ToString(), "", sSQL, "", ex.Message.ToString());
            }
            return lstModelList;
        }

        #endregion

        /// <summary> 
        /// �����Ƿ�������� 
        /// </summary> 
        /// <returns></returns> 
        public static bool Exists(String sSQL)
        {
            SqlConnection _Conn = new SqlConnection(GetConnString());
            SqlDataReader _dr;
            Boolean _bResult = false;

            try
            {
                _Conn.Open();
                SqlCommand _Cmd = new SqlCommand(sSQL, _Conn);
                _dr = _Cmd.ExecuteReader();
                if (_dr.Read())
                {
                    _bResult = true;
                }
                _dr.Close();
            }
            catch (SqlException ex)
            {
                LogHelper.LOG("DB", MethodBase.GetCurrentMethod().Name, ex.Number.ToString(), "", sSQL, "", ex.Message.ToString());
            }
            finally
            {
                Dispose(_Conn);
            }
            return _bResult;
        }

        ///<summary>
        ///����SQL��䣬ֻ������Ӱ�������
        ///</summary>
        ///<param name="sSQL"></param>
        ///<returns></returns>
        public static Int32 ExecSQL_NonQuery(String sSQL)
        {
            SqlConnection _Conn = new SqlConnection(GetConnString());
            Int32 _nResult = -1;

            try
            {
                _Conn.Open();
                SqlCommand _Cmd = new SqlCommand(sSQL, _Conn);
                _nResult = _Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LogHelper.LOG("DB", MethodBase.GetCurrentMethod().Name, ex.Source.ToString(), "", sSQL, "", ex.Message.ToString());
            }
            finally
            {
                Dispose(_Conn);
            }
            return _nResult;
        }

        /// <summary>  
        /// ��ȡBLOB�ֶε��ļ�,�����ļ���
        /// </summary>  
        /// <param name="idValue">����ֵ</param>  
        /// <param name="idField">�����ֶ�����</param>  
        /// <param name="table">Ҫ��ѯ�ı�����</param>  
        /// <param name="blobField">����ļ����ֶ�����</param>  
        /// <param name="outFileFullName">���浽���ص��ļ���</param>  
        /// <returns></returns>  
        public static bool ExecSQL_BlobToFile(string idValue, string idField, string table, string blobField, string outFileFullName)
        {
            int PictureCol = 0;
            SqlConnection _Conn = new SqlConnection(GetConnString());
            outFileFullName = outFileFullName.Trim();

            try
            {
                SqlCommand cmd = new SqlCommand("Select " + blobField + " From " + table +
                    " Where " + idField + "='" + idValue + "'", _Conn);

                SqlDataReader myReader = cmd.ExecuteReader();
                myReader.Read();

                if (myReader.HasRows == false)
                {
                    return false;
                }

                byte[] b = new byte[myReader.GetBytes(PictureCol, 0, null, 0, int.MaxValue) - 1];
                myReader.GetBytes(PictureCol, 0, b, 0, b.Length);
                myReader.Close();

                System.IO.FileStream fileStream = new System.IO.FileStream(
                    outFileFullName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                fileStream.Write(b, 0, b.Length);
                fileStream.Close();
            }
            catch
            {
                return false;
            }
            return true;
        }

        ///<summary>
        ///����SQL��䣬���ز���ID
        ///</summary>
        ///<param name="sSQL"></param>
        ///<returns></returns>
        public static Int32 ExecSQL_GetInsertId(String sSQL)
        {
            SqlConnection _Conn = new SqlConnection(GetConnString());
            int id = -1;
            SqlDataReader _dr;
            try
            {
                _Conn.Open();
                sSQL += ";Select SCOPE_IDENTITY();";

                SqlCommand _Cmd = new SqlCommand(sSQL, _Conn);
                _dr = _Cmd.ExecuteReader();
                if (_dr.Read())
                {
                    id = int.Parse(_dr[0].ToString());
                }
                _dr.Close();

            }
            catch (Exception ex)
            {
                LogHelper.LOG("DB", MethodBase.GetCurrentMethod().Name, ex.Source.ToString(), "", sSQL, "", ex.Message.ToString());
            }
            finally
            {
                Dispose(_Conn);
            }
            return id;
        }


        ///// <summary>
        ///// ִ��sql ����DataRow
        ///// </summary>
        ///// <param name="sqlStr"></param>
        ///// <returns></returns>
        //public static DataRow ExecSQL_GetDataRow(String sqlStr)
        //{

        //    DataSet _ds = new DataSet();
        //    DB.ExecSQL(sqlStr, ref _ds);

        //    if (_ds == null)
        //    {
        //        return null;
        //    }
        //    if (_ds.Tables.Count ==0)
        //    {
        //        return null;
        //    }
        //    if (_ds.Tables[0].Rows.Count == 0)
        //    {
        //        return null;
        //    }
        //    else if (_ds.Tables[0].Rows.Count > 1)
        //    {
        //        //return null;
        //        return _ds.Tables[0].Rows[0];
        //    }

        //    return _ds.Tables[0].Rows[0];
        //}

        /// <summary>
        /// ִ��sql ����DataRow
        /// </summary>
        /// <param name="sSQL"></param>
        /// <returns></returns>
        public static DataRow ExecSQL_GetDataRow(String sSQL)
        {
            DataSet _ds = new DataSet();
            SqlConnection _Conn = new SqlConnection(GetConnString());
            try
            {
                _Conn.Open();
                SqlDataAdapter _da = new SqlDataAdapter(sSQL, _Conn);
                _da.Fill(_ds);
            }
            catch (Exception ex)
            {
                LogHelper.LOG("DB", MethodBase.GetCurrentMethod().Name, ex.Source.ToString(), "", sSQL, "", ex.Message.ToString());
            }
            finally
            {
                Dispose(_Conn);
            }

            if (_ds != null && _ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
            {
                return _ds.Tables[0].Rows[0];
            }

            return null;
        }

        /// <summary>
        /// ִ��sql ����DataTable
        /// </summary>
        /// <param name="sSQL"></param>
        /// <param name="bBackNull"></param>
        /// <returns></returns>
        public static DataTable ExecSQL_GetTable(String sSQL, Boolean bBackNull = true)
        {

            DataSet _ds = new DataSet();
            ExecSQL(sSQL, ref _ds);

            if (_ds.Tables[0].Rows.Count == 0 && bBackNull) return null;

            return _ds.Tables[0];

        }

        /// <summary> 
        /// ����SQL���ִ�н���ĵ�һ�е�һ�� 
        /// </summary> 
        /// <param name="sSQL">SQL ���</param>
        /// <returns>�ַ���</returns> 
        public static string ExecSQL_GetValue(string sSQL)
        {
            SqlConnection _Conn = new SqlConnection(GetConnString());
            string _sResult = string.Empty;
            SqlDataReader _dr;
            try
            {
                _Conn.Open();
                SqlCommand _Cmd = new SqlCommand(sSQL, _Conn);
                _dr = _Cmd.ExecuteReader();
                if (_dr.Read())
                {
                    _sResult = _dr[0].ToString();
                }
                _dr.Close();
            }
            catch (Exception ex)
            {
                LogHelper.LOG("DB", MethodBase.GetCurrentMethod().Name, ex.Source.ToString(), "", sSQL, "", ex.Message.ToString());
            }
            finally
            {
                Dispose(_Conn);
            }
            return _sResult;
        }

        ///<summary>
        ///����SQL��䣬ִ�����ݸ�����䣨���趨SQLԤ�Ƴ�ʱʱ�䣩
        ///</summary>
        ///<param name="nExecTime">��ʱʱ��</param>
        ///<param name="sSQL"></param>
        ///<returns></returns>
        public static Int32 ExecSQL_SetTime(Int32 nExecTime, String sSQL)
        {
            SqlConnection _Conn = new SqlConnection(GetConnString());
            Int32 _nResult = -1;

            try
            {
                _Conn.Open();
                SqlCommand _Cmd = new SqlCommand(sSQL, _Conn);
                _Cmd.CommandTimeout = nExecTime;
                _nResult = _Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LogHelper.LOG("DB", MethodBase.GetCurrentMethod().Name, ex.Source.ToString(), "", sSQL, "", ex.Message.ToString());
            }
            finally
            {
                Dispose(_Conn);
            }
            return _nResult;
        }

        /// <summary> 
        /// ����SQL���,����DataSet���� �����趨SQLԤ�Ƴ�ʱʱ�䣩
        /// </summary> 
        /// <param name="nExecTime"></param>
        /// <param name="sSQL">SQL���</param> 
        /// <param name="Ds">DataSet����</param> 
        public static int ExecSQL_SetTime(Int32 nExecTime, String sSQL, ref DataSet Ds)
        {
            SqlConnection _Conn = new SqlConnection(GetConnString());
            if (Ds == null) Ds = new DataSet();
            Int32 _nResult = -1;

            try
            {
                SqlCommand _Cmd = new SqlCommand(sSQL, _Conn);
                _Cmd.CommandTimeout = nExecTime;
                SqlDataAdapter _da = new SqlDataAdapter(_Cmd);
                _da.Fill(Ds);
                _nResult = 1;
            }
            catch (Exception ex)
            {
                LogHelper.LOG("DB", MethodBase.GetCurrentMethod().Name, ex.Source.ToString(), "", sSQL, "", ex.Message.ToString());
            }
            finally
            {
                Dispose(_Conn);
            }
            return _nResult;
        }

        /// <summary> 
        /// ����SQL���,����DataSet���� 
        /// </summary> 
        /// <param name="sSQL">SQL���</param> 
        /// <param name="Ds">DataSet����</param> 
        public static int ExecSQL(String sSQL, ref DataSet Ds)
        {
            SqlConnection _Conn = new SqlConnection(GetConnString());
            if (Ds == null) Ds = new DataSet();
            Int32 _nResult = -1;

            try
            {
                _Conn.Open();
                SqlDataAdapter _da = new SqlDataAdapter(sSQL, _Conn);
                _da.Fill(Ds);
                _nResult = 1;
            }
            catch (Exception ex)
            {
                LogHelper.LOG("DB", MethodBase.GetCurrentMethod().Name, ex.Source.ToString(), "", sSQL, "", ex.Message.ToString());
            }
            finally
            {
                Dispose(_Conn);
            }
            return _nResult;
        }

        ///// <summary>
        ///// ���ж�̬�̶����SQL���,����DataSet���� 
        ///// </summary>
        ///// <param name="sSQL"></param>
        ///// <param name="sDeepLevel"></param>
        ///// <param name="Ds"></param>
        ///// <returns></returns>
        //public static int ExecDynamicDeepSQL(String sSQL, ref DataSet Ds, int sDeepLevel = 2)
        //{
        //    if (sDeepLevel<2)
        //    {
        //        throw new ArgumentException("DeepLevel Can't < 2");
        //    }
        //    StringBuilder _sb = new StringBuilder();
        //    SqlConnection _Conn = new SqlConnection(GetConnString());
        //    if (Ds == null) Ds = new DataSet();
        //    Int32 _nResult = -1;

        //    try
        //    {
        //        _sb.Append("CREATE TABLE #t0(sqlStr0 varchar(MAX));");
        //        _sb.Append("DECLARE @sqlStr0 VARCHAR(MAX);"); 
        //        _sb.Append("SELECT @sqlStr0 = (" + sSQL + ")");
        //        _sb.Append("INSERT INTO #t0 EXEC(@sqlStr0);");
        //        _sb.Append("SET @sqlStr0 =(select sqlStr0 from #t0);");
        //        _sb.Append("DROP TABLE #t0;");

        //        if (sDeepLevel > 2)
        //        {
        //            for (int i = 1; i < sDeepLevel + 1; i++)
        //            {
        //                _sb.Append("CREATE TABLE #t" + i + "(sqlStr" + i + " varchar(MAX));");
        //                _sb.Append("DECLARE @sqlStr" + i + " VARCHAR(MAX);");
        //                _sb.Append("SELECT @sqlStr" + i + " = (@sqlStr" + (i - 1) + ")");
        //                _sb.Append("INSERT INTO #t" + i + " EXEC(@sqlStr" + (i - 1) + ");");
        //                _sb.Append("SET @sqlStr" + i + " =(select sqlStr0 from #t" + i + ");");
        //                _sb.Append("DROP TABLE #t" + i + ";");        
        //            }
        //            _sb.Append("SELECT @sqlStr" + sDeepLevel + ";");
        //        }
        //        else
        //        {
        //            _sb.Append("SELECT @sqlStr0;");
        //        }

        //        _Conn.Open();
        //        SqlDataAdapter _da = new SqlDataAdapter(_sb.ToString(), _Conn);
        //        _da.Fill(Ds);
        //        _nResult = 1;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.WriterDBLOG(ex.Source.ToString(), ex.Message.ToString(), sSQL);
        //    }
        //    finally
        //    {
        //        Dispose(_Conn);
        //    }
        //    return _nResult;
        //}

        /// <summary> 
        /// ���д洢����,����dataset. 
        /// </summary> 
        /// <param name="procName">�洢������.</param> 
        /// <param name="Ds"></param> 
        /// <returns>dataset����.</returns> 
        public static void ExecProc(string procName, ref DataSet Ds)
        {
            SqlConnection _Conn = new SqlConnection(GetConnString());
            try
            {
                _Conn.Open();
                SqlCommand _Cmd = new SqlCommand(procName, _Conn);
                SqlDataAdapter _da = new SqlDataAdapter(_Cmd);
                _da.Fill(Ds);
            }
            catch (Exception ex)
            {
                LogHelper.LOG("DB", MethodBase.GetCurrentMethod().Name, ex.Source.ToString(), "", procName, "", ex.Message.ToString());
            }
            finally
            {
                Dispose(_Conn);
            }
        }

        ///<summary>
        ///���д洢���̣�ִ�е������ݸ������
        ///</summary>
        ///<param name="procName"></param>
        ///<returns></returns>
        public static Int32 ExecProc(String procName)
        {
            SqlConnection _Conn = new SqlConnection(GetConnString());
            Int32 _nResult = -1;

            try
            {
                _Conn.Open();
                SqlCommand _Cmd = new SqlCommand(procName, _Conn);
                _nResult = _Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LogHelper.LOG("DB", MethodBase.GetCurrentMethod().Name, ex.Source.ToString(), "", procName, "", ex.Message.ToString());
            }
            finally
            {
                Dispose(_Conn);
            }
            return _nResult;
        }

        ///<summary>
        ///���д洢���̣�ִ�е������ݸ������
        ///</summary>
        ///<param name="procName"></param>
        ///<returns></returns>
        public static Int32 ExecProc_GetValue(String procName)
        {
            SqlConnection _Conn = new SqlConnection(GetConnString());
            Int32 _nResult = -1;

            try
            {
                _Conn.Open();
                SqlCommand _Cmd = new SqlCommand(procName, _Conn);
                _nResult = Convert.ToInt32((_Cmd.ExecuteScalar()));
            }
            catch (Exception ex)
            {
                LogHelper.LOG("DB", MethodBase.GetCurrentMethod().Name, ex.Source.ToString(), "", procName, "", ex.Message.ToString());
            }
            finally
            {
                Dispose(_Conn);
            }
            return _nResult;
        }

        ///<summary>
        ///���д洢���̣�ִ�е������ݸ�����䣨���趨SQLԤ�Ƴ�ʱʱ�䣩
        ///</summary>
        /// <param name="sConnName">�����ַ���������</param>
        ///<param name="nExecTime">��ʱʱ��</param>
        ///<param name="procName"></param>
        ///<returns></returns>
        public static Int32 ExecProc(String sConnName, Int32 nExecTime, String procName)
        {
            SqlConnection _Conn = new SqlConnection(GetConnString());
            Int32 _nResult = -1;

            try
            {
                _Conn.Open();
                SqlCommand _Cmd = new SqlCommand(procName, _Conn);
                _Cmd.CommandTimeout = nExecTime;
                _nResult = _Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LogHelper.LOG("DB", MethodBase.GetCurrentMethod().Name, ex.Source.ToString(), "", procName, "", ex.Message.ToString());
            }
            finally
            {
                Dispose(_Conn);
            }
            return _nResult;
        }

        ///// <summary> 
        ///// ��DataReader תΪ DataTable 
        ///// </summary> 
        ///// <param name="reader">DataReader</param> 
        //public static DataTable ConvertDataReaderToDataTable(IDataReader reader)
        //{
        //    DataTable _dt = new DataTable();
        //    int _nFieldCount = reader.FieldCount;
        //    for (int intCounter = 0; intCounter < _nFieldCount; ++intCounter)
        //    {
        //        _dt.Columns.Add(reader.GetName(intCounter), reader.GetFieldType(intCounter));
        //    }

        //    _dt.BeginLoadData();

        //    object[] objValues = new object[_nFieldCount];
        //    while (reader.Read())
        //    {
        //        reader.GetValues(objValues);
        //        _dt.LoadDataRow(objValues, true);
        //    }
        //    reader.Close();
        //    _dt.EndLoadData();

        //    return _dt;
        //}

        ///// <summary> 
        ///// ��DataRow[]  תΪ DataTable 
        ///// </summary> 
        ///// <param name="drs">DataRow[]</param> 
        //public DataTable ConvertDataRowsToDataTable(DataRow[] drs)
        //{
        //    if (drs == null || drs.Length == 0) return null;
        //    DataTable _dtTmp = drs[0].Table.Clone();  // ����DataRow�ı�ṹ
        //    foreach (DataRow _dr in drs)
        //        _dtTmp.Rows.Add(_dr);  // ��DataRow��ӵ�DataTable��
        //    return _dtTmp;
        //}


        /// <summary>
        /// ����DataSet���е����ݵ����ݿ���
        /// </summary>
        /// <param name="sSQL"></param>
        /// <param name="dsChanged"></param>
        /// <returns></returns>
        public static Int32 UpdateDs(String sSQL, DataSet dsChanged)
        {

            SqlConnection _Conn = new SqlConnection(GetConnString());
            Int32 _nResult = -1;
            try
            {
                SqlDataAdapter _da = new SqlDataAdapter(sSQL, _Conn);
                SqlCommandBuilder _cmdb = new SqlCommandBuilder(_da);
                _da.InsertCommand = _cmdb.GetInsertCommand();
                _da.UpdateCommand = _cmdb.GetUpdateCommand();
                _da.DeleteCommand = _cmdb.GetDeleteCommand();

                //SqlCommandBuilder _sqlCmdBuilder = new SqlCommandBuilder(_da);

                _da.Update(dsChanged.Tables[0]);

                dsChanged.AcceptChanges();

                _nResult = 1;
            }
            catch (Exception ex)
            {
                LogHelper.LOG("DB", MethodBase.GetCurrentMethod().Name, ex.Source.ToString(), "", "���� DataSet �����ݿ����", "", ex.Message.ToString());
            }
            finally
            {
                Dispose(_Conn);
            }
            return _nResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sSQL"></param>
        /// <param name="dsChanged"></param>
        /// <returns></returns>
        public static Int32 UpdateDsGetArg(String sSQL, DataSet dsChanged)
        {

            SqlConnection _Conn = new SqlConnection(GetConnString());
            Int32 _nResult = -1;
            try
            {
                SqlDataAdapter _da = new SqlDataAdapter(sSQL, _Conn);
                SqlCommandBuilder _cmdb = new SqlCommandBuilder(_da);
                _da.InsertCommand = _cmdb.GetInsertCommand();
                _da.UpdateCommand = _cmdb.GetUpdateCommand();
                _da.DeleteCommand = _cmdb.GetDeleteCommand();

                //SqlCommandBuilder _sqlCmdBuilder = new SqlCommandBuilder(_da);

                _nResult = _da.Update(dsChanged.Tables[0]);

                dsChanged.AcceptChanges();

            }
            catch (Exception ex)
            {
                LogHelper.LOG("DB", MethodBase.GetCurrentMethod().Name, ex.Source.ToString(), "", "���� DataSet �����ݿ����", "", ex.Message.ToString());
            }
            finally
            {
                Dispose(_Conn);
            }
            return _nResult;
        }

        #region SQLע�ṥ������滻

        /// <summary>
        /// SQLע�ṥ������滻
        /// </summary>
        /// <param name="str">�ַ���</param>
        /// <returns></returns>
        public static string SQLStringCheck(string str)
        {
            if (str == null) return str;

            string _sCheck = "\"'|��";
            char[] _arrCheck = _sCheck.ToCharArray();

            if (str != "" && str.IndexOfAny(_arrCheck) >= 0)
            {
                str = str.Replace("'", "��");
                str = str.Replace("\"", "��");
                str = str.Replace("|", "��");
                str = str.Replace("��", "��");
            }

            return str;
        }

        /// <summary>
        /// SQLע�ṥ������滻 ����б�ܡ�\��
        /// </summary>
        /// <param name="str">�ַ���</param>
        /// <returns></returns>
        public static string SQLStringCheck2(string str)
        {
            string _sCheck = "'|��";
            char[] _arrCheck = _sCheck.ToCharArray();

            if (str != "" && str.IndexOfAny(_arrCheck) >= 0)
            {
                str = str.Replace("'", "��");
                str = str.Replace("|", "��");
                str = str.Replace("��", "��");
            }

            return str;
        }

        /// <summary>
        /// SQLע�ṥ������滻,�޶��ַ�����
        /// </summary>
        /// <param name="str">�ַ���</param>
        /// <param name="nCharLen">�ַ���</param>
        /// <returns></returns>
        public static string SQLStringCheck3(string str, int? nCharLen)
        {
            string _sCheck = "\"'|��";
            char[] _arrCheck = _sCheck.ToCharArray();

            if (str != "" && str.IndexOfAny(_arrCheck) >= 0)
            {
                str = str.Replace("'", "��");
                str = str.Replace("\"", "��");
                str = str.Replace("|", "��");
                str = str.Replace("��", "��");
            }
            if (nCharLen != null)
            {
                str = str.Substring(0, (int)nCharLen);
            }

            return str;
        }

        #endregion

        #region ��ҳ��ѯ

        /// <summary>
        /// ��ҳ SQL ���
        /// </summary>
        public static string FullSQL_Page(string sSelect, string sGetFields, string sFrom, string sWhere, string sOrderBy, int nPageSize, int nPageCurrent)
        {
            int _nRowNumBegin = nPageCurrent * nPageSize - nPageSize + 1;
            int _nRowNumEnd = nPageCurrent * nPageSize;

            StringBuilder _sbSQL = new StringBuilder();
            _sbSQL.Append(sSelect + " COUNT(*) as lngRowCount,ceiling(1.0*COUNT(*)/" + nPageSize.ToString() + ") as lngPageCount, " + nPageCurrent.ToString() + " as lngCurrentPage ");
            _sbSQL.Append(sFrom);
            _sbSQL.Append(sWhere);
            _sbSQL.Append(" ;");
            _sbSQL.Append(" SELECT * FROM ");
            _sbSQL.Append(" (");
            _sbSQL.Append("     " + sSelect);
            _sbSQL.Append("     ROW_NUMBER() OVER(" + sOrderBy + ") IECS 'RowNumber', ");
            _sbSQL.Append("     " + sGetFields);
            _sbSQL.Append("     " + sFrom);
            _sbSQL.Append("     " + sWhere);
            _sbSQL.Append(" ) t ");
            _sbSQL.Append(" WHERE t.RowNumber BETWEEN " + _nRowNumBegin.ToString() + " AND " + _nRowNumEnd.ToString() + " ");

            return _sbSQL.ToString();
        }

        /// <summary>
        /// ��ҳ SQL ��䣬����
        /// </summary>
        public static string FullSQL_ByGroup_Page(string sSelect, string sGetFields, string sFrom, string sWhere, string sOrderBy, string sGroupBy, int nPageSize, int nPageCurrent)
        {
            int _nRowNumBegin = nPageCurrent * nPageSize - nPageSize + 1;
            int _nRowNumEnd = nPageCurrent * nPageSize;

            StringBuilder _sbSQL = new StringBuilder();
            _sbSQL.Append(" SELECT COUNT(*) as lngRowCount,ceiling(1.0*COUNT(*)/" + nPageSize.ToString() + ") as lngPageCount, " + nPageCurrent.ToString() + " as lngCurrentPage ");
            _sbSQL.Append(" FROM ");
            _sbSQL.Append(" ( ");
            _sbSQL.Append(sSelect);
            _sbSQL.Append(sFrom);
            _sbSQL.Append(sWhere);
            _sbSQL.Append(sGroupBy);
            _sbSQL.Append(" ) s ");
            _sbSQL.Append(" ;");
            _sbSQL.Append(" SELECT * FROM ");
            _sbSQL.Append(" (");
            _sbSQL.Append("     " + sSelect);
            _sbSQL.Append("     , ROW_NUMBER() OVER(" + sOrderBy + ") IECS 'RowNumber', ");
            _sbSQL.Append("     " + sGetFields);
            _sbSQL.Append("     " + sFrom);
            _sbSQL.Append("     " + sWhere);
            _sbSQL.Append("     " + sGroupBy);
            _sbSQL.Append(" ) t ");
            _sbSQL.Append(" WHERE t.RowNumber BETWEEN " + _nRowNumBegin.ToString() + " AND " + _nRowNumEnd.ToString() + " ");

            return _sbSQL.ToString();
        }

        #endregion

        /// <summary> 
        /// ��DataReader תΪ DataTable 
        /// </summary> 
        /// <param name="reader">DataReader</param> 
        public static DataTable ConvDataReaderToDataTable(IDataReader reader)
        {
            DataTable _dt = new DataTable();
            int _nFieldCount = reader.FieldCount;
            for (int intCounter = 0; intCounter < _nFieldCount; ++intCounter)
            {
                _dt.Columns.Add(reader.GetName(intCounter), reader.GetFieldType(intCounter));
            }

            _dt.BeginLoadData();

            object[] objValues = new object[_nFieldCount];
            while (reader.Read())
            {
                reader.GetValues(objValues);
                _dt.LoadDataRow(objValues, true);
            }
            reader.Close();
            _dt.EndLoadData();

            return _dt;
        }

        /// <summary> 
        /// ��DataRow[]  תΪ DataTable 
        /// </summary> 
        /// <param name="drs">DataRow[]</param> 
        public static DataTable ConvDataRowsToDataTable(DataRow[] drs)
        {
            if (drs == null || drs.Length == 0) return null;
            DataTable _dt = new DataTable();
            _dt = drs[0].Table.Clone();  // ����DataRow�ı�ṹ
            foreach (DataRow _dr in drs)
                _dt.Rows.Add(_dr.ItemArray);  // ��DataRow��ӵ�DataTable��
            return _dt;
        }

        /// <summary>
        /// ��tDataRow ת��Ϊ Hashtable ��ʽ������ֻ��ת�ַ��ͣ�
        /// </summary>
        /// <param name="Dr"></param>
        /// <returns></returns>
        public static Hashtable ConvDataRowToHashTable(DataRow Dr)
        {
            Hashtable _ht = new Hashtable();
            try
            {
                if (Dr != null)
                {
                    for (int i = 0; i < Dr.Table.Columns.Count; i++)
                    {
                        _ht.Add(Dr.Table.Columns[i].ColumnName, Dr[i].ToString());

                        ////if (Dr.Table.Columns[i].DataType == typeof(bool))
                        //if (Dr.Table.Columns[i].DataType.Equals(System.Type.GetType("System.Boolean")))
                        //{
                        //    //_ht.Add(Dr.Table.Columns[i].ColumnName, Conver.CString(row[i]));
                        //    _ht.Add(Dr.Table.Columns[i].ColumnName, (bool)Dr[i]);
                        //}
                    }
                }
            }
            catch
            {
            }
            return _ht;
        }


        /// <summary>
        /// �Ƚ�����DataSet����������������
        /// </summary>
        /// <param name="oldDataSet">ԭʼDataSet</param>
        /// <param name="nowDataSet">����DataSet</param>
        /// <param name="primaryKey">ԭʼDataSet������DataSet Tables[0]������</param>
        /// <returns>DataSet</returns>
        public DataSet CompareDataSet(DataSet oldDataSet, DataSet nowDataSet, string primaryKey)
        {
            //����nowDataSet�е������У�ʹ֮״̬ΪDataRowState.UnChanged
            nowDataSet.AcceptChanges();
            //Ҫ���ص�DataSet
            DataSet newDataSet = new DataSet();

            //����oldDataSet �� newDataSet Tables[0] ������
            DataColumn dcOld = oldDataSet.Tables[0].Columns[primaryKey];
            DataColumn dcNow = nowDataSet.Tables[0].Columns[primaryKey];
            //��������
            oldDataSet.Tables[0].PrimaryKey = new DataColumn[1] { dcOld };
            nowDataSet.Tables[0].PrimaryKey = new DataColumn[1] { dcNow };

            string primaryValue = string.Empty;
            string where = string.Empty;

            foreach (DataRow drNow in nowDataSet.Tables[0].Rows)
            {
                //ÿ�������У�����������Ӧ��ֵ
                primaryValue = drNow[primaryKey].ToString();
                where = primaryKey + " = '" + primaryValue + "'";

                //��oldDataSet �в��Һ�������ͬ����
                DataRow[] drOld = oldDataSet.Tables[0].Select(where);
                if (drOld.Length == 0)
                {
                    drNow.SetModified();
                }//˵��nowDataSet�е����������������ӵ�����
                else
                {
                    for (int i = 0; i < drNow.ItemArray.Length; i++)
                    {
                        if (drNow.ItemArray.GetValue(i).ToString().Equals(drOld[0].ItemArray.GetValue(i).ToString()) == false)
                        {
                            drNow.SetModified();
                            break;
                        }//˵��nowDataSet�е������������޸Ĺ�������
                    }
                }//�ж������е�ÿһ���Ƿ����
            }

            //װ��DataRowStateΪ Modified����
            newDataSet.Tables.Add(nowDataSet.Tables[0].GetChanges(DataRowState.Modified));

            return newDataSet;
        }

        /// <summary>
        /// ���ݿ�����c#�е��������Ͷ���
        /// </summary>
        /// <param name="sType"></param>
        /// <returns></returns>
        private string GetSqlTypeToCsType(string sType)
        {
            string _nReval = string.Empty;
            switch (sType.ToLower())
            {
                case "int":
                    _nReval = "int32";
                    break;
                case "text":
                    _nReval = "string";
                    break;
                case "bigint":
                    _nReval = "int64";
                    break;
                case "binary":
                    _nReval = "system.byte[]";
                    break;
                case "bit":
                    _nReval = "boolean";
                    break;
                case "char":
                    _nReval = "string";
                    break;
                case "datetime":
                    _nReval = "system.datetime";
                    break;
                case "decimal":
                    _nReval = "system.decimal";
                    break;
                case "float":
                    _nReval = "system.double";
                    break;
                case "image":
                    _nReval = "system.byte[]";
                    break;
                case "money":
                    _nReval = "system.decimal";
                    break;
                case "nchar":
                    _nReval = "string";
                    break;
                case "ntext":
                    _nReval = "string";
                    break;
                case "numeric":
                    _nReval = "system.decimal";
                    break;
                case "nvarchar":
                    _nReval = "string";
                    break;
                case "real":
                    _nReval = "system.single";
                    break;
                case "smalldatetime":
                    _nReval = "system.datetime";
                    break;
                case "smallint":
                    _nReval = "int16";
                    break;
                case "smallmoney":
                    _nReval = "system.decimal";
                    break;
                case "timestamp":
                    _nReval = "system.datetime";
                    break;
                case "tinyint":
                    _nReval = "system.byte";
                    break;
                case "uniqueidentifier":
                    _nReval = "system.guid";
                    break;
                case "varbinary":
                    _nReval = "system.byte[]";
                    break;
                case "varchar":
                    _nReval = "string";
                    break;
                case "variant":
                    _nReval = "object";
                    break;
                default:
                    _nReval = "string";
                    break;
            }
            return _nReval;
        }
    }
}
