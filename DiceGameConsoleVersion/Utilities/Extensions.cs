using System.Text.Json;

namespace DiceGameConsoleVersion.Utilities
{
    public static class Extensions
    {
        public static T Clone<T>(T obj) where T : class
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            string jsonString = JsonSerializer.Serialize(obj);

            return JsonSerializer.Deserialize<T>(jsonString)!;
        }

        public static List<IPlayer> OrderByScore(this List<IPlayer> playerList)
        {
            return playerList.OrderByDescending(x => x.Score).ToList();
        }

        public static IPlayer? GetPlayerWithHigherIndex(this List<IPlayer> players, IPlayer player)
        {
            int index = players.IndexOf(player);

            if (index > 0)
            {
                return players[index - 1];
            }

            return null;
        }

        public static IEnumerable<int> Throw(this Random random, int diceAmount)
        {
            return Enumerable.Range(0, diceAmount)
                         .Select(_ => random.Next(1, 7))
                         .ToList();
        }
    }
}
