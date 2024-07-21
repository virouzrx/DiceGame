using DiceGameConsoleVersion.Enums;
using DiceGameConsoleVersion.Logic;
namespace DiceGameConsoleVersion.Players
{
    internal class PlayerFactory
    {
        public static IPlayer CreatePlayer(PlayerType playerType, string name, BotType? botType = null)
        {
            if (playerType == PlayerType.Human)
            {
                return new HumanPlayer(name);
            }

            if (botType == null)
            {
                throw new ArgumentException("BotType cannot be null while generating bot player");
            }

            return botType switch
            {
                BotType.NoRisk => new NoRiskBotPlayer(name),
                BotType.LittleRisk => new LittleRiskBotPlayer(name),
                BotType.ModerateRisk => new ModerateRiskBotPlayer(name),
                BotType.Risky => new RiskyBotPlayer(name),
                _ => throw new ArgumentException("Unknown bot type"),
            };
        }
    }
}
