using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YiDaBus.Com.Dal.Base;
using YiDaBus.Com.Mobile.Web.Base;
using YiDaBus.Com.Model;

namespace YiDaBus.Com.Mobile.Web.Areas.ShangHaiManager.Controllers
{
    public class ShangHaiController : BaseController
    {
        #region 区域
        /// <summary>
        /// 上海订票
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        [ValidateInput(false)]
        public ActionResult OrderInfo()
        {
            return View();
        }

        public ActionResult Success()
        {
            return View();
        }
        #endregion

        #region 操作
        [HttpPost]
        public ActionResult CreateOrdersInfo(Orders orders)
        {
            return Sucess();
        }
        #endregion
    }
}