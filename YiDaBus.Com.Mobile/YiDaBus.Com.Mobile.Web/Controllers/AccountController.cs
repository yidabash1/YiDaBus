using Newtonsoft.Json;
using Senparc.Weixin.HttpUtility;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using YiDaBus.Com.UtilsModel;
using YiDaBus.Com.Manager.Common;
using YiDaBus.Com.Mobile.Model;
using YiDaBus.Com.Mobile.Web.App_Start;
using YiDaBus.Com.Mobile.Model.ResponseModel;

namespace YiDaBus.Com.Mobile.Web.Controllers
{
    public class AccountController : BaseController
    {
        

        //
        // GET: /JSSDK/


        // GET: Account
        public ActionResult Index()
        {
            ////TOTO by gaoxiang
            //WebHelper.WriteCookie("openid", "0oosadfjjhuyhwjenihkljsareuih");
            //WebHelper.WriteCookie("headimgurl", "~/Content/img/ad6.png");

            //判断是否是微信浏览器，如果是微信浏览器则进行微信授权
            string agent = Request.Headers["User-Agent"];
            if (!(agent.IndexOf("MicroMessenger") > -1))
            {
                return View();
            }

            
            #region 微信授权
            string openid = WebHelper.GetCookie("openid");
            string headimgurl = WebHelper.GetCookie("headimgurl");
            if (string.IsNullOrEmpty(openid) || string.IsNullOrEmpty(headimgurl))
            {
                string wxDomain = Configs.GetValue("wxDomain");
                string appId = Configs.GetValue("WeixinAppId");
                var state = "ZFX-" + DateTime.Now.Millisecond;//随机数，用于识别请求可靠性
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

        #region 登录

        public ActionResult Login(string logAccount, string vaildCode, int vaildId)
        {
            //验证用户登录
            string openid = WebHelper.GetCookie("openid");
            string avatar = WebHelper.GetCookie("headimgurl");
            
            UserLoginResponse currentUser = CheckLogin(logAccount, vaildCode, vaildId, openid, avatar);

            //登录失败
            if (currentUser == null)
            {
                return Json(new { code = 0, msg = "登录失败！" }, JsonRequestBehavior.AllowGet);
            }
            //else if (response.code != "200" && !string.IsNullOrEmpty(response.msg))
            //{
            //    return Json(new { code = 0, msg = !string.IsNullOrEmpty(response.msg) ? response.msg : "登录失败" }, JsonRequestBehavior.AllowGet);
            //}
            //UserData currentUser = response.data;

            //登录成功
            #region  创造票据（1年）、输出到客户端（加密票据）
            //【用户id】
           //创造票据
            FormsAuthenticationTicket userid_ticket = new FormsAuthenticationTicket(currentUser.Id.ToString(), true, 525600);
            //加密票据并输出到客户端
            WebHelper.WriteCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(userid_ticket), 525600);

            //【用户手机号】
            FormsAuthenticationTicket logaccount_ticket = new FormsAuthenticationTicket(currentUser.LogAccount, true, 525600);
            WebHelper.WriteCookie("logaccount", FormsAuthentication.Encrypt(logaccount_ticket), 525600);

            ////【用户头像地址】
            //FormsAuthenticationTicket avatar_ticket = new FormsAuthenticationTicket(avatar, true, 525600);
            //Response.Cookies.Add(new HttpCookie("avatar", FormsAuthentication.Encrypt(avatar_ticket)));
            #endregion

            //如果为空，则跳转到首页
            if (Request["ReturnUrl"].IsEmpty())
            {
                return Json(new
                {
                    code = 200,
                    msg = "登录成功",
                    data = "",
                    userId = DESEncrypt.Encrypt(currentUser.Id.ToString())
                }, JsonRequestBehavior.AllowGet);
            }
            else//否则跳转到登录前页面
            {
                return Json(new
                {
                    code = 200,
                    msg = "登录成功",
                    data = Request["ReturnUrl"] ?? "",
                    userId = DESEncrypt.Encrypt(currentUser.Id.ToString())
                }, JsonRequestBehavior.AllowGet);
            }
        }

        private UserLoginResponse CheckLogin(string logAccount, string vaildCode, int vaildId, string openid, string avatar)
        {
            //通过接口验证用户登录
            string url = getInterFaceDomain() + "api/Users/VaildUserLogin";
            StringBuilder param = new StringBuilder();
            param.AppendFormat("mobileNo={0}", logAccount);
            param.AppendFormat("&verificationId={0}", vaildId);
            param.AppendFormat("&verificationCode={0}", vaildCode);
            param.AppendFormat("&pathWay={0}", 1);
            param.AppendFormat("&OpendId={0}", openid);
            param.AppendFormat("&Avatar={0}", avatar);
            param.AppendFormat("&appID={0}", "");
            string responseData = Utils.HttpPost(url, param.ToString());

            UserLoginResponse currentUser = JsonConvert.DeserializeObject<UserLoginResponse>(responseData);
            return currentUser;
        }
        #endregion

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            try
            {
                //清除授权时的cookie
                WebHelper.RemoveCookie("openid");
                WebHelper.RemoveCookie("logaccount");
                WebHelper.RemoveCookie("avatar");

                FormsAuthentication.SignOut();
                //FormsAuthentication.LoginUrl
                return Json(new
                {
                    code = 200,
                    msg = "退出成功！",
                    data = Request["ReturnUrl"] ?? ""
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 0,
                    msg = "退出失败！",
                    data = ""
                }, JsonRequestBehavior.AllowGet);
            }

            //return RedirectToAction("~/Home/Index");
        }

        /// <summary>
        /// 账号信息
        /// </summary>
        /// <returns></returns>
        public ActionResult MyAccount()
        {
            return View();
        }

        /// <summary>
        /// 账号信息-编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult MyAccountEdit()
        {
            ViewData["type"] = Request["type"] ?? "";
            ViewData["uname"] = Request["uname"] ?? "";
            ViewData["ugender"] = Request["ugender"] ?? "";
            return View();
        }

        /// <summary>
        /// 我的优惠券
        /// </summary>
        /// <returns></returns>
        [HandlerLogin]
        public ActionResult MyCoupon()
        {
            return View();
        }

        /// <summary>
        /// 我的消息
        /// </summary>
        /// <returns></returns>
        [HandlerLogin]
        public ActionResult Message()
        {
            return View();
        }

        /// <summary>
        /// 设置密码
        /// </summary>
        /// <returns></returns>
        [HandlerLogin]
        public ActionResult SetPassword()
        {
            return View();
        }

        /// <summary>
        /// 我的收藏
        /// </summary>
        /// <returns></returns>
        [HandlerLogin]
        public ActionResult MyCollect()
        {
            return View();
        }

        /// <summary>
        /// 我的合同
        /// </summary>
        /// <returns></returns>
        [HandlerLogin]
        public ActionResult MyContract()
        {
            return View();
        }


        /// <summary>
        /// 我的交易保障金
        /// </summary>
        /// <returns></returns>
        [HandlerLogin]
        public ActionResult MyDeposit()
        {
            return View();
        }
        /// <summary>
        /// 交易保障金详细
        /// </summary>
        /// <returns></returns>
        [HandlerLogin]
        public ActionResult DepositDetail()
        {
            return View();
        }
        /// <summary>
        /// 退款成功
        /// </summary>
        /// <returns></returns>
        [HandlerLogin]
        public ActionResult DRefundSuc()
        {
            return View();
        }
        
        /// <summary>
        /// 我的投诉
        /// </summary>
        /// <returns></returns>
        [HandlerLogin]
        public ActionResult MyComplaint()
        {
            return View();
        }

        /// <summary>
        /// 帮助与反馈
        /// </summary>
        /// <returns></returns>
        public ActionResult Feedback()
        {
            return View();
        }

        /// <summary>
        /// 帮助与反馈-详情
        /// </summary>
        /// <returns></returns>
        public ActionResult FeedbackDetail(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        /// <summary>
        /// 我要反馈
        /// </summary>
        /// <returns></returns>
        public ActionResult FeedbackAdd()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        /// <summary>
        /// 新增反馈结果提示
        /// </summary>
        /// <returns></returns>
        [HandlerLogin]
        public ActionResult FeedbackAddResult()
        {
            return View();
        }

        /// <summary>
        /// 房东中心
        /// </summary>
        /// <returns></returns>
        [HandlerLogin]
        public ActionResult LandlordCenter()
        {
            return View();
        }
        /// <summary>
        /// 房东根据房源获取用户出价列表详细
        /// </summary>
        /// <returns></returns>
        [HandlerLogin]
        public ActionResult LPriceList()
        {
            return View();
        }

        /// <summary>
        /// 我的房屋
        /// </summary>
        /// <returns></returns>
        [HandlerLogin]
        public ActionResult MyHouse()
        {
            return View();
        }
        /// <summary>
        /// 房东的租赁合同
        /// </summary>
        /// <returns></returns>
        [HandlerLogin]
        public ActionResult MyLContract()
        {
            return View();
        }
        /// <summary>
        /// 房东的我的约看
        /// </summary>
        /// <returns></returns>
        [HandlerLogin]
        public ActionResult MyLBooking()
        {
            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }

        /// <summary>
        /// 更新后台用户的openid
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateUserOpenId()
        {
            string openid = WebHelper.GetCookie("openid");
            if (string.IsNullOrEmpty(openid))
            {
                string wxDomain = Configs.GetValue("wxDomain");
                string appId = Configs.GetValue("WeixinAppId");
                var state = "ZFX-" + DateTime.Now.Millisecond;//随机数，用于识别请求可靠性
                Session["State"] = state;//储存随机数到Session
                //此页面引导用户点击授权
                string redirectURL = OAuthApi.GetAuthorizeUrl(appId,
                   wxDomain + "/oauth2/UserInfoCallback?returnUrl=" + Request.Url.AbsoluteUri.UrlEncode(),
                   state, OAuthScope.snsapi_userinfo);
                return Redirect(redirectURL);
            }
            ViewData["openid"] = openid;
            ViewData["userId"] = Request["userId"];
            return View();
        }
    }

}