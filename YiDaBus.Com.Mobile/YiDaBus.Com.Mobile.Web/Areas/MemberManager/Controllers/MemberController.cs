using Senparc.Weixin.HttpUtility;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using YiDaBus.Com.Dal.Base;
using YiDaBus.Com.Manager.Common;
using YiDaBus.Com.Mobile.BLL;
using YiDaBus.Com.Mobile.Model.Const;
using YiDaBus.Com.Mobile.Model.Enum;
using YiDaBus.Com.Mobile.Web.Base;
using YiDaBus.Com.Model;

namespace YiDaBus.Com.Mobile.Web.Areas.MemberManager.Controllers
{
    public class MemberController : BaseController
    {
        //Cookie保存是时间
        private const int CookieSaveDays = 365;
        #region 视图
        /// <summary>
        /// 个人信息
        /// </summary>
        /// <returns></returns>
        public ActionResult ClearCache()
        {
            return View();
        }
        /// <summary>
        /// 个人信息
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberInfo()
        {
            //WebHelper.WriteCookie("openid", "0oosadfjjhuyhwjenihkljsareuih");
            //WebHelper.WriteCookie("nickname", "开启我亲爱的小耗子");
            //WebHelper.WriteCookie("headimgurl", "~/Content/img/ad6.png");

            //判断是否是微信浏览器，如果是微信浏览器则进行微信授权
            //string agent = Request.Headers["User-Agent"];
            //if (!(agent.IndexOf("MicroMessenger") > -1))
            //{
            //    return View();
            //}
            #region 微信授权
            string openid = WebHelper.GetCookie("openid");
            string nickname = WebHelper.GetCookie("nickname");
            string headimgurl = WebHelper.GetCookie("headimgurl");
            if (string.IsNullOrEmpty(openid) || string.IsNullOrEmpty(headimgurl) || string.IsNullOrEmpty(nickname))
            {
                string wxDomain = Configs.GetValue("wxDomain");
                string appId = Configs.GetValue("WeixinAppId");
                var state = "YDBS-" + DateTime.Now.Millisecond;//随机数，用于识别请求可靠性
                Session["State"] = state;//储存随机数到Session
                //此页面引导用户点击授权
                string redirectURL = OAuthApi.GetAuthorizeUrl(appId,
                   wxDomain + "/oauth2/UserInfoCallback?returnUrl=" + Request.Url.AbsoluteUri.UrlEncode(),
                   state, OAuthScope.snsapi_userinfo);
                return Redirect(redirectURL);
            }
            #endregion

            return View();
        }

        /// <summary>
        /// 我的车票
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberTicket()
        {
            return View();
        }
        public ActionResult MemberTicketDetail()
        {
            return View();
        }

        /// <summary>
        /// 投诉热线
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberComplaint()
        {
            ViewBag.ComplaintsHotline = CommonBLL.GetGlobalConstVariable(YiDaBusConst.投诉热线).FirstOrDefault()?.F_Description;
            return View();
        }
        #endregion

        #region 操作
        [HttpPost]
        public ActionResult UpdateUserInfo(Wx_Users wx_Users)
        {
            //验证用户登录
            string openid = WebHelper.GetCookie("openid");
            string nickname = WebHelper.GetCookie("nickname");
            wx_Users.UserName = wx_Users.Mobile;
            //去数据库中查询，如果有就更新，如果没有就新增
            var currentUser = GetUserByOpenId(openid);
            if (currentUser == null)//如果没有就新增
            {
                wx_Users.WxNickName = nickname;
                wx_Users.CreateTime = DateTime.Now;
                wx_Users.UpdateTime = DateTime.Now;
                wx_Users.IsDel = (int)IsDel.否;
                wx_Users.OpenId = openid;
                int r = Db.MySqlContext.Insert<Wx_Users>(wx_Users);
                if (r <= 0)
                {
                    return Error("创建账号失败");
                }
                currentUser = wx_Users;
                currentUser.Id = r;
            }
            else//如果有就更新
            {
                wx_Users.OpenId = openid;
                currentUser.WxNickName = nickname;
                currentUser.UserName = wx_Users.UserName;
                currentUser.UserNickName = wx_Users.UserNickName;
                currentUser.Gender = wx_Users.Gender;
                currentUser.UpdateTime = DateTime.Now;
                int r = Db.MySqlContext.Update<Wx_Users>(currentUser);
                if (r <= 0)
                {
                    return Error("更新账号信息失败");
                }
            }


            //登录成功
            #region  创造票据（1年）、输出到客户端（加密票据）
            //【用户id】
            //创造票据
            FormsAuthenticationTicket userid_ticket = new FormsAuthenticationTicket(2, currentUser.Id.ToString(), DateTime.Now, DateTime.Now.AddDays(CookieSaveDays), true, currentUser.ToJson());
            //加密票据并输出到客户端
            WebHelper.WriteCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(userid_ticket), CookieSaveDays * 24 * 60);

            ////【用户手机号】
            //FormsAuthenticationTicket logaccount_ticket = new FormsAuthenticationTicket(currentUser.UserName, true, 525600);
            //WebHelper.WriteCookie("logaccount", FormsAuthentication.Encrypt(logaccount_ticket), 525600);

            #endregion


            //如果为空，则跳转到首页
            if (Request["ReturnUrl"].IsEmpty())
            {
                return Json(new
                {
                    code = 200,
                    msg = "保存成功",
                    data = "",
                    userId = DESEncrypt.Encrypt(currentUser.Id.ToString())
                }, JsonRequestBehavior.AllowGet);
            }
            else//否则跳转到登录前页面
            {
                return Json(new
                {
                    code = 200,
                    msg = "保存成功",
                    data = Request["ReturnUrl"] ?? "",
                    userId = DESEncrypt.Encrypt(currentUser.Id.ToString())
                }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult GetUserIdByOpenId(Wx_Users wx_Users)
        {
            //验证用户登录
            string openid = WebHelper.GetCookie("openid");

            var currentUser = GetUserByOpenId(openid);

            return Json(new
            {
                code = 200,
                msg = "操作成功",
                data = "",
                userId = currentUser == null ? "" : DESEncrypt.Encrypt(currentUser.Id.ToString())
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetUserInfo()
        {
            var currentUser = base.GetUserInfo();

            return Json(new
            {
                code = 200,
                msg = "登录成功",
                data = currentUser,
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ClearCaches(Wx_Users wx_Users)
        {
            //验证用户登录
            WebHelper.RemoveCookie("openid");
            WebHelper.RemoveCookie("nickname");
            WebHelper.RemoveCookie("headimgurl");
            WebHelper.RemoveCookie(".ASPXAUTH");
            return Json(new
            {
                code = 200,
                msg = "清除成功",
                data = "",
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}