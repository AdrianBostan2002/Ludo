using Ludo.MediatRPattern.Interfaces;

namespace Ludo.Business.UseCases.JoinLobbyUseCase
{
    public class JoinLobbyRequest : IRequest<string>
    {
        public string Username { get; set; }
    }
}