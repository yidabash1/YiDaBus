using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using YiDaBus.Com.Dal.Base;
using YiDaBus.Com.Manager.Common;
using YiDaBus.Com.Mobile.Model.Enum;
using YiDaBus.Com.Model;

namespace YiDaBus.Com.Manager.Web.Areas.UserManage.Controllers
{
    public class UserController : ControllerBase
    {
        public override string tableName { get; set; } = "Wx_Users";//表名
        public override string f_ModuleName { get; set; } = "微信用户";//表名

        #region 获取数据
        public async Task<ActionResult> GetGridJson(Pagination pagination)
        {
            string sqlWhere = " WHERE 1=1  AND Isdel = 0";
            string Mobile = Request["Mobile"];
            if (!string.IsNullOrEmpty(Mobile))
            {
                sqlWhere += $" AND Mobile = '{Mobile}' ";
            }

            string UserNickName = Request["UserNickName"];
            if (!string.IsNullOrEmpty(UserNickName))
            {
                sqlWhere += $" AND UserNickName = '{UserNickName}' ";
            }

            
            string sqlOrder = string.Empty;
            if (!pagination.sord.IsEmpty() && !pagination.sidx.IsEmpty())
            {
                sqlOrder = $" ORDER BY {pagination.sidx} {pagination.sord} ";
            }
            else
            {
                sqlOrder = $" ORDER BY CreateTime DESC ";
            }


            string sql = string.Format(@"SELECT * FROM Wx_Users {0} {1} ", sqlWhere, sqlOrder);

            return getListByPaging<Wx_Users>(Db.MySqlContext, sql, pagination);
        }
        
        /// <summary>
        /// 表单赋值
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue, string actiontype)
        {
            var dataTable = Db.MySqlContext.From<Wx_Users>().Where(Wx_Users._.Id == keyValue).ToDataTable();
            string contents = dataTable.ToJsonByColName();
            return Content(contents);
        }
        #endregion

        #region 操作数据
        /// <summary>
        /// 新增更新
        /// </summary>
        /// <param name="model"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult SubmitForm(string keyValue, Wx_Users model)
        {
            OperatorModel op = OperatorProvider.Provider.GetCurrent();
            if (string.IsNullOrEmpty(keyValue))
            {
                model.CreateTime = DateTime.Now;
                model.UpdateTime = DateTime.Now;
            }
            else
            {
                model.Id = keyValue.ToInt();
                model.UpdateTime = DateTime.Now;
            }
            return SubmitForms(Db.MySqlContext, model, keyValue);
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            return DeleteFormBySql(Db.MySqlContext, keyValue, "Wx_Users", true);
        }
        #endregion
    }
}