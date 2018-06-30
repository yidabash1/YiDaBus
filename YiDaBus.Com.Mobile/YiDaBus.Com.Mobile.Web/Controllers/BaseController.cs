using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP;
using Senparc.Weixin.HttpUtility;

using YiDaBus.Com.Manager.Common;
using Senparc.Weixin.Exceptions;
using System.IO;
using YiDaBus.Com.Mobile.Model.Const;

namespace YiDaBus.Com.Mobile.Web.Controllers
{
    public class BaseController : Controller
    {

        /// <summary>
        /// 获取接口域名（末尾带反斜杠）
        /// </summary>
        /// <returns></returns>
        protected string getInterFaceDomain()
        {
            return Configs.GetValue(YiDaBusConst.INTERFACE_DOMAIN);
        }
        #region 返回数据
        /// <summary>
        /// 返回错误消息
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        protected virtual ActionResult Error(int code, string message = "")
        {
            return Content(new AjaxResult { status = code, msg = message }.ToJson());
        }
        /// <summary>
        /// 提示alert信息，然后跳转
        /// </summary>
        /// <param name="message">提示信息</param>
        /// <param name="responseUrl">跳转页面</param>
        /// <returns></returns>
        protected virtual ActionResult Alert(string message, string responseUrl = "/home/index")
        {
            return Content("<script>alert('" + message + "');window.location.href='" + responseUrl + "';</script>");
        }

        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        protected virtual ActionResult Success(int code, object data, string message = "")
        {
            return Content(new AjaxResult { status = code, msg = message, data = data }.ToJson());
        }


        #endregion

        #region 保存文件
        protected virtual void SaveFile(MemoryStream docStream, string fileName, string ContentType = "application/msword;charset=utf-8")
        {
            Response.AppendHeader("content-disposition", "attachment;filename=" + fileName);
            Response.Charset = "UTF-8";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.ContentType = ContentType;
            HttpContext.Response.ClearContent();
            Response.BinaryWrite(docStream.ToArray());
            HttpContext.Response.End();
        }
        protected virtual void SaveFile(byte[] byteFile, string fileName, string ContentType = "application/msword;charset=utf-8")
        {
            Response.AppendHeader("content-disposition", "attachment;filename=" + fileName);
            Response.Charset = "UTF-8";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.ContentType = ContentType;
            HttpContext.Response.ClearContent();
            Response.BinaryWrite(byteFile);
            HttpContext.Response.End();
        }
        #endregion

        #region 获取openId
        protected string GetOpenId()
        {
            string openid = WebHelper.GetCookie("openid");
            if (string.IsNullOrEmpty(openid))
            {
                var state = "ZFX-" + DateTime.Now.Millisecond;//随机数，用于识别请求可靠性
                Session["State"] = state;//储存随机数到Session
                //此页面引导用户点击授权
                string redirectURL = OAuthApi.GetAuthorizeUrl(Configs.GetValue("WeixinAppId"),
                   Configs.GetValue("wxDomain") + "/oauth2/UserInfoCallback?returnUrl=" + Request.Url.AbsoluteUri.UrlEncode(),
                   state, OAuthScope.snsapi_userinfo);
                Response.Redirect(redirectURL);
            }
            return openid;
        }
        protected string OnlyGetOpenId()
        {
            string openid = WebHelper.GetCookie("openid");
            if (string.IsNullOrEmpty(openid))
            {
                var state = "ZFX-" + DateTime.Now.Millisecond;//随机数，用于识别请求可靠性
                Session["State"] = state;//储存随机数到Session
                //此页面引导用户点击授权
                string redirectURL = OAuthApi.GetAuthorizeUrl(Configs.GetValue("WeixinAppId"),
                   Configs.GetValue("wxDomain") + "/oauth2/BaseCallback?returnUrl=" + Request.Url.AbsoluteUri.UrlEncode(),
                   state, OAuthScope.snsapi_userinfo);
                Response.Redirect(redirectURL);
            }
            return openid;
        }
        #endregion
    }
}