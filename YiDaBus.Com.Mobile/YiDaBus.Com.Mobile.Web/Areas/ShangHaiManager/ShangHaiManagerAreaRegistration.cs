using System.Web.Mvc;

namespace YiDaBus.Com.Mobile.Web.Areas.ShangHaiManager
{
    public class ShangHaiManagerAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ShangHaiManager";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ShangHaiManager_default",
                "ShangHaiManager/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}