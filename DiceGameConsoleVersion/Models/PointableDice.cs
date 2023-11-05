namespace DiceGameConsoleVersion.Models
{
    public class PointableDice
    {
        public int Score { get; set; }
        public int Count { get; set; }

        public PointableDice(int score, int count)
        {
            Score = score;
            Count = count;
        }

        public PointableDice(string[] input)
        {
            Score = Int32.Parse(input[0]);
            Count = Int32.Parse(input[1]);
        }
    }
}
