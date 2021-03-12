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
using ContractorSearch.Data;
using Microsoft.AspNetCore.SignalR;
using ContractorSearch.Models;
using System.Linq;

namespace ContractorSearch.Hubs
{
    public class ChatHub : Hub //added string Name to all methods
    {
        private readonly ApplicationDbContext _context;
        public ChatHub(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SendMessageToGroup(string groupName, string message, string name)
        {
            Message message1 = new Message();
            message1.AppointmentId = Int32.Parse(groupName);
            message1.Text = message;
            message1.UserName = name;
            _context.Add(message1);
            await _context.SaveChangesAsync(); //might need await
            //return Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId}: {message}");
            await Clients.Group(groupName).SendAsync("Send", $"{name}: {message}");
        }

        public async Task AddToGroup(string groupName, string name)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            List<string> names = _context.Messages.Where(m => m.AppointmentId == Int32.Parse(groupName)).Select(m0 => m0.UserName).ToList();
            List<string> messages = _context.Messages.Where(m => m.AppointmentId == Int32.Parse(groupName)).Select(m0 => m0.Text).ToList();

            //await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {groupName}.");
            await Clients.Group(groupName).SendAsync("Send", $"{name} has joined the group {groupName}.");
            for(int i = 0; i < messages.Count(); i++)
            {
               // await Clients.Group(groupName).SendAsync(messages[i]);
                await Clients.Caller.SendAsync("Send", $"{names[i]}: {messages[i]}");
            }
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