namespace SmartPlantWaterer.Models
{
    public class HealthStatus
    {
        public bool IsApiRunning { get; set; }

        public bool IsDatabaseRunning { get; set; }

        public bool IsMqttWorking { get; set; }

        public bool IsOnnxPredicting { get; set; }

        public bool IsTelemetryFresh { get; set; }

        public string? OverallStatus { get; set; }

        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    }
}
