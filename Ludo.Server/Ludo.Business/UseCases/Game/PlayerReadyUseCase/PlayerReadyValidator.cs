using FluentValidation;

namespace Ludo.Business.UseCases.Game.PlayerReadyUseCase
{
    public class PlayerReadyValidator: AbstractValidator<PlayerReadyRequest>
    {
        public PlayerReadyValidator()
        {
            RuleFor(x => x.Username).NotNull().NotEmpty();
            RuleFor(x => x.ConnectionId).NotNull().NotEmpty();
            RuleFor(x => x.LobbyId).ExclusiveBetween(100, 999);
        }
    }
}