using Dos.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using YiDaBus.Com.Dal.Base;
using YiDaBus.Com.Manager.Common;
using YiDaBus.Com.Mobile.Model.Enum;
using YiDaBus.Com.Mobile.Model.ResponseModel;
using YiDaBus.Com.Model;

namespace YiDaBus.Com.Mobile.Web.Base
{
    public class BaseController : Controller
    {
        #region 返回方法
        public ActionResult Sucess(string msg = "操作成功", int code = 200, object data = null)
        {
            return Json(new
            {
                code = code,
                msg = msg,
                data = data
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Error(string msg = "操作失败", int code = -1, object data = null)
        {
            return Json(new
            {
                code = code,
                msg = msg,
                data = data
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Error(ErrCode errcode, object data = null)
        {
            return Json(new
            {
                code = (int)errcode,
                msg = errcode.ToString(),
                data = data
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 公用方法
        /// <summary>
        /// 根据OpenId获取用户ID  
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public Wx_Users GetUserByOpenId(string openid)
        {
            return Db.MySqlContext.From<Wx_Users>().Where(d => d.OpenId == openid && d.IsDel == (int)IsDel.否).First();
        }

        //从Request中解析出Ticket,UserData
        public int GetUserId()
        {
            // 1. 读登录Cookie
            var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie == null || string.IsNullOrEmpty(cookie.Value)) return -1;

            try
            {
                // 2. 解密Cookie值，获取FormsAuthenticationTicket对象
                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                if (ticket != null && !string.IsNullOrEmpty(ticket.Name))
                {
                    return ticket.Name.ToInt();
                }
                return -1;
            }
            catch
            {
                /* 有异常也不要抛出，防止攻击者试探。 */
                return -1;
            }
        }

        public Wx_Users GetUserInfo()
        {
            // 1. 读登录Cookie
            var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie == null || string.IsNullOrEmpty(cookie.Value)) return null;

            try
            {
                // 2. 解密Cookie值，获取FormsAuthenticationTicket对象
                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                if (ticket != null && !string.IsNullOrEmpty(ticket.UserData))
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<Wx_Users>(ticket.UserData);
                }
                return null;
            }
            catch
            {
                /* 有异常也不要抛出，防止攻击者试探。 */
                return null;
            }
        }

        public DateTime GetDateTimeByWeek(int week)
        {
            var curDateTime = DateTime.Now;
            string weekstr = curDateTime.DayOfWeek.ToString();
            int curWeek = (int)((WeekEn)Enum.Parse(typeof(WeekEn), weekstr));//获取当前是星期几
            var day = week - curWeek;
            curDateTime = curDateTime.AddDays(day);
            return curDateTime;
        }

        public int GetWeekByDateTime(DateTime DateTime)
        {
            string weekstr = DateTime.DayOfWeek.ToString();
            return (int)((WeekEn)Enum.Parse(typeof(WeekEn), weekstr));//获取当前是星期几
        }
        #endregion
        #region 分页
        protected PageResponse<T> getListByPaging<T>(DbSession ds, string sql, int pageSize, int pageIndex)
        {
            if (pageIndex <= 0)
                pageIndex = 1;
            if (pageSize <= 0)
                pageSize = 10;
            string sqlCount, sqlPage;
            DatabaseType dstype = ds.Db.DbProvider.DatabaseType;
            BuildPageQueries<T>(dstype, (pageIndex - 1) * pageSize, pageSize, sql, out sqlCount, out sqlPage);
            // Save the one-time command time out and use it for both queries
            // Setup the paged result
            var result = new PageResponse<T>();
            result.currentPage = pageIndex;
            result.itemsPerPage = pageSize;
            result.totalItems = ds.FromSql(sqlCount).ToScalar<int>();
            result.totalPages = result.totalItems / pageSize;
            if ((result.totalItems % pageSize) != 0)
                result.totalPages++;
            // Get the records
            result.items = ds.FromSql(sqlPage).ToList<T>();
            // Done
            return result;
        }
        protected PageResponse<T> getListByPaging<T>(DbSession ds, string sqlPage, string sqlCount, int pageSize, int pageIndex)
        {
            var result = new PageResponse<T>();
            result.currentPage = pageIndex;
            result.itemsPerPage = pageSize;
            result.totalItems = ds.FromSql(sqlCount).ToScalar<int>();
            result.totalPages = result.totalItems / pageSize;
            if ((result.totalItems % pageSize) != 0)
                result.totalPages++;
            // Get the records
            result.items = ds.FromSql(sqlPage).ToList<T>();
            // Done
            return result;
        }
        /// <summary>
        /// 使用linq查询（分页）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="OutEntity"></typeparam>
        /// <param name="linq"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="lambdaOrderBy"></param>
        /// <returns></returns>
        public PageResponse<OutEntity> getListByPaging<TEntity, OutEntity>(FromSection<TEntity> linq, int pageSize, int pageIndex, Expression<Func<TEntity, object>> lambdaOrderBy = null) where TEntity : Entity
        {
            if (pageIndex <= 0)
                pageIndex = 1;
            if (pageSize <= 0)
                pageSize = 10;
            PageResponse<OutEntity> result = new PageResponse<OutEntity>();
            result.totalItems = linq.Count();
            List<OutEntity> sqlList = new List<OutEntity>();
            if (lambdaOrderBy != null)
            {
                sqlList = linq.OrderBy(lambdaOrderBy).Page(pageSize, pageIndex).ToList<OutEntity>();
            }
            else
            {
                sqlList = linq.Page(pageSize, pageIndex).ToList<OutEntity>();
            }

            if (sqlList == null || sqlList.Count == 0)
                return null;
            result.items = sqlList;
            result.currentPage = pageIndex;
            result.itemsPerPage = pageSize;
            result.totalPages = result.totalItems / pageSize;
            if ((result.totalItems % pageSize) != 0)
                result.totalPages++;
            return result;
        }
        #endregion
        #region 私有方法
        private void BuildPageQueries<T>(DatabaseType dstype, long skip, long take, string sql, out string sqlCount, out string sqlPage)
        {
            // Split the SQL into the bits we need
            string sqlSelectRemoved, sqlOrderBy;
            if (!SplitSqlForPaging(sql, out sqlCount, out sqlSelectRemoved, out sqlOrderBy))
                throw new Exception("Unable to parse SQL statement for paged query");
            if (dstype == DatabaseType.Oracle && sqlSelectRemoved.StartsWith("*"))
                throw new Exception("Query must alias '*' when performing a paged query.\neg. select t.* from table t order by t.id");

            // Build the SQL for the actual final result
            if (dstype == DatabaseType.SqlServer || dstype == DatabaseType.Oracle)
            {
                sqlSelectRemoved = rxOrderBy.Replace(sqlSelectRemoved, "");
                if (rxDistinct.IsMatch(sqlSelectRemoved))
                {
                    sqlSelectRemoved = "peta_inner.* FROM (SELECT " + sqlSelectRemoved + ") peta_inner";
                }
                sqlPage = string.Format("SELECT * FROM (SELECT ROW_NUMBER() OVER ({0}) peta_rn, {1}) peta_paged WHERE peta_rn>{2} AND peta_rn<={3}",
                                        sqlOrderBy == null ? "ORDER BY (SELECT NULL)" : sqlOrderBy, sqlSelectRemoved, skip, skip + take);
            }
            else
            {
                sqlPage = string.Format("{0}\nLIMIT {1},{2}", sql, skip, take);
            }
        }
        static Regex rxColumns = new Regex(@"\A\s*SELECT\s+((?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|.)*?)(?<!,\s+)\bFROM\b", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
        static Regex rxDistinct = new Regex(@"\ADISTINCT\s", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
        static Regex rxOrderBy = new Regex(@"\bORDER\s+BY\s+(?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|[\w\(\)\.])+(?:\s+(?:ASC|DESC))?(?:\s*,\s*(?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|[\w\(\)\.])+(?:\s+(?:ASC|DESC))?)*", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
        private static bool SplitSqlForPaging(string sql, out string sqlCount, out string sqlSelectRemoved, out string sqlOrderBy)
        {
            sqlSelectRemoved = null;
            sqlCount = null;
            sqlOrderBy = null;

            // Extract the columns from "SELECT <whatever> FROM"
            var m = rxColumns.Match(sql);
            if (!m.Success)
                return false;

            // Save column list and replace with COUNT(*)
            Group g = m.Groups[1];
            sqlSelectRemoved = sql.Substring(g.Index);

            if (rxDistinct.IsMatch(sqlSelectRemoved))
                sqlCount = sql.Substring(0, g.Index) + "COUNT(" + m.Groups[1].ToString().Trim() + ") " + sql.Substring(g.Index + g.Length);
            else
                sqlCount = sql.Substring(0, g.Index) + "COUNT(*) " + sql.Substring(g.Index + g.Length);


            // Look for an "ORDER BY <whatever>" clause
            m = rxOrderBy.Match(sqlCount);
            if (!m.Success)
            {
                sqlOrderBy = null;
            }
            else
            {
                g = m.Groups[0];
                sqlOrderBy = g.ToString();
                sqlCount = sqlCount.Substring(0, g.Index) + sqlCount.Substring(g.Index + g.Length);
            }

            return true;
        }
        #endregion
    }
}