using DiceGameConsoleVersion.Logic;

namespace DiceGameConsoleVersion.GameLogic
{
    public class GameHistory
    {
        public List<List<IPlayer>> History => new();

        public bool PlayerScoredInLastRounds(string playerName, int roundsAmountToCheck)
        {
            return History.TakeLast(roundsAmountToCheck)
                    .Select(x => x
                        .Where(y => y.Name == playerName)
                        .Select(y => y.Score))
                    .Distinct()
                    .Count() > 1;
        }

        public void UpdatePlayer(IPlayer player) 
        {
            var playerToUpdate = History.Last().Where(p => p.Name == player.Name).First();
            playerToUpdate.Score = player.Score;
            playerToUpdate.CurrentGamePhase = player.CurrentGamePhase;
            playerToUpdate.MoveNumber = player.MoveNumber;
        }

        public List<IPlayer> GetLastHistoryItem()
        {
            return History.Last().OrderByDescending(p => p.Score).ToList();
        }
    }
}
