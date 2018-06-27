	
using System;
using Dos.ORM;

namespace YiDaBus.Com.UtilsModel
{


    [Serializable]
	public  class bookviewhouse
    {

	    #region Field
	    /// <summary>
        /// 主键自增ID号
        /// </summary>
		 public int Id { get; set; }
	    /// <summary>
        /// 预约人ID号
        /// </summary>
		 public int? UserId { get; set; }
	    /// <summary>
        /// 房源ID号
        /// </summary>
		 public int? HouseId { get; set; }
	    /// <summary>
        /// 预约人账号
        /// </summary>
		 public string UserAccount { get; set; }
	    /// <summary>
        /// 预约人称呼
        /// </summary>
		 public string UserCall { get; set; }
	    /// <summary>
        /// 性别：0表示先生；1表示女士；
        /// </summary>
		 public int? Gender { get; set; }
	    /// <summary>
        /// 预约时间
        /// </summary>
		 public DateTime? BookTime { get; set; }
	    /// <summary>
        /// 创建时间
        /// </summary>
		 public DateTime? AddTime { get; set; }
	    /// <summary>
        /// 软删除：0表示未删除；1表示已删除
        /// </summary>
		 public int? IsDel { get; set; }
		#endregion
    }

}
