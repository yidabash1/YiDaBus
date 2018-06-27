	
using System;
using Dos.ORM;

namespace YiDaBus.Com.UtilsModel
{


    [Serializable]
	public  class userinfoexpand
    {

	    #region Field
	    /// <summary>
        /// 主键自增ID号
        /// </summary>
		 public int Id { get; set; }
	    /// <summary>
        /// 用户ID号
        /// </summary>
		 public int? UserID { get; set; }
	    /// <summary>
        /// 身份证号
        /// </summary>
		 public string PID { get; set; }
	    /// <summary>
        /// 生日
        /// </summary>
		 public DateTime? Birthday { get; set; }
	    /// <summary>
        /// 联系手机号
        /// </summary>
		 public string LinkMobile { get; set; }
	    /// <summary>
        /// 联系邮箱
        /// </summary>
		 public string LinkMail { get; set; }
	    /// <summary>
        /// 联系地址
        /// </summary>
		 public string LinkAddress { get; set; }
	    /// <summary>
        /// 省
        /// </summary>
		 public string Province { get; set; }
	    /// <summary>
        /// 市
        /// </summary>
		 public string City { get; set; }
	    /// <summary>
        /// 区
        /// </summary>
		 public string District { get; set; }
	    /// <summary>
        /// 用户来源途径
        /// </summary>
		 public string PathWay { get; set; }
	    /// <summary>
        /// 备注
        /// </summary>
		 public string Remark { get; set; }
	    /// <summary>
        /// 最后登录时间
        /// </summary>
		 public DateTime? LastLoginTime { get; set; }
		#endregion
    }

}
