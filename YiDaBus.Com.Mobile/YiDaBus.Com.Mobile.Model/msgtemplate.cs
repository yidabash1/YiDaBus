	
using System;
using Dos.ORM;

namespace YiDaBus.Com.UtilsModel
{


    [Serializable]
	public  class msgtemplate
    {

	    #region Field
	    /// <summary>
        /// 主键自增ID号
        /// </summary>
		 public int Id { get; set; }
	    /// <summary>
        /// 模板类型：0表示短信；1表示邮箱；
        /// </summary>
		 public int? TemplateType { get; set; }
	    /// <summary>
        /// 模板标题
        /// </summary>
		 public string TemplateTitle { get; set; }
	    /// <summary>
        /// 调用别名
        /// </summary>
		 public string CallName { get; set; }
	    /// <summary>
        /// 模板内容
        /// </summary>
		 public string TemplateContent { get; set; }
	    /// <summary>
        /// 创建人ID
        /// </summary>
		 public string CreatorID { get; set; }
	    /// <summary>
        /// 创建人姓名
        /// </summary>
		 public string CreatorName { get; set; }
	    /// <summary>
        /// 操作人ID
        /// </summary>
		 public string OperatorID { get; set; }
	    /// <summary>
        /// 操作人姓名
        /// </summary>
		 public string OperatorName { get; set; }
	    /// <summary>
        /// 备注
        /// </summary>
		 public string Remark { get; set; }
	    /// <summary>
        /// 模板创建时间
        /// </summary>
		 public DateTime? AddTime { get; set; }
	    /// <summary>
        /// 模板更新时间
        /// </summary>
		 public DateTime? LastUpdateTime { get; set; }
	    /// <summary>
        /// 软删除：0表示未删除；1表示已删除
        /// </summary>
		 public int? IsDel { get; set; }
		#endregion
    }

}
