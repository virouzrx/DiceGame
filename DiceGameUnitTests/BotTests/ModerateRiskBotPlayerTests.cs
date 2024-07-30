using DiceGameConsoleVersion.GameLogic;
using DiceGameConsoleVersion.GameLogic.ProbabilityHelpers;
using DiceGameConsoleVersion.Logic;
using DiceGameConsoleVersion.Models;
using DiceGameConsoleVersion.Players;

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
    }
}