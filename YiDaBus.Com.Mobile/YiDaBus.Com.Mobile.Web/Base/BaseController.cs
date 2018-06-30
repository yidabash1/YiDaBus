using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YiDaBus.Com.Dal.Base;
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
        public ActionResult Error(string msg = "操作失败",int code = -1,object data = null)
        {
            return Json(new
            {
                code = code,
                msg = msg,
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
        #endregion
    }
}