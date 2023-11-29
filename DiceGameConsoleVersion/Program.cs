using DiceGameConsoleVersion.Models;

namespace DiceGameConsoleVersion
{
    public class Program
    {
        private static readonly Dictionary<int, int> SingleDicePoints = new()
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

        public static void PlayersTurn(Player player, PointingSystem pointingSystem)
        {
            bool playerEndendTheirTurn = false;
            int alreadyPointedDice = 0;
            int playerScore = 0;
            int moveNumber = 0;
            while (!playerEndendTheirTurn)
            {
                //start
                moveNumber++;
                if (alreadyPointedDice == 6)
                {
                    alreadyPointedDice = 0;
                }
                var playerThrow = Throw(6 - alreadyPointedDice);
                var diceToPoint = pointingSystem.FindDiceToPoint(playerThrow);
                Console.WriteLine($"Player: {player.Name}, Phase: {player.CurrentPlayerPhase}, {moveNumber} throw: {string.Join(", ", playerThrow)}");
                Console.WriteLine("-------------");
                //-50 scenario
                if (!diceToPoint.Any())
                {
                    if (alreadyPointedDice != 0)
                    {
                        Console.WriteLine("No dice to point was thrown.\n");
                        return;
                    }
                    playerScore += -50;
                    Console.WriteLine("No dice to point was thrown by player. -50 points.");
                    return;
                }

                DisplayTheDiceThrown(diceToPoint);
                //all dice are pointable
                //todo => check if dice already pointed and dice thrown are all pointable
                if (diceToPoint.Sum(die => die.Count) == 6)
                {
                    playerScore += pointingSystem.CalculatePointsFromDice(diceToPoint);
                    Console.WriteLine("All dice were pointable. Current score = {0}", playerScore);
                }

                //selection
                var tempscore = 0;
                bool incorrectInput = true;
                while (incorrectInput)
                {
                    var values = InputManagement.GetInputAndRetrieveValues();

                    foreach (var value in values)
                    {
                        var pointableDice = new PointableDice(value
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
                playerScore += tempscore;
                //todo => add mechanics of substracting player's score if they surpass one another
                if (player.CurrentPlayerPhase == GamePhase.Entered)
                {
                    if (playerScore < 30)
                    {
                        Console.WriteLine("Your score is {0}", playerScore);
                        continue;
                    }
                    Console.WriteLine("Your score is {0}. Do you wish to continue?", playerScore);
                    var response = Console.ReadLine();
                    if (response != "Y")
                    {
                        player.Score += playerScore;
                        Console.WriteLine($"{player.Name}'s score: {player.Score}");
                        break;
                    }
                }

                if (playerScore < 100)
                {
                    Console.WriteLine("Your score is {0}", playerScore);
                }
                else
                {
                    if (player.CurrentPlayerPhase == GamePhase.Finishing)
                    {
                        player.CurrentPlayerPhase = GamePhase.Finished;
                    }
                    Console.WriteLine("Your score is {0}. Do you wish to continue?", playerScore);
                    var response = Console.ReadLine();
                    if (response != "Y")
                    {
                        pointingSystem.UpdateScoreboard(players, player, playerScore)
                        player.Score += playerScore;
                        player.CurrentPlayerPhase = GamePhase.Entered;
                        Console.WriteLine($"{player.Name}'s score: {player.Score}\n");
                        break;
                    }
                }
            }
        }
        public static void Main()
        {
            var players = new List<Player>
            {
                new() { Name = "Jan", Score = 0, CurrentPlayerPhase = 0 },
                new() { Name = "Kamil", Score = 0, CurrentPlayerPhase = 0 },
                new() { Name = "Stefan", Score = 0, CurrentPlayerPhase = 0 },
                new() { Name = "Tomasz", Score = 0, CurrentPlayerPhase = 0 },
            };
            PointingSystem pointingSystem = new(SingleDicePoints);
            while (!players.Any(x => x.CurrentPlayerPhase == GamePhase.Finished))
            {
                foreach (var player in players)
                {
                    PlayersTurn(player, pointingSystem);
                }
            }
        }
    }
}