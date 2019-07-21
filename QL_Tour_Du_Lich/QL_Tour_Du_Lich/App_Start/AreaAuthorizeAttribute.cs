using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QL_Tour_Du_Lich.App_Start
{
    public class AreaAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string area;

        public AreaAuthorizeAttribute(string area)
        {
            this.area = area;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            string loginUrl = "";

            if (area == "admin")
            {
                loginUrl = "~/TaiKhoan/DangNhap";
            }
            else if (area == "member")
            {
                loginUrl = "~/TaiKhoan/DangNhap";
            }
            filterContext.Result = new RedirectResult(loginUrl);
        }
    }
}