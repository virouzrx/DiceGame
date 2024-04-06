using DiceGameConsoleVersion.Models;

namespace DiceGameConsoleVersion
{
    public class Program
    {
        public static void Main()
        {
            var players = new List<Player>
            {
                new() { Name = "Jan", Score = 0, CurrentGamePhase = 0, PlayerType = PlayerType.Real },
                new() { Name = "Kamil", Score = 0, CurrentGamePhase = 0, PlayerType = PlayerType.Real },
                new() { Name = "Stefan", Score = 0, CurrentGamePhase = 0, PlayerType = PlayerType.Real },
                new() { Name = "Tomasz", Score = 0, CurrentGamePhase = 0, PlayerType = PlayerType.Real },
            };

            var game = new Game(players);
            game.StartGame();
        }
    }
}