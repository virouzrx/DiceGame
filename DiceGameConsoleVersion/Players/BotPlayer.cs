using DiceGameConsoleVersion.GameLogic;
using DiceGameConsoleVersion.Models;

namespace DiceGameConsoleVersion.Logic
{
    internal class BotPlayer : IPlayer
    {
        public string Name { get; init; }

        public int Score { get; set; }
        public GamePhase CurrentGamePhase { get; set; }
        public PlayerType PlayerType { get; init; }
        public int MoveNumber { get; set; }
        public

        public int ChooseDice(List<PointableDice> diceToPoint, ref int tempscore, ref int alreadyPointedDice)
        {

        }

        public bool EndTurn(int roundScore)
        {
            throw new NotImplementedException();
        }
    }
}
