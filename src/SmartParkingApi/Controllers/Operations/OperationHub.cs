using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SmartParking.Share.Constants;
using SmartParkingAbstract.ViewModels.Operation;
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
            var card = new SignalRCardData
            {
                GateName = multiGateName,
                CardID = cardId
            };
            UartDataResponse<SignalRCardData> response = new() {
                Action = OperationConstants.Action.ScanCard,
                GateName = multiGateName,
                Data = card
            };
            await Clients.Group(multiGateName).SendAsync("CardScan_" + multiGateName, response);
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

        public async Task OpenGate(string deviceName)
        {
            await Clients.Group(deviceName).SendAsync("OpenGate_" + deviceName);
        }
    }
}
