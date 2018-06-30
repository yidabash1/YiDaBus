using Aspose.Words;
using Newtonsoft.Json;
using Senparc.Weixin.MP.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using YiDaBus.Com.Manager.Common;
using YiDaBus.Com.Mobile.Model;
using YiDaBus.Com.Mobile.Web.App_Start;
namespace YiDaBus.Com.Mobile.Web.Controllers
{
    public class OrderController : BaseController
    {
        private string appId = ConfigurationManager.AppSettings["WeixinAppId"];
        private string secret = ConfigurationManager.AppSettings["WeixinAppSecret"];
        // GET: Order
        [HandlerLogin(false)]
        public ActionResult Index()
        {
            return View();
        }

    }
}