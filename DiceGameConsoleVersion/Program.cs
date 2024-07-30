using DiceGameConsoleVersion.Enums;
using DiceGameConsoleVersion.GameLogic;
using DiceGameConsoleVersion.GameLogic.ProbabilityHelpers;
using DiceGameConsoleVersion.Logic;
using DiceGameConsoleVersion.Players;
using Microsoft.Extensions.DependencyInjection;
using System;

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
            var players = new List<IPlayer>
            {
                playerFactory.CreatePlayer(PlayerType.Human, "Janek"),
                playerFactory.CreatePlayer(PlayerType.Human, "Bartek"),
                playerFactory.CreatePlayer(PlayerType.Human, "Czesiek"),
                playerFactory.CreatePlayer(PlayerType.Bot, "Czesiek", BotType.ModerateRisk),
            };

            var game = new Game(players);
            game.StartGame();
        }
    }
}