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
            //todo: finish risky bot
            //      implement guid to player class
            //      move project to dotnet 8      
            var players = new List<IPlayer>
            {
                playerFactory.CreatePlayer(PlayerType.Human, "Janek"),
                playerFactory.CreatePlayer(PlayerType.Bot, "Czesiek", BotType.ModerateRisk),
            };

            var game = new Game(players);
            game.StartGame();
        }
    }
}