using YiDaBus.Com.Manager.Common;
using System.Web.Mvc;

namespace YiDaBus.Com.Manager.Web
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
            if (OperatorProvider.Provider.GetCurrent() == null)
            {
                WebHelper.WriteCookie("nfine_login_error", "overdue");
                string hrefUrl = string.Empty;
                if (IsMobileDevice(filterContext))
                {
                    hrefUrl = "/Login/MobileIndex";
                }
                else
                {
                    hrefUrl = "/Login/Index";
                }
                filterContext.HttpContext.Response.Write("<script>top.location.href = '" + hrefUrl + "';</script>");
                return;
            }
        }

        public bool IsMobileDevice(AuthorizationContext filterContext)
        {
            string[] mobileAgents = { "iphone", "android", "phone", "mobile", "wap", "netfront", "java", "opera mobi", "opera mini", "ucweb", "windows ce", "symbian", "series", "webos", "sony", "blackberry", "dopod", "nokia", "samsung", "palmsource", "xda", "pieplus", "meizu", "midp", "cldc", "motorola", "foma", "docomo", "up.browser", "up.link", "blazer", "helio", "hosin", "huawei", "novarra", "coolpad", "webos", "techfaith", "palmsource", "alcatel", "amoi", "ktouch", "nexian", "ericsson", "philips", "sagem", "wellcom", "bunjalloo", "maui", "smartphone", "iemobile", "spice", "bird", "zte-", "longcos", "pantech", "gionee", "portalmmm", "jig browser", "hiptop", "benq", "haier", "^lct", "320x320", "240x320", "176x220", "w3c ", "acs-", "alav", "alca", "amoi", "audi", "avan", "benq", "bird", "blac", "blaz", "brew", "cell", "cldc", "cmd-", "dang", "doco", "eric", "hipt", "inno", "ipaq", "java", "jigs", "kddi", "keji", "leno", "lg-c", "lg-d", "lg-g", "lge-", "maui", "maxo", "midp", "mits", "mmef", "mobi", "mot-", "moto", "mwbp", "nec-", "newt", "noki", "oper", "palm", "pana", "pant", "phil", "play", "port", "prox", "qwap", "sage", "sams", "sany", "sch-", "sec-", "send", "seri", "sgh-", "shar", "sie-", "siem", "smal", "smar", "sony", "sph-", "symb", "t-mo", "teli", "tim-", "tosh", "tsm-", "upg1", "upsi", "vk-v", "voda", "wap-", "wapa", "wapi", "wapp", "wapr", "webc", "winw", "winw", "xda", "xda-", "Googlebot-Mobile" };
            bool isMoblie = false;
            if (filterContext.HttpContext.Request.UserAgent.ToString().ToLower() != null)
            {
                for (int i = 0; i < mobileAgents.Length; i++)
                {
                    if (filterContext.HttpContext.Request.UserAgent.ToString().ToLower().IndexOf(mobileAgents[i]) >= 0)
                    {
                        isMoblie = true;
                        break;
                    }
                }
            }
            return isMoblie;
        }
    }
}