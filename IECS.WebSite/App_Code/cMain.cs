using IECS.Library;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

/// <summary>
/// cMain 的摘要说明
/// </summary>
public class cMain : Page
{
    public IECSFun IECS = null;

    public cMain()
    {
        IECS = new IECSFun();
    }

}