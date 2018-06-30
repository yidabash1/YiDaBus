using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YiDaBus.Com.Mobile.Web;
using YiDaBus.Com.Mobile.Web.Controllers;
using Newtonsoft.Json;
using YiDaBus.Com.Manager.Common;

namespace YiDaBus.Com.Mobile.Web.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        public int CouponId { get; set; } = 1;
        public string userID { get; set; } = "34";
        [TestMethod]
        public void testGetUserNeedPayAmount()
        {
            string url = "http://localhost:9000/api/Users/GetUserNeedPayAmount";
            StringBuilder param = new StringBuilder();
            param.AppendFormat("userId={0}", DESEncrypt.Encrypt(userID));
            param.AppendFormat("&couponId={0}", CouponId);
            param.AppendFormat("&appID={0}", "");
            string responseData = Utils.HttpPost(url, param.ToString());
            GetUserNeedPayAmountResponse objResponse = JsonConvert.DeserializeObject<GetUserNeedPayAmountResponse>(responseData);
        }
       
        
        public class NeedPayAmount
        {
            /// <summary>
            /// 
            /// </summary>
            public decimal payAmount { get; set; }
        }

        public class GetUserNeedPayAmountResponse
        {
            /// <summary>
            /// 
            /// </summary>
            public int code { get; set; }
            /// <summary>
            /// 获取成功！
            /// </summary>
            public string msg { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public NeedPayAmount data { get; set; }
        }
        [TestMethod]
        public void xiaoqu_anjuke()
        {

            string responseData = Utils.HttpPost(@"http://m.anjuke.com/sh/community/?p=4","");

        }
    }
}
