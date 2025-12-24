using SmartPlantWaterer.Models;
using SmartPlantWaterer.Services.Interfaces;

namespace SmartPlantWaterer.Services.Implementations
{
    public class WateringRuleEngine : IWateringRuleEngine
    {
        public bool ShouldWater(Telemetry telemetry) => telemetry.Moisture < 300 && telemetry.Temperature > 20;
    }
}
