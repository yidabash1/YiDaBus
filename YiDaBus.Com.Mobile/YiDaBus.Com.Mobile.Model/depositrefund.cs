	
using System;
using Dos.ORM;

namespace YiDaBus.Com.UtilsModel
{


    [Serializable]
	public  class depositrefund
    {

	    #region Field
	    /// <summary>
        /// 主键自增ID号
        /// </summary>
		 public int Id { get; set; }
	    /// <summary>
        /// 交易号
        /// </summary>
		 public string TransactionNo { get; set; }
	    /// <summary>
        /// 支付平台
        /// </summary>
		 public string PayPlatform { get; set; }
	    /// <summary>
        /// 退款人ID号
        /// </summary>
		 public int? RefundUserId { get; set; }
	    /// <summary>
        /// 退款人姓名
        /// </summary>
		 public string RefundName { get; set; }
	    /// <summary>
        /// 退款金额
        /// </summary>
		 public decimal RefundAmount { get; set; }
	    /// <summary>
        /// 退款原因
        /// </summary>
		 public string RefundReason { get; set; }
	    /// <summary>
        /// 退款时间
        /// </summary>
		 public DateTime? RefundTime { get; set; }
	    /// <summary>
        /// 审核人ID号
        /// </summary>
		 public string AuditorId { get; set; }
	    /// <summary>
        /// 审核人姓名
        /// </summary>
		 public string AuditorName { get; set; }
	    /// <summary>
        /// 退款状态：0表示待审核；1表示审核通过；2表示审核未通过；3表示退款成功；4表示退款失败；
        /// </summary>
		 public int? RefundStatus { get; set; }
	    /// <summary>
        /// 审核备注
        /// </summary>
		 public string AuditRemark { get; set; }
	    /// <summary>
        /// 退款创建时间
        /// </summary>
		 public DateTime? AddTime { get; set; }
	    /// <summary>
        /// 退款更新时间
        /// </summary>
		 public DateTime? LastUpdateTime { get; set; }
	    /// <summary>
        /// 审核时间
        /// </summary>
		 public DateTime? AuditTime { get; set; }
	    /// <summary>
        /// 软删除：0表示未删除；1表示已删除
        /// </summary>
		 public int? IsDel { get; set; }
	    /// <summary>
        /// 押金ID
        /// </summary>
		 public int? DepositID { get; set; }
		#endregion
    }

}
