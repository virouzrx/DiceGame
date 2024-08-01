using DiceGame.Common.Enums;
using DiceGame.Common.GameLogic;
using DiceGame.Common.GameLogic.ProbabilityHelpers;
using DiceGame.Common.Players;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Diagnostics;

var serviceCollection = new ServiceCollection();
serviceCollection.AddScoped<ProbabilityHelper>();
serviceCollection.AddScoped<PlayerFactory>();
var serviceProvider = serviceCollection.BuildServiceProvider();
var probabilityHelper = serviceProvider.GetService<ProbabilityHelper>();
var playerFactory = new PlayerFactory(probabilityHelper!);
var table = new ConcurrentDictionary<string, int>
{
    ["Risky"] = 0,
    ["NoRisk"] = 0,
    ["ModerateRisk"] = 0,
    ["LittleRisk"] = 0,
};
for (int i = 0; i < 100; i++)
{
    var tasks = new List<Task>();
    for (int j = 0; j < 100; j++)
    {
        tasks.Add(Task.Run(() =>
        {
            var players = new List<IPlayer>
                {
                    playerFactory.CreatePlayer(PlayerType.Bot, "NoRisk", BotType.NoRisk),
                    playerFactory.CreatePlayer(PlayerType.Bot, "LittleRisk", BotType.LittleRisk),
                    playerFactory.CreatePlayer(PlayerType.Bot, "ModerateRisk", BotType.ModerateRisk),
                    playerFactory.CreatePlayer(PlayerType.Bot, "Risky", BotType.Risky),
                };
            var game = new Game(players, j);
            game.StartGame();

            var finishedPlayer = players.FirstOrDefault(x => x.CurrentGamePhase == GamePhase.Finished);
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