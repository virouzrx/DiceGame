namespace DiceGameConsoleVersion.GameLogic.ProbabilityHelpers
{
    public class MonteCarloCache
    {
        public int DiceCount { get; set; }
        public int DesiredPoints { get; set; }
        public double Probability { get; set; }
        public MonteCarloCache(int diceCount, int desiredPoints, double probability) 
        { 
            DiceCount = diceCount;
            DesiredPoints = desiredPoints;
            Probability = probability;
        }
    }
}
