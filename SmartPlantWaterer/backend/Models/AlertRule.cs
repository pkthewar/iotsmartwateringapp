namespace SmartPlantWaterer.Models
{
    public class AlertRule
    {
        public int PlantId { get; set; }

        public float MinMoisture { get; set; }

        public float MaxTemperature { get; set; }
    }
}
