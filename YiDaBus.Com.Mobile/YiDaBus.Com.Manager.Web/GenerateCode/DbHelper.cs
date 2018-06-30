using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using System.Text;


public class MysqlDbTypeMap
{
    //暂时不知道怎么映射返回null, 根据自己的需要再自己改吧
    public static string MapDefaultType(string type, string defaultValue)
    {
        
        switch (type)
        {
            case "bigint": return string.IsNullOrEmpty(defaultValue) ? "int.MinValue" : defaultValue;
            case "bit": return string.IsNullOrEmpty(defaultValue) ? "false" : defaultValue == "b'1'" ? "true" : "false";
            case "char": return string.IsNullOrEmpty(defaultValue) ? "string.Empty" : string.Format("\"{0}\"", defaultValue);
            case "date": return "DateTime.MinValue";
            case "datetime": return "DateTime.MinValue";
            case "datetime2": return "DateTime.MinValue";
            case "datetimeoffset": return "DateTimeOffset.MinValue";
            case "decimal": return string.IsNullOrEmpty(defaultValue) ? "0" : defaultValue + "m";
            case "float": return string.IsNullOrEmpty(defaultValue) ? "double.MinValue" : defaultValue;
            case "double": return string.IsNullOrEmpty(defaultValue) ? "double.MinValue" : defaultValue;
            case "int": return string.IsNullOrEmpty(defaultValue) ? "int.MinValue" : defaultValue;
            case "nchar": return string.IsNullOrEmpty(defaultValue) ? "string.Empty" : string.Format("\"{0}\"", defaultValue);
            case "ntext": return string.IsNullOrEmpty(defaultValue) ? "string.Empty" : string.Format("\"{0}\"", defaultValue);
            case "numeric": return "0";
            case "nvarchar": return string.IsNullOrEmpty(defaultValue) ? "string.Empty" : string.Format("\"{0}\"", defaultValue);
            case "real": return string.IsNullOrEmpty(defaultValue) ? "float.MinValue" : defaultValue;
            case "smalldatetime": return "DateTime.MinValue";
            case "smallint": return string.IsNullOrEmpty(defaultValue) ? "short.MinValue" : defaultValue;
            case "smallmoney": return "0";
            case "text": return string.IsNullOrEmpty(defaultValue) ? "string.Empty" : string.Format("\"{0}\"", defaultValue);
            case "time": return string.IsNullOrEmpty(defaultValue) ? "TimeSpan.MinValue" : defaultValue;
            case "timestamp": return "null";
            case "tinyint": return string.IsNullOrEmpty(defaultValue) ? "byte.MinValue" : string.Format("\"{0}\"", defaultValue);
            case "uniqueidentifier": return string.IsNullOrEmpty(defaultValue) ? "Guid.NewGuid()" : string.Format("\"{0}\"", defaultValue);
            case "varbinary": return "null";
            case "varchar": return string.IsNullOrEmpty(defaultValue) ? "string.Empty" : string.Format("\"{0}\"", defaultValue);
            case "xml": return string.IsNullOrEmpty(defaultValue) ? "string.Empty" : string.Format("\"{0}\"", defaultValue);
            default: return "null";
        }

    }

    public static string MapCsharpType(string dbtype, bool isNull)
    {
        if (string.IsNullOrEmpty(dbtype)) return dbtype;
        dbtype = dbtype.ToLower();
        string csharpType = "object";
        switch (dbtype)
        {
            case "bigint": csharpType = isNull ? "long?" : "long"; break;
            case "binary": csharpType = "byte[]"; break;
            case "bit": csharpType = isNull ? "bool?" : "bool"; break;
            case "char": csharpType = "string"; break;
            case "date": csharpType = isNull ? "DateTime?" : "DateTime"; break;
            case "datetime": csharpType = isNull ? "DateTime?" : "DateTime"; break;
            case "datetime2": csharpType = isNull ? "DateTime?" : "DateTime"; break;
            case "datetimeoffset": csharpType = "DateTimeOffset"; break;
            case "dityint": csharpType = isNull ? "bool?" : "bool"; break;
            case "decimal": csharpType = "decimal"; break;
            case "float": csharpType = isNull ? "double?" : "double"; break;
            case "double": csharpType = isNull ? "double?" : "double"; break;
            case "image": csharpType = "byte[]"; break;
            case "int": csharpType = isNull ? "int?" : "int"; break;
            case "money": csharpType = "decimal"; break;
            case "nchar": csharpType = "string"; break;
            case "ntext": csharpType = "string"; break;
            case "numeric": csharpType = "decimal"; break;
            case "nvarchar": csharpType = "string"; break;
            case "real": csharpType = "Single"; break;
            case "smalldatetime": csharpType = isNull ? "DateTime?" : "DateTime"; break;
            case "smallint": csharpType = isNull ? "short?" : "short"; break;
            case "smallmoney": csharpType = "decimal"; break;
            case "sql_variant": csharpType = "object"; break;
            case "sysname": csharpType = "object"; break;
            case "text": csharpType = "string"; break;
            case "time": csharpType = "TimeSpan"; break;
            case "timestamp": csharpType = "byte[]"; break;
            case "tinyint": csharpType = "byte"; break;
            case "uniqueidentifier": csharpType = "Guid"; break;
            case "varbinary": csharpType = "byte[]"; break;
            case "varchar": csharpType = "string"; break;
            case "xml": csharpType = "string"; break;
            default: csharpType = "object"; break;
        }
        return csharpType;
    }

    public static Type MapCommonType(string dbtype, bool isNull)
    {
        if (string.IsNullOrEmpty(dbtype)) return Type.Missing.GetType();
        dbtype = dbtype.ToLower();
        Type commonType = typeof(object);
        switch (dbtype)
        {
            case "bigint": commonType = isNull ? typeof(long?) : typeof(long); break;
            case "binary": commonType = typeof(byte[]); break;
            case "bit": commonType = isNull ? typeof(bool?) : typeof(bool); break;
            case "char": commonType = typeof(string); break;
            case "date": commonType = isNull ? typeof(DateTime?) : typeof(DateTime); break;
            case "datetime": commonType = isNull ? typeof(DateTime?) : typeof(DateTime); break;
            case "datetime2": commonType = isNull ? typeof(DateTime?) : typeof(DateTime); break;
            case "datetimeoffset": commonType = typeof(DateTimeOffset); break;
            case "dityint": commonType = isNull ? typeof(Boolean?) : typeof(Boolean); break;
            case "decimal": commonType = typeof(decimal); break;
            case "float": commonType = isNull ? typeof(double?) : typeof(double); break;
            case "double": commonType = isNull ? typeof(double?) : typeof(double); break;
            case "image": commonType = typeof(byte[]); break;
            case "int": commonType = isNull ? typeof(int?) : typeof(int); break;
            case "money": commonType = typeof(decimal); break;
            case "nchar": commonType = typeof(string); break;
            case "ntext": commonType = typeof(string); break;
            case "numeric": commonType = typeof(decimal); break;
            case "nvarchar": commonType = typeof(string); break;
            case "real": commonType = isNull ? typeof(float?) : typeof(float); break;
            case "smalldatetime": commonType = isNull ? typeof(DateTime?) : typeof(DateTime); break;
            case "smallint": commonType = isNull ? typeof(short?) : typeof(short); break;
            case "smallmoney": commonType = typeof(decimal); break;
            case "sql_variant": commonType = typeof(object); break;
            case "sysname": commonType = typeof(object); break;
            case "text": commonType = typeof(string); break;
            case "time": commonType = typeof(TimeSpan); break;
            case "timestamp": commonType = typeof(byte[]); break;
            case "tinyint": commonType = typeof(byte); break;
            case "uniqueidentifier": commonType = typeof(Guid); break;
            case "varbinary": commonType = typeof(byte[]); break;
            case "varchar": commonType = typeof(string); break;
            case "xml": commonType = typeof(string); break;
            default: commonType = typeof(object); break;
        }
        return commonType;
    }
}

public class DbHelper
{

    #region GetDbTables
    private static DataTable GetDataTable(string connectionString, string commandText, params SqlParameter[] parms)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = commandText;
            command.Parameters.AddRange(parms);
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);

            DataTable dt = new DataTable();
            adapter.Fill(dt);

            return dt;
        }
    }

    public static List<DbTable> GetDbTables(string connectionString, string database)
    {

        #region SQL
        string sql = string.Format("SHOW TABLE STATUS FROM {0};", database);
        #endregion
        DataTable dt = GetDataTable(connectionString, sql);
        return dt.Rows.Cast<DataRow>().Select(row => new DbTable
        {
            TableName = row.Field<string>("Name"),
            Rows = row.Field<UInt64>("Rows"),
            Comment = row.Field<string>("Comment")
        }).ToList();
    }
    #endregion

    #region GetDbColumns

    public static List<DbColumn> GetDbColumns(string connectionString, string database, string tablename)
    {
        #region SQL
        string sql = string.Format("SHOW FULL COLUMNS FROM {0} FROM {1};", tablename, database);
        #endregion
        DataTable dt = GetDataTable(connectionString, sql);
        return dt.Rows.Cast<DataRow>().Select(row => new DbColumn
        {
            IsPrimaryKey = !String.IsNullOrEmpty(row.Field<string>("Key")),
            Field = row.Field<string>("Field"),
            Type = row.Field<string>("Type"),
            Comment = row.Field<string>("Comment"),
            IsNullable = row.Field<string>("NULL") == "YES",
            Default = row.Field<string>("Default")
        }).ToList();
    }

    public static List<DbColumn> GetDbColumns(string connectionString, string database, DbTable table)
    {
        string sql = string.Format("SHOW FULL COLUMNS FROM {0} FROM {1};", table.TableName, database);
        table.PrimaryKey = new List<string>();
        DataTable dt = GetDataTable(connectionString, sql);
        var list = dt.Rows.Cast<DataRow>().Select(row => new DbColumn
        {
            IsPrimaryKey = !String.IsNullOrEmpty(row.Field<string>("Key")),
            IsIdentity = !String.IsNullOrEmpty(row.Field<string>("Extra")),
            Field = row.Field<string>("Field"),
            Type = row.Field<string>("Type"),
            Comment = row.Field<string>("Comment"),
            IsNullable = row.Field<string>("NULL") == "YES",
            Default = row.Field<string>("Default")
        }).ToList();
        foreach (var column in list)
        {
            if (column.IsPrimaryKey)
            {
                table.PrimaryKey.Add(column.Field);
            }
            if (column.IsIdentity)
            {
                table.Identity = column.Field;
            }
        }
        return list;
    }

    #endregion


}

public sealed class DbTable
{
    /// <summary>
    /// 主键
    /// </summary>
    public List<string> PrimaryKey { get; set; }

    /// <summary>
    /// Dos.Orm使用
    /// </summary>
    public string PrimaryKeyString
    {
        get
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in PrimaryKey)
            {
                sb.AppendFormat("_.{0},", item);
            }
            return sb.ToString().Trim(',');
        }

    }
    /// <summary>
    /// 标识字段
    /// </summary>
    public string Identity { get; set; }

    /// <summary>
    /// 表名称
    /// </summary>
    public string TableName { get; set; }

    /// <summary>
    /// 类名
    /// </summary>
    public string ClassName { get { return TableName; } }
    /// <summary>
    /// 行数
    /// </summary>
    public UInt64 Rows { get; set; }

    /// <summary>
    /// 描述信息
    /// </summary>
    public string Comment { get; set; }

    /// <summary>
    /// 行信息
    /// </summary>
    public List<DbColumn> ColumnList { get; set; }

    /// <summary>
    /// Dos.Orm 使用
    /// </summary>
    public string DosGetFields
    {
        get
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in this.ColumnList)
            {
                sb.AppendFormat("_.{0},", item.Field);
            }
            return sb.ToString().TrimEnd(',');
        }
    }

    /// <summary>
    /// Dos.Orm使用
    /// </summary>
    public string DosGetValues
    {
        get
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in this.ColumnList)
            {
                sb.AppendFormat("this._{0},", item.Field);
            }
            return sb.ToString().TrimEnd(',');
        }
    }
}

public sealed class DbColumn
{
    /// <summary>
    /// 默认值
    /// </summary>
    public string Default { get; set; }
    /// <summary>
    /// 默认值
    /// </summary>
    public string DefaultString
    {
        get { return MysqlDbTypeMap.MapDefaultType(ColumnType, Default); }
    }
    /// <summary>
    /// 是否主键
    /// </summary>
    public bool IsPrimaryKey { get; set; }
    /// <summary>
    /// 是否标识
    /// </summary>
    public bool IsIdentity { get; set; }
    /// <summary>
    /// 字段名称
    /// </summary>
    public string Field { get; set; }
    /// <summary>
    /// 字段类型 int(11)
    /// </summary>
    public string Type { get; set; }
    /// <summary>
    /// 字段类型int
    /// </summary>
    public string ColumnType
    {
        get
        {
            return Type.IndexOf('(') == -1 ? Type : Type.Substring(0, Type.IndexOf('('));
        }
    }
    /// <summary>
    /// 数据库类型对应的C#类型
    /// </summary>
    public string CSharpType
    {
        get
        {
            return MysqlDbTypeMap.MapCsharpType(ColumnType, this.IsNullable);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public Type CommonType
    {
        get
        {
            return MysqlDbTypeMap.MapCommonType(ColumnType, this.IsNullable);
        }
    }
    /// <summary>
    /// 描述
    /// </summary>
    public string Comment { get; set; }
    /// <summary>
    /// 是否允许空
    /// </summary>
    public bool IsNullable { get; set; }

}

