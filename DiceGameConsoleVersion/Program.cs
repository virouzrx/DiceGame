using DiceGameConsoleVersion.Enums;
using DiceGameConsoleVersion.GameLogic;
using DiceGameConsoleVersion.Logic;
using DiceGameConsoleVersion.Players;

namespace DiceGameConsoleVersion
{
    public class Program
    {
        public static void Main()
        {
            var playerFactory = new PlayerFactory();
            var players = new List<IPlayer>
            {
                PlayerFactory.CreatePlayer(PlayerType.Human, "Janek"),
                PlayerFactory.CreatePlayer(PlayerType.Human, "Bartek"),
                PlayerFactory.CreatePlayer(PlayerType.Human, "Czesiek"),
                PlayerFactory.CreatePlayer(PlayerType.Bot, "Czesiek", BotType.ModerateRisk),
            };

            var game = new Game(players);
            game.StartGame();
        }
    }
}