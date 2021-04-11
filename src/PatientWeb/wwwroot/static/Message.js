function ShowTip(type,msg){
    const Toast = Swal.mixin({
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: 1113000
    });

    Toast.fire({
        type: type,
        title: msg
    })
}

function Tip(type, msg) {
    var modelHead = '<div class="modal fade" id="tip" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true"> <div class="modal-dialog" role="document">'
    var modelFooter = '</div></div>'

    var tip = "";
    if (type == "success")
        tip = '<div class="alert alert-success" style="text-align:center" role="alert">' + msg + '</div>'
    if (type == "warning")
        tip = '<div class="alert alert-warning" style="text-align:center"  role="alert">' + msg + '</div>'
    if (type == "danger")
        tip = '<div class="alert alert-danger" style="text-align:center" role="alert">' + msg + '</div>'
    if (type == "info")
        tip = '<div class="alert alert-info" style="text-align:center"  role="alert">' + msg + '</div>'

    var html = modelHead + tip + modelFooter;

    $("body").append(html);

    $("#tip").modal("show");
}

