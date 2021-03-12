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
    public class ChatHub : Hub //added string Name to all methods
    {
        public Task SendMessageToGroup(string groupName, string message, string name)
        {
            //return Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId}: {message}");
            return Clients.Group(groupName).SendAsync("Send", $"{name}: {message}");
        }

        public async Task AddToGroup(string groupName, string name)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            //await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {groupName}.");
            await Clients.Group(groupName).SendAsync("Send", $"{name} has joined the group {groupName}.");
        }

        public async Task RemoveFromGroup(string groupName, string name)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            //await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}.");
            await Clients.Group(groupName).SendAsync("Send", $"{name} has left the group {groupName}.");
        }

        public Task SendPrivateMessage(string user, string message, string name)
        {
            return Clients.User(user).SendAsync("ReceiveMessage", message);
        }
    }
}