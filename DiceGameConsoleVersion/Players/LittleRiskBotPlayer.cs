using DiceGameConsoleVersion.GameLogic;
using DiceGameConsoleVersion.Logic;
using DiceGameConsoleVersion.Models;
using System;

namespace DiceGameConsoleVersion.Players
{
    internal class LittleRiskBotPlayer : IPlayer
    {
        public string Name { get; init; }
        public int Score { get; set; }
        public GamePhase CurrentGamePhase { get; set; }
        public PlayerType PlayerType { get; init; }
        public int MoveNumber { get; set; }

        public IEnumerable<PointableDice> ChooseDice(List<PointableDice> diceToPoint, int alreadyPointedDice)
        {
            if (diceToPoint.Count == 1)
            {
                var die = diceToPoint.First();

                if (die.DiceCount == 1) //(1,5)
                {
                    return diceToPoint;
                }

                if (die.DiceCount > 2 || (diceToPoint.Count == 2 && alreadyPointedDice >= 3))
                {
                    return diceToPoint;
                }
                return new[] { new PointableDice(die.Score, 1) };
            }

            if (alreadyPointedDice < 2)
            {
                if (diceToPoint.Any(x => x.DiceCount > 2))
                {
                    if (diceToPoint.First(x => x.DiceCount > 2).Score == 2)
                    {
                        if (diceToPoint.Any(x => x.Score == 1))
                        {
                            return new[] { new PointableDice(1, 1) };
                        }
                        return new[] { new PointableDice(5, 1) };
                    }
                    return diceToPoint;
                }

                if (!diceToPoint.Any(x => x.Score == 1))
                {
                    return new[] { new PointableDice(5, 1) };
                }
                return new[] { new PointableDice(1, 1) };
            }
            return diceToPoint;
        }

        public bool EndTurn(int roundScore, List<List<IPlayer>> history, int alreadyPointedDice)
        {
            if (CurrentGamePhase == GamePhase.Entered)
            {
                if (roundScore < 30 || alreadyPointedDice == 0 || alreadyPointedDice == 6)
                {
                    Console.WriteLine($"{Name} score is {roundScore}");
                    return false;
                }

                if (!PlayerScoredInLastTwoRounds(history, 3, Name)) 
                {
                    return true;
                }

                var scoreBoard = history.Last();
                var playerIndex = scoreBoard.IndexOf(scoreBoard.First(x => x.Name == Name));
                if (playerIndex == scoreBoard.Count - 1)
                {
                    return true;
                }

                return ShouldRisk(history.Last());
            }
            else
            {
                if (roundScore < 100)
                {
                    Console.WriteLine($"{Name} score is {roundScore}");
                    return false;
                }
                else
                {
                    if (CurrentGamePhase == GamePhase.Finishing)
                    {
                        CurrentGamePhase = GamePhase.Finished;
                        return true;
                    }
                    return true;
                }
            }
        }

        private bool ShouldRisk(List<IPlayer> players)
        {
            var index = players.IndexOf(this);
            if (index == 0)
            {
                if (Score - players[1].Score > 200)
                {
                    return true;
                }
            }
            if (players[index - 1].Score - Score <= 45 && Score - players[index + 1].Score > 75) 
            {
                return true;
            }

            return false;
        }

        private static bool PlayerScoredInLastTwoRounds(List<List<IPlayer>> history, int numberOfRoundsToCheck, string playerName)
        {
           return history.TakeLast(numberOfRoundsToCheck)
            .Select(x => x
                .Where(y => y.Name == playerName)
                .Select(y => y.Score))
            .Distinct()
            .Count() > 1;
        }
    }
}
