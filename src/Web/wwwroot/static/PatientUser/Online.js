
$(function () {
        LoadDoctor();
    //"use strict";

    //var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

    ////Disable send button until connection is established
    //document.getElementById("sendBtn").disabled = true;

    //connection.on("ReceiveMessage", function (user, message) {
    //    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    //    var encodedMsg = user + " says " + msg;
    //    var li = document.createElement("li");
    //    li.textContent = encodedMsg;
    //    document.getElementById("messageList").appendChild(li);
    //});

    //connection.start().then(function () {
    //    document.getElementById("sendBtn").disabled = false;
    //}).catch(function (err) {
    //    return console.error(err.toString());
    //});

    //document.getElementById("sendBtn").addEventListener("click", function (event) {
    //    var user = document.getElementById("userInput").value;
    //    var message = document.getElementById("message").value;
    //    connection.invoke("SendMessage", toId, message).catch(function (err) {
    //        return console.error(err.toString());
    //    });
    //    event.preventDefault();
    //});
   

  
}

)

function LoadDoctor() {
    $.get(
        "/Department/GetDoctorList",
        function (result) {
            console.log(result);
         
            var html = template("doctorList", result);
            console.log(html);

            $("#userList").html(html);

        }
    )
}

let toId;
function SelectDoctor(doctorId,userName) {
    toId = "D" + doctorId;

    $.get(
        `/PatientFunction/GetChatLogByDoctorId?doctorId=${doctorId}`,
        function (result) {
            console.log(result);
            if (result.isSuccess) {
                $("#sendName").text(userName);
                var html = template("historyList", result);
                $("#messageList").html(html);
            }
        }
    )
    const uri = "wss://localhost:5002/ws?connId="+toId;
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
        console.log(toId);
        if (data.From == toId) {
            var html = template("fromHtml", JSON.parse(message));
            $("#messageList").append(html);
        } else {
            var html = template("toHtml", JSON.parse(message));
            $("#messageList").append(html);
        }
    }
}