<%@ WebHandler Language="C#" Class="CssHandler" %>

using System;
using System.Web;
using System.IO;
using IECS.Library;
using System.Reflection;
using System.Collections.Generic;
using System.Threading;

public class CssHandler : IHttpHandler {

    public void ProcessRequest (HttpContext context)
    {
        byte[] byts = new byte[context.Request.InputStream.Length];
        context.Request.InputStream.Read(byts, 0, byts.Length);
        string req = System.Text.Encoding.Default.GetString(byts);//Unicode 解码    

        ThreadPool.QueueUserWorkItem((arg) =>
        {
            CssHandlerData _oChd = Util.JsonStrToObj<CssHandlerData>(req);
            try
            {
                List<string> lines = new List<string>(File.ReadAllLines(HttpRuntime.AppDomainAppPath.ToString()+"css/Main.css"));//先读取到内存
                if (_oChd.mode.Equals(1))
                {
                    lines.Add("@import './PageCss/"+_oChd.pageName+".css';");
                }
                else
                {
                    int lineCount = 0;
                    foreach (string str in lines)
                    {
                        if (str.Contains("@import './PageCss/"+_oChd.pageName+".css';"))
                        {
                            lines.RemoveAt(lineCount);//指定删除的行
                            break;
                        }
                        lineCount++;
                    }

                }
                File.WriteAllLines(HttpRuntime.AppDomainAppPath.ToString()+"css/Main.css",lines.ToArray());//写回硬盤

                context.Response.Write("1");
            }
            catch (Exception ex){
                LogHelper.LOG("CssHandler", MethodBase.GetCurrentMethod().Name, "", _oChd.pageName,_oChd.mode.ToString(),"",ex.Message);
                context.Response.Write("0");
            }

        });

    }


    public bool IsReusable {
        get {
            return false;
        }
    }

}
internal class CssHandlerData
{
    private string _pageName = String.Empty;
    public string pageName
    {
        get { return _pageName; }
        set { _pageName = SQLHelper.SQLStringCheck(value.ToLower().Trim()); }
    }

    private int _mode = 0;
    public int mode
    {
        get { return _mode; }
        set { _mode = value; }
    }
}