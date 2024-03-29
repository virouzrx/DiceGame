﻿namespace DiceGameConsoleVersion.Models
{
    public class PointableDice
    {
        public int Score { get; set; }
        public int Count { get; set; }

        public PointableDice(int score, int count)
        {
            Score = score;
            Count = count;
        }

        public PointableDice(string[] input)
        {
            Score = Int32.Parse(input[0]);
            Count = Int32.Parse(input[1]);
        }

        public static List<PointableDice> GetAllPointableDiceByStringInput(IEnumerable<string> input) 
        {
            var dice = new List<PointableDice>();
            foreach (var item in input)
            {
                var die = new PointableDice(item
                            .ToString()
                            .Trim('(', ')')
                            .Split(','));

                dice.Add(die);
            }

            return dice;
        }

        public static bool ValidateInput(IEnumerable<PointableDice> selection, IEnumerable<PointableDice> dice)
        {
            if (selection.GroupBy(x => x.Score).Any(x => x.Count() > 1))
            {
                return false;
            }

            foreach (var item in selection)
            {
                //user selected dice that are not pointable
                if (!dice.Any(x => x.Score == item.Score))
                {
                    return false;
                }
                //user selected too many dice
                if(dice.First(x => x.Score == item.Score).Count < item.Count)
                {
                    return false;
                }
                //user selected one die more than once 
                if (item.Count < 1)
                {
                    return false;
                }
            }
            
            return true;
        }
    }
}
