using DiceGame.Common.Extensions;

namespace DiceGame.Common.Players.Bots
{
    public class LittleRiskBotPlayer(string name) : IPlayer
    {
        public string Name { get; init; } = name;
        public Guid Id { get; init; } = Guid.NewGuid();

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
                return [new PointableDice(die.Score, 1)];
            }

            if (alreadyPointedDice < 2)
            {
                if (diceToPoint.Any(x => x.DiceCount > 2))
                {
                    if (diceToPoint.First(x => x.DiceCount > 2).Score == 2)
                    {
                        if (diceToPoint.Any(x => x.Score == 1))
                        {
                            return [new PointableDice(1, 1)];
                        }
                        return [new PointableDice(5, 1)];
                    }
                    return diceToPoint;
                }

                if (!diceToPoint.Any(x => x.Score == 1))
                {
                    return [new PointableDice(5, 1)];
                }
                return [new PointableDice(1, 1)];
            }
            return diceToPoint;
        }

        public bool EndTurn(int roundScore, GameStateOverview gameStateOverview, int alreadyPointedDice)
        {
            var playerInfo = gameStateOverview.Leaderboard!.First(x => x.Id == Id);
            if (playerInfo.CurrentGamePhase == GamePhase.Entered)
            {
                if (!gameStateOverview.PlayerScoredInLastRounds(2))
                {
                    return true;
                }

                if (gameStateOverview.IsPlayerLast())
                {
                    return true;
                }
            }
            return !ShouldRisk(gameStateOverview.Leaderboard, playerInfo);
        }

        private static bool ShouldRisk(IReadOnlyList<PlayerInfo> players, PlayerInfo playerInfo)
        {
            var index = players.IndexOf(playerInfo);
            if (index == 0)
            {
                return playerInfo.Score - players[1].Score > 200;
            }

            return ArePlayersWithinScoreRange(players, playerInfo, index);
        }

        private static bool ArePlayersWithinScoreRange(IReadOnlyList<PlayerInfo> players, PlayerInfo playerInfo, int index)
        {
            return index != players.Count - 1
                && players[index - 1].Score - playerInfo.Score <= 45
                && playerInfo.Score - players[index + 1].Score > 75;
        }
    }
}
