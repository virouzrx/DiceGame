using DiceGame.Common.Players.Bots;

namespace DiceGame.Common.Players
{
    public class PlayerFactory(ProbabilityHelper probabilityHelper)
    {
        private readonly ProbabilityHelper _probabilityHelper = probabilityHelper;

        public IPlayer CreatePlayer(PlayerType playerType, string name, BotType? botType = null)
        {
            //if (playerType == PlayerType.Human)
            //{
            //    return new HumanPlayer(name);
            //}

            if (botType == null)
            {
                throw new ArgumentException("BotType cannot be null while generating bot player");
            }

            return botType switch
            {
                BotType.NoRisk => new NoRiskBotPlayer(name),
                BotType.LittleRisk => new LittleRiskBotPlayer(name),
                BotType.ModerateRisk => new ModerateRiskBotPlayer(name, _probabilityHelper),
                BotType.Risky => new RiskyBotPlayer(name, _probabilityHelper),
                _ => throw new ArgumentException("Unknown bot type"),
            };
        }
    }
}
