//订单信息提交页业务逻辑
var chooseSeats = $.request("chooseSeats");
var week = $.request("week");
var area = $.request("area");
var address = $.request("address");
var from = "", to = "";
//初始化
function InitInfo() {
    calcTotalMoney();
    InitControl();//初始化控件
    InitForm()//初始化表单
}

//初始化控件
function InitControl() {
    if (area == "shanghai") {
        from = "南通";
        to = "上海";
    } else if (area == "hangzhou") {
        from = address;
        to = "杭州";
    }
    $("#PFromTo").text(from + " - " + to);


    //设置座位
    var SeatsArr = chooseSeats.split(",")
    var SeatsStr = "";
    for (var i = 0; i < SeatsArr.length; i++) {
        SeatsStr += SeatsArr[i] + "座、";
    }
    SeatsStr = $.TrimEnd(SeatsStr, "、");


    $("#PSeats").text(SeatsStr);
    $("input[name='ticketType']").on("change", function () {
        calcTotalMoney();
    });
}

//计算票价总额
function calcTotalMoney() {
    var seatCount = chooseSeats.split(',').length;
    if (seatCount === 0) {
        $.toast("预订座位数量不能为0", "text");
        return;
    }
    var seatPrice = Number($("#seatPrice").val());
    var way = $("#elemSwitchOneway").is(":checked") ? 1 : 2;
    //如果需要接送补接送价20元
    var shuttlePrice = $("#elemSwitchShuttle").is(":checked") ? Number($("#shuttlePrice").val()) : 0;
    var totalMoney = (seatCount * (seatPrice + shuttlePrice) * way).toFixed(2);

    $("#totalMoney").val(totalMoney);
    $("#elemTotalMoney").text("¥" + totalMoney);
}


//初始化表单
function InitForm() {
    layui.use(['form'], function () {
        var form = layui.form();

        //是否单程
        form.on('switch(SwitchOneway)', function (data) {
            var flag = data.elem.checked;
            var isshuttle = flag ? 1 : 0;
            $("#isOneway").val(isshuttle);
            calcTotalMoney();
        })

        //是否接送
        form.on('switch(SwitchShuttle)', function (data) {
            var flag = data.elem.checked;
            var isshuttle = flag ? 1 : 0;
            $("#isShuttle").val(isshuttle);
            var obj = $("#shuttleInfo");
            if (flag) {
                obj.show();
            } else {
                obj.hide();
            }
            calcTotalMoney();
        });
        //监听提交
        form.on('submit(Submit)', function (data) {
            var formData = data.field;
            formData["FromPosition"] = from;
            formData["ToPosition"] = to;
            formData["IsShuttle"] = formData["IsShuttle"] == "on" ? 1 : 0;
            formData["IsOneWay"] = formData["IsOneWay"] == "on" ? 1 : 0;
            formData["Area"] = area;
            formData["ChooseSeats"] = chooseSeats;
            console.log(formData);
            $.ar({
                url: 'CreateOrdersInfo'
                       , data: formData
                       , success: function (data) {
                           location.href = "/ShangHaiManager/ShangHai/Success";
                       }
                       , error: function (data) {
                           $.modalMsg({
                               content: data.msg, success: function () {

                               }
                           });
                       }
            });
            return false;
        });
    });
}

//重选座位
function repickSeatsHandler() {
    if (area == "shanghai") {
        window.location.href = "ShangHaiIndex?week=" + week + "&area=" + area;
    } else if (area == "hangzhou") {
        window.location.href = "HangZhouIndex?address=" + address + "&area=" + area;
    }
}