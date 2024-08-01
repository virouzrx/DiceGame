using DiceGame.Common.Enums;
using DiceGame.Common.Players;
using System.Reflection.Metadata.Ecma335;

namespace DiceGame.Common.GameLogic
{
    public class Game
    {
        public GameHistory GameHistory { get; set; }
        public List<IPlayer> Players { get; set; }
        private readonly Random _random;
        public Game(List<IPlayer> players)
        {
            Players = players;
            GameHistory = new GameHistory();
            _random = new Random();
        }

        public void StartGame()
        {
            while (!Players.Any(x => x.CurrentGamePhase == GamePhase.Finished))
            {
                GameHistory.History.Add(Players);
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
                    GameHistory.UpdatePlayer(player);
                }
            }
        }

        public int PlayersTurn(IPlayer player)
        {
            bool playerEndendTheirTurn = false;
            int alreadyPointedDice = 0;
            int playerScore = 0;
            int moveNumber = 0;
            player.MoveNumber++;

            while (!playerEndendTheirTurn)
            {
                moveNumber++;
                if (alreadyPointedDice >= 6)
                {
                    alreadyPointedDice = 0;
                }
                var playerThrow = _random.Throw(6 - alreadyPointedDice);
                var diceToPoint = PointingSystem.FindDiceToPoint(playerThrow);
                Console.WriteLine($"Player: {player.Name}, Phase: {player.CurrentGamePhase}, {moveNumber} throw: {string.Join(", ", playerThrow)}");
                Console.WriteLine("------------------------");
                if (!diceToPoint.Any())
                {
                    if (alreadyPointedDice != 0)
                    {
                        Console.WriteLine("No dice to point was thrown.\n");
                        return 0;
                    }
                    if (player.CurrentGamePhase != GamePhase.NotEntered)
                    {
                        Console.WriteLine("No dice to point was thrown by player. -50 points.");
                        return -50;
                    }
                    Console.WriteLine("No dice to point was thrown by player.");
                    return 0;
                }

                ConsoleManagement.DisplayTheDiceThrown(diceToPoint);
                var diceCount = diceToPoint.Sum(die => die.DiceCount);
                if (diceCount == 6 || diceCount + alreadyPointedDice == 6)
                {
                    playerScore += PointingSystem.CalculatePointsFromDice(diceToPoint);
                    Console.WriteLine("All dice were pointable. Current score = {0}", playerScore);
                    playerEndendTheirTurn = player.EndTurn(playerScore, GameHistory, alreadyPointedDice);
                    alreadyPointedDice = 6;
                }
                else
                {
                    var diceChose = player.ChooseDice(diceToPoint, GameHistory, alreadyPointedDice);
                    alreadyPointedDice += diceChose.Sum(x => x.DiceCount);
                    var tempscore = PointingSystem.CalculatePointsFromDice(diceChose);
                    playerScore += tempscore;
                    if ((playerScore >= 100 && player.CurrentGamePhase == GamePhase.Finishing) || playerScore + player.Score >= 1000)
                    {
                        return playerScore;
                    }
                    if (CheckIfPlayerCanFinishTheTurn(player.CurrentGamePhase, playerScore))
                    {
                        playerEndendTheirTurn = player.EndTurn(playerScore, GameHistory, alreadyPointedDice);
                    }
                    else
                    {
                        Console.WriteLine($"{player.Name}'s score is {playerScore}");
                    }
                }
            }
            return playerScore;
        }

        private static bool CheckIfPlayerCanFinishTheTurn(GamePhase gamePhase, int score)
        {
            return gamePhase switch
            {
                GamePhase.NotEntered or GamePhase.Finishing => score >= 100,
                GamePhase.Entered => score >= 30,
                _ => throw new ArgumentException("Player with wrong gamephase was present in the game"),
            };
        }
    }
}
