using DiceGame.Common.Enums;
using DiceGame.Common.GameLogic;
using DiceGame.Common.GameLogic.ProbabilityHelpers;
using DiceGame.Common.Players;
using DiceGame.Common.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Diagnostics;

var serviceCollection = new ServiceCollection();
serviceCollection.RegisterServicesNecessaryForGame();

var serviceProvider = serviceCollection.BuildServiceProvider();

var probabilityHelper = serviceProvider.GetService<ProbabilityHelper>();
var playerFactory = serviceProvider.GetService<PlayerFactory>()!;
var gameHistory = serviceProvider.GetService<GameHistory>()!;
var gameState = serviceProvider.GetService<GameState>()!;
var players = serviceProvider.GetService<IEnumerable<IPlayer>>()!.ToList();
var table = new ConcurrentDictionary<string, int>
{
    ["Risky"] = 0,
    ["NoRisk"] = 0,
    ["ModerateRisk"] = 0,
    ["LittleRisk"] = 0,
};
for (int i = 0; i < 10; i++)
{
    var tasks = new List<Task>();
    for (int j = 0; j < 10; j++)
    {
        tasks.Add(Task.Run(() =>
        {
            var game = new Game(players, gameState);
            game.StartGame();

            var finishedPlayer = gameState.Leaderboard.FirstOrDefault(x => x.CurrentGamePhase == GamePhase.Finished);
            if (finishedPlayer != null)
            {
                table.AddOrUpdate(finishedPlayer.Name!, 1, (key, oldValue) => oldValue + 1);
            }
        }));
    }
    Stopwatch stopwatch = new Stopwatch();
    stopwatch.Start();
    await Task.WhenAll(tasks);
    stopwatch.Stop();
    Console.WriteLine($"Finished 100 tasks in {stopwatch.Elapsed}");
}

foreach (var item in table)
{
    Console.WriteLine($"{item.Key}: {item.Value}");
}