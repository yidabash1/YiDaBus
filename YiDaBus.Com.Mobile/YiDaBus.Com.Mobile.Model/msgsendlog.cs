	
using System;
using Dos.ORM;

namespace YiDaBus.Com.UtilsModel
{


    [Serializable]
	public  class msgsendlog
    {

	    #region Field
	    /// <summary>
        /// 主键自增ID号
        /// </summary>
		 public int Id { get; set; }
	    /// <summary>
        /// 消息类型：0代表系统消息；1代表用户消息
        /// </summary>
		 public int? MsgType { get; set; }
	    /// <summary>
        /// 发送人ID
        /// </summary>
		 public string SenderID { get; set; }
	    /// <summary>
        /// 发送人账号
        /// </summary>
		 public string SenderAccount { get; set; }
	    /// <summary>
        /// 发送人姓名
        /// </summary>
		 public string SenderName { get; set; }
	    /// <summary>
        /// 发送内容
        /// </summary>
		 public string SendContent { get; set; }
	    /// <summary>
        /// 发送时间
        /// </summary>
		 public DateTime? AddTime { get; set; }
	    /// <summary>
        /// 备注
        /// </summary>
		 public string Remark { get; set; }
	    /// <summary>
        /// 软删除：0表示未删除；1表示已删除
        /// </summary>
		 public int? IsDel { get; set; }
		#endregion
    }

}
