	
using System;
using Dos.ORM;

namespace YiDaBus.Com.UtilsModel
{


    [Serializable]
	public  class loginlog
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
        /// 登录账号
        /// </summary>
		 public string LoginAccount { get; set; }
	    /// <summary>
        /// 登录时间
        /// </summary>
		 public DateTime? LoginTime { get; set; }
	    /// <summary>
        /// 登录的IP地址
        /// </summary>
		 public string LoginIp { get; set; }
	    /// <summary>
        /// 软删除：0表示未删除；1表示已删除
        /// </summary>
		 public int? IsDel { get; set; }
		#endregion
    }

}
