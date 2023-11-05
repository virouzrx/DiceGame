namespace DiceGameConsoleVersion.Models
{
    public class PlayerTemporaryScore
    {
        public int DiceAmount { get; set; }
        public int Score { get; set; }

        public PlayerTemporaryScore(string[] input)
        {
            Score = Int32.Parse(input[0]);
            DiceAmount = Int32.Parse(input[1]);
        }
    }
}
