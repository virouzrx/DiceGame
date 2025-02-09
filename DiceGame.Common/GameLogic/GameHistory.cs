using DiceGame.Common.Players;

namespace DiceGame.Common.GameLogic
{
    public class GameHistory
    {
        public List<(Guid Id, int Score)> History { get; set; } = [];

        public bool PlayerScoredInLastRounds(Guid id, int roundsAmountToCheck)
        {
            return History
                .Where(x => x.Id == id)
                .TakeLast(roundsAmountToCheck)
                .Select(y => y.Score)
                .Distinct()
                .Count() > 1;
        }
    }
}
