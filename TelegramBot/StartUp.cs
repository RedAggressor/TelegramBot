
using TelegramBot.Services;
using TelegramBot.Services.Interfaces;

namespace TelegramBot
{
    public class StartUp
    {
        private readonly ITelegramService _teleramService;
        public StartUp(ITelegramService telegramService)
        {
            _teleramService = telegramService;
        }

        public async Task Start()
        {
            await _teleramService.StartBot();
        }
    }
}
