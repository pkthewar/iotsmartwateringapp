using Microsoft.AspNetCore.SignalR;

namespace SmartPlantWaterer.Hubs
{
    public class TelemetryHub : Hub
    {
        /// <summary>
        /// Client subscribes to a specific plant
        /// </summary>
        /// <param name="plantId">Id of the specific plant to be subscribed</param>
        /// <returns></returns>
        public async Task SubscribePlant(int plantId) => await Groups.AddToGroupAsync(Context.ConnectionId, $"plant{plantId}");

        /// <summary>
        /// Client unsubscribes from a specific plant
        /// </summary>
        /// <param name="plantId">Id of the plant to be unsubscribed</param>
        /// <returns></returns>
        public async Task UnsubscribePlant(int plantId) => await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"plant-{plantId}");

        /// <summary>
        /// Manual watering trigger from UI
        /// </summary>
        /// <param name="plantId">Id of the plant to be watered</param>
        /// <returns></returns>
        public async Task ManualWater(int plantId) => await Clients.Group($"plant-{plantId}").SendAsync("ManualWaterRequested", plantId);

        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("Connected", Context.ConnectionId);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception) => await base.OnDisconnectedAsync(exception);
    }
}
