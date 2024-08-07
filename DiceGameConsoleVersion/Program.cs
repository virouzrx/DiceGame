using Microsoft.Extensions.DependencyInjection;
using DiceGame.Common.Extensions;

namespace DiceGameConsoleVersion
{
    public class Program
    {
        public static void Main()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.RegisterServicesNecessaryForGame();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var probabilityHelper = serviceProvider.GetService<ProbabilityHelper>();
            var playerFactory = serviceProvider.GetService<PlayerFactory>()!;
            var gameHistory = serviceProvider.GetService<GameHistory>()!;
            var gameState = serviceProvider.GetService<GameState>()!;
            var players = serviceProvider.GetService<IEnumerable<IPlayer>>()!.ToList();
            var game = new Game(players, gameState);
            game.StartGame();
        }
    }
}