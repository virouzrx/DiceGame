using DiceGameConsoleVersion.Models;
using System.Text.RegularExpressions;

namespace DiceGameConsoleVersion
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

        public static void DisplayLeaderboard(List<Player> players)
        {
            Console.WriteLine("------------------------");
            foreach (var player in players.OrderByDescending(x => x.Score)) 
            {
                Console.WriteLine($"{player.Name}: {player.Score}");
            }
            Console.WriteLine("------------------------");
        }
    }
}
