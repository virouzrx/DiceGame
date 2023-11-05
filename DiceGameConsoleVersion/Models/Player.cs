namespace DiceGameConsoleVersion.Models
{
    public class Player
    {
        public string? Name { get; set; }
        public int Score { get; set; }
        public GamePhase CurrentPlayerPhrase { get; set; }
    }
}
