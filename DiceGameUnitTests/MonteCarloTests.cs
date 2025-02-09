using System.Diagnostics;

namespace DiceGameUnitTests
{
    public class MonteCarloTests
    {
        [Fact]
        public void RunMonteCarloSimulation()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var probabilityWith1 = MonteCarloGenerationHelper.SimulateDiceRolls(1, 50); //50: 0.35  |   40: 0.47  |  30: 0:57
            var probabilityWith2 = MonteCarloGenerationHelper.SimulateDiceRolls(2, 50); //50: 0.39  |   40: 0.49  |  30: 0:54
            var probabilityWith3 = MonteCarloGenerationHelper.SimulateDiceRolls(3, 50); //50: 0.45  |   40: 0.48  |  30: 0:43
            var probabilityWith4 = MonteCarloGenerationHelper.SimulateDiceRolls(4, 50); //50: 0.48  |   40: 0.46  |  30: 0:37
            var probabilityWith5 = MonteCarloGenerationHelper.SimulateDiceRolls(5, 50); //50: 0.49  |   40: 0.43  |  30: 0:55
            var probabilityWith6 = MonteCarloGenerationHelper.SimulateDiceRolls(6, 50); //50: 0.48  |   40: 0.54  |  30: 0.77
            stopwatch.Stop(); 
            var elapsed = stopwatch.ElapsedMilliseconds;
            var x = 0;
        }
    }
}
