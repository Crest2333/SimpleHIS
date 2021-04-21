let editId;
var layedit;
$(function () {
    var appointmentId = $("#appointmentId").val();

    layui.use('layedit', function () {
            layedit = layui.layedit;
            editId = layedit.build('demo',
                {
                    tool: [
                        'strong' //加粗
                        , 'italic' //斜体
                        , 'underline' //下划线
                        , 'del' //删除线
                        , '|' //分割线
                        , 'left' //左对齐
                        , 'center' //居中对齐
                        , 'right' //右对齐
                    ]
                })
        } //建立编辑器
    );
    InitMedicalAdvice(appointmentId);
    InitAppointment(appointmentId);

})

function InitMedicalAdvice(appointmentId) {
    $.get(
        `/Doctor/GetMedicalAdvice?appointmentId=${appointmentId}`,
        function (result) {
            if (result.isSuccess) {
                layedit.setContent(editId, result.result.content,false)
                console.log(result);
            } else {
                ShowTip("warning", result.message);
            }
        }
    )
}

function InitAppointment(appointmentId) {
    $.get(
        `/PA/GetAppointmentDetailInfo?id=${appointmentId}`,
        function (result) {
            if (result.isSuccess) {
                console.log(result);
                $("#patientName").html(result.result.patientName);
                $("#describe").html(result.result.describe);
                $("#gender").html(result.result.gender);
                $("#height").html(result.result.height +"<span>KG</span>");
                $("#bloodType").html(result.result.bloodType);
                $("#weight").html(result.result.weight +"<span>CM</span>");
                $("#dateOfBirth").html(result.result.dateOfBirth);
            } else {
                ShowTip("warning", result.message);
            }
        }
    )
}

function AddOrEditMedicalAdvice() {
    console.log(editId);
   
    $.post(
        "/Doctor/AddOrEditMedicalAdvice",
        GetData(),
        function (result) {
            if (result.isSuccess) {
                console.log(result);
                window.location.href = "/Doctor/AppointmentList";

            } else {
                ShowTip("warning", result.message);
            }
        }
    )
}

function GetData() {
    var data = {
        Content: layedit.getContent(editId),
        AppointmentId: $("#appointmentId").val()
    }
    return data;
}