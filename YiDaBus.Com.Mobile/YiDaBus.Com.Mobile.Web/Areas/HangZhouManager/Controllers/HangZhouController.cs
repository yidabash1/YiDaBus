using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YiDaBus.Com.Mobile.Web.Areas.HangZhouManager.Controllers
{
    public class HangZhouController : Controller
    {
        /// <summary>
        /// 杭州订票
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OrderInfo()
        {
            return View();
        }

        public ActionResult Success()
        {
            return View();
        }
    }
}