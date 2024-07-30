namespace DiceGameConsoleVersion.GameLogic
{
    public class PointableDice
    {
        public int Score { get; set; }
        public int DiceCount { get; set; }

        public PointableDice(int score, int count)
        {
            Score = score;
            DiceCount = count;
        }

        public PointableDice(string[] input)
        {
            Score = int.Parse(input[0]);
            DiceCount = int.Parse(input[1]);
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            PointableDice other = (PointableDice)obj;
            return Score == other.Score && DiceCount == other.DiceCount;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Score, DiceCount);
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
                if (dice.First(x => x.Score == item.Score).DiceCount < item.DiceCount)
                {
                    return false;
                }
                //user selected one die more than once 
                if (item.DiceCount < 1)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
