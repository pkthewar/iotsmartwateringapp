using SmartPlantWaterer.Models;

namespace SmartPlantWaterer.Services.Interfaces
{
    public interface ITelemetryService
    {
        Task ProcessTelemetryAsync(TelemetryDto dto, PlantProfile plantProfile);
    }
}
