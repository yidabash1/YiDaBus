var orderId = $.request("orderId");
function GetOrderDetails() {
    $.LayDetails({
        elem: '#orderDetails'
      , Template: '#orderDetailsTemplate'
      , mb: 100
      , url: '/OrderManage/Order/GetOrderDetailsById'
      , type: 'post'
      , data: { orderId: orderId }//参数
      , success: function (html) {
          $('#orderDetails').html(html);
          $("#btnDel").on("click", function () {
              $.BottomDialog({
                  title: '删除提示',
                  content: '您确定删除这条预订信息吗？只能删除过期预订信息哟',
                  callBack: function () {
                      $.ar({
                          url: '/OrderManage/Order/DeleteOrderByOrderId'
                           , data: { orderId: orderId }
                           , success: function (data) {
                               $.modalMsg({
                                   content: data.msg, success: function () {
                                       location.href = "/OrderManage/Order/MobileIndex";
                                   }
                               });

                           }
                           , error: function (data) {
                               $.modalMsg({
                                   content: data.msg, success: function () {

                                   }
                               });
                           }
                      });
                      return false;
                  }
              });
          });
      }
    });
}