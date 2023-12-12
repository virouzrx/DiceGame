using DiceGameConsoleVersion.Models;
using static DiceGameConsoleVersion.Extensions;

namespace DiceGameConsoleVersion
{
    public class PointingSystem
    {
        private Dictionary<int, int> SingleDicePoints { get; set; }
        public PointingSystem(Dictionary<int, int> singleDicePoints)
        {
            SingleDicePoints = singleDicePoints;
        }
        public int CalculatePointsFromDice(int dieScore, int count)
        {
            var diePointValue = dieScore == 1 ? 10 : dieScore;
            return (int)(count < 3
                ? SingleDicePoints[dieScore] * count
                : diePointValue * 10 * Math.Pow(2, Math.Abs(3 - count)));
        }

        public int CalculatePointsFromDice(IEnumerable<PointableDice> dice)
        {
            return dice.Sum(die => CalculatePointsFromDice(die.Score, die.Count));
        }

        public List<PointableDice> FindDiceToPoint(IEnumerable<int> hand)
        {
            var pointableDice = hand
                .GroupBy(x => x)
                .Select(g => new PointableDice(g.Key, g.Count()))
                .Where(p => p.Count > 2 || p.Score is 1 or 5)
                .ToList();
            return pointableDice.Count > 0 ? pointableDice : new List<PointableDice>();
        }

        public List<Player> UpdateScoreboard(List<Player> playerList, string name, int score)
        {
            var playerListOld = Clone(playerList).OrderByScore();               
                
            playerList.GetPlayerByName(name).Score += score;                    
            playerList = playerList.OrderByScore();                             
            
            var playerIndex = GetPlayerIndex(playerList, name);         
            var playerScoreDecreased = true;
            while (playerScoreDecreased)
            {
                playerList = playerList.OrderByScore();
                
                var differences = playerListOld
                    .Where(player => playerListOld.IndexOf(player) < playerList.IndexOf(player))
                    .OrderByDescending(player => player.Score)
                    .ToList();

                if (differences.Any())
                {
                    playerListOld = Clone(playerList).OrderByScore();
                    foreach (var diff in differences)
                    {
                        var playerMarkedForPointDecrease = playerList.First(x => x.Name == diff.Name);
                        if (playerMarkedForPointDecrease.CurrentPlayerPhase == GamePhase.NotEntered)
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

                        if (playerWithHigherIndex.CurrentPlayerPhase == GamePhase.NotEntered)
                        {
                            continue;
                        }

                        playerMarkedForPointDecrease.Score -= 100;
                    }
                }
                else
                {
                    playerScoreDecreased = false;
                }
            }
            return playerList;
        }

        private static int GetPlayerIndex(List<Player> players, string playerName)
        {
            return players.IndexOf(players.GetPlayerByName(playerName));
        }
    }
}
