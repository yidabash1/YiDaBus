	
using System;
using Dos.ORM;

namespace YiDaBus.Com.UtilsModel
{


    [Serializable]
	public  class sys_user
    {

	    #region Field
	    /// <summary>
        /// 
        /// </summary>
		 public string F_Id { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string F_Account { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string F_RealName { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string F_NickName { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string F_HeadIcon { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public byte F_Gender { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public byte[] F_Birthday { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string F_MobilePhone { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string F_Email { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string F_WeChat { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string F_ManagerId { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public int? F_SecurityLevel { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string F_Signature { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string F_OrganizeId { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string F_DepartmentId { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string F_RoleId { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string F_DutyId { get; set; }
	    /// <summary>
        /// 系统用户所在的区域
        /// </summary>
		 public string F_Area { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public byte F_IsAdministrator { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public int? F_SortCode { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public byte F_DeleteMark { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public byte F_EnabledMark { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string F_Description { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public byte[] F_CreatorTime { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string F_CreatorUserId { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public byte[] F_LastModifyTime { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string F_LastModifyUserId { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public byte[] F_DeleteTime { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string F_DeleteUserId { get; set; }
		#endregion
    }

}
