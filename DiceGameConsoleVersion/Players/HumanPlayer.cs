﻿using DiceGame.Common.Enums;
using DiceGame.Common.GameLogic;
using DiceGame.Common.Players;

namespace DiceGameConsoleVersion.Logic
{
    public class HumanPlayer : IPlayer
    {
        public string? Name { get; init; }
        public int Score { get; set; }
        public GamePhase CurrentGamePhase { get; set; }
        public int MoveNumber { get; set; }

        public HumanPlayer(string name)
        {
            Name = name;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            IPlayer other = (IPlayer)obj;
            return Name == other.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Score, CurrentGamePhase);
        }

        public IEnumerable<PointableDice> ChooseDice(IEnumerable<PointableDice> diceToPoint, int alreadyPointedDice)
        {
            var incorrectInput = true;
            var selectedDice = new List<PointableDice>();
            while (incorrectInput)
            {
                var values = ConsoleManagement.GetInputAndRetrieveValues();
                selectedDice = PointableDice.GetAllPointableDiceByStringInput(values);

                if (!PointableDice.ValidateInput(selectedDice, diceToPoint))
                {
                    Console.WriteLine("Incorrect selection. Please select correct dice.");
                    continue;
                }

                foreach (var die in selectedDice)
                {
                    if (diceToPoint.Any(x => x.Score == die.Score && x.DiceCount >= die.DiceCount))
                    {
                        if (!PointingSystem.SingleDicePoints.ContainsKey(die.Score) && die.DiceCount < 3)
                        {
                            Console.WriteLine("Only 1 and 5 can be scored as a single dice. The rest has to be thrown in quantity of 3.");
                            incorrectInput = true;
                            break;
                        }
                        incorrectInput = false;
                    }
                }
            }

            return selectedDice;
        }

        public bool EndTurn(int roundScore, GameHistory history, int alreadyPointedDice)
        {
            Console.WriteLine("Your score is {0}. Do you wish to continue?", roundScore);
            var response = Console.ReadLine();
            if (response != "Y")
            {
                return true;
            }
            return false;
        }
    }
}
