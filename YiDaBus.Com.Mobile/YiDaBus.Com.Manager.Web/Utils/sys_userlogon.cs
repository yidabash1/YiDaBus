	
using System;
using Dos.ORM;

namespace YiDaBus.Com.UtilsModel
{


    [Serializable]
	public  class sys_userlogon
    {

	    #region Field
	    /// <summary>
        /// 
        /// </summary>
		 public string F_Id { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string F_UserId { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string F_UserPassword { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string F_UserSecretkey { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public byte[] F_AllowStartTime { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public byte[] F_AllowEndTime { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public byte[] F_LockStartDate { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public byte[] F_LockEndDate { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public byte[] F_FirstVisitTime { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public byte[] F_PreviousVisitTime { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public byte[] F_LastVisitTime { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public byte[] F_ChangePasswordDate { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public byte F_MultiUserLogin { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public int? F_LogOnCount { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public byte F_UserOnLine { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string F_Question { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string F_AnswerQuestion { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public byte F_CheckIPAddress { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string F_Language { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string F_Theme { get; set; }
		#endregion
    }

}
