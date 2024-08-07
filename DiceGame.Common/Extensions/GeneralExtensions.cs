using DiceGame.Common.Players;
using System.Text.Json;

namespace DiceGame.Common.Extensions
{
    public static class GeneralExtensions
    {
        public static T Clone<T>(T obj) where T : class
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            string jsonString = JsonSerializer.Serialize(obj);

            return JsonSerializer.Deserialize<T>(jsonString)!;
        }

        public static List<PlayerInfo> OrderByScore(this List<PlayerInfo> state)
        {
            return [.. state.OrderByDescending(x => x.Score)];
        }

        public static PlayerInfo? GetPlayerWithHigherIndex(this List<PlayerInfo> players, Guid id)
        {
            int index = players.IndexOf(players.First(x => x.Id == id));

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

        public static int IndexOf<T>(this IReadOnlyList<T> list, T item) where T : class
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Equals(item)) return i;
            }
            return -1;
        }
    }
}
