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
            await Clients.Group(multiGateName).SendAsync("ACTION_" + multiGateName, response);
        }

        public async Task SubscribeDevice(string deviceName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, deviceName);
            UartDataResponse<string> response = new()
            {
                Action = OperationConstants.Action.Subscribe,
                GateName = deviceName,
                Data = Context.ConnectionId
            };

            await Clients.Group(deviceName).SendAsync("ACTION_" + deviceName, response);
        }

        public async Task UnsubscribeDevice(string deviceName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, deviceName);

            UartDataResponse<string> response = new()
            {
                Action = OperationConstants.Action.Unsubscribe,
                GateName = deviceName,
                Data = Context.ConnectionId
            };

            await Clients.Group(deviceName).SendAsync("ACTION_" + deviceName, response);
        }

        public async Task OpenGate(string deviceName)
        {
            UartDataResponse<string> response = new()
            {
                Action = OperationConstants.Action.AllowIn,
                GateName = deviceName,
                Data = Context.ConnectionId
            };
            await Clients.Group(deviceName).SendAsync("ACTION_" + deviceName, response);
        }
    }
}
