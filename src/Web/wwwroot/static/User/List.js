$(function () {
    GetList(1)
})

let index = 1;

let size = 10;


function GetList(index) {
    var model = GetData(index);
    $.post(
        "/User/GetUserInfoList",
        model,
        function (res) {
            console.log(res)
            if (res.isSuccess) {
                if (res.result.count > 0) {
                    //var html = $("#listHtml").tmpl(res.result.list)
                    var html = template("listHtml", res.result);
                    PageTool(res.result.count);
                    $("#listBody").html(html);
                }
                else {
                    $("#listBody").html('<tr><td colspan="7">暂无数据</td></tr>')
                }
            }
        }
    )
}


function GetData(index) {
    var data = {
        PageIndex: index || 1,
        PageSize: size || 10
    }

    return data;
}


function PageTool(count) {
    layui.use('laypage', function () {
        var laypage = layui.laypage;

        //执行一个laypage实例
        laypage.render({
            elem: 'page' //注意，这里的 test1 是 ID，不用加 # 号
            , count: count //数据总数，从服务端得到
            , limit: 20
            , jump: function (obj, first) {
                //obj包含了当前分页的所有参数，比如：

                //首次不执行
                if (!first) {
                    //do something
                }
            }
        });
    });

}


function AddUser() {
    var model = GetAddUserInfo();
    $.post(
        "AddUser",
        model,
        function (res) {
            console.log(res)
        }
    )
}

function GetAddUserInfo() {
    var data = {
        Name: $("#uname").val(),
        Gender: $("#gender").val(),
        PhoneNumber: $("#phone").val(),
        Email: $("#email").val(),
        Identity: $("#identity").val()
    }
    return data;
}


function ShowAddUserPage() {
    ClearInput();
    $("#exampleModal").modal("show")
}

function ClearInput() {
    $("#uname").val(null);
    $("#gender").val(1);
    $("#phone").val(null);
    $("#email").val(null);
    $("#identity").val(null)
}

function ShowBatchAddUserPage() {
    $("#batchAddUser").modal("show")
}

function BatchAddUser() {
    var file = $('#batchFile').prop("files");

    var data = new FormData()

    data.append("file", file[0]);
    $.ajax(
        {
            method: 'post',
            url: "BatchAddUser",
            data: data,
            cache: false,
            processData: false,
            contentType: false,
            success: function (res) {
                if (res.isSuccess) {
                    ShowTip('success', '添加成功')
                }
                else {
                    ShowTip('warning', '添加成功')
                }
            }

        }
    )
}
function test() {
    Tip('success', '添加成功')
}
