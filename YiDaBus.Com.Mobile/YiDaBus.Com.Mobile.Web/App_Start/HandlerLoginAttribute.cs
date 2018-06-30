using YiDaBus.Com.Manager.Common;
using System.Web.Mvc;
using System.Web;

namespace YiDaBus.Com.Mobile.Web.App_Start
{
    public class HandlerLoginAttribute : AuthorizeAttribute
    {
        public bool Ignore = true;
        public HandlerLoginAttribute(bool ignore = true)
        {
            Ignore = ignore;
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (Ignore == false)
            {
                return;
            }
            if (!filterContext.HttpContext.Request.IsAuthenticated)
            {
                filterContext.HttpContext.Response.Write("<script>window.location.href = '/MemberManager/Member/MemberInfo?ReturnUrl=" + HttpUtility.UrlEncode(filterContext.HttpContext.Request.Url.AbsoluteUri) +"';</script>");
                return;                
            }
        }
    }
}