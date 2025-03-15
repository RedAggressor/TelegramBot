using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TelegramBot;
using TelegramBot.Configs;
using TelegramBot.Services.Interfaces;
using TelegramBot.Services;
using Microsoft.Extensions.Logging;
using TelegramBot.Handlers.Interfaces;
using TelegramBot.Handlers;

public class Program 
{
    static async Task Main(string[] args)
    {
        void ConfigureServices(ServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection
                .AddOptions<TelegramConfig>()
                .Bind(configuration.GetSection("TelegramBot"));

            serviceCollection
                .AddLogging(configure => configure.AddConsole())
                .AddHttpClient()
                .AddTransient<ITelegramService, TelegramService>()
                .AddTransient<ITelegramHandler, TelegramHandler>()
                .AddTransient<StartUp>();
        }

        ServiceCollection serviceCollection = new ServiceCollection();

        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        ConfigureServices(serviceCollection, configuration);

        var provider = serviceCollection.BuildServiceProvider();

        var app = provider.GetService<StartUp>();
        await app!.Start();
    }
}
