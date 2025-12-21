using MQTTnet;

namespace SmartPlantWaterer.Services
{
    public class PumpService
    {
        public async Task ActivatePumpAsync(int plantId)
        {
            MqttClientFactory factory = new();

            IMqttClient client = factory.CreateMqttClient();

            await client.ConnectAsync(new MqttClientOptionsBuilder().WithTcpServer("localhost").Build());

            MqttApplicationMessage message = new MqttApplicationMessageBuilder().WithTopic($"/plants/{plantId}/pump").WithPayload("ON").WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce).WithRetainFlag(false).Build();

            await client.PublishAsync(message);
        }
    }
}
