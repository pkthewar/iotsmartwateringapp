using SmartPlantWaterer.Models;

namespace SmartPlantWaterer.Services.Interfaces
{
    public interface IHeartbeatService
    {
        Task ProcessHeartbeatAsync(HeartBeatDto heartBeatDto);
    }
}
