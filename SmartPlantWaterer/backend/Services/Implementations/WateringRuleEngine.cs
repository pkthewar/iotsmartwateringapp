using SmartPlantWaterer.Models;
using SmartPlantWaterer.Models.DbModels;
using SmartPlantWaterer.Services.Interfaces;

namespace SmartPlantWaterer.Services.Implementations
{
    public class WateringRuleEngine : IWateringRuleEngine
    {
        public bool ShouldWater(Telemetry telemetry, PlantProfile plantProfile) => telemetry.Moisture < plantProfile.MinMoisture;
    }
}
