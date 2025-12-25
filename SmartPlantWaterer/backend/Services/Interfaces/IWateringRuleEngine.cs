using SmartPlantWaterer.Models;
using SmartPlantWaterer.Models.DbModels;

namespace SmartPlantWaterer.Services.Interfaces
{
    public interface IWateringRuleEngine
    {
        bool ShouldWater(Telemetry telemetry, PlantProfile plantProfile);
    }
}
