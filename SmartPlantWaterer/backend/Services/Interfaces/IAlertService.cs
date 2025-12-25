using SmartPlantWaterer.Models.DbModels;

namespace SmartPlantWaterer.Services.Interfaces
{
    public interface IAlertService
    {
        Task CheckAsync(Telemetry telemetry);

        //Task SendAlert(string msg);
    }
}
