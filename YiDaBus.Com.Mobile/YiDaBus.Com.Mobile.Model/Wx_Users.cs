using System;	
using System.IO;
using System.Data;
using System.Data.Common;
using Dos.ORM;
using Dos.ORM.Common;

namespace YiDaBus.Com.Model
{


    [Serializable]
	public partial class Wx_Users : Entity
    {

		public Wx_Users():base("Wx_Users") {}

	    #region Field
		
        private int _Id = int.MinValue;
	    /// <summary>
        /// 用户ID
        /// </summary>
        public int Id
        {
            get { return _Id; }
            set
            {
                this.OnPropertyValueChange(_.Id, _Id, value);
                this._Id = value;
                
            }
        }
        private string _UserName = string.Empty;
	    /// <summary>
        /// 用户账号
        /// </summary>
        public string UserName
        {
            get { return _UserName; }
            set
            {
                this.OnPropertyValueChange(_.UserName, _UserName, value);
                this._UserName = value;
                
            }
        }
        private string _UserNickName = string.Empty;
	    /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserNickName
        {
            get { return _UserNickName; }
            set
            {
                this.OnPropertyValueChange(_.UserNickName, _UserNickName, value);
                this._UserNickName = value;
                
            }
        }
        private string _Mobile = string.Empty;
	    /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile
        {
            get { return _Mobile; }
            set
            {
                this.OnPropertyValueChange(_.Mobile, _Mobile, value);
                this._Mobile = value;
                
            }
        }
        private string _Gender = string.Empty;
	    /// <summary>
        /// 性别：帅哥；美女；
        /// </summary>
        public string Gender
        {
            get { return _Gender; }
            set
            {
                this.OnPropertyValueChange(_.Gender, _Gender, value);
                this._Gender = value;
                
            }
        }
        private string _OpenId = string.Empty;
	    /// <summary>
        /// 微信用户唯一标识
        /// </summary>
        public string OpenId
        {
            get { return _OpenId; }
            set
            {
                this.OnPropertyValueChange(_.OpenId, _OpenId, value);
                this._OpenId = value;
                
            }
        }
        private int _IsDel = 0;
	    /// <summary>
        /// 是否已删除（0：未删除；1：已删除；）
        /// </summary>
        public int IsDel
        {
            get { return _IsDel; }
            set
            {
                this.OnPropertyValueChange(_.IsDel, _IsDel, value);
                this._IsDel = value;
                
            }
        }
        private DateTime? _CreateTime = DateTime.MinValue;
	    /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime
        {
            get { return _CreateTime; }
            set
            {
                this.OnPropertyValueChange(_.CreateTime, _CreateTime, value);
                this._CreateTime = value;
                
            }
        }
        private DateTime? _UpdateTime = DateTime.MinValue;
	    /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime
        {
            get { return _UpdateTime; }
            set
            {
                this.OnPropertyValueChange(_.UpdateTime, _UpdateTime, value);
                this._UpdateTime = value;
                
            }
        }
        private string _WxNickName = string.Empty;
	    /// <summary>
        /// 用户微信昵称
        /// </summary>
        public string WxNickName
        {
            get { return _WxNickName; }
            set
            {
                this.OnPropertyValueChange(_.WxNickName, _WxNickName, value);
                this._WxNickName = value;
                
            }
        }
		#endregion

		#region Method
		        /// <summary>
        /// 获取实体中的标识列
        /// </summary>
        public override Field GetIdentityField()
        {
            return _.Id;
        }
				        /// <summary>
        /// 获取实体中的主键列
        /// </summary>
        public override Field[] GetPrimaryKeyFields()
        {
            return new Field[] {_.Id };
        }
				 /// <summary>
        /// 获取列信息
        /// </summary>
        public override Field[] GetFields()
        {
            return new Field[] {_.Id,_.UserName,_.UserNickName,_.Mobile,_.Gender,_.OpenId,_.IsDel,_.CreateTime,_.UpdateTime,_.WxNickName };
        }

        /// <summary>
        /// 获取值信息
        /// </summary>
        public override object[] GetValues()
        {
            return new object[] {this._Id,this._UserName,this._UserNickName,this._Mobile,this._Gender,this._OpenId,this._IsDel,this._CreateTime,this._UpdateTime,this._WxNickName };
        }
		#endregion
		
		#region _
        /// <summary>
        /// 字段信息
        /// </summary>
        public class _
        {
            /// <summary>
            /// *
            /// </summary>
            public readonly static Field All = new Field("*", "Wx_Users");

			/// <summary>
            /// 用户ID
            /// </summary>
            public readonly static Field Id = new Field("Id", "Wx_Users", "用户ID");

			/// <summary>
            /// 用户账号
            /// </summary>
            public readonly static Field UserName = new Field("UserName", "Wx_Users", "用户账号");

			/// <summary>
            /// 用户姓名
            /// </summary>
            public readonly static Field UserNickName = new Field("UserNickName", "Wx_Users", "用户姓名");

			/// <summary>
            /// 手机号
            /// </summary>
            public readonly static Field Mobile = new Field("Mobile", "Wx_Users", "手机号");

			/// <summary>
            /// 性别：帅哥；美女；
            /// </summary>
            public readonly static Field Gender = new Field("Gender", "Wx_Users", "性别：帅哥；美女；");

			/// <summary>
            /// 微信用户唯一标识
            /// </summary>
            public readonly static Field OpenId = new Field("OpenId", "Wx_Users", "微信用户唯一标识");

			/// <summary>
            /// 是否已删除（0：未删除；1：已删除；）
            /// </summary>
            public readonly static Field IsDel = new Field("IsDel", "Wx_Users", "是否已删除（0：未删除；1：已删除；）");

			/// <summary>
            /// 创建时间
            /// </summary>
            public readonly static Field CreateTime = new Field("CreateTime", "Wx_Users", "创建时间");

			/// <summary>
            /// 更新时间
            /// </summary>
            public readonly static Field UpdateTime = new Field("UpdateTime", "Wx_Users", "更新时间");

			/// <summary>
            /// 用户微信昵称
            /// </summary>
            public readonly static Field WxNickName = new Field("WxNickName", "Wx_Users", "用户微信昵称");

			
        }
		#endregion
        
    }

}
