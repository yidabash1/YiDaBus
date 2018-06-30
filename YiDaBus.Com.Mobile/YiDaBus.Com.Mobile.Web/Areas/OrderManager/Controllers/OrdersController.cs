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


            //生成订单号
            orders.OrderNo = OrderHeader.U.ToString() + DateTime.Now.ToString("yyyyMMddHHmmss") + RandomHelper.GenetatorNumbers();
            string Area = Request["Area"] ?? "";
            GetOrderBaseInfoByArea(Area);
            var IsOneWay = (Request["IsOneWay"] ?? "0").ToInt();//是否单程
            var IsShuttle = (Request["IsShuttle"] ?? "0").ToInt();//是否接送
            var seatCount = ChooseSeatsArr.Length;
            var seatPrice = TicketPrice;
            var shuttlePrice = IsShuttle == 1 ? DeliveryFee : 0;
            var way = IsOneWay == 1 ? 1 : 2;
            var totalMoney = (seatCount * (seatPrice + shuttlePrice) * way).ToDecimal(2);
            orders.TotalAmount = totalMoney;
            orders.CreateTime = DateTime.Now;
            orders.UpdateTime = DateTime.Now;
            orders.IsDel = 0;
            orders.PayState = 0;

            string Seats = string.Empty;
            foreach (var item in ChooseSeatsArr)
            {
                Seats += YiDaBusConst.SeatSign + item + YiDaBusConst.SeatSign + ",";
            }
            orders.Seats = Seats.TrimEnd(',');

            Db.MySqlContext.Insert(orders);
            return Sucess("下单成功");
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
    }
}