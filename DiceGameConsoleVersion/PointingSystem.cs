using DiceGameConsoleVersion.Models;
using System.Reflection;
using System.Runtime.CompilerServices;
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

        public List<Player> UpdateScoreboard(List<Player> playerList, Player player, int score)
        {
            var noChangeHasBeenDone = false;
            var listOfPlayersLastUpdate = Clone(playerList).OrderBy(x => x.Score).ToList();
            playerList.First(x => x.Name == player.Name).Score += score;
            while (!noChangeHasBeenDone)
            {
                noChangeHasBeenDone = true;
                playerList = playerList.OrderBy(x => x.Score).ToList();
                if (playerList.SequenceEqual(listOfPlayersLastUpdate))
                {
                    return playerList;
                }

                for(int i = 0; i < playerList.Count; i++)
                {
                    if (playerList[i].Name != listOfPlayersLastUpdate[i].Name)
                    {
                        var playerMarkedForScoreDecrease = playerList.First(x => x.Name == listOfPlayersLastUpdate[i].Name);
                        if (playerMarkedForScoreDecrease.CurrentPlayerPhase == GamePhase.NotEntered)
                        {
                            continue;
                        }
                        playerMarkedForScoreDecrease.Score -= 100;
                        listOfPlayersLastUpdate = listOfPlayersLastUpdate.OrderBy(x => x.Score).ToList();
                        noChangeHasBeenDone = false;
                    }
                }
            }
            return playerList;
        }
    }
}
