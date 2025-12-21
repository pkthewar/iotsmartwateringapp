using SmartPlantWaterer.Models;

namespace SmartPlantWaterer.Services
{
    public class WateringRuleEngine
    {
        public bool ShouldWater(Telemetry telemetry) => telemetry.Moisture < 300 && telemetry.Temperature > 20;
    }
}
