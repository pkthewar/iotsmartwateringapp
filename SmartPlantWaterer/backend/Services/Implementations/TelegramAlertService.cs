using SmartPlantWaterer.Models;
using SmartPlantWaterer.Services.Interfaces;

namespace SmartPlantWaterer.Services.Implementations
{
    public class TelegramAlertService(HttpClient httpClient, IConfiguration configuration) : IAlertChannel
    {
        private readonly HttpClient httpClient = httpClient;

        private readonly string botToken = configuration["Telegram: BotToken"]!;

        private readonly string chatId = configuration["Telegram: ChatId"]!;

        public async Task SendAlert(string msg)
        {
            string url = $"https://api.telegram.org/bot{botToken}/sendMessage?chat_id={chatId}&text={Uri.EscapeDataString(msg)}";

            await httpClient.GetAsync(url);
        }
    }
}
