using Dos.Common;
using Dos.ORM;

namespace YiDaBus.Com.Dal.Base
{
    public class Db
    {
        public static readonly DbSession MySqlContext = new DbSession("YiDaBusDbContext");
        //public static readonly DbSession Context = new DbSession("SqliteConn");
        //public static readonly DbSession Context = new DbSession("AccessConn");
        //public static readonly DbSession Context = new DbSession("OracleConn");
        //public static readonly DbSession Context = new DbSession("PostgreSqlConn");
        static Db()
        {
           
            #region mysql
            MySqlContext.RegisterSqlLogger(delegate (string sql)
            {
                //在此可以记录sql日志
                //写日志会影响性能，建议开发版本记录sql以便调试，发布正式版本不要记录
                //LogHelper.Debug(sql, "MySQL日志");
            });
            #endregion
        }
    }
}
