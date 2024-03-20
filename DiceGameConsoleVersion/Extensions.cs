using DiceGameConsoleVersion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DiceGameConsoleVersion
{
    public static class Extensions
    {
        public static T Clone<T>(T obj) where T : class
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            string jsonString = JsonSerializer.Serialize(obj);

            return JsonSerializer.Deserialize<T>(jsonString);
        }

        public static List<Player> OrderByScore(this List<Player> playerList)
        {
            return playerList.OrderByDescending(x => x.Score).ToList();
        }

        public static Player GetPlayerByName(this List<Player> playerList, string name)
        {
            return playerList.First(p => p.Name == name);
        }
        
        public static Player? GetPlayerWithHigherIndex(this List<Player> players, Player player)
        {
            int index = players.IndexOf(player);

            if (index > 0)
            {
                return players[index - 1];
            }

            return null;
        }
    }
}
