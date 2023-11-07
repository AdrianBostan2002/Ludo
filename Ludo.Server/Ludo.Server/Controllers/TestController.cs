using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Ludo.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IHubContext<TestHub> _hub;

        public TestController(IHubContext<TestHub> hub)
        {
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));
        }

        [HttpGet]
        public IActionResult Get()
        {
            _hub.Clients.All.SendAsync("NewMessageToOtherClients", "Another client created a new instance");

            return Ok(new {Message = "Successfully sent message to all clients"});
        }
    }
}
