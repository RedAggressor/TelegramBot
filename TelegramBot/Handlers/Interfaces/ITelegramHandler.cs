using Telegram.Bot.Types;
using Telegram.Bot;

namespace TelegramBot.Handlers.Interfaces
{
    public interface ITelegramHandler
    {
        Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);
        Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken);
    }
}
