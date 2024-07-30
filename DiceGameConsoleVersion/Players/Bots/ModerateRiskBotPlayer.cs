using DiceGameConsoleVersion.GameLogic.ProbabilityHelpers;

namespace DiceGameConsoleVersion.Players.Bots
{
    public class ModerateRiskBotPlayer : IPlayer
    {
        public string? Name { get; init; }
        public int Score { get; set; }
        public GamePhase CurrentGamePhase { get; set; }
        public int MoveNumber { get; set; }
        public readonly ProbabilityHelper _probabilityHelper;

        public ModerateRiskBotPlayer(string name, ProbabilityHelper helper)
        {
            Name = name;
            _probabilityHelper = helper;
        }

        public IEnumerable<PointableDice> ChooseDice(IEnumerable<PointableDice> diceToPoint, GameHistory history, int alreadyPointedDice)
        {
            if (diceToPoint.Count() == 1)
            {
                var die = diceToPoint.First();

                if (die.DiceCount == 1) //5 or 1
                {
                    return diceToPoint;
                }

                if (die.DiceCount > 3 || alreadyPointedDice < 3) //1,1 or 5,5
                {
                    return diceToPoint;
                }

                return new[] { new PointableDice(die.Score, 1) };
            }

            if (diceToPoint.Count() == 3)
            {
                if (CheckProbabilityOfThrowingHigherScore(diceToPoint, alreadyPointedDice, 70))
                {
                    return Get1Or5(diceToPoint);
                }
            }

            if (diceToPoint.Any(x => x.Score == 2))
            {
                if (ShouldPlayerTakeTripletOfTwos(alreadyPointedDice, diceToPoint))
                {
                    return diceToPoint;
                }

                return Get1Or5(diceToPoint);
            }

            if (PointingSystem.CalculatePointsFromDice(diceToPoint) > 45)
            {
                return diceToPoint;
            }

            return Get1Or5(diceToPoint);
        }

        public bool EndTurn(int roundScore, GameHistory gameHistory, int alreadyPointedDice)
        {
            var currentLeaderboard = gameHistory.GetLastHistoryItem();
            if (CurrentGamePhase == GamePhase.NotEntered)
            {
                if (roundScore < 100)
                {
                    Console.WriteLine($"{Name} score is {roundScore}");
                    return false;
                }

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
            else if (CurrentGamePhase == GamePhase.Entered)
            {
                if (Score > 800 && Score < 900)
                {
                    return Score + roundScore <= 895 || Score + roundScore > 940;
                }

                if (gameHistory.History.Count >= 3 && !gameHistory.PlayerScoredInLastRounds(Name!, 3))
                {
                    return true;
                }


                return !ShouldRisk(currentLeaderboard, roundScore, alreadyPointedDice) && alreadyPointedDice <= 3;
            }
            else
            {
                if (roundScore < 100)
                {
                    Console.WriteLine($"{Name} score is {roundScore}");
                    return false;
                }
                return true;
            }
        }

        private static bool ShouldPlayerTakeTripletOfTwos(int alreadyPointedDice, IEnumerable<PointableDice> diceToPoint)
        {
            return alreadyPointedDice == 2 || diceToPoint.Where(x => x.Score != 2).Sum(x => x.DiceCount) >= 2;
        }

        private bool CheckProbabilityOfThrowingHigherScore(IEnumerable<PointableDice> diceToPoint, int alreadyPointedDice, int desiredProbability)
        {
            return _probabilityHelper.CalculateProbabilityOfThrowingParticularScoreOrHigher(
                    PointingSystem.CalculatePointsFromDice(diceToPoint), alreadyPointedDice) >= desiredProbability;
        }

        private static IEnumerable<PointableDice> Get1Or5(IEnumerable<PointableDice> diceToPoint)
        {
            return diceToPoint.Any(x => x.Score == 1)
                        ? new[] { new PointableDice(1, 1) }
                        : new[] { new PointableDice(5, 1) };
        }

        private static bool CanPlayerBeOvertakenOnEnteringTheGame(List<IPlayer> players, int score, int alreadyPointedDice)
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

        private bool ShouldRisk(List<IPlayer> players, int roundScore, int alreadyPointedDice)
        {
            var index = players.IndexOf(this);
            var playerRoundscore = Score + roundScore;
            if (index == 0)
            {
                if (playerRoundscore - players[1].Score > 100)
                {
                    return true;
                }
            }

            if (index != players.Count - 1 && //not last
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
