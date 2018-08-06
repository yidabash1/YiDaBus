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
namespace YiDaBus.Com.Manager.Web.Areas.SystemManage.Controllers
{
    public class TimeEndConfigController : ControllerBase
    {
        public override string tableName { get; set; } = "TimeEndConfig";//表名
        public override string f_ModuleName { get; set; } = "结束配置";//表名

        #region 获取数据
        public async Task<ActionResult> GetGridJson(Pagination pagination)
        {
            string sqlWhere = " WHERE 1=1  ";

            string sqlOrder = string.Empty;
            if (!pagination.sord.IsEmpty() && !pagination.sidx.IsEmpty())
            {
                sqlOrder = $" ORDER BY {pagination.sidx} {pagination.sord} ";
            }
            else
            {
                sqlOrder = $" ORDER BY Id ASC ";
            }


            string sql = string.Format(@"SELECT * FROM TimeEndConfig {0} {1} ", sqlWhere, sqlOrder);

            return getListByPaging<TimeEndConfig>(Db.MySqlContext, sql, pagination);
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
            var dataTable = Db.MySqlContext.From<TimeEndConfig>().Where(TimeEndConfig._.Id == keyValue).ToDataTable();
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
        public ActionResult SubmitForm(string keyValue, TimeEndConfig model)
        {
            OperatorModel op = OperatorProvider.Provider.GetCurrent();
            if (string.IsNullOrEmpty(keyValue))
            {
            }
            else
            {
                model.Id = keyValue.ToInt();
            }
            return SubmitForms(Db.MySqlContext, model, keyValue);
        }
        #endregion
    }
}