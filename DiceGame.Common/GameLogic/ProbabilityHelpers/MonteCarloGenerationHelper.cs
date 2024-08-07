using DiceGame.Common.Extensions;

namespace DiceGame.Common.GameLogic.ProbabilityHelpers
{
    public class MonteCarloGenerationHelper
    {
        private static readonly Random _random = new();

        public static double SimulateDiceRolls(int diceAmount, int desiredScore)
        {
            var totalSimulations = 100000;
            var totalSuccesses = 0;
            var totalFailures = 0;

            var lockObject = new object();

            Parallel.For(0, totalSimulations, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, i =>
            {
                var (successes, failures) = SimulateSingleGame(diceAmount, desiredScore);
                lock (lockObject)
                {
                    totalSuccesses += successes;
                    totalFailures += failures;
                }
            });

            return (double)totalSuccesses / (totalSuccesses + totalFailures);
        }

        private static (int successes, int failures) SimulateSingleGame(int diceAmount, int desiredScore)
        {
            return SimulateDiceRollsRecursive(new List<PointableDice>(), diceAmount, 0, desiredScore);
        }

        private static (int successes, int failures) SimulateDiceRollsRecursive(List<PointableDice> keptDice, int remainingDice, int currentScore, int desiredScore)
        {
            if (currentScore >= desiredScore)
            {
                return (1, 0);
            }

            var roll = _random.Throw(remainingDice);
            var dice = PointingSystem.FindDiceToPoint(roll).ToList();
            var choices = GetAllCombinationsOfChoices(dice.ToList());

            var totalSuccesses = 0;
            var totalFailures = 0;

            foreach (var choice in choices)
            {
                var newKeptDice = new List<PointableDice>(keptDice);
                newKeptDice.AddRange(choice);

                var choiceScore = PointingSystem.CalculatePointsFromDice(choice);
                var choiceDiceCount = choice.Sum(x => x.DiceCount);
                var remainingDiceRec = remainingDice - choiceDiceCount == 0
                    ? 6
                    : remainingDice - choiceDiceCount;

                var (successes, failures) = SimulateDiceRollsRecursive(newKeptDice, remainingDiceRec, currentScore + choiceScore, desiredScore);
                totalSuccesses += successes;
                totalFailures += failures;
            }


            if (!choices.Any())
            {
                totalFailures++;
            }

            return (totalSuccesses, totalFailures);
        }

        private static List<List<PointableDice>> GetAllCombinationsOfChoices(List<PointableDice> choices)
        {
            var result = new List<List<PointableDice>>();
            var n = choices.Count;

            var subsetCount = 1 << n;

            for (var i = 1; i < subsetCount; i++)
            {
                var subset = new List<PointableDice>();

                for (var j = 0; j < n; j++)
                {
                    if ((i & 1 << j) != 0)
                    {
                        subset.Add(choices[j]);
                    }
                }

                result.Add(subset);
            }

            return result;
        }
    }
}
