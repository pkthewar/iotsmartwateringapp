using Microsoft.AspNetCore.Mvc;
using SmartPlantWaterer.Models;
using SmartPlantWaterer.Models.DbModels;
using SmartPlantWaterer.Services.Interfaces;

namespace SmartPlantWaterer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlantsController(IPlantService plantService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetPlants() => Ok(await plantService.GetActivePlantsAsync());

        [HttpPost]
        public async Task<IActionResult> AddPlant(CreatePlantDto createPlantDto)
        {
            Plants plant = new()
            {
                Name = createPlantDto.Name,
                Location = createPlantDto.Location
            };

            int plantId = await plantService.AddPlantAsync(plant);

            return CreatedAtAction(nameof(GetPlants), new
            {
                plantId
            }, plant);
        }
    }
}
