using DiceGameConsoleVersion.GameLogic;
using DiceGameConsoleVersion.Logic;
using DiceGameConsoleVersion.Models;

namespace DiceGameConsoleVersion.Players
{
    internal class ModerateRiskBotPlayer : IPlayer
    {
        public string? Name { get; init; }
        public int Score { get; set; }
        public GamePhase CurrentGamePhase { get; set; }
        public int MoveNumber { get; set; }

        public ModerateRiskBotPlayer(string name)
        {
            Name = name;
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
                if (!currentLeaderboard.Any(x => x.CurrentGamePhase != GamePhase.NotEntered))
                {
                    return roundScore > 120 || ProbabilityHelper.CalculateProbabilityOfThrowingSomethingPointable(alreadyPointedDice) < 70;
                }

                if (!currentLeaderboard
                    .Where(x => x.Name != Name)
                    .All(x => x.CurrentGamePhase != GamePhase.NotEntered))
                {
                    return true;
                }

                if (roundScore < FindClosestScoreToPlayersScore(currentLeaderboard, roundScore))
                {
                    return ProbabilityHelper.CalculateProbabilityOfThrowingSomethingPointable(alreadyPointedDice) < 70;
                }
                return true;

            }
            else if (CurrentGamePhase == GamePhase.Entered)
            {
                if (Score > 800 && Score < 900)
                {
                    return Score + roundScore < 870 || Score + roundScore > 940;
                }

                if (!gameHistory.PlayerScoredInLastRounds(Name!, 2))
                {
                    return true;
                }

                return !ShouldRisk(currentLeaderboard);
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

        private static bool CheckProbabilityOfThrowingHigherScore(IEnumerable<PointableDice> diceToPoint, int alreadyPointedDice, int desiredProbability)
        {
            return ProbabilityHelper.CalculateProbabilityOfThrowingHigherScore(
                    PointingSystem.CalculatePointsFromDice(diceToPoint), alreadyPointedDice) >= desiredProbability;
        }

        private static IEnumerable<PointableDice> Get1Or5(IEnumerable<PointableDice> diceToPoint)
        {
            return diceToPoint.Any(x => x.Score == 1)
                        ? new[] { new PointableDice(1, 1) }
                        : new[] { new PointableDice(5, 1) };
        }

        private static int FindClosestScoreToPlayersScore(List<IPlayer> players, int score)
        {
            return players
                .OrderBy(n => Math.Abs(n.Score - score))
                .First().Score;
        }

        private bool ShouldRisk(List<IPlayer> players)
        {
            var index = players.IndexOf(this);
            if (index == 0)
            {
                if (Score - players[1].Score > 70)
                {
                    return true;
                }
            }

            if (index != players.Count - 1 && 
                players[index - 1].Score - Score <= 45 && 
                Score - players[index + 1].Score > 75)
            {
                return true;
            }

            return false;
        }
    }
}
