using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QL_Tour_Du_Lich.App_Start
{
    public class InitializeData
    {
        public static void Initiallize()
        {
            HttpContext.Current.Session["adminlogin"] = false;
            HttpContext.Current.Session["emailadminlogin"] = "";
            HttpContext.Current.Session["email"] = "";
        }
    }
}