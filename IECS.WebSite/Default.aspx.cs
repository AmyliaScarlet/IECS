using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using IECS.Library;

public partial class Default : cMain
{

    protected void Page_Load(object sender, EventArgs e)
    {





        //DirectoryInfo di = new DirectoryInfo(@"F:\nsfw_data_scrapper-master\raw_data");
        //FindFile(di);

    }

    public static void DoSQL(String sSQL)
    {
        SqlConnection con = new SqlConnection();
        con.ConnectionString = "server=127.0.0.1;database=the;uid=locthe;pwd=loc";
        con.Open();
        /*
        SqlDataAdapter 对象。 用于填充DataSet （数据集）。
        SqlDataReader 对象。 从数据库中读取流..
        后面要做增删改查还需要用到 DataSet 对象。
        */

        SqlCommand com = new SqlCommand();
        com.Connection = con;
        com.CommandType = CommandType.Text;
        com.CommandText = sSQL;
        SqlDataReader dr = com.ExecuteReader();//执行SQL语句
        dr.Close();//关闭执行
        con.Close();//关闭数据库

    }


    // DirectoryInfo di = new DirectoryInfo(@"D:\Test");
    // FindFile(di); 
    static void FindFile(DirectoryInfo di)
    {
        FileInfo[] fis = di.GetFiles();
        for (int i = 0; i < fis.Length; i++)
        {
            Console.WriteLine("文件：" + fis[i].FullName);
        }
        DirectoryInfo[] dis = di.GetDirectories();
        for (int j = 0; j < dis.Length; j++)
        {
            Console.WriteLine("目录：" + dis[j].FullName);
            FindFile(dis[j]);
        }
    }
}