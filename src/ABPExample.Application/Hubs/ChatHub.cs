using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ABPExample.Domain.Public;
using HIS.Domain.Dtos.Chat;
using HIS.Query.Interface;

namespace HIS.Application.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatQuery _chatQuery;

        const string defaultGroup = "小兰聊天室";
        private readonly static Dictionary<string, string> _connections = new Dictionary<string, string>();
        private readonly static object _lock = new object();

        public ChatHub(IChatQuery chatQuery)
        {
            _chatQuery = chatQuery;
        }
        public override Task OnConnectedAsync()
        {

            //Clients.Caller.SendAsync("getProfileInfo", "test", "test");
            Groups.AddToGroupAsync(Context.ConnectionId, defaultGroup);

            var userId = CurrentUserId;
            lock (_lock)
            {

                if (string.IsNullOrEmpty(_connections.Keys.FirstOrDefault(t => t.Equals(userId))))
                {
                    _connections.Add(userId, Context.ConnectionId);
                }
                else
                {
                    _connections[userId] = Context.ConnectionId;
                }
            }
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _connections.Remove(CurrentUserId);
            return base.OnDisconnectedAsync(exception);
        }

        #region 用户

        /// <summary>
        /// 加入私聊频道
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="friendId"></param>
        /// <returns></returns>
        public async Task JoinFriendSocket(string userId, string friendId)
        {

            var roomName = CurrentUserId.Contains("D") ? CurrentUserId + friendId : friendId + CurrentUserId;
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            await Clients.Caller.SendAsync("JoinFriendSocket", ModelResult.Instance.Ok("连接成功"));

        }

        /// <summary>
        /// 发送私聊消息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task FriendMessage(SendMessageDto input)
        {

            try
            {
                var userId = CurrentUserId;
                var roomName = CurrentUserId.Contains("D") ? CurrentUserId + input.SendToId : input.SendToId + CurrentUserId;
                await Clients.Group(roomName).SendAsync("FriendMessage", ModelResult<SendMessageDto>.Instance.Ok("", input));
            }
            catch (Exception ex)
            {
                await SendError(ex.Message);
            }
        }

        #endregion

        #region 群组

        #endregion

        private async Task SendError(string message)
        {
            await Clients.Caller.SendAsync("OnError", ModelResult.Instance.Error("连接成功"));
        }
        private async Task SendCallerMessage(string method, string message, object data)
        {
            await Clients.Caller.SendAsync(method, ModelResult.Instance.Error("连接成功"));
        }

        private async Task SendUserMessage(string method, string userId, string message, object data)
        {
            if (_connections.Keys.FirstOrDefault(t => t.Equals(userId)) != null)
            {
                await Clients.Client(_connections[userId]).SendAsync(method, ModelResult.Instance.Error("连接成功"));
            }
        }
        /// <summary>
        /// 添加在线人员
        /// </summary>
        public void AddOnlineUser(string nickName)
        {
            if (!string.IsNullOrWhiteSpace(nickName))
            {
                var uid = Guid.NewGuid().ToString().ToUpper();
                //添加在线人员
                //userInfoList.Add(new UserInfo
                //{
                //    ConnectionId = Context.ConnectionId,
                //    UserID = uid,//随机用户id
                //    UserName = nickName,
                //    LoginTime = DateTime.Now,
                //    JoimTime = DateTime.Now
                //});
                //Clients.Client(Context.ConnectionId).showJoinMessage(uid);
            }
        }

        public string CurrentUserId
        {
            get
            {

                if (Context.User.Claims.FirstOrDefault(c => c.Type == "UserId") == null)
                {
                    return null;
                }
                var userId = Context.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                var role = Context.User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;



                if (string.IsNullOrEmpty(userId))
                {
                    return null;
                }

                if (role.IsNullOrWhiteSpace())
                    return $"P{userId}";
                return $"D{userId}";

            }
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }


    }
}
