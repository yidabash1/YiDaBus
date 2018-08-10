using NFine.Application;
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

namespace YiDaBus.Com.Manager.Web.Areas.OrderManage.Controllers
{
    public class OrderController : ControllerBase
    {
        public override string tableName { get; set; } = "Orders";//表名
        public override string f_ModuleName { get; set; } = "订单信息";//表名

        #region 视图
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult GroupMsg()
        {
           
            return View();
        }
        #endregion
        #region 获取数据
        public async Task<ActionResult> GetGridJson(Pagination pagination)
        {
            string sql = GetGridOrderSql(pagination);

            return getListByPaging<OrderExt>(Db.MySqlContext, sql, pagination);
        }

        private string GetGridOrderSql(Pagination pagination)
        {
            string sqlWhere = " WHERE 1=1  ";
            string OrderNo = Request["OrderNo"];
            if (!string.IsNullOrEmpty(OrderNo))
            {
                sqlWhere += $" AND t1.OrderNo = '{OrderNo}' ";
            }

            string CarNumber = HttpUtility.UrlDecode(Request["CarNumber"]);
            if (!string.IsNullOrEmpty(CarNumber))
            {
                sqlWhere += $" AND t1.CarNumber = '{CarNumber}' ";
            }

            string DepartureTimeStart = Request["DepartureTimeStart"];
            if (!string.IsNullOrEmpty(DepartureTimeStart))
            {
                sqlWhere += $" AND t1.DepartureTime >= '{DepartureTimeStart}' ";
            }

            string DepartureTimeEnd = Request["DepartureTimeEnd"];
            if (!string.IsNullOrEmpty(DepartureTimeEnd))
            {
                sqlWhere += $" AND t1.DepartureTime <= '{DepartureTimeEnd}' ";
            }

            string Mobile = Request["Mobile"];
            if (!string.IsNullOrEmpty(Mobile))
            {
                sqlWhere += $" AND t1.Mobile = '{Mobile}' ";
            }

            string IsOneWay = Request["IsOneWay"];
            if (!string.IsNullOrEmpty(IsOneWay))
            {
                sqlWhere += $" AND t1.IsOneWay = '{IsOneWay}' ";
            }

            string IsShuttle = Request["IsShuttle"];
            if (!string.IsNullOrEmpty(IsShuttle))
            {
                sqlWhere += $" AND t1.IsShuttle = '{IsShuttle}' ";
            }


            string PayState = Request["PayState"];
            if (!string.IsNullOrEmpty(PayState))
            {
                sqlWhere += $" AND t1.PayState = '{PayState}' ";
            }

            string FromPosition = HttpUtility.UrlDecode(Request["FromPosition"]);
            if (!string.IsNullOrEmpty(FromPosition))
            {
                sqlWhere += $" AND t1.FromPosition = '{FromPosition}' ";
            }

            string ToPosition = HttpUtility.UrlDecode(Request["ToPosition"]);
            if (!string.IsNullOrEmpty(ToPosition))
            {
                sqlWhere += $" AND t1.ToPosition = '{ToPosition}' ";
            }

            string sqlOrder = string.Empty;
            if (pagination != null && !pagination.sord.IsEmpty() && !pagination.sidx.IsEmpty())
            {
                sqlOrder = $" ORDER BY t1.{pagination.sidx} {pagination.sord} ";
            }
            else
            {
                sqlOrder = $" ORDER BY t1.UpdateTime DESC,t1.CreateTime DESC ";
            }



            string sql = string.Format(@"SELECT t1.*,t2.Gender FROM Orders AS t1
							LEFT JOIN Wx_Users AS t2 ON t1.UserId=t2.Id {0} {1} ", sqlWhere, sqlOrder);
            return sql;
        }

        public class OrderExt : Orders
        {
            public string Gender { get; set; }
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
            var dataTable = Db.MySqlContext.From<Orders>().Where(Orders._.Id == keyValue).ToDataTable();
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
        public ActionResult SubmitForm(string keyValue, Orders model)
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
            return DeleteFormBySql(Db.MySqlContext, keyValue, "Orders", true);
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Audit(string keyValue, int auditStatus)
        {
            try
            {
                if (string.IsNullOrEmpty(keyValue))
                {
                    return Error("参数错误！");
                }
                var orderInfo = Db.MySqlContext.From<Orders>().Where(d => d.Id == keyValue.ToInt()).First();
                if (orderInfo == null)
                {
                    return Error("订单不存在！");
                }
                if (orderInfo.IsDel == (int)IsDel.是)
                {
                    return Error("该订单已被删除！");
                }
                if (orderInfo.PayState == (int)PayState.支付失败)
                {
                    return Error("该订单已被设置为支付失败，请不要重复设置！");
                }
                if (orderInfo.PayState == (int)PayState.支付成功)
                {
                    return Error("该订单已被设置为支付成功，请不要重复设置！");
                }
                orderInfo.PayState = auditStatus;
                int r = Db.MySqlContext.Update(orderInfo);
                if (r > 0)
                {
                    LogApp(f_ModuleName, DbLogType.Audit, "审核成功！");
                    return Success("审核成功！");
                }
                else
                {
                    LogApp(f_ModuleName, DbLogType.Audit, "审核失败！");
                    return Error("审核失败");
                }
            }
            catch (Exception ex)
            {
                LogApp(f_ModuleName, DbLogType.Exception, ex.Message);
                return Error("审核失败");
            }
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAuthorize]
        public void Export()
        {
            try
            {
                string sql = GetGridOrderSql(null);
                var dataTable = Db.MySqlContext.FromSql(sql).ToDataTable();
                NPOIExcel nPOIExcel = new NPOIExcel();

                string baseDictioneryPath = AppDomain.CurrentDomain.BaseDirectory + "\\Excel";
                if (!Directory.Exists(baseDictioneryPath))
                    Directory.CreateDirectory(baseDictioneryPath);

                string fileName = "订单列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";

                string excelFilePath = baseDictioneryPath + "\\" + fileName;

                bool r = nPOIExcel.ToExcel(dataTable, "订单列表", "订单列表", excelFilePath);

                FileDownHelper.DownLoadold(excelFilePath, fileName);
            }
            catch (Exception ex)
            {
                LogApp(f_ModuleName, DbLogType.Exception, ex.Message);
            }
        }


        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        public ActionResult SendMsg(string ids,string MsgTemplate)
        {
            string WeixinAppId = ConfigurationManager.AppSettings["WeixinAppId"] ?? "";
            string WxDomain = ConfigurationManager.AppSettings["wxDomain"] ?? "";
            try
            {
                string sql = string.Format(@"select t1.*,t2.OpenId,t2.WxNickName from Orders as t1
                            INNER join Wx_Users as t2 on t1.UserId =  t2.Id
                            where t1.Id in ({0})", ids);
                var orders = Db.MySqlContext.FromSql(sql).ToList<OrderExt1>();
                foreach (var item in orders)
                {
                    string ComplaintsHotline = CommonBLL.GetGlobalConstVariable(YiDaBusConst.投诉热线).FirstOrDefault()?.F_Description;
                    //发送消息通知生成状态
                    var TempleteData = new
                    {
                        first = new TemplateDataItem($"尊敬的{item.WxNickName}您好，您已预约租车成功。"),
                        productType = new TemplateDataItem("服务"),
                        name = new TemplateDataItem($"前往：{item.ToPosition}，时间：{item.DepartureTime}，座位号：{item.SeatTexts}，已预约成功"),
                        time = new TemplateDataItem(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),//时间
                        result = new TemplateDataItem("已预约"),//结果
                        remark = new TemplateDataItem($"{MsgTemplate}如有疑问，请咨询{ComplaintsHotline}。")//结果
                    };
                    var tmResult = TemplateApi.SendTemplateMessage(WeixinAppId, item.OpenId, "ugJ8nxawp2ZE53lrDMCpCVB0lI1iSKn2PSFK-rLrqP4",
                                (WxDomain + "/MemberManager/Member/MemberTicketDetail?orderId=" + item.Id)
                                , TempleteData);
                }
                return Success("发送成功");
            }
            catch (Exception ex)
            {
                LogApp(f_ModuleName, DbLogType.Exception, ex.Message);
                return Error("发送消息失败");
            }
        }
        #endregion

        public class OrderExt1 : Orders
        {
            public string OpenId { get; set; }
            public string WxNickName { get; set; }
            
        }
    }
}