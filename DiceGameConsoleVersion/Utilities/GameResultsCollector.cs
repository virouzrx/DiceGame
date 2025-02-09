using System.Collections.Concurrent;

namespace DiceGame.ConsoleVersion.Utilities
{
    public class GameResultsCollector
    {
        private readonly ConcurrentDictionary<BotType, int> _winners = new();

        public void AddWinner(BotType winner)
        {
            _winners.AddOrUpdate(winner, 1, (_, currentCount) => currentCount + 1);
        }

        public IDictionary<BotType, int> GetWinners()
        {
            return new Dictionary<BotType, int>(_winners);
        }

        public int GetPlayerScore(BotType botType)
        {
            if (_winners.TryGetValue(botType, out int value))
            {
                return value;
            }
            return 0;
        } 
    }
}