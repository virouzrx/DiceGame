using DiceGame.Common.Extensions;

namespace DiceGame.Common.GameLogic
{
    public class Game(List<IPlayer> players, GameState gameState)
    {
        public GameState GameState { get; set; } = gameState;
        public List<IPlayer> Players { get; set; } = players;
        private readonly Random _random = new();

        public void StartGame()
        {
            while (!GameState.Leaderboard.Any(x => x.CurrentGamePhase == GamePhase.Finished))
            {
                foreach (var player in Players)
                {
                    var playerState = GameState.Leaderboard.First(x => x.Id == player.Id);
                    ConsoleManagement.DisplayLeaderboard(GameState.Leaderboard);
                    var score = PlayersTurn(player, playerState);
                    if (score != 0)
                    {
                        PointingSystem.UpdateScoreboard(GameState, player, score);
                    }

                    if (playerState.CurrentGamePhase == GamePhase.Finished)
                    {
                        Console.WriteLine($"{player.Name} wins! Congratulations!");
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
                Console.WriteLine($"Player: {player.Name}, Phase: {playerInfo.CurrentGamePhase}, {moveNumber} throw: {string.Join(", ", playerThrow)}");
                Console.WriteLine("------------------------");
                if (!diceToPoint.Any())
                {
                    if (alreadyPointedDice != 0)
                    {
                        Console.WriteLine("No dice to point was thrown.\n");
                        return 0;
                    }
                    if (playerInfo.CurrentGamePhase != GamePhase.NotEntered)
                    {
                        Console.WriteLine("No dice to point was thrown by player. -50 points.");
                        return -50;
                    }
                    Console.WriteLine("No dice to point was thrown by player.");
                    return 0;
                }

                ConsoleManagement.DisplayTheDiceThrown(diceToPoint);

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
                    Console.WriteLine($"{player.Name}'s score is {playerScore}");
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
    }
}
