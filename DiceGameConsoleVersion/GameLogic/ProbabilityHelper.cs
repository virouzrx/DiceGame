namespace DiceGameConsoleVersion.GameLogic
{
    internal class ProbabilityHelper
    {
        public static List<List<int>> GenerateDiceCombinations(int numOfDice)
        {
            List<List<int>> combinations = new();
            List<int> currentCombination = new(new int[numOfDice]);
            GenerateDiceCombinationsHelper(combinations, currentCombination, 0, numOfDice);
            return combinations;
        }

        private static void GenerateDiceCombinationsHelper(List<List<int>> combinations, List<int> currentCombination, int index, int numOfDice)
        {
            if (index == numOfDice)
            {
                combinations.Add(new List<int>(currentCombination));
                return;
            }

            for (int i = 1; i <= 6; i++)
            {
                currentCombination[index] = i;
                GenerateDiceCombinationsHelper(combinations, currentCombination, index + 1, numOfDice);
            }
        }

        public static double CalculateProbabilityOfThrowingHigherScore(int score, int alreadyPointedDice)
        {
            var allPossibleCombinations = GenerateDiceCombinations(6 - alreadyPointedDice);
            var pointableCombinations = FindPointableCombinations(allPossibleCombinations);
            var combinationsThatGiveScoreHigherThanWanted = 0;
            foreach (var combination in pointableCombinations)
            {
                var pointableDiceFromCombination = combination
                    .GroupBy(n => n)
                    .Select(g => new PointableDice(g.Key, g.Count()))
                    .ToList();

                if (PointingSystem.CalculatePointsFromDice(pointableDiceFromCombination) >= score)
                {
                    combinationsThatGiveScoreHigherThanWanted++;
                }
            }

            return Math.Round((double)combinationsThatGiveScoreHigherThanWanted / allPossibleCombinations.Count, 2);
        }

        private static List<List<int>> FindPointableCombinations(List<List<int>> allCombinations)
        {
            List<List<int>> pointableCombinations = new();

            foreach (var combination in allCombinations)
            {
                if (HasCombinationSatisfyingConditions(combination, x => x.FindAll(val => val == x[0]).Count >= 3 || x.Contains(1) || x.Contains(5)))
                {
                    pointableCombinations.Add(combination);
                }
            }

            return pointableCombinations;
        }

        private static bool HasCombinationSatisfyingConditions(List<int> combination, Func<List<int>, bool> condition)
        {
            return condition(combination);
        }

        public static double CalculateProbabilityOfThrowingSomethingPointable(int numberOfDiceToThrow)
        {
            var allCombinations = GenerateDiceCombinations(numberOfDiceToThrow);
            var pointableCombinations = FindPointableCombinations(allCombinations);
            var unpointableCombinations = allCombinations.Except(pointableCombinations).ToList();
            return Math.Round((double)pointableCombinations.Count / allCombinations.Count, 2);
        }
    }
}
