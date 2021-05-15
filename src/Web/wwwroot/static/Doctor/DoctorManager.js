$(function () {
    $("#imgFile").on("change", function () {
        var objUrl = getObjectURL(this.files[0]); //获取图片的路径，该路径不是图片在本地的路径
        if (objUrl) {
            $("#imgUrl").attr("src", objUrl); //将图片路径存入src中，显示出图片
        }
    });
   
})

function getObjectURL(file) {
    var url = null;
    if (window.createObjectURL != undefined) { // basic
        url = window.createObjectURL(file);
    } else if (window.URL != undefined) { // mozilla(firefox)
        url = window.URL.createObjectURL(file);
    } else if (window.webkitURL != undefined) { // webkit or chrome
        url = window.webkitURL.createObjectURL(file);
    }
    return url;
}

let pageIndex = 1;
let pageSize = 10;
function Search(index) {
    pageIndex = index;
    var model = GetData(pageIndex);
    $.post(
        `/Doctor/GetDoctorInfoList`,
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

let userId = 0;

function OpenEdit(id) {
    userId = id;
    $.get(
        `/Doctor/DoctorInfoDetail?userId=${id}`,
        function (result) {
            console.log(result)
            $("#editModal").modal("show");
            $("#name").val(result.result.name);
            $("#Introduce").val(result.result.introduce);
            if (result.result.imgUrl != "") {
                $("#imgUrl").val(result.result.imgUrl);
                $("#imgUrl").attr("src", result.result.imgUrl);
            }
               
        }
    )
}




function Edit() {
    var file = $('#imgFile').prop("files");
    var model = {
        UserId: userId,
        Introduce: $("#Introduce").val()
    }
    var data = new FormData()
    data.append("file", file[0]);
    data.append("UserId", model.UserId);
    data.append("Introduce", model.Introduce);

    $.ajax(
        {
            method: 'post',
            url: "/Doctor/EditDoctorInfo",
            data: data,
            cache: false,
            processData: false,
            contentType: false,
            success: function (res) {
                if (res.isSuccess) {
                    ShowTip('success', '添加成功')
                }
                else {
                    ShowTip('warning', res.Message);
                }
            }

        }
    )
}

function changeImg() {
    $("#imgFile").click();

}