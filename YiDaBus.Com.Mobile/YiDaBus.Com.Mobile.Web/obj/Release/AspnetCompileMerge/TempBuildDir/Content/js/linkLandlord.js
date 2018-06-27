/// <reference path="Mobiscroll/js/mobiscroll.custom-3.0.0-beta6.min.js" />
$(document).ready(function () {
    //加入意向房
    $(".detail_btmset_part2 .detail_btm_btn.bg1").click(function () {
        $(".overlay").show();
        $("#DivGene").show();
    });
    $(".overlay").click(function () {
        $(this).hide();
        $(".fangdong_tc").slideUp();
        $("#DivGene").hide();
        $('.yajin_tc').hide();
        $('.yxferror_tc').hide();
        $(".share_tc").slideUp();
        $(".kanfang_tc").slideUp();
        $(".fangdong_tc2").slideUp();
    });
    //分享
    $("#share").click(function () {
        $(".overlay").show();
        $(".share_tc").slideDown();

    });
    //性别切换
    $('#gender>a').each(function (index, obj) {
        $(obj).click(function () {
            $(obj).addClass('cur').siblings('a').removeClass('cur');
        });
    });
    //联系房东（拨打电话，创建订单）
    $('#DivTelToLandlor').click(function () {
        TelToLandlordAndBookingHandler();
    });
    $('.qy_btn1').click(function () {
        $(".overlay").trigger('click');
    });
});
////联系房东（完善信息，拨打电话，创建订单）
function TelToLandlordWithCompleteUserInfoHandler() {
    $(".overlay").show();
    $.ar({
        url: globalKey.InterfaceDomain + 'api/Users/CompleteUserInfo'
            , data: { userId: $('#Uid').val(), UserRealName: $('#UserRealName').val(), UserGender: $('a.xingbie_chose.cur').text() }
            , success: function (data) {
                createOrder($('#houseId').val());
            }
    });
    addCallLog();//添加拨打电话的日志
}
//添加拨打电话的日志
function addCallLog() {
    try {
        ///记录拨打电话日志
        $.ar({
            url: globalKey.InterfaceDomain + 'api/Users/AddCallLog'
                    , data: { userId: $('#Uid').val(), houseId: $('#houseId').val() }
                    , success: function (data) {

                    }, error: null
        });
    } catch (e) { }
}
//联系房东（拨打电话，创建订单）
function TelToLandlordAndBookingHandler() {
    //创建订单，拨打电话。
    //创建订单（如果没有创建订单），并弹出预约框（如果尚未预约）
    addCallLog();//添加拨打电话的日志
    createOrder($('#houseId').val());
}
//联系房东
function linkLandlordHandler(tel) {
    //alert('开始...');
    //【1】判断是否已经登录，如果没有登录，则跳转到登录页面。
    //【2】获取用户信息，判断是否已经缴纳交易保障金，如果没有，则跳到支付页面，支付成功之后在跳转到该房源详细。
    //【3】以上两步都验证成功之后，则弹出拨打房东电话弹出层。
    if (!UserIsLog()) {
        //没有登录直接返回；
        //alert('尚未登录，跳转登录...');
        return false;
    }
    $.ar({
        url: globalKey.InterfaceDomain + '/api/Users/CheUIsDepositAndGetUserInfo'
          , data: { userId: $("#Uid").val() }
          , success: function (data) {
              if (!data.data.isDeposit) {
                  //$('.yajin_tc').show();
                  //$(".overlay").show();
                  //跳转至交易保障金说明页面
                  window.location.href = "/WxPay/DepositExplain";
                  return false;
              } else {
                  $('#telTo1').attr('href', 'tel:' + tel);//系统拨号框
                  $('#telTo2').attr('href', 'tel:' + tel);//系统拨号框
                  if (!data.data.isComplete) {
                      //alert('尚未完善信息，则弹出完善信息拨打电话框...');
                      //尚未完善信息，则弹出完善信息拨打电话框。
                      $(".overlay").show();
                      $(".fangdong_tc").slideDown();
                  } else {
                      $(".overlay").show();
                      $(".fangdong_tc2").slideDown();
                  }
              }
          }
    });
}

//跳转至支付交易保障金
function GoToPayHandler() {
    window.location.href = "/WxPay/Index?payReturnUrl=" + encodeURIComponent(window.location.href + "?action=call");
    return;
}
//创建订单
function createOrder(houseId) {
    $.ar({
        url: globalKey.InterfaceDomain + '/api/Order/Create'
      , data: { userId: $("#Uid").val(), houseId: houseId, source: 1 }
      , success: function (data) {
          //alert('弹出系统拨号框...');
          //判断是否已经预约看房，如果已经预约，则不弹出预约对话框
          //isExist：判断订单是否存在；
          //isBooking：判断订单是否被预约；
          if (!data.data.isExist) {

              $.modalConfirm({
                  content: "此房源已加入意向单，是否前去查看？", btn: ['前去查看', '继续浏览'], callBack: function () {
                      window.location.href = "/order/index";
                  }
              });
              //$.modalMsg({
              //    content: data.msg, success: function () {
              //        window.location.href = "/order/index";
              //    }
              //});
          }
      }
      , error: function (data) {
          if (data.data) {
              if (data.data.errCode == -3) {
                  $('.yxferror_tc').show();
                  $('.fangdong_tc2').slideUp();
                  $('.fangdong_tc').slideUp();
              } else {
                  $.modalAlert(data.msg);
              }
          }
          else {
              $.modalAlert(data.msg);
          }
          return false;
      }
    });
}
//不需要
function NoNeedHandler() {
    $(".kanfang_tc").slideUp();
    $(".overlay").slideUp();
    $(".share_tc").hide();
}
//初始化日期控件
var fun = function () {
    var currYear = (new Date()).getFullYear();
    $('#bookingTime').scroller('destroy').scroller({
        preset: 'datetime',
        theme: 'android-ics light',
        minDate: new Date(),
        endYear: currYear + 1, //结束年份
        lang: 'zh'
    });
}
//初始化确认提醒表单
function InitSureRemind(orderNo, action) {
    layui.use(['form'], function () {
        var form = layui.form()
        , layer = layui.layer
        var curr = new Date().getFullYear();
        $(".fangdong_tc2").slideUp();
        $('.settings select').bind('change', function () {
            fun();
        });
        fun();
        //alert('初始化确认提醒表单...');
        //监听提交
        form.on('submit(SureRemind)', function (data) {
            data.field["userId"] = $('#Uid').val();
            //data.field["houseId"] = $('#houseId').val();
            data.field["orderNo"] = orderNo;
            //确认提醒表单
            var action_url = '';
            if (action == 'add') { action_url = 'api/Order/AddBooking'; }
            if (action == 'change') { action_url = 'api/Order/UpdateBooking'; }
            $.ar({
                url: globalKey.InterfaceDomain + action_url,
                data: data.field,
                success: function (data) {
                    $.modalMsg({
                        content: data.data, success: function () {
                            window.location.href = "/order/index";
                        }
                    });
                    $(".overlay").hide();
                    $(".kanfang_tc").slideUp();
                }
            })
            return false;
        });
    });
}