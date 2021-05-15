
$(function () {
    var appointmentId = $("#appointmentId").val();

    InitAppointment(appointmentId)
    InitMedicalAdvice(appointmentId)
})
function InitAppointment(appointmentId) {
    $.get(
        `/PA/GetAppointmentDetailInfo?id=${appointmentId}`,
        function (result) {
            if (result.isSuccess) {
                console.log(result);
                $("#patientName").html(result.result.patientName);
                $("#describe").html(result.result.describe);
                $("#gender").html(result.result.genderDesc);
                $("#height").html(result.result.height + "<span>KG</span>");
                $("#bloodType").html(result.result.bloodType);
                $("#weight").html(result.result.weight + "<span>CM</span>");
                $("#dateOfBirth").html(result.result.dateOfBirth);
                $("#doctor").html(result.result.doctorName);
                $("#appointmentDate").html(result.result.appointmentDate);

            } else {
                ShowTip("warning", result.message);
            }
        }
    )
}

function InitMedicalAdvice(appointmentId) {
    $.get(
        `/Doctor/GetMedicalAdvice?appointmentId=${appointmentId}`,
        function (result) {
            if (result.isSuccess) {
                $("#adviceInfo").html(result.result.content);
                console.log(result);
            } else {
                ShowTip("warning", result.message);
            }
        }
    )
}