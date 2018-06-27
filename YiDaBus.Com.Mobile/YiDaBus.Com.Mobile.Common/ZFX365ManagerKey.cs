using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiDaBus.Com.Manager.Common
{
    public class ZFX365ManagerKey
    {
        ///Session信息
        /// <summary>
        /// 后台管理员
        /// </summary>
        public const string SESSION_ADMIN_INFO = "session_admin_info";
        /// <summary>
        /// 会员用户
        /// </summary>
        public const string SESSION_USER_INFO = "session_user_info";
        /// <summary>
        /// 企业用户
        /// </summary>
        public const string SESSION_COUSER_INFO = "session_couser_info";
        ///Cookie信息
        /// <summary>
        /// 后台管理员登录model
        /// </summary>
        public const string COOKIE_ADMIN_INFO = "cookie_admin_info";
        /// <summary>
        /// 后台管理员登录model
        /// </summary>
        public const string COOKIE_ADMIN_INFO_NAME = "cookie_admin_info_name";

        ///Cookie信息
        /// <summary>
        /// 前台会员登录model
        /// </summary>
        public const string COOKIE_USER_INFO = "cookie_user_info";
        /// <summary>
        /// 前台会员登录model
        /// </summary>
        public const string COOKIE_USER_INFO_NAME = "cookie_user_info_name";
        /// <summary>
        /// 前台验证码
        /// </summary>
        public const string SESSION_USER_VALID_CODE = "session_user_valid_code";
        /// <summary>
        /// 前台验证码获取时间
        /// </summary>
        public const string COOKIE_USER_VALID_TIME = "cookie_user_valid_time";
        /// <summary>
        /// 站点配置
        /// </summary>
        public const string CACHE_SITE_CONFIG = "cache_site_config";
        /// <summary>
        /// 站点配置文件名
        /// </summary>
        public const string FILE_SITE_XML_CONFING = "Configpath";
        /// <summary>
        /// 请求ID
        /// </summary>
        public const string REQ_ID = "Configpath";
        /// <summary>
        /// 接口域名
        /// </summary>
        public const string INTERFACE_DOMAIN = "interfaceDomain";
        
    }
}
