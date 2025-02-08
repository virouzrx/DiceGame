namespace DiceGame.Common.Players.Bots
{
    public class NoRiskBotPlayer : IPlayer
    {
        public NoRiskBotPlayer()
        {
            Name = "NoRisk";
        }

        public NoRiskBotPlayer(string name)
        {
            Name = name;
        }
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; init; }
        public bool IsBot { get; init; } = true;
        public BotType BotType { get; init; } = BotType.NoRisk;

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
