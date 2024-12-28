using System.Collections.Concurrent;

namespace DiceGame.ConsoleVersion.Utilities
{
    public class GameResultsCollector
    {
        private readonly ConcurrentDictionary<string, int> _winners = new();

        public void AddWinner(string winner)
        {
            _winners.AddOrUpdate(winner, 1, (_, currentCount) => currentCount + 1);
        }

        public IDictionary<string, int> GetWinners()
        {
            return new Dictionary<string, int>(_winners);
        }
    }
}