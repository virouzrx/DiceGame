using DiceGame.Common.GameLogic;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using Microsoft.Extensions.Hosting;
using DiceGame.ConsoleVersion.Extensions;
using DiceGame.ConsoleVersion.Utilities;

var serviceCollection = new ServiceCollection();
serviceCollection.AddSingleton<GameResultsCollector>();
serviceCollection.RegisterServicesNecessaryForGame(false);

var serviceProvider = serviceCollection.BuildServiceProvider();
var resultCollector = serviceProvider.GetRequiredService<GameResultsCollector>();

Stopwatch stopwatch = new();
stopwatch.Start();

var tasks = new List<Task>();
for (int i = 0; i < 10; i++)
{
    for (int j = 0; j < 10; j++)
    {
        tasks.Add(Task.Run(async () =>
        {
            using var localHost = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton(resultCollector);
                    services.RegisterServicesNecessaryForGame(false);
                    services.AddHostedService<Game>();
                })
                .Build();

            await localHost.StartAsync();
            await localHost.StopAsync();
        }));
    }
}

await Task.WhenAll(tasks);

stopwatch.Stop();
Console.WriteLine($"Finished 100 tasks in {stopwatch.Elapsed}");

var winnerCounts = resultCollector.GetWinners();

Console.WriteLine("Player Win Counts:");
foreach (var winner in winnerCounts)
{
    Console.WriteLine($"{winner.Key}: {winner.Value}");
}
