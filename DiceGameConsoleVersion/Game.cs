using DiceGameConsoleVersion.Models;

namespace DiceGameConsoleVersion
{
    public class Game
    {
        public List<Player> Players { get; set; }
        public Game(List<Player> players)
        {
            Players = players;
        }
        private static List<int> Throw(int diceAmount = 6)
        {
            Random rnd = new();
            var hand = new List<int>();
            for (var i = 0; i < diceAmount; i++)
            {
                hand.Add(rnd.Next(1, 6));
            }

            return hand;
        }

        public void StartGame()
        {
            while (!Players.Any(x => x.CurrentGamePhase == GamePhase.Finished))
            {
                foreach (var player in Players)
                {
                    ConsoleManagement.DisplayLeaderboard(Players);
                    var score = PlayersTurn(player);
                    if (score != 0)
                    {
                        PointingSystem.UpdateScoreboard(Players, player, score);
                    }

                    if (player.CurrentGamePhase == GamePhase.Finished)
                    {
                        Console.WriteLine($"{player.Name} wins! Congratulations!");
                        return;
                    }
                }
            }
        }

        public static int PlayersTurn(Player player)
        {
            bool playerEndendTheirTurn = false;
            int alreadyPointedDice = 0;
            int playerScore = 0;
            int moveNumber = 0;
            player.MoveNumber++;

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
                Console.WriteLine($"Player: {player.Name}, Phase: {player.CurrentGamePhase}, {moveNumber} throw: {string.Join(", ", playerThrow)}");
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

                ConsoleManagement.DisplayTheDiceThrown(diceToPoint);
                //all dice are pointable
                var diceCount = diceToPoint.Sum(die => die.Count);
                if (diceCount == 6 || (diceCount + alreadyPointedDice == 6 && player.CurrentGamePhase == GamePhase.NotEntered))
                {
                    playerScore += PointingSystem.CalculatePointsFromDice(diceToPoint);
                    Console.WriteLine("All dice were pointable. Current score = {0}", playerScore);
                    continue;
                }

                //selection
                var tempscore = 0;
                tempscore = player.ChooseDice(diceToPoint, ref tempscore, ref alreadyPointedDice);
                //DecideNextMove
                playerScore += tempscore;
                playerEndendTheirTurn = player.EndTurn(playerScore);
            }
            return playerScore;
        }
    }
}
