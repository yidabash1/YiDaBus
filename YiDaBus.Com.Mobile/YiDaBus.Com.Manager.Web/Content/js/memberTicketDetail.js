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

//确认收款
function AuditOrdersHandler() {
    var index = mlayer.open({
        content: '您确定要审核该条信息吗？'
         , btn: ['支付成功', '支付失败']
         , skin: 'footer'
         , yes: function (index) {
             AuditHandlerBase(1);
             if (close) {
                 mlayer.close(index);
             }
         }, no: function (index) {
             AuditHandlerBase(2);
             if (close) {
                 mlayer.close(index);
             }
         }
    });
}

function AuditHandlerBase(auditStatus) {
    $.ar({
        url: 'Audit'
            , data: { keyValue: orderId, auditStatus: auditStatus }
            , success: function (data) {
                $.modalMsg({
                    content: data.msg, success: function () {
                        window.location.href = "/OrderManage/Order/MobileIndex";
                        return;
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
}

//作废订单
function DeleteOrdersHandler() {
    $.BottomDialog({
        title: '删除提示',
        content: '您确定删除这条预订信息吗？',
        callBack: function () {
            $.ar({
                url: 'DeleteForm'
               , data: { keyValue: orderId }
               , success: function (data) {
                   $.modalMsg({
                       content: "订单作废成功！", success: function () {
                           window.location.href = "/OrderManage/Order/MobileIndex";
                           return;
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

}

//迁票修改
function ModifyOrderHandler() {
    window.location.href = "/OrderManage/Order/Form?keyValue=" + orderId + "&from=mobile";
}