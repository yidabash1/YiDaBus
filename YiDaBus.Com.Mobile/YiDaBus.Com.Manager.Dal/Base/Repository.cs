using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Dos.Common;
using Dos.ORM;
using Dos.ORM.Common;
using System.Text.RegularExpressions;

namespace YiDaBus.Com.Dal.Base
{
    public abstract class Repository<T> where T : Entity
    {
        static Repository()
        {
            Db.MySqlContext.RegisterSqlLogger(delegate (string sql)
            {
                //在此可以记录sql日志
                //写日志会影响性能，建议开发版本记录sql以便调试，发布正式版本不要记录
                LogHelper.Debug(sql, "SQL日志");
            });
        }
        #region 查询
        /// <summary>
        /// 获取整表数据
        /// </summary>
        /// <returns></returns>
        public static List<T> GetAll()
        {
            return Db.MySqlContext.From<T>().ToList();
        }
        /// <summary>
        /// 通用查询
        /// </summary>
        public static List<T> Query(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy = null, string ascOrDesc = "asc", int? top = null, int? pageSize = null, int? pageIndex = null)
        {
            var fs = Db.MySqlContext.From<T>().Where(where);
            if (top != null)
            {
                fs.Top(top.Value);
            }
            else if (pageIndex != null && pageSize != null)
            {
                fs.Page(pageSize.Value, pageIndex.Value);
            }
            if (orderBy != null)
            {
                if (ascOrDesc.ToLower() == "asc")
                {
                    fs.OrderBy(orderBy);
                }
                else
                {
                    fs.OrderByDescending(orderBy);
                }
            }
            return fs.ToList();
        }
        /// <summary>
        /// 通用查询
        /// </summary>
        public static List<T> Query(Where<T> where, Expression<Func<T, object>> orderBy = null, string ascOrDesc = "asc", int? top = null, int? pageSize = null, int? pageIndex = null)
        {
            var fs = Db.MySqlContext.From<T>().Where(where);
            if (top != null)
            {
                fs.Top(top.Value);
            }
            else if (pageIndex != null && pageSize != null)
            {
                fs.Page(pageSize.Value, pageIndex.Value);
            }
            if (orderBy != null)
            {
                if (ascOrDesc.ToLower() == "asc")
                {
                    fs.OrderBy(orderBy);
                }
                else
                {
                    fs.OrderByDescending(orderBy);
                }
            }
            return fs.ToList();
        }

        
        /// <summary>
        /// 通用查询
        /// </summary>
        public static List<T> Query(Where<T> where, OrderByClip orderBy = null, string ascOrDesc = "asc", int? top = null, int? pageSize = null, int? pageIndex = null)
        {
            var fs = Db.MySqlContext.From<T>().Where(where);
            if (top != null)
            {
                fs.Top(top.Value);
            }
            else if (pageIndex != null && pageSize != null)
            {
                fs.Page(pageSize.Value, pageIndex.Value);
            }
            if (orderBy != null)
            {
                fs.OrderBy(orderBy);
            }
            return fs.ToList();
        }
        /// <summary>
        /// 通用查询
        /// </summary>
        public static T First(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy = null, string ascOrDesc = "asc", int? top = null, int? pageSize = null, int? pageIndex = null)
        {
            var fs = Db.MySqlContext.From<T>().Where(where);
            if (top != null)
            {
                fs.Top(top.Value);
            }
            else if (pageIndex != null && pageSize != null)
            {
                fs.Page(pageSize.Value, pageIndex.Value);
            }
            if (orderBy != null)
            {
                if (ascOrDesc.ToLower() == "asc")
                {
                    return fs.OrderBy(orderBy).First();
                }
                return fs.OrderByDescending(orderBy).First();
            }
            var model = fs.First();
            return model;
        }
        /// <summary>
        /// 通用查询
        /// </summary>
        public static T First(Where<T> where, Expression<Func<T, object>> orderBy = null, string ascOrDesc = "asc", int? top = null, int? pageSize = null, int? pageIndex = null)
        {
            var fs = Db.MySqlContext.From<T>().Where(where);
            if (top != null)
            {
                fs.Top(top.Value);
            }
            else if (pageIndex != null && pageSize != null)
            {
                fs.Page(pageSize.Value, pageIndex.Value);
            }
            if (orderBy != null)
            {
                if (ascOrDesc.ToLower() == "asc")
                {
                    return fs.OrderBy(orderBy).First();
                }
                return fs.OrderByDescending(orderBy).First();
            }
            return fs.First();
        }
        /// <summary>
        /// 根据条件判断是否存在数据
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static bool Any(Expression<Func<T, bool>> where)
        {
            return Db.MySqlContext.Exists<T>(where);
        }
        /// <summary>
        /// 取总数
        /// </summary>
        public static int Count(Expression<Func<T, bool>> where)
        {
            return Db.MySqlContext.From<T>().Where(where).Count();
        }
        /// <summary>
        /// 取总数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static int Count(Where<T> where)
        {
            return Db.MySqlContext.From<T>().Where(where).Count();
        }
        #endregion
        #region 插入
        /// <summary>
        /// 插入单个实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static int Insert(T entity)
        {
            return Db.MySqlContext.Insert<T>(entity);
        }
        /// <summary>
        /// 插入单个实体
        /// </summary>
        /// <param name="context"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static void Insert(DbTrans context, T entity)
        {
            Db.MySqlContext.Insert<T>(context, entity);
            //context.Set<T>().Add(entity);
        }
        /// <summary>
        /// 插入多个实体
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static int Insert(IEnumerable<T> entities)
        {
            return Db.MySqlContext.Insert<T>(entities);
        }
        public static void Insert(DbTrans context, IEnumerable<T> entities)
        {
            Db.MySqlContext.Insert<T>(context, entities.ToArray());
        }
        #endregion
        #region 更新
        /// <summary>
        /// 更新单个实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static int Update(T entity)
        {
            return Db.MySqlContext.Update(entity);
        }
        /// <summary>
        /// 更新单个实体
        /// </summary>
        public static int Update(T entity, Where where)
        {
            return Db.MySqlContext.Update(entity, where);
        }
        /// <summary>
        /// 更新单个实体
        /// </summary>
        public static int Update(T entity, Expression<Func<T, bool>> lambdaWhere)
        {
            return Db.MySqlContext.Update(entity, lambdaWhere);
        }
        public static void Update(DbTrans context, T entity)
        {
            Db.MySqlContext.Update(context, entity);
        }
        /// <summary>
        /// 更新多个实体
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static int Update(IEnumerable<T> entities)
        {
            var enumerable = entities as T[] ?? entities.ToArray();
            Db.MySqlContext.Update(enumerable.ToArray());
            return 1;
        }
        public static void Update(DbTrans context, IEnumerable<T> entities)
        {
            Db.MySqlContext.Update(context, entities.ToArray());
        }
        #endregion
        #region 删除
        /// <summary>
        /// 删除单个实体
        /// </summary>
        public static int Delete(T entitie)
        {
            return Db.MySqlContext.Delete<T>(entitie);
        }
        /// <summary>
        /// 删除多个实体
        /// </summary>
        public static int Delete(IEnumerable<T> entities)
        {
            return Db.MySqlContext.Delete<T>(entities);
        }
        /// <summary>
        /// 删除单个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int Delete(int? id)
        {
            if (id == null)
            {
                return 0;
            }
            return Db.MySqlContext.Delete<T>(id.Value);
        }
        /// <summary>
        /// 删除单个实体
        /// </summary>
        public static int Delete(Expression<Func<T, bool>> where)
        {
            return Db.MySqlContext.Delete<T>(where);
        }
        /// <summary>
        /// 删除单个实体
        /// </summary>
        public static int Delete(Where<T> where)
        {
            return Db.MySqlContext.Delete<T>(where.ToWhereClip());
        }
        #endregion
    }
}
