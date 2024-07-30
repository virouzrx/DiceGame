using DiceGameConsoleVersion.Logic;

namespace DiceGameConsoleVersion.GameLogic
{
    public class GameHistory
    {
        public List<List<IPlayer>> History { get; set; } = new List<List<IPlayer>>();

        public bool PlayerScoredInLastRounds(string playerName, int roundsAmountToCheck)
        {
            return History
                .TakeLast(roundsAmountToCheck)
                .SelectMany(x => x
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

        public bool IsPlayerLast(string Name)
        {
            var scoreBoard = GetLastHistoryItem();
            var playerIndex = scoreBoard.IndexOf(scoreBoard.First(x => x.Name == Name));
            if (playerIndex == scoreBoard.Count - 1)
            {
                return true;
            }
            return false;
        }
    }
}
