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

        private static List<Player> OrderByScore(this List<Player> playerList)
        {
            return playerList.OrderByDescending(x => x.Score).ToList();
        }
    }
}
