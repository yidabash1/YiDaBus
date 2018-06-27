	
using System;
using Dos.ORM;

namespace YiDaBus.Com.UtilsModel
{


    [Serializable]
	public  class users
    {

	    #region Field
	    /// <summary>
        /// 主键自增ID号
        /// </summary>
		 public int Id { get; set; }
	    /// <summary>
        /// 登录账号
        /// </summary>
		 public string LogAccount { get; set; }
	    /// <summary>
        /// 微信ID号
        /// </summary>
		 public string OpenId { get; set; }
	    /// <summary>
        /// 用户头像
        /// </summary>
		 public string Avatar { get; set; }
	    /// <summary>
        /// 用户真实姓名
        /// </summary>
		 public string UserRealName { get; set; }
	    /// <summary>
        /// 用户性别
        /// </summary>
		 public string UserGender { get; set; }
	    /// <summary>
        /// 用户押金
        /// </summary>
		 public decimal UserDeposit { get; set; }
	    /// <summary>
        /// 是否锁定押金：0表示未锁定；1表示已锁定；
        /// </summary>
		 public int? IsLockDeposit { get; set; }
	    /// <summary>
        /// 账户余额
        /// </summary>
		 public decimal AccountBalance { get; set; }
	    /// <summary>
        /// 用户类型：0-租客用户 ；1-房东
        /// </summary>
		 public int? UserType { get; set; }
	    /// <summary>
        /// 用户状态：0-待审核；1-已审核；2-审核失败；3-已禁用；4-黑名单
        /// </summary>
		 public int? UserStatus { get; set; }
	    /// <summary>
        /// 软删除：0表示未删除；1表示已删除
        /// </summary>
		 public int? IsDel { get; set; }
	    /// <summary>
        /// 用户创建时间
        /// </summary>
		 public DateTime? AddTime { get; set; }
	    /// <summary>
        /// 用户更新时间
        /// </summary>
		 public DateTime? LastUpdateTime { get; set; }
		#endregion
    }

}
