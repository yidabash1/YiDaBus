using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiDaBus.Com.Mobile.Model.ResponseModel
{
    public class PageResponse<T>
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public long currentPage { get; set; }
        /// <summary>
        /// 每页的个数
        /// </summary>
        public long itemsPerPage { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public long totalPages { get; set; }
        /// <summary>
        /// 总个数
        /// </summary>
        public long totalItems { get; set; }
        
        /// <summary>
        /// 列表数据
        /// </summary>
        public List<T> items { get; set; }
    }
}
