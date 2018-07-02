//重新加载
$.reload = function () {
    location.reload();
    return false;
}

$.request = function (name) {
    var search = location.search.slice(1);
    search = decodeURI(search);
    var arr = search.split("&");
    for (var i = 0; i < arr.length; i++) {
        var ar = arr[i].split("=");
        if (ar[0] == name) {
            if (unescape(ar[1]) == 'undefined') {
                return "";
            } else {
                return unescape(ar[1]);
            }
        }
    }
    return "";
}

$.requestWithOutDecode = function (name) {
    var search = location.search.slice(1);
    var arr = search.split("&");
    for (var i = 0; i < arr.length; i++) {
        var ar = arr[i].split("=");
        if (ar[0] == name) {
            if (unescape(ar[1]) == 'undefined') {
                return "";
            } else {
                return unescape(ar[1]);
            }
        }
    }
    return "";
}
//下载
$.download = function (url, data, method) {
    if (url && data) {
        data = typeof data == 'string' ? data : jQuery.param(data);
        var inputs = '';
        $.each(data.split('&'), function () {
            var pair = this.split('=');
            inputs += '<input type="hidden" name="' + pair[0] + '" value="' + pair[1] + '" />';
        });
        $('<form action="' + url + '" method="' + (method || 'post') + '">' + inputs + '</form>').appendTo('body').submit().remove();
    };
};
//提示框
$.modalAlert = function (content) {
    var index = mlayer.open({
        content: content
      , anim: false
      , skin: 'msg'
      , style: 'min-width: 100px;background-color: #000;filter: alpha(opacity=60);background-color: rgba(0,0,0,.6);color: #fff;border: none;top: -10em;' //自定风格
      , time: 2
    });
}

//消息框
$.modalMsg = function (options) {
    var defaults = {
        title: '',
        content: '',
        btn: '确认',
        success: null,
        alwaysClose: true
    };
    var options = $.extend(defaults, options);
    var index = mlayer.open({
        title: options.title == "" ? "" : [options.title, 'background-color: #eee;'],
        content: options.content
      , btn: options.btn
      , yes: function () {
          if (options.success != null) options.success();
          if (options.alwaysClose) mlayer.close(index);
      }
    });
}
//询问框
$.modalConfirm = function (options) {
    var defaults = {
        content: '',
        btn: ['确认', '取消'],
        callBack: null,
        close: true
    };
    var options = $.extend(defaults, options);
    var index = mlayer.open({
        content: options.content
      , btn: options.btn
      , yes: function (index) {
          options.callBack();
          if (options.close) {
              mlayer.close(index);
          }
      }
    });
    console.log(index);
}
//底部对话框
$.BottomDialog = function (options) {
    var defaults = {
        id: options.id,
        content: '',
        btn: ['确认', '取消'],
        callBack: null,
        close: true
    };
    var options = $.extend(defaults, options);
    var index = mlayer.open({
        content: options.content
      , btn: options.btn
      , skin: 'footer'
      , yes: function (index) {
          options.callBack(options.id);
          if (close) {
              mlayer.close(index);
          }
      }
    });
}
//底部提示
$.BottomMsg = function (options) {
    var defaults = {
        content: ''
    };
    var options = $.extend(defaults, options);
    return mlayer.open({
        content: options.content
      , skin: 'footer'
    });
}
$.modalClose = function () {
    var index = mlayer.getFrameIndex(window.name); //先得到当前iframe层的索引
    mlayer.close(index);
}
//关闭所有模态框
$.modalCloseALL = function () {
    mlayer.closeAll();
}
//全屏的页面层
$.modalPage = function (options) {
    var defaults = {
        html: ''
        , style: 'position:fixed; left:0; top:0; width:100%; height:100%; border: none; -webkit-animation-duration: .5s; animation-duration: .5s;',
        background_color:'#fff'
    };
    var options = $.extend(defaults, options);
    var pageii = mlayer.open({
        type: 1
       , content: options.html
       , anim: 'up'
       , style: options.style + 'background-color: ' + options.background_color + ';'
    });
    return pageii;
}
//全屏的页面层
$.modalLoad = function (options) {
    var defaults = {
        content: ''
      , shadeClose: false
    };
    var options = $.extend(defaults, options);
    var index = mlayer.open({
        type: 2
        , content: options.content
        , shadeClose: options.shadeClose
    });
    return index;
}
//ajax请求
$.ar = function (options) {
    var loadIndex;
    var defaults = {
        url: "",
        data: {},
        type: "post",
        loading: "",
        success: null,
        error: null,
        close: true,
        load: true,//是否在请求的时候显示加载中弹出层
        isalert: true
    };
    var options = $.extend(defaults, options);

    $.ajax({
        url: options.url,
        data: options.data,
        type: options.type,
        dataType: "json",
        success: function (data) {
            if (data.code == 200) {//成功
                if (options.success != null) options.success(data);
            }
            else {//失败
                // console.log(options);
                if (options.error != null) options.error(data);
                else if (data.code == 301) {
                    $.modalAlert(data.msg);
                    $.modalMsg({
                        title: '提示',
                        content: data.msg,
                        btn: '确认',
                        success: function () {
                            window.location.href = "/MemberManager/Member/MemberInfo?" + encodeURIComponent(window.location.href);
                            return;
                        },
                        alwaysClose: true
                    })
                }
                else if (data.code != 201 || data.msg != '查询成功无数据') {
                    if (options.isalert) $.modalAlert(data.msg);
                } 
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            if (options.load) mlayer.close(loadIndex);
            $.modalAlert("请求错误");
        },
        beforeSend: function () {
            if (options.load)
                loadIndex = $.modalLoad({ content: options.loading, shadeClose: false });
        },
        complete: function () {
            if (options.load)
                mlayer.close(loadIndex);
        }
    });
}
//删除表单
$.deleteForm = function (options) {
    var defaults = {
        prompt: "注：您确定要删除该项数据吗？",
        url: "",
        param: [],
        loading: "正在删除数据...",
        success: null,
        close: true
    };
    var options = $.extend(defaults, options);
    if ($('[name=__RequestVerificationToken]').length > 0) {
        options.param["__RequestVerificationToken"] = $('[name=__RequestVerificationToken]').val();
    }
    $.modalConfirm(options.prompt, function (r) {
        if (r) {
            $.loading(true, options.loading);
            window.setTimeout(function () {
                $.ajax({
                    url: options.url,
                    data: options.param,
                    type: "post",
                    dataType: "json",
                    success: function (data) {
                        if (data.status == 1) {
                            options.success(data);
                            $.modalMsg(data.msg, data.status);
                        } else {
                            $.modalAlert(data.msg, data.status);
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        $.loading(false);
                        $.modalMsg(errorThrown, 0);
                    },
                    beforeSend: function () {
                        $.loading(true, options.loading);
                    },
                    complete: function () {
                        $.loading(false);
                    }
                });
            }, 500);
        }
    });

}
//绑定选择
$.fn.bindSelect = function (options) {
    var defaults = {
        id: "id",
        text: "text",
        search: false,
        url: "",
        param: [],
        change: null
    };
    var options = $.extend(defaults, options);
    var $element = $(this);
    if (options.url != "") {
        $.ajax({
            url: options.url,
            data: options.param,
            dataType: "json",
            async: false,
            success: function (data) {
                $.each(data, function (i) {
                    $element.append($("<option></option>").val(data[i][options.id]).html(data[i][options.text]));
                });
                $element.select2({
                    minimumResultsForSearch: options.search == true ? 0 : -1
                });
                $element.on("change", function (e) {
                    if (options.change != null) {
                        options.change(data[$(this).find("option:selected").index()]);
                    }
                    $("#select2-" + $element.attr('id') + "-container").html($(this).find("option:selected").text().replace(/　　/g, ''));
                });
            }
        });
    } else {
        $element.select2({
            minimumResultsForSearch: -1
        });
    }
}
//下拉分页
$.LayPage = function (options) {
    var defaults = {
        url: ''
        , pagesize: 20
        , data: {}
        , type: 'get'
        , Template: ''
        , NoMsg: '没有数据'
        , isAuto: true
        , mb: 50
        , error: null
        , isalert: true
        , load: false
        , success: null
        , NullColor: '#F5F5F5'
    };
    var options = $.extend(defaults, options);

    layui.use(['laytpl', 'flow'], function () {
        var laytpl = layui.laytpl;
        var flow = layui.flow;
        flow.lazyimg();
        var dataMsg = '';
        flow.load({
            elem: options.elem //指定列表容器
            , isLazyimg: true
            , isAuto: options.isAuto
            , mb: options.mb
            , done: function (page, next) { //到达临界点（默认滚动触发），触发下一页
                //以jQuery的Ajax请求为例，请求下一页数据（注意：page是从2开始返回）
                options.data["pageindex"] = page;
                if (options.pagesize != null) options.data["pagesize"] = options.pagesize;
                $.ar({
                    url: options.url,
                    data: options.data,
                    type: options.type,
                    isalert: options.isalert,
                    load: options.load,
                    success: function (data) {
                        if (options.success == null) {

                            //第三步：渲染模版
                            var getTpl = $(options.Template).html();
                            laytpl(getTpl).render(data.data.items, function (html) {
                                //执行下一页渲染，第二参数为：满足“加载更多”的条件，即后面仍有分页
                                //pages为Ajax返回的总页数，只有当前页小于总页数的情况下，才会继续出现加载更多

                                next(html, page < data.data.totalPages);
                            });
                        }
                        else {
                            options.success(laytpl, next, page, data);
                        }
                    },
                    error: function (data) {
                        dataMsg = data.msg;
                        if (options.error != null) { options.error(); next('', false); }
                        else {
                            if (data.code == 201 && data.msg == '查询成功无数据') {
                                if ($(options.elem).find('.search_null').length == 0) {
                                    $(options.elem).append('<div class="search_null"><dl style="border-width: 0px;background: ' + options.NullColor + ';"><dt style="float: none;width:100%;text-align: center;"><img width="75" src="/Content/img/tousu_nullicon.png"></dt><dd style="margin-left: 0px;position: relative;margin-top: 10px;text-align: center;">' + options.NoMsg + '</dd></dl></div>');
                                }
                                next('', false);
                            } else {
                                next('', false);
                            }
                        }
                    }
                })
            },
            end: function () {
                if (dataMsg == '查询成功无数据') {
                    return '';
                } else {
                    return '没有更多信息';
                }
            }
        });
    });
}
//详情
$.LayDetails = function (options) {
    var defaults = {
        url: ''
        , data: {}
        , type: 'post'
        , Template: ''
        , isAuto: true
        , mb: 50
        , elme: ""
        , success: null
        , error: null
    };
    var options = $.extend(defaults, options);
    layui.use(['laytpl'], function () {
        var laytpl = layui.laytpl;
        $.ar({
            url: options.url,
            data: options.data,
            type: options.type,
            success: function (data) {
                //if (data.code == 200) {
                var getTpl = $(options.Template).html();
                laytpl(getTpl).render(data.data, function (html) {
                    options.success(html, data.data);
                });
                //}
            },
            error: function (data) {
                if (options.error != null)
                    options.error(data)
            }
        });
    })
}

//表单赋值
$.fn.formSerialize = function (formdate) {
    var element = $(this);

    if (!!formdate) {
        for (var key in formdate) {
            var $id = element.find('#' + key);
            var value = $.trim(formdate[key]).replace(/&nbsp;/g, '');
            var type = $id.attr('type');
            switch (type) {
                case "checkbox":
                    if (value == "true") {
                        $id.attr("checked", 'checked');
                    } else {
                        $id.removeAttr("checked");
                    }
                    break;
                case "select":
                    $id.val(value).trigger("change");
                    break;
                default:
                    $id.val(value);
                    break;
            }
        };
        return false;
    }
    var postdata = {};
    element.find('input,select,textarea').each(function (r) {
        var $this = $(this);
        var id = $this.attr('id');
        var type = $this.attr('type');
        switch (type) {
            case "checkbox":
                postdata[id] = $this.is(":checked");
                break;
            default:
                postdata[id] = $this.val();
                break;
        }
    });
    if ($('[name=__RequestVerificationToken]').length > 0) {
        postdata["__RequestVerificationToken"] = $('[name=__RequestVerificationToken]').val();
    }
    return postdata;
};
//序列化成对象
$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
}
/** 
   * 字符串转时间（yyyy-MM-dd T HH:mm:ss） 
   * result （分钟） 
   */
$.stringToDate = function (fDate) {
    return new Date(fDate.replace('T', ' ').replace(/-/g, "/"));
}

/** 
     * 格式化日期 
     * @param date 日期 
     * @param format 格式化样式,例如yyyy-MM-dd HH:mm:ss E 
     * @return 格式化后的金额 
     */
$.formatDate = function (date, format) {
    date = new Date(date.replace('T', ' ').replace(/-/g, "/"));
    var v = "";
    if (typeof date == "string" || typeof date != "object") {
        return;
    }
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var day = date.getDate();
    var hour = date.getHours();
    var minute = date.getMinutes();
    var second = date.getSeconds();
    var weekDay = date.getDay();
    var ms = date.getMilliseconds();
    var weekDayString = "";

    if (weekDay == 1) {
        weekDayString = "星期一";
    } else if (weekDay == 2) {
        weekDayString = "星期二";
    } else if (weekDay == 3) {
        weekDayString = "星期三";
    } else if (weekDay == 4) {
        weekDayString = "星期四";
    } else if (weekDay == 5) {
        weekDayString = "星期五";
    } else if (weekDay == 6) {
        weekDayString = "星期六";
    } else if (weekDay == 7) {
        weekDayString = "星期日";
    }

    v = format;
    //Year 
    v = v.replace(/yyyy/g, year);
    v = v.replace(/YYYY/g, year);
    v = v.replace(/yy/g, (year + "").substring(2, 4));
    v = v.replace(/YY/g, (year + "").substring(2, 4));

    //Month 
    var monthStr = ("0" + month);
    v = v.replace(/MM/g, monthStr.substring(monthStr.length - 2));

    //Day 
    var dayStr = ("0" + day);
    v = v.replace(/dd/g, dayStr.substring(dayStr.length - 2));

    //hour 
    var hourStr = ("0" + hour);
    v = v.replace(/HH/g, hourStr.substring(hourStr.length - 2));
    v = v.replace(/hh/g, hourStr.substring(hourStr.length - 2));

    //minute 
    var minuteStr = ("0" + minute);
    v = v.replace(/mm/g, minuteStr.substring(minuteStr.length - 2));

    //Millisecond 
    v = v.replace(/sss/g, ms);
    v = v.replace(/SSS/g, ms);

    //second 
    var secondStr = ("0" + second);
    v = v.replace(/ss/g, secondStr.substring(secondStr.length - 2));
    v = v.replace(/SS/g, secondStr.substring(secondStr.length - 2));

    //weekDay 
    v = v.replace(/E/g, weekDayString);
    return v;
}
//获取当前时间
$.getNowFormatDate = function () {
    var date = new Date();
    var seperator1 = "-";
    var seperator2 = ":";
    var month = date.getMonth() + 1;
    var strDate = date.getDate();
    if (month >= 1 && month <= 9) {
        month = "0" + month;
    }
    if (strDate >= 0 && strDate <= 9) {
        strDate = "0" + strDate;
    }
    var currentdate = date.getFullYear() + seperator1 + month + seperator1 + strDate;
    return currentdate;
}
$.diffDate = function (evalue) {
    var dB = new Date(evalue.replace(/-/g, "/"));
    if (new Date() > Date.parse(dB)) {
        return true;
    }
    return false;
}
//电话加密
$.formatMobile = function (mobile) {
    if (mobile.length == 11) {
        return mobile.substr(0, 3) + "****" + mobile.substr(7);
    } else {
        return '';
    }
}
//微信在浏览器中打开提示
$.weixinOpenInBrowserTip = function (ele) {
    var ua = navigator.userAgent;
    var isWeixin = !!/MicroMessenger/i.test(ua);
    if (isWeixin) {
        ele.onclick = function (e) {
            window.event ? window.event.returnValue = false : e.preventDefault();
            document.getElementById('JweixinTip').style.display = 'block';
        }
        document.getElementById('JweixinTip').onclick = function () {
            this.style.display = 'none';
        }
    }
}

/**
   * 序列化表单元素为JSON对象
   * @param form          Form表单id或表单jquery DOM对象
   * @returns {}
   */
$.serialize = function (form) {
    var $form = (typeof (form) == "string" ? $("#" + form) : form);
    var dataArray = $form.serializeArray();
    var result = {};
    $(dataArray).each(function () {
        //如果在结果对象中存在这个值，那么就说明是多选的情形。
        if (result[this.name]) {
            //多选的情形，值是数组，直接push
            result[this.name].push(this.value);
        } else {
            //获取当前表单控件元素
            var element = $form.find("[name='" + this.name + "']")[0];
            //获取当前控件类型
            var type = (element.type || element.nodeName).toLowerCase();
            //如果控件类型为多选那么值就是数组形式，否则就是单值形式。
            result[this.name] = (/^(select-multiple|checkbox)$/i).test(type) ? [this.value] : this.value;
        }
    });
    return result;
};
/**
     * 设置表单值
     * @param form          Form表单id或表单jquery DOM对象
     * @param data          json对象，多选时为数组
     * 代码实现参考此开源项目https://github.com/kflorence/jquery-deserialize/
     */
$.deserialize = function (form, data) {
    var rcheck = /^(?:radio|checkbox)$/i,
        rselect = /^(?:option|select-one|select-multiple)$/i,
        rvalue = /^(?:button|color|date|datetime|datetime-local|email|hidden|month|number|password|range|reset|search|submit|tel|text|textarea|time|url|week)$/i;

    var $form = (typeof (form) == "string" ? $("#" + form) : form);

    //得到所有表单元素
    function getElements(elements) {
        //此处elements为jquery对象。这个map函数使用来便利elements数组的.如存在多个form表单，则便利多个form表单
        return elements.map(function (index, domElemen) {
            //this代表form表单，this.elements获取表单中的DOM数组. jQuery.makeArray 转换一个类似数组的对象成为真正的JavaScript数组。
            return this.elements ? jQuery.makeArray(this.elements) : this;
            //过滤不启用的选项
        }).filter(":input:not(:disabled)").get();
    }
    //把表单元素转为json对象
    function elementsToJson(elements) {
        var current, elementsByName = {};
        //elementsByName对象为{控件名：控件元素或控件元素数组}
        jQuery.each(elements, function (i, element) {
            current = elementsByName[element.name];
            elementsByName[element.name] = current === undefined ? element :
                //如果已经是一个数组了，那么就添加，否则构造一个数组
                (jQuery.isArray(current) ? current.concat(element) : [current, element]);
        });
        return elementsByName;
    }

    var elementsJson = elementsToJson(getElements($form));
    //data是一个对象
    for (var key in data) {
        var val = data[key];
        var dataArr = [];//更具数据直接构造一个jQUery序列化后的数组形式。
        //判断值是否为数组
        if ($.isArray(val)) {
            for (var i = 0, v; v = val[i++];) {
                //是数组那么就变成多个对象形式
                dataArr.push({ name: key, value: v });
            }
        } else {
            //不是数组直接构造
            dataArr.push({ name: key, value: val });
        }

        //根据数据构造的这个数组进行操作
        for (var m = 0, vObj; vObj = dataArr[m++];) {
            var element;
            //如果表单中无元素则跳过
            if (!(element = elementsJson[vObj.name])) {
                continue;
            }
            //判断元素是否为数组,暂时获取第一个元素，后面会有迭代赋值。
            var type = element.length ? element[0] : element;
            //元素类型
            type = (type.type || type.nodeName).toLowerCase();

            var property = null;
            //是单值类型
            if (rvalue.test(type)) {
                element.value = (typeof (vObj.value) == "undefined" || vObj.value == null) ? "" : vObj.value;
                //checkbox
            } else if (rcheck.test(type)) {
                property = "checked";
                //select
            } else if (rselect.test(type)) {
                property = "selected";
            }
            //判断类型是否为多选
            if (property) {
                //如果是，则迭代多选的元素赋值
                for (var n = 0, e; e = element[n++];) {
                    if (e.value == vObj.value) {
                        //设置选中
                        e[property] = true;
                    }
                }
            }
        }
    }
};
//如果数据为空，则设置一个值（默认值：暂无数据）
$.FormatEmpty = function (data, emptyMsg) {
    if (emptyMsg == undefined) { emptyMsg = "暂无数据"; }
    if (data == "null" || data == null || data == undefined || data == "") {
        return emptyMsg;
    } else {
        return data;
    }
}

//删除末尾符号
$.TrimEnd = function(str, char) {
    eval("var reg = /" + char + "$/gi;");
    return str.replace(reg, "");
}

var globalKey = {
    //InterfaceDomain: "https://wxapi.YiDaBus.com/",
    InterfaceDomain: "http://localhost:9000/",
    MobileUrl: "http://m.YiDaBus.com/",
}

//获取用户ID
$.GetUserId = function() {
    return $("#userId").val();
}

$.SetUserId = function (userId) {
    $("#userId").val(userId);
}
//根据OpenId获取UserId
$.GetUserIdByOpenId = function () {
    $.ar({
        url: '/MemberManager/Member/GetUserIdByOpenId'
        , data: null
        , success: function (data) {
            $.SetUserId(data.userId);
        }
    });
}