using Senparc.Weixin.HttpUtility;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YiDaBus.Com.Manager.Common;
using YiDaBus.Com.Mobile.Web.App_Start;

namespace YiDaBus.Com.Mobile.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Index1()
        {
            return View();
        }

        #region 上海订票
        /// <summary>
        /// 上海订票
        /// </summary>
        /// <returns></returns>
        public ActionResult ShangHai()
        {
            string viewPath = "~/Views/html/shanghai/index.cshtml";
            return View(viewPath);
        }

        public ActionResult ShangHaiOrderInfo()
        {
            string viewPath = "~/Views/html/shanghai/orderInfo.cshtml";
            return View(viewPath);
        }

        public ActionResult ShangHaiSuccess()
        {
            string viewPath = "~/Views/html/shanghai/success.cshtml";
            return View(viewPath);
        }
        #endregion

        #region 杭州订票
        /// <summary>
        /// 杭州订票
        /// </summary>
        /// <returns></returns>
        public ActionResult HangZhou()
        {
            string viewPath = "~/Views/html/hangzhou/index.cshtml";
            return View(viewPath);
        }

        public ActionResult HangZhouOrderInfo()
        {
            string viewPath = "~/Views/html/hangzhou/orderInfo.cshtml";
            return View(viewPath);
        }

        public ActionResult HangZhouSuccess()
        {
            string viewPath = "~/Views/html/hangzhou/success.cshtml";
            return View(viewPath);
        }
        #endregion
        public ActionResult Login()
        {

            return View();
        }
        public ActionResult LayFormDemo()
        {
            ViewBag.Title = "layer表单例子";
            return View();
        }
        public ActionResult LayModalDemo()
        {
            ViewBag.Title = "layer模态窗口例子";
            return View();
        }
        public ActionResult LaytplDemo()
        {
            ViewBag.Title = "layer模板引擎文档";
            return View();
        }
        public ActionResult TestVideo()
        {
            ViewBag.Title = "测试视频播放";
            return View();
        }
        public ActionResult Activities()
        {
            ViewBag.Title = "活动专题";
            return View();
        }
        public ActionResult Lucene4NetDemo()
        {
            ViewBag.Title = "分词例子";
            return View();
        }

        /// <summary>
        /// TOTO：4月份活动临时使用
        /// </summary>
        /// <returns></returns>
        public ActionResult TempActivity()
        {
            if (!HttpContext.Request.IsAuthenticated)
            {
                return Redirect("/Home/Login"); ;
            }
            return Redirect("/Home/Index");
        }

    }
}