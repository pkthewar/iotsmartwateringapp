using SmartPlantWaterer.Models.DbModels;

namespace SmartPlantWaterer.Services.Interfaces
{
    public interface IPlantService
    {
        Task<List<Plants>> GetActivePlantsAsync();

        Task<int> AddPlantAsync(Plants plant);
    }
}
