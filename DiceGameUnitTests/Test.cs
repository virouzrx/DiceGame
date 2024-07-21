using System.Text;

namespace DiceGameUnitTests
{
    public class Test
    {
        static List<List<int>> GenerateDiceCombinations(int numOfDice)
        {
            List<List<int>> combinations = new();
            List<int> currentCombination = new(new int[numOfDice]);
            GenerateDiceCombinationsHelper(combinations, currentCombination, 0, numOfDice);
            return combinations;
        }

        static void GenerateDiceCombinationsHelper(List<List<int>> combinations, List<int> currentCombination, int index, int numOfDice)
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

        static List<List<int>> FindPointableCombinations(List<List<int>> allCombinations)
        {
            List<List<int>> triplets = new();

            foreach (var combination in allCombinations)
            {
                if (HasExactNumberOfAKind(combination))
                {
                    triplets.Add(combination);
                }
            }

            return triplets;
        }

        static bool HasExactNumberOfAKind(List<int> combination)
        {
            HashSet<int> uniqueValues = new HashSet<int>(combination);
            foreach (int value in uniqueValues)
            {
                if (combination.FindAll(x => x == value).Count >= 3 || combination.Contains(1) || combination.Contains(5))
                {
                    return true;
                }
            }
            return false;
        }

        static double CalculateProbabilityOfThrowingSomethingPointable(int numberOfDiceToThrow)
        {
            var allCombinations = GenerateDiceCombinations(numberOfDiceToThrow);
            var pointableCombinations = FindPointableCombinations(allCombinations);
            return Math.Round((double)pointableCombinations.Count / allCombinations.Count, 2);
        }

        [Fact]
        public void DumbTest()
        {
            var probability = CalculateProbabilityOfThrowingSomethingPointable(6);
            var probability1 = CalculateProbabilityOfThrowingSomethingPointable(5);
            var probability2 = CalculateProbabilityOfThrowingSomethingPointable(4);
            var probability3 = CalculateProbabilityOfThrowingSomethingPointable(3);
            var probability4 = CalculateProbabilityOfThrowingSomethingPointable(2);
            var probability5 = CalculateProbabilityOfThrowingSomethingPointable(1);
            var x = 0;
        }
    }
}
