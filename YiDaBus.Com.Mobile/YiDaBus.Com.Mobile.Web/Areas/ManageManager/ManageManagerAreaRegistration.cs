using System.Web.Mvc;

namespace YiDaBus.Com.Mobile.Web.Areas.ManageManager
{
    public class ManageManagerAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ManageManager";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ManageManager_default",
                "ManageManager/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}