using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using TelegramBot.Handlers.Interfaces;
using Microsoft.Extensions.Logging;

namespace TelegramBot.Handlers
{
    public class TelegramHandler : ITelegramHandler
    {
        private readonly ILogger<TelegramHandler> _logger;

        public TelegramHandler(ILogger<TelegramHandler> logger)
        { 
            _logger = logger;
        }

        public async Task UpdateHandler(
            ITelegramBotClient botClient,
            Update update,
            CancellationToken cancellationToken)
        {
            try
            {
                switch (update.Type)
                {
                    case UpdateType.Message:
                        {
                            await TypeMessageHandler(botClient, update, cancellationToken);
                            return;                            
                        }

                    case UpdateType.CallbackQuery:
                        {
                            await TypeCallbackQuery(botClient, update, cancellationToken);
                            return;
                        }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }

        public Task ErrorHandler(
            ITelegramBotClient botClient,
            Exception error,
            CancellationToken cancellationToken)
        {
            var ErrorMessage = error switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => error.ToString()
            };

            _logger.LogError(ErrorMessage);

            return Task.CompletedTask;
        }

        private async Task TypeCallbackQuery(ITelegramBotClient botClient,
            Update update,
            CancellationToken cancellationToken)
        {
            var callbackQuery = update.CallbackQuery;
            var user = callbackQuery!.From;
            var chat = callbackQuery.Message!.Chat;

            _logger.LogInformation($"{user.FirstName} ({user.Id}) put on button: {callbackQuery.Data}");

            switch (callbackQuery.Data)
            {
                case "button1":
                    {
                        await botClient.AnswerCallbackQuery(callbackQuery.Id);

                        await botClient!.SendMessage(
                            chat.Id,
                            $"You put on button {callbackQuery.Data}", 
                            cancellationToken: cancellationToken);

                        return;
                    }
                case "button2":
                    {
                        await botClient.AnswerCallbackQuery(callbackQuery.Id);

                        await botClient!.SendMessage(
                            chat.Id,
                            $"You put on button {callbackQuery.Data}",
                            cancellationToken: cancellationToken);

                        return;
                    }
                case "button3":
                    {
                        await botClient.AnswerCallbackQuery(callbackQuery.Id);

                        await botClient!.SendMessage(
                            chat.Id,
                            $"You put on button {callbackQuery.Data}",
                            cancellationToken: cancellationToken);

                        return;
                    }
            }
        }

        private async Task TypeMessageHandler(
            ITelegramBotClient botClient,
            Update update,
            CancellationToken cancellationToken)
        {
            var message = update.Message;
            var user = message!.From;
            var chat = message.Chat;

            _logger.LogInformation($"{user!.FirstName} ({user.Id}) writes message: {message.Text}");

            switch (message.Type)
            {
                case MessageType.Text:
                    {
                        if (message.Text == "/start")
                        {
                            await botClient.SendMessage(
                                chat.Id,
                                "Choose part:\n" +
                                "/inline\n" +
                                "/reply\n",
                                cancellationToken: cancellationToken);

                            return;
                        }

                        if (message.Text == "/inline")
                        {
                            var inlineKeyboard = new InlineKeyboardMarkup(
                                new List<InlineKeyboardButton[]>()
                                {
                                                    new InlineKeyboardButton[]
                                                    {
                                                        InlineKeyboardButton.WithUrl("Site link", "https://habr.com/"),
                                                        InlineKeyboardButton.WithCallbackData("It`s button", "button1"),
                                                    },
                                                    new InlineKeyboardButton[]
                                                    {
                                                        InlineKeyboardButton.WithCallbackData("This another button", "button2"),
                                                        InlineKeyboardButton.WithCallbackData("and this", "button3"),
                                                    },
                                });

                            await botClient.SendMessage(
                                chat.Id,
                                "It`s inline part!",
                                replyMarkup: inlineKeyboard,
                                cancellationToken: cancellationToken);

                            return;
                        }

                        if (message.Text == "/reply")
                        {
                            var replyKeyboard = new ReplyKeyboardMarkup(
                                new List<KeyboardButton[]>()
                                {
                                                    new KeyboardButton[]
                                                    {
                                                        new KeyboardButton("Hi!"),
                                                        new KeyboardButton("Buy!"),
                                                    },
                                                    new KeyboardButton[]
                                                    {
                                                        new KeyboardButton("Call me!")
                                                    },
                                                    new KeyboardButton[]
                                                    {
                                                        new KeyboardButton("Write somebody!")
                                                    }
                                })
                            {
                                ResizeKeyboard = true,
                            };

                            await botClient.SendMessage(
                                chat.Id,
                                "It`s reply part!",
                                replyMarkup: replyKeyboard,
                                cancellationToken: cancellationToken);

                            return;
                        }

                        if (message.Text == "Call me!")
                        {
                            await botClient.SendMessage(
                               chat.Id,
                               "may be some day!",
                               cancellationToken: cancellationToken);

                            return;
                        }

                        return;
                    }
                default:
                    {
                        await botClient.SendMessage(
                            chat.Id,
                            "Text is used only",
                            cancellationToken: cancellationToken);

                        return;
                    }
            }
        }
    }
}
