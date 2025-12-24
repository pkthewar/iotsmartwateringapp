namespace SmartPlantWaterer.Services.Interfaces
{
    public interface IPumpService
    {
        Task ActivatePumpAsync(int plantId);
    }
}
