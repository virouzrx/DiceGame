using DiceGame.Common.Players.Bots;

namespace DiceGameUnitTests.BotTests
{
    public class ModerateRiskBotPlayerTests
    {
        public ProbabilityHelper ProbabilityHelper { get; set; } = new();
        [Fact]
        public void ModerateRiskBot_CheckIfBotEndsTurn_WhenBotHasntScoredInLast3Rounds()
        {
            var bot = new ModerateRiskBotPlayer("Bot", ProbabilityHelper) { Score = 100, CurrentGamePhase = GamePhase.Entered };
            var gameHistory = new GameHistory
            {
                History = new List<List<IPlayer>>
                {
                    new List<IPlayer>
                    {
                        bot
                    },
                    new List<IPlayer>
                    {
                        bot
                    },
                    new List<IPlayer>
                    {
                        bot
                    }
                }
            };

            var turnEnded = bot.EndTurn(30, gameHistory, 3);
            Assert.True(turnEnded);
        }

        [Fact]
        public void ModerateRiskBot_CheckIfBotEndsTurn_WhenNoOneEnteredTheGame_WithInsufficientScore_AndLowProbability()
        {
            var bot = new ModerateRiskBotPlayer("Bot", ProbabilityHelper) { Score = 0, CurrentGamePhase = GamePhase.NotEntered };
            var gameHistory = new GameHistory
            {
                History = new List<List<IPlayer>>
                {
                    new List<IPlayer>
                    {
                        new HumanPlayer("Chester"),
                        new HumanPlayer("Steven"),
                        new HumanPlayer("Jack"),
                        bot
                    }
                }
            };

            var turnEnded = bot.EndTurn(110, gameHistory, 5);
            Assert.True(turnEnded);
        }

        [Fact]
        public void ModerateRiskBot_CheckIfBotEndsTurn_WhenNoOneEnteredTheGame_WithSufficientScore_AndLowProbability()
        {
            var bot = new ModerateRiskBotPlayer("Bot", ProbabilityHelper) { Score = 0, CurrentGamePhase = GamePhase.NotEntered };
            var gameHistory = new GameHistory
            {
                History = new List<List<IPlayer>>
                {
                    new List<IPlayer>
                    {
                        new HumanPlayer("Chester"),
                        new HumanPlayer("Steven"),
                        new HumanPlayer("Jack"),
                        bot
                    }
                }
            };

            var turnEnded = bot.EndTurn(110, gameHistory, 5);
            Assert.True(turnEnded);
        }

        [Fact]
        public void ModerateRiskBot_CheckIfBotEndsTurn_WhenNoOneEnteredTheGame_WithInsufficientScore_AndHighProbability()
        {
            var bot = new ModerateRiskBotPlayer("Bot", ProbabilityHelper) { Score = 0, CurrentGamePhase = GamePhase.NotEntered };
            var gameHistory = new GameHistory
            {
                History = new List<List<IPlayer>>
                {
                    new List<IPlayer>
                    {
                        new HumanPlayer("Chester"),
                        new HumanPlayer("Steven"),
                        new HumanPlayer("Jack"),
                        bot
                    }
                }
            };

            var turnEnded = bot.EndTurn(110, gameHistory, 2);
            Assert.False(turnEnded);
        }

        [Fact]
        public void ModerateRiskBot_CheckIfBotEndsTurn_WhenBotThrows100_AndRestOfThePlayersEnteredTheGame()
        {
            var bot = new ModerateRiskBotPlayer("Bot", ProbabilityHelper) { Score = 0, CurrentGamePhase = GamePhase.NotEntered };
            var gameHistory = new GameHistory
            {
                History = new List<List<IPlayer>>
                {
                    new List<IPlayer>
                    {
                        new HumanPlayer("Chester") {Score = 120, CurrentGamePhase = GamePhase.Entered},
                        new HumanPlayer("Steven") { Score = 130, CurrentGamePhase = GamePhase.Entered},
                        new HumanPlayer("Jack") { Score = 160, CurrentGamePhase = GamePhase.Entered},
                        bot
                    }
                }
            };

            var turnEnded = bot.EndTurn(100, gameHistory, 1);
            Assert.True(turnEnded);
        }

        [Fact]
        public void ModerateRiskBot_CheckIfBotContinues_WhenBotCanOvertakePlayerOnEntering()
        {
            var bot = new ModerateRiskBotPlayer("Bot", ProbabilityHelper) { Score = 0, CurrentGamePhase = GamePhase.NotEntered };
            var gameHistory = new GameHistory
            {
                History = new List<List<IPlayer>>
                {
                    new List<IPlayer>
                    {
                        new HumanPlayer("Chester") {Score = 0, CurrentGamePhase = GamePhase.NotEntered},
                        new HumanPlayer("Steven") { Score = 300, CurrentGamePhase = GamePhase.Entered},
                        new HumanPlayer("Jack") { Score = 110, CurrentGamePhase = GamePhase.Entered},
                        bot
                    }
                }
            };

            var turnEnded = bot.EndTurn(100, gameHistory, 1);
            Assert.False(turnEnded);
        }

        [Fact]
        public void ModerateRiskBot_CheckIfBotEndsTurn_WhenBotCanHaveScoreJustBelow900()
        {
            var bot = new ModerateRiskBotPlayer("Bot", ProbabilityHelper) { Score = 860, CurrentGamePhase = GamePhase.Entered };

            var gameHistory = new GameHistory
            {
                History = new List<List<IPlayer>>
                {
                    new List<IPlayer>
                    {
                        new HumanPlayer("Chester") {Score = 0, CurrentGamePhase = GamePhase.NotEntered},
                        new HumanPlayer("Steven") { Score = 300, CurrentGamePhase = GamePhase.Entered},
                        new HumanPlayer("Jack") { Score = 110, CurrentGamePhase = GamePhase.Entered},
                        bot
                    }
                }
            };

            var turnEnded = bot.EndTurn(30, gameHistory, 1);
            Assert.True(turnEnded);
        }

        [Fact]
        public void ModerateRiskBot_CheckIfBotContinues_WhenBotHasScoreBetween900And940()
        {
            var bot = new ModerateRiskBotPlayer("Bot", ProbabilityHelper) { Score = 880, CurrentGamePhase = GamePhase.Entered };

            var gameHistory = new GameHistory
            {
                History = new List<List<IPlayer>>
                {
                    new List<IPlayer>
                    {
                        new HumanPlayer("Chester") {Score = 0, CurrentGamePhase = GamePhase.NotEntered},
                        new HumanPlayer("Steven") { Score = 300, CurrentGamePhase = GamePhase.Entered},
                        new HumanPlayer("Jack") { Score = 110, CurrentGamePhase = GamePhase.Entered},
                        bot
                    }
                }
            };

            var turnEnded = bot.EndTurn(30, gameHistory, 1);
            Assert.False(turnEnded);
        }

        [Fact]
        public void ModerateRiskBot_CheckIfBotContinues_WhenOtherPlayersHave75OrLessPointsAdvantage_AndPlayerBelowIsBehind100Points()
        {
            var bot = new ModerateRiskBotPlayer("Bot", ProbabilityHelper) { Score = 330, CurrentGamePhase = GamePhase.Entered };

            var gameHistory = new GameHistory
            {
                History = new List<List<IPlayer>>
                {
                    new List<IPlayer>
                    {
                        new HumanPlayer("Chester") {Score = 0, CurrentGamePhase = GamePhase.NotEntered},
                        new HumanPlayer("Steven") { Score = 400, CurrentGamePhase = GamePhase.Entered},
                        new HumanPlayer("Jack") { Score = 110, CurrentGamePhase = GamePhase.Entered},
                        bot
                    }
                }
            };

            var turnEnded = bot.EndTurn(30, gameHistory, 1);
            Assert.False(turnEnded);
        }

        [Fact]
        public void ModerateRiskBot_ChooseDiceTest_WhenSetWithTwosGive45OrMorePoints()
        {
            var bot = new ModerateRiskBotPlayer("Bot", ProbabilityHelper) { Score = 200, CurrentGamePhase = GamePhase.Entered };

            var gameHistory = new GameHistory();

            var dice = new[] { new PointableDice(2, 4), new PointableDice(5, 1) };

            var diceChosen = bot.ChooseDice(dice, gameHistory, 1);
            Assert.Equal(dice, diceChosen);
        }

        [Fact]
        public void ModerateRiskBot_ChooseDiceTest_WhenSetOfTwosGiveLessThan45Points()
        {
            var bot = new ModerateRiskBotPlayer("Bot", ProbabilityHelper) { Score = 200, CurrentGamePhase = GamePhase.Entered };

            var gameHistory = new GameHistory();

            var dice = new[] { new PointableDice(2, 3), new PointableDice(5, 1) };

            var diceChosen = bot.ChooseDice(dice, gameHistory, 1);
            Assert.Equal(new[] { new PointableDice(5, 1) }, diceChosen);
        }


        [Fact]
        public void ModerateRiskBot_ChooseDiceTest_TwoDiceOfTheSameScoreAreThrown_AndMoreThan3DiceWerePointed()
        {
            var bot = new ModerateRiskBotPlayer("Bot", ProbabilityHelper) { Score = 200, CurrentGamePhase = GamePhase.Entered };

            var gameHistory = new GameHistory();

            var dice = new[] { new PointableDice(1, 2) };

            var diceChosen = bot.ChooseDice(dice, gameHistory, 3);
            Assert.Equal(dice, diceChosen);
        }

        [Fact]
        public void ModerateRiskBot_ChooseDiceTest_TwoDiceOfTheSameScoreAreThrown_AndLessThan3DiceWerePointed()
        {
            var bot = new ModerateRiskBotPlayer("Bot", ProbabilityHelper) { Score = 200, CurrentGamePhase = GamePhase.Entered };

            var gameHistory = new GameHistory();

            var dice = new[] { new PointableDice(1, 2) };

            var diceChosen = bot.ChooseDice(dice, gameHistory, 1);
            Assert.Equal(new[] { new PointableDice(1, 1) }, diceChosen);
        }

        [Fact]
        public void ModerateRiskBot_ChooseDiceTest_WhenTripletIsThrown()
        {
            var bot = new ModerateRiskBotPlayer("Bot", ProbabilityHelper) { Score = 200, CurrentGamePhase = GamePhase.Entered };

            var gameHistory = new GameHistory();

            var dice = new[] { new PointableDice(5, 3) };

            var diceChosen = bot.ChooseDice(dice, gameHistory, 2);
            Assert.Equal(dice, diceChosen);
        }

        [Fact]
        public void ModerateRiskBot_ChooseDiceTest_When1And5AreThrown()
        {
            var bot = new ModerateRiskBotPlayer("Bot", ProbabilityHelper) { Score = 200, CurrentGamePhase = GamePhase.Entered };

            var gameHistory = new GameHistory();

            var dice = new[] { new PointableDice(1, 1), new PointableDice(5,1) };

            var diceChosen = bot.ChooseDice(dice, gameHistory, 2);
            Assert.Equal(new[] { new PointableDice(1,1)}, diceChosen);
        }

        [Fact]
        public void ModerateRiskBot_ChooseDiceTest_When3DifferentPointableDiceAreThrown_ButPlayerCanAchieveHigherScore()
        {
            var bot = new ModerateRiskBotPlayer("Bot", ProbabilityHelper) { Score = 200, CurrentGamePhase = GamePhase.Entered };

            var gameHistory = new GameHistory();

            var dice = new[] { new PointableDice(1, 1), new PointableDice(5, 1), new PointableDice(2,3) };

            var diceChosen = bot.ChooseDice(dice, gameHistory, 4);
            Assert.Equal(new[] { new PointableDice(1, 1) }, diceChosen);
        }

        [Fact]
        public void ModerateRiskBot_ChooseDiceTest_When3DifferentPointableDiceAreThrown_AndChancesForHigherScoreAreLow()
        {
            var bot = new ModerateRiskBotPlayer("Bot", ProbabilityHelper) { Score = 200, CurrentGamePhase = GamePhase.Entered };

            var gameHistory = new GameHistory();

            var dice = new[] { new PointableDice(1, 1), new PointableDice(5, 1), new PointableDice(5, 3) };

            var diceChosen = bot.ChooseDice(dice, gameHistory, 2);
            Assert.Equal(dice, diceChosen);
        }
    }
}