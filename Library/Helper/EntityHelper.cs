using System;
using System.Collections.Generic;
using System.Reflection;

namespace IECS.Library
{
    /// <summary>
    /// 实体类的相关处理
    /// </summary>
    public class EntityHelper
    {
        #region 比较差异

        /// <summary>
        /// 遍历类的属性及属性的值
        /// </summary>
        /// <returns>返回 true 有不同</returns>
        public static Boolean GetClassCompare<T>(T tNew, T tOld)
        {
            string tStr = string.Empty;
            if (tNew == null || tOld == null)
            {
                return false;
            }
            System.Reflection.PropertyInfo[] _arrPINew = tNew.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            System.Reflection.PropertyInfo[] _arrPIOld = tOld.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            if (_arrPINew.Length <= 0 || _arrPIOld.Length <= 0 || _arrPINew.Length != _arrPIOld.Length)
            {
                return false;
            }
            for (Int32 i = 0; i < _arrPINew.Length; i++)
            {
                System.Reflection.PropertyInfo _PINew = _arrPINew[i];
                System.Reflection.PropertyInfo _PIOld = _arrPIOld[i];
                if (_PINew.PropertyType.IsValueType || _PINew.PropertyType.Name.StartsWith("String"))
                {
                    object _oNew = _PINew.GetValue(tNew, null);
                    object _oOld = _PIOld.GetValue(tOld, null);
                    if (!_oNew.Equals(_oOld))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 遍历获取类的属性及属性的值
        /// </summary>
        /// <returns>返回 List 包含 类名称/旧值/新值/类类型</returns>
        public static List<String[]> GetClassDiffer<T>(T tNew, T tOld)
        {
            List<String[]> _oDiffer = new List<String[]>();
            string tStr = string.Empty;
            if (tNew != null && tOld != null)
            {
                System.Reflection.PropertyInfo[] _arrPINew = tNew.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                System.Reflection.PropertyInfo[] _arrPIOld = tOld.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

                if (_arrPINew.Length > 0 && _arrPIOld.Length > 0 && _arrPINew.Length == _arrPIOld.Length)
                {
                    for (Int32 i = 0; i < _arrPINew.Length; i++)
                    {
                        System.Reflection.PropertyInfo _PINew = _arrPINew[i];
                        System.Reflection.PropertyInfo _PIOld = _arrPIOld[i];
                        if (_PINew.PropertyType.IsValueType || _PINew.PropertyType.Name.StartsWith("String"))
                        {
                            object _oNew = _PINew.GetValue(tNew, null);
                            object _oOld = _PIOld.GetValue(tOld, null);
                            if (!_oNew.Equals(_oOld))
                            {
                                String[] _arrDiffer = { "", "", "", "" };
                                _arrDiffer[0] = _PINew.Name;
                                _arrDiffer[1] = _oOld.ToString();
                                _arrDiffer[2] = _oNew.ToString();
                                _arrDiffer[3] = _PINew.PropertyType.Name;
                                _oDiffer.Add(_arrDiffer);
                            }
                        }
                    }
                }
            }

            return _oDiffer;
        }

        /// <summary>
        /// 遍历获取类的拷贝
        /// </summary>
        /// <returns>返回 List 包含 类名称/旧值/新值/类类型</returns>
        public static void GetClassCopy<T>(T tOld, ref T tNew)
        {
            if (tNew != null && tOld != null)
            {
                PropertyInfo[] _arrPINew = tNew.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                PropertyInfo[] _arrPIOld = tOld.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                
                if (_arrPIOld.Length > 0 && _arrPINew.Length == _arrPIOld.Length) //&& _arrPIOld.Length > 0
                {
                    for (Int32 i = 0; i < _arrPIOld.Length; i++)
                    {
                        if (_arrPIOld[i].PropertyType.IsValueType || _arrPIOld[i].PropertyType.Name.StartsWith("String"))
                        {
                            //if (tNew.GetType().GetProperty(_arrPIOld[i].Name) != null)
                            //{
                                tNew.GetType().GetProperty(_arrPIOld[i].Name).SetValue(tNew, _arrPIOld[i].GetValue(tOld, null), null);
                            //}
                            //else
                            //{
                            //    sErrInfo = "反射错误：" + _arrPIOld[i].PropertyType.Name + ";" + _arrPIOld[i].Name + ";" + _arrPIOld[i].GetValue(tOld, null).ToString();
                            //}

                            ////if (_arrPINew[i].PropertyType.Name == _arrPIOld[i].PropertyType.Name && _arrPINew[i].Name == _arrPIOld[i].Name)
                            ////{
                            ////    _arrPINew[i].SetValue(tNew, _arrPIOld[i].GetValue(tOld, null), null);
                            ////}
                            ////else
                            ////{
                            ////    sErrInfo = "反射错误：" + _arrPINew[i].PropertyType.Name + ":" + _arrPIOld[i].PropertyType.Name + ";" + _arrPINew[i].Name + ":" + _arrPIOld[i].Name + ";" + _arrPIOld[i].GetValue(tOld, null).ToString();
                            ////}

                        }
                    }
                }
            }
        }

        #endregion

    }
}
