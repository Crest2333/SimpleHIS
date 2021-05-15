using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HIS.Application.Interface
{
    public interface ISocketManagerApplication
    {
        ConcurrentDictionary<string, WebSocket> GetAllConnections();

        /// <summary>
        ///     获取指定 id 的 socket
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        WebSocket GetSocketById(string id);


        /// <summary>
        /// 根据 socket 获取其 id
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        string GetId(WebSocket socket);


        /// <summary>
        /// 删除指定 id 的 socket，并关闭该链接
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task RemoveSocketAsync(string id);


        /// <summary>
        /// 添加一个 socket
        /// </summary>
        /// <param name="socket"></param>
        void AddSocket(WebSocket socket);

        /// <summary>
        /// 添加一个 socket
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="userId"></param>

        void AddSocket(WebSocket socket, string userId);

    }
}
