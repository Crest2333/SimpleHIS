$(function () {
    LoadDepartment()
})
let departmentId = 1;
let pageIndex = 1;
let pageSize = 10;
function Search(index) {
    pageIndex = index;
    var model = GetData(pageIndex);
    $.post(
        "/Department/GetSchedulingList",
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
        UseNo: $("#workNumber").val(),
        StartDate: $("#startDate").val(),
        EndDate: $("#endDate").val(),
        DepartmentId: $("#department").val(),
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

function openAdd() {
    if ($("#workNumber").val() == null || $("#workNumber").val() == "") {
        alert("请输入工号");
        return;
    }
    $("#addSchedulingModal").modal("show");

}

function GetAddData() {
    return {
        UserNo: $("#workNumber").val(),
        StartDate: $("#addStartDate").val(),
        EndDate: $("#addEndDate").val(),
        DepartmentId: $("#addDepartment").val(),
        SchedulingType: $("#SchedulingType").val()
    }
}

function AddScheduling() {
    var model = GetAddData();
    $.post(
        "/Department/AddScheduling",
        model,
        function (result) {
            if (result.isSuccess) {
                ShowTip("success", "添加成功");
            } else {
                ShowTip("warning", result.message);
            }
        }
    )
}

function Delete(id) {
    var isDelete = confirm("确认是否删除该条数据？");
    if (isDelete) {
        $.post(
            `/Department/DeleteScheduling?scheduling=${id}`,
            function (result) {
                if (result.isSuccess) {
                    ShowTip("success", "删除成功");

                } else {
                    ShowTip("warning", result.message);

                }
            }
        )
    }
}

function LoadDepartment() {
    $.get(
        `/Department/GetAllDepartment`,
        function (result) {
            if (result.isSuccess) {
                var html = template("departmentHtml", result);
                $("#addDepartment").html(html);
            }
        }
    )
}