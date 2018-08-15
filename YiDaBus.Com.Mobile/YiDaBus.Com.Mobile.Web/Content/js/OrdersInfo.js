//订单信息提交页业务逻辑
var chooseSeats = $.request("chooseSeats");
var week = $.request("week");
var area = $.request("area");
var address = $.request("address");
var from = "", to = "";
var busNum = $.request("busNum");
var TicketPriceData = null;
//初始化
function InitInfo() {
    GetTicketPriceDetails();
    calcTotalMoney();
    InitControl();//初始化控件
    InitForm()//初始化表单
    InitData();//初始化数据
}

function GetTicketPriceDetails() {
    //ajax获取
    $.ar({
        url: 'GetTicketPriceDetails'
        , async: false
        , success: function (data) {
            TicketPriceData = data.data;
        }
        , error: function (data) {

        }
    });
}
//初始化控件
function InitControl() {
    $("#CarNumber").val(busNum);
    $("#busNumDiv").text(busNum);
    if (area == "shanghai") {
        $("#AreaHead").text("上海订票");
        from = "南通";
        to = "上海";
    } else if (area == "hangzhou") {
        $(".showOnlyShanghai").remove();
        $("#AreaHead").text("杭州订票");
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
    var SingleTicketPrice = 0;//单程
    var MutilTicketPrice = 0;//双程
    var ShuttlePrice = 0;//接送费
    var SeatPrice = 0;//票价
    var IsShuttlePrice = $("#elemSwitchShuttle").is(":checked");
    var IsOneWay = $("#elemSwitchOneway").is(":checked");
    if (area == "shanghai") {
        for (var i = 0; i < TicketPriceData.length; i++) {
            //接送费
            if (IsShuttlePrice && TicketPriceData[i]["F_ItemCode"] == "ShangHaiDeliveryFee") {
                ShuttlePrice = parseFloat(TicketPriceData[i]["F_Description"]);
            }
            //单程
            if (IsOneWay &&  TicketPriceData[i]["F_ItemCode"] == "ShangHaiTicketPrice") {
                SingleTicketPrice = parseFloat(TicketPriceData[i]["F_Description"]);
            }
            //双程
            if (!IsOneWay &&  TicketPriceData[i]["F_ItemCode"] == "ShangHaiMutilTicketPrice") {
                MutilTicketPrice = parseFloat(TicketPriceData[i]["F_Description"]);
            }
        }
    } else if (area == "hangzhou") {
        for (var i = 0; i < TicketPriceData.length; i++) {
            //单程
            if (IsOneWay && TicketPriceData[i]["F_ItemCode"] == "HangZhouTicketPrice") {
                SingleTicketPrice = parseFloat(TicketPriceData[i]["F_Description"]);
            }
            //双程
            if (!IsOneWay && TicketPriceData[i]["F_ItemCode"] == "HangZhouMutilTicketPrice") {
                MutilTicketPrice = parseFloat(TicketPriceData[i]["F_Description"]);
            }
        }
    }
    var SeatCount = chooseSeats.split(',').length;
    if (SeatCount === 0) {
        $.toast("预订座位数量不能为0", "text");
        return;
    }
    var SeatPrice = IsOneWay ? SingleTicketPrice : MutilTicketPrice;

    var totalMoney = (SeatCount * (SeatPrice + ShuttlePrice)).toFixed(2);

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


//初始化数据
function InitData() {
    $.ar({
        url: '/MemberManager/Member/GetUserInfo'
              , data: null
              , success: function (data) {
                  var userInfo = data.data;
                  if (userInfo) {
                      $("#UserNickName").val(userInfo.UserNickName);
                      $("#Mobile").val(userInfo.Mobile);
                  }
              }
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