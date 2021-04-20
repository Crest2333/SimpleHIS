using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Interface
{
    public  interface IWebSocketManager
    {
        ConcurrentDictionary<string, WebSocket> GetAllConnections();

        WebSocket GetSocketById(string id);

        string GetId(WebSocket socket);

        Task RemoveSocketAsync(string id);

        void AddSocket(WebSocket socket);

    }
}
