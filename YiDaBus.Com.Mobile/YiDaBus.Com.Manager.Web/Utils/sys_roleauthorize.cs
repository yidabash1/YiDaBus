	
using System;
using Dos.ORM;

namespace YiDaBus.Com.UtilsModel
{


    [Serializable]
	public  class sys_roleauthorize
    {

	    #region Field
	    /// <summary>
        /// 
        /// </summary>
		 public string F_Id { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public int? F_ItemType { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string F_ItemId { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public int? F_ObjectType { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string F_ObjectId { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public int? F_SortCode { get; set; }
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
