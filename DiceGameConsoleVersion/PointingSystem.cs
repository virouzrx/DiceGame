using DiceGameConsoleVersion.Models;

namespace DiceGameConsoleVersion
{
    public class PointingSystem
    {
        private Dictionary<int, int> SingleDicePoints { get; set; }
        public PointingSystem(Dictionary<int, int> singleDicePoints)
        {
            SingleDicePoints = singleDicePoints;
        }
        public int CalculatePointsFromDice(int dieScore, int count)
        {
            var diePointValue = dieScore == 1 ? 10 : dieScore;
            return (int)(count < 3
                ? SingleDicePoints[dieScore] * count
                : diePointValue * 10 * Math.Pow(2, Math.Abs(3 - count)));
        }

        public int CalculatePointsFromDice(IEnumerable<PointableDice> dice)
        {
            return dice.Sum(die => CalculatePointsFromDice(die.Score, die.Count));
        }

        public List<PointableDice> FindDiceToPoint(IEnumerable<int> hand)
        {
            var pointableDice = hand
                .GroupBy(x => x)
                .Select(g => new PointableDice(g.Key, g.Count()))
                .Where(p => p.Count > 2 || p.Score is 1 or 5)
                .ToList();
            return pointableDice.Count > 0 ? pointableDice : new List<PointableDice>();
        }
    }
}
