/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: NFine
 * Description: 易达巴士后台管理系统
 * Website：http://www.nfine.cn
*********************************************************************************/
namespace YiDaBus.Com.Manager.Common
{
    /// <summary>
    /// MD5加密
    /// </summary>
    public class Md5
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str">加密字符</param>
        /// <param name="code">加密位数16/32</param>
        /// <returns></returns>
        public static string md5(string str, int code = 16)
        {
            string strEncrypt = string.Empty;
            try
            {
                if (code == 16)
                {
                    strEncrypt = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").Substring(8, 16);
                }

                if (code == 32)
                {
                    strEncrypt = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
                }
            }
            catch { }
            return strEncrypt;
        }
    }
}
