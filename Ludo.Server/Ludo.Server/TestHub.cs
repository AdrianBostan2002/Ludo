using Microsoft.AspNetCore.SignalR;

namespace Ludo.Server
{
    public class TestHub: Hub
    {
        public async Task Test(string[] yourMessage) =>
        await Clients.All.SendAsync("your message", yourMessage);
    }
}
