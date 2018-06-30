using YiDaBus.Com.Manager.Common;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web.Mvc;
using Dos.ORM;
using NFine.Application;
using NFine.Domain.Entity.SystemSecurity;
using NFine.Application.SystemSecurity;
using YiDaBus.Com.Dal.Base;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using YiDaBus.Com.Mobile.Model.Const;

namespace YiDaBus.Com.Manager.Web
{
    [HandlerLogin]
    public abstract class ControllerBase : Controller
    {
        public virtual string UserCode { get; set; } = YiDaBus.Com.Manager.Common.OperatorProvider.Provider.GetCurrent().UserCode;
        public virtual string UserName { get; set; } = YiDaBus.Com.Manager.Common.OperatorProvider.Provider.GetCurrent().UserName;
        public virtual string tableName { get; set; } //表名
        public virtual string f_ModuleName { get; set; } //模块的中文名称
        /// <summary>
                                                         /// 获取接口域名（末尾带反斜杠）
                                                         /// </summary>
                                                         /// <returns></returns>
        public static string getInterFaceDomain()
        {
            return Configs.GetValue(YiDaBusConst.INTERFACE_DOMAIN);
        }
        #region 【1】页面初始化
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult Form()
        {
            return View();
        }
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult Details()
        {
            return View();
        }
        #endregion
        #region 【2】返回操作类
        protected virtual ActionResult Success(string message)
        {
            return Content(new AjaxResult { status = 1, msg = message }.ToJson());
        }
        protected virtual ActionResult Success(string message, string F_ModuleName, DbLogType DbLogType)
        {
            LogApp(F_ModuleName, DbLogType);
            return Content(new AjaxResult { status = 1, msg = message }.ToJson());
        }
        protected virtual ActionResult Success(string message, string F_ModuleName, DbLogType DbLogType, string F_Description)
        {
            LogApp(F_ModuleName, DbLogType, F_Description);
            return Content(new AjaxResult { status = 1, msg = message }.ToJson());
        }
        protected virtual ActionResult Success(string message, object data)
        {
            return Content(new AjaxResult { status = 1, msg = message, data = data }.ToJson());
        }
        protected virtual ActionResult Error(string message)
        {
            return Content(new AjaxResult { status = 0, msg = message }.ToJson());
        }
        protected virtual ActionResult Error(string message, string F_ModuleName, DbLogType DbLogType)
        {
            LogApp(F_ModuleName, DbLogType);
            return Content(new AjaxResult { status = 0, msg = message }.ToJson());
        }
        protected virtual ActionResult Error(string message, string F_ModuleName, DbLogType DbLogType, string F_Description)
        {
            LogApp(F_ModuleName, DbLogType, F_Description);
            return Content(new AjaxResult { status = 0, msg = message }.ToJson());
        }
        #endregion
        #region 【3】添加日志
        public Log FileLog
        {
            get { return LogFactory.GetLogger(this.GetType().ToString()); }
        }
        /// <summary>
        /// 添加系统日志
        /// </summary>
        /// <param name="F_ModuleName">模块名称</param>
        /// <param name="F_Type">模块类型【DbLogType】</param>
        protected void LogApp(string F_ModuleName, DbLogType F_Type)
        {
            LogApp(F_ModuleName, F_Type, GetEnumDescription(F_Type) + F_ModuleName);
        }

        /// <summary>
        /// 添加系统日志
        /// </summary>
        /// <param name="F_ModuleName">模块名称</param>
        /// <param name="F_Type">模块类型【DbLogType】</param>
        /// <param name="F_Description">模块描述</param>
        protected void LogApp(string F_ModuleName, DbLogType F_Type, string F_Description)
        {
            OperatorModel op = OperatorProvider.Provider.GetCurrent();
            LogEntity logEntity = new LogEntity();
            logEntity.F_ModuleName = F_ModuleName;
            logEntity.F_Type = F_Type.ToString();
            logEntity.F_Account = op.UserCode;
            logEntity.F_NickName = op.UserName;
            logEntity.F_Result = true;
            logEntity.F_Description = F_Description;
            new LogApp().WriteDbLog(logEntity);
        }
        #endregion
        #region 【4】数据操作类
        #region 通用分页查询
        protected ActionResult getListByPaging<T>(DbSession ds, string sql, Pagination pagination)
        {
            int pageIndex = pagination.page;
            int pageSize = pagination.rows;
            string sqlCount, sqlPage;
            DatabaseType dstype = ds.Db.DbProvider.DatabaseType;
            BuildPageQueries<T>(dstype, (pageIndex - 1) * pageSize, pageSize, sql, out sqlCount, out sqlPage);

            int totalCount = ds.FromSql(sqlCount).ToScalar<int>();
            return Content(new
            {
                rows = ds.FromSql(sqlPage).ToList<T>(),
                total = (totalCount / pageSize) + 1,
                page = pagination.page,
                records = totalCount
            }.ToJson());
        }

        /// <summary>
        /// 使用linq查询（分页）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="OutEnity"></typeparam>
        /// <param name="linq"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="lambdaOrderBy"></param>
        /// <returns></returns>
        public ActionResult getListByPaging<TEntity, OutEnity>(FromSection<TEntity> linq, Pagination pagination, OrderByClip lambdaOrderBy = null) where TEntity : Entity
        {
            int pageIndex = pagination.page;
            int pageSize = pagination.rows <= 0 ? 20 : pagination.rows;
            List<OutEnity> sqlList = new List<OutEnity>();
            if (lambdaOrderBy != null)
            {
                sqlList = linq.OrderBy(lambdaOrderBy).Page(pageSize, pageIndex).ToList<OutEnity>();
            }
            else
            {
                sqlList = linq.Page(pageSize, pageIndex).ToList<OutEnity>();
            }
            int totalCount = linq.Count();
            return Content(new
            {
                rows = sqlList,
                total = (totalCount / pageSize) + 1,
                page = pagination.page,
                records = totalCount
            }.ToJson());
        }
        #endregion
        /// <summary>
        /// 获取列表数据（不分页）
        /// </summary>
        /// <param name="sqlWhere">条件语句</param>
        /// <param name="sqlOrder">排序语句</param>
        /// <param name="tableName">表名</param>
        /// <param name="f_ModuleName">模块名称</param>
        /// <returns></returns>
        protected ActionResult GetGridJsonBySql(DbSession ds, string sqlWhere, string sqlOrder, string tableName)
        {
            try
            {
                string sql = string.Format(@"SELECT  *
                            FROM    {0}
                            WHERE   IsDel = 0
                            {1} {2}", tableName, sqlWhere, sqlOrder);
                return Content(ds.FromSql(sql).ToDataTable().ToJson());
            }
            catch (System.Exception ex)
            {
                LogApp(f_ModuleName, DbLogType.Exception, ex.Message);
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 获取表单数据
        /// </summary>
        /// <param name="ds">操作对象</param>
        /// <param name="keyValue">主键ID</param>
        /// <param name="tableName">表名</param>
        /// <param name="f_ModuleName">模块名称</param>
        /// <param name="isSoftDel">是否软删除（默认为软删除）</param>
        /// <returns></returns>
        protected ActionResult GetFormJsonBySql(DbSession ds, string keyValue)
        {
            try
            {
                string sql = string.Format(@"SELECT  *
                            FROM    {0}
                            WHERE   IsDel = 0
                            AND Id= {1}", tableName, keyValue);
                return Content(ds.FromSql(sql).ToDataTable().ToJsonByColName());
            }
            catch (System.Exception ex)
            {
                LogApp(f_ModuleName, DbLogType.Exception, ex.Message);
                return Error(ex.Message);
            }
        }
        public delegate ActionResult AddSubmitCallHandle(int Id);
        /// <summary>
        /// 利用泛型新增或者修改form表单，如果keyvalue为空，则表示新增；否则表示修改
        /// </summary>
        /// <param name="ds">操作对象</param>
        /// <param name="model">模型</param>
        /// <param name="keyValue">主键ID</param>
        /// <param name="f_ModuleName">模块名称</param>
        /// <returns></returns>
        protected ActionResult SubmitForms<TEntity>(DbSession ds, TEntity entity, string keyValue, AddSubmitCallHandle addSubmitCallHandle = null) where TEntity : Entity
        {
            try
            {
                if (string.IsNullOrEmpty(keyValue))//新增
                {
                    int autoId = ds.Insert<TEntity>(entity);
                    if (autoId > 0)
                    {
                        LogApp(f_ModuleName, DbLogType.Create, "新增对象【" + f_ModuleName + "】；生成主键ID【" + autoId + "】！");
                        if (addSubmitCallHandle != null) return addSubmitCallHandle(autoId);
                        else return Success("新增" + f_ModuleName + "成功");
                    }
                    else
                    {
                        LogApp(f_ModuleName, DbLogType.Fail, "新增对象【" + f_ModuleName + "】失败！");
                        return Error("新增" + f_ModuleName + "失败");
                    }
                }
                else//修改
                {
                    int affectedId = ds.Update<TEntity>(entity);
                    if (affectedId > 0)
                    {
                        LogApp(f_ModuleName, DbLogType.Update, "修改对象【" + f_ModuleName + "】成功；主键ID【" + keyValue + "】！");
                        return Success("修改" + f_ModuleName + "成功");
                    }
                    else
                    {
                        LogApp(f_ModuleName, DbLogType.Fail, "修改对象【" + f_ModuleName + "】失败！");
                        return Error("修改" + f_ModuleName + "失败");
                    }
                }
            }
            catch (System.Exception ex)
            {
                LogApp(f_ModuleName, DbLogType.Exception, ex.Message);
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 删除数据行
        /// </summary>
        /// <param name="ds">操作对象</param>
        /// <param name="keyValue">主键ID</param>
        /// <param name="tableName">表名</param>
        /// <param name="f_ModuleName">模块名称</param>
        /// <param name="isSoftDel">是否软删除（默认为软删除）</param>
        /// <returns></returns>
        protected ActionResult DeleteFormBySql(DbSession ds, string keyValue, string tableName, bool isSoftDel = true)
        {
            try
            {
                if (string.IsNullOrEmpty(keyValue))
                {
                    return Error("请选择要删除的记录！");
                }
                string sql = string.Empty;
                if (isSoftDel)
                {
                    sql = string.Format(@"UPDATE  {0}
                                SET     IsDel = 1
                                WHERE   Id = {1}", tableName, keyValue);
                }
                else
                {
                    sql = string.Format(@"DELETE  {0}
                                    WHERE   Id = {1}", tableName, keyValue);
                }

                int affectedId = ds.FromSql(sql).ExecuteNonQuery();
                if (affectedId > 0)
                {
                    LogApp(f_ModuleName, DbLogType.Create, "删除对象【" + f_ModuleName + "】；主键ID【" + keyValue + "】！");
                    return Success("删除" + f_ModuleName + "成功");
                }
                else
                {
                    LogApp(f_ModuleName, DbLogType.Fail, "删除对象【" + f_ModuleName + "】失败！");
                    return Error("删除" + f_ModuleName + "失败");
                }
            }
            catch (System.Exception ex)
            {
                LogApp(f_ModuleName, DbLogType.Exception, ex.Message);
                return Error(ex.Message);
            }
        }
        #endregion
        #region 【5】uip调用方法
        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="str">键值对</param>
        /// <param name="appkey">appkey</param>
        /// <returns></returns>
        protected string GetSignature(string str, string appkey)
        {
            //signature不参与签名验证
            var bytes = Encoding.UTF8.GetBytes(str);
            var hash = System.Security.Cryptography.MD5.Create();
            //使用MD5加密
            byte[] hashBytes = hash.ComputeHash(bytes);

            StringBuilder signStringBuilder = new StringBuilder();
            //把二进制转化为大写的十六进制
            foreach (byte hashByte in hashBytes)
            {
                signStringBuilder.Append(hashByte.ToString("X2"));
            }

            return signStringBuilder.ToString().ToUpper();
        }
        /// <summary>
        /// 时间转时间戳
        /// </summary>
        /// <returns></returns>
        protected double ConvertDateTimeInt()
        {
            return (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }

        protected class RequestParam
        {
            /// <summary>
            /// 请求的方法
            /// </summary>
            public string MethodId { get; set; }

            /// <summary>
            /// 请求方法使用的参数
            /// application/x-www-form-urlencoded 参数使用键值对拼接<![CDATA[如：a=1&b=2&c=3]]>
            /// application/json 参数使用json，序列化后的字符串形式
            /// </summary>
            public string MethodParam { get; set; }
        }
        /// <summary>
        /// 调用UIP
        /// </summary>
        /// <param name="methodId">方法ID号</param>
        /// <param name="methodParam">方法参数</param>
        /// <returns></returns>
        protected string UIPPost(string methodId, string methodParam)
        {
            string url = @"http://192.168.1.44:1001//api/uip/";
            var appid = "appid1";
            var appkey = "AppKey1";
            var nonce = Guid.NewGuid().ToString();
            var timestamp = ConvertDateTimeInt();
            var str = $"appid={appid}&methodid={methodId}&methodparam={methodParam }&nonce={nonce}&timestamp={timestamp}&appkey={appkey}";
            string signature = GetSignature(str, "");
            HttpClient hc = new HttpClient();
            FormUrlEncodedContent content = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                {"appid", appid},
                {"appkey", appkey},
                {"methodid", methodId},
                { "methodparam", methodParam},
                {"nonce", nonce},
                {"timestamp", timestamp.ToString()},
                {"signature", signature}
            });
            //await异步等待回应
            HttpResponseMessage hrm = hc.PostAsync(url, content).Result;
            return hrm.Content.ReadAsStringAsync().Result;
        }
        #endregion
        #region 常用方法

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
        private string GetEnumDescription(Enum enumValue)
        {
            string str = enumValue.ToString();
            System.Reflection.FieldInfo field = enumValue.GetType().GetField(str);
            object[] objs = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            if (objs == null || objs.Length == 0) return str;
            System.ComponentModel.DescriptionAttribute da = (System.ComponentModel.DescriptionAttribute)objs[0];
            return da.Description;
        }
        #endregion
    }
}
