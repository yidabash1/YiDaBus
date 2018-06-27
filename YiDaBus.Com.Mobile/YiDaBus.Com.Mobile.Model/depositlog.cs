	
using System;
using Dos.ORM;

namespace YiDaBus.Com.UtilsModel
{


    [Serializable]
	public  class depositlog
    {

	    #region Field
	    /// <summary>
        /// 主键自增ID号
        /// </summary>
		 public int Id { get; set; }
	    /// <summary>
        /// 外键ID号
        /// </summary>
		 public int? ForeignID { get; set; }
	    /// <summary>
        /// 备注
        /// </summary>
		 public string Remark { get; set; }
	    /// <summary>
        /// 押金类型：0表示充值；1表示退款；2表示扣除；
        /// </summary>
		 public int? Type { get; set; }
	    /// <summary>
        /// 押金金额
        /// </summary>
		 public decimal Amount { get; set; }
	    /// <summary>
        /// 交易平台
        /// </summary>
		 public string Platform { get; set; }
	    /// <summary>
        /// 创建时间
        /// </summary>
		 public DateTime? AddTime { get; set; }
	    /// <summary>
        /// 操作人ID号
        /// </summary>
		 public string OperateUserId { get; set; }
	    /// <summary>
        /// 操作人姓名
        /// </summary>
		 public string OperateName { get; set; }
	    /// <summary>
        /// 软删除：0表示未删除；1表示已删除
        /// </summary>
		 public int? IsDel { get; set; }
		#endregion
    }

}
