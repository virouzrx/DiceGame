using DiceGame.Common.Enums;
using DiceGame.Common.GameLogic;

namespace DiceGame.Common.Players.Bots
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

        public IEnumerable<PointableDice> ChooseDice(IEnumerable<PointableDice> diceToPoint, int alreadyPointedDice)
        {
            if (diceToPoint.Sum(x => x.DiceCount) + alreadyPointedDice == 6)
            {
                return diceToPoint;
            }

            if (diceToPoint.Count() == 1)
            {
                var die = diceToPoint.First();

                if (die.DiceCount == 1) //(1,5)
                {
                    return diceToPoint;
                }

                if (die.DiceCount > 2 || diceToPoint.Count() == 2 && alreadyPointedDice >= 3)
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
                if (!gameHistory.PlayerScoredInLastRounds(Name!, 2))
                {
                    return true;
                }

                if (gameHistory.IsPlayerLast(Name!))
                {
                    return true;
                }
            }
            return !ShouldRisk(gameHistory.GetLastHistoryItem());
        }

        private bool ShouldRisk(List<IPlayer> players)
        {
            var index = players.FindIndex(x => x.Name == Name);
            if (index == 0)
            {
                return Score - players[1].Score > 200;
            }

            return ArePlayersWithinScoreRange(players, index);
        }

        private bool ArePlayersWithinScoreRange(List<IPlayer> players, int index)
        {
            return index != players.Count - 1
                && players[index - 1].Score - Score <= 45
                && Score - players[index + 1].Score > 75;
        }
    }
}
