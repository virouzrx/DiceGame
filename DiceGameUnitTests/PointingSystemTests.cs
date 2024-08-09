//namespace DiceGameUnitTests
//{
//    public class PointingSystemTests
//    {
//        public static TheoryData<int[], List<PointableDice>> FindFiceToPointTestData =>
//            new TheoryData<int[], List<PointableDice>>()
//            {
//                       { new int[] {1, 1, 5, 4, 2, 3}, new List<PointableDice> { new(1, 2), new(5,1) } },
//                       { new int[] {2, 2, 2, 3, 3, 3}, new List<PointableDice> {new(2, 3), new(3,3) } },
//                       { new int[] {1, 1, 5, 1, 2, 2}, new List<PointableDice> {new(1, 3), new(5,1) } },
//                       { new int[] {1, 1, 1, 1, 1, 1}, new List<PointableDice> {new(1, 6) } },
//            };

//        public static TheoryData<PointableDice[], int> CalculatePointsFromDiceTestData =>
//            new TheoryData<PointableDice[], int>()
//            {
//                         { new PointableDice[] { new(5,1), new(1,3) }, 105 },
//                         { new PointableDice[] { new(5,1), new(1,2) }, 25 },
//                         { new PointableDice[] { new(3,3), new(2,3) }, 50 },
//                         { new PointableDice[] { new(4,5) }, 160 },
//                         { new PointableDice[] { new(1,2), new(5,1), new(4,3) }, 65 },
//            };

//        [Theory]
//        [MemberData(nameof(CalculatePointsFromDiceTestData))]
//        public void CalculatePointsFromDiceTest_ShouldCalculateCorrectScore(PointableDice[] dice, int expectedScore)
//        {
//            var score = PointingSystem.CalculatePointsFromDice(dice);
//            Assert.Equal(expectedScore, score);
//        }

//        [Fact]
//        public void FindDiceToPointTest_ShouldFindNoPointableDice()
//        {
//            var hand = new[] { 2, 2, 3, 3, 4, 4 };
//            var pointableDice = PointingSystem.FindDiceToPoint(hand);
//            Assert.Empty(pointableDice);
//        }

//        [Theory]
//        [MemberData(nameof(FindFiceToPointTestData))]
//        public void FindTheDiceToPointTest_ShouldContainAllPointableDice(int[] hand, IEnumerable<PointableDice> expectedPointableDice)
//        {
//            var pointableDice = PointingSystem.FindDiceToPoint(hand);
//            Assert.Equal(expectedPointableDice.Count(), pointableDice.Count());
//            foreach (var dice in pointableDice)
//            {
//                Assert.Contains(expectedPointableDice, d => d.DiceCount == dice.DiceCount && d.Score == d.Score);
//            }
//        }

//        [Fact]
//        public void UpdateScoreboardTest_PlayerEntersTheGame_AfterThrowing100()
//        {
//            var players = new List<IPlayer>
//            {
//                new HumanPlayer("Player1"),
//                new HumanPlayer ("Player2"),
//                new HumanPlayer ("Player3"),
//                new HumanPlayer ("Player4")
//            };

//            var player1 = players.First();
//            PointingSystem.UpdateScoreboard(players, player1, 100);

//            Assert.True(player1.Score == 100);
//            Assert.True(player1.CurrentGamePhase == GamePhase.Entered);
//        }

//        [Fact]
//        public void UpdateScoreboardTest_Player2OvertakesPlayer1()
//        {
//            var player1 = new HumanPlayer ("Player1");
//            var player2 = new HumanPlayer ("Player2");
//            var player3 = new HumanPlayer ("Player3");
//            var player4 = new HumanPlayer ("Player4");

//            player1.Score = 300;
//            player1.CurrentGamePhase = GamePhase.Entered;

//            player2.Score = 295;
//            player2.CurrentGamePhase = GamePhase.Entered;

//            var players = new List<IPlayer>
//            {
//                player1, player2, player3, player4
//            };

//            PointingSystem.UpdateScoreboard(players, player2, 55);

//            Assert.True(player2.Score == 350);
//            Assert.True(player1.Score == 200);
//        }

//        [Fact]
//        public void UpdateScoreboardTest_Player2DoesntOvertakePlayer1_WhenTheirScoreIsEqual()
//        {
//            var player1 = new HumanPlayer ("Player1");
//            var player2 = new HumanPlayer ("Player2");
//            var player3 = new HumanPlayer ("Player3");
//            var player4 = new HumanPlayer ("Player4");

//            player1.Score = 300;
//            player1.CurrentGamePhase = GamePhase.Entered;

//            player2.Score = 200;
//            player2.CurrentGamePhase = GamePhase.Entered;

//            var players = new List<IPlayer>
//            {
//                player1, player2, player3, player4
//            };

//            PointingSystem.UpdateScoreboard(players, player2, 100);

//            Assert.True(player2.Score == 300);
//            Assert.True(player1.Score == 300);
//            Assert.True(players.IndexOf(player1) == 0);
//        }

//        [Fact]
//        public void UpdateScoreboardTest_Player2DoesntOvertakePlayer1_WhenEnteringTheGame()
//        {
//            var player1 = new HumanPlayer("Player1");
//            var player2 = new HumanPlayer("Player2");
//            var player3 = new HumanPlayer("Player3");
//            var player4 = new HumanPlayer("Player4");

//            player1.Score = 420;
//            player1.CurrentGamePhase = GamePhase.Entered;

//            player2.Score = 105;
//            player2.CurrentGamePhase = GamePhase.Entered;

//            player3.Score = 80;
//            player3.CurrentGamePhase = GamePhase.Entered;

//            var players = new List<IPlayer>
//            {
//                player1, player2, player3, player4
//            };

//            PointingSystem.UpdateScoreboard(players, player4, 105);

//            Assert.True(player4.Score == 105);
//            Assert.True(player2.Score == 105);
//            Assert.True(players.IndexOf(player1) < players.IndexOf(player2));
//        }

//        [Fact]
//        public void UpdateScoreboardTest_PlayerThrewNothingPointable_GetsOvertakenByRestOfThePlayers()
//        {
//            var player1 = new HumanPlayer ("Player1");
//            var player2 = new HumanPlayer ("Player2");
//            var player3 = new HumanPlayer ("Player3");
//            var player4 = new HumanPlayer ("Player4");

//            player1.Score = 420;
//            player1.CurrentGamePhase = GamePhase.Entered;

//            player2.Score = 400;
//            player2.CurrentGamePhase = GamePhase.Entered;

//            player3.Score = 390;
//            player3.CurrentGamePhase = GamePhase.Entered;

//            player4.Score = 375;
//            player4.CurrentGamePhase = GamePhase.Entered;

//            var players = new List<IPlayer>
//            {
//                player1, player2, player3, player4
//            };

//            players = PointingSystem.UpdateScoreboard(players, player1, -50);

//            Assert.Equal(70, player1.Score);
//            Assert.Equal(3, players.IndexOf(player1));
//        }

//        [Fact]
//        public void UpdateScoreboardTest_Player3Overtakes2Players()
//        {
//            var player1 = new HumanPlayer("Player1");
//            var player2 = new HumanPlayer("Player2");
//            var player3 = new HumanPlayer("Player3");
//            var player4 = new HumanPlayer("Player4");

//            player1.Score = 420;
//            player1.CurrentGamePhase = GamePhase.Entered;

//            player2.Score = 400;
//            player2.CurrentGamePhase = GamePhase.Entered;

//            player3.Score = 310;
//            player3.CurrentGamePhase = GamePhase.Entered;

//            player4.Score = 0;
//            player4.CurrentGamePhase = GamePhase.Entered;

//            var players = new List<IPlayer>
//            {
//                player1, player2, player3, player4
//            };

//            players = PointingSystem.UpdateScoreboard(players, player3, 115);

//            Assert.Equal(1, players.IndexOf(player1));
//            Assert.Equal(2, players.IndexOf(player2));
//        }

//        [Fact]
//        public void UpdateScoreboardTest_PlayerLosesPoints_ShouldChangeGamePhaseFromFinishingToEntered()
//        {
//            var player1 = new HumanPlayer("Player1");
//            var player2 = new HumanPlayer("Player2");
//            var player3 = new HumanPlayer("Player3");
//            var player4 = new HumanPlayer("Player4");

//            player1.Score = 900;
//            player1.CurrentGamePhase = GamePhase.Finishing;

//            player2.Score = 400;
//            player2.CurrentGamePhase = GamePhase.Entered;

//            player3.Score = 310;
//            player3.CurrentGamePhase = GamePhase.Entered;

//            player4.Score = 0;
//            player4.CurrentGamePhase = GamePhase.Entered;

//            var players = new List<IPlayer>
//            {
//                player1, player2, player3, player4
//            };

//           PointingSystem.UpdateScoreboard(players, player1, -50);

//            Assert.Equal(GamePhase.Entered, player1.CurrentGamePhase);
//        }
//    }
//}