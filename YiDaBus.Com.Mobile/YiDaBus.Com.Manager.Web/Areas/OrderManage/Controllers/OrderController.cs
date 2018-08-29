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

        private string GetGridOrderSql(Pagination pagination, bool isChangeSelect = false)
        {
            string sqlWhere = " WHERE 1=1 And t1.Isdel=0";
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

            string sql = string.Empty;
            if (!isChangeSelect)
            {
                sql = string.Format(@"SELECT t1.*,t2.Gender FROM Orders AS t1
							LEFT JOIN Wx_Users AS t2 ON t1.UserId=t2.Id {0} {1} ", sqlWhere, sqlOrder);
            }
            else
            {
                if (string.IsNullOrEmpty(ToPosition))
                {
                    sql = string.Format(@"SELECT t1.*,t2.Gender FROM Orders AS t1
							LEFT JOIN Wx_Users AS t2 ON t1.UserId=t2.Id {0} {1} ", sqlWhere, sqlOrder);
                }
                else if (ToPosition == "上海")
                {
                    sql = string.Format(@"SELECT SeatTexts as '座位号',t1.UserNickName as '真实姓名',t1.Mobile as '电话号码',t1.MeetPosition AS '接客地点',t1.SendPosition AS '送客地点',case IsOneWay when 0 then '否' when 1 then '是' else '' end as '单程',TotalAmount as '车费' FROM Orders AS t1
							{0} {1} ", sqlWhere, sqlOrder);
                }
                else if (ToPosition == "杭州")
                {
                    sql = string.Format(@"SELECT SeatTexts as '座位号',t1.UserNickName as '真实姓名',t1.Mobile as '电话号码',case IsOneWay when 0 then '否' when 1 then '是' else '' end as '单程',TotalAmount as '车费' FROM Orders AS t1
							{0} {1} ", sqlWhere, sqlOrder);
                }
            }

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
        public ActionResult SubmitForm(string keyValue, Orders orders)
        {
            OperatorModel op = OperatorProvider.Provider.GetCurrent();
            if (string.IsNullOrEmpty(keyValue))
            {
                orders.CreateTime = DateTime.Now;
                orders.UpdateTime = DateTime.Now;
            }
            else
            {
                orders.Id = keyValue.ToInt();
                orders.UpdateTime = DateTime.Now;

                var ticketPriceData = CommonBLL.GetGlobalConstVariable();

                //修改座位
                decimal SingleTicketPrice = 0;//单程
                decimal MutilTicketPrice = 0;//双程
                decimal ShuttlePrice = 0;//接送费
                decimal SeatPrice = 0;//票价
                var IsShuttlePrice = orders.IsShuttle == 1;
                var IsOneWay = orders.IsOneWay == 1;
                if (orders.Area == AreaType.shanghai.ToString())
                {
                    //接送费
                    if (IsShuttlePrice)
                    {
                        ShuttlePrice = ticketPriceData.Where(d => d.F_ItemCode == YiDaBusConst.上海接送费).FirstOrDefault().F_Description.ToDecimal();
                    }
                    //单程
                    if (IsOneWay)
                    {
                        SingleTicketPrice = ticketPriceData.Where(d => d.F_ItemCode == YiDaBusConst.上海单程票价不含接送).FirstOrDefault().F_Description.ToDecimal();
                    }
                    //双程
                    if (!IsOneWay)
                    {
                        MutilTicketPrice = ticketPriceData.Where(d => d.F_ItemCode == YiDaBusConst.上海双程票价不含接送).FirstOrDefault().F_Description.ToDecimal();
                    }
                }
                else
                {
                    //单程
                    if (IsOneWay)
                    {
                        SingleTicketPrice = ticketPriceData.Where(d => d.F_ItemCode == YiDaBusConst.杭州单程票价).FirstOrDefault().F_Description.ToDecimal();
                    }
                    //双程
                    if (!IsOneWay)
                    {
                        MutilTicketPrice = ticketPriceData.Where(d => d.F_ItemCode == YiDaBusConst.杭州双程票价).FirstOrDefault().F_Description.ToDecimal();
                    }
                }


                var ChooseSeatsArr = orders.SeatTexts.Replace("座", "").Split('、');
                if (ChooseSeatsArr.Length == 0) { return Error("请先选择座位！"); }
                string SeatIds = string.Empty;
                string SeatTexts = string.Empty;
                string areaCn = string.Empty;
                var SeatCount = ChooseSeatsArr.Length;
                var selectWeek = (int)Convert.ToDateTime(orders.DepartureTime).DayOfWeek;
                if (selectWeek == 0) { selectWeek = 7; }
                orders.Week = selectWeek;
                foreach (var item in ChooseSeatsArr)
                {
                    SeatIds += YiDaBusConst.SeatSign + item + YiDaBusConst.SeatSign;
                    //判断座位是否已经被其他人下单
                    var isExist = Db.MySqlContext.Exists<Orders>(d => d.DepartureTime == orders.DepartureTime && d.IsDel == (int)IsDel.否 && d.SeatIds.Contains(SeatIds) && d.Area == orders.Area && d.UserId != orders.UserId);

                    if (orders.Area == AreaType.shanghai.ToString()) { areaCn = "上海"; }
                    else if (orders.Area == AreaType.hangzhou.ToString()) { areaCn = "杭州"; }
                    if (isExist)
                    {
                        return Error($"车号：{orders.CarNumber}</br>前往：{areaCn}</br>时间：{orders.DepartureTime}</br>座位号：{item}</br>已被其他人购买，请选择其他座位。");
                    }
                    SeatIds += ",";
                    SeatTexts += item + "座、";
                }
                orders.SeatIds = SeatIds.TrimEnd(',');
                orders.SeatTexts = SeatTexts.TrimEnd('、');
                orders.WeekTextCn = Enum.GetName(typeof(WeekCn), orders.Week.ToInt());


                SeatPrice = IsOneWay ? SingleTicketPrice : MutilTicketPrice;
                var totalMoney = (SeatCount * (SeatPrice + ShuttlePrice)).ToDecimal(2);
                orders.TotalAmount = totalMoney;
            }
            return SubmitForms(Db.MySqlContext, orders, keyValue);
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            return MultiDeleteFormBySql(Db.MySqlContext, keyValue, "Orders", true);
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
                //总人数，单程人数，双程人数
                var ToPosition = HttpUtility.UrlDecode(Request["ToPosition"]);
                string sql = GetGridOrderSql(null, true);
                var dataTable = Db.MySqlContext.FromSql(sql).ToDataTable();
                NPOIExcel nPOIExcel = new NPOIExcel();

                string baseDictioneryPath = AppDomain.CurrentDomain.BaseDirectory + "\\Excel";
                if (!Directory.Exists(baseDictioneryPath))
                    Directory.CreateDirectory(baseDictioneryPath);

                string fileName = "订单列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";

                string excelFilePath = baseDictioneryPath + "\\" + fileName;
                string CarNumber = HttpUtility.UrlDecode(Request["CarNumber"]);
                string DepartureTimeStart = Request["DepartureTimeStart"];
                string DepartureTimeEnd = Request["DepartureTimeEnd"];
                bool r = nPOIExcel.ToExcel(dataTable, "订单列表", "订单列表", excelFilePath, (workBook, sheet) =>
                {
                    //自定义行
                    object TotalAmountSum = dataTable.Compute("sum(车费)", "TRUE");
                    IRow row = sheet.CreateRow(0);
                    var _DepartureTimeStart = DepartureTimeStart.ToDate().ToString("yyyy年MM月dd日");
                    var _DepartureTimeEnd = DepartureTimeEnd.ToDate().ToString("yyyy年MM月dd日");
                    var DepartureTimeStr = string.Empty;
                    if (_DepartureTimeStart == _DepartureTimeEnd)
                    {
                        DepartureTimeStr = _DepartureTimeStart;
                    }
                    else
                    {
                        DepartureTimeStr = $"{ _DepartureTimeStart}-{ _DepartureTimeEnd}";
                    }

                    int totalCount = dataTable.Rows.Count;//总人数
                    int singleCount = dataTable.Select("单程='是'").Count();//单程
                    int doubleCount = totalCount - singleCount;//双程

                    row.CreateCell(0).SetCellValue($"目的地：{ToPosition}     车号：{CarNumber}      发车时间：{DepartureTimeStr}   合计:{TotalAmountSum}   总人数:{totalCount}    单程:{singleCount}    双程:{doubleCount}  ");
                    sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, dataTable.Columns.Count - 1));
                    row.Height = 500;
                    ICellStyle cellStyle = workBook.CreateCellStyle();
                    IFont font = workBook.CreateFont();
                    font.FontName = "微软雅黑";
                    font.FontHeightInPoints = 17;
                    cellStyle.SetFont(font);
                    cellStyle.VerticalAlignment = VerticalAlignment.Center;
                    cellStyle.Alignment = HorizontalAlignment.Left;
                    row.Cells[0].CellStyle = cellStyle;

                    return 1;
                });

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
        public ActionResult SendMsg(string ids, string MsgTemplate)
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