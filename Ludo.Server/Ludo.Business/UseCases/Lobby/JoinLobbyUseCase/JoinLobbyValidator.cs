using FluentValidation;

namespace Ludo.Business.UseCases.Lobby.JoinLobbyUseCase
{
    public class JoinLobbyValidator: AbstractValidator<JoinLobbyRequest>
    {
        public JoinLobbyValidator()
        {
            RuleFor(x => x.Username).NotNull().NotEmpty();
            RuleFor(x => x.ConnectionId).NotNull().NotEmpty();
            RuleFor(x => x.LobbyId).ExclusiveBetween(100, 999);
        }
    }
}
