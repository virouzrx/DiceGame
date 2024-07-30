using DiceGame.Common.Enums;
using DiceGame.Common.GameLogic.ProbabilityHelpers;

namespace DiceGame.Common.Players
{
    public class PlayerFactory
    {
        private readonly ProbabilityHelper _probabilityHelper;

        public PlayerFactory(ProbabilityHelper probabilityHelper)
        {
            _probabilityHelper = probabilityHelper;
        }

        public IPlayer CreatePlayer(PlayerType playerType, string name, BotType? botType = null)
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
                BotType.ModerateRisk => new ModerateRiskBotPlayer(name, _probabilityHelper),
                BotType.Risky => new RiskyBotPlayer(name),
                _ => throw new ArgumentException("Unknown bot type"),
            };
        }
    }
}
