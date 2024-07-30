using DiceGame.Common.Enums;
using DiceGame.Common.GameLogic;

namespace DiceGame.Common.Players
{
    public interface IPlayer
    {
        string? Name { get; init; }
        public int Score { get; set; }
        public GamePhase CurrentGamePhase { get; set; }
        public int MoveNumber { get; set; }
        public IEnumerable<PointableDice> ChooseDice(IEnumerable<PointableDice> diceToPoint, GameHistory history, int alreadyPointedDice);
        public bool EndTurn(int roundScore, GameHistory history, int alreadyPointedDice);
    }
}
