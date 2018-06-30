using System;	
using System.IO;
using System.Data;
using System.Data.Common;
using Dos.ORM;
using Dos.ORM.Common;

namespace YiDaBus.Com.Model
{


    [Serializable]
	public partial class Sys_Filterip : Entity
    {

		public Sys_Filterip():base("Sys_Filterip") {}

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
        private int _F_Type = int.MinValue;
	    /// <summary>
        /// 
        /// </summary>
        public int F_Type
        {
            get { return _F_Type; }
            set
            {
                this.OnPropertyValueChange(_.F_Type, _F_Type, value);
                this._F_Type = value;
                
            }
        }
        private string _F_StartIP = string.Empty;
	    /// <summary>
        /// 
        /// </summary>
        public string F_StartIP
        {
            get { return _F_StartIP; }
            set
            {
                this.OnPropertyValueChange(_.F_StartIP, _F_StartIP, value);
                this._F_StartIP = value;
                
            }
        }
        private string _F_EndIP = string.Empty;
	    /// <summary>
        /// 
        /// </summary>
        public string F_EndIP
        {
            get { return _F_EndIP; }
            set
            {
                this.OnPropertyValueChange(_.F_EndIP, _F_EndIP, value);
                this._F_EndIP = value;
                
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
            return _.F_CreatorTime;
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
            return new Field[] {_.F_Id,_.F_Type,_.F_StartIP,_.F_EndIP,_.F_SortCode,_.F_DeleteMark,_.F_EnabledMark,_.F_Description,_.F_CreatorTime,_.F_CreatorUserId,_.F_LastModifyTime,_.F_LastModifyUserId,_.F_DeleteTime,_.F_DeleteUserId };
        }

        /// <summary>
        /// 获取值信息
        /// </summary>
        public override object[] GetValues()
        {
            return new object[] {this._F_Id,this._F_Type,this._F_StartIP,this._F_EndIP,this._F_SortCode,this._F_DeleteMark,this._F_EnabledMark,this._F_Description,this._F_CreatorTime,this._F_CreatorUserId,this._F_LastModifyTime,this._F_LastModifyUserId,this._F_DeleteTime,this._F_DeleteUserId };
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
            public readonly static Field All = new Field("*", "Sys_Filterip");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_Id = new Field("F_Id", "Sys_Filterip", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_Type = new Field("F_Type", "Sys_Filterip", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_StartIP = new Field("F_StartIP", "Sys_Filterip", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_EndIP = new Field("F_EndIP", "Sys_Filterip", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_SortCode = new Field("F_SortCode", "Sys_Filterip", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_DeleteMark = new Field("F_DeleteMark", "Sys_Filterip", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_EnabledMark = new Field("F_EnabledMark", "Sys_Filterip", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_Description = new Field("F_Description", "Sys_Filterip", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_CreatorTime = new Field("F_CreatorTime", "Sys_Filterip", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_CreatorUserId = new Field("F_CreatorUserId", "Sys_Filterip", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_LastModifyTime = new Field("F_LastModifyTime", "Sys_Filterip", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_LastModifyUserId = new Field("F_LastModifyUserId", "Sys_Filterip", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_DeleteTime = new Field("F_DeleteTime", "Sys_Filterip", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_DeleteUserId = new Field("F_DeleteUserId", "Sys_Filterip", "");

			
        }
		#endregion
        
    }

}
