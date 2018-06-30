	
using System;
using Dos.ORM;

namespace YiDaBus.Com.UtilsModel
{


    [Serializable]
	public  class sys_log
    {

	    #region Field
	    /// <summary>
        /// 
        /// </summary>
		 public string F_Id { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public byte[] F_Date { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string F_Account { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string F_NickName { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string F_Type { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string F_IPAddress { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string F_IPAddressName { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string F_ModuleId { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string F_ModuleName { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public byte F_Result { get; set; }
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
		#endregion
    }

}
