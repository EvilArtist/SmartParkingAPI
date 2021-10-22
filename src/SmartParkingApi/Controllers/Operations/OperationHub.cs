using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SmartParking.Share.Constants;
using SmartParkingAbstract.Services.Operation;
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
        private readonly IOperationService operationService;

        public OperationHub(IOperationService operationService)
        {
            this.operationService = operationService;
        }

        public async Task CheckInCard(string multiGateName, string cardId) 
        {
            var card = new SignalRCardData
            {
                GateName = multiGateName,
                CardID = cardId
            };
            var activeClient = await operationService.GetActiveClient(multiGateName);
            UartDataResponse<SignalRCardData> response = new() {
                Action = OperationConstants.Action.ScanCard,
                GateName = multiGateName,
                Data = card
            };

            await Clients.Client(activeClient).SendAsync("ACTION_" + multiGateName, response);
        }

        public async Task SubscribeDevice(string deviceName, bool requestControl = false)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, deviceName);
            string error = String.Empty;
            bool success = true;
            if(requestControl)
            {
                try
                {
                    await operationService.LockDevice(deviceName, Context.ConnectionId);
                }
                catch (Exception e)
                {
                    error = e.Message;
                    success = false;
                }
                
            }
            UartDataResponse<SignalRConnectStatus> response = new()
            {
                Action = OperationConstants.Action.Subscribe,
                GateName = deviceName,
                Data = new()
                {
                    ConnectionId = Context.ConnectionId,
                    Error = error,
                    LockSuccess = success,
                    GateName = deviceName,
                    RequestLock = requestControl
                }
            };

            await Clients.Caller.SendAsync("ACTION_" + deviceName, response);
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

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await operationService.UnlockDevice(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        public override async Task OnConnectedAsync()
        {
            await operationService.UnlockDevice(Context.ConnectionId);
            await base.OnConnectedAsync();
        }
    }
}
