namespace SmartPlantWaterer.Services.Interfaces
{
    public interface IAlertChannel
    {
        Task SendAlert(string msg);
    }
}
