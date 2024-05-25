using DiceGameConsoleVersion.Logic;
using DiceGameConsoleVersion.Models;
using DiceGameConsoleVersion.Utilities;
using System;

namespace DiceGameConsoleVersion.GameLogic
{
    public class Game
    {
        public List<List<IPlayer>> History { get; set; }
        public List<IPlayer> Players { get; set; }
        private readonly Random _random;
        public Game(List<IPlayer> players)
        {
            Players = players;
            History = new List<List<IPlayer>>();
            _random = new Random();
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

                    if (Players.Select(x => x.MoveNumber).Distinct().Count() == 1)
                    {
                        History.Add(Players);
                    }
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
                    playerEndendTheirTurn = player.EndTurn(playerScore, History, alreadyPointedDice);
                    alreadyPointedDice = 6;
                }
                else
                {
                    var diceChose = player.ChooseDice(diceToPoint, alreadyPointedDice);
                    alreadyPointedDice += diceChose.Sum(x => x.DiceCount);
                    var tempscore = PointingSystem.CalculatePointsFromDice(diceChose);
                    playerScore += tempscore;
                    playerEndendTheirTurn = player.EndTurn(playerScore, History, alreadyPointedDice);
                }
            }
            return playerScore;
        }
    }
}
