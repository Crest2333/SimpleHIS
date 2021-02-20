$(function () {

});
let pageIndex = 1;
let pageSize = 10;
function Search(index) {
    pageIndex = index;
    var model = GetData(pageIndex);
    $.post(
        "/PA/GetAppointmentList",
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
        PatientName: $("#PatientName").val(),
        PhoneNumber: $("#PhoneNumber").val(),
        StartDate: $("#StartDate").val(),
        EndDate: $("#EndDate").val(),
        DoctorName: $("#DoctorName").val(),
        DepartmentId: $("#DepartmentId").val(),
        Status: $("#Status").val(),
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