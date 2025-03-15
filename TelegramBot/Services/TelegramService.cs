using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using TelegramBot.Configs;
using TelegramBot.Handlers.Interfaces;
using TelegramBot.Services.Interfaces;

namespace TelegramBot.Services
{
    public class TelegramService : ITelegramService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ReceiverOptions _receiverOptions;
        private readonly TelegramConfig _telegramConfig;
        private readonly ITelegramHandler _telegramHandler;
        private readonly ILogger<TelegramService> _logger;

        public TelegramService(
            IOptions<TelegramConfig> options,
            ITelegramHandler telegramHandler,
            ILogger<TelegramService> logger) 
        {
            _telegramConfig = options.Value;

            _botClient = new TelegramBotClient($"{_telegramConfig.Token}");

            _receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new[]
                {
                    UpdateType.Message,
                    UpdateType.CallbackQuery
                }                
            };

            _telegramHandler = telegramHandler;
            _logger = logger;
        }

        public async Task StartBot()
        {
            using var cts  = new CancellationTokenSource();

            _botClient.StartReceiving(
                _telegramHandler.UpdateHandler,
                _telegramHandler.ErrorHandler,
                _receiverOptions,
                cts.Token);

            var me = await _botClient.GetMe();

            _logger.LogInformation($"{me.FirstName} start!");            

            await Task.Delay(-1);
        }
    }
}
