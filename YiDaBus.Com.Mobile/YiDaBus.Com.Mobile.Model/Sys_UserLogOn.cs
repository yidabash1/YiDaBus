using System;	
using System.IO;
using System.Data;
using System.Data.Common;
using Dos.ORM;
using Dos.ORM.Common;

namespace YiDaBus.Com.Model
{


    [Serializable]
	public partial class Sys_UserLogOn : Entity
    {

		public Sys_UserLogOn():base("Sys_UserLogOn") {}

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
        private string _F_UserId = string.Empty;
	    /// <summary>
        /// 
        /// </summary>
        public string F_UserId
        {
            get { return _F_UserId; }
            set
            {
                this.OnPropertyValueChange(_.F_UserId, _F_UserId, value);
                this._F_UserId = value;
                
            }
        }
        private string _F_UserPassword = string.Empty;
	    /// <summary>
        /// 
        /// </summary>
        public string F_UserPassword
        {
            get { return _F_UserPassword; }
            set
            {
                this.OnPropertyValueChange(_.F_UserPassword, _F_UserPassword, value);
                this._F_UserPassword = value;
                
            }
        }
        private string _F_UserSecretkey = string.Empty;
	    /// <summary>
        /// 
        /// </summary>
        public string F_UserSecretkey
        {
            get { return _F_UserSecretkey; }
            set
            {
                this.OnPropertyValueChange(_.F_UserSecretkey, _F_UserSecretkey, value);
                this._F_UserSecretkey = value;
                
            }
        }
        private DateTime _F_AllowStartTime = DateTime.MinValue;
	    /// <summary>
        /// 
        /// </summary>
        public DateTime F_AllowStartTime
        {
            get { return _F_AllowStartTime; }
            set
            {
                this.OnPropertyValueChange(_.F_AllowStartTime, _F_AllowStartTime, value);
                this._F_AllowStartTime = value;
                
            }
        }
        private DateTime _F_AllowEndTime = DateTime.MinValue;
	    /// <summary>
        /// 
        /// </summary>
        public DateTime F_AllowEndTime
        {
            get { return _F_AllowEndTime; }
            set
            {
                this.OnPropertyValueChange(_.F_AllowEndTime, _F_AllowEndTime, value);
                this._F_AllowEndTime = value;
                
            }
        }
        private DateTime _F_LockStartDate = DateTime.MinValue;
	    /// <summary>
        /// 
        /// </summary>
        public DateTime F_LockStartDate
        {
            get { return _F_LockStartDate; }
            set
            {
                this.OnPropertyValueChange(_.F_LockStartDate, _F_LockStartDate, value);
                this._F_LockStartDate = value;
                
            }
        }
        private DateTime _F_LockEndDate = DateTime.MinValue;
	    /// <summary>
        /// 
        /// </summary>
        public DateTime F_LockEndDate
        {
            get { return _F_LockEndDate; }
            set
            {
                this.OnPropertyValueChange(_.F_LockEndDate, _F_LockEndDate, value);
                this._F_LockEndDate = value;
                
            }
        }
        private DateTime _F_FirstVisitTime = DateTime.MinValue;
	    /// <summary>
        /// 
        /// </summary>
        public DateTime F_FirstVisitTime
        {
            get { return _F_FirstVisitTime; }
            set
            {
                this.OnPropertyValueChange(_.F_FirstVisitTime, _F_FirstVisitTime, value);
                this._F_FirstVisitTime = value;
                
            }
        }
        private DateTime _F_PreviousVisitTime = DateTime.MinValue;
	    /// <summary>
        /// 
        /// </summary>
        public DateTime F_PreviousVisitTime
        {
            get { return _F_PreviousVisitTime; }
            set
            {
                this.OnPropertyValueChange(_.F_PreviousVisitTime, _F_PreviousVisitTime, value);
                this._F_PreviousVisitTime = value;
                
            }
        }
        private DateTime _F_LastVisitTime = DateTime.MinValue;
	    /// <summary>
        /// 
        /// </summary>
        public DateTime F_LastVisitTime
        {
            get { return _F_LastVisitTime; }
            set
            {
                this.OnPropertyValueChange(_.F_LastVisitTime, _F_LastVisitTime, value);
                this._F_LastVisitTime = value;
                
            }
        }
        private DateTime _F_ChangePasswordDate = DateTime.MinValue;
	    /// <summary>
        /// 
        /// </summary>
        public DateTime F_ChangePasswordDate
        {
            get { return _F_ChangePasswordDate; }
            set
            {
                this.OnPropertyValueChange(_.F_ChangePasswordDate, _F_ChangePasswordDate, value);
                this._F_ChangePasswordDate = value;
                
            }
        }
        private int _F_MultiUserLogin = int.MinValue;
	    /// <summary>
        /// 
        /// </summary>
        public int F_MultiUserLogin
        {
            get { return _F_MultiUserLogin; }
            set
            {
                this.OnPropertyValueChange(_.F_MultiUserLogin, _F_MultiUserLogin, value);
                this._F_MultiUserLogin = value;
                
            }
        }
        private int? _F_LogOnCount = int.MinValue;
	    /// <summary>
        /// 
        /// </summary>
        public int? F_LogOnCount
        {
            get { return _F_LogOnCount; }
            set
            {
                this.OnPropertyValueChange(_.F_LogOnCount, _F_LogOnCount, value);
                this._F_LogOnCount = value;
                
            }
        }
        private int _F_UserOnLine = int.MinValue;
	    /// <summary>
        /// 
        /// </summary>
        public int F_UserOnLine
        {
            get { return _F_UserOnLine; }
            set
            {
                this.OnPropertyValueChange(_.F_UserOnLine, _F_UserOnLine, value);
                this._F_UserOnLine = value;
                
            }
        }
        private string _F_Question = string.Empty;
	    /// <summary>
        /// 
        /// </summary>
        public string F_Question
        {
            get { return _F_Question; }
            set
            {
                this.OnPropertyValueChange(_.F_Question, _F_Question, value);
                this._F_Question = value;
                
            }
        }
        private string _F_AnswerQuestion = string.Empty;
	    /// <summary>
        /// 
        /// </summary>
        public string F_AnswerQuestion
        {
            get { return _F_AnswerQuestion; }
            set
            {
                this.OnPropertyValueChange(_.F_AnswerQuestion, _F_AnswerQuestion, value);
                this._F_AnswerQuestion = value;
                
            }
        }
        private int _F_CheckIPAddress = int.MinValue;
	    /// <summary>
        /// 
        /// </summary>
        public int F_CheckIPAddress
        {
            get { return _F_CheckIPAddress; }
            set
            {
                this.OnPropertyValueChange(_.F_CheckIPAddress, _F_CheckIPAddress, value);
                this._F_CheckIPAddress = value;
                
            }
        }
        private string _F_Language = string.Empty;
	    /// <summary>
        /// 
        /// </summary>
        public string F_Language
        {
            get { return _F_Language; }
            set
            {
                this.OnPropertyValueChange(_.F_Language, _F_Language, value);
                this._F_Language = value;
                
            }
        }
        private string _F_Theme = string.Empty;
	    /// <summary>
        /// 
        /// </summary>
        public string F_Theme
        {
            get { return _F_Theme; }
            set
            {
                this.OnPropertyValueChange(_.F_Theme, _F_Theme, value);
                this._F_Theme = value;
                
            }
        }
		#endregion

		#region Method
		        /// <summary>
        /// 获取实体中的标识列
        /// </summary>
        public override Field GetIdentityField()
        {
            return _.F_AllowStartTime;
        }
				        /// <summary>
        /// 获取实体中的主键列
        /// </summary>
        public override Field[] GetPrimaryKeyFields()
        {
            return new Field[] {_.F_Id };
        }
				 /// <summary>
        /// 获取列信息
        /// </summary>
        public override Field[] GetFields()
        {
            return new Field[] {_.F_Id,_.F_UserId,_.F_UserPassword,_.F_UserSecretkey,_.F_AllowStartTime,_.F_AllowEndTime,_.F_LockStartDate,_.F_LockEndDate,_.F_FirstVisitTime,_.F_PreviousVisitTime,_.F_LastVisitTime,_.F_ChangePasswordDate,_.F_MultiUserLogin,_.F_LogOnCount,_.F_UserOnLine,_.F_Question,_.F_AnswerQuestion,_.F_CheckIPAddress,_.F_Language,_.F_Theme };
        }

        /// <summary>
        /// 获取值信息
        /// </summary>
        public override object[] GetValues()
        {
            return new object[] {this._F_Id,this._F_UserId,this._F_UserPassword,this._F_UserSecretkey,this._F_AllowStartTime,this._F_AllowEndTime,this._F_LockStartDate,this._F_LockEndDate,this._F_FirstVisitTime,this._F_PreviousVisitTime,this._F_LastVisitTime,this._F_ChangePasswordDate,this._F_MultiUserLogin,this._F_LogOnCount,this._F_UserOnLine,this._F_Question,this._F_AnswerQuestion,this._F_CheckIPAddress,this._F_Language,this._F_Theme };
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
            public readonly static Field All = new Field("*", "Sys_UserLogOn");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_Id = new Field("F_Id", "Sys_UserLogOn", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_UserId = new Field("F_UserId", "Sys_UserLogOn", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_UserPassword = new Field("F_UserPassword", "Sys_UserLogOn", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_UserSecretkey = new Field("F_UserSecretkey", "Sys_UserLogOn", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_AllowStartTime = new Field("F_AllowStartTime", "Sys_UserLogOn", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_AllowEndTime = new Field("F_AllowEndTime", "Sys_UserLogOn", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_LockStartDate = new Field("F_LockStartDate", "Sys_UserLogOn", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_LockEndDate = new Field("F_LockEndDate", "Sys_UserLogOn", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_FirstVisitTime = new Field("F_FirstVisitTime", "Sys_UserLogOn", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_PreviousVisitTime = new Field("F_PreviousVisitTime", "Sys_UserLogOn", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_LastVisitTime = new Field("F_LastVisitTime", "Sys_UserLogOn", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_ChangePasswordDate = new Field("F_ChangePasswordDate", "Sys_UserLogOn", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_MultiUserLogin = new Field("F_MultiUserLogin", "Sys_UserLogOn", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_LogOnCount = new Field("F_LogOnCount", "Sys_UserLogOn", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_UserOnLine = new Field("F_UserOnLine", "Sys_UserLogOn", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_Question = new Field("F_Question", "Sys_UserLogOn", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_AnswerQuestion = new Field("F_AnswerQuestion", "Sys_UserLogOn", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_CheckIPAddress = new Field("F_CheckIPAddress", "Sys_UserLogOn", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_Language = new Field("F_Language", "Sys_UserLogOn", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_Theme = new Field("F_Theme", "Sys_UserLogOn", "");

			
        }
		#endregion
        
    }

}
