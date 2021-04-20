using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PatientWeb.Common
{
    public class SocketsMiddleware
    {
        private readonly RequestDelegate _next;

        public SocketsMiddleware( RequestDelegate next,SocketsHandler socketHandler)
        {
            _next = next;
            Handler = socketHandler;
        }

        private SocketsHandler Handler { get; }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                // 转换当前连接为一个 ws 连接
                var socket = await context.WebSockets.AcceptWebSocketAsync();
                await Handler.OnConnected(socket);

                // 接收消息的 buffer
                var buffer = new byte[1024 * 4];
                // 判断连接类型，并执行相应操作
                while (socket.State == WebSocketState.Open)
                {
                    // 这句执行之后，buffer 就是接收到的消息体，可以根据需要进行转换。
                    var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    switch (result.MessageType)
                    {
                        case WebSocketMessageType.Text:
                            await Handler.Receive(socket, result, buffer);
                            break;
                        case WebSocketMessageType.Close:
                            await Handler.OnDisconnected(socket);
                            break;
                        case WebSocketMessageType.Binary:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            else
            {
                await _next(context);
            }
        }
      
    }
}
