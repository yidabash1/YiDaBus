using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using YiDaBus.Com.Dal.Base;
using YiDaBus.Com.Manager.Common;
using YiDaBus.Com.Mobile.Model.Enum;
using YiDaBus.Com.Model;

namespace YiDaBus.Com.Mobile.Web.Base
{
    public class BaseController : Controller
    {
        #region 返回方法
        public ActionResult Sucess(string msg = "操作成功", int code = 200, object data = null)
        {
            return Json(new
            {
                code = code,
                msg = msg,
                data = data
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Error(string msg = "操作失败", int code = -1, object data = null)
        {
            return Json(new
            {
                code = code,
                msg = msg,
                data = data
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Error(ErrCode errcode, object data = null)
        {
            return Json(new
            {
                code = (int)errcode,
                msg = errcode.ToString(),
                data = data
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 公用方法
        /// <summary>
        /// 根据OpenId获取用户ID  
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public Wx_Users GetUserByOpenId(string openid)
        {
            return Db.MySqlContext.From<Wx_Users>().Where(d => d.OpenId == openid && d.IsDel == (int)IsDel.否).First();
        }

        //从Request中解析出Ticket,UserData
        public int GetUserId()
        {
            // 1. 读登录Cookie
            var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie == null || string.IsNullOrEmpty(cookie.Value)) return -1;

            try
            {
                // 2. 解密Cookie值，获取FormsAuthenticationTicket对象
                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                if (ticket != null && !string.IsNullOrEmpty(ticket.Name))
                {
                    return ticket.Name.ToInt();
                }
                return -1;
            }
            catch
            {
                /* 有异常也不要抛出，防止攻击者试探。 */
                return -1;
            }
        }

        public Wx_Users GetUserInfo()
        {
            // 1. 读登录Cookie
            var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie == null || string.IsNullOrEmpty(cookie.Value)) return null;

            try
            {
                // 2. 解密Cookie值，获取FormsAuthenticationTicket对象
                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                if (ticket != null && !string.IsNullOrEmpty(ticket.UserData))
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<Wx_Users>(ticket.UserData);
                }
                return null;
            }
            catch
            {
                /* 有异常也不要抛出，防止攻击者试探。 */
                return null;
            }
        }

        public DateTime GetDateTimeByWeek(int week)
        {
            var curDateTime = DateTime.Now;
            string weekstr = curDateTime.DayOfWeek.ToString();
            int curWeek = (int)((WeekEn)Enum.Parse(typeof(WeekEn), weekstr));//获取当前是星期几
            var day = week - curWeek;
            curDateTime.AddDays(day);
            return curDateTime;
        }
        #endregion
    }
}