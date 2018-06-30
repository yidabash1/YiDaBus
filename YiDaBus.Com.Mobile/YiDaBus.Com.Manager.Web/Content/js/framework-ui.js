$(function () {
    document.body.className = localStorage.getItem('config-skin');
    $("[data-toggle='tooltip']").tooltip();
})
$.reload = function () {
    location.reload();
    return false;
}
$.loading = function (bool, text) {
    var $loadingpage = top.$("#loadingPage");
    var $loadingtext = $loadingpage.find('.loading-content');
    if (bool) {
        $loadingpage.show();
    } else {
        if ($loadingtext.attr('istableloading') == undefined) {
            $loadingpage.hide();
        }
    }
    if (!!text) {
        $loadingtext.html(text);
    } else {
        $loadingtext.html("数据加载中，请稍后…");
    }
    $loadingtext.css("left", (top.$('body').width() - $loadingtext.width()) / 2 - 50);
    $loadingtext.css("top", (top.$('body').height() - $loadingtext.height()) / 2);
}
$.request = function (name) {
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
$.currentWindow = function () {
    var iframeId = top.$(".NFine_iframe:visible").attr("id");
    return top.frames[iframeId];
}
$.browser = function () {
    var userAgent = navigator.userAgent;
    var isOpera = userAgent.indexOf("Opera") > -1;
    if (isOpera) {
        return "Opera"
    };
    if (userAgent.indexOf("Firefox") > -1) {
        return "FF";
    }
    if (userAgent.indexOf("Chrome") > -1) {
        if (window.navigator.webkitPersistentStorage.toString().indexOf('DeprecatedStorageQuota') > -1) {
            return "Chrome";
        } else {
            return "360";
        }
    }
    if (userAgent.indexOf("Safari") > -1) {
        return "Safari";
    }
    if (userAgent.indexOf("compatible") > -1 && userAgent.indexOf("MSIE") > -1 && !isOpera) {
        return "IE";
    };
}
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
$.downFile = function (options) {
    var defaults = {
        url: "",
        data: options.data,
        method: "POST",
    };
    var options = $.extend(defaults, options);
    if (options.url && options.data) {
        var inputs = '';
        console.log(options.data);
        for (var key in options.data) {
            inputs += '<input type="hidden" name="' + key + '" value="' + options.data[key] + '" />';
        }
        console.log(inputs);
        $('<form action="' + options.url + '" method="' + options.method + '">' + inputs + '</form>').appendTo('body').submit().remove();
    };
};
$.modalOpen = function (options) {
    var defaults = {
        id: null,
        title: '系统窗口',
        width: "100px",
        height: "100px",
        url: '',
        shade: 0.3,
        btn: ['确认', '关闭'],
        btnclass: ['btn btn-primary', 'btn btn-danger'],
        maxmin: false,
        callBack: null
    };
    var options = $.extend(defaults, options);
    var _width = top.$(window).width() > parseInt(options.width.replace('px', '')) ? options.width : top.$(window).width() + 'px';
    var _height = top.$(window).height() > parseInt(options.height.replace('px', '')) ? options.height : top.$(window).height() + 'px';
    var index = top.layer.open({
        id: options.id,
        type: 2,
        shade: options.shade,
        title: options.title,
        fix: false,
        area: [_width, _height],
        content: options.url,
        btn: options.btn,
        btnclass: options.btnclass,
        //maxmin: options.maxmin,
        maxmin: true,
        yes: function () {
            options.callBack(options.id)
        }, cancel: function () {
            return true;
        }
    });
    //if (options.maxmin) {
    //    top.layer.full(index);
    //}
}
$.modalConfirm = function (content, callBack) {
    top.layer.confirm(content, {
        icon: "fa-exclamation-circle",
        title: "系统提示",
        btn: ['确认'],
        btnclass: ['btn btn-primary', 'btn btn-danger'],
    }, function () {
        callBack(true);
    }, function () {
        callBack(false)
    });
}
$.modalAlert = function (content, type) {
    var icon = "";
    if (type == 1) {
        icon = "fa-check-circle";
    }
    if (type == 0) {
        icon = "fa-times-circle";
    }
    if (type == 2) {
        icon = "fa-exclamation-circle";
    }
    top.layer.alert(content, {
        icon: icon,
        title: "系统提示",
        btn: ['确认'],
        btnclass: ['btn btn-primary'],
    });
}
$.modalMsg = function (content, type) {
    if (type != undefined) {
        var icon = "";
        if (type == 1) {
            type = "success";
            icon = "fa-check-circle";
        }
        if (type == 0) {
            type = "error";
            icon = "fa-times-circle";
        }
        if (type == 2) {
            type = "warning";
            icon = "fa-exclamation-circle";
        }
        top.layer.msg(content, { icon: icon, time: 4000, shift: 5 });
        top.$(".layui-layer-msg").find('i.' + icon).parents('.layui-layer-msg').addClass('layui-layer-msg-' + type);
    } else {
        top.layer.msg(content);
    }
}
$.modalClose = function () {
    var index = top.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
    var $IsdialogClose = top.$("#layui-layer" + index).find('.layui-layer-btn').find("#IsdialogClose");
    var IsClose = $IsdialogClose.is(":checked");
    if ($IsdialogClose.length == 0) {
        IsClose = true;
    }
    if (IsClose) {
        top.layer.close(index);
    } else {
        location.reload();
    }
}
$.submitForm = function (options) {
    var defaults = {
        url: "",
        param: [],
        loading: "正在提交数据...",
        success: null,
        close: true
    };
    var options = $.extend(defaults, options);
    $.loading(true, options.loading);
    window.setTimeout(function () {
        if ($('[name=__RequestVerificationToken]').length > 0) {
            options.param["__RequestVerificationToken"] = $('[name=__RequestVerificationToken]').val();
        }
        $.ajax({
            url: options.url,
            data: options.param,
            type: "post",
            dataType: "json",
            success: function (data) {
                if (data.status == 1) {
                    options.success(data);
                    $.modalMsg(data.msg, data.status);
                    if (options.close == true) {
                        $.modalClose();
                    }
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
$.jsonWhere = function (data, action) {
    if (action == null) return;
    var reval = new Array();
    $(data).each(function (i, v) {
        if (action(v)) {
            reval.push(v);
        }
    })
    return reval;
}
$.fn.jqGridRowValue = function () {
    var $grid = $(this);
    var selectedRowIds = $grid.jqGrid("getGridParam", "selarrrow");
    if (selectedRowIds != "") {
        var json = [];
        var len = selectedRowIds.length;
        for (var i = 0; i < len ; i++) {
            var rowData = $grid.jqGrid('getRowData', selectedRowIds[i]);
            json.push(rowData);
        }
        return json;
    } else {
        return $grid.jqGrid('getRowData', $grid.jqGrid('getGridParam', 'selrow'));
    }
}
$.fn.formValid = function () {
    return $(this).valid({
        errorPlacement: function (error, element) {
            element.parents('.formValue').addClass('has-error');
            element.parents('.has-error').find('i.error').remove();
            element.parents('.has-error').append('<i class="form-control-feedback fa fa-exclamation-circle error" data-placement="left" data-toggle="tooltip" title="' + error + '"></i>');
            $("[data-toggle='tooltip']").tooltip();
            if (element.parents('.input-group').hasClass('input-group')) {
                element.parents('.has-error').find('i.error').css('right', '33px')
            }
        },
        success: function (element) {
            element.parents('.has-error').find('i.error').remove();
            element.parent().removeClass('has-error');
        }
    });
}
$.fn.formSerialize = function (formdate) {
    var element = $(this);
    if (!!formdate) {
        for (var key in formdate) {
            var $id = element.find('#' + key);
            var value = $.trim(formdate[key]).replace(/&nbsp;/g, '');
            var type = $id.attr('type');
            if ($id.hasClass("select2-hidden-accessible")) {
                type = "select";
            }
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
                //if (typeof postdata[id] === 'object' && postdata[id] == object.constructor) {

                //} else {
                //    postdata[id] = $this.val();
                //}
                postdata[id] = $this.val();
                break;
        }
    });
    if ($('[name=__RequestVerificationToken]').length > 0) {
        postdata["__RequestVerificationToken"] = $('[name=__RequestVerificationToken]').val();
    }
    return postdata;
};
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
$.fn.authorizeButton = function () {
    var moduleId = top.$(".NFine_iframe:visible").attr("id").substr(6);
    var dataJson = top.clients.authorizeButton[moduleId];
    var $element = $(this);
    $element.find('a[authorize=yes]').attr('authorize', 'no');
    if (dataJson != undefined) {
        $.each(dataJson, function (i) {
            $element.find("#" + dataJson[i].F_EnCode).attr('authorize', 'yes');
        });
    }
    $element.find("[authorize=no]").parents('li').prev('.split').remove();
    $element.find("[authorize=no]").parents('li').remove();
    $element.find('[authorize=no]').remove();
}
$.fn.dataGrid = function (options) {
    var defaults = {
        datatype: "json",
        autowidth: true,
        rownumbers: true,
        shrinkToFit: false,
        gridview: true
    };
    var options = $.extend(defaults, options);
    var $element = $(this);
    options["onSelectRow"] = function (rowid) {
        var length = $(this).jqGrid("getGridParam", "selrow").length;
        var $operate = $(".operate");
        if (length > 0) {
            $operate.animate({ "left": 0 }, 200);
        } else {
            $operate.animate({ "left": '-100.1%' }, 200);
        }
        $operate.find('.close').click(function () {
            $operate.animate({ "left": '-100.1%' }, 200);
        })
    };
    $element.jqGrid(options);
};
//跳转到指定模块菜单
$.OpenNav = function (Navid, options) {
    var defaults = {
        param: ""
    };
    var options = $.extend(defaults, options);
    var $a = top.$("#nav-col").find('a[data-id="' + Navid + '"]');
    $a.attr('href', $a.attr('href') + "?" + options.param).trigger("click");
}
$.RemoveNavParam = function (Navid) {
    var $a = top.$("#nav-col").find('a[data-id="' + Navid + '"]');
    $a.attr('href', $a.attr('href').split('?')[0]);
}
$.modalOpenInCurrent = function (options) {
    var defaults = {
        id: null,
        title: '系统窗口',
        width: "100px",
        height: "100px",
        url: '',
        shade: 0.3,
        btn: ['确认', '关闭'],
        btnclass: ['btn btn-primary', 'btn btn-danger'],
        maxmin: false,
        callBack: null
    };
    var options = $.extend(defaults, options);
    var _width = $(window).width() > parseInt(options.width.replace('px', '')) ? options.width : top.$(window).width() + 'px';
    var _height = $(window).height() > parseInt(options.height.replace('px', '')) ? options.height : top.$(window).height() + 'px';
    var index = layer.open({
        id: options.id,
        type: 2,
        shade: options.shade,
        title: options.title,
        fix: false,
        area: [_width, _height],
        content: options.url,
        btn: options.btn,
        btnclass: options.btnclass,
        maxmin: options.maxmin,
        // maxmin: true,
        yes: function () {
            options.callBack(options.id)
        }, cancel: function () {
            return true;
        }, end: function () {
            options.end(options.id)
        }
    });
    //if (options.maxmin) {
    //    top.layer.full(index);
    //}
}
$.modalCloseInCurrent = function () {
    var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
    parent.layer.close(index); //再执行关闭   
}
$.GetSelectValue = function (obj, ValueElement) {
    var strJson = $(ValueElement).val();
    if (strJson == '') {
        $(obj).removeClass('btn-success').addClass('btn-danger').html('<i class="fa fa-arrows"></i>亲~选择失败，请重新选择');
        return '';
    }
    $(obj).removeClass('btn-danger').addClass('btn-success').html('<i class="fa fa-arrows"></i>重新选择');
    return strJson;
}
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
//判断查询条件是否全部为空，如果全部为空则返回false；否则返回ture；
$.fn.IsFormValueAllEmpty = function () {
    var count = 0;
    var a = $('#SearchForm').serializeArray();
    $.each(a, function () {
        if (this.value == '') {
            count++;
        }
    });
    if (count == a.length) {
        return false;
    } else {
        return true;
    }
}
//定义了菜单栏中的data-id；
var menuItemKey = {
    HOUSEADD: "38ce14b5-b38a-44f2-8c00-5a6c7f575a8a",
    USAERADD: "160eac20-318a-4aea-b4df-4066c0799dec",
    DEMAND: "62dfc595-2b5e-4150-9a91-60b2036007af"
}
//城市控件（单选或多选）
$.fn.GetSelectCity = function (options) {
    var defaults = {
        multiple: true
    };
    var options = $.extend(defaults, options);
    var $this = $(this);
    cityMYDATA = sessionStorage.getItem("cityselectData");
    if (cityMYDATA == null) {
        //所属城市
        $.ajax({
            url: "/SystemManage/Area/GetTreeSelectJson1",
            dataType: "json",
            async: false,
            success: function (MYDATA) {
                sessionStorage.setItem("cityselectData", JSON.stringify(MYDATA));
                $this.select2({
                    placeholder: "请选择城市",
                    multiple: options.multiple,
                    allowClear: true,
                    data: MYDATA
                });
            }
        });
    } else {
        $this.select2({
            placeholder: "请选择城市",
            multiple: options.multiple,
            allowClear: true,
            data: JSON.parse(cityMYDATA)
        });
    }
}
//新增用户弹窗
$.OpenAddUserModal = function () {
    $.modalOpen({
        id: "UserEditForm",
        title: "新增用户",
        url: "/UserManage/User/edituser",
        width: "800px",
        height: "600px",
        callBack: function (iframeId) {
        }
    });
}
//用户控件（单选或多选）
$.fn.GetSelectUser = function (options) {
    var defaults = {
        multiple: true
    };
    var options = $.extend(defaults, options);
    var $this = $(this);
    $.ajax({
        url: "/UserManage/User/GetSelectJson",
        dataType: "json",
        async: false,
        success: function (MYDATA) {
            sessionStorage.setItem("userselectData", JSON.stringify(MYDATA));
            $this.select2({
                placeholder: "请选择用户",
                multiple: options.multiple,
                allowClear: true,
                data: MYDATA
            });
        }
    });
}
//小区控件（单选）
$.fn.GetSelectVillage = function (options) {
    var defaults = {
        multiple: true
        , placeholder: null
    };
    var options = $.extend(defaults, options);
    var $this = $(this);
    $.ajax({
        url: "/housevillage/village/GetSelectJson",
        dataType: "json",
        async: false,
        success: function (MYDATA) {
            sessionStorage.setItem("villageselectData", JSON.stringify(MYDATA));
            $this.select2({
                placeholder: options.placeholder || "请选择用户",
                multiple: options.multiple,
                allowClear: true,
                data: MYDATA
            });
        }
    });
}//房源控件（单选）
$.fn.GetSelectHouse = function (options) {
    var defaults = {
        multiple: true
    };
    var options = $.extend(defaults, options);
    var $this = $(this);
    $.ajax({
        url: "/housemanage/house/GetSelectJson",
        dataType: "json",
        async: false,
        success: function (MYDATA) {
            sessionStorage.setItem("houseselectData", JSON.stringify(MYDATA));
            $this.select2({
                placeholder: "请选择用户",
                multiple: options.multiple,
                allowClear: true,
                data: MYDATA
            });
        }
    });
}
//获取用户详细（模态框形式）
$.GetUserDetailModal = function (userid) {
    $.modalOpen({
        id: "UserDetailModal",
        title: "用户详细",
        url: "/CommonPageManage/User/UserDetail?userId=" + userid + "&action=onlyGetUserDetail",
        width: "900px",
        height: "600px",
        btn: null
    });
}
//获取房源详细（模态框形式）
$.GetHouseDetailModal = function (houseid) {
    $.modalOpen({
        id: "HouseDetailModal",
        title: "房源详细",
        url: "/CommonPageManage/House/HouseDetail?houseId=" + houseid,
        width: "1000px",
        height: "700px",
        btn: null
    });
}
$.fn.GetTagsSelectJson = function (options) {
    var defaults = {
        multiple: true
    };
    var options = $.extend(defaults, options);
    var $this = $(this);
    $.ajax({
        url: "/SystemManage/itemsdata/GetTreeSelectJson",
        data: { enCode: "HouseLabel" },
        dataType: "json",
        async: false,
        success: function (MYDATA) {
            sessionStorage.setItem("tagsselectData", JSON.stringify(MYDATA));
            $this.select2({
                placeholder: "请选择标签",
                multiple: options.multiple,
                allowClear: true,
                data: MYDATA
            });
        }
    });
}
$.fn.formImgSerialize = function () {
    var element = $(this);
    var postdata = element.serializeArray();
    if ($('[name=__RequestVerificationToken]').length > 0) {
        postdata.push({ name: "__RequestVerificationToken", value: $('[name=__RequestVerificationToken]').val() });
    }
    return postdata;
};


$.CheckMobileNo = function (mobile) {
    return !!mobile.match(/^(13[0-9]|15[012356789]|17[678]|18[0-9]|14[57])[0-9]{8}$/);
};


