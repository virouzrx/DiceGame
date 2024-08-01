using DiceGame.Common.Enums;
using DiceGame.Common.GameLogic;
using DiceGame.Common.GameLogic.ProbabilityHelpers;

namespace DiceGame.Common.Players.Bots
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

                if (die.DiceCount == 1 || die.DiceCount >= 3 || alreadyPointedDice >= 3)
                {
                    return diceToPoint;
                }

                //(1,2), (5,2)
                return new[] { new PointableDice(die.Score, 1) };
            }

            //rare situation where there are 3 different dice and 
            if (diceToPoint.Count() == 3)
            {
                if (CheckProbabilityOfThrowingHigherScore(diceToPoint, 6 - alreadyPointedDice, 0.4))
                {
                    return Get1Or5(diceToPoint);
                }
            }

            if (diceToPoint.Any(x => x.Score == 2))
            {
                if (PointingSystem.CalculatePointsFromDice(diceToPoint) > 40)
                {
                    return diceToPoint;
                }

                if (ShouldPlayerTakeTripletOfTwos(alreadyPointedDice, diceToPoint))
                {
                    return diceToPoint;
                }

                return Get1Or5(diceToPoint);
            }

            if (PointingSystem.CalculatePointsFromDice(diceToPoint) > 40 || alreadyPointedDice >= 3)
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
                if (alreadyPointedDice == 6 || alreadyPointedDice == 0)
                {
                    return true;
                }
                
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
