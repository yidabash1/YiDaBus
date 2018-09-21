using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YiDaBus.Com.Dal.Base;
using YiDaBus.Com.Mobile.BLL;
using YiDaBus.Com.Mobile.Model.Const;
using YiDaBus.Com.Mobile.Web.Base;
using YiDaBus.Com.Model;
using YiDaBus.Com.Manager.Common;
using YiDaBus.Com.Mobile.Model.Enum;
using YiDaBus.Com.Mobile.Common;
using YiDaBus.Com.Mobile.Web.App_Start;
using Dos.ORM;
using YiDaBus.Com.Mobile.Model.ResponseModel;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using Senparc.Weixin.MP.AdvancedAPIs;
using System.Configuration;

namespace YiDaBus.Com.Mobile.Web.Areas.OrderManager.Controllers
{
    [HandlerLogin]
    public class OrdersController : BaseController
    {
        /// <summary>
        /// 票价（不含接送费）
        /// </summary>
        public decimal TicketPrice { get; set; } = 0;
        /// <summary>
        /// 接送费
        /// </summary>
        public decimal DeliveryFee { get; set; } = 0;
        #region 区域
        /// <summary>
        /// 订票
        /// </summary>
        /// <returns></returns>
        public ActionResult ShangHaiIndex(string week)
        {
            //上海订座，需要在后台增加每天停止订座的时间，如：19:00。这样，到周一19:00，只能订周二至五的票，以此类推……，周五19:00后（包含周六周日）只能订下周1-5的票。
            ViewBag.IsExipre = false;//是否已经过期
            int day = 0;
            var curDateTime = DateTime.Now;
            int curWeek = (int)curDateTime.DayOfWeek;
            int selectWeek = week.ToInt();
            var endConfigList = Db.MySqlContext.From<TimeEndConfig>().Where(d => d.Area == "上海").ToList<TimeEndConfigExt>();
            foreach (var item in endConfigList)
            {
                switch (item.Week)
                {
                    case "星期一":
                        item.IntWeek = 1;
                        break;
                    case "星期二":
                        item.IntWeek = 2;
                        break;
                    case "星期三":
                        item.IntWeek = 3;
                        break;
                    case "星期四":
                        item.IntWeek = 4;
                        break;
                    case "星期五":
                        item.IntWeek = 5;
                        break;
                    default:
                        break;
                }
            }
            var curEndConfig = endConfigList.Where(d => d.IntWeek == curWeek).FirstOrDefault();
            var selectEndConfig = endConfigList.Where(d => d.IntWeek == selectWeek).FirstOrDefault();
            if (selectEndConfig.IsClosed == 1)
            {
                ViewBag.IsClose = true;
                return View();
            }
            //如果当前不是星期六并且不是星期天，则判断是否过期
            if (curWeek != 0 && curWeek != 6)
            {
                string endTime = string.Empty;
                ViewBag.ExipreEndTime = endConfigList.Where(d=>d.Week == "星期五").FirstOrDefault().EndTime;
                

              
                if (curEndConfig != null) { endTime = curEndConfig.EndTime; }
                DateTime endDateTime = string.Format("{0} {1}", curDateTime.ToString("yyyy-MM-dd"), endTime).ToDate();
                var DepartureTime = GetDateTimeByWeek(selectWeek, ref day);
                if (curWeek == 5)
                {
                    //当前时间比结束时间大，直接可以任意选择下周的车票
                    if (curDateTime > endDateTime)
                    {
                        ViewBag.IsExipre = false;
                    }
                    else
                    {
                        //当前时间比结束时间小，只能选择当天的车票了
                        if (day < 0)
                        {
                            ViewBag.IsExipre = true;
                            return View();
                        }
                    }
                }
                else
                {
                    //当前时间比结束时间大，只能选择当天之后的票
                    if (curDateTime > endDateTime)
                    {
                        if (day <= 0)
                        {
                            ViewBag.IsExipre = true;
                            return View();
                        }
                    }
                    else {
                        //当前时间比结束时间小，只能选择当天以及当天以后的票
                        if (day < 0)
                        {
                            ViewBag.IsExipre = true;
                            return View();
                        }
                    }
                }
            }

            ViewBag.FriendlyReminder = CommonBLL.GetGlobalConstVariable(YiDaBusConst.友情提醒).FirstOrDefault()?.F_Description;
            return View();
        }
        /// <summary>
        /// 订票
        /// </summary>
        /// <returns></returns>
        public ActionResult HangZhouIndex()
        {
            ViewBag.FriendlyReminder = CommonBLL.GetGlobalConstVariable(YiDaBusConst.友情提醒).FirstOrDefault()?.F_Description;
            return View();
        }
        [ValidateInput(false)]
        public ActionResult OrderInfo()
        {
            string Area = Request["area"] ?? "";
            string week = Request["week"] ?? "";
            GetOrderBaseInfoByArea(Area, week);//根据区域获取订单的一些基础信息

            return View();
        }


        public ActionResult Success()
        {
            return View();
        }
        #endregion

        #region 操作
        [HttpPost]
        public ActionResult CreateOrdersInfo(Orders orders)
        {
            string WeixinAppId = ConfigurationManager.AppSettings["WeixinAppId"] ?? "";
            string WxDomain = ConfigurationManager.AppSettings["wxDomain"] ?? "";
            string ManagerOpenId = ConfigurationManager.AppSettings["ManagerOpenId"] ?? "";
            


            string ChooseSeats = Request["ChooseSeats"] ?? "";
            var ChooseSeatsArr = ChooseSeats.Split(',');
            if (ChooseSeatsArr.Length == 0) { return Error("请先选择座位！"); }

            var userInfo = GetUserInfo();//获取用户信息
            if (userInfo == null) { return Error(ErrCode.用户信息失效请重新登录); }
            if (orders.Area == AreaType.shanghai.ToString() && orders.Week == null) { return Error("请选择班次"); }
            var areaCn = string.Empty;

            int totalCount = ChooseSeatsArr.Length;
            //开始事务
            DbTrans trans = Db.MySqlContext.BeginTransaction();
            try
            {
                //一个用户某一周最多只能预定两张票
                string startTime = string.Empty;
                string endTime = string.Empty;
                GetStartEndTimeByWeek(ref startTime, ref endTime);
                //获取当周的星期一
                //string sql = string.Format(@"SELECT
	               //                 SeatIds
                //                FROM
	               //                 Orders 
                //                WHERE
	               //                 UserId = {0} 
	               //                 AND IsDel = {1} 
	               //                 AND DepartureTime >= '{2}' 
	               //                 AND DepartureTime <= '{3}' ", userInfo.Id, (int)IsDel.否, startTime, endTime);
                //var SeatIdsList = Db.MySqlContext.FromSql(sql).ToList<string>();

                //foreach (var item in SeatIdsList)
                //{
                //    if (string.IsNullOrEmpty(item)) continue;
                //    if (item.Contains(","))
                //        totalCount += 2;
                //    else
                //        totalCount++;
                //}
                //if (totalCount > 2) { return Error("一周最多只能预定两张票！"); }

                //获取发车时间
                DateTime DepartureTime = DateTime.Now;
                string orderHeader = string.Empty;
                string MoneyConst = string.Empty;
                //修改座位
                decimal SingleTicketPrice = 0;//单程
                decimal MutilTicketPrice = 0;//双程
                decimal ShuttlePrice = 0;//接送费
                decimal SeatPrice = 0;//票价
                var IsShuttlePrice = orders.IsShuttle == 1;
                var IsOneWay = orders.IsOneWay == 1;
                var ticketPriceData = CommonBLL.GetGlobalConstVariable();
                if (orders.Area == AreaType.shanghai.ToString())
                {
                    int day = 0;
                    DepartureTime = GetDateTimeByWeek(orders.Week.ToInt(), ref day);
                    if (day < 0)
                    {
                        return Error("请选择当前以及当前日期之后的车次！");
                    }

                    orderHeader = "SH";
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
                    orders.Week = GetWeekByDateTime(DepartureTime);
                    orderHeader = "HZ";
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
                

                orders.DepartureTime = DepartureTime.ToString("yyyy-MM-dd");
                string SeatIds = string.Empty;
                string SeatTexts = string.Empty;
                foreach (var item in ChooseSeatsArr)
                {
                    SeatIds += YiDaBusConst.SeatSign + item + YiDaBusConst.SeatSign;
                    //判断座位是否已经被其他人下单
                    var isExist = Db.MySqlContext.Exists<Orders>(d => d.DepartureTime == orders.DepartureTime && d.IsDel == (int)IsDel.否 && d.SeatIds.Contains(SeatIds) && d.Area == orders.Area);

                    if (orders.Area == AreaType.shanghai.ToString()) { areaCn = "上海"; }
                    else if (orders.Area == AreaType.hangzhou.ToString()) { areaCn = "杭州"; }
                    if (isExist) { return Error($"车号：{orders.CarNumber}</br>前往：{areaCn}</br>时间：{orders.DepartureTime}</br>座位号：{item}</br>已被其他人购买，请选择其他座位。"); }
                    SeatIds += ",";
                    SeatTexts += item + "座、";
                }
                orders.SeatIds = SeatIds.TrimEnd(',');
                orders.SeatTexts = SeatTexts.TrimEnd('、');
                orders.UserId = GetUserId();
                orders.UserName = userInfo.UserName;
                //生成订单号
                orders.OrderNo = orderHeader + DateTime.Now.ToString("yyyyMMddHHmmss") + RandomHelper.GenetatorNumbers();
                //string Area = Request["Area"] ?? "";
                //GetOrderBaseInfoByArea(Area);
                //var IsOneWay = (Request["IsOneWay"] ?? "0").ToInt();//是否单程
                //var IsShuttle = (Request["IsShuttle"] ?? "0").ToInt();//是否接送
                var SeatCount = ChooseSeatsArr.Length;
                SeatPrice = IsOneWay ? SingleTicketPrice : MutilTicketPrice;
                var totalMoney = (SeatCount * (SeatPrice + ShuttlePrice)).ToDecimal(2);
                orders.TotalAmount = totalMoney;
                orders.CreateTime = DateTime.Now;
                orders.UpdateTime = DateTime.Now;
                orders.IsDel = 0;
                orders.PayState = 0;
                orders.WeekTextCn = Enum.GetName(typeof(WeekCn), orders.Week.ToInt());
                orders.WxNickName = userInfo.WxNickName;
                int r = Db.MySqlContext.Insert(trans, orders);

                trans.Commit();
                if (r > 0)
                {
                    if (!userInfo.OpenId.IsEmpty())
                    {
                        string ComplaintsHotline = CommonBLL.GetGlobalConstVariable(YiDaBusConst.投诉热线).FirstOrDefault()?.F_Description;
                        //发送消息通知生成状态
                        var TempleteData = new
                        {
                            first = new TemplateDataItem($"尊敬的{userInfo.WxNickName}您好，您已预约租车成功。"),
                            productType = new TemplateDataItem("服务"),
                            name = new TemplateDataItem($"前往：{areaCn}，时间：{orders.DepartureTime}，座位号：{ChooseSeats}，已预约成功"),
                            time = new TemplateDataItem(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),//时间
                            result = new TemplateDataItem("已预约"),//结果
                            remark = new TemplateDataItem($"如有疑问，请咨询{ComplaintsHotline}。")//结果
                        };
                        //给客户
                        var tmResult = TemplateApi.SendTemplateMessage(WeixinAppId, userInfo.OpenId, "ugJ8nxawp2ZE53lrDMCpCVB0lI1iSKn2PSFK-rLrqP4",
                                    (WxDomain + "/MemberManager/Member/MemberTicketDetail?orderId=" + r)
                                    , TempleteData);
                        //给后台管理员
                        var tmResult1 = TemplateApi.SendTemplateMessage(WeixinAppId, ManagerOpenId, "ugJ8nxawp2ZE53lrDMCpCVB0lI1iSKn2PSFK-rLrqP4",
                                    (WxDomain + "/MemberManager/Member/MemberTicketDetail?orderId=" + r)
                                    , TempleteData);
                    }
                    return Sucess("下单成功");
                }
                else
                {
                    return Error("下单失败");
                }
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return Error(ex.Message);
            }
            finally
            {
                trans.Close();
            }
        }
        [HttpPost]
        public ActionResult GetTicketPriceDetails(Orders orders)
        {
            var Details = CommonBLL.GetGlobalConstVariable();
            return base.Sucess("操作成功", 200, Details);
        }
        /// <summary>
        /// 根据区域获取已选中的座位号
        /// </summary>
        /// <param name="orders"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetSelectedSeatsByAare(string area, string week)
        {
            WhereClipBuilder wherebuilder = new WhereClipBuilder();
            wherebuilder.And(Orders._.IsDel == IsDel.否);
            var DepartureTime = string.Empty;
            string IsOpenF56789 = "是";
            if (area == AreaType.shanghai.ToString())
            {
                wherebuilder.And(Orders._.CarNumber.In(YiDaBusConst.上海车牌号1, YiDaBusConst.上海车牌号2));
                int day = 0;
                DepartureTime = GetDateTimeByWeek(week.ToInt(), ref day).ToString("yyyy-MM-dd");
                IsOpenF56789 = CommonBLL.GetGlobalConstVariable(YiDaBusConst.是否开启苏F56789).FirstOrDefault()?.F_Description;
            }
            else if (area == AreaType.hangzhou.ToString())
            {
                wherebuilder.And(Orders._.CarNumber == YiDaBusConst.杭州车牌号);
                DepartureTime = DateTime.Now.ToString("yyyy-MM-dd");
            }
            wherebuilder.And(Orders._.DepartureTime == DepartureTime);

            var data = Db.MySqlContext.From<Orders>().Where(wherebuilder.ToWhereClip()).ToList();
            var groupData = data.GroupBy(d => d.CarNumber);

            SeatsDetails seatsDetails = new SeatsDetails();
            List<OrdersByGroupCarNumber> ordersByGroupCarNumberList = new List<OrdersByGroupCarNumber>();
            foreach (var groupitem in groupData)
            {
                List<string> seatsList = new List<string>();
                List<string> unSelectList = new List<string>();

                foreach (var item in groupitem)
                {
                    seatsList = seatsList.Concat(item.SeatIds.Replace(YiDaBusConst.SeatSign, "").Split(',').ToList()).ToList();
                }

                ordersByGroupCarNumberList.Add(new OrdersByGroupCarNumber()
                {
                    CarNumber = groupitem.Key,
                    SeatIds = seatsList
                });
            }
            seatsDetails.OrdersByGroupCarNumberList = ordersByGroupCarNumberList;
            seatsDetails.IsOpenF56789 = IsOpenF56789;
            return base.Sucess("操作成功", 200, seatsDetails);
        }

        [HttpPost]
        public ActionResult GetMyOrderList(int pageSize, int pageIndex)
        {
            var userid = GetUserId();
            var linq = Db.MySqlContext.From<Orders>().Where(d => d.UserId == userid && d.IsDel == (int)IsDel.否).OrderByDescending(d => d.Id);

            PageResponse<OrdersExt> pageList = getListByPaging<Orders, OrdersExt>(linq, pageSize, pageIndex);
            if (pageList == null || pageList.totalItems == 0)
                return Error(ErrCode.查询成功无数据);
            else
            {
                foreach (var item in pageList.items)
                {
                    //var seatsArr = item.SeatIds.Replace(YiDaBusConst.SeatSign, "").Split(',').ToList();
                    //for (int i = 0; i < seatsArr.Count; i++)
                    //{
                    //    seatsArr[i] = seatsArr[i] + "座";
                    //}

                    //item.SeatIds = string.Join(",", seatsArr);
                    if (item.DepartureTime.ToDate() < DateTime.Now.Date)
                    {
                        item.IsExpir = 1;
                    }
                    else
                    {
                        item.IsExpir = 0;
                    }

                }
                return Sucess("获取成功", 200, pageList);
            }

        }

        [HttpPost]
        public ActionResult GetOrderDetailsById()
        {
            var orderId = (Request["orderId"] ?? "").ToInt();
            if (orderId <= 0) { return Error("订单不存在！"); }
            var orderDetails = Db.MySqlContext.From<Orders>().Where(d => d.Id == orderId).First();
            //if (orderDetails == null) { return Error("订单不存在！"); }

            return base.Sucess("获取成功", 200, orderDetails);
        }

        [HttpPost]
        public ActionResult DeleteOrderByOrderId()
        {
            var orderId = (Request["orderId"] ?? "").ToInt();
            if (orderId <= 0) { return Error("订单不存在！"); }
            var orderDetails = Db.MySqlContext.From<Orders>().Where(d => d.Id == orderId).First();
            if (orderDetails == null) { return Error("订单不存在！"); }
            orderDetails.IsDel = (int)IsDel.是;
            int r = Db.MySqlContext.Update(orderDetails);
            if (r > 0)
            {
                return Sucess();
            }
            else
            {
                return Error();
            }
        }

        #endregion

        #region 私有方法
        /// <summary>
        /// 根据区域获取订单的一些基础信息
        /// </summary>
        private void GetOrderBaseInfoByArea(string area, string week)
        {
            List<Sys_ItemsDetail> Sys_ItemsDetailList = CommonBLL.GetGlobalConstVariable();
            DateTime DepartureTime = DateTime.Now;

            string F_ItemCode = string.Empty;
            if (area == YiDaBusConst.上海)
            {
                //F_ItemCode = YiDaBusConst.上海单程票价不含接送;
                //DeliveryFee = Sys_ItemsDetailList.Where(d => d.F_ItemCode == YiDaBusConst.上海接送费).FirstOrDefault().F_Description.ToDecimal();
                int day = 0;
                DepartureTime = GetDateTimeByWeek(week.ToInt(), ref day);
            }
            else if (area == YiDaBusConst.杭州)
            {
                //F_ItemCode = YiDaBusConst.杭州单程票价;
            }
            //TicketPrice = Sys_ItemsDetailList.Where(d => d.F_ItemCode == F_ItemCode).FirstOrDefault().F_Description.ToDecimal();
            //ViewBag.TicketPrice = TicketPrice;
            //ViewBag.DeliveryFee = DeliveryFee;
            ViewBag.DepartureTime = DepartureTime;
        }
        #endregion

        #region 私有类
        protected class OrdersByGroupCarNumber
        {
            public string CarNumber { get; set; }
            /// <summary>
            /// 已选中的座位号
            /// </summary>
            public List<string> SeatIds { get; set; }
        }
        protected class SeatsDetails
        {
            public List<OrdersByGroupCarNumber> OrdersByGroupCarNumberList { get; set; }
            /// <summary>
            /// 不可以被选择的座位号
            /// </summary>
            public string IsOpenF56789 { get; set; }
        }

        protected class OrdersExt : Orders
        {
            /// <summary>
            /// 是否过期
            /// </summary>
            public int IsExpir { get; set; }
            //public string WeekText { get; set; }
        }
        protected class TimeEndConfigExt : TimeEndConfig
        {
            /// <summary>
            /// 星期几
            /// </summary>
            public int IntWeek { get; set; }
        }

        #endregion
    }
}