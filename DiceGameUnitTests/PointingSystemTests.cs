using DiceGameConsoleVersion.GameLogic;
using DiceGameConsoleVersion.Logic;
using DiceGameConsoleVersion.Models;

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
                Assert.Contains(expectedPointableDice, d => d.Count == dice.Count && d.Score == d.Score);
            }
        }

        [Fact]
        public void UpdateScoreboardTest_PlayerEntersTheGame_AfterThrowing100()
        {
            var players = new List<IPlayer>
            {
                new HumanPlayer() { Name = "Player1", Score = 0, PlayerType = PlayerType.Real },
                new HumanPlayer() { Name = "Player2", Score = 0, PlayerType = PlayerType.Real },
                new HumanPlayer() { Name = "Player3", Score = 0, PlayerType = PlayerType.Real },
                new HumanPlayer() { Name = "Player4", Score = 0, PlayerType = PlayerType.Real },
            };

            var player1 = players.First();
            PointingSystem.UpdateScoreboard(players, player1, 100);

            Assert.True(player1.Score == 100);
            Assert.True(player1.CurrentGamePhase == GamePhase.Entered);
        }

        [Fact]
        public void UpdateScoreboardTest_Player2OvertakesPlayer1()
        {
            var players = new List<IPlayer>
            {
                new HumanPlayer() { Name = "Player1", Score = 300, CurrentGamePhase = GamePhase.Entered, PlayerType = PlayerType.Real },
                new HumanPlayer() { Name = "Player2", Score = 295, CurrentGamePhase = GamePhase.Entered, PlayerType = PlayerType.Real },
                new HumanPlayer() { Name = "Player3", Score = 0, PlayerType = PlayerType.Real },
                new HumanPlayer() { Name = "Player4", Score = 0, PlayerType = PlayerType.Real },
            };

            var player2 = players.First(p => p.Name == "Player2");
            var player1 = players.First(p => p.Name == "Player1");
            PointingSystem.UpdateScoreboard(players, player2, 55);

            Assert.True(player2.Score == 350);
            Assert.True(player1.Score == 200);
        }

        [Fact]
        public void UpdateScoreboardTest_Player2DoesntOvertakePlayer1_WhenTheirScoreIsEqual()
        {
            var players = new List<IPlayer>
            {
                new HumanPlayer() { Name = "Player1", Score = 300, CurrentGamePhase = GamePhase.Entered, PlayerType = PlayerType.Real },
                new HumanPlayer() { Name = "Player2", Score = 200, CurrentGamePhase = GamePhase.Entered, PlayerType = PlayerType.Real },
                new HumanPlayer() { Name = "Player3", Score = 0, PlayerType = PlayerType.Real },
                new HumanPlayer() { Name = "Player4", Score = 0, PlayerType = PlayerType.Real },
            };

            var player2 = players.First(p => p.Name == "Player2");
            var player1 = players.First(p => p.Name == "Player1");
            PointingSystem.UpdateScoreboard(players, player2, 100);

            Assert.True(player2.Score == 300);
            Assert.True(player1.Score == 300);
            Assert.True(players.IndexOf(player1) == 0);
        }

        [Fact]
        public void UpdateScoreboardTest_Player2DoesntOvertakePlayer1_WhenEnteringTheGame()
        {
            var players = new List<IPlayer>
            {
                new HumanPlayer() { Name = "Player1", Score = 420, CurrentGamePhase = GamePhase.Entered, PlayerType = PlayerType.Real },
                new HumanPlayer() { Name = "Player2", Score = 105, CurrentGamePhase = GamePhase.Entered, PlayerType = PlayerType.Real },
                new HumanPlayer() { Name = "Player3", Score = 80, CurrentGamePhase = GamePhase.Entered, PlayerType = PlayerType.Real },
                new HumanPlayer() { Name = "Player4", Score = 0, PlayerType = PlayerType.Real },
            };

            var player2 = players.First(p => p.Name == "Player4");
            var player1 = players.First(p => p.Name == "Player2");
            PointingSystem.UpdateScoreboard(players, player2, 105);

            Assert.True(player2.Score == 105);
            Assert.True(player1.Score == 105);
            Assert.True(players.IndexOf(player1) < players.IndexOf(player2));
        }

        [Fact]
        public void UpdateScoreboardTest_PlayerThrewNothingPointable_GetsOvertakenByRestOfThePlayers()
        {
            var players = new List<IPlayer>
            {
                new HumanPlayer() { Name = "Player1", Score = 420, CurrentGamePhase = GamePhase.Entered, PlayerType = PlayerType.Real },
                new HumanPlayer() { Name = "Player2", Score = 400, CurrentGamePhase = GamePhase.Entered, PlayerType = PlayerType.Real },
                new HumanPlayer() { Name = "Player3", Score = 390, CurrentGamePhase = GamePhase.Entered, PlayerType = PlayerType.Real },
                new HumanPlayer() { Name = "Player4", Score = 375, CurrentGamePhase = GamePhase.Entered, PlayerType = PlayerType.Real },
            };

            var player1 = players.First(p => p.Name == "Player1");
            players = PointingSystem.UpdateScoreboard(players, player1, -50);

            Assert.Equal(70, player1.Score);
            Assert.Equal(3, players.IndexOf(player1));
        }

        [Fact]
        public void UpdateScoreboardTest_Player3Overtakes2Players()
        {
            var players = new List<IPlayer>
            {
                new HumanPlayer() { Name = "Player1", Score = 420, CurrentGamePhase = GamePhase.Entered, PlayerType = PlayerType.Real },
                new HumanPlayer() { Name = "Player2", Score = 400, CurrentGamePhase = GamePhase.Entered, PlayerType = PlayerType.Real },
                new HumanPlayer() { Name = "Player3", Score = 310, CurrentGamePhase = GamePhase.Entered, PlayerType = PlayerType.Real },
                new HumanPlayer() { Name = "Player4", Score = 0, CurrentGamePhase = GamePhase.NotEntered, PlayerType = PlayerType.Real },
            };

            var player3 = players.First(p => p.Name == "Player3");
            var player2 = players.First(p => p.Name == "Player2");
            var player1 = players.First(p => p.Name == "Player1");
            players = PointingSystem.UpdateScoreboard(players, player3, 115);

            Assert.Equal(1, players.IndexOf(player1));
            Assert.Equal(2, players.IndexOf(player2));
        }
    }
}