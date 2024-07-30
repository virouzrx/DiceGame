namespace DiceGameConsoleVersion.Players.Bots
{
    internal class NoRiskBotPlayer : IPlayer
    {
        public string? Name { get; init; }
        public int Score { get; set; }
        public GamePhase CurrentGamePhase { get; set; }
        public int MoveNumber { get; set; }

        public NoRiskBotPlayer(string name)
        {
            Name = name;
        }

        public IEnumerable<PointableDice> ChooseDice(IEnumerable<PointableDice> diceToPoint, GameHistory gameHistory, int alreadyPointedDice)
        {
            return diceToPoint;
        }

        public bool EndTurn(int roundScore, GameHistory gameHistory, int alreadyPointedDice)
        {
            return roundScore >= 30;
        }
    }
}
