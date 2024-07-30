using System.Text.RegularExpressions;

namespace DiceGameConsoleVersion.Utilities
{
    public class ConsoleManagement
    {
        private const string entireInputMatch = @"^\(\d,[1-6]\)(,\(\d,[1-6]\))*$";
        private const string multipleValuesPattern = @"\((?:[1-6],[1-6](?:,\s*)?)+\)"; //this should validate both if the format is correct and if the player chose correct dice
        private Dictionary<int, int> SingleDicePoints { get; set; }

        public ConsoleManagement(Dictionary<int, int> singleDicePoints)
        {
            SingleDicePoints = singleDicePoints;
        }

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

        public static void DisplayLeaderboard(List<IPlayer> players)
        {
            Console.WriteLine("------------------------");
            foreach (var player in players.OrderByDescending(x => x.Score))
            {
                Console.WriteLine($"{player.Name}: {player.Score}");
            }
            Console.WriteLine("------------------------");
        }

        public static void DisplayTheDiceThrown(IEnumerable<PointableDice> dice)
        {
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
    }
}
