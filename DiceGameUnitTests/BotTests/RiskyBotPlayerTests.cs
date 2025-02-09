using DiceGame.Common.Players.Bots;
using DiceGame.UnitTests.Helpers;

namespace DiceGame.UnitTests.BotTests
{
    public class RiskyBotPlayerTests
    {
        public ProbabilityHelper ProbabilityHelper { get; set; } = new();
        [Fact]
        public void RiskyBot_CheckIfBotEndsTurn_WhenBotHasntScoredInLast3Rounds()
        {
            var bot = new RiskyBotPlayer("Bot", ProbabilityHelper);
            var gameStateOverview = new GameStateBuilder()
                .WithPlayer("Player1", GamePhase.Entered, 200)
                .WithPlayer("Player2", GamePhase.Entered, 500)
                .WithPlayer("Player3", GamePhase.Entered, 600)
                .WithPlayerToTest(bot, GamePhase.Entered, 300)
                .WithPlayerHistory([300, 300, 300])
                .BuildGameStateOverview();

            var turnEnded = bot.EndTurn(30, gameStateOverview, 5);
            Assert.True(turnEnded);
        }

        [Fact]
        public void RiskyBot_CheckIfBotEndsTurn_WhenNoOneEnteredTheGame_WithInsufficientScore_AndLowProbability()
        {
            var bot = new RiskyBotPlayer("Bot", ProbabilityHelper);
            var gameStateOverview = new GameStateBuilder()
                .WithPlayer("Player1", GamePhase.NotEntered, 0)
                .WithPlayer("Player2", GamePhase.NotEntered, 0)
                .WithPlayer("Player3", GamePhase.NotEntered, 0)
                .WithPlayerToTest(bot, GamePhase.NotEntered, 0)
                .BuildGameStateOverview();

            var turnEnded = bot.EndTurn(110, gameStateOverview, 5);
            Assert.True(turnEnded);
        }

        [Fact]
        public void RiskyBot_CheckIfBotEndsTurn_WhenNoOneEnteredTheGame_WithSufficientScore_AndLowProbability()
        {
            var bot = new RiskyBotPlayer("Bot", ProbabilityHelper);
            var gameStateOverview = new GameStateBuilder()
                .WithPlayer("Player1", GamePhase.NotEntered, 0)
                .WithPlayer("Player2", GamePhase.NotEntered, 0)
                .WithPlayer("Player3", GamePhase.NotEntered, 0)
                .WithPlayerToTest(bot, GamePhase.NotEntered, 0)
                .BuildGameStateOverview();

            var turnEnded = bot.EndTurn(120, gameStateOverview, 5);
            Assert.True(turnEnded);
        }

        [Fact]
        public void RiskyBot_CheckIfBotEndsTurn_WhenNoOneEnteredTheGame_WithInsufficientScore_AndHighProbability()
        {
            var bot = new RiskyBotPlayer("Bot", ProbabilityHelper);
            var gameStateOverview = new GameStateBuilder()
                .WithPlayer("Player1", GamePhase.NotEntered, 0)
                .WithPlayer("Player2", GamePhase.NotEntered, 0)
                .WithPlayer("Player3", GamePhase.NotEntered, 0)
                .WithPlayerToTest(bot, GamePhase.NotEntered, 0)
                .BuildGameStateOverview();


            var turnEnded = bot.EndTurn(110, gameStateOverview, 2);
            Assert.False(turnEnded);
        }

        [Fact]
        public void RiskyBot_CheckIfBotEndsTurn_WhenBotThrows100_AndRestOfThePlayersEnteredTheGame()
        {
            var bot = new RiskyBotPlayer("Bot", ProbabilityHelper);
            var gameStateOverview = new GameStateBuilder()
                .WithPlayer("Player1", GamePhase.Entered, 120)
                .WithPlayer("Player2", GamePhase.Entered, 130)
                .WithPlayer("Player3", GamePhase.Entered, 160)
                .WithPlayerToTest(bot, GamePhase.NotEntered, 0)
                .BuildGameStateOverview();

            var turnEnded = bot.EndTurn(100, gameStateOverview, 1);
            Assert.True(turnEnded);
        }

        [Fact]
        public void RiskyBot_CheckIfBotContinues_WhenBotCanOvertakePlayerOnEntering()
        {
            var bot = new RiskyBotPlayer("Bot", ProbabilityHelper);
            var gameStateOverview = new GameStateBuilder()
                .WithPlayer("Player1", GamePhase.NotEntered, 0)
                .WithPlayer("Player2", GamePhase.Entered, 110)
                .WithPlayer("Player3", GamePhase.Entered, 300)
                .WithPlayerToTest(bot, GamePhase.NotEntered, 0)
                .BuildGameStateOverview();

            var turnEnded = bot.EndTurn(100, gameStateOverview, 1);
            Assert.False(turnEnded);
        }

        [Fact]
        public void RiskyBot_CheckIfBotEndsTurn_WhenBotCanHaveScoreJustBelow900()
        {
            var bot = new RiskyBotPlayer("Bot", ProbabilityHelper);
            var gameStateOverview = new GameStateBuilder()
                .WithPlayer("Player1", GamePhase.NotEntered, 0)
                .WithPlayer("Player2", GamePhase.Entered, 110)
                .WithPlayer("Player3", GamePhase.Entered, 300)
                .WithPlayerToTest(bot, GamePhase.Entered, 860)
                .BuildGameStateOverview();

            var turnEnded = bot.EndTurn(35, gameStateOverview, 1);
            Assert.True(turnEnded);
        }

        [Fact]
        public void RiskyBot_CheckIfBotContinues_WhenBotHasScoreBetween900And940()
        {
            var bot = new RiskyBotPlayer("Bot", ProbabilityHelper);
            var gameStateOverview = new GameStateBuilder()
                .WithPlayer("Player1", GamePhase.NotEntered, 0)
                .WithPlayer("Player2", GamePhase.Entered, 130)
                .WithPlayer("Player3", GamePhase.Entered, 160)
                .WithPlayerToTest(bot, GamePhase.Entered, 880)
                .BuildGameStateOverview();

            var turnEnded = bot.EndTurn(30, gameStateOverview, 1);
            Assert.False(turnEnded);
        }

        [Fact]
        public void RiskyBot_CheckIfBotContinues_WhenOtherPlayersHave75OrLessPointsAdvantage_AndPlayerBelowIsBehind100Points()
        {
            var bot = new RiskyBotPlayer("Bot", ProbabilityHelper);
            var gameStateOverview = new GameStateBuilder()
                .WithPlayer("Player1", GamePhase.NotEntered, 0)
                .WithPlayer("Player2", GamePhase.Entered, 110)
                .WithPlayer("Player3", GamePhase.Entered, 400)
                .WithPlayerToTest(bot, GamePhase.Entered, 330)
                .BuildGameStateOverview();

            var turnEnded = bot.EndTurn(30, gameStateOverview, 1);
            Assert.False(turnEnded);
        }

        [Fact]
        public void RiskyBot_ChooseDiceTest_WhenSetWithTwosGive45OrMorePoints()
        {
            var bot = new RiskyBotPlayer("Bot", ProbabilityHelper);
            var dice = new[] { new PointableDice(2, 4), new PointableDice(5, 1) };

            var diceChosen = bot.ChooseDice(dice, 1);
            Assert.Equal(dice, diceChosen);
        }

        [Fact]
        public void RiskyBot_ChooseDiceTest_WhenSetOfTwosGiveLessThan45Points()
        {
            var bot = new RiskyBotPlayer("Bot", ProbabilityHelper);
            var dice = new[] { new PointableDice(2, 3), new PointableDice(5, 1) };

            var diceChosen = bot.ChooseDice(dice, 1);
            Assert.Equal([new PointableDice(5, 1)], diceChosen);
        }


        [Fact]
        public void RiskyBot_ChooseDiceTest_TwoDiceOfTheSameScoreAreThrown_AndMoreThan3DiceWerePointed()
        {
            var bot = new RiskyBotPlayer("Bot", ProbabilityHelper);
            var dice = new[] { new PointableDice(1, 2) };

            var diceChosen = bot.ChooseDice(dice, 3);
            Assert.Equal(dice, diceChosen);
        }

        [Fact]
        public void RiskyBot_ChooseDiceTest_TwoDiceOfTheSameScoreAreThrown_AndLessThan3DiceWerePointed()
        {
            var bot = new RiskyBotPlayer("Bot", ProbabilityHelper);
            var dice = new[] { new PointableDice(1, 2) };

            var diceChosen = bot.ChooseDice(dice, 1);
            Assert.Equal([new PointableDice(1, 1)], diceChosen);
        }

        [Fact]
        public void RiskyBot_ChooseDiceTest_WhenTripletIsThrown()
        {
            var bot = new RiskyBotPlayer("Bot", ProbabilityHelper);
            var dice = new[] { new PointableDice(5, 3) };

            var diceChosen = bot.ChooseDice(dice, 2);
            Assert.Equal(dice, diceChosen);
        }

        [Fact]
        public void RiskyBot_ChooseDiceTest_When1And5AreThrown()
        {
            var bot = new RiskyBotPlayer("Bot", ProbabilityHelper);
            var dice = new[] { new PointableDice(1, 1), new PointableDice(5, 1) };

            var diceChosen = bot.ChooseDice(dice, 2);
            Assert.Equal([new PointableDice(1, 1)], diceChosen);
        }

        [Fact]
        public void RiskyBot_ChooseDiceTest_When3DifferentPointableDiceAreThrown_ButPlayerCanAchieveHigherScore()
        {
            var bot = new RiskyBotPlayer("Bot", ProbabilityHelper);
            var dice = new[] { new PointableDice(1, 1), new PointableDice(5, 1), new PointableDice(2, 3) };

            var diceChosen = bot.ChooseDice(dice, 4);
            Assert.Equal([new PointableDice(1, 1)], diceChosen);
        }

        [Fact]
        public void RiskyBot_ChooseDiceTest_When3DifferentPointableDiceAreThrown_AndChancesForHigherScoreAreLow()
        {
            var bot = new RiskyBotPlayer("Bot", ProbabilityHelper);
            var dice = new[] { new PointableDice(1, 1), new PointableDice(5, 1), new PointableDice(5, 3) };

            var diceChosen = bot.ChooseDice(dice, 2);
            Assert.Equal([new PointableDice(1, 1)], diceChosen);
        }
    }
}
