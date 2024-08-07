namespace DiceGame.Common.Players.Bots
{
    public class NoRiskBotPlayer(string name) : IPlayer
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; init; } = name;

        public IEnumerable<PointableDice> ChooseDice(IEnumerable<PointableDice> diceToPoint, int alreadyPointedDice)
        {
            return diceToPoint;
        }

        public bool EndTurn(int roundScore, GameStateOverview gameStateOverview, int alreadyPointedDice)
        {
            return true;
        }
    }
}
