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
            return View();
        }
        /// <summary>
        /// 订票
        /// </summary>
        /// <returns></returns>
        public ActionResult HangZhouIndex()
        {
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
                //获取班次
                DateTime Shift = DateTime.Now;
                if (orders.Area == AreaType.shanghai.ToString())
                {
                    Shift = GetDateTimeByWeek(orders.Week.ToInt());
                }

                orders.Shift = Shift.ToString("yyyy-MM-dd");
                string Seats = string.Empty;
                foreach (var item in ChooseSeatsArr)
                {
                    Seats += YiDaBusConst.SeatSign + item + YiDaBusConst.SeatSign;
                    //判断座位是否已经被其他人下单
                    var isExist = Db.MySqlContext.Exists<Orders>(d => d.Shift == orders.Shift && d.IsDel == (int)IsDel.否 && d.Seats.Contains(Seats) && d.Area == orders.Area);
                    var areaCn = string.Empty;
                    if (orders.Area == AreaType.shanghai.ToString()) { areaCn = "上海"; }
                    else if (orders.Area == AreaType.hangzhou.ToString()) { areaCn = "杭州"; }
                    if (isExist) { return Error($"车号：{orders.CarNumber}</br>前往：{areaCn}</br>时间：{orders.Shift}</br>座位号：{item}</br>已被其他人购买，请选择其他座位。"); }
                    Seats += ",";
                }
                orders.Seats = Seats.TrimEnd(',');

                orders.UserId = GetUserId();
                orders.UserName = userInfo.UserName;
                //生成订单号
                orders.OrderNo = OrderHeader.U.ToString() + DateTime.Now.ToString("yyyyMMddHHmmss") + RandomHelper.GenetatorNumbers();
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
            var Shift = string.Empty;
            if (area == AreaType.shanghai.ToString())
            {
                wherebuilder.And(Orders._.CarNumber.In(YiDaBusConst.上海车牌号1, YiDaBusConst.上海车牌号2));
                Shift = GetDateTimeByWeek(week.ToInt()).ToString("yyyy-MM-dd");
            }
            else if (area == AreaType.hangzhou.ToString())
            {
                wherebuilder.And(Orders._.CarNumber == YiDaBusConst.杭州车牌号);
                DateTime.Now.ToString("yyyy-MM-dd");
            }
            wherebuilder.And(Orders._.Shift == Shift);

            var data = Db.MySqlContext.From<Orders>().Where(wherebuilder.ToWhereClip()).ToList();
            var groupData = data.GroupBy(d => d.CarNumber);

            List<OrdersByGroupCarNumber> ordersByGroupCarNumberList = new List<OrdersByGroupCarNumber>();
            foreach (var groupitem in groupData)
            {
                List<string> seatsList = new List<string>();
                foreach (var item in groupitem)
                {
                    seatsList = seatsList.Concat(item.Seats.Replace(YiDaBusConst.SeatSign, "").Split(',').ToList()).ToList();
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
        #endregion
    }
}