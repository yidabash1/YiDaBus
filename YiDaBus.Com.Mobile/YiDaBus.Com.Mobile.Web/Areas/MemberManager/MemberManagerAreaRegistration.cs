using System.Web.Mvc;

namespace YiDaBus.Com.Mobile.Web.Areas.MemberManager
{
    public class MemberManagerAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "MemberManager";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "MemberManager_default",
                "MemberManager/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}