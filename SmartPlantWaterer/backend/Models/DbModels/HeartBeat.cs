namespace SmartPlantWaterer.Models.DbModels
{
    public class HeartBeat
    {
        public Guid Id { get; set; }
        
        public int PlantId { get; set; }

        public string? Type { get; set; }

        public string? Message { get; set; }

        public float Value { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool Acknowledged { get; set; }
    }
}
