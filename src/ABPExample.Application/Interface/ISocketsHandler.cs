using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Interface
{
    public interface ISocketsHandler
    {
        /// <summary>
        /// 连接一个 socket
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        Task OnConnected(WebSocket socket);

        Task OnConnected(WebSocket socket, string userId);


        /// <summary>
        /// 断开指定 socket
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        Task OnDisconnected(WebSocket socket);


        /// <summary>
        /// 发送消息给指定 socket
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendMessage(WebSocket socket, string message);


        /// <summary>
        /// 发送消息给指定 id 的 socket
        /// </summary>
        /// <param name="id"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendMessage(string id, string message);


        /// <summary>
        /// 给所有 sockets 发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendMessageToAll(string message);

    }
}
