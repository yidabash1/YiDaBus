	
using System;
using Dos.ORM;

namespace YiDaBus.Com.UtilsModel
{


    [Serializable]
	public  class usercoupon
    {

	    #region Field
	    /// <summary>
        /// 主键自增ID号
        /// </summary>
		 public int Id { get; set; }
	    /// <summary>
        /// 用户ID号
        /// </summary>
		 public int? UserId { get; set; }
	    /// <summary>
        /// 用户登录账号
        /// </summary>
		 public string UserLogAccount { get; set; }
	    /// <summary>
        /// 优惠券ID号
        /// </summary>
		 public int? CouponId { get; set; }
	    /// <summary>
        /// 优惠券金额
        /// </summary>
		 public decimal CouponAmount { get; set; }
	    /// <summary>
        /// 优惠券名称
        /// </summary>
		 public string CouponName { get; set; }
	    /// <summary>
        /// 优惠券结束时间
        /// </summary>
		 public DateTime? EndTime { get; set; }
	    /// <summary>
        /// 是否已使用：0表示未使用；1表示已使用；
        /// </summary>
		 public int? IsUse { get; set; }
	    /// <summary>
        /// 使用时间
        /// </summary>
		 public DateTime? UseTime { get; set; }
	    /// <summary>
        /// 领取来源
        /// </summary>
		 public string PathWay { get; set; }
	    /// <summary>
        /// 领取时间
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
	    /// <summary>
        /// 备注
        /// </summary>
		 public string Remark { get; set; }
		#endregion
    }

}
