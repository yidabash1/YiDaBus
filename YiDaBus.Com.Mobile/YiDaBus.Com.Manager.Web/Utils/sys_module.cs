	
using System;
using Dos.ORM;

namespace YiDaBus.Com.UtilsModel
{


    [Serializable]
	public  class sys_module
    {

	    #region Field
	    /// <summary>
        /// 
        /// </summary>
		 public string F_Id { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string F_ParentId { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public int? F_Layers { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string F_EnCode { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string F_FullName { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string F_Icon { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string F_UrlAddress { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string F_Target { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public byte F_IsMenu { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public byte F_IsExpand { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public byte F_IsPublic { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public byte F_AllowEdit { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public byte F_AllowDelete { get; set; }
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
