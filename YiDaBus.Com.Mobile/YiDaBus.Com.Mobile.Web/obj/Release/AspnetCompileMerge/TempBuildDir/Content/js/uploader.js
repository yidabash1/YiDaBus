﻿$(function () {
    //初始化绑定默认的属性
    $.upLoadDefaults = $.upLoadDefaults || {};
    $.upLoadDefaults.property = {
        multiple: false, //是否多文件
        water: false, //是否加水印
        thumbnail: true, //是否生成缩略图
        sendurl: globalKey.InterfaceDomain + 'api/upload/Upload?isThumbnail=true', //发送地址
        filetypes: "jpg,jpge,png,mp4", //文件类型
        filesize: "20480000", //文件大小
        btntext: "", //上传按钮的文字
        swf: null //SWF上传控件相对地址
    };
    //初始化上传控件
    $.fn.InitUploader = function (b) {
        var fun = function (parentObj) {
            var p = $.extend({}, $.upLoadDefaults.property, b || {});
            var btnObj = $('<div class="upload-btn">' + p.btntext + '</div>').appendTo(parentObj);
            //初始化WebUploader
            var uploader = WebUploader.create({
                compress: false, //不压缩
                auto: true, //自动上传
                swf: p.swf, //SWF路径
                server: p.sendurl, //上传地址
                pick: {
                    id: btnObj,
                    multiple: p.multiple
                },
                accept: {
                    /*title: 'Images',*/
                    extensions: p.filetypes.toLowerCase()
                    /*mimeTypes: 'image/*'*/
                },
                formData: {
                    'DelFilePath': '' //定义参数
                },
                fileVal: 'Filedata', //上传域的名称
                fileSingleSizeLimit: p.filesize * 1024 //文件大小
            });

            //当validate不通过时，会以派送错误事件的形式通知
            uploader.on('error', function (type) {
                switch (type) {
                    case 'Q_EXCEED_NUM_LIMIT':
                        alert("错误：上传文件数量过多！");
                        break;
                    case 'Q_EXCEED_SIZE_LIMIT':
                        alert("错误：文件总大小超出限制！");
                        break;
                    case 'F_EXCEED_SIZE':
                        alert("错误：文件大小超出限制！");
                        break;
                    case 'Q_TYPE_DENIED':
                        alert("错误：禁止上传该类型文件！");
                        break;
                    case 'F_DUPLICATE':
                        alert("错误：请勿重复上传该文件！");
                        break;
                    default:
                        alert('错误代码：' + type);
                        break;
                }
            });

            //当有文件添加进来的时候
            uploader.on('fileQueued', function (file) {
                if ($(".upload_img ul li .addImg").length >= 6) {
                    $.modalAlert("只支持6张图片哦！")
                    uploader.cancelFile(file);
                    return;
                }
                //如果是单文件上传，把旧的文件地址传过去
                if (!p.multiple) {
                    uploader.options.formData.DelFilePath = parentObj.siblings(".upload-path").val();
                }
                //防止重复创建
                if (parentObj.children(".upload-progress").length == 0) {
                    //创建进度条
                    var fileProgressObj = $('<div class="upload-progress"></div>').appendTo(parentObj);
                    var progressText = $('<span class="txt"></span>').appendTo(fileProgressObj);
                    var progressBar = $('<span class="bar"><b></b></span>').appendTo(fileProgressObj);
                    var progressCancel = $('<a class="close" title="取消上传"></a>').appendTo(fileProgressObj);
                    //绑定点击事件
                    progressCancel.click(function () {
                        uploader.cancelFile(file);
                        fileProgressObj.remove();
                    });
                }
            });

            //文件上传过程中创建进度条实时显示
            uploader.on('uploadProgress', function (file, percentage) {
                var progressObj = parentObj.children(".upload-progress");
                progressObj.children(".txt").html(file.name);
                progressObj.find(".bar b").width(percentage * 100 + "%");
            });

            //当文件上传出错时触发
            uploader.on('uploadError', function (file, reason) {
                uploader.removeFile(file); //从队列中移除
                alert(file.name + "上传失败，错误代码：" + reason);
            });

            //当文件上传成功时触发
            uploader.on('uploadSuccess', function (file, data) {
                if (data.status == '0') {
                    var progressObj = parentObj.children(".upload-progress");
                    progressObj.children(".txt").html(data.msg);
                }
                if (data.code == 200) {
                    //如果是单文件上传，则赋值相应的表单
                    if (!p.multiple) {
                        $("#uploader-demo").parent().before("<li onclick='clickImg(this)'><img class='addImg' src='" + data.data[0].path + "'></li>");
                     
                    } else {
                        addImage(parentObj, data.path, data.thumb, data.name);
                    }
                    var progressObj = parentObj.children(".upload-progress");
                    progressObj.children(".txt").html("上传成功：" + file.name);
                }
                uploader.removeFile(file); //从队列中移除
            });

            //不管成功或者失败，文件上传完成时触发
            uploader.on('uploadComplete', function (file) {
                var progressObj = parentObj.children(".upload-progress");
                progressObj.children(".txt").html("上传完成");
                //如果队列为空，则移除进度条
                if (uploader.getStats().queueNum == 0) {
                    progressObj.remove();
                }
            });
        };
        return $(this).each(function () {
            fun($(this));
        });
    }
});

/*图片相册处理事件
=====================================================*/
//添加图片相册
function addImage(targetObj, originalSrc, thumbSrc, fileName) {
    //插入到相册UL里面
    var newLi = $('<li>'
    + '<input type="hidden" id="hid_photo_name" name="hid_photo_name" value="0|' + originalSrc + '|' + thumbSrc + '|' + fileName + '" />'
    + '<input type="hidden" id="hid_photo_remark" name="hid_photo_remark" value="" />'
    + '<div class="img-box" onclick="setFocusImg(this);">'
    + '<img src="' + thumbSrc + '" bigsrc="' + originalSrc + '"/>'
    + '<span class="remark"><i>暂无描述...</i></span>'
    + '</div>'
    + '<a href="javascript:;" onclick="setRemark(this);">描述</a>'
    + '<a href="javascript:;" onclick="delImg(this);">删除</a>'
    + '<a href="/Upload/DownLoadFile?filePath=' + thumbSrc + '&fileName=' + fileName + '" >下载</a>'
    + '</li>');
    newLi.appendTo(targetObj.siblings(".photo-list").children("ul"));

    //默认第一个为相册封面
    var focusPhotoObj = targetObj.siblings(".focus-photo");
    if (focusPhotoObj.val() == "") {
        focusPhotoObj.val(thumbSrc);
        newLi.children(".img-box").addClass("selected");
    }
}
//设置相册封面
function setFocusImg(obj) {
    var focusPhotoObj = $(obj).parents(".photo-list").siblings(".focus-photo");
    focusPhotoObj.val($(obj).children("img").eq(0).attr("src"));
    $(obj).parent().siblings().children(".img-box").removeClass("selected");
    $(obj).addClass("selected");
}
//设置图片描述
function setRemark(obj) {
    var parentObj = $(obj); //父对象
    var hidRemarkObj = parentObj.prevAll("input[name='hid_photo_remark']").eq(0); //取得隐藏值
    //自定页
    var objIndex = top.layer.open({
        title: "描述",
        type: 1,
        skin: 'layui-layer-demo', //样式类名
        closeBtn: 0, //不显示关闭按钮
        shift: 2,
        shadeClose: true, //开启遮罩关闭
        btn: ['单张描述', '批量描述', '关闭'],
        btnclass: ['btn btn-primary', 'btn btn-primary', 'btn btn-danger'],
        content: '<textarea id="ImageRemark" style="margin:20px;font-size:12px;padding:3px;color:#000;border:1px #d2d2d2 solid;vertical-align:middle;width:300px;height:50px;">' + hidRemarkObj.val() + '</textarea>',
        yes: function () {
            var remarkObj = $('#ImageRemark', parent.document);
            if (remarkObj.val() == "") {
                $.modalMsg("亲，总该写点什么吧！", 2);
                return false;
            }
            hidRemarkObj.val(remarkObj.val());
            parentObj.siblings(".img-box").children(".remark").children("i").html(remarkObj.val());
            top.layer.close(objIndex);
            return true;
        }, btn2: function () {
            var remarkObj = $('#ImageRemark', parent.document);
            if (remarkObj.val() == "") {
                parent.dialog({
                    title: '提示',
                    content: '亲，总该写点什么吧？',
                    okValue: '确定',
                    ok: function () { },
                    onclose: function () {
                        remarkObj.focus();
                    }
                }).showModal();
                return false;
            }
            parentObj.parent().parent().find("li input[name='hid_photo_remark']").val(remarkObj.val());
            parentObj.parent().parent().find("li .img-box .remark i").html(remarkObj.val());
            top.layer.close(objIndex);
            return true;
        }, btn3: function () {
            return true;
        }
    });
}
//删除图片LI节点
function delImg(obj) {
    var parentObj = $(obj).parent().parent();
    var focusPhotoObj = parentObj.parent().siblings(".focus-photo");
    var smallImg = $(obj).siblings(".img-box").children("img").attr("src");
    $(obj).parent().remove(); //删除的LI节点
    //检查是否为封面
    if (focusPhotoObj.val() == smallImg) {
        focusPhotoObj.val("");
        var firtImgBox = parentObj.find("li .img-box").eq(0); //取第一张做为封面
        firtImgBox.addClass("selected");
        focusPhotoObj.val(firtImgBox.children("img").attr("src")); //重新给封面的隐藏域赋值
    }
}
//下载文件
function downFile(fileName) {
    $.submitForm({
        url: "/Upload/DownLoadFile?fileName=" + fileName,
        success: function () {
        }
    })
}