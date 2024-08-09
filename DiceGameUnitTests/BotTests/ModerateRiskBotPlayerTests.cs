﻿using DiceGame.Common.Players.Bots;
using DiceGame.UnitTests.Helpers;

namespace DiceGame.UnitTests.BotTests
{
    public class ModerateRiskBotPlayerTests
    {
        public ProbabilityHelper ProbabilityHelper { get; set; } = new();
        [Fact]
        public void ModerateRiskBot_CheckIfBotEndsTurn_WhenBotHasntScoredInLast3Rounds()
        {
            var bot = new ModerateRiskBotPlayer("Bot", ProbabilityHelper);
            var gameStateOverview = new GameStateOverviewBuilder()
                .WithPlayer("Player1", GamePhase.NotEntered, 300)
                .WithPlayer("Player2", GamePhase.Entered, 350)
                .WithPlayer("Player3", GamePhase.Entered, 400)
                .WithPlayerToTest(bot, GamePhase.Entered, 500)
                .WithPlayerHistory([500, 500, 500])
                .Build();

            var turnEnded = bot.EndTurn(30, gameStateOverview, 3);
            Assert.True(turnEnded);
        }

        [Fact]
        public void ModerateRiskBot_CheckIfBotEndsTurn_WhenNoOneEnteredTheGame_WithInsufficientScore_AndLowProbability()
        {
            var bot = new ModerateRiskBotPlayer("Bot", ProbabilityHelper);
            var gameStateOverview = new GameStateOverviewBuilder()
                .WithPlayer("Player1", GamePhase.NotEntered, 0)
                .WithPlayer("Player2", GamePhase.NotEntered, 0)
                .WithPlayer("Player3", GamePhase.NotEntered, 0)
                .WithPlayerToTest(bot, GamePhase.NotEntered, 0)
                .Build();

            var turnEnded = bot.EndTurn(110, gameStateOverview, 5);
            Assert.True(turnEnded);
        }

        [Fact]
        public void ModerateRiskBot_CheckIfBotEndsTurn_WhenNoOneEnteredTheGame_WithSufficientScore_AndLowProbability()
        {
            var bot = new ModerateRiskBotPlayer("Bot", ProbabilityHelper);
            var gameStateOverview = new GameStateOverviewBuilder()
                .WithPlayer("Player1", GamePhase.NotEntered, 0)
                .WithPlayer("Player2", GamePhase.NotEntered, 0)
                .WithPlayer("Player3", GamePhase.NotEntered, 0)
                .WithPlayerToTest(bot, GamePhase.NotEntered, 0)
                .Build();

            var turnEnded = bot.EndTurn(120, gameStateOverview, 5);
            Assert.True(turnEnded);
        }

        [Fact]
        public void ModerateRiskBot_CheckIfBotEndsTurn_WhenNoOneEnteredTheGame_WithInsufficientScore_AndHighProbability()
        {
            var bot = new ModerateRiskBotPlayer("Bot", ProbabilityHelper);
            var gameStateOverview = new GameStateOverviewBuilder()
                .WithPlayer("Player1", GamePhase.NotEntered, 0)
                .WithPlayer("Player2", GamePhase.NotEntered, 0)
                .WithPlayer("Player3", GamePhase.NotEntered, 0)
                .WithPlayerToTest(bot, GamePhase.NotEntered, 0)
                .Build();

            var turnEnded = bot.EndTurn(110, gameStateOverview, 2);
            Assert.False(turnEnded);
        }

        [Fact]
        public void ModerateRiskBot_CheckIfBotEndsTurn_WhenBotThrows100_AndRestOfThePlayersEnteredTheGame()
        {
            var bot = new ModerateRiskBotPlayer("Bot", ProbabilityHelper);
            var gameStateOverview = new GameStateOverviewBuilder()
                .WithPlayer("Player1", GamePhase.NotEntered, 120)
                .WithPlayer("Player2", GamePhase.Entered, 130)
                .WithPlayer("Player3", GamePhase.Entered, 160)
                .WithPlayerToTest(bot, GamePhase.NotEntered, 0)
                .Build();

            var turnEnded = bot.EndTurn(100, gameStateOverview, 1);
            Assert.True(turnEnded);
        }

        [Fact]
        public void ModerateRiskBot_CheckIfBotContinues_WhenBotCanOvertakePlayerOnEntering()
        {
            var bot = new ModerateRiskBotPlayer("Bot", ProbabilityHelper);
            var gameStateOverview = new GameStateOverviewBuilder()
                .WithPlayer("Player1", GamePhase.NotEntered, 0)
                .WithPlayer("Player2", GamePhase.Entered, 110)
                .WithPlayer("Player3", GamePhase.Entered, 300)
                .WithPlayerToTest(bot, GamePhase.NotEntered, 0)
                .Build();

            var turnEnded = bot.EndTurn(100, gameStateOverview, 1);
            Assert.False(turnEnded);
        }

        [Fact]
        public void ModerateRiskBot_CheckIfBotEndsTurn_WhenBotCanHaveScoreJustBelow900()
        {
            var bot = new ModerateRiskBotPlayer("Bot", ProbabilityHelper);
            var gameStateOverview = new GameStateOverviewBuilder()
                .WithPlayer("Player1", GamePhase.NotEntered, 0)
                .WithPlayer("Player2", GamePhase.Entered, 110)
                .WithPlayer("Player3", GamePhase.Entered, 300)
                .WithPlayerToTest(bot, GamePhase.Entered, 860)
                .Build();

            var turnEnded = bot.EndTurn(30, gameStateOverview, 1);
            Assert.True(turnEnded);
        }

        [Fact]
        public void ModerateRiskBot_CheckIfBotContinues_WhenBotHasScoreBetween900And940()
        {
            var bot = new ModerateRiskBotPlayer("Bot", ProbabilityHelper);
            var gameStateOverview = new GameStateOverviewBuilder()
                .WithPlayer("Player1", GamePhase.NotEntered, 0)
                .WithPlayer("Player2", GamePhase.Entered, 110)
                .WithPlayer("Player3", GamePhase.Entered, 300)
                .WithPlayerToTest(bot, GamePhase.Entered, 880)
                .Build();

            var turnEnded = bot.EndTurn(30, gameStateOverview, 1);
            Assert.False(turnEnded);
        }

        [Fact]
        public void ModerateRiskBot_CheckIfBotContinues_WhenOtherPlayersHave75OrLessPointsAdvantage_AndPlayerBelowIsBehind100Points()
        {
            var bot = new ModerateRiskBotPlayer("Bot", ProbabilityHelper);
            var gameStateOverview = new GameStateOverviewBuilder()
                .WithPlayer("Player1", GamePhase.NotEntered, 0)
                .WithPlayer("Player2", GamePhase.Entered, 400)
                .WithPlayer("Player3", GamePhase.Entered, 110)
                .WithPlayerToTest(bot, GamePhase.Entered, 330)
                .Build();

            var turnEnded = bot.EndTurn(30, gameStateOverview, 1);
            Assert.False(turnEnded);
        }

        [Fact]
        public void ModerateRiskBot_ChooseDiceTest_WhenSetWithTwosGive45OrMorePoints()
        {
            var bot = new ModerateRiskBotPlayer("Bot", ProbabilityHelper);

            var dice = new[] { new PointableDice(2, 4), new PointableDice(5, 1) };

            var diceChosen = bot.ChooseDice(dice, 1);
            Assert.Equal(dice, diceChosen);
        }

        [Fact]
        public void ModerateRiskBot_ChooseDiceTest_WhenSetOfTwosGiveLessThan45Points()
        {
            var bot = new ModerateRiskBotPlayer("Bot", ProbabilityHelper);

            var dice = new[] { new PointableDice(2, 3), new PointableDice(5, 1) };

            var diceChosen = bot.ChooseDice(dice, 1);
            Assert.Equal([new PointableDice(5, 1)], diceChosen);
        }


        [Fact]
        public void ModerateRiskBot_ChooseDiceTest_TwoDiceOfTheSameScoreAreThrown_AndMoreThan3DiceWerePointed()
        {
            var bot = new ModerateRiskBotPlayer("Bot", ProbabilityHelper);

            var dice = new[] { new PointableDice(1, 2) };

            var diceChosen = bot.ChooseDice(dice, 3);
            Assert.Equal(dice, diceChosen);
        }

        [Fact]
        public void ModerateRiskBot_ChooseDiceTest_TwoDiceOfTheSameScoreAreThrown_AndLessThan3DiceWerePointed()
        {
            var bot = new ModerateRiskBotPlayer("Bot", ProbabilityHelper);

            var dice = new[] { new PointableDice(1, 2) };

            var diceChosen = bot.ChooseDice(dice, 1);
            Assert.Equal([new PointableDice(1, 1)], diceChosen);
        }

        [Fact]
        public void ModerateRiskBot_ChooseDiceTest_WhenTripletIsThrown()
        {
            var bot = new ModerateRiskBotPlayer("Bot", ProbabilityHelper);

            var dice = new[] { new PointableDice(5, 3) };

            var diceChosen = bot.ChooseDice(dice, 2);
            Assert.Equal(dice, diceChosen);
        }

        [Fact]
        public void ModerateRiskBot_ChooseDiceTest_When1And5AreThrown()
        {
            var bot = new ModerateRiskBotPlayer("Bot", ProbabilityHelper);

            var gameHistory = new GameHistory();

            var dice = new[] { new PointableDice(1, 1), new PointableDice(5, 1) };

            var diceChosen = bot.ChooseDice(dice, 2);
            Assert.Equal([new PointableDice(1, 1)], diceChosen);
        }

        [Fact]
        public void ModerateRiskBot_ChooseDiceTest_When3DifferentPointableDiceAreThrown_ButPlayerCanAchieveHigherScore()
        {
            var bot = new ModerateRiskBotPlayer("Bot", ProbabilityHelper);

            var dice = new[] { new PointableDice(1, 1), new PointableDice(5, 1), new PointableDice(2, 3) };

            var diceChosen = bot.ChooseDice(dice, 4);
            Assert.Equal([new PointableDice(1, 1)], diceChosen);
        }

        [Fact]
        public void ModerateRiskBot_ChooseDiceTest_When3DifferentPointableDiceAreThrown_AndChancesForHigherScoreAreLow()
        {
            
            var bot = new ModerateRiskBotPlayer("Bot", ProbabilityHelper);


            var dice = new[] { new PointableDice(1, 1), new PointableDice(5, 1), new PointableDice(5, 3) };

            var diceChosen = bot.ChooseDice(dice, 2);
            Assert.Equal(dice, diceChosen);
        }
    }
}