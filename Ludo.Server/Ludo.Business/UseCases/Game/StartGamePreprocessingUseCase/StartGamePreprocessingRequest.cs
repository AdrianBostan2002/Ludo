using Ludo.MediatRPattern.Interfaces;

namespace Ludo.Business.UseCases.Game.StartGamePreprocessing
{
    public class StartGamePreprocessingRequest: IRequest<List<string>>
    {
        public int LobbyId { get; set; }
        public string Username { get; set; }
        public string ConnectionId { get; set; }
    }
}