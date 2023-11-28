using Ludo.Business.UseCases.PostTestUseCase;
using Ludo.Business.UseCases.TestUseCase;
using Ludo.Domain.Interfaces;
using Ludo.MediatRPattern.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Ludo.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IHubContext<TestHub> _hub;
        private readonly IMediator _mediatR;
        private readonly IBoardService _boardService;

        public TestController(IHubContext<TestHub> hub, IMediator mediatR, IBoardService boardService)
        {
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));
            _mediatR = mediatR ?? throw new ArgumentNullException(nameof(mediatR));
            _boardService = boardService ?? throw new ArgumentNullException(nameof(boardService));
        }

        [HttpGet]
        public IActionResult Get()
        {
            _hub.Clients.All.SendAsync("NewMessageToOtherClients", "Another client created a new instance");

            return Ok(new {Message = "Successfully sent message to all clients"});
        }

        [HttpGet]
        [Route("MediatR_test")]
        public async Task<IActionResult> GetMediatRAsync()
        {
            TestRequest request = new TestRequest();

            var result = await _mediatR.Send(request);

            return Ok(result);
        }

        [HttpPost]
        [Route("post")]
        public async Task<IActionResult> PostMediatRAsync(PostTestRequest request)
        {
            var result = await _mediatR.Send(request);

            return Ok(result);
        }

        [HttpGet]
        [Route("board")]
        public IActionResult GetBoard()
        {
            var result = _boardService.CreateBoard();

            return Ok(result);
        }
    }
}