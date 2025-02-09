namespace DiceGame.Common.Players
{
    public class GameStateOverview
    {
        public IReadOnlyList<PlayerInfo> Leaderboard { get; init; }
        public IReadOnlyList<int> LastPlayersMoves { get; init; }
        public GameStateOverview(GameState state, Guid id)
        {
            Leaderboard = state.Leaderboard;
            LastPlayersMoves = state.History
                .Where(x => x.Id == id)
                .TakeLast(3)
                .Select(x => x.Score)
                .ToList()
                .AsReadOnly();
            PlayerId = id;
            var playerInfo = Leaderboard.First(x => x.Id == PlayerId);
            PlayerGamePhase = playerInfo.CurrentGamePhase;
            PlayersScore = playerInfo.Score;
        }
 
        private Guid PlayerId { get; init; } 
        public int PlayersScore { get; init; }
        public GamePhase PlayerGamePhase { get; init; }

        public bool PlayerScoredInLastRounds(int roundsToCheck)
        {
            return LastPlayersMoves
                .TakeLast(roundsToCheck)
                .Distinct()
                .Count() > 1;
        }

        public bool IsPlayerLast()
        {
            return Leaderboard[Leaderboard.Count - 1].Id == PlayerId;
        }
    }
}
