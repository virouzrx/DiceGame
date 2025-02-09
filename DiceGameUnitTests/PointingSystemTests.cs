using DiceGame.Common.Players.Bots;
using DiceGame.UnitTests.Helpers;

namespace DiceGameUnitTests
{
    public class PointingSystemTests
    {
        public static TheoryData<int[], List<PointableDice>> FindFiceToPointTestData =>
            new TheoryData<int[], List<PointableDice>>()
            {
                       { new int[] {1, 1, 5, 4, 2, 3}, new List<PointableDice> { new(1, 2), new(5,1) } },
                       { new int[] {2, 2, 2, 3, 3, 3}, new List<PointableDice> {new(2, 3), new(3,3) } },
                       { new int[] {1, 1, 5, 1, 2, 2}, new List<PointableDice> {new(1, 3), new(5,1) } },
                       { new int[] {1, 1, 1, 1, 1, 1}, new List<PointableDice> {new(1, 6) } },
            };

        public static TheoryData<PointableDice[], int> CalculatePointsFromDiceTestData =>
            new TheoryData<PointableDice[], int>()
            {
                         { new PointableDice[] { new(5,1), new(1,3) }, 105 },
                         { new PointableDice[] { new(5,1), new(1,2) }, 25 },
                         { new PointableDice[] { new(3,3), new(2,3) }, 50 },
                         { new PointableDice[] { new(4,5) }, 160 },
                         { new PointableDice[] { new(1,2), new(5,1), new(4,3) }, 65 },
            };

        [Theory]
        [MemberData(nameof(CalculatePointsFromDiceTestData))]
        public void CalculatePointsFromDiceTest_ShouldCalculateCorrectScore(PointableDice[] dice, int expectedScore)
        {
            var score = PointingSystem.CalculatePointsFromDice(dice);
            Assert.Equal(expectedScore, score);
        }

        [Fact]
        public void FindDiceToPointTest_ShouldFindNoPointableDice()
        {
            var hand = new[] { 2, 2, 3, 3, 4, 4 };
            var pointableDice = PointingSystem.FindDiceToPoint(hand);
            Assert.Empty(pointableDice);
        }

        [Theory]
        [MemberData(nameof(FindFiceToPointTestData))]
        public void FindTheDiceToPointTest_ShouldContainAllPointableDice(int[] hand, IEnumerable<PointableDice> expectedPointableDice)
        {
            var pointableDice = PointingSystem.FindDiceToPoint(hand);
            Assert.Equal(expectedPointableDice.Count(), pointableDice.Count());
            foreach (var dice in pointableDice)
            {
                Assert.Contains(expectedPointableDice, d => d.DiceCount == dice.DiceCount && d.Score == d.Score);
            }
        }

        [Fact]
        public void UpdateScoreboardTest_PlayerEntersTheGame_AfterThrowing100()
        {
            var testPlayer = new NoRiskBotPlayer("Bot");
            var gameState = new GameStateBuilder()
                .WithPlayer("Player1", GamePhase.NotEntered, 0)
                .WithPlayerToTest(testPlayer, GamePhase.NotEntered, 0)
                .BuildGameState();

            PointingSystem.UpdateScoreboard(gameState, testPlayer, 100);
            Assert.True(gameState.Leaderboard.First(x => x.Id == testPlayer.Id).CurrentGamePhase == GamePhase.Entered);
        }

        [Fact]
        public void UpdateScoreboardTest_Player2OvertakesPlayer1()
        {
            var testPlayer = new NoRiskBotPlayer("Bot");
            var gameState = new GameStateBuilder()
                .WithPlayer("Player1", GamePhase.Entered, 300)
                .WithPlayerToTest(testPlayer, GamePhase.Entered, 295)
                .BuildGameState();

            PointingSystem.UpdateScoreboard(gameState, testPlayer, 55);

            var player1 = gameState.Leaderboard.First(x => x.Name == "Player1");

            Assert.True(player1.Score == 200);
            Assert.True(gameState.Leaderboard.First(x => x.Id == testPlayer.Id).Score == 350);
            Assert.True(gameState.PlayerIndex(testPlayer.Id) == 0);
        }

        [Fact]
        public void UpdateScoreboardTest_Player2DoesntOvertakePlayer1_WhenTheirScoreIsEqual()
        {
            var testPlayer = new NoRiskBotPlayer("Bot");
            var gameState = new GameStateBuilder()
                .WithPlayer("Player1", GamePhase.Entered, 300)
                .WithPlayerToTest(testPlayer, GamePhase.Entered, 200)
                .BuildGameState();

            PointingSystem.UpdateScoreboard(gameState, testPlayer, 100);

            var player1 = gameState.Leaderboard.First(x => x.Name == "Player1");

            Assert.True(player1.Score == 300);
            Assert.True(gameState.Leaderboard.First(x => x.Id == testPlayer.Id).Score == 300);
            Assert.True(gameState.PlayerIndex(player1.Id) == 0);
        }

        [Fact]
        public void UpdateScoreboardTest_Player2DoesntOvertakePlayer1_WhenEnteringTheGame()
        {
            var testPlayer = new NoRiskBotPlayer("Bot");
            var gameState = new GameStateBuilder()
                .WithPlayer("Player1", GamePhase.Entered, 400)
                .WithPlayer("Player2", GamePhase.Entered, 105)
                .WithPlayer("Player3", GamePhase.Entered, 80)
                .WithPlayerToTest(testPlayer, GamePhase.NotEntered, 0)
                .BuildGameState();

            PointingSystem.UpdateScoreboard(gameState, testPlayer, 105);

            var player2 = gameState.Leaderboard.First(x => x.Name == "Player2");
            Assert.True(gameState.Leaderboard.First(x => x.Id == testPlayer.Id).Score == 105);
            Assert.True(player2.Score == 105);
            Assert.True(gameState.PlayerIndex(player2.Id) < gameState.PlayerIndex(testPlayer.Id));
        }

        [Fact]
        public void UpdateScoreboardTest_PlayerThrewNothingPointable_GetsOvertakenByRestOfThePlayers()
        {
            var testPlayer = new NoRiskBotPlayer("Bot");
            var gameState = new GameStateBuilder()
                .WithPlayer("Player1", GamePhase.Entered, 400)
                .WithPlayer("Player2", GamePhase.Entered, 390)
                .WithPlayer("Player4", GamePhase.Entered, 375)
                .WithPlayerToTest(testPlayer, GamePhase.Entered, 420)
                .BuildGameState();

            PointingSystem.UpdateScoreboard(gameState, testPlayer, -50);

            Assert.Equal(70, gameState.Leaderboard.First(x => x.Id == testPlayer.Id).Score);
            Assert.Equal(3, gameState.PlayerIndex(testPlayer.Id));
        }

        [Fact]
        public void UpdateScoreboardTest_Player3Overtakes2Players()
        {
            var testPlayer = new NoRiskBotPlayer("Bot");
            var gameState = new GameStateBuilder()
                .WithPlayer("Player1", GamePhase.Entered, 420)
                .WithPlayer("Player2", GamePhase.Entered, 400)
                .WithPlayer("Player4", GamePhase.NotEntered, 0)
                .WithPlayerToTest(testPlayer, GamePhase.Entered, 310)
                .BuildGameState();


            PointingSystem.UpdateScoreboard(gameState, testPlayer, 115);

            var player1 = gameState.Leaderboard.First(x => x.Name == "Player1");
            var player2 = gameState.Leaderboard.First(x => x.Name == "Player2");
            Assert.Equal(1, gameState.PlayerIndex(player1.Id));
            Assert.Equal(2, gameState.PlayerIndex(player2.Id));
        }

        [Fact]
        public void UpdateScoreboardTest_PlayerLosesPoints_ShouldChangeGamePhaseFromFinishingToEntered()
        {
            var testPlayer = new NoRiskBotPlayer("Bot");
            var gameState = new GameStateBuilder()
                .WithPlayer("Player1", GamePhase.Entered, 500)
                .WithPlayer("Player2", GamePhase.Entered, 400)
                .WithPlayer("Player3", GamePhase.Entered, 310)
                .WithPlayerToTest(testPlayer, GamePhase.Finishing, 910)
                .BuildGameState();


            PointingSystem.UpdateScoreboard(gameState, testPlayer, -50);

            Assert.Equal(GamePhase.Entered, gameState.Leaderboard.First(x => x.Id == testPlayer.Id).CurrentGamePhase);
        }
    }
}