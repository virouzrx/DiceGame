using System.Text.RegularExpressions;

namespace DiceGameConsoleVersion.Utilities
{
    public class ConsoleManagement
    {
        private const string entireInputMatch = @"^\(\d,[1-6]\)(,\(\d,[1-6]\))*$";
        private const string multipleValuesPattern = @"\((?:[1-6],[1-6](?:,\s*)?)+\)"; //this should validate both if the format is correct and if the player chose correct dice

        public static IEnumerable<string> GetInputAndRetrieveValues()
        {
            while (true)
            {
                Console.WriteLine("Select the dice to point.");
                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input) || !Regex.IsMatch(input, entireInputMatch))
                {
                    Console.WriteLine("Incorrect selection. Please select correct dice.");
                    continue;
                }
                return Regex.Matches(input, multipleValuesPattern).Select(v => v.Value);
            }
        }

        public static void DisplayCurrentPlayerInfo(bool useConsole, string name, GamePhase gamePhase, int moveNumber, IEnumerable<int> playerThrow)
        {
            if (!useConsole)
                return;
            Console.WriteLine($"Player: {name}, Phase: {gamePhase}, {moveNumber} throw: {string.Join(", ", playerThrow)}");
            Console.WriteLine("------------------------");
        }

        public static void DisplayLeaderboard(bool useConsole, List<PlayerInfo> players)
        {
            if (!useConsole)
                return;

            Console.WriteLine("------------------------");
            foreach (var player in players.OrderByDescending(x => x.Score))
            {
                Console.WriteLine($"{player.Name}: {player.Score}");
            }
            Console.WriteLine("------------------------");
        }

        public static void DisplayTheDiceThrown(bool useConsole, IEnumerable<PointableDice> dice)
        {
            if (!useConsole)
                return;
            foreach (var die in dice)
            {
                if (die.DiceCount < 2)
                {
                    Console.WriteLine(die.Score);
                }
                else
                {
                    Console.WriteLine($"{string.Join(", ", Enumerable.Repeat(die.Score.ToString(), die.DiceCount))}");
                }
            }
        }

        public static void DisplayNoDiceToPointMessage(bool useConsole)
        {
            if (!useConsole)
                return;
            Console.WriteLine("No dice to point was thrown.\n");
        }

        public static void DisplayNoDiceToPointWithMinusPointsMessage(bool useConsole)
        {
            if (!useConsole)
                return;
            Console.WriteLine("No dice to point was thrown by player. -50 points.");
        }

        public static void DisplayNoDiceToPointAtAllMessage(bool useConsole)
        {
            if (!useConsole)
                return;
            Console.WriteLine("No dice to point was thrown in first throw.");
        }

        public static void DisplayPlayerScore(bool useConsole, string playerName, int playerScore)
        {
            if (!useConsole)
                return;
            Console.WriteLine($"{playerName}'s score is {playerScore}");
        }
    }
}
