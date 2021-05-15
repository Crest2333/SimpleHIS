let patientId;

$(function () {
    var request = GetRequest();
    patientId = request["patientId"];
    $("#wizard").steps({
        headerTag: "h2",
        bodyTag: "section",
        transitionEffect: "slideLeft",
        labels: {
            finish: "完成", // 修改按钮得文本
            next: "下一步", // 下一步按钮的文本
            previous: "上一步", // 上一步按钮的文本
            loading: "Loading ..."
        },
        onStepChanging: function (event, currentIndex, newIndex) {// 下一步切换时的监听
            if (currentIndex == 0 && newIndex == 1) {
                var departmentId = $("input[name='department']:checked").val();
                console.log(departmentId);
                if (staticDepartmentId != departmentId) {
                    staticDepartmentId = departmentId;
                    LoadDoctor(departmentId);
                }
            }

            if (currentIndex == 1 && newIndex == 2) {
                var doctorChecked = $("input[name='doctor']:checked").val();
                if (doctorChecked == null) {
                    ShowTip("warning", "请选择一名医生");
                    return;
                }
            }

            if (currentIndex == 2 && newIndex == 3) {
                if ($("input[name='appointmentTime']:checked").val() == null) {
                    ShowTip("warning", "请选择一个时间");
                    return;
                }
                
            }


            return true;
        },
        //onStepChanged: function (event, currentIndex, priorIndex) {// 下一步切换完成得监听

        //},
        //onCanceled: function (event) {// 取消按钮得监听

        //},
        onFinishing: function (event, currentIndex) {// 完成时得监听
            // form.validate().settings.ignore = ":disabled";
            Add();
            return true;
        },
    });

    LoadDepartment();
    layui.use("laydate",

        function () {
            var laydate = layui.laydate;
            var start = laydate.render({
                elem: '#startDate' //指定元素
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
                elem: '#endDate' //指定元素
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
                        return
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
})
let staticDepartmentId;
let staticDoctorId;

function GetRequest() {
    const url = location.search; //获取url中"?"符后的字串
    let theRequest = new Object();
    if (url.indexOf("?") != -1) {
        let str = url.substr(1);
        strs = str.split("&");
        for (let i = 0; i < strs.length; i++) {
            theRequest[strs[i].split("=")[0]] = unescape(strs[i].split("=")[1]);
        }
    }
    return theRequest;
}

function LoadDepartment() {
    $.get(
        "/Department/GetAllDepartment",
        function (result) {
            if (result.isSuccess) {
                var html = template("departmentHtml", result);
                console.log(html);
                $("#departmentList").html(html);
            } else {
                ShowTip("warning", "获取科室数据失败");
            }
        })
}

function LoadDoctor(departmentId) {
    $.get(
        `/Department/GetDoctorListByDepartmentId?departmentId=${departmentId}`,
        function (result) {
            if (result.isSuccess) {
                var html = template("doctorHtml", result.result);
                console.log(html);
                $("#doctorList").html(html);
            } else {
                ShowTip("warning", "获取医生信息失败");
            }
        }
    )
}

function LoadDate() {
    $.get(
        `/Department/GetDoctorScheduling?departmentId=${staticDepartmentId}&doctorId=${staticDoctorId}`,
        function (result) {
            if (result.isSuccess) {
                var html = template("dateHtml", result);
                $("#date").html(html);
            } else {
                ShowTip("warning", "获取医生排班信息失败");
            }
        }
    )
}

function Add() {
    var model = GetAddData();
    $.post(
        "/PA/AddAppointment",
        model,
        function (result) {
            if (result.isSuccess) {
                ShowTip("success", "添加成功");
                window.location.href = "/PA/AppointmentInfoList";
            } else {
                ShowTip("warning", result.message);
            }
        }
    )
}

function GetScheduling() {
    var departmentId = $("input[name='department']:checked").val();
    var userId = $("input[name='doctor']:checked").val();
    var startDate = $("#startDate").val();
    var endDate = $("#endDate").val();

    $.get(
        `/Doctor/GetSchedulingByUserId?userId=${userId}&startDate=${startDate}&endDate=${endDate}&departmentId=${departmentId}`,
        function (result) {
            if (result.isSuccess) {
                var html = template("dateHtml", result);
                $("#date").html(html);
            } else {
                ShowTip("warning", result.message);
            }
        }
    )
}

function GetAddData() {
    return {
        PatientId: patientId,
        DepartmentId: $("input[name='department']:checked").val(),
        DoctorId: $("input[name='doctor']:checked").val(),
        AppointmentDate: $("input[name='appointmentTime']:checked").val(),
        Describe: $("#describe").val(),
        AppointmentTime: $("input[name='appointmentTime']:checked").val()
    }
}