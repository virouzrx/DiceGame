using DiceGameConsoleVersion.GameLogic;
using DiceGameConsoleVersion.Models;

namespace DiceGameConsoleVersion.Logic
{
    public interface IPlayer
    {
        string Name { get; init; }
        public int Score { get; set; }
        public GamePhase CurrentGamePhase { get; set; }
        public PlayerType PlayerType { get; init; }
        public int MoveNumber { get; set; }
        public IEnumerable<PointableDice> ChooseDice(List<PointableDice> diceToPoint, int alreadyPointedDice);
        public bool EndTurn(int roundScore, List<List<IPlayer>> history, int alreadyPointedDice);
    }
}
