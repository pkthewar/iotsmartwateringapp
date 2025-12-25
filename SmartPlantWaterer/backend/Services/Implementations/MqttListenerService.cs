using MQTTnet;
using SmartPlantWaterer.Models;
using SmartPlantWaterer.Services.Interfaces;
using System.Text.Json;

namespace SmartPlantWaterer.Services.Implementations
{
    public class MqttListenerService(IServiceProvider serviceProvider) : BackgroundService
    {
        private readonly IServiceProvider serviceProvider = serviceProvider;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            IMqttClient client = new MqttClientFactory().CreateMqttClient();

            client.ApplicationMessageReceivedAsync += async e =>
            {
                string topic = e.ApplicationMessage.Topic;
                string payload = e.ApplicationMessage.ConvertPayloadToString();

                using IServiceScope scope = serviceProvider.CreateScope();

                switch (topic)
                {
                    case "plants/telemetry":
                        ITelemetryService svc = scope.ServiceProvider.GetRequiredService<ITelemetryService>();

                        //await svc.ProcessTelemetryAsync(dto!); //To-Do: Modify the call to add PlantProfile object as well.

                        break;

                    case "plants/heartbeat":
                        HeartBeatDto? heartBeatDto = JsonSerializer.Deserialize<HeartBeatDto>(payload);

                        if (heartBeatDto is null)
                            return;

                        //IHeartBeatService 
                        break;
                }

                await client.ConnectAsync(new MqttClientOptionsBuilder().WithTcpServer("localhost").Build(), stoppingToken);

                await client.SubscribeAsync("plants/telemetry", cancellationToken: stoppingToken);
                await client.SubscribeAsync("plants/heartbeat", cancellationToken: stoppingToken);
            };

            //client.ApplicationMessageReceivedAsync += async e =>
            //{
            //    TelemetryDto? dto = JsonSerializer.Deserialize<TelemetryDto>(e.ApplicationMessage.ConvertPayloadToString());

            //    using IServiceScope scope = serviceProvider.CreateScope();


            //};


        }
    }
}
