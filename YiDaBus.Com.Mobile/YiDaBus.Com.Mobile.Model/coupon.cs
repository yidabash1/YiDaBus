	
using System;
using Dos.ORM;

namespace YiDaBus.Com.UtilsModel
{


    [Serializable]
	public  class coupon
    {

	    #region Field
	    /// <summary>
        /// 主键自增ID号
        /// </summary>
		 public int Id { get; set; }
	    /// <summary>
        /// 优惠券名称
        /// </summary>
		 public string CouponName { get; set; }
	    /// <summary>
        /// 优惠券金额
        /// </summary>
		 public decimal CouponAmount { get; set; }
	    /// <summary>
        /// 优惠券库存数
        /// </summary>
		 public int CouponCount { get; set; }
	    /// <summary>
        /// 优惠券开始时间
        /// </summary>
		 public DateTime? StartTime { get; set; }
	    /// <summary>
        /// 优惠券结束时间
        /// </summary>
		 public DateTime? EndTime { get; set; }
	    /// <summary>
        /// 软删除：0表示未删除；1表示已删除
        /// </summary>
		 public int? IsDel { get; set; }
	    /// <summary>
        /// 创建时间
        /// </summary>
		 public DateTime? AddTime { get; set; }
	    /// <summary>
        /// 更新时间
        /// </summary>
		 public DateTime? LastUpdateTime { get; set; }
	    /// <summary>
        /// 备注
        /// </summary>
		 public string Remark { get; set; }
		#endregion
    }

}
