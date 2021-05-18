let socket;
$(function () {
    GetOnlineUser();

})

function GetOnlineUser() {
    $.get(
        "/Doctor/GetOnlineUser",
        function (result) {
            var html = template("userHtmlList", result);
            console.log(html);

            $("#userList").html(html);
        })


}

let toId;
function SelectUser(userId,userName) {
    toId = "P" + userId;
  
    $.get(
        `/Doctor/GetChatLogByUserId?userId=${userId}`,
        function (result) {
            console.log(result);
            if (result.isSuccess) {
                var html = template("historyList", result);
                $("#sendName").text(userName)
                $("#messageList").html(html);
            }
        }
    )
    const uri = "wss://localhost:5002/ws?connId=" + toId;
    socket = new WebSocket(uri);
    socket.onopen = function (e) {
        console.log("websocket estabished!");
    }

    socket.onclose = function (e) {
        console.log('websocket closed!');
    }

    socket.onmessage = function (e) {
        appendItem(e.data);
    }

    const btn = document.getElementById("sendBtn");
    btn.addEventListener("click", function () {
        var message = $("#message").val();
        var msg = {
            Message: message,
            SendToId: toId
        };
        socket.send(JSON.stringify(msg));
    })

    function appendItem(message) {
        var data = JSON.parse(message);

        if (data.From == toId) {
            var html = template("fromHtml", JSON.parse(message));
            $("#messageList").append(html);
        } else {
            var html = template("toHtml", JSON.parse(message));
            $("#messageList").append(html);
        }
           
    }
}

