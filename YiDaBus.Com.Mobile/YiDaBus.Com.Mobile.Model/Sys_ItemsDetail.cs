using System;	
using System.IO;
using System.Data;
using System.Data.Common;
using Dos.ORM;
using Dos.ORM.Common;

namespace YiDaBus.Com.Model
{


    [Serializable]
	public partial class Sys_ItemsDetail : Entity
    {

		public Sys_ItemsDetail():base("Sys_ItemsDetail") {}

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
        private string _F_ItemId = string.Empty;
	    /// <summary>
        /// 
        /// </summary>
        public string F_ItemId
        {
            get { return _F_ItemId; }
            set
            {
                this.OnPropertyValueChange(_.F_ItemId, _F_ItemId, value);
                this._F_ItemId = value;
                
            }
        }
        private string _F_ParentId = string.Empty;
	    /// <summary>
        /// 
        /// </summary>
        public string F_ParentId
        {
            get { return _F_ParentId; }
            set
            {
                this.OnPropertyValueChange(_.F_ParentId, _F_ParentId, value);
                this._F_ParentId = value;
                
            }
        }
        private string _F_ItemCode = string.Empty;
	    /// <summary>
        /// 
        /// </summary>
        public string F_ItemCode
        {
            get { return _F_ItemCode; }
            set
            {
                this.OnPropertyValueChange(_.F_ItemCode, _F_ItemCode, value);
                this._F_ItemCode = value;
                
            }
        }
        private string _F_ItemName = string.Empty;
	    /// <summary>
        /// 
        /// </summary>
        public string F_ItemName
        {
            get { return _F_ItemName; }
            set
            {
                this.OnPropertyValueChange(_.F_ItemName, _F_ItemName, value);
                this._F_ItemName = value;
                
            }
        }
        private string _F_SimpleSpelling = string.Empty;
	    /// <summary>
        /// 
        /// </summary>
        public string F_SimpleSpelling
        {
            get { return _F_SimpleSpelling; }
            set
            {
                this.OnPropertyValueChange(_.F_SimpleSpelling, _F_SimpleSpelling, value);
                this._F_SimpleSpelling = value;
                
            }
        }
        private int _F_IsDefault = int.MinValue;
	    /// <summary>
        /// 
        /// </summary>
        public int F_IsDefault
        {
            get { return _F_IsDefault; }
            set
            {
                this.OnPropertyValueChange(_.F_IsDefault, _F_IsDefault, value);
                this._F_IsDefault = value;
                
            }
        }
        private int? _F_Layers = int.MinValue;
	    /// <summary>
        /// 
        /// </summary>
        public int? F_Layers
        {
            get { return _F_Layers; }
            set
            {
                this.OnPropertyValueChange(_.F_Layers, _F_Layers, value);
                this._F_Layers = value;
                
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
        /// 获取列信息
        /// </summary>
        public override Field[] GetFields()
        {
            return new Field[] {_.F_Id,_.F_ItemId,_.F_ParentId,_.F_ItemCode,_.F_ItemName,_.F_SimpleSpelling,_.F_IsDefault,_.F_Layers,_.F_SortCode,_.F_DeleteMark,_.F_EnabledMark,_.F_Description,_.F_CreatorTime,_.F_CreatorUserId,_.F_LastModifyTime,_.F_LastModifyUserId,_.F_DeleteTime,_.F_DeleteUserId };
        }

        /// <summary>
        /// 获取值信息
        /// </summary>
        public override object[] GetValues()
        {
            return new object[] {this._F_Id,this._F_ItemId,this._F_ParentId,this._F_ItemCode,this._F_ItemName,this._F_SimpleSpelling,this._F_IsDefault,this._F_Layers,this._F_SortCode,this._F_DeleteMark,this._F_EnabledMark,this._F_Description,this._F_CreatorTime,this._F_CreatorUserId,this._F_LastModifyTime,this._F_LastModifyUserId,this._F_DeleteTime,this._F_DeleteUserId };
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
            public readonly static Field All = new Field("*", "Sys_ItemsDetail");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_Id = new Field("F_Id", "Sys_ItemsDetail", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_ItemId = new Field("F_ItemId", "Sys_ItemsDetail", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_ParentId = new Field("F_ParentId", "Sys_ItemsDetail", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_ItemCode = new Field("F_ItemCode", "Sys_ItemsDetail", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_ItemName = new Field("F_ItemName", "Sys_ItemsDetail", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_SimpleSpelling = new Field("F_SimpleSpelling", "Sys_ItemsDetail", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_IsDefault = new Field("F_IsDefault", "Sys_ItemsDetail", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_Layers = new Field("F_Layers", "Sys_ItemsDetail", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_SortCode = new Field("F_SortCode", "Sys_ItemsDetail", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_DeleteMark = new Field("F_DeleteMark", "Sys_ItemsDetail", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_EnabledMark = new Field("F_EnabledMark", "Sys_ItemsDetail", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_Description = new Field("F_Description", "Sys_ItemsDetail", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_CreatorTime = new Field("F_CreatorTime", "Sys_ItemsDetail", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_CreatorUserId = new Field("F_CreatorUserId", "Sys_ItemsDetail", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_LastModifyTime = new Field("F_LastModifyTime", "Sys_ItemsDetail", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_LastModifyUserId = new Field("F_LastModifyUserId", "Sys_ItemsDetail", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_DeleteTime = new Field("F_DeleteTime", "Sys_ItemsDetail", "");

			/// <summary>
            /// 
            /// </summary>
            public readonly static Field F_DeleteUserId = new Field("F_DeleteUserId", "Sys_ItemsDetail", "");

			
        }
		#endregion
        
    }

}
