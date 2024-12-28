using DiceGame.Common.Extensions;
using DiceGame.ConsoleVersion.Utilities;
using Microsoft.Extensions.Hosting;

namespace DiceGame.Common.GameLogic
{
    public class Game(IEnumerable<IPlayer> players, GameState gameState, GameResultsCollector resultCollector, ConsoleSettings consoleSettings) : IHostedService
    {
        public GameState GameState { get; set; } = gameState;
        private readonly GameResultsCollector _resultCollector = resultCollector;
        private readonly ConsoleSettings _consoleSettings = consoleSettings;
        public IEnumerable<IPlayer> Players { get; set; } = players;
        private readonly Random _random = new();

        public void StartGame()
        {
            while (!GameState.Leaderboard.Any(x => x.CurrentGamePhase == GamePhase.Finished))
            {
                foreach (var player in Players)
                {
                    var playerState = GameState.Leaderboard.First(x => x.Id == player.Id);
                    ConsoleManagement.DisplayLeaderboard(_consoleSettings.UseConsole, GameState.Leaderboard);
                    var score = PlayersTurn(player, playerState);
                    if (score != 0)
                    {
                        PointingSystem.UpdateScoreboard(GameState, player, score);
                    }

                    if (playerState.CurrentGamePhase == GamePhase.Finished)
                    {
                        Console.WriteLine($"{player.Name} wins! Congratulations!");
                        _resultCollector.AddWinner(player.Name);
                        return;
                    }
                    playerState.MoveNumber++;
                    GameState.History.Add((player.Id, playerState.Score));
                }
            }
        }

        public int PlayersTurn(IPlayer player, PlayerInfo playerInfo)
        {
            bool playerEndendTheirTurn = false;
            int alreadyPointedDice = 0;
            int playerScore = 0;
            int moveNumber = 0;

            while (!playerEndendTheirTurn)
            {
                moveNumber++;
                var playerThrow = _random.Throw(6 - alreadyPointedDice);
                var diceToPoint = PointingSystem.FindDiceToPoint(playerThrow);
                ConsoleManagement.DisplayCurrentPlayerInfo(_consoleSettings.UseConsole, player.Name, playerInfo.CurrentGamePhase, moveNumber, playerThrow);
                if (!diceToPoint.Any())
                {
                    if (alreadyPointedDice != 0)
                    {
                        ConsoleManagement.DisplayNoDiceToPointMessage(_consoleSettings.UseConsole);
                        return 0;
                    }
                    if (playerInfo.CurrentGamePhase != GamePhase.NotEntered)
                    {
                        ConsoleManagement.DisplayNoDiceToPointWithMinusPointsMessage(_consoleSettings.UseConsole);
                        return -50;
                    }
                    ConsoleManagement.DisplayNoDiceToPointAtAllMessage(_consoleSettings.UseConsole);
                    return 0;
                }

                ConsoleManagement.DisplayTheDiceThrown(_consoleSettings.UseConsole, diceToPoint);

                var diceChose = player.ChooseDice(diceToPoint, alreadyPointedDice);
                alreadyPointedDice += diceChose.Sum(x => x.DiceCount);
                var tempscore = PointingSystem.CalculatePointsFromDice(diceChose);
                playerScore += tempscore;
                if ((playerScore >= 100 && playerInfo.CurrentGamePhase == GamePhase.Finishing) || playerScore + playerInfo.Score >= 1000)
                {
                    return playerScore;
                }
                if (CheckIfPlayerCanFinishTheTurn(playerInfo.CurrentGamePhase, playerScore))
                {
                    var gameStateOverview = new GameStateOverview(GameState, player.Id);
                    playerEndendTheirTurn = player.EndTurn(playerScore, gameStateOverview, alreadyPointedDice);
                }
                else
                {
                    ConsoleManagement.DisplayPlayerScore(_consoleSettings.UseConsole, player.Name, playerScore);
                }

            }
            return playerScore;
        }

        private static bool CheckIfPlayerCanFinishTheTurn(GamePhase gamePhase, int score)
        {
            return gamePhase switch
            {
                GamePhase.NotEntered or GamePhase.Finishing => score >= 100,
                GamePhase.Entered => score >= 30,
                _ => throw new ArgumentException("Player with wrong gamephase was present in the game"),
            };
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            StartGame();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
