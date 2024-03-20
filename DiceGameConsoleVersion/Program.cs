using DiceGameConsoleVersion.Models;

namespace DiceGameConsoleVersion
{
    public class Program
    {
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

        public static int PlayersTurn(Player player)
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
                var diceToPoint = PointingSystem.FindDiceToPoint(playerThrow);
                Console.WriteLine($"Player: {player.Name}, Phase: {player.CurrentPlayerPhase}, {moveNumber} throw: {string.Join(", ", playerThrow)}");
                Console.WriteLine("------------------------");
                //-50 scenario
                if (!diceToPoint.Any())
                {
                    if (alreadyPointedDice != 0)
                    {
                        Console.WriteLine("No dice to point was thrown.\n");
                        return 0;
                    }
                    Console.WriteLine("No dice to point was thrown by player. -50 points.");
                    return -50;
                }

                DisplayTheDiceThrown(diceToPoint);
                //all dice are pointable
                //todo => check if dice already pointed and dice thrown are all pointable
                if (diceToPoint.Sum(die => die.Count) == 6)
                {
                    playerScore += PointingSystem.CalculatePointsFromDice(diceToPoint);
                    Console.WriteLine("All dice were pointable. Current score = {0}", playerScore);
                    continue;
                }

                //selection
                var tempscore = 0;
                bool incorrectInput = true;
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
                playerScore += tempscore;
                if (player.CurrentPlayerPhase == GamePhase.Entered)
                {
                    if (playerScore < 30)
                    {
                        Console.WriteLine("Your score is {0}", playerScore);
                        continue;
                    }
                    if (alreadyPointedDice < 6)
                    {
                        Console.WriteLine("Your score is {0}. Do you wish to continue?", playerScore);
                        var response = Console.ReadLine();
                        if (response != "Y")
                        {
                            return playerScore;
                        }
                    }
                }
                else
                {
                    if (playerScore < 100)
                    {
                        Console.WriteLine("Your score is {0}", playerScore);
                    }
                    else
                    {
                        if (player.CurrentPlayerPhase == GamePhase.Finishing)
                        {
                            player.CurrentPlayerPhase = GamePhase.Finished;
                            return playerScore;
                        }
                        Console.WriteLine("Your score is {0}. Do you wish to continue?", playerScore);
                        var response = Console.ReadLine();
                        if (response != "Y")
                        {
                            return playerScore;
                        }
                    }
                }
            }
            return playerScore;
        }

        public static void StartGame(List<Player> players)
        {
            var playerNames = players.Select(p => p.Name);
            while (!players.Any(x => x.CurrentPlayerPhase == GamePhase.Finished))
            {
                foreach (var name in playerNames)
                {
                    ConsoleManagement.DisplayLeaderboard(players);
                    var player = players.First(p => p.Name == name);
                    var score = PlayersTurn(player);
                    if (score != 0)
                    {
                        PointingSystem.UpdateScoreboard(players, player, score);
                    }

                    if (player.CurrentPlayerPhase == GamePhase.Finished)
                    {
                        Console.WriteLine($"{name} wins! Congratulations!");
                        return;
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

            StartGame(players);
        }
    }

}