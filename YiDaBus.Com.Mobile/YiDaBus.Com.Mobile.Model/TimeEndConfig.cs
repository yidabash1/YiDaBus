using System;	
using System.IO;
using System.Data;
using System.Data.Common;
using Dos.ORM;
using Dos.ORM.Common;

namespace YiDaBus.Com.Model
{


    [Serializable]
	public partial class TimeEndConfig : Entity
    {

		public TimeEndConfig():base("TimeEndConfig") {}

	    #region Field
		
        private int _Id = int.MinValue;
	    /// <summary>
        /// 主键ID
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
        private string _Area = string.Empty;
	    /// <summary>
        /// 区域
        /// </summary>
        public string Area
        {
            get { return _Area; }
            set
            {
                this.OnPropertyValueChange(_.Area, _Area, value);
                this._Area = value;
                
            }
        }
        private string _Week = string.Empty;
	    /// <summary>
        /// 星期
        /// </summary>
        public string Week
        {
            get { return _Week; }
            set
            {
                this.OnPropertyValueChange(_.Week, _Week, value);
                this._Week = value;
                
            }
        }
        private string _EndTime = string.Empty;
	    /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime
        {
            get { return _EndTime; }
            set
            {
                this.OnPropertyValueChange(_.EndTime, _EndTime, value);
                this._EndTime = value;
                
            }
        }
        private int? _IsClosed = int.MinValue;
	    /// <summary>
        /// 是否关闭（0：不关闭；1：关闭；）
        /// </summary>
        public int? IsClosed
        {
            get { return _IsClosed; }
            set
            {
                this.OnPropertyValueChange(_.IsClosed, _IsClosed, value);
                this._IsClosed = value;
                
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
            return new Field[] {_.Id,_.Area,_.Week,_.EndTime,_.IsClosed };
        }

        /// <summary>
        /// 获取值信息
        /// </summary>
        public override object[] GetValues()
        {
            return new object[] {this._Id,this._Area,this._Week,this._EndTime,this._IsClosed };
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
            public readonly static Field All = new Field("*", "TimeEndConfig");

			/// <summary>
            /// 主键ID
            /// </summary>
            public readonly static Field Id = new Field("Id", "TimeEndConfig", "主键ID");

			/// <summary>
            /// 区域
            /// </summary>
            public readonly static Field Area = new Field("Area", "TimeEndConfig", "区域");

			/// <summary>
            /// 星期
            /// </summary>
            public readonly static Field Week = new Field("Week", "TimeEndConfig", "星期");

			/// <summary>
            /// 结束时间
            /// </summary>
            public readonly static Field EndTime = new Field("EndTime", "TimeEndConfig", "结束时间");

			/// <summary>
            /// 是否关闭（0：不关闭；1：关闭；）
            /// </summary>
            public readonly static Field IsClosed = new Field("IsClosed", "TimeEndConfig", "是否关闭（0：不关闭；1：关闭；）");

			
        }
		#endregion
        
    }

}
