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
    }
}