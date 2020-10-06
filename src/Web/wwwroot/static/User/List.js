$(function () {
    GetList(1)
})

let index = 1;

let size = 10;


function GetList(index) {
    var model = GetData(index);
    $.post(
        "/User/GetUserInfoList",
        model,
        function (res) {
            console.log(res)
            if (res.isSuccess) {
                if ( res.result.count > 0) {
                    //var html = $("#listHtml").tmpl(res.result.list)
                    var html = template("listHtml", res.result);
                    PageTool(res.result.count);
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
        PageIndex: index|| 1,
        PageSize:size||10
    }

    return data;
}


function PageTool (count) {
    layui.use('laypage', function () {
        var laypage = layui.laypage;

        //执行一个laypage实例
        laypage.render({
            elem: 'page' //注意，这里的 test1 是 ID，不用加 # 号
            , count: count //数据总数，从服务端得到
            , limit: 20
            , jump: function (obj, first) {
                //obj包含了当前分页的所有参数，比如：

                //首次不执行
                if (!first) {
                    //do something
                }
            }
        });
    });

}


function AddUser() {
    var model = GetAddUserInfo();
    $.post(
        "AddUser",
        model,
        function (res) {
            console.log(res)
        }
    )
}

function GetAddUserInfo() {
    var data = {
        Name: $("#uname").val(),
        Gender: $("#gender").val(),
        PhoneNumber: $("#phone").val(),
        Email: $("#email").val(),
        Identity: $("#identity").val()
    }
    return data;
}

function LoadData(data) {
    layui.use('table', function () {
        var table = layui.table;

        //第一个实例
        table.render({
            elem: '#demo'
            , height: 312
            , url: '/demo/table/user/' //数据接口
            , page: true //开启分页
            , cols: [[ //表头
                { field: 'id', title: 'ID', width: 80, sort: true, fixed: 'left' }
                , { field: 'username', title: '用户名', width: 80 }
                , { field: 'sex', title: '性别', width: 80, sort: true }
                , { field: 'city', title: '城市', width: 80 }
                , { field: 'sign', title: '签名', width: 177 }
                , { field: 'experience', title: '积分', width: 80, sort: true }
                , { field: 'score', title: '评分', width: 80, sort: true }
                , { field: 'classify', title: '职业', width: 80 }
                , { field: 'wealth', title: '财富', width: 135, sort: true }
            ]]
        });

    });
}

