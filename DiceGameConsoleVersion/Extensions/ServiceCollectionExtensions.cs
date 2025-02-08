using DiceGame.ConsoleVersion.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace DiceGame.ConsoleVersion.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterServicesNecessaryForGame(this IServiceCollection serviceCollection, bool useConsole = true)
        {
            if (!serviceCollection.Any(sd => sd.ServiceType == typeof(ProbabilityHelper)))
            {
                serviceCollection.AddSingleton<ProbabilityHelper>();
            }
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
                    playerFactory.CreatePlayer("NoRisk", BotType.NoRisk),
                    playerFactory.CreatePlayer("LittleRisk", BotType.LittleRisk),
                    playerFactory.CreatePlayer("ModerateRisk", BotType.ModerateRisk),
                    playerFactory.CreatePlayer("Risky", BotType.Risky),
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
