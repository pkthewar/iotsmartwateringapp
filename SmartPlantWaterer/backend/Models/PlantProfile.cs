namespace SmartPlantWaterer.Models
{
    public class PlantProfile
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int MinMoisture { get; set; }

        public int MaxMoisture { get; set; }

        public int Temperature { get; set; }

        public int WaterDurationSeconds { get; set; }
    }
}
