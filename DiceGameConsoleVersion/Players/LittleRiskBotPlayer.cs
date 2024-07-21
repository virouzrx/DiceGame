using DiceGameConsoleVersion.GameLogic;
using DiceGameConsoleVersion.Logic;
using DiceGameConsoleVersion.Models;
using System;

namespace DiceGameConsoleVersion.Players
{
    public class LittleRiskBotPlayer : IPlayer
    {
        public string? Name { get; init; }
        public int Score { get; set; }
        public GamePhase CurrentGamePhase { get; set; }
        public int MoveNumber { get; set; }

        public LittleRiskBotPlayer(string name)
        {
            Name = name;
        }

        public IEnumerable<PointableDice> ChooseDice(IEnumerable<PointableDice> diceToPoint, GameHistory gameHistory, int alreadyPointedDice)
        {
            if (diceToPoint.Count() == 1)
            {
                var die = diceToPoint.First();

                if (die.DiceCount == 1) //(1,5)
                {
                    return diceToPoint;
                }

                if (die.DiceCount > 2 || (diceToPoint.Count() == 2 && alreadyPointedDice >= 3))
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

        public bool EndTurn(int roundScore, GameHistory gameHistory, int alreadyPointedDice)
        {
            if (CurrentGamePhase == GamePhase.Entered)
            {
                if (roundScore < 30 || alreadyPointedDice == 0 || alreadyPointedDice == 6)
                {
                    Console.WriteLine($"{Name} score is {roundScore}");
                    return false;
                }

                if (!gameHistory.PlayerScoredInLastRounds(Name!, 3)) 
                {
                    return true;
                }

                var scoreBoard = gameHistory.GetLastHistoryItem();
                var playerIndex = scoreBoard.IndexOf(scoreBoard.First(x => x.Name == Name));
                if (playerIndex == scoreBoard.Count - 1)
                {
                    return true;
                }

                return ShouldRisk(gameHistory.GetLastHistoryItem());
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
            if (index != players.Count - 1 && players[index - 1].Score - Score <= 45 && Score - players[index + 1].Score > 75) 
            {
                return true;
            }

            return false;
        }
    }
}
