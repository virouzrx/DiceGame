﻿using DiceGameConsoleVersion.Models;
using static DiceGameConsoleVersion.Extensions;

namespace DiceGameConsoleVersion
{
    public class PointingSystem
    {
        public static readonly Dictionary<int, int> SingleDicePoints = new()
        {
            { 1, 10 },
            { 5, 5 }
        };

        public static int CalculatePointsFromDice(int dieScore, int count)
        {
            var diePointValue = dieScore == 1 ? 10 : dieScore;
            return (int)(count < 3
                ? SingleDicePoints[dieScore] * count
                : diePointValue * 10 * Math.Pow(2, Math.Abs(3 - count)));
        }

        public static int CalculatePointsFromDice(IEnumerable<PointableDice> dice)
        {
            return dice.Sum(die => CalculatePointsFromDice(die.Score, die.Count));
        }

        public static List<PointableDice> FindDiceToPoint(IEnumerable<int> hand)
        {
            var pointableDice = hand
                .GroupBy(x => x)
                .Select(g => new PointableDice(g.Key, g.Count()))
                .Where(p => p.Count > 2 || p.Score is 1 or 5)
                .ToList();
            return pointableDice.Count > 0 ? pointableDice : new List<PointableDice>();
        }

        public static List<Player> UpdateScoreboard(List<Player> playerList, Player player, int score)
        {
            var initialList = Clone(playerList).OrderByScore();
            var playerListOld = Clone(playerList).OrderByScore();

            player.Score += score;
            playerList = playerList.OrderByScore();
            
            if (player.Score >= 1000)
            {
                player.CurrentGamePhase = GamePhase.Finished;
            }

            if (player.CurrentGamePhase == GamePhase.NotEntered)
            {
                player.CurrentGamePhase = GamePhase.Entered;
            }

            if (player.CurrentGamePhase == GamePhase.Entered && player.Score > 900)
            {
                player.CurrentGamePhase = GamePhase.Finishing;
            }

            var playerIndex = playerList.IndexOf(player);       
            var playerScoreDecreased = true;
            while (playerScoreDecreased)
            {
                playerList = playerList.OrderByScore();
                
                var differences = playerListOld
                    .Where(p => playerListOld.IndexOf(p) < playerList.IndexOf(p))
                    .OrderByDescending(p => p.Score)
                    .ToList();

                if (differences.Any())
                {
                    playerListOld = Clone(playerList).OrderByScore();
                    foreach (var diff in differences)
                    {
                        var playerMarkedForPointDecrease = playerList.First(x => x.Name == diff.Name);
                        if (playerMarkedForPointDecrease.CurrentGamePhase == GamePhase.NotEntered)
                        {
                            continue;
                        }

                        var playerWithHigherIndex = playerList.GetPlayerWithHigherIndex(playerMarkedForPointDecrease);
                        if (playerWithHigherIndex == null)
                        {
                            continue;
                        }

                        if (playerMarkedForPointDecrease.Score == playerWithHigherIndex.Score)
                        {
                            continue;
                        }

                        if (playerWithHigherIndex.CurrentGamePhase == GamePhase.NotEntered)
                        {
                            continue;
                        }

                        playerMarkedForPointDecrease.Score -= (playerListOld.IndexOf(playerMarkedForPointDecrease) - initialList.IndexOf(playerMarkedForPointDecrease)) * 100;
                    }
                }
                else
                {
                    playerScoreDecreased = false;
                }
            }

            var playersWithTheSameScore = playerList.Where(p => p.Score == player.Score);
            if (playersWithTheSameScore.Count() > 1)
            {
                var playerThatShouldBeHigher = playersWithTheSameScore.First(p => p.Name != player.Name);
                var playerThatShouldBeHigherIndex = playerList.IndexOf(playerThatShouldBeHigher);
                var playerIndexAfterUpdate = playerList.IndexOf(player);
                
                if (playerIndexAfterUpdate < playerThatShouldBeHigherIndex)
                {
                    (playerList[playerIndexAfterUpdate], playerList[playerThatShouldBeHigherIndex]) =
                        (playerList[playerThatShouldBeHigherIndex], playerList[playerIndexAfterUpdate]);
                }
            }
            return playerList.OrderByScore();
        }
    }
}
