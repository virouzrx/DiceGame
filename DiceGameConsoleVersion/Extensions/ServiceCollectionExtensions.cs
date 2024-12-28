using DiceGame.ConsoleVersion.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace DiceGame.ConsoleVersion.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterServicesNecessaryForGame(this IServiceCollection serviceCollection, bool useConsole = true)
        {
            serviceCollection.AddScoped<ProbabilityHelper>();
            serviceCollection.AddScoped<PlayerFactory>();
            if (useConsole)
            {
                var consoleSetting = new ConsoleSettings
                {
                    UseConsole = useConsole
                };
                serviceCollection.AddSingleton(consoleSetting);
            }
            else
            {
                serviceCollection.AddSingleton<ConsoleSettings>();
            }

            if (!serviceCollection.Any(sd => sd.ServiceType == typeof(GameResultsCollector)))
            {
                serviceCollection.AddSingleton<GameResultsCollector>();
            }
            serviceCollection.AddSingleton<IEnumerable<IPlayer>>(provider =>
            {
                var playerFactory = provider.GetRequiredService<PlayerFactory>()!;
                return
                [
                    playerFactory.CreatePlayer(PlayerType.Bot, "NoRisk", BotType.NoRisk),
                    playerFactory.CreatePlayer(PlayerType.Bot, "LittleRisk", BotType.LittleRisk),
                    playerFactory.CreatePlayer(PlayerType.Bot, "ModerateRisk", BotType.ModerateRisk),
                    playerFactory.CreatePlayer(PlayerType.Bot, "Risky", BotType.Risky),
                ];
            });

            serviceCollection.AddScoped(provider =>
            {
                var players = provider.GetRequiredService<IEnumerable<IPlayer>>();
                return new GameState(players);
            });
            serviceCollection.AddScoped<GameHistory>();
        }
    }
}
