using DiceGame.Common.Players.Bots;

namespace DiceGame.UnitTests.Helpers
{
    public class GameStateOverviewBuilder
    {
        private readonly List<(string Name, GamePhase GamePhase, int Score)> _playerData = [];
        private IPlayer? _testPlayer;
        private int _testPlayersScore;
        private GamePhase _testPlayersGamePhase;
        private IEnumerable<int>? _testPlayersHistoryOfThrows;

        public GameStateOverviewBuilder WithPlayer(string name, GamePhase gamePhase, int score)
        {
            _playerData.Add((name, gamePhase, score));
            return this;
        }

        public GameStateOverviewBuilder WithPlayerToTest(IPlayer player, GamePhase gamePhase, int score)
        {
            _testPlayer = player;
            _testPlayersScore = score;
            _testPlayersGamePhase = gamePhase;
            return this;
        }

        public GameStateOverviewBuilder WithPlayerHistory(IEnumerable<int> scores)
        {
            _testPlayersHistoryOfThrows = scores;
            return this;
        }

        public GameStateOverview Build()
        {
            var players = new List<IPlayer>();
            foreach (var item in _playerData)
            {
                players.Add(new NoRiskBotPlayer(item.Name));
            }

            if (_testPlayer != null)
            {
                players.Add(_testPlayer);
            }

            var gameState = new GameState(players);

            foreach (var item in _playerData)
            {
                var playerInfo = gameState.Leaderboard.First(x => x.Name == item.Name);
                if (playerInfo != null)
                {
                    playerInfo.Score = item.Score;
                    playerInfo.CurrentGamePhase = item.GamePhase;
                }
            }
            if (_testPlayersHistoryOfThrows is not null)
            {
                foreach (var historyThrow in _testPlayersHistoryOfThrows!)
                {
                    gameState.History.Add((_testPlayer!.Id, historyThrow));
                }
            }
            gameState.Leaderboard.First(x => x.Id == _testPlayer!.Id).CurrentGamePhase = _testPlayersGamePhase;
            gameState.Leaderboard.First(x => x.Id == _testPlayer!.Id).Score = _testPlayersScore;
            gameState.Leaderboard = [.. gameState.Leaderboard.OrderByDescending(x => x.Score)];

            return new GameStateOverview(gameState, _testPlayer!.Id);
        }
    }
}