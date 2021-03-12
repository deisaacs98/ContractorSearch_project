//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.SignalR;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace ContractorSearch.Hubs
//{
//    [Authorize]
//    public class ChatHub : Hub
//    {
//        public async Task SendMessage(string user, string message)
//        {
//            await Clients.All.SendAsync("ReceiveMessage", user, message);
//        }
//        public void SendChatMessage(string who, string message)
//        {
//            string name = Context.User.Identity.Name;

//            Clients.Group(who).SendAsync(name + ": " + message);
//        }

//        public override Task OnConnectedAsync()
//        {
//            string name = Context.User.Identity.Name;

//            Groups.AddToGroupAsync(Context.ConnectionId, name);

//            return base.OnConnectedAsync();
//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ContractorSearch.Hubs
{
    public class ChatHub : Hub
    {
        public Task SendMessageToGroup(string groupName, string message)
        {
            return Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId}: {message}");
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {groupName}.");
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}.");
        }

        public Task SendPrivateMessage(string user, string message)
        {
            return Clients.User(user).SendAsync("ReceiveMessage", message);
        }
    }
}