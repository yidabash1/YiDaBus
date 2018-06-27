	
using System;
using Dos.ORM;

namespace YiDaBus.Com.UtilsModel
{


    [Serializable]
	public  class usercomplaint
    {

	    #region Field
	    /// <summary>
        /// 主键自增ID号
        /// </summary>
		 public int Id { get; set; }
	    /// <summary>
        /// 投诉人ID
        /// </summary>
		 public int? UserID { get; set; }
	    /// <summary>
        /// 投诉人姓名
        /// </summary>
		 public string UserName { get; set; }
	    /// <summary>
        /// 投诉人联系方式
        /// </summary>
		 public string UserLinkWay { get; set; }
	    /// <summary>
        /// 投诉内容
        /// </summary>
		 public string ComplaintContent { get; set; }
	    /// <summary>
        /// 投诉创建时间
        /// </summary>
		 public DateTime? AddTime { get; set; }
	    /// <summary>
        /// 回复人ID
        /// </summary>
		 public string ReplyID { get; set; }
	    /// <summary>
        /// 回复人姓名
        /// </summary>
		 public string ReplyName { get; set; }
	    /// <summary>
        /// 回复状态：0表示待回复；1表示已回复；
        /// </summary>
		 public int? ReplyStatus { get; set; }
	    /// <summary>
        /// 回复内容
        /// </summary>
		 public string ReplyContent { get; set; }
	    /// <summary>
        /// 投诉回复时间
        /// </summary>
		 public DateTime? ReplyTime { get; set; }
	    /// <summary>
        /// 软删除：0表示未删除；1表示已删除
        /// </summary>
		 public int? IsDel { get; set; }
		#endregion
    }

}
