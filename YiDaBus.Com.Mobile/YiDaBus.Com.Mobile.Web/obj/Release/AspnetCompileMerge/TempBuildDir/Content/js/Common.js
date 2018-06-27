var pageii;
$(function () {
    var $listheader = $('.search_listheader');

    if ($listheader.css('position') == "fixed") {
        if (!$listheader.next().hasClass('nopadding'))
            $listheader.next().css('padding-top', '45px');
    }
});
//显示登录框
function ShowLogin(options) {
    var defaults = {
        url: ''
    };
    var options = $.extend(defaults, options);

    pageii = $.modalPage({ html: $('#loginTemplate').html() });
    $('#ReturnUrl').val(encodeURIComponent(window.location.href));
    InitUserLoginForm(options.url);
}
function InitUserLoginForm(returnUrl) {
    layui.use(['form'], function () {
        var form = layui.form()
        , layer = layui.layer;
        //监听提交

        form.on('submit(userlogin)', function (data) {
            var captchaId = $.cookie("captchaId");
            if (captchaId == 'null' || captchaId == '' || captchaId == undefined) {
                $.modalAlert("请先发送验证码！");
                return false;
            }
            data.field["vaildId"] = captchaId;
            $.ar({
                url: '/Account/Login',
                data: data.field,
                type: 'post',
                success: function (data) {
                    var reqMobile = $.request('mobile');
                    if (reqMobile != '') {
                        try {
                            $.ar({
                                url: globalKey.InterfaceDomain + 'api/Order/UpdateDemand',
                                data: { UserAccount: reqMobile },
                                type: 'post',
                                success: function () {

                                }, erorr: null
                            })
                        } catch (e) {

                        }
                        $.ar({
                            url: globalKey.InterfaceDomain + '/api/Users/CheUIsDepositAndGetUserInfo'
                          , data: { userId: data.userId }
                          , success: function (data) {
                              if (!data.data.isDeposit) {
                                  //没有缴纳金额，跳转到押金支付页面
                                  return window.location.href = "/WxPay/index?payReturnUrl=" + encodeURIComponent(globalKey.MobileUrl);
                              } else {
                                  return window.location.href = "/home/index";
                              }
                          }
                        });
                        return false;
                        //var returnUrl = "/WxPay/Index?payReturnUrl=" + encodeURIComponent(globalKey.MobileUrl);
                        //return window.location.href = returnUrl;
                    }
                    HideLogin();
                    $.cookie('captchaId', null);
                    if (data.data != "") {
                        window.location.href = returnUrl || decodeURIComponent(data.data);
                    }
                    else {
                        if (window.location.pathname.toLowerCase() == "/home/login") {
                            window.location.href = "/Home/Index";
                        }
                        else {
                            window.location.reload();
                        }
                    }
                    return;
                }
            })

            //console.log(data);
            //layer.alert(JSON.stringify(data.field), {
            //    title: '最终的提交信息'
            //})
            return false;
        });
        //错误验证样式修改
        //form.set({
        //    errorMsgCall: function (msg, ss) {
        //        console.log(ss);
        //        layer.msg(msg, {
        //            offset: 40,
        //            anim: 0,
        //            time: 2000
        //        });
        //    }
        //});
    });
}
//关闭登录框
function HideLogin() {
    //$('#layui-m-layer' + pageii).remove();
    mlayer.close(pageii);
}
//获取验证码
function GetVaildCode(obj) {
    var $this = $(obj);
    var mobile = $('#logAccount').val();
    var reg = /^0?1[3|4|5|7|8][0-9]\d{8}$/;
    if (mobile == "") {
        $.modalAlert('请输入手机号');
    }
    else if (reg.test(mobile)) {//手机号正确
        $.ar({
            url: globalKey.InterfaceDomain + 'api/Captcha/SendCaptcha',
            data: {
                "userId": 0,
                "mobileNo": mobile,
                "AppID": ""
            },
            type: 'post',
            success: function (data) {
                console.log(data);
                if (data.code == 200) {
                    //获取验证码成功,设置为每隔60s再次获取
                    //$('#vaildCode').val(data.data.captcha);
                    //$('#vaildId').val(data.data.id);
                    //存入cookie中
                    $.cookie("captchaId", data.data.id);
                    time($this);
                    return;
                }
                else {
                    $.modalAlert(data.msg);
                    return;
                }
            }
        })
    }
    else {
        $.modalAlert("请输入正确的手机号");
    }
}
var wait = 60;//每隔60秒获取一次验证码
//点击完验证按钮，添加样式。
function time(o) {
    if (wait == 0) {
        //o.prop("disabled", false);
        o.html('获取验证码').attr("onclick", 'GetVaildCode(this)').css({ 'color': '#E51532', 'border': '1px solid #E51532' });
        wait = 60;
    } else {
        o.html('重新发送(' + wait + ')').attr("onclick", '').css({ 'color': '#666', 'border': '1px solid #666' });
        $('#login_btn').addClass('bgred');
        wait--;
        setTimeout(function () {
            time(o)
        },
        1000)
    }
}
//显示协议
var regIndex;
function ShowAgreementHandler() {
    regIndex = mlayer.open({
        content: "<div class=\"div_show_title\"><header class=\"div_show_font\" style=\"line-height: 40px;background: #333333;\">用户注册协议 </header> <a onclick=\"close_show();\" class=\"div_show_btn\" style=\"position: absolute;top: 12px;right: 20px;color: #fff;\" ><i class=\"icon iconfont icon-guanbi\"></i></a> </div>" + $("#regDetails").html()
            , type: 1
            , style: 'position:fixed; left:0; top:0; width:100%; height:100%; border: none; -webkit-animation-duration: .5s; animation-duration: .5s;'
    });
    $('#div_regDetails').height($(window).height() - 20);
}
function close_show() {
    mlayer.close(regIndex);
}
//同意指房向服务协议
function Agree() {
    $("#ok").css("color", "")
    var isAgree = $("#isAgree").val();
    if (isAgree == '') {
        $("#ok").attr("color", "#FD7A20");
        $("#isAgree").val("1");
    }
    else {
        $("#ok").css("color", "#7b7069");
        $("#isAgree").val("");
    }
}


/*******
 * 验证手机号
 * @param value
 * @returns {boolean}
 */
function isMobel(mobile) {
    return !!mobile.match(/^(13[0-9]|15[012356789]|17[678]|18[0-9]|14[57])[0-9]{8}$/);
}



//判断用户是否已经登录，没有登录，
//则type=0,弹出登录（默认形式），type=1,跳转登录
function UserIsLog(options) {
    var defaults = {
        type: 0
    };
    var options = $.extend(defaults, options);
    if (!CheckUserLog()) {
        if (options.type == 0) {
            ShowLogin();
        } else {
            window.location.href = "/Home/Login?ReturnUrl=" + encodeURIComponent(window.location.href);
        }
        return false;
    } else {
        return true;
    }
}

function CheckUserLogRedirect(url) {
    if (Boolean($('#IsLogin').val() == "True")) {
        window.location.href = url;
    } else {
        ShowLogin({ url: url });
    }
}

//判断用户是否已经登录，返回bool类型
function CheckUserLog() {
    return Boolean($('#IsLogin').val() == "True");
}
//判断用户是否已经缴纳交易保障金
$.CheckUserPayDeposit = function (options) {
    var defaults = {
        trueHandler: null,
        falseHandler: null
    };
    var options = $.extend(defaults, options);
    $.ar({
        url: globalKey.InterfaceDomain + '/api/Users/CheUIsDepositAndGetUserInfo'
      , data: { userId: $("#Uid").val() }
      , success: function (data) {
          if (!data.data.isDeposit) {
              //没有缴纳交易保障金，跳转到支付页面
              //alert('没有缴纳交易保障金，跳转到支付页面...');
              if (options.falseHandler != null) { options.falseHandler(); }
              else {
                  //window.location.href = "/WxPay/Index?payReturnUrl=" + encodeURIComponent(window.location.href);
                  window.location.href = "/WxPay/DepositExplain?payReturnUrl=" + encodeURIComponent(globalKey.MobileUrl);
                  return false;
              }
          } else {
              if (options.trueHandler != null) { options.trueHandler(); }
          }
      }
    });
}
//跳转至支付页面
$.GoToPayHandler = function () {
    window.location.href = "/WxPay/Index?payReturnUrl=" + encodeURIComponent(document.referrer);
    return;
}