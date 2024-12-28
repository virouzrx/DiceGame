using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DiceGame.ConsoleVersion.Extensions;

namespace DiceGameConsoleVersion
{
    public class Program
    {
        public static async Task Main()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.RegisterServicesNecessaryForGame();
                    services.AddHostedService<Game>();
                })
                .Build();

                await host.RunAsync();
        }
    }
}