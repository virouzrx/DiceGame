using DiceGame.Common.Players.Bots;

namespace DiceGame.Common.Players
{
    public class PlayerFactory(ProbabilityHelper probabilityHelper)
    {
        private readonly ProbabilityHelper _probabilityHelper = probabilityHelper;

        public IPlayer CreatePlayer(string name, BotType? botType = null)
        {
            if (botType == null)
            {
                throw new ArgumentException("BotType cannot be null while generating bot player");
            }

            return botType switch
            {
                BotType.NoRisk => new NoRiskBotPlayer(),
                BotType.LittleRisk => new LittleRiskBotPlayer(),
                BotType.ModerateRisk => new ModerateRiskBotPlayer(_probabilityHelper),
                BotType.Risky => new RiskyBotPlayer(_probabilityHelper),
                _ => throw new ArgumentException("Unknown bot type"),
            };
        }
    }
}
