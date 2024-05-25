using DiceGameConsoleVersion.GameLogic;
using DiceGameConsoleVersion.Models;
using System.Text;

namespace DiceGameConsoleVersion.Logic
{
    internal class BotPlayer : IPlayer
    {
        public string Name { get; init; }
        public int Score { get; set; }
        public GamePhase CurrentGamePhase { get; set; }
        public PlayerType PlayerType { get; init; }
        public int MoveNumber { get; set; }
        public DefinedBehavior Behavior { get; init; }

        public int MakeMove(List<PointableDice> diceToPoint, ref int tempscore, ref int alreadyPointedDice, List<List<IPlayer>> history)
        {
            switch (Behavior)
            {
                case DefinedBehavior.NoRisk:
                    return PointingSystem.CalculatePointsFromDice(diceToPoint);
                case DefinedBehavior.LittleRisk:
                    if (CurrentGamePhase == GamePhase.Entered)
                    {
                        var lastTurn = history.Last();
                        var index = lastTurn.IndexOf(this);
                        if (diceToPoint.Count == 1)
                        {
                            var die = diceToPoint.First();

                            if (die.DiceCount == 1)
                            {
                                return PointingSystem.CalculatePointsFromDice(diceToPoint);
                            }

                            if (die.DiceCount > 2 || (diceToPoint.Count == 2 && alreadyPointedDice >= 3))
                            {
                                return PointingSystem.CalculatePointsFromDice(diceToPoint);
                            }

                            return PointingSystem.CalculatePointsFromDice(die.Score, die.DiceCount);
                        }
                        //2 - 3 pointable dice thrown
                        var diceCount = diceToPoint.Sum(x => x.DiceCount);
                        if (alreadyPointedDice < 2)
                        {
                            if (diceToPoint.Any(x => x.DiceCount > 2))
                            {
                                return PointingSystem.CalculatePointsFromDice(diceToPoint.Where(x => x.DiceCount > 2));
                            }

                            if (!diceToPoint.Any(x => x.Score == 1))
                            {
                                return PointingSystem.CalculatePointsFromDice(5, 1);
                            }
                            return PointingSystem.CalculatePointsFromDice(1, 1);
                        }
                        return PointingSystem.CalculatePointsFromDice(diceToPoint);
                    }
                    break;

            }

            return 0;
        }

        public bool EndTurn(int roundScore)
        {
            throw new NotImplementedException();
        }

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

        static List<List<int>> FindPointableCombinations(List<List<int>> allCombinations, Func<List<int>, bool> condition)
        {
            List<List<int>> pointableCombinations = new();

            foreach (var combination in allCombinations)
            {
                if (HasCombinationSatisfyingConditions(combination, condition))
                {
                    pointableCombinations.Add(combination);
                }
            }

            return pointableCombinations;
        }

        static bool HasCombinationSatisfyingConditions(List<int> combination, Func<List<int>, bool> condition)
        {
            return condition(combination);
        }

        static double CalculateProbabilityOfThrowingSomethingPointable(int numberOfDiceToThrow)
        {
            var allCombinations = GenerateDiceCombinations(numberOfDiceToThrow);
            var pointableCombinations = FindPointableCombinations(allCombinations, x => x.FindAll(val => val == x[0]).Count >= 3 || x.Contains(1) || x.Contains(5));
            var unpointableCombinations = allCombinations.Except(pointableCombinations).ToList();
            return Math.Round((double)pointableCombinations.Count / allCombinations.Count, 2);
        }

        public IEnumerable<PointableDice> ChooseDice(List<PointableDice> diceToPoint, int alreadyPointedDice)
        {
            throw new NotImplementedException();
        }

        public bool EndTurn(int roundScore, List<List<IPlayer>> history, int alreadyPointedDice)
        {
            throw new NotImplementedException();
        }
    }
}
