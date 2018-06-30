using System;	
using System.IO;
using System.Data;
using System.Data.Common;
using Dos.ORM;
using Dos.ORM.Common;

namespace YiDaBus.Com.Model
{


    [Serializable]
	public partial class Orders : Entity
    {

		public Orders():base("Orders") {}

	    #region Field
		
        private int _Id = int.MinValue;
	    /// <summary>
        /// 订单主键ID
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
        private string _OrderNo = string.Empty;
	    /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo
        {
            get { return _OrderNo; }
            set
            {
                this.OnPropertyValueChange(_.OrderNo, _OrderNo, value);
                this._OrderNo = value;
                
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
        private string _CarNumber = string.Empty;
	    /// <summary>
        /// 车牌号
        /// </summary>
        public string CarNumber
        {
            get { return _CarNumber; }
            set
            {
                this.OnPropertyValueChange(_.CarNumber, _CarNumber, value);
                this._CarNumber = value;
                
            }
        }
        private string _FromPosition = string.Empty;
	    /// <summary>
        /// 出发地
        /// </summary>
        public string FromPosition
        {
            get { return _FromPosition; }
            set
            {
                this.OnPropertyValueChange(_.FromPosition, _FromPosition, value);
                this._FromPosition = value;
                
            }
        }
        private string _ToPosition = string.Empty;
	    /// <summary>
        /// 终点地
        /// </summary>
        public string ToPosition
        {
            get { return _ToPosition; }
            set
            {
                this.OnPropertyValueChange(_.ToPosition, _ToPosition, value);
                this._ToPosition = value;
                
            }
        }
        private string _Seats = string.Empty;
	    /// <summary>
        /// 座位（以逗号隔开，@Z@作为标识）
        /// </summary>
        public string Seats
        {
            get { return _Seats; }
            set
            {
                this.OnPropertyValueChange(_.Seats, _Seats, value);
                this._Seats = value;
                
            }
        }
        private string _Shift = string.Empty;
	    /// <summary>
        /// 班次
        /// </summary>
        public string Shift
        {
            get { return _Shift; }
            set
            {
                this.OnPropertyValueChange(_.Shift, _Shift, value);
                this._Shift = value;
                
            }
        }
        private decimal _TotalAmount = 0;
	    /// <summary>
        /// 订单总金额（单位元）
        /// </summary>
        public decimal TotalAmount
        {
            get { return _TotalAmount; }
            set
            {
                this.OnPropertyValueChange(_.TotalAmount, _TotalAmount, value);
                this._TotalAmount = value;
                
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
        private int? _IsOneWay = int.MinValue;
	    /// <summary>
        /// 是否单程（0：是；1：否）
        /// </summary>
        public int? IsOneWay
        {
            get { return _IsOneWay; }
            set
            {
                this.OnPropertyValueChange(_.IsOneWay, _IsOneWay, value);
                this._IsOneWay = value;
                
            }
        }
        private int? _IsShuttle = int.MinValue;
	    /// <summary>
        /// 是否接送（0：是；1：否）
        /// </summary>
        public int? IsShuttle
        {
            get { return _IsShuttle; }
            set
            {
                this.OnPropertyValueChange(_.IsShuttle, _IsShuttle, value);
                this._IsShuttle = value;
                
            }
        }
        private string _MeetPosition = string.Empty;
	    /// <summary>
        /// 接的地点
        /// </summary>
        public string MeetPosition
        {
            get { return _MeetPosition; }
            set
            {
                this.OnPropertyValueChange(_.MeetPosition, _MeetPosition, value);
                this._MeetPosition = value;
                
            }
        }
        private string _SendPosition = string.Empty;
	    /// <summary>
        /// 送的地点
        /// </summary>
        public string SendPosition
        {
            get { return _SendPosition; }
            set
            {
                this.OnPropertyValueChange(_.SendPosition, _SendPosition, value);
                this._SendPosition = value;
                
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
        private int? _PayState = 0;
	    /// <summary>
        /// 支付状态（0：待支付；1：支付成功；2：支付失败；）
        /// </summary>
        public int? PayState
        {
            get { return _PayState; }
            set
            {
                this.OnPropertyValueChange(_.PayState, _PayState, value);
                this._PayState = value;
                
            }
        }
        private string _Week = string.Empty;
	    /// <summary>
        /// 星期几
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
            return new Field[] {_.Id,_.OrderNo,_.UserId,_.CarNumber,_.FromPosition,_.ToPosition,_.Seats,_.Shift,_.TotalAmount,_.UserName,_.UserNickName,_.Mobile,_.IsOneWay,_.IsShuttle,_.MeetPosition,_.SendPosition,_.CreateTime,_.UpdateTime,_.PayState,_.Week,_.Area,_.IsDel };
        }

        /// <summary>
        /// 获取值信息
        /// </summary>
        public override object[] GetValues()
        {
            return new object[] {this._Id,this._OrderNo,this._UserId,this._CarNumber,this._FromPosition,this._ToPosition,this._Seats,this._Shift,this._TotalAmount,this._UserName,this._UserNickName,this._Mobile,this._IsOneWay,this._IsShuttle,this._MeetPosition,this._SendPosition,this._CreateTime,this._UpdateTime,this._PayState,this._Week,this._Area,this._IsDel };
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
            public readonly static Field All = new Field("*", "Orders");

			/// <summary>
            /// 订单主键ID
            /// </summary>
            public readonly static Field Id = new Field("Id", "Orders", "订单主键ID");

			/// <summary>
            /// 订单号
            /// </summary>
            public readonly static Field OrderNo = new Field("OrderNo", "Orders", "订单号");

			/// <summary>
            /// 用户ID
            /// </summary>
            public readonly static Field UserId = new Field("UserId", "Orders", "用户ID");

			/// <summary>
            /// 车牌号
            /// </summary>
            public readonly static Field CarNumber = new Field("CarNumber", "Orders", "车牌号");

			/// <summary>
            /// 出发地
            /// </summary>
            public readonly static Field FromPosition = new Field("FromPosition", "Orders", "出发地");

			/// <summary>
            /// 终点地
            /// </summary>
            public readonly static Field ToPosition = new Field("ToPosition", "Orders", "终点地");

			/// <summary>
            /// 座位（以逗号隔开，@Z@作为标识）
            /// </summary>
            public readonly static Field Seats = new Field("Seats", "Orders", "座位（以逗号隔开，@Z@作为标识）");

			/// <summary>
            /// 班次
            /// </summary>
            public readonly static Field Shift = new Field("Shift", "Orders", "班次");

			/// <summary>
            /// 订单总金额（单位元）
            /// </summary>
            public readonly static Field TotalAmount = new Field("TotalAmount", "Orders", "订单总金额（单位元）");

			/// <summary>
            /// 用户账号
            /// </summary>
            public readonly static Field UserName = new Field("UserName", "Orders", "用户账号");

			/// <summary>
            /// 用户姓名
            /// </summary>
            public readonly static Field UserNickName = new Field("UserNickName", "Orders", "用户姓名");

			/// <summary>
            /// 手机号
            /// </summary>
            public readonly static Field Mobile = new Field("Mobile", "Orders", "手机号");

			/// <summary>
            /// 是否单程（0：是；1：否）
            /// </summary>
            public readonly static Field IsOneWay = new Field("IsOneWay", "Orders", "是否单程（0：是；1：否）");

			/// <summary>
            /// 是否接送（0：是；1：否）
            /// </summary>
            public readonly static Field IsShuttle = new Field("IsShuttle", "Orders", "是否接送（0：是；1：否）");

			/// <summary>
            /// 接的地点
            /// </summary>
            public readonly static Field MeetPosition = new Field("MeetPosition", "Orders", "接的地点");

			/// <summary>
            /// 送的地点
            /// </summary>
            public readonly static Field SendPosition = new Field("SendPosition", "Orders", "送的地点");

			/// <summary>
            /// 创建时间
            /// </summary>
            public readonly static Field CreateTime = new Field("CreateTime", "Orders", "创建时间");

			/// <summary>
            /// 更新时间
            /// </summary>
            public readonly static Field UpdateTime = new Field("UpdateTime", "Orders", "更新时间");

			/// <summary>
            /// 支付状态（0：待支付；1：支付成功；2：支付失败；）
            /// </summary>
            public readonly static Field PayState = new Field("PayState", "Orders", "支付状态（0：待支付；1：支付成功；2：支付失败；）");

			/// <summary>
            /// 星期几
            /// </summary>
            public readonly static Field Week = new Field("Week", "Orders", "星期几");

			/// <summary>
            /// 区域
            /// </summary>
            public readonly static Field Area = new Field("Area", "Orders", "区域");

			/// <summary>
            /// 是否已删除（0：未删除；1：已删除；）
            /// </summary>
            public readonly static Field IsDel = new Field("IsDel", "Orders", "是否已删除（0：未删除；1：已删除；）");

			
        }
		#endregion
        
    }

}
