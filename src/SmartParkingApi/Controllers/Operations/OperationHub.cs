using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartParkingApi.Controllers.Operations
{
    [Authorize]
    public class OperationHub : Hub
    {
        public async Task CheckInCard(string multiGateName, string cardId) 
        {
            await Clients.Group(multiGateName).SendAsync("Notify", $"Check in on Gate ${multiGateName} with ID: {cardId}");
        }

        public async Task SubscribeDevice(string deviceName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, deviceName);

            await Clients.Group(deviceName).SendAsync("Notify", $"{Context.ConnectionId} has joined the group {deviceName}.");
        }

        public async Task UnsubscribeDevice(string deviceName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, deviceName);

            await Clients.Group(deviceName).SendAsync("Notify", $"{Context.ConnectionId} has left the group {deviceName}.");
        }
    }

    public class CardCheckIn
    {
        public string Name { get; set; }
        public string RFID { get; set; }
    }
}
