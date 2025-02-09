namespace DiceGame.Common.GameLogic
{
    public class PlayerInfo(Guid guid, string name)
    {
        public Guid Id { get; init; } = guid;
        public int Score { get; set; }
        public string Name { get; init; } = name;
        public GamePhase CurrentGamePhase { get; set; }
        public int MoveNumber { get; set; }
    }
}
