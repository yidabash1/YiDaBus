	
using System;
using Dos.ORM;

namespace YiDaBus.Com.UtilsModel
{


    [Serializable]
	public  class depositrecharge
    {

	    #region Field
	    /// <summary>
        /// 主键自增ID号
        /// </summary>
		 public int Id { get; set; }
	    /// <summary>
        /// 充值人ID号
        /// </summary>
		 public int? UserId { get; set; }
	    /// <summary>
        /// 支付账号
        /// </summary>
		 public string PayAccount { get; set; }
	    /// <summary>
        /// 支付平台
        /// </summary>
		 public string PayPlatform { get; set; }
	    /// <summary>
        /// 充值人真实姓名
        /// </summary>
		 public string UserRealName { get; set; }
	    /// <summary>
        /// 充值金额
        /// </summary>
		 public decimal RechargeAmount { get; set; }
	    /// <summary>
        /// 交易号
        /// </summary>
		 public string TransactionNo { get; set; }
	    /// <summary>
        /// 充值备注
        /// </summary>
		 public string RechargeRemark { get; set; }
	    /// <summary>
        /// 充值状态：0代表待充值；1表示充值成功；2表示充值失败,3表示已申请退款
        /// </summary>
		 public int? RechargeStatus { get; set; }
	    /// <summary>
        /// 充值时间
        /// </summary>
		 public DateTime? AddTime { get; set; }
	    /// <summary>
        /// 软删除：0表示未删除；1表示已删除
        /// </summary>
		 public int? IsDel { get; set; }
		#endregion
    }

}
