using SmartPlantWaterer.Models;
using System.Collections.Concurrent;

namespace SmartPlantWaterer.Simulation
{
    public class TelemetrySimulator
    {
        private readonly int[] _plants;
        private readonly Timer _timer;
        private readonly Random _rand = new();

        public event Func<TelemetryDto, PlantProfile, Task>? OnTelemetry;

        public TelemetrySimulator(int[] plants)
        {
            _plants = plants;
            _timer = new Timer(Generate, null, 0, 5000);
        }

        private async void Generate(object? _)
        {
            foreach (int plantId in _plants)
            {
                TelemetryDto dto = new()
                {
                    PlantId = plantId,
                    Moisture = _rand.Next(20, 80),
                    Temperature = _rand.Next(18, 35),
                    Humidity = _rand.Next(30, 90),
                    BatteryVoltage = Math.Round(3.2 + _rand.NextDouble() * 1.3, 2)
                };

                PlantProfile profile = new()
                {
                    Id = plantId,
                    MinMoisture = 35,
                    MaxMoisture = 65,
                    Temperature = 30,
                    WaterDurationSeconds = 30
                };

                if (OnTelemetry != null)
                    await OnTelemetry(dto, profile);
            }
        }
    }
}
