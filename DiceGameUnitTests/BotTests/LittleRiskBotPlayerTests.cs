using DiceGame.Common.Players.Bots;
using DiceGame.UnitTests.Helpers;

namespace DiceGame.UnitTests.BotTests
{
    public class LittleRiskBotPlayerTests
    {
        private LittleRiskBotPlayer botPlayer = new("HumanPlayer1");

        public static TheoryData<PointableDice[], PointableDice[], int> CalculatePointsFromDiceTestData =>
        new TheoryData<PointableDice[], PointableDice[], int>()
        {
                             { new PointableDice[] { new(2,3), new(1,1), new(5,1) }, new PointableDice[] {new(1,1) }, 0 },
                             { new PointableDice[] { new(1,2) }, new PointableDice[] {new(1,1) }, 0 },
                             { new PointableDice[] { new(3,3), new(5,2) }, new PointableDice[] {new(3,3), new(5,2) }, 0 },
                             { new PointableDice[] { new(5,1), new(1,3) }, new PointableDice[] {new(5,1), new(1,3) }, 0 },
                             { new PointableDice[] { new(5,1), new(1,1) }, new PointableDice[] {new(5,1), new(1,1) }, 3 },
                             { new PointableDice[] { new(2,3), new(1,1) }, new PointableDice[] {new(2,3), new(1,1) }, 3 },
        };

        [Theory]
        [MemberData(nameof(CalculatePointsFromDiceTestData))]
        public void LittleRiskBot_CheckIfBotChoosesCorrectDice(IEnumerable<PointableDice> diceThrown, IEnumerable<PointableDice> expectedChoose, int alreadyPointedDice)
        {
            var diceChosenByBot = botPlayer.ChooseDice(diceThrown.ToList(), alreadyPointedDice).ToList();
            Assert.Equal(expectedChoose, diceChosenByBot);
        }

        [Fact]
        public void LittleRiskBot_CheckIfBotEndsTurn_WhenBotHasntScoredInLast2Rounds()
        {
            var bot = new LittleRiskBotPlayer("Bot");
            var gameStateOverview = new GameStateOverviewBuilder()
                .WithPlayer("Player1", GamePhase.Entered, 180)
                .WithPlayer("Player2", GamePhase.Entered, 200)
                .WithPlayer("Player3", GamePhase.Entered, 240)
                .WithPlayerToTest(bot, GamePhase.Entered, 280)
                .WithPlayerHistory([100, 100, 100])
                .Build();

            var turnEnded = bot.EndTurn(30, gameStateOverview, 0);
            Assert.True(turnEnded);
        }

        [Fact]
        public void LittleRiskBot_CheckIfBotEndsTurn_WhenBotIsLast()
        {
            var bot = new LittleRiskBotPlayer("Bot");
            var gameStateOverview = new GameStateOverviewBuilder()
                .WithPlayer("Player1", GamePhase.Entered, 350)
                .WithPlayer("Player2", GamePhase.Entered, 400)
                .WithPlayer("Player3", GamePhase.Entered, 600)
                .WithPlayerToTest(bot, GamePhase.Entered, 280)
                .WithPlayerHistory([150, 230, 280])
                .Build();

            var turnEnded = bot.EndTurn(30, gameStateOverview, 0);
            Assert.True(turnEnded);
        }

        [Fact]
        public void LittleRiskBot_CheckIfBotEndsTurn_WhenBotIsFirstWithoutBigAdvantage()
        {
            var bot = new LittleRiskBotPlayer("Bot");
            var gameStateOverview = new GameStateOverviewBuilder()
                .WithPlayer("Player1", GamePhase.Entered, 350)
                .WithPlayer("Player2", GamePhase.Entered, 400)
                .WithPlayer("Player3", GamePhase.Entered, 600)
                .WithPlayerToTest(bot, GamePhase.Entered, 780)
                .WithPlayerHistory([660, 700, 750])
                .Build();

            var turnEnded = bot.EndTurn(30, gameStateOverview, 0);
            Assert.True(turnEnded);
        }

        [Fact]
        public void LittleRiskBot_CheckIfBotEndsTurn_WhenBotIsFirstWithBigAdvantage()
        {
            var bot = new LittleRiskBotPlayer("Bot");
            var gameStateOverview = new GameStateOverviewBuilder()
                .WithPlayer("Player1", GamePhase.Entered, 350)
                .WithPlayer("Player2", GamePhase.Entered, 400)
                .WithPlayer("Player3", GamePhase.Entered, 600)
                .WithPlayerToTest(bot, GamePhase.Entered, 810)
                .WithPlayerHistory([670, 700, 750])
                .Build();

            var turnEnded = bot.EndTurn(30, gameStateOverview, 0);
            Assert.False(turnEnded);
        }

        [Fact]
        public void LittleRiskBot_CheckIfBotEndsTurn_WhenBotHasntEnteredTheGame()
        {
            var bot = new LittleRiskBotPlayer("Bot");
            var gameStateOverview = new GameStateOverviewBuilder()
                .WithPlayer("Player1", GamePhase.NotEntered, 0)
                .WithPlayer("Player2", GamePhase.NotEntered, 0)
                .WithPlayer("Player3", GamePhase.NotEntered, 0)
                .WithPlayerToTest(bot, GamePhase.Entered, 100)
                .Build();

            var turnEnded = bot.EndTurn(100, gameStateOverview, 0);
            Assert.True(turnEnded);
        }
    }
}
