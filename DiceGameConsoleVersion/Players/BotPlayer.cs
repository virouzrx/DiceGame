﻿using DiceGameConsoleVersion.GameLogic;
using DiceGameConsoleVersion.Models;

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

        public int ChooseDice(List<PointableDice> diceToPoint, ref int tempscore, ref int alreadyPointedDice)
        {
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
    }
}
