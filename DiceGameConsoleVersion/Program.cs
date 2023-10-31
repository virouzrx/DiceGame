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
        private static readonly Dictionary<int, int> SingleDicePoints = new Dictionary<int, int>
        {
            { 1, 10 },
            { 5, 5 }
        };

        static List<int> Throw(int diceAmount = 6)
        {
            Random rnd = new();
            var hand = new List<int>();
            for (var i = 0; i < diceAmount; i++)
            {
                hand.Add(rnd.Next(1, 6));
            }

            return hand;
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

        private static void MakeNextMove(Player player, int alreadyPointedDice, int playerScore, int moveNumber, bool canStillThrow = true)
        {
            if (!canStillThrow)
            {
                return;
            }
            if (alreadyPointedDice == 6)
            {
                PlayerMove(player, 0, playerScore, moveNumber + 1);
            }
            PlayerMove(player, alreadyPointedDice, playerScore, moveNumber + 1);
        }

        public static void PlayerMove(Player player, int alreadyPointedDice = 0, int playerScore = 0, int moveNumber = 1)
        {
            //start
            Program p = new();
            PointingSystem pointingSystem = new(SingleDicePoints);
            var playerThrow = Throw(6 - alreadyPointedDice);
            var diceToPoint = pointingSystem.FindDiceToPoint(playerThrow);
            Console.WriteLine($"Player: {player.Name}, Phase: {player.CurrentPlayerPhrase}, {moveNumber} throw: {string.Join(", ", playerThrow)}");
            Console.WriteLine("-------------");
            //-50 scenario
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
            //all dice are pointable
            if (diceToPoint.Sum(die => die.Count) == 6)
            {
                playerScore += pointingSystem.CalculatePointsFromDice(diceToPoint);
                Console.WriteLine("All dice were pointable. Current score = {0}", playerScore);
                PlayerMove(player, 0, playerScore, moveNumber + 1);
            }

            //selection
            const string entireInputMatch = @"^\(\d,[1-6]\)(,\(\d,[1-6]\))*$";
            const string multipleValuesPattern = @"\((?:[1-6],[1-6](?:,\s*)?)+\)"; //this should validate both if the format is correct and if the player chose correct dice
            var regex = new Regex(entireInputMatch);
            var tempscore = 0;
            bool incorrectInput = true;
            while (incorrectInput)
            {
                Console.WriteLine("Select the dice to point.");
                var input = Console.ReadLine();

                if (!string.IsNullOrEmpty(input))
                {
                    if (regex.IsMatch(input))
                    {  
                        MatchCollection matches = Regex.Matches(input, multipleValuesPattern);

                        foreach (Match match in matches.Cast<Match>())
                        {
                            var pointableDice = new PointableDice(match.Value
                                .ToString()
                                .Trim('(', ')')
                                .Split(','));

                            //dice which player chose
                            if (diceToPoint.Any(x => x.Score == pointableDice.Score && x.Count >= pointableDice.Count))
                            {
                                if (!SingleDicePoints.ContainsKey(pointableDice.Score) && pointableDice.Count < 3)
                                {
                                    Console.WriteLine("Only 1 and 5 can be scored as a single dice. The rest has to be thrown in quantity of 3.");
                                    break;
                                }
                                incorrectInput = false;
                                tempscore += pointingSystem.CalculatePointsFromDice(pointableDice.Score, pointableDice.Count);
                                alreadyPointedDice += pointableDice.Count;
                            }
                        }
                    }
                    else //incorrect selection
                    {
                        Console.WriteLine("Incorrect selection. Please select correct dice.");
                    }
                }
                else //no match for regex
                {
                    Console.WriteLine("Incorrect selection. Please select correct dice.");
                }
            }
            playerScore += tempscore;
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
                    Console.WriteLine($"{player.Name}'s score: {player.Score}");
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
                    player.CurrentPlayerPhrase = GamePhase.Entered;
                    Console.WriteLine($"{player.Name}'s score: {player.Score}");
                    return;
                }
                MakeNextMove(player, alreadyPointedDice, playerScore, moveNumber);
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