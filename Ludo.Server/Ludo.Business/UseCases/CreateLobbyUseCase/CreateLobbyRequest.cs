using Ludo.MediatRPattern.Interfaces;

namespace Ludo.Business.UseCases.CreateLobbyUseCase
{
    public class CreateLobbyRequest: IRequest<string>
    {
        public string Username { get; set; }
    }
}