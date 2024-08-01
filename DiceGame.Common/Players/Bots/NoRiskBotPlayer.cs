using DiceGame.Common.Enums;
using DiceGame.Common.GameLogic;

namespace DiceGame.Common.Players.Bots
{
    public class NoRiskBotPlayer : IPlayer
    {
        public string? Name { get; init; }
        public int Score { get; set; }
        public GamePhase CurrentGamePhase { get; set; }
        public int MoveNumber { get; set; }

        public NoRiskBotPlayer(string name)
        {
            Name = name;
        }

        public IEnumerable<PointableDice> ChooseDice(IEnumerable<PointableDice> diceToPoint, int alreadyPointedDice)
        {
            return diceToPoint;
        }

        public bool EndTurn(int roundScore, GameHistory gameHistory, int alreadyPointedDice)
        {
            return true;
        }
    }
}
