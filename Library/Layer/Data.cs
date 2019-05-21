


namespace IECS.Library
{
    /// <summary>
    /// 数据逻辑层 运用Cache DB 等取得数据 该层表明具体的SQL语句或数据逻辑、算法等
    /// 常规用法：将SQL语句或数据逻辑、算法等在该类中封装为public函数 在业务层以函数名为缓存键值获取数据
    /// </summary>
    class Data
    {
        /// <summary>
        /// 数据逻辑层 运用Cache DB 等取得数据 该层表明具体的SQL语句或数据逻辑、算法等
        /// 常规用法：将SQL语句或数据逻辑、算法等在该类中封装为public函数 在业务层以函数名为缓存键值获取数据
        /// </summary>
        public Data() { }
        

        private static string Get()
        {
            
            return "Get";
        }
        public static string Get2()
        {
            return "Get2";
        }
        public static int Get3()
        {
            return 2;
        }


    }
}
