var heightShowMore = '170px';
var inithHeight = '120px';
//监听回车查询事件
$(document).keyup(function (event) {
    if (event.keyCode == 13) {
        SearchHandler();
    }
});
//查询事件
function SearchHandler() {
    var $gridList = $("#gridList");
    $gridList.jqGrid('setGridParam', {
        postData: $('#SearchForm').formSerialize(),
    }).trigger('reloadGrid');
};
//显示更多查询条件
function ShowMoreSearchHandler(obj) {
    $('#TbMoreSeach').show();
    $(obj).attr('onclick', 'HideMoreSearchHandler(this)');
}
//隐藏查询条件
function HideMoreSearchHandler(obj) {
    $('#TbMoreSeach').hide();
    $(obj).attr('onclick', 'ShowMoreSearchHandler(this)');
}