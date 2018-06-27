	
using System;
using Dos.ORM;

namespace YiDaBus.Com.UtilsModel
{


    [Serializable]
	public  class userfeedback
    {

	    #region Field
	    /// <summary>
        /// 主键自增ID号
        /// </summary>
		 public int Id { get; set; }
	    /// <summary>
        /// 反馈人ID
        /// </summary>
		 public int? UserID { get; set; }
	    /// <summary>
        /// 反馈人姓名
        /// </summary>
		 public string UserName { get; set; }
	    /// <summary>
        /// 反馈人联系方式
        /// </summary>
		 public string UserLinkWay { get; set; }
	    /// <summary>
        /// 反馈时间
        /// </summary>
		 public DateTime? AddTime { get; set; }
	    /// <summary>
        /// 反馈处理状态
        /// </summary>
		 public int? HandleStatus { get; set; }
	    /// <summary>
        /// 软删除：0表示未删除；1表示已删除
        /// </summary>
		 public int? IsDel { get; set; }
	    /// <summary>
        /// 反馈内容
        /// </summary>
		 public string FeedbackContent { get; set; }
		#endregion
    }

}
