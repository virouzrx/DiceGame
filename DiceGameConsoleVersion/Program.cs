using DiceGameConsoleVersion.GameLogic;
using DiceGameConsoleVersion.Logic;
using DiceGameConsoleVersion.Models;
using DiceGameConsoleVersion.Players;

namespace DiceGameConsoleVersion
{
    public class Program
    {
        public static void Main()
        {
            var players = new List<IPlayer>
            {
                new HumanPlayer("Jan", PlayerType.Real),
                new HumanPlayer("Kamil", PlayerType.Real),
                new HumanPlayer("Stefan", PlayerType.Real),
                new LittleRiskBotPlayer { Name = "Bot", PlayerType = PlayerType.Bot }
            };

            var game = new Game(players);
            game.StartGame();
        }
    }
}