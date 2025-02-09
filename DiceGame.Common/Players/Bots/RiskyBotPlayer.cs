namespace DiceGame.Common.Players.Bots
{
    public class RiskyBotPlayer : IPlayer
    {
        public RiskyBotPlayer(string name, ProbabilityHelper probabilityHelper)
        {
            Name = name;
            _probabilityHelper = probabilityHelper;
        }
        public RiskyBotPlayer(ProbabilityHelper probabilityHelper)
        {
            Name = "Risky";
            _probabilityHelper = probabilityHelper;
        }
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; init; }
        public bool IsBot { get; init; } = true;
        public BotType BotType { get; init; } = BotType.Risky;

        public readonly ProbabilityHelper _probabilityHelper;

        public IEnumerable<PointableDice> ChooseDice(IEnumerable<PointableDice> diceToPoint, int alreadyPointedDice)
        {
            if (diceToPoint.Sum(x => x.DiceCount) + alreadyPointedDice == 6)
            {
                return diceToPoint;
            }

            if (diceToPoint.Count() == 1)
            {
                var die = diceToPoint.First();

                if (die.DiceCount == 1 || die.DiceCount >= 3 || alreadyPointedDice >= 3)
                {
                    return diceToPoint;
                }

                return [new PointableDice(die.Score, 1)];
            }

            if (diceToPoint.Count() == 3)
            {
                if (CheckProbabilityOfThrowingHigherScore(diceToPoint, 6 - alreadyPointedDice, 0.35))
                {
                    return Get1Or5(diceToPoint);
                }
            }

            if (diceToPoint.Any(x => x.Score == 2))
            {
                if (PointingSystem.CalculatePointsFromDice(diceToPoint) > 45)
                {
                    return diceToPoint;
                }

                if (ShouldPlayerTakeTripletOfTwos(alreadyPointedDice, diceToPoint))
                {
                    return diceToPoint;
                }

                return Get1Or5(diceToPoint);
            }

            if (PointingSystem.CalculatePointsFromDice(diceToPoint) > 50 || alreadyPointedDice >= 3)
            {
                return diceToPoint;
            }

            return Get1Or5(diceToPoint);
        }

        public bool EndTurn(int roundScore, GameStateOverview gameStateOverview, int alreadyPointedDice)
        {
            var currentLeaderboard = gameStateOverview.Leaderboard.OrderByDescending(x => x.Score).ToList();
            if (gameStateOverview.PlayerGamePhase == GamePhase.NotEntered)
            {
                if (!currentLeaderboard.Any(x => x.CurrentGamePhase != GamePhase.NotEntered))
                {
                    var probability = ProbabilityHelper.CalculateProbabilityOfThrowingSomethingPointable(6 - alreadyPointedDice);
                    return roundScore > 120 || probability < 0.7;
                }

                if (currentLeaderboard
                    .Where(x => x.Name != Name)
                    .All(x => x.CurrentGamePhase != GamePhase.NotEntered))
                {
                    return true;
                }

                return !CanPlayerBeOvertakenOnEnteringTheGame(currentLeaderboard, roundScore, alreadyPointedDice);
            }
            else if (gameStateOverview.PlayerGamePhase == GamePhase.Entered)
            {
                if (alreadyPointedDice == 6 || alreadyPointedDice == 0)
                {
                    return true;
                }

                if (gameStateOverview.PlayersScore > 800 && gameStateOverview.PlayersScore < 900)
                {
                    return gameStateOverview.PlayersScore + roundScore <= 895 || gameStateOverview.PlayersScore + roundScore > 940;
                }

                if (gameStateOverview.LastPlayersMoves.Count >= 3 && !gameStateOverview.PlayerScoredInLastRounds(3))
                {
                    return true;
                }

                return !ShouldRisk(currentLeaderboard, roundScore, alreadyPointedDice) && alreadyPointedDice <= 3;
            }
            return true;
        }

        private static bool ShouldPlayerTakeTripletOfTwos(int alreadyPointedDice, IEnumerable<PointableDice> diceToPoint)
        {
            return alreadyPointedDice == 2 || diceToPoint
                .Where(x => x.Score != 2)
                .Sum(x => x.DiceCount) >= 2;
        }

        private bool CheckProbabilityOfThrowingHigherScore(IEnumerable<PointableDice> diceToPoint, int alreadyPointedDice, double desiredProbability)
        {
            return _probabilityHelper.CalculateProbabilityOfThrowingParticularScoreOrHigher(
                    PointingSystem.CalculatePointsFromDice(diceToPoint), alreadyPointedDice) >= desiredProbability;
        }

        private static IEnumerable<PointableDice> Get1Or5(IEnumerable<PointableDice> diceToPoint)
        {
            return diceToPoint.Any(x => x.Score == 1)
                        ? [new PointableDice(1, 1)]
                        : new[] { new PointableDice(5, 1) };
        }

        private static bool CanPlayerBeOvertakenOnEnteringTheGame(List<PlayerInfo> players, int score, int alreadyPointedDice)
        {
            var playersThatEnteredTheGame = players
                .Where(x => x.CurrentGamePhase != GamePhase.NotEntered)
                .OrderBy(x => x.Score);

            if (playersThatEnteredTheGame.First().Score > score)
            {
                return playersThatEnteredTheGame.First().Score - score < 30
                    && ProbabilityHelper.CalculateProbabilityOfThrowingSomethingPointable(6 - alreadyPointedDice) > 0.7;
            }
            return false;
        }

        private bool ShouldRisk(List<PlayerInfo> players, int roundScore, int alreadyPointedDice)
        {
            var player = players.First(x => x.Id == Id);
            var index = players.IndexOf(player);
            var playerRoundscore = player.Score + roundScore;
            if (index == 0)
            {
                return playerRoundscore - players[1].Score > 100;
            }

            if (index != players.Count - 1 &&
                players[index - 1].Score - playerRoundscore <= 75 &&
                playerRoundscore - players[index + 1].Score >= 100)
            {
                return _probabilityHelper.CalculateProbabilityOfThrowingParticularScoreOrHigher(
                    players[index - 1].Score - playerRoundscore, 6 - alreadyPointedDice) >= 0.4;
            }

            return false;
        }
    }
}
