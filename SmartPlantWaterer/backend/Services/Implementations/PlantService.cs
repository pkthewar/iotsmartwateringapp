using Microsoft.EntityFrameworkCore;
using SmartPlantWaterer.Data;
using SmartPlantWaterer.Models.DbModels;
using SmartPlantWaterer.Services.Interfaces;

namespace SmartPlantWaterer.Services.Implementations
{
    public class PlantService(AppDbContext appDbContext) : IPlantService
    {
        private readonly AppDbContext appDbContext = appDbContext;

        public async Task<List<Plants>> GetActivePlantsAsync() => await appDbContext.Plants.Where(p => p.IsActive).ToListAsync();

        public async Task<int> AddPlantAsync(Plants plant)
        {
            await appDbContext.Plants.AddAsync(plant);
            await appDbContext.SaveChangesAsync();

            return plant.Id;
        }
    }
}
