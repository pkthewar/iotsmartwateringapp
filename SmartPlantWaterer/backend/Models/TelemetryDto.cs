namespace SmartPlantWaterer.Models
{
    public class TelemetryDto
    {
        public int PlantId { get; set; }
        public float Moisture { get; set; }
        public float Temperature { get; set; }
        public float Humidity { get; set; }
    }
}
