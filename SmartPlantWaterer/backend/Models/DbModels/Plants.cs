using Microsoft.EntityFrameworkCore;

namespace SmartPlantWaterer.Models.DbModels
{
    public class Plants
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Location { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
