using DiceGame.Common.Players.Bots;

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
            var bot = new LittleRiskBotPlayer("Bot") { Score = 100, CurrentGamePhase = GamePhase.Entered };
            var gameHistory = new GameHistory
            {
                History = new List<List<IPlayer>>
                {
                    new List<IPlayer>
                    {
                        new HumanPlayer("Chester") { Score = 100, CurrentGamePhase = GamePhase.Entered },
                        new HumanPlayer("Steven") { Score = 100, CurrentGamePhase = GamePhase.Entered },
                        new HumanPlayer("Jack") { Score = 100, CurrentGamePhase = GamePhase.Entered },
                        bot
                    },
                    new List<IPlayer>
                    {
                        new HumanPlayer("Chester") { Score = 150, CurrentGamePhase = GamePhase.Entered },
                        new HumanPlayer("Steven") { Score = 130, CurrentGamePhase = GamePhase.Entered },
                        new HumanPlayer("Jack") { Score = 200, CurrentGamePhase = GamePhase.Entered },
                        bot
                    },
                    new List<IPlayer>
                    {
                        new HumanPlayer("Chester") { Score = 180, CurrentGamePhase = GamePhase.Entered },
                        new HumanPlayer("Steven") { Score = 200, CurrentGamePhase = GamePhase.Entered },
                        new HumanPlayer("Jack") { Score = 240, CurrentGamePhase = GamePhase.Entered },
                        bot
                    }
                }
            };

            var turnEnded = bot.EndTurn(30, gameHistory, 3);
            Assert.True(turnEnded);
        }

        [Fact]
        public void LittleRiskBot_CheckIfBotEndsTurn_WhenBotIsLast()
        {
            var bot = new LittleRiskBotPlayer("Bot") { Score = 280, CurrentGamePhase = GamePhase.Entered };
            var gameHistory = new GameHistory
            {
                History = new List<List<IPlayer>>
                {
                    new List<IPlayer>
                    {
                        new HumanPlayer("Chester") { Score = 200, CurrentGamePhase = GamePhase.Entered },
                        new HumanPlayer("Steven") { Score = 200, CurrentGamePhase = GamePhase.Entered },
                        new HumanPlayer("Jack") { Score = 200, CurrentGamePhase = GamePhase.Entered },
                        new LittleRiskBotPlayer(bot.Name!) { Score = 200, CurrentGamePhase = GamePhase.Entered }
                    },
                    new List<IPlayer>
                    {
                        new HumanPlayer("Chester") { Score = 300, CurrentGamePhase = GamePhase.Entered },
                        new HumanPlayer("Steven") { Score = 350, CurrentGamePhase = GamePhase.Entered },
                        new HumanPlayer("Jack") { Score = 320, CurrentGamePhase = GamePhase.Entered },
                        new LittleRiskBotPlayer(bot.Name!) { Score = 230, CurrentGamePhase = GamePhase.Entered }
                    },
                    new List<IPlayer>
                    {
                        new HumanPlayer("Chester") { Score = 350, CurrentGamePhase = GamePhase.Entered },
                        new HumanPlayer("Steven") { Score = 400, CurrentGamePhase = GamePhase.Entered },
                        new HumanPlayer("Jack") { Score = 600, CurrentGamePhase = GamePhase.Entered },
                        bot
                    }
                }
            };

            var turnEnded = bot.EndTurn(30, gameHistory, 0);
            Assert.True(turnEnded);
        }

        [Fact]
        public void LittleRiskBot_CheckIfBotEndsTurn_WhenBotIsFirstWithoutBigAdvantage()
        {
            var bot = new LittleRiskBotPlayer("Bot") { Score = 780, CurrentGamePhase = GamePhase.Entered };
            var gameHistory = new GameHistory
            {
                History = new List<List<IPlayer>>
                {
                    new List<IPlayer>
                    {
                        new HumanPlayer("Chester") { Score = 200, CurrentGamePhase = GamePhase.Entered },
                        new HumanPlayer("Steven") { Score = 250, CurrentGamePhase = GamePhase.Entered },
                        new HumanPlayer("Jack") { Score = 280, CurrentGamePhase = GamePhase.Entered },
                        new LittleRiskBotPlayer(bot.Name!) { Score = 700, CurrentGamePhase = GamePhase.Entered }
                    },
                    new List<IPlayer>
                    {
                        new HumanPlayer("Chester") { Score = 230, CurrentGamePhase = GamePhase.Entered },
                        new HumanPlayer("Steven") { Score = 280, CurrentGamePhase = GamePhase.Entered },
                        new HumanPlayer("Jack") { Score = 330, CurrentGamePhase = GamePhase.Entered },
                        new LittleRiskBotPlayer(bot.Name!) { Score = 750, CurrentGamePhase = GamePhase.Entered }
                    },
                    new List<IPlayer>
                    {
                        new HumanPlayer("Chester") { Score = 350, CurrentGamePhase = GamePhase.Entered },
                        new HumanPlayer("Steven") { Score = 400, CurrentGamePhase = GamePhase.Entered },
                        new HumanPlayer("Jack") { Score = 600, CurrentGamePhase = GamePhase.Entered },
                        bot
                    }
                }
            };

            var turnEnded = bot.EndTurn(30, gameHistory, 0);
            Assert.True(turnEnded);
        }

        [Fact]
        public void LittleRiskBot_CheckIfBotEndsTurn_WhenBotIsFirstWithBigAdvantage()
        {
            var bot = new LittleRiskBotPlayer("Bot") { Score = 810, CurrentGamePhase = GamePhase.Entered };
            var gameHistory = new GameHistory
            {
                History = new List<List<IPlayer>>
                {
                    new List<IPlayer>
                    {
                        new HumanPlayer("Chester") { Score = 200, CurrentGamePhase = GamePhase.Entered },
                        new HumanPlayer("Steven") { Score = 250, CurrentGamePhase = GamePhase.Entered },
                        new HumanPlayer("Jack") { Score = 280, CurrentGamePhase = GamePhase.Entered },
                        new LittleRiskBotPlayer(bot.Name!) { Score = 700, CurrentGamePhase = GamePhase.Entered }
                    },
                    new List<IPlayer>
                    {
                        new HumanPlayer("Chester") { Score = 230, CurrentGamePhase = GamePhase.Entered },
                        new HumanPlayer("Steven") { Score = 280, CurrentGamePhase = GamePhase.Entered },
                        new HumanPlayer("Jack") { Score = 330, CurrentGamePhase = GamePhase.Entered },
                        new LittleRiskBotPlayer(bot.Name!) { Score = 750, CurrentGamePhase = GamePhase.Entered }
                    },
                    new List<IPlayer>
                    {
                        new HumanPlayer("Chester") { Score = 350, CurrentGamePhase = GamePhase.Entered },
                        new HumanPlayer("Steven") { Score = 400, CurrentGamePhase = GamePhase.Entered },
                        new HumanPlayer("Jack") { Score = 600, CurrentGamePhase = GamePhase.Entered },
                        bot
                    }
                }
            };

            var turnEnded = bot.EndTurn(30, gameHistory, 0);
            Assert.False(turnEnded);
        }

        [Fact]
        public void LittleRiskBot_CheckIfBotEndsTurn_WhenBotIsFinishing()
        {
            var bot = new LittleRiskBotPlayer("Bot") { Score = 0, CurrentGamePhase = GamePhase.Finishing };
            var gameHistory = new GameHistory
            {
                History = new List<List<IPlayer>>
                {
                    new List<IPlayer>
                    {
                        new HumanPlayer("Chester") { Score = 0, CurrentGamePhase = GamePhase.NotEntered },
                        new HumanPlayer("Steven") { Score = 0, CurrentGamePhase = GamePhase.NotEntered },
                        new HumanPlayer("Jack") { Score = 0, CurrentGamePhase = GamePhase.NotEntered },
                        bot
                    }
                }
            };
            var turnEnded = bot.EndTurn(100, gameHistory, 0);
            Assert.True(turnEnded);
        }

        [Fact]
        public void LittleRiskBot_CheckIfBotEndsTurn_WhenBotHasntEnteredTheGame()
        {
            var bot = new LittleRiskBotPlayer("Bot") { Score = 0, CurrentGamePhase = GamePhase.NotEntered };
            var gameHistory = new GameHistory
            {
                History = new List<List<IPlayer>>
                {
                    new List<IPlayer>
                    {
                        new HumanPlayer("Chester") { Score = 500, CurrentGamePhase = GamePhase.Entered },
                        new HumanPlayer("Steven") { Score = 600, CurrentGamePhase = GamePhase.Entered },
                        new HumanPlayer("Jack") { Score = 630, CurrentGamePhase = GamePhase.Entered },
                        bot
                    }
                }
            };
            var turnEnded = bot.EndTurn(100, gameHistory, 0);
            Assert.True(turnEnded);
        }
    }
}
