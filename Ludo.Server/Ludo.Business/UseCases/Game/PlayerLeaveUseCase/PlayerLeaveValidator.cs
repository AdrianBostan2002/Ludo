using FluentValidation;

namespace Ludo.Business.UseCases.Game.PlayerLeaveUseCase
{
    public class PlayerLeaveValidator : AbstractValidator<PlayerLeaveRequest>
    {
        public PlayerLeaveValidator()
        {
            RuleFor(x => x.Username).NotNull().NotEmpty();
            RuleFor(x => x.ConnectionId).NotNull().NotEmpty();
            RuleFor(x => x.LobbyId).ExclusiveBetween(100, 999);
        }
    }
}