using System;	
using System.IO;
using System.Data;
using System.Data.Common;
using Dos.ORM;
using Dos.ORM.Common;

namespace YiDaBus.Com.Model
{


    [Serializable]
	public partial class Sys_User : Entity
    {

		public Sys_User():base("Sys_User") {}

	    #region Field
		
        private string _F_Id = string.Empty;
	    /// <summary>
        /// 
        /// </summary>
        public string F_Id
        {
            get { return _F_Id; }
            set
            {
                this.OnPropertyValueChange(_.F_Id, _F_Id, value);
                this._F_Id = value;
                
            }
        }
        private string _F_Account = string.Empty;
	    /// <summary>
        /// 
        /// </summary>
        public string F_Account
        {
            get { return _F_Account; }
            set
            {
                this.OnPropertyValueChange(_.F_Account, _F_Account, value);
                this._F_Account = value;
                
            }
        }
        private string _F_RealName = string.Empty;
	    /// <summary>
        /// 
        /// </summary>
        public string F_RealName
        {
            get { return _F_RealName; }
            set
            {
                this.OnPropertyValueChange(_.F_RealName, _F_RealName, value);
                this._F_RealName = value;
                
            }
        }
        private string _F_NickName = string.Empty;
	    /// <summary>
        /// 
        /// </summary>
        public string F_NickName
        {
            get { return _F_NickName; }
            set
            {
                this.OnPropertyValueChange(_.F_NickName, _F_NickName, value);
                this._F_NickName = value;
                
            }
        }
        private string _F_HeadIcon = string.Empty;
	    /// <summary>
        /// 
        /// </summary>
        public string F_HeadIcon
        {
            get { return _F_HeadIcon; }
            set
            {
                this.OnPropertyValueChange(_.F_HeadIcon, _F_HeadIcon, value);
                this._F_HeadIcon = value;
                
            }
        }
        private int _F_Gender = int.MinValue;
	    /// <summary>
        /// 
        /// </summary>
        public int F_Gender
        {
            get { return _F_Gender; }
            set
            {
                this.OnPropertyValueChange(_.F_Gender, _F_Gender, value);
                this._F_Gender = value;
                
            }
        }
        private DateTime _F_Birthday = DateTime.MinValue;
	    /// <summary>
        /// 
        /// </summary>
        public DateTime F_Birthday
        {
            get { return _F_Birthday; }
            set
            {
                this.OnPropertyValueChange(_.F_Birthday, _F_Birthday, value);
                this._F_Birthday = value;
                
            }
        }
        private string _F_MobilePhone = string.Empty;
	    /// <summary>
        /// 
        /// </summary>
        public string F_MobilePhone
        {
            get { return _F_MobilePhone; }
            set
            {
                this.OnPropertyValueChange(_.F_MobilePhone, _F_MobilePhone, value);
                this._F_MobilePhone = value;
                
            }
        }
        private string _F_Email = string.Empty;
	    /// <summary>
        /// 
        /// </summary>
        public string F_Email
        {
            get { return _F_Email; }
            set
            {
                this.OnPropertyValueChange(_.F_Email, _F_Email, value);
                this._F_Email = value;
                
            }
        }
        private string _F_WeChat = string.Empty;
	    /// <summary>
        /// 
        /// </summary>
        public string F_WeChat
        {
            get { return _F_WeChat; }
            set
            {
                this.OnPropertyValueChange(_.F_WeChat, _F_WeChat, value);
                this._F_WeChat = value;
                
            }
        }
        private string _F_ManagerId = string.Empty;
	    /// <summary>
        /// 
        /// </summary>
        public string F_ManagerId
        {
            get { return _F_ManagerId; }
            set
            {
                this.OnPropertyValueChange(_.F_ManagerId, _F_ManagerId, value);
                this._F_ManagerId = value;
                
            }
        }
        private int? _F_SecurityLevel = int.MinValue;
	    /// <summary>
        /// 
        /// </summary>
        public int? F_SecurityLevel
        {
            get { return _F_SecurityLevel; }
            set
            {
                this.OnPropertyValueChange(_.F_SecurityLevel, _F_SecurityLevel, value);
                this._F_SecurityLevel = value;
                
            }
        }
        private string _F_Signature = string.Empty;
	    /// <summary>
        /// 
        /// </summary>
        public string F_Signature
        {
            get { return _F_Signature; }
            set
            {
                this.OnPropertyValueChange(_.F_Signature, _F_Signature, value);
                this._F_Signature = value;
                
            }
        }
        private string _F_OrganizeId = string.Empty;
	    /// <summary>
        /// 
        /// </summary>
        public string F_OrganizeId
        {
            get { return _F_OrganizeId; }
            set
            {
                this.OnPropertyValueChange(_.F_OrganizeId, _F_OrganizeId, value);
                this._F_OrganizeId = value;
                
            }
        }
        private string _F_DepartmentId = string.Empty;
	    /// <summary>
        /// 
        /// </summary>
        public string F_DepartmentId
        {
            get { return _F_DepartmentId; }
            set
            {
                this.OnPropertyValueChange(_.F_DepartmentId, _F_DepartmentId, value);
                this._F_DepartmentId = value;
                
            }
        }
        private string _F_RoleId = string.Empty;
	    /// <summary>
        /// 
        /// </summary>
        public string F_RoleId
        {
            get { return _F_RoleId; }
            set
            {
                this.OnPropertyValueChange(_.F_RoleId, _F_RoleId, value);
                this._F_RoleId = value;
                
            }
        }
        private string _F_DutyId = string.Empty;
	    /// <summary>
        /// 
        /// </summary>
        public string F_DutyId
        {
            get { return _F_DutyId; }
            set
            {
                this.OnPropertyValueChange(_.F_DutyId, _F_DutyId, value);
                this._F_DutyId = value;
                
            }
        }
        private string _F_Area = string.Empty;
	    /// <summary>
        /// 系统用户所在的区域
        /// </summary>
        public string F_Area
        {
            get { return _F_Area; }
            set
            {
                this.OnPropertyValueChange(_.F_Area, _F_Area, value);
                this._F_Area = value;
                
            }
        }
        private int _F_IsAdministrator = int.MinValue;
	    /// <summary>
        /// 
        /// </summary>
        public int F_IsAdministrator
        {
            get { return _F_IsAdministrator; }
            set
            {
                this.OnPropertyValueChange(_.F_IsAdministrator, _F_IsAdministrator, value);
                this._F_IsAdministrator = value;
                
            }
        }
        private int? _F_SortCode = int.MinValue;
	    /// <summary>
        /// 
        /// </summary>
        public int? F_SortCode
        {
            get { return _F_SortCode; }
            set
            {
                this.OnPropertyValueChange(_.F_SortCode, _F_SortCode, value);
                this._F_SortCode = value;
                
            }
        }
        private int _F_DeleteMark = int.MinValue;
	    /// <summary>
        /// 
        /// </summary>
        public int F_DeleteMark
        {
            get { return _F_DeleteMark; }
            set
            {
                this.OnPropertyValueChange(_.F_DeleteMark, _F_DeleteMark, value);
                this._F_DeleteMark = value;
                
            }
        }
        private int _F_EnabledMark = int.MinValue;
	    /// <summary>
        /// 
        /// </summary>
        public int F_EnabledMark
        {
            get { return _F_EnabledMark; }
            set
            {
                this.OnPropertyValueChange(_.F_EnabledMark, _F_EnabledMark, value);
                this._F_EnabledMark = value;
                
            }
        }
        private string _F_Description = string.Empty;
	    /// <summary>
        /// 
        /// </summary>
        public string F_Description
        {
            get { return _F_Description; }
            set
            {
                this.OnPropertyValueChange(_.F_Description, _F_Description, value);
                this._F_Description = value;
                
            }
        }
        private DateTime _F_CreatorTime = DateTime.MinValue;
	    /// <summary>
        /// 
        /// </summary>
        public DateTime F_CreatorTime
        {
            get { return _F_CreatorTime; }
            set
            {
                this.OnPropertyValueChange(_.F_CreatorTime, _F_CreatorTime, value);
                this._F_CreatorTime = value;
                
            }
        }
        private string _F_CreatorUserId = string.Empty;
	    /// <summary>
        /// 
        /// </summary>
        public string F_CreatorUserId
        {
            get { return _F_CreatorUserId; }
            set
            {
                this.OnPropertyValueChange(_.F_CreatorUserId, _F_CreatorUserId, value);
                this._F_CreatorUserId = value;
                
            }
        }
        private DateTime _F_LastModifyTime = DateTime.MinValue;
	    /// <summary>
        /// 
        /// </summary>
        public DateTime F_LastModifyTime
        {
            get { return _F_LastModifyTime; }
            set
            {
                this.OnPropertyValueChange(_.F_LastModifyTime, _F_LastModifyTime, value);
                this._F_LastModifyTime = value;
                
            }
        }
        private string _F_LastModifyUserId = string.Empty;
	    /// <summary>
        /// 
        /// </summary>
        public string F_LastModifyUserId
        {
            get { return _F_LastModifyUserId; }
            set
            {
                this.OnPropertyValueChange(_.F_LastModifyUserId, _F_LastModifyUserId, value);
                this._F_LastModifyUserId = value;
                
            }
        }
        private DateTime _F_DeleteTime = DateTime.MinValue;
	    /// <summary>
        /// 
        /// </summary>
        public DateTime F_DeleteTime
        {
            get { return _F_DeleteTime; }
            set
            {
                this.OnPropertyValueChange(_.F_DeleteTime, _F_DeleteTime, value);
                this._F_DeleteTime = value;
                
            }
        }
        private string _F_DeleteUserId = string.Empty;
	    /// <summary>
        /// 
        /// </summary>
        public string F_DeleteUserId
        {
            get { return _F_DeleteUserId; }
            set
            {
                this.OnPropertyValueChange(_.F_DeleteUserId, _F_DeleteUserId, value);
                this._F_DeleteUserId = value;
                
            }
        }
		#endregion

		#region Method
		        /// <summary>
        /// 获取实体中的标识列
        /// </summary>
        public override Field GetIdentityField()
        {
            return _.F_Birthday;
        }
						 /// <summary>
        /// 获取列信息
        /// </summary>
        public override Field[] GetFields()
        {
            return new Field[] {_.F_Id,_.F_Account,_.F_RealName,_.F_NickName,_.F_HeadIcon,_.F_Gender,_.F_Birthday,_.F_MobilePhone,_.F_Email,_.F_WeChat,_.F_ManagerId,_.F_SecurityLevel,_.F_Signature,_.F_OrganizeId,_.F_DepartmentId,_.F_RoleId,_.F_DutyId,_.F_Area,_.F_IsAdministrator,_.F_SortCode,_.F_DeleteMark,_.F_EnabledMark,_.F_Description,_.F_CreatorTime,_.F_CreatorUserId,_.F_LastModifyTime,_.F_LastModifyUserId,_.F_DeleteTime,_.F_DeleteUserId };
        }

        /// <summary>
        /// 获取值信息
        /// </summary>
        public override object[] GetValues()
        {
            return new object[] {this._F_Id,this._F_Account,this._F_RealName,this._F_NickName,this._F_HeadIcon,this._F_Gender,this._F_Birthday,this._F_MobilePhone,this._F_Email,this._F_WeChat,this._F_ManagerId,this._F_SecurityLevel,this._F_Signature,this._F_OrganizeId,this._F_DepartmentId,this._F_RoleId,this._F_DutyId,this._F_Area,this._F_IsAdministrator,this._F_SortCode,this._F_DeleteMark,this._F_EnabledMark,this._F_Description,this._F_CreatorTime,this._F_CreatorUserId,this._F_LastModifyTime,this._F_LastModifyUserId,this._F_DeleteTime,this._F_DeleteUserId };
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
            public readonly static Field All = new Field("*", "Sys_User");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_Id = new Field("F_Id", "Sys_User", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_Account = new Field("F_Account", "Sys_User", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_RealName = new Field("F_RealName", "Sys_User", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_NickName = new Field("F_NickName", "Sys_User", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_HeadIcon = new Field("F_HeadIcon", "Sys_User", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_Gender = new Field("F_Gender", "Sys_User", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_Birthday = new Field("F_Birthday", "Sys_User", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_MobilePhone = new Field("F_MobilePhone", "Sys_User", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_Email = new Field("F_Email", "Sys_User", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_WeChat = new Field("F_WeChat", "Sys_User", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_ManagerId = new Field("F_ManagerId", "Sys_User", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_SecurityLevel = new Field("F_SecurityLevel", "Sys_User", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_Signature = new Field("F_Signature", "Sys_User", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_OrganizeId = new Field("F_OrganizeId", "Sys_User", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_DepartmentId = new Field("F_DepartmentId", "Sys_User", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_RoleId = new Field("F_RoleId", "Sys_User", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_DutyId = new Field("F_DutyId", "Sys_User", "");

			/// <summary>
            /// 系统用户所在的区域
            /// </summary>
            public readonly static Field F_Area = new Field("F_Area", "Sys_User", "系统用户所在的区域");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_IsAdministrator = new Field("F_IsAdministrator", "Sys_User", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_SortCode = new Field("F_SortCode", "Sys_User", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_DeleteMark = new Field("F_DeleteMark", "Sys_User", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_EnabledMark = new Field("F_EnabledMark", "Sys_User", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_Description = new Field("F_Description", "Sys_User", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_CreatorTime = new Field("F_CreatorTime", "Sys_User", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_CreatorUserId = new Field("F_CreatorUserId", "Sys_User", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_LastModifyTime = new Field("F_LastModifyTime", "Sys_User", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_LastModifyUserId = new Field("F_LastModifyUserId", "Sys_User", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_DeleteTime = new Field("F_DeleteTime", "Sys_User", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_DeleteUserId = new Field("F_DeleteUserId", "Sys_User", "");

			
        }
		#endregion
        
    }

}
