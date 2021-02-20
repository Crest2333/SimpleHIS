$(function () {
    GetList(1)
})

let pageIndex = 1;

let size = 10;

function GetList(index) {
    pageIndex = index;
    var model = GetData(pageIndex);
    $.post(
        "/User/GetUserInfoList",
        model,
        function (res) {
            console.log(res)
            if (res.isSuccess) {
                if (res.result.count > 0) {
                    //var html = $("#listHtml").tmpl(res.result.list)
                    var html = template("listHtml", res.result);
                    if (pageIndex == 1) {
                        PageTool(res.result.count);
                    }

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
            , limit: 10
            , jump: function (obj, first) {
                //obj包含了当前分页的所有参数，比如：
                //首次不执行
                console.log(obj.curr)
                console.log(first);
                if (!first) {
                    GetList(obj.curr)
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
                    ShowTip('warning', res.Message);
                }
                $("#batchAddUser").modal("hide")
            }

        }
    )
}

function test() {
    Tip('success', '添加成功')
}

function ResetPassWord(userId) {
    $.ajax({
        method: 'put',
        url: '/User/ResetPassWord',
        data: { userId: userId },
        success: function (res) {
            if (res.isSuccess) {
                ShowTip('success', '重置成功');
            }
            else {
                ShowTip('warning', res.Message);
            }
        }
    })
}

function Delete() {
    $.ajax({
        method: 'put',
        url: '/User/ResetPassWord',
        data: { userId: userId },
        success: function (res) {
            if (res.isSuccess) {
                ShowTip('success', '重置成功');
            }
            else {
                ShowTip('warning', res.Message);
            }
        }
    })
}

function Export() {
    Loading('请稍后');
    $.ajax({
        method: 'get',
        url: '/User/ExportUserInfo',
        responseType: 'arraybuffer',
        success: function (res) {
            console.log(res);
            const url = window.URL.createObjectURL(new Blob([res]))
            const link = document.createElement('a')
            link.style.display = 'none'
            link.href = url
            link.setAttribute('download', 'excel.xlsx')
            document.body.appendChild(link)
            link.click()
            document.body.removeChild(link)
        }
    })
}

let userId;

function OpenRoleModal(id) {
    $("#roleModal").modal("show");
    userId = id;
    InitRole();
}

function InitRole() {
    $.get(
        "/Role/GetAllRole",
        function (result) {
            var html = template("roleHtml", result);
            $("#roleDiv").html(html);
        }
    )
}

function AddRole() {
    var model = {
        UserId: userId,
        RoleId: $("input[name='role']:checked").val()
    }
    $.post(
        "/Role/AddOrEditRole",
        model,
        function (result) {
            if (result.isSuccess) {
                ShowTip('success', '添加成功');
            }
            else {
                ShowTip('warning', result.message);
            }
        }
    )
}



