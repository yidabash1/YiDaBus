	
using System;
using Dos.ORM;

namespace YiDaBus.Com.UtilsModel
{


    [Serializable]
	public  class paylog
    {

	    #region Field
	    /// <summary>
        /// 主键自增ID号
        /// </summary>
		 public int Id { get; set; }
	    /// <summary>
        /// 交易号
        /// </summary>
		 public string TradeNo { get; set; }
	    /// <summary>
        /// 交易类型：押金
        /// </summary>
		 public string TradeType { get; set; }
	    /// <summary>
        /// 支付平台
        /// </summary>
		 public string PayPlatform { get; set; }
	    /// <summary>
        /// 用户ID号
        /// </summary>
		 public int? UserId { get; set; }
	    /// <summary>
        /// 用户姓名
        /// </summary>
		 public string UserName { get; set; }
	    /// <summary>
        /// 应付金额
        /// </summary>
		 public decimal PayableAmount { get; set; }
	    /// <summary>
        /// 支付状态：0表示未支付；1表示支付成功；2表示支付失败；
        /// </summary>
		 public int PayStatus { get; set; }
	    /// <summary>
        /// 支付金额（退款金额）
        /// </summary>
		 public decimal PayAmount { get; set; }
	    /// <summary>
        /// 用户优惠券ID
        /// </summary>
		 public int UserCouponId { get; set; }
	    /// <summary>
        /// 优惠券金额
        /// </summary>
		 public decimal CouponAmount { get; set; }
	    /// <summary>
        /// 备注
        /// </summary>
		 public string Remark { get; set; }
	    /// <summary>
        /// 添加时间
        /// </summary>
		 public DateTime? AddTime { get; set; }
	    /// <summary>
        /// 更新时间
        /// </summary>
		 public DateTime? LastUpdateTime { get; set; }
	    /// <summary>
        /// 软删除：0表示未删除；1表示已删除
        /// </summary>
		 public int? IsDel { get; set; }
		#endregion
    }

}
