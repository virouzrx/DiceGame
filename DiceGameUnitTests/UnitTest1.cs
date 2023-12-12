using DiceGameConsoleVersion;
using DiceGameConsoleVersion.Models;

namespace DiceGameUnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Dictionary<int, int> SingleDicePoints = new()
            {
                { 1, 10 },
                { 5, 5 }
            };
            PointingSystem ps = new(SingleDicePoints);

            List<Player> players = new List<Player>
            {
                new Player { Score = 180, Name = "Darek", CurrentPlayerPhase = GamePhase.Entered },
                new Player { Score = 130, Name = "Marek", CurrentPlayerPhase = GamePhase.Entered },
                new Player { Score = 130, Name = "Czarek", CurrentPlayerPhase = GamePhase.Entered },
                new Player { Score = 100, Name = "Jarek", CurrentPlayerPhase = GamePhase.Entered },
            };
            
            var result = ps.UpdateScoreboard(players, "Jarek", 50);
            Assert.True(result[1].Name == "Jarek");
            Assert.Equal(30, result[2].Score);
            Assert.Equal(30, result[3].Score);
        }
    }
}