let patientId;
let isAddOrEdit;
$(function () {
    var request = GetRequest();
    patientId = request["patientId"];
    GetPatientInfo(patientId);
    console.log(request);
    Search(1);
    GetHistoryInfo(1);
    isAddOrEdit = 1;
});


function GetPatientInfo(id) {
    $.get(
        `/PatientFunction/GetPatientInfoDetail?id=${id}`,
        function (result) {
            if (result.isSuccess) {
                var html = template("baseInfo", result.result);
                console.log(html);
                $("#patientiInfo").html(html);
            } else {
                ShowTip("warning", "获取信息失败");
            }
        });
}

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

let pageIndex = 1;
let pageSize = 10;
function Search(index) {
    pageIndex = index;
    var model = GetData(pageIndex);
    $.post(
        `/PatientFunction/GetAppointmentList`,
        model,
        function (res) {
            console.log(res)
            if (res.isSuccess) {
                if (res.result.count > 0) {
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

let historyPageIndex = 1;



function ResetInput() {
    $("#HistoryName").val();
    $("#Describe").val();
    $("#StartDate").val();
}





function ShowAdd() {
    $("#addModal").modal("show");
}

function Add() {
    var model = GetAddInfo();
    $.post(
        "/PatientFunction/AddPatient",
        model,
        function (result) {
            if (result.isSuccess) {
                ShowTip("success", result.Message);
                Search(pageIndex);
                $("#addModal").modal("hide");
            } else {
                ShowTip("warning", result.Message);
            }
        });
}


function GetAddInfo() {
    var model = {
        FullName: $("#FullName").val(),
        PhoneNumber: $("#PhoneNumber").val(),
        IdentityId: $("#IdentityId").val(),
        Gender: $("#Gender").val(),
        Height: $("#Height").val(),
        Weight: $("#Weight").val(),
        Address: $("#Address").val(),
        DateOfBirth: $("#DateOfBirth").val(),
        BloodType: $("#BloodType").val()
    }
    return model;
}

