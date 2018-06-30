	
using System;
using Dos.ORM;

namespace YiDaBus.Com.UtilsModel
{


    [Serializable]
	public  class messagetemplate
    {

	    #region Field
	    /// <summary>
        /// 
        /// </summary>
		 public int Id { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string MCode { get; set; }
	    /// <summary>
        /// 模块:数据字典中设置
        /// </summary>
		 public string Module { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string Content { get; set; }
	    /// <summary>
        /// 用途:1通用消息;2微信消息
        /// </summary>
		 public int MUse { get; set; }
	    /// <summary>
        /// 软删除：0表示未删除；1表示已删除
        /// </summary>
		 public int IsDel { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string Creator { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public DateTime CreateTime { get; set; }
	    /// <summary>
        /// 
        /// </summary>
		 public string Remark { get; set; }
		#endregion
    }

}
