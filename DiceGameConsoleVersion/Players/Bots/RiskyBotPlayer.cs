namespace DiceGameConsoleVersion.Players.Bots
{
    internal class RiskyBotPlayer : IPlayer
    {
        public string? Name { get; init; }
        public int Score { get; set; }
        public GamePhase CurrentGamePhase { get; set; }
        public int MoveNumber { get; set; }

        public RiskyBotPlayer(string name)
        {
            Name = name;
        }

        public IEnumerable<PointableDice> ChooseDice(IEnumerable<PointableDice> diceToPoint, GameHistory history, int alreadyPointedDice)
        {
            throw new NotImplementedException();
        }

        public bool EndTurn(int roundScore, GameHistory history, int alreadyPointedDice)
        {
            throw new NotImplementedException();
        }
    }
}
