using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Zy.Common;

namespace YiDaBus.Com.Manager.Common
{
    /// <summary>  
    /// 实体转换辅助类  
    /// </summary>  
    public class ModelConvertHelper<T> 
    {
        /// <summary>
        /// 参数可以不填，但填写了必须不能为""
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string ValidModelIsEmpty(T model)
        {
            string r = string.Empty;
            Type t = typeof(T);
            PropertyInfo[] properties = t.GetProperties();
            foreach (PropertyInfo i in properties)
            {
                object o = i.GetValue(model);
                if (i.PropertyType == typeof(int) && (int)o <= 0)
                {
                    return ((DescriptionAttribute)i.GetCustomAttributes(typeof(DescriptionAttribute), true)[0]).Description + "必须大于0";
                }
                if (i.PropertyType == typeof(string) && (string)o != null && (string)o == "")
                {
                    return ((DescriptionAttribute)i.GetCustomAttributes(typeof(DescriptionAttribute), true)[0]).Description + "不能为空";
                }
            }
            return r;
        }

        /// <summary>
        /// 参数必须填写，且不能为空
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string ValidModelIsNullOrEmpty(T model)
        {
            string r = string.Empty;
            Type t = typeof(T);
            PropertyInfo[] properties = t.GetProperties();
            foreach (PropertyInfo i in properties)
            {
                object o = i.GetValue(model);
                if (i.PropertyType == typeof(int) && (int)o <= 0)
                {
                    return ((DescriptionAttribute)i.GetCustomAttributes(typeof(DescriptionAttribute), true)[0]).Description + "必须大于0";
                }
                if (i.PropertyType == typeof(string) && string.IsNullOrEmpty((string)o))
                {
                    return ((DescriptionAttribute)i.GetCustomAttributes(typeof(DescriptionAttribute), true)[0]).Description + "不能为空";
                }
            }
            return r;
        }
        /// <summary>
        /// 参数必须填写，且不能为空
        /// </summary>
        /// <param name="model">模型</param>
        /// <param name="param">不需要判断的属性</param>
        /// <returns></returns>
        public static string ValidModelIsNullOrEmpty(T model, params string[] param)
        {
            string r = string.Empty;
            Type t = typeof(T);
            PropertyInfo[] properties = t.GetProperties();
            foreach (PropertyInfo i in properties)
            {
                object o = i.GetValue(model);
                if (i.PropertyType == typeof(int) && (int)o <= 0)
                {
                    if (!param.Contains<string>(i.Name))
                        return ((DescriptionAttribute)i.GetCustomAttributes(typeof(DescriptionAttribute), true)[0]).Description + "必须大于0";
                }
                if (i.PropertyType == typeof(string) && string.IsNullOrEmpty((string)o))
                {
                    if (!param.Contains<string>(i.Name))
                        return ((DescriptionAttribute)i.GetCustomAttributes(typeof(DescriptionAttribute), true)[0]).Description + "不能为空";
                }
            }
            return r;
        }

        public static void SetEmptyWhenNull(T model)
        {
            Type t = typeof(T);
            PropertyInfo[] properties = t.GetProperties();
            foreach (PropertyInfo i in properties)
            {
                object o = i.GetValue(model);
                if (i.PropertyType == typeof(int?) && (int?)o == null)
                {
                    i.SetValue(model, 0);
                }
                if (i.PropertyType == typeof(string) && string.IsNullOrEmpty((string)o))
                {
                    i.SetValue(model, string.Empty);
                }
            }
        }
    }

    public class ModelHelper
    {
       
        /// <summary>
        /// 将model1中不为null的值赋值到model2中
        /// </summary>
        /// <param name="model1"></param>
        /// <param name="model2"></param>
        public static void SetValueByModel<T1, T2>(T1 model1, T2 model2)
        {
            Type t1 = typeof(T1);
            PropertyInfo[] properties1 = t1.GetProperties();

            Type t2 = typeof(T2);
            PropertyInfo[] properties2 = t2.GetProperties();
            foreach (PropertyInfo i in properties1)
            {
                object o = i.GetValue(model1);
                if (o != null && o != DBNull.Value)
                {
                    t1.GetProperty(i.Name).SetValue(model2, o);
                }
            }
        }
        
    }
}
