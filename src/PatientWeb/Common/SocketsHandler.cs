﻿using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HIS.Application.Interface;

namespace PatientWeb.Common
{
    public abstract class SocketsHandler
    {
        public SocketsManager Manager { get; set; }

        protected  SocketsHandler(SocketsManager manager)
        {
            Manager = manager;
        }

        /// <summary>
        /// 连接一个 socket
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public virtual async Task OnConnected(WebSocket socket)
        {
            await Task.Run(() => { Manager.AddSocket(socket); });
        }

        /// <summary>
        /// 断开指定 socket
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public virtual async Task OnDisconnected(WebSocket socket)
        {
            await Manager.RemoveSocketAsync(Manager.GetId(socket));
        }

        /// <summary>
        /// 发送消息给指定 socket
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessage(WebSocket socket, string message)
        {
            if (socket.State != WebSocketState.Open) return;

            await socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(message)),
                WebSocketMessageType.Text, true, CancellationToken.None);
        }

        /// <summary>
        /// 发送消息给指定 id 的 socket
        /// </summary>
        /// <param name="id"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessage(string id, string message)
        {
            await SendMessage(Manager.GetSocketById(id), message);
        }

        /// <summary>
        /// 给所有 sockets 发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessageToAll(string message)
        {
            foreach (var connection in Manager.GetAllConnections()) await SendMessage(connection.Value, message);
        }

        /// <summary>
        /// 接收到消息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="result"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public abstract Task Receive(WebSocket socket, WebSocketReceiveResult result,
            byte[] buffer);
    }
}
