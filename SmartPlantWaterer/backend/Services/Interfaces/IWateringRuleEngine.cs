using SmartPlantWaterer.Models;

namespace SmartPlantWaterer.Services.Interfaces
{
    public interface IWateringRuleEngine
    {
        bool ShouldWater(Telemetry telemetry);
    }
}
