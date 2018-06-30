	
using System;
using Dos.ORM;

namespace YiDaBus.Com.UtilsModel
{


    [Serializable]
	public  class newsdetail
    {

	    #region Field
	    /// <summary>
        /// 主键自增ID号
        /// </summary>
		 public int Id { get; set; }
	    /// <summary>
        /// 标签
        /// </summary>
		 public string Tags { get; set; }
	    /// <summary>
        /// 关键词描述
        /// </summary>
		 public string Keywords { get; set; }
	    /// <summary>
        /// 标题
        /// </summary>
		 public string NewsTitle { get; set; }
	    /// <summary>
        /// 简介
        /// </summary>
		 public string NewsIntro { get; set; }
	    /// <summary>
        /// 封面图
        /// </summary>
		 public string CoverImg { get; set; }
	    /// <summary>
        /// 内容
        /// </summary>
		 public string NewsContent { get; set; }
	    /// <summary>
        /// 发布平台，如：PC、Android、IOS、微信
        /// </summary>
		 public string PublishPlatform { get; set; }
	    /// <summary>
        /// 是否轮播：0-否 1-是
        /// </summary>
		 public int IsBanner { get; set; }
	    /// <summary>
        /// 是否置顶：0-否 1-是
        /// </summary>
		 public int IsTop { get; set; }
	    /// <summary>
        /// 是否热门：0-否 1-是
        /// </summary>
		 public int IsHot { get; set; }
	    /// <summary>
        /// 是否首页显示：0-否 1-是
        /// </summary>
		 public int IsHomepage { get; set; }
	    /// <summary>
        /// 排序
        /// </summary>
		 public int? Sort { get; set; }
	    /// <summary>
        /// 浏览量
        /// </summary>
		 public int? PV { get; set; }
	    /// <summary>
        /// 点赞量
        /// </summary>
		 public int? PQ { get; set; }
	    /// <summary>
        /// 分享量
        /// </summary>
		 public int? SQ { get; set; }
	    /// <summary>
        /// 评论数
        /// </summary>
		 public int? CC { get; set; }
	    /// <summary>
        /// 新闻来源
        /// </summary>
		 public string Source { get; set; }
	    /// <summary>
        /// 发布人Id
        /// </summary>
		 public string PublishUserId { get; set; }
	    /// <summary>
        /// 发布人姓名
        /// </summary>
		 public string PublishUserName { get; set; }
	    /// <summary>
        /// 发布时间
        /// </summary>
		 public DateTime? PublishTime { get; set; }
	    /// <summary>
        /// 最后修改时间
        /// </summary>
		 public DateTime? LastUpdateTime { get; set; }
	    /// <summary>
        /// 软删除：0表示未删除；1表示已删除
        /// </summary>
		 public int? IsDel { get; set; }
		#endregion
    }

}
