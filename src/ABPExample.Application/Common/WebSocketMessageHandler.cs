using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using ABPExample.Query.Common;
using HIS.Application.Interface;
using HIS.Domain.Dtos.Chat;
using HIS.Query.Interface;
using Newtonsoft.Json;
using Volo.Abp.DependencyInjection;

namespace HIS.Application.Common
{
    public class WebSocketMessageHandler : SocketsHandler, IWebSocketMessageHandler, ISingletonDependency
    {
        private readonly IChatQuery _chatQuery;

        public WebSocketMessageHandler(ISocketManagerApplication sockets, IChatQuery chatQuery) : base(sockets)
        {
            _chatQuery = chatQuery;
        }

        public override async Task OnConnected(WebSocket socket)
        {
            await base.OnConnected(socket);
            var socketId = Sockets.GetId(socket);
            await SendMessageToAll($"{socketId}已加入");
        }

        public override async Task OnDisconnected(WebSocket socket)
        {
            await base.OnDisconnected(socket);
            var socketId = Sockets.GetId(socket);
            await SendMessageToAll($"{socketId}离开了");
        }

        public override async Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffer, string userId)
        {
            var socketId = Sockets.GetId(socket);
            var message = $"{Encoding.UTF8.GetString(buffer, 0, result.Count)}";
            var data = JsonConvert.DeserializeObject<SendMessageDto>(message);
            var toSocket = Sockets.GetSocketById(data.SendToId);
            var sendData = new MessageResult
            {
                Message = data.Message,
                From = userId,
                SendDate = $"{DateTime.Now:yyyy-MM-dd}"
            };

            if (toSocket != null)
                await SendMessage(toSocket, JsonConvert.SerializeObject(sendData));
            await SendMessage(socket, JsonConvert.SerializeObject(sendData));

            var sendUserId = 0;
            var sendDoctorId = 0;
            int sendFrom = 1;
            if (userId.Contains("P"))
            {
                sendUserId = userId.Contains("P")
                    ? userId.Replace("P", string.Empty).ToInt()
                    : userId.Replace("D", string.Empty).ToInt();
                sendDoctorId = data.SendToId.Contains("P")
                    ? data.SendToId.Replace("P", string.Empty).ToInt()
                    : data.SendToId.Replace("D", string.Empty).ToInt();
                sendFrom = 1;
            }

            else
            {
                sendUserId = data.SendToId.Contains("P")
                    ? data.SendToId.Replace("P", string.Empty).ToInt()
                    : data.SendToId.Replace("D", string.Empty).ToInt();
                sendDoctorId = userId.Contains("P")
                    ? userId.Replace("P", string.Empty).ToInt()
                    : userId.Replace("D", string.Empty).ToInt();
                sendFrom = 2;
            }

            if (toSocket == null)

                await _chatQuery.AddAsync(data.Message, sendUserId, sendDoctorId, sendFrom, true);
            else

                await _chatQuery.AddAsync(data.Message, sendUserId, sendDoctorId, sendFrom, false);

        }
    }
}
