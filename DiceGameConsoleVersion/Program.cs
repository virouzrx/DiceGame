using DiceGameConsoleVersion.Logic;
using DiceGameConsoleVersion.Models;

namespace DiceGameConsoleVersion
{
    public class Program
    {
        public static void Main()
        {
            var players = new List<IPlayer>
            {
                new HumanPlayer { Name = "Jan", Score = 0, CurrentGamePhase = 0, PlayerType = PlayerType.Real },
                new HumanPlayer { Name = "Kamil", Score = 0, CurrentGamePhase = 0, PlayerType = PlayerType.Real },
                new HumanPlayer { Name = "Stefan", Score = 0, CurrentGamePhase = 0, PlayerType = PlayerType.Real },
                new HumanPlayer { Name = "Tomasz", Score = 0, CurrentGamePhase = 0, PlayerType = PlayerType.Real },
            };

            var game = new Game(players);
            game.StartGame();
        }
    }
}