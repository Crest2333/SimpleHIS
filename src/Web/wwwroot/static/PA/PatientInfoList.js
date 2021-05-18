$(function () {
    layui.use("laydate",

        function () {
            var laydate = layui.laydate;
            var start = laydate.render({
                elem: '#DateOfBirth' //指定元素



            });

        });
});
let pageIndex = 1;
let pageSize = 10;
function Search(index) {
    pageIndex = index;
    var model = GetData(pageIndex);
    $.post(
        "/PA/GetPatientInfoList",
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
        Gender: $("#gender").val(),
        PhoneNumber: $("#phone").val(),
        IdentityId: $("#identityNo").val(),
        Name: $("#name").val(),
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

function ShowAdd() {
    $("#FullName").val(null);
    $("#PhoneNumber").val(null);
    $("#IdentityId").val(null);
    $("#Gender").val(null);
    $("#Height").val(null);
    $("#Weight").val(null);
    $("#Address").val(null);
    $("#DateOfBirth").val(null);
    $("#BloodType").val(null)
    $("#addModal").modal("show");
}

function Add() {
    var model = GetAddInfo();
    if (model.DateOfBirth == "") {
        ShowTip("warning", "请选择出生年月");
        return;
    }
    if (model.Gender == ""||model.Gender==null) {
        ShowTip("warning", "请选择性别");
        return;
    }
    $.post(
        "/PA/AddPatient",
        model,
        function (result) {
            if (result.isSuccess) {
                ShowTip("success", result.message);
                Search(pageIndex);
                $("#addModal").modal("hide");
            } else {
                ShowTip("warning", result.message);
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

function Delete(id) {
    var isDelete = confirm("确认删除？");
    if (isDelete) {
        $.get(
            `/PA/DeletePatient?id=${id}`,
            function (result) {
                if (result.isSuccess) {
                    ShowTip("success", result.Message);
                    Search(pageIndex);
                } else {
                    ShowTip("warning", result.Message);

                }
            });
    }
}

function OpenEdit(id) {
    $.get(
        `/PA/GetPatientInfoDetail?id=${id}`,
        function (result) {
            if (result.isSuccess) {
                console.log(result);
                InitEditUserInfo(result.result);
                $("#editModal").modal("show")
            } else {
                ShowTip("warning", result.Message);

            }
        }
    )
}

function InitEditUserInfo(data) {
    $("#patientId").val(data.id)
    $("#EditFullName").val(data.fullName);
    $("#EditPhoneNumber").val(data.phoneNumber);
    $("#EditIdentityId").val(data.identityId);
    $("#EditGender").val(data.gender);
    $("#EditHeight").val(data.height);
    $("#EditWeight").val(data.weight);
    $("#EditAddress").val(data.address);
    $("#EditDateOfBirth").val(data.dateOfBirth);
    $("#EditBloodType").val(data.bloodType)
}

function Edit() {
    if (confirm("确认保存？")) {
        var model = {
            Id: $("#patientId").val(),
            FullName: $("#EditFullName").val(),
            PhoneNumber: $("#EditPhoneNumber").val(),
            IdentityId: $("#EditIdentityId").val(),
            Gender: $("#EditGender").val(),
            Height: $("#EditHeight").val(),
            Weight: $("#EditWeight").val(),
            Address: $("#EditAddress").val(),
            DateOfBirth: $("#EditDateOfBirth").val(),
            BloodType: $("#EditBloodType").val()
        }

        if (model.DateOfBirth == "") {
            ShowTip("warning", "请选择出生年月");
            return;
        }
        if (model.Gender == "" || model.Gender == null) {
            ShowTip("warning", "请选择性别");
            return;
        }
        $.post(
            "/PA/EditPatientInfo",
            model,
            function (result) {
                if (result.isSuccess) {
                    ShowTip("success", result.message);
                    $("#editModal").modal("hide");
                    Search(pageIndex);
                } else {
                    ShowTip("warning", result.message);

                }
            }
        )
    }
}

function LoadDepartment() {

}

