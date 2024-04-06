using System.Numerics;

namespace DiceGameConsoleVersion.Models
{
    public class Player
    {
        public string? Name { get; set; }
        public int Score { get; set; }
        public GamePhase CurrentGamePhase { get; set; }
        public PlayerType PlayerType { get; set; }
        public int MoveNumber { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Player other = (Player)obj;
            return Name == other.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Score, CurrentGamePhase);
        }

        public int ChooseDice(List<PointableDice> diceToPoint, ref int tempscore, ref int alreadyPointedDice)
        {
            var incorrectInput = true;
            while (incorrectInput)
            {
                var values = ConsoleManagement.GetInputAndRetrieveValues();
                var selectedDice = PointableDice.GetAllPointableDiceByStringInput(values);

                if (!PointableDice.ValidateInput(selectedDice, diceToPoint))
                {
                    Console.WriteLine("Incorrect selection. Please select correct dice.");
                    continue;
                }

                foreach (var die in selectedDice)
                {
                    if (diceToPoint.Any(x => x.Score == die.Score && x.Count >= die.Count))
                    {
                        if (!PointingSystem.SingleDicePoints.ContainsKey(die.Score) && die.Count < 3)
                        {
                            Console.WriteLine("Only 1 and 5 can be scored as a single dice. The rest has to be thrown in quantity of 3.");
                            break;
                        }
                        incorrectInput = false;
                        tempscore += PointingSystem.CalculatePointsFromDice(die.Score, die.Count);
                        alreadyPointedDice += die.Count;
                    }
                }
            }

            return tempscore;
        }

        public bool EndTurn(int roundScore)
        {
            if (CurrentGamePhase == GamePhase.Entered)
            {
                if (roundScore < 30)
                {
                    Console.WriteLine("Your score is {0}", roundScore);
                    return false;
                }
                Console.WriteLine("Your score is {0}. Do you wish to continue?", roundScore);
                var response = Console.ReadLine();
                if (response != "Y")
                {
                    return true;
                }
                return false;
            }
            else
            {
                if (roundScore < 100)
                {
                    Console.WriteLine("Your score is {0}", roundScore);
                    return false;
                }
                else
                {
                    if (CurrentGamePhase == GamePhase.Finishing)
                    {
                        CurrentGamePhase = GamePhase.Finished;
                        return true;
                    }
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
    }
}
