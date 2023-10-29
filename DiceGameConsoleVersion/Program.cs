using System.Text.RegularExpressions;
using static System.Int32;

namespace DiceGameConsoleVersion
{
    public class Player
    {
        public string? Name { get; set; }
        public int Score { get; set; }
        public GamePhase CurrentPlayerPhrase { get; set; }
    }

    public enum GamePhase
    {
        NotEntered = 0,
        Entered = 1,
        Finishing = 2,
        Finished = 3
    }
    
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
            Score = Parse(input[0]);
            Count = Parse(input[1]);
        }
    }

    public class PlayerTemporaryScore
    {
        public int DiceAmount { get; set; }
        public int Score { get; set; }

        public PlayerTemporaryScore(string[] input)
        {
            Score = Parse(input[0]);
            DiceAmount = Parse(input[1]);
        }
    }

    public class Program
    {
        private static readonly Dictionary<int,int> SingleDicePoints = new Dictionary<int, int>
        {
            { 1, 10 },
            { 5, 5 }
        };
        
        List<int> Throw(int diceAmount = 6)
        {
            Random rnd = new();
            var hand = new List<int>();
            for (var i = 0; i < diceAmount; i++)
            {
                hand.Add(rnd.Next(1, 6));
            }

            return hand;
            //return new List<int> { 4, 4, 4, 5, 5, 5, };
        }

        private static int CalculatePointsFromDice(int dieScore, int count)
        {
            var diePointValue = dieScore == 1 ? 10 : dieScore;
            return (int)(count < 3 
                ? SingleDicePoints[dieScore] * count 
                : diePointValue * 10 * Math.Pow(2, Math.Abs(3 - count)));
        }

        private static int CalculatePointsFromDice(IEnumerable<PointableDice> dice)
        {
            return dice.Sum(die => CalculatePointsFromDice(die.Score, die.Count));
        }

        private static List<PointableDice> FindDiceToPoint(IEnumerable<int> hand)
        {
            var pointableDice = hand
                .GroupBy(x => x)
                .Select(g => new PointableDice(g.Key, g.Count()))
                .Where(p => p.Count > 2 || p.Score is 1 or 5)
                .ToList();
            return pointableDice.Count > 0 ? pointableDice : new List<PointableDice>();
        }

        private static void DisplayTheDiceThrown(List<PointableDice> dice)
        {
            foreach (var die in dice)
            {
                if (die.Count < 2)
                {
                    Console.WriteLine(die.Score);
                }
                else
                {
                    Console.WriteLine($"{string.Join(", ", Enumerable.Repeat(die.Score.ToString(), die.Count))}");
                }
            }
        }

        private static void MakeNextMove(Player player, int alreadyPointedDice, int playerScore, int moveNumber)
        {
            if (alreadyPointedDice == 6)
            {
                PlayerMove(player, 0, playerScore, moveNumber + 1);
            }
            PlayerMove(player, alreadyPointedDice, playerScore);
        }

        private static void PlayerMove(Player player, int alreadyPointedDice = 0, int playerScore = 0, int moveNumber = 1)
        {
            Program p = new();
            var playerThrow = p.Throw(6 - alreadyPointedDice);
            var diceToPoint = FindDiceToPoint(playerThrow);
            Console.WriteLine($"{moveNumber} throw: {string.Join(", ", playerThrow)}");
            Console.WriteLine("-------------");
            if (!diceToPoint.Any())
            {
                if (alreadyPointedDice != 0)
                {
                    Console.WriteLine("No dice to point was thrown.");
                    return;
                }
                playerScore += -50;
                Console.WriteLine("No dice to point was thrown by player. -50 points.");
                return;
            }

            DisplayTheDiceThrown(diceToPoint);

            if (diceToPoint.Sum(die => die.Count) == 6)
            {
                playerScore += CalculatePointsFromDice(diceToPoint);
                Console.WriteLine("All dice were pointable. Current score = {0}", playerScore);
                PlayerMove(player,0, playerScore, moveNumber + 1);
            }

            Console.WriteLine("Select the dice to point.");
            var input = Console.ReadLine();
            const string multipleValuesPattern = @"\([^)]+\)";

            if (!string.IsNullOrEmpty(input))
            {
                var isMatch = Regex.IsMatch(input, multipleValuesPattern);
                if (isMatch)
                {
                    Regex regex = new Regex(multipleValuesPattern);
                    MatchCollection matches = regex.Matches(input);
                    foreach (Match match in matches.Cast<Match>())
                    {
                        var pointableDice = new PointableDice(match.Value
                            .ToString()
                            .Trim('(', ')')
                            .Split(','));
                    }
                    
                    var dieFromThrow = diceToPoint.FirstOrDefault(x => x.Score == selection.Score && x.Count >= selection.Count);
                    if (dieFromThrow != null) 
                    {
                        if (!SingleDicePoints.ContainsKey(dieFromThrow.Score) && dieFromThrow.Count < 3)
                        {
                            Console.WriteLine("Only 1 and 5 can be scored as a single dice. The rest has to be thrown in quantity of 3.");
                            return;
                        }
                        playerScore += CalculatePointsFromDice(selection.Score, selection.Count);
                        alreadyPointedDice += selection.Count;

                        if (player.CurrentPlayerPhrase == GamePhase.Entered)
                        {
                            if (playerScore < 30)
                            {
                                Console.WriteLine("Your score is {0}", playerScore);
                                PlayerMove(player, alreadyPointedDice, playerScore);
                            }
                            Console.WriteLine("Your score is {0}. Do you wish to continue?", playerScore);
                            var response = Console.ReadLine();
                            if (response != "Y")
                            {
                                player.Score += playerScore;
                                return;
                            }
                            MakeNextMove(player, alreadyPointedDice, playerScore, moveNumber);
                        }

                        if (playerScore < 100)
                        {
                            Console.WriteLine("Your score is {0}", playerScore);
                            MakeNextMove(player, alreadyPointedDice, playerScore, moveNumber);
                        }
                        else
                        {
                            if (player.CurrentPlayerPhrase == GamePhase.Finishing)
                            {
                                player.CurrentPlayerPhrase = GamePhase.Finished;
                                return;
                            }
                            Console.WriteLine("Your score is {0}. Do you wish to continue?", playerScore);
                            var response = Console.ReadLine();
                            if (response != "Y")
                            {
                                player.Score += playerScore;
                                return;
                            }
                            MakeNextMove(player, alreadyPointedDice, playerScore, moveNumber);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Incorrect selection. Please select correct dice.");
                    }
                }
                else
                {
                    Console.WriteLine("Incorrect selection. Please select correct dice.");
                }
            }
            else
            {
                Console.WriteLine("Incorrect selection. Please select correct dice.");
            }
        }

        public static void Main()
        {

            Console.WriteLine("xD");
            var players = new List<Player>
            {
                new() { Name = "Jan", Score = 0, CurrentPlayerPhrase = 0 },
                new() { Name = "Kamil", Score = 0, CurrentPlayerPhrase = 0 },
                new() { Name = "Stefan", Score = 0, CurrentPlayerPhrase = 0 },
                new() { Name = "Tomasz", Score = 0, CurrentPlayerPhrase = 0 },
            };
            while (!players.Any(x => x.CurrentPlayerPhrase == GamePhase.Finished))
            {
                foreach (var player in players)
                {
                    PlayerMove(player);
                }
            }
        }
    }
}