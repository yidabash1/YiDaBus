using System.Web.Mvc;

namespace YiDaBus.Com.Mobile.Web.Areas.HangZhouManager
{
    public class HangZhouManagerAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "HangZhouManager";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "HangZhouManager_default",
                "HangZhouManager/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}