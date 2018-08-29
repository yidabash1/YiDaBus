using NFine.Application;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using YiDaBus.Com.Dal.Base;
using YiDaBus.Com.Manager.Common;
using YiDaBus.Com.Manager.Common.Excel;
using YiDaBus.Com.Mobile.BLL;
using YiDaBus.Com.Mobile.Model.Const;
using YiDaBus.Com.Mobile.Model.Enum;
using YiDaBus.Com.Model;

namespace YiDaBus.Com.Manager.Web.Areas.SystemSecurity.Controllers
{
    public class AppLogController : ControllerBase
    {
        public override string tableName { get; set; } = "Wx_APPLogs";//表名
        public override string f_ModuleName { get; set; } = "微信端日志";//表名
        public async Task<ActionResult> GetGridJson(Pagination pagination)
        {
            string sqlWhere = " WHERE 1=1 ";
            string UserName = Request["UserName"];
            if (!string.IsNullOrEmpty(UserName))
            {
                sqlWhere += $" AND UserName = '{UserName}' ";
            }

            string ActionType = Request["ActionType"];
            if (!string.IsNullOrEmpty(ActionType))
            {
                sqlWhere += $" AND ActionType = '{ActionType}' ";
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


            string sql = string.Format(@"SELECT * FROM Wx_APPLogs {0} {1} ", sqlWhere, sqlOrder);

            return getListByPaging<Wx_APPLogs>(Db.MySqlContext, sql, pagination);
        }
    }
}