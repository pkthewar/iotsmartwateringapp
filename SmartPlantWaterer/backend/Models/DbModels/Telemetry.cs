namespace SmartPlantWaterer.Models.DbModels
{
    public class Telemetry
    {
        public int Id { get; set; }
        public int PlantId { get; set; }
        public float Moisture { get; set; }
        public float Temperature { get; set; }
        public float Humidity { get; set; }
        public float Score { get; set; }
        public bool WaterNow { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
