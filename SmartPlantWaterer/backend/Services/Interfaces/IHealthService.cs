using SmartPlantWaterer.Models;

namespace SmartPlantWaterer.Services.Interfaces
{
    public interface IHealthService
    {
        Task<HealthStatus> CheckAsync();
    }
}
