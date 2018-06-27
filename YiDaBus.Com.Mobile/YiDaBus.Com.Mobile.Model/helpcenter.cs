	
using System;
using Dos.ORM;

namespace YiDaBus.Com.UtilsModel
{


    [Serializable]
	public  class helpcenter
    {

	    #region Field
	    /// <summary>
        /// 主键自增ID号
        /// </summary>
		 public int Id { get; set; }
	    /// <summary>
        /// 帮助标题
        /// </summary>
		 public string HelpTitle { get; set; }
	    /// <summary>
        /// 帮助内容说明
        /// </summary>
		 public string HelpContent { get; set; }
	    /// <summary>
        /// 排序
        /// </summary>
		 public int? Sort { get; set; }
	    /// <summary>
        /// 操作人ID
        /// </summary>
		 public string OperatorID { get; set; }
	    /// <summary>
        /// 操作人姓名
        /// </summary>
		 public string OperatorName { get; set; }
	    /// <summary>
        /// 软删除：0表示未删除；1表示已删除
        /// </summary>
		 public int? IsDel { get; set; }
	    /// <summary>
        /// 创建时间
        /// </summary>
		 public DateTime? AddTime { get; set; }
	    /// <summary>
        /// 最后修改时间
        /// </summary>
		 public DateTime? LastUpdateTime { get; set; }
		#endregion
    }

}
