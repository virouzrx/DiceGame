using Microsoft.Extensions.DependencyInjection;

namespace DiceGameConsoleVersion
{
    public class Program
    {
        public static void Main()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<ProbabilityHelper>();
            serviceCollection.AddScoped<PlayerFactory>();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var probabilityHelper = serviceProvider.GetService<ProbabilityHelper>();
            var playerFactory = new PlayerFactory(probabilityHelper!);     
            for (int i = 0; i < 1000; i++)
            {
                var players = new List<IPlayer>
                {
                    playerFactory.CreatePlayer(PlayerType.Bot, "NoRisk", BotType.NoRisk),
                    playerFactory.CreatePlayer(PlayerType.Bot, "LittleRisk", BotType.LittleRisk),
                    playerFactory.CreatePlayer(PlayerType.Bot, "ModerateRisk", BotType.ModerateRisk),
                    playerFactory.CreatePlayer(PlayerType.Bot, "Risky", BotType.Risky),
                };

                var game = new Game(players);
                game.StartGame();
            }
        }
    }
}