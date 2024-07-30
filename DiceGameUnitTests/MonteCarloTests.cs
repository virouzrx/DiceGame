using DiceGameConsoleVersion.GameLogic.ProbabilityHelpers;
using System.Diagnostics;

namespace DiceGameUnitTests
{
    public class MonteCarloTests
    {
        [Fact]
        public async Task RunMonteCarloSimulation()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var probability = MonteCarloGenerationHelper.SimulateDiceRolls(4, 50);
            stopwatch.Stop(); 
            var x = 0;
        }
    }
}
