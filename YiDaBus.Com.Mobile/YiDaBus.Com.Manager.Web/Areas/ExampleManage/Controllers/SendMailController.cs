/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: NFine
 * Description: 指房向后台管理系统
 * Website：http://www.nfine.cn
*********************************************************************************/
using YiDaBus.Com.Manager.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YiDaBus.Com.Manager.Web.Areas.ExampleManage.Controllers
{
    public class SendMailController : ControllerBase
    {
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult SendMail(string account, string title, string content)
        {
            MailHelper mail = new MailHelper();
            mail.MailServer = Configs.GetValue("MailHost");
            mail.MailUserName = Configs.GetValue("MailUserName");
            mail.MailPassword = Configs.GetValue("MailPassword");
            mail.MailName = "指房向后台管理系统";
            mail.Send(account, title, content);
            return Success("发送成功。");
        }
    }
}
