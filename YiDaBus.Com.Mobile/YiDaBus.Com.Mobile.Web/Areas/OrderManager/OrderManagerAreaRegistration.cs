using System.Web.Mvc;

namespace YiDaBus.Com.Mobile.Web.Areas.OrderManager
{
    public class OrderManagerAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "OrderManager";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "OrderManager_default",
                "OrderManager/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}