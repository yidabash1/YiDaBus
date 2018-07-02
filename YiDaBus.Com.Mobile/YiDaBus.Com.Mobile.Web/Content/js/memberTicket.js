//订单列表业务逻辑

function GetMyOrder() {
    $.LayPage({
        elem: '#ordersList'
      , Template: '#ordersTemplate'
      , load: false
      , mb: 100
      , url: '/OrderManager/Orders/GetMyOrderList'
      , type: 'post'
      , data: {}//参数
      , NoMsg: '<p>异步加载数据....</p>'
    });
}