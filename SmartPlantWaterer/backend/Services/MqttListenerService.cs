
using MQTTnet;
using SmartPlantWaterer.Models;
using System.Text.Json;

namespace SmartPlantWaterer.Services
{
    public class MqttListenerService(IServiceProvider serviceProvider) : BackgroundService
    {
        private readonly IServiceProvider serviceProvider = serviceProvider;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            IMqttClient client = new MqttClientFactory().CreateMqttClient();

            client.ApplicationMessageReceivedAsync += async e =>
            {
                TelemetryDto? dto = JsonSerializer.Deserialize<TelemetryDto>(e.ApplicationMessage.ConvertPayloadToString());

                using IServiceScope scope = serviceProvider.CreateScope();

                TelemetryService svc = scope.ServiceProvider.GetRequiredService<TelemetryService>();

                await svc.ProcessTelemetryAsync(dto!);
            };

            await client.ConnectAsync(new MqttClientOptionsBuilder().WithTcpServer("localhost").Build(), stoppingToken);

            await client.SubscribeAsync("plants/telemetry", cancellationToken: stoppingToken);
        }
    }
}
