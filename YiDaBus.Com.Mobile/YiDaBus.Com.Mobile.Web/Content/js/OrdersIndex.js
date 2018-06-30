//订单js业务逻辑

//初始化
var area = $.request("area");
var week = $.request("week");
var address = $.request("address");
function Init() {

    //获取当次的已下订单
    //遍历seat-container下的a标签设置样式
    $(".seat-unsold").on("click", SeatClickHandler);
}

//座位点击处理
function SeatClickHandler() {
    //判断该改座位是否已被选中
    var $this = $(this);
    var bus = $this.closest("div.bus");
    var buscaption = bus.find("p.bus-caption");
    var seatNum = $this.text();
    var busNum = buscaption.text();
    var msg = "车辆=" + busNum + " | 座位=" + seatNum;
    var curSelect = { seatNum: seatNum, busNum: busNum }

    var exist = $this.hasClass("seat-select");
    if (exist) {//已被选中，直接设置为未选中
        RemoveSelectByValue(chooseList, curSelect);
        $this.removeClass("seat-select");
    } else {//设置选中
        //最多不能超过两个被选中
        var selectedLength = $(".seat-select").length;
        if (selectedLength >= 2) {
            $.toast('每人最多只能选2个座', 'forbidden');
            return;
        }
        //不能即选左边的又选右边（仅针对上海的）
        if (!IsSampleBusNumByChooseList(chooseList, busNum)) {
            $.toast('只能选择一个车辆', 'forbidden');
            return;
        }

        chooseList.push(curSelect);
        //将当前的作为设置为选中状态
        $this.addClass("seat-select");
        $.toast(msg, 'text');
    }
    console.log(chooseList);
}

//下一步处理
function NextHandler() {
    //判断用户是否已经选择
    var selected = $(".seat-container > .seat-select");
    if (selected.length == 0) {
        $.toast('请先选择座位', 'forbidden');
        return;
    }

    var param = "";
    if (area == "shanghai") {
        param += "week=" + week;//日期
    } else if (area == "hangzhou") {
        param += "address=" + address;//区域
    }
    var chooseSeats = '';
    for (var i = 0; i < chooseList.length; i++) {
        chooseSeats += chooseList[i].seatNum + ",";
    }
    chooseSeats = $.TrimEnd(chooseSeats, ",");
    param += "&area=" + area;//区域
    param += "&chooseSeats=" + chooseSeats;//座位号
    param += "&busNum=" + chooseList[0].busNum;//车辆

    window.location.href = "/OrderManager/Orders/OrderInfo?" + param;
}

//删除选中项
function RemoveSelectByValue(arr, val) {
    for (var i = 0; i < arr.length; i++) {
        var arritem = arr[i];
        if (arritem.busNum == val.busNum && arritem.seatNum == val.seatNum) {
            arr.splice(i, 1);
            break;
        }
    }
}

//判断传入的项busNum是否与已有的数组一致，一致返回true，否则返回false
function IsSampleBusNumByChooseList(arr, busNum) {
    for (var i = 0; i < arr.length; i++) {
        var arritem = arr[i];
        if (arritem.busNum != busNum) {
            return false;
        }
    }
    return true;
}


