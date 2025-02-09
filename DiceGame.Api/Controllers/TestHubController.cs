using DiceGame.Api.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace DiceGame.Api.Controllers
{
    public class TestHubController(IHubContext<TestHub> hubContext) : Controller
    {
        private readonly IHubContext<TestHub> _hubContext = hubContext;

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateNumbers()
        {
            var randomNumbers = new int[6];
            var random = new Random();
            for (int i = 0; i < 6; i++)
            {
                randomNumbers[i] = random.Next(1, 100);
            }

            await _hubContext.Clients.All.SendAsync("ReceiveRandomNumbers", randomNumbers);

            return Ok(randomNumbers);
        }
    }
}
