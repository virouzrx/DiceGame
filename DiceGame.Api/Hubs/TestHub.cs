using Microsoft.AspNetCore.SignalR;

namespace DiceGame.Api.Hubs
{
    public class TestHub : Hub
    {
        public async Task BroadcastRandomNumbers(int[] numbers)
        {
            await Clients.All.SendAsync("ReceiveRandomNumbers", numbers);
        }
    }
}
