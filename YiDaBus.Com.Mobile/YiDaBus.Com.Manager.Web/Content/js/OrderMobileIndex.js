//订单列表业务逻辑

$(function () {
    GetOrderList();
    $("#btnSearch").on("click", function () {
        $("#ordersList").html('');
        GetOrderList();
    });
    $("#btnExit").on("click", function () {
        $.BottomDialog({
            title: '提示',
            content: '确认退出吗？',
            callBack: function () {
                $.ar({
                    url: '/Login/OutLogin4Mobile'
                  , type: 'get'
                  , async: false
                  , success: function (data) {
                      location.href = "/Login/MobileIndex";
                  }
                  , error: function (data) {

                  }
                });
                return false;
            }
        });
    });
});


function GetOrderList() {
    $.LayPage({
        elem: '#ordersList'
      , Template: '#ordersTemplate'
      , load: false
      , mb: 100
      , url: '/OrderManage/Order/GetOrderList4Mobile'
      , type: 'post'
      , data: { orderNo: $("#orderNo").val(), area: $("#area").val() }//参数
      , NoMsg: '<p>无订单</p>'
    });
}

