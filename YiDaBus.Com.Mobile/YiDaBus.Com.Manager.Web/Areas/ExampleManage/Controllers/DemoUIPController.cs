using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace YiDaBus.Com.Manager.Web.Areas.ExampleManage.Controllers
{
    public class DemoUIPController : ControllerBase
    {
        // GET: ExampleManage/DemoUIP
        public override ActionResult Index()
        {
            string methodId = "up2";
            var methodParam = JsonConvert.SerializeObject(new { appid = "3", userId = 1 });
            return Content(UIPPost(methodId, methodParam));
        }
    }
}