	
using System;
using Dos.ORM;

namespace YiDaBus.Com.UtilsModel
{


    [Serializable]
	public  class userinfotracklog
    {

	    #region Field
	    /// <summary>
        /// 主键自增ID号
        /// </summary>
		 public int Id { get; set; }
	    /// <summary>
        /// 用户编号
        /// </summary>
		 public int? UserId { get; set; }
	    /// <summary>
        /// 用户登录账号（登录手机号）
        /// </summary>
		 public string UserLogAccount { get; set; }
	    /// <summary>
        /// 日志类型
        /// </summary>
		 public int? LogType { get; set; }
	    /// <summary>
        /// 日志备注
        /// </summary>
		 public string LogRemark { get; set; }
	    /// <summary>
        /// 操作人ID号
        /// </summary>
		 public string OperatorId { get; set; }
	    /// <summary>
        /// 操作人姓名
        /// </summary>
		 public string OperatorName { get; set; }
	    /// <summary>
        /// 新增时间
        /// </summary>
		 public DateTime? CreateTime { get; set; }
		#endregion
    }

}
