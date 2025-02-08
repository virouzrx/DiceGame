namespace DiceGame.Common.Players
{
    public interface IPlayer
    {
        Guid Id { get; init; }
        string Name { get; init; }
        public bool IsBot { get; init; }
        public BotType BotType { get; init; }
        public IEnumerable<PointableDice> ChooseDice(IEnumerable<PointableDice> diceToPoint, int alreadyPointedDice);
        public bool EndTurn(int roundScore, GameStateOverview gameStateOverview, int alreadyPointedDice);

        public int Test()
        {
            return 0; 
        }
    }
}
