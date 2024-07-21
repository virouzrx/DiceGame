using DiceGameConsoleVersion.GameLogic;
using DiceGameConsoleVersion.Logic;
using DiceGameConsoleVersion.Players;

namespace DiceGameUnitTests.BotTests
{
    public class LittleRiskBotPlayerTests
    {
        private LittleRiskBotPlayer botPlayer = new("HumanPlayer1")
        { 
            Score = 100, 
            CurrentGamePhase = DiceGameConsoleVersion.Models.GamePhase.Entered 
        };

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
            var diceChosenByBot = botPlayer.ChooseDice(diceThrown.ToList(), new GameHistory(), alreadyPointedDice).ToList();
            Assert.Equal(expectedChoose, diceChosenByBot);
        }

        //[Theory]
        //[MemberData(nameof(CalculatePointsFromDiceTestData))]
        //public void LittleRiskBot_CheckIfBotEndsTurnCorrect(IEnumerable<PointableDice> diceThrown, IEnumerable<PointableDice> expectedChoose, int alreadyPointedDice)
        //{
        //    var diceChosenByBot = botPlayer.EndTurn(diceThrown.ToList(), new GameHistory(), alreadyPointedDice).ToList();
        //    Assert.Equal(expectedChoose, diceChosenByBot);
        //}
    }
}
