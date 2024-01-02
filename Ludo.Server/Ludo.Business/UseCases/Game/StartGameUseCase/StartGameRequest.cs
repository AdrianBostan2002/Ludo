using Ludo.Domain.DTOs;
using Ludo.Domain.Interfaces;
using Ludo.MediatRPattern.Interfaces;

namespace Ludo.Business.UseCases.Game.CreateGameUseCase
{
    public class StartGameRequest: IRequest<(GameDto, List<IPlayer>)>
    {
        public string ConnectionId { get; set; }
        public int LobbyId { get; set; }
    }
}