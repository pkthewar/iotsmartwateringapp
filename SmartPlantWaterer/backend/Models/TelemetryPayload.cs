namespace SmartPlantWaterer.Models
{
    public class TelemetryPayload
    {
        public string? PlantId { get; set; }
        public double SoilMoisture { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double LightLux { get; set; }
        public double BatteryVoltage { get; set; }
        public bool PumpOn { get; set; }
        public DateTime TimestampUtc { get; set; }
    }
}
