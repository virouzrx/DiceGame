namespace DiceGame.Common.GameLogic.ProbabilityHelpers
{
    public class MonteCarloCache(int diceCount, int desiredPoints, double probability)
    {
        public int DiceCount { get; set; } = diceCount;
        public int DesiredPoints { get; set; } = desiredPoints;
        public double Probability { get; set; } = probability;
    }
}
