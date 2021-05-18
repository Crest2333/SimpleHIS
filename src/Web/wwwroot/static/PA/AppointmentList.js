$(function () {
    LoadDepartment()

    layui.use("laydate",

        function () {
            var laydate = layui.laydate;
            var start = laydate.render({
                elem: '#StartDate' //指定元素
                ,
                format: 'yyyy-MM-dd',
                done: function (value, date, endDate) {
                    //结束时间的最小时间
                    end.config.min = {
                        year: date.year,
                        month: date.month - 1,
                        date: date.date,
                        hours: date.hours,
                        minutes: date.minutes,
                        seconds: date.seconds
                    }
                }
            });

            var end = laydate.render({
                elem: '#EndDate' //指定元素
                ,
                format: 'yyyy-MM-dd',

                done: function (value, date, endDate) {
                    if (value === '' || value === null) {
                        //清空时，开始时间的最大时间是当前时间
                        var nowDate = new Date();
                        start.config.max = {
                            year: nowDate.getFullYear(),
                            month: nowDate.getMonth(),
                            date: nowDate.getDate(),
                            hours: nowDate.getHours(),
                            minutes: nowDate.getMinutes(),
                            seconds: nowDate.getSeconds()
                        };
                        return;
                    }
                    //开始时间的最大时间
                    start.config.max = {
                        year: date.year,
                        month: date.month - 1,
                        date: date.date,
                        hours: date.hours,
                        minutes: date.minutes,
                        seconds: date.seconds
                    }
                }
            });
        });
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
                    Search(obj.curr)
                    //do something
                }
            }
        });
    });
}

let appointment;

function Cancel(id) {
    $.post(
        `/Pa/DeleteAppointment?appointmentId=${id}`,
        function (result) {
            if (result.isSuccess) {
                ShowTip("success", "操作成功");

            } else {
                ShowTip("warning", result.message);

            }
        }
    )
}

function ChangeStatus(appointmentId, status) {
    if (confirm("确认该操作?")) {
        $.post(
            `/PA/ChangeAppointmentStatus`,
            { AppointmentId: appointmentId, Status: status },
            function (result) {
                if (result.isSuccess) {
                    ShowTip("success", "操作成功")
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
                $("#DepartmentId").html(html);
            }
        }
    )
}