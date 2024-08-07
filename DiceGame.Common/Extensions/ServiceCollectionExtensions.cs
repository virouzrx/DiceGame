using Microsoft.Extensions.DependencyInjection;

namespace DiceGame.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterServicesNecessaryForGame(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ProbabilityHelper>();
            serviceCollection.AddScoped<PlayerFactory>();
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
