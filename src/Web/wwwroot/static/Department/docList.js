$(function () {
    Search(1)

})
let departmentId = 1;
let pageIndex = 1;
let pageSize = 10;
function Search(index) {
    pageIndex = index;
    var model = GetData(pageIndex);
    $.post(
        "/Department/GetDepartmentDocList",
        model,
        function (res) {
            console.log(res)
            if (res.isSuccess) {
                if (res.result.count > 0) {
                    //var html = $("#listHtml").tmpl(res.result.list)
                    var html = template("listHtml", res.result);
                    console.log(html)
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
        Name: $("#name").val(),
        WorkNumber: $("#workNumber").val(),
        DepartmentId: $("#departmentId").val(),
        PageIndex: index || 1,
        PageSize: pageSize || 10
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


function Delete(id) {
    var isDelete = confirm("确认删除？");
    if (isDelete) {
        $.post(
            `/Department/DeleteDeportmentDoc?id=${id}`,
            function (result) {
                if (result.isSuccess) {
                    ShowTip('success', '删除成功');
                    Search(pageIndex);
                }
                else {
                    ShowTip('warning', result.Message);
                }
            }
        )
    }
}

function Import() {
    var departmentId = $("#departmentId").val();
    var file = $('#batchFile').prop("files");
    var data = new FormData()
    data.append("file", file[0]);
    $.post(
        `/Department/ImportDoctToDepartment?departmentId=${departmentId}`,
        data,
        function (result) {
            if (result.isSuccess) {
                ShowTip("success", result.Message);
            }
            else {
                ShowTip("warning", result.Message);

            }
            $("#BatchAddModal").modal("hide");
        }
    )
}

function ShowBatchAdd() {
    $("#AddModal").modal("show");
    SearchOther(1)
}


let pageOtherIndex = 1;
let pageOtherSize = 10;
function SearchOther(index) {
    pageOtherIndex = index;
    var model = GetOtherData(pageIndex);
    $.post(
        "/Department/GetUserInfoByDepartmentId",
        model,
        function (res) {
            console.log(res)
            if (res.isSuccess) {
                if (res.result.count > 0) {
                    //var html = $("#listHtml").tmpl(res.result.list)
                    var html = template("otherListHtml", res.result);
                    console.log(html)
                    if (pageIndex == 1) {
                        PageOtherTool(res.result.count);
                    }
                    $("#listOtherBody").html(html);
                }
                else {
                    $("#listOtherBody").html('<tr><td colspan="7">暂无数据</td></tr>')
                }
            }
        }
    )
}

function GetOtherData(index) {
    var data = {
        Name: $("#otherName").val(),
        WorkNumber: $("#otherWorkNumber").val(),
        DepartmentId: $("#departmentId").val(),
        PageIndex: pageOtherIndex || 1,
        PageSize: pageOtherSize || 10
    }
    return data;
}

function PageOtherTool(count) {
    layui.use('laypage', function () {
        var laypage = layui.laypage;
        //执行一个laypage实例
        laypage.render({
            elem: 'pageOther' //注意，这里的 test1 是 ID，不用加 # 号
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

function AddDoc(id) {
    var model = {
        DepartmentId: $("#departmentId").val(),
        UserId: id
    }
    $.post(
        "/Department/AddDepartmentPersonnel",
        model,
        function (result) {
            if (result.isSuccess) {
                SearchOther(pageOtherIndex)
                ShowTip("success", "添加成功");
            } else {
                ShowTip("warning", result.message);

            }
        }
    )
}

