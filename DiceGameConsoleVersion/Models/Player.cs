namespace DiceGameConsoleVersion.Models
{
    public class Player
    {
        public string? Name { get; set; }
        public int Score { get; set; }
        public GamePhase CurrentPlayerPhase { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Player other = (Player)obj;
            return Name == other.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Score, CurrentPlayerPhase);
        }


    }
}
