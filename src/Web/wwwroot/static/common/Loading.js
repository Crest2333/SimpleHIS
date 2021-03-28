$(function () {
    AjaxLoad();
    AjaxEndLoad();
    AjaxError();
})

function AjaxLoad() {
    $(document).ajaxSend(function (e, xhr, opt) {
        Loading()
    });
}

function AjaxEndLoad() {
    $(document).ajaxComplete(function (e, xhr, opt) {
        setTimeout(function () {
            RemoveLoading()
        },300)
    });
}

function AjaxError() {
    $(document).ajaxError(function (e, xhr, opt) {
        setTimeout(function () {
            RemoveLoading()
            ShowTip('warning', '网络错误')
        }, 300)
    });
}

function Loading() {
    $("#loadModal").modal('show')
}
function RemoveLoading() {
    $("#loadModal").modal('hide')
}