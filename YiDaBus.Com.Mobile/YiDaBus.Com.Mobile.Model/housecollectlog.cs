	
using System;
using Dos.ORM;

namespace YiDaBus.Com.UtilsModel
{


    [Serializable]
	public  class housecollectlog
    {

	    #region Field
	    /// <summary>
        /// 主键自增ID号
        /// </summary>
		 public int Id { get; set; }
	    /// <summary>
        /// 收藏人ID号
        /// </summary>
		 public int? UserId { get; set; }
	    /// <summary>
        /// 房源ID号
        /// </summary>
		 public int? HouseId { get; set; }
	    /// <summary>
        /// 房源标题
        /// </summary>
		 public string HouseTitle { get; set; }
	    /// <summary>
        /// 软删除：0表示未删除；1表示已删除
        /// </summary>
		 public int? IsDel { get; set; }
	    /// <summary>
        /// 收藏时间
        /// </summary>
		 public DateTime? AddTime { get; set; }
	    /// <summary>
        /// 收藏类型：0表示房源；1表示小区；
        /// </summary>
		 public int? CollectType { get; set; }
		#endregion
    }

}
