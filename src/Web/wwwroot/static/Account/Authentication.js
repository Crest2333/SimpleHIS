$(function () {

})

let index = 1;

let size = 10;

function Login() {
    var model = {
        AccountNo: $("#workNo").val(),
        PassWord: $("#passWord").val(),
        RememberMe: false
    }
    $.post(
        "/Account/Authentication",
        model,
        function (res) {
            console.log(res);
            if (res.IsSuccess) {
                if (res.Result) {
                    Tip('success', res.Message)
                }
                else {
                    Tip('danger', res.Message)
                }
            }
            else {
                Tip('danger', "网络错误")
            }
        }
    )
}
