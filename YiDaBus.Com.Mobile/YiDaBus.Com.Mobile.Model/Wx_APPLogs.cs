using System;	
using System.IO;
using System.Data;
using System.Data.Common;
using Dos.ORM;
using Dos.ORM.Common;

namespace YiDaBus.Com.Model
{


    [Serializable]
	public partial class Wx_APPLogs : Entity
    {

		public Wx_APPLogs():base("Wx_APPLogs") {}

	    #region Field
		
        private int _Id = int.MinValue;
	    /// <summary>
        /// 主键
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
        private int? _UserId = int.MinValue;
	    /// <summary>
        /// 用户ID
        /// </summary>
        public int? UserId
        {
            get { return _UserId; }
            set
            {
                this.OnPropertyValueChange(_.UserId, _UserId, value);
                this._UserId = value;
                
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
        private string _ActionType = string.Empty;
	    /// <summary>
        /// 操作类型
        /// </summary>
        public string ActionType
        {
            get { return _ActionType; }
            set
            {
                this.OnPropertyValueChange(_.ActionType, _ActionType, value);
                this._ActionType = value;
                
            }
        }
        private string _Descriptions = string.Empty;
	    /// <summary>
        /// 详细
        /// </summary>
        public string Descriptions
        {
            get { return _Descriptions; }
            set
            {
                this.OnPropertyValueChange(_.Descriptions, _Descriptions, value);
                this._Descriptions = value;
                
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
            return new Field[] {_.Id,_.UserId,_.UserName,_.UserNickName,_.ActionType,_.Descriptions,_.CreateTime };
        }

        /// <summary>
        /// 获取值信息
        /// </summary>
        public override object[] GetValues()
        {
            return new object[] {this._Id,this._UserId,this._UserName,this._UserNickName,this._ActionType,this._Descriptions,this._CreateTime };
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
            public readonly static Field All = new Field("*", "Wx_APPLogs");

			/// <summary>
            /// 主键
            /// </summary>
            public readonly static Field Id = new Field("Id", "Wx_APPLogs", "主键");

			/// <summary>
            /// 用户ID
            /// </summary>
            public readonly static Field UserId = new Field("UserId", "Wx_APPLogs", "用户ID");

			/// <summary>
            /// 用户账号
            /// </summary>
            public readonly static Field UserName = new Field("UserName", "Wx_APPLogs", "用户账号");

			/// <summary>
            /// 用户姓名
            /// </summary>
            public readonly static Field UserNickName = new Field("UserNickName", "Wx_APPLogs", "用户姓名");

			/// <summary>
            /// 操作类型
            /// </summary>
            public readonly static Field ActionType = new Field("ActionType", "Wx_APPLogs", "操作类型");

			/// <summary>
            /// 详细
            /// </summary>
            public readonly static Field Descriptions = new Field("Descriptions", "Wx_APPLogs", "详细");

			/// <summary>
            /// 创建时间
            /// </summary>
            public readonly static Field CreateTime = new Field("CreateTime", "Wx_APPLogs", "创建时间");

			
        }
		#endregion
        
    }

}
