namespace DiceGame.Common.GameLogic
{
    public class GameState(IEnumerable<IPlayer> players)
    {
        public List<PlayerInfo> Leaderboard { get; set; } = InsertPlayers(players);
        public List<(Guid Id, int Score)> History { get; set; } = [];

        public bool IsPlayerLast(Guid id)
        {
            return Leaderboard.OrderBy(x => x.Score).First().Id == id;
        }
        
        private static List<PlayerInfo> InsertPlayers(IEnumerable<IPlayer> players)
        {
            var list = new List<PlayerInfo>();
            foreach (var player in players)
            {
                list.Add(new PlayerInfo(player.Id, player.Name));
            }
            return list;
        }
    }
}
