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
        public ActionResult ShangHaiIndex()
        {
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
            GetOrderBaseInfoByArea(Area);//根据区域获取订单的一些基础信息
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
            string ChooseSeats = Request["ChooseSeats"] ?? "";
            var ChooseSeatsArr = ChooseSeats.Split(',');
            if (ChooseSeatsArr.Length == 0) { return Error("请先选择座位！"); }

            var userInfo = GetUserInfo();//获取用户信息
            if (userInfo == null) { return Error(ErrCode.用户信息失效请重新登录); }
            if (orders.Area == AreaType.shanghai.ToString() && orders.Week == null) { return Error("请选择班次"); }

            //开始事务
            DbTrans trans = Db.MySqlContext.BeginTransaction();
            try
            {
                //获取发车时间
                DateTime DepartureTime = DateTime.Now;
                string orderHeader = string.Empty;
                if (orders.Area == AreaType.shanghai.ToString())
                {
                    int day = 0;
                    DepartureTime = GetDateTimeByWeek(orders.Week.ToInt(),ref day);
                    if (day < 0)
                    {
                        return Error("请选择当前以及当前日期之后的车次！");
                    }
                    orderHeader = "SH";
                }
                else
                {
                    orders.Week = GetWeekByDateTime(DepartureTime);
                    orderHeader = "HZ";
                }

                orders.DepartureTime = DepartureTime.ToString("yyyy-MM-dd");
                string SeatIds = string.Empty;
                string SeatTexts = string.Empty;
                foreach (var item in ChooseSeatsArr)
                {
                    SeatIds += YiDaBusConst.SeatSign + item + YiDaBusConst.SeatSign;
                    //判断座位是否已经被其他人下单
                    var isExist = Db.MySqlContext.Exists<Orders>(d => d.DepartureTime == orders.DepartureTime && d.IsDel == (int)IsDel.否 && d.SeatIds.Contains(SeatIds) && d.Area == orders.Area);
                    var areaCn = string.Empty;
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
                string Area = Request["Area"] ?? "";
                GetOrderBaseInfoByArea(Area);
                //var IsOneWay = (Request["IsOneWay"] ?? "0").ToInt();//是否单程
                //var IsShuttle = (Request["IsShuttle"] ?? "0").ToInt();//是否接送
                var seatCount = ChooseSeatsArr.Length;
                var seatPrice = TicketPrice;
                var shuttlePrice = orders.IsShuttle == 1 ? DeliveryFee : 0;
                var way = orders.IsOneWay == 1 ? 1 : 2;
                var totalMoney = (seatCount * (seatPrice + shuttlePrice) * way).ToDecimal(2);
                orders.TotalAmount = totalMoney;
                orders.CreateTime = DateTime.Now;
                orders.UpdateTime = DateTime.Now;
                orders.IsDel = 0;
                orders.PayState = 0;
                orders.WeekTextCn = Enum.GetName(typeof(WeekCn), orders.Week.ToInt());
                int r = Db.MySqlContext.Insert(trans, orders);

                trans.Commit();
                if (r > 0)
                {
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
            if (area == AreaType.shanghai.ToString())
            {
                wherebuilder.And(Orders._.CarNumber.In(YiDaBusConst.上海车牌号1, YiDaBusConst.上海车牌号2));
                int day = 0;
                DepartureTime = GetDateTimeByWeek(week.ToInt(), ref day).ToString("yyyy-MM-dd");
                
            }
            else if (area == AreaType.hangzhou.ToString())
            {
                wherebuilder.And(Orders._.CarNumber == YiDaBusConst.杭州车牌号);
                DepartureTime = DateTime.Now.ToString("yyyy-MM-dd");
            }
            wherebuilder.And(Orders._.DepartureTime == DepartureTime);

            var data = Db.MySqlContext.From<Orders>().Where(wherebuilder.ToWhereClip()).ToList();
            var groupData = data.GroupBy(d => d.CarNumber);

            List<OrdersByGroupCarNumber> ordersByGroupCarNumberList = new List<OrdersByGroupCarNumber>();
            foreach (var groupitem in groupData)
            {
                List<string> seatsList = new List<string>();
                foreach (var item in groupitem)
                {
                    seatsList = seatsList.Concat(item.SeatIds.Replace(YiDaBusConst.SeatSign, "").Split(',').ToList()).ToList();
                }

                ordersByGroupCarNumberList.Add(new OrdersByGroupCarNumber()
                {
                    CarNumber = groupitem.Key,
                    //OrdersList = orderList
                    SeatIds = seatsList
                });
            }
            return base.Sucess("操作成功", 200, ordersByGroupCarNumberList);
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
        private void GetOrderBaseInfoByArea(string area)
        {
            List<Sys_ItemsDetail> Sys_ItemsDetailList = CommonBLL.GetGlobalConstVariable();

            string F_ItemCode = string.Empty;
            if (area == YiDaBusConst.上海)
            {
                F_ItemCode = YiDaBusConst.上海票价不含接送;
                DeliveryFee = Sys_ItemsDetailList.Where(d => d.F_ItemCode == YiDaBusConst.上海接送费).FirstOrDefault().F_Description.ToDecimal();
            }
            else if (area == YiDaBusConst.杭州)
            {
                F_ItemCode = YiDaBusConst.杭州票价;
            }
            TicketPrice = Sys_ItemsDetailList.Where(d => d.F_ItemCode == F_ItemCode).FirstOrDefault().F_Description.ToDecimal();
            ViewBag.TicketPrice = TicketPrice;
            ViewBag.DeliveryFee = DeliveryFee;
        }
        #endregion

        #region 私有类
        protected class OrdersByGroupCarNumber
        {
            public string CarNumber { get; set; }
            //public List<Orders> OrdersList { get; set; }
            public List<string> SeatIds { get; set; }
        }

        protected class OrdersExt : Orders
        {
            /// <summary>
            /// 是否过期
            /// </summary>
            public int IsExpir { get; set; }
            //public string WeekText { get; set; }
        }
        #endregion
    }
}