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
    layui.use("laydate",

        function () {
            var laydate = layui.laydate;
            var start = laydate.render({
                elem: '#StartDate' //指定元素



            });

        });
});


function GetPatientInfo(id) {
    $.get(
        `/PA/GetPatientInfoDetail?id=${id}`,
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
        `/PA/GetAppointmentList`,
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
        PatientId: patientId,
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
function GetHistoryInfo(index) {
    historyPageIndex = index;
    var model = GetData(index);
    console.log(model);
    $.post(
        `/PA/GetMedicalHistoryByPatientId`,
        model,
        function (res) {
            console.log(res)
            if (res.isSuccess) {
                if (res.result.count > 0) {
                    //var html = $("#listHtml").tmpl(res.result.list)
                    var html = template("historyListHtml", res.result);
                    console.log(html)
                    if (pageIndex == 1) {
                        PageToolHistory(res.result.count);
                    }
                    $("#historyTableBody").html(html);
                }
                else {
                    $("#historyTableBody").html('<tr><td colspan="7">暂无数据</td></tr>')
                }
            }
        }
    )
}

function PageToolHistory(count) {
    layui.use('laypage', function () {
        var laypage = layui.laypage;
        //执行一个laypage实例
        laypage.render({
            elem: 'historyPage' //注意，这里的 test1 是 ID，不用加 # 号
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

function ShowAddHistoryPage() {
    isAddOrEdit = 1;
    ResetInput();
    $("#AddHistoryModal").modal("show");
}

function ResetInput() {
    $("#HistoryName").val();
    $("#Describe").val();
    $("#StartDate").val();
}

function AddOrEditHistory() {
    if (isAddOrEdit == 1) {
        var model = GetAddHistoryData();
        $("#AddHistoryModal").modal("hide");
        $.post(
            "/PA/AddMedicalHistory",
            model,
            function (result) {
                if (result.isSuccess) {
                    ShowTip("success", result.Message);
                } else {
                    ShowTip("warning", result.Message);
                }
            })
    }
    else {
        EditHistory();
    }
}

function GetAddHistoryData() {
    return {
        Name: $("#HistoryName").val(),
        PatientId: patientId,
        Describe: $("#Describe").val(),
        StartDate: $("#StartDate").val()
    }
}

let historyId;
function InitHistoryInfo(id) {
    isAddOrEdit = 2;
    historyId = id;
    $.get(
        `/PA/GetMedicalHistoryInfoById?id=${historyId}`,
        function (result) {
            if (result.isSuccess) {
                $("#HistoryName").val(result.result.name);
                $("#Describe").val(result.result.describe);
                $("#StartDate").val(result.result.startDate);
                $("#AddHistoryModal").modal("show");

            } else {
                ShowTip("warning", "获取信息失败");
            }
        }
    )
}

function GetEditHistoryData() {
    return {
        Name: $("#HistoryName").val(),
        PatientId: patientId,
        Describe: $("#Describe").val(),
        StartDate: $("#StartDate").val(),
        Id: historyId
    }
}

function EditHistory() {
    var model = GetEditHistoryData();
    $("#AddHistoryModal").modal("hide");
    $.post(
        "/PA/EditMedicalHistory",
        model,
        function (result) {
            if (result.isSuccess) {
                ShowTip("success", "添加成功");
            } else {
                ShowTip("warning", "编辑失败");

            }
        }
    )
}

