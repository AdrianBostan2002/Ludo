using Ludo.Business.UseCases.CreateLobbyUseCase;
using Ludo.Business.UseCases.JoinLobbyUseCase;
using Ludo.MediatRPattern.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ludo.Server.Controllers
{
    public class LobbyController : Controller
    {
        private readonly IMediator _mediatR;

        public LobbyController(IMediator mediatR)
        {
            _mediatR = mediatR ?? throw new ArgumentNullException(nameof(mediatR));
        }

        [HttpPost]
        [Route("new")]
        public IActionResult CreateLobby(CreateLobbyRequest request)
        {
            _mediatR.Send(request);

            return Ok(request);
        }

        public IActionResult JoinLobby(JoinLobbyRequest request)
        {
            var result = _mediatR.Send(request);

            return Ok(result);
        }
    }
}