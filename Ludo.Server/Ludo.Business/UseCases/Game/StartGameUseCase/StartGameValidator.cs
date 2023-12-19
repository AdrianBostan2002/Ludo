using FluentValidation;

namespace Ludo.Business.UseCases.Game.CreateGameUseCase
{
    public class StartGameValidator: AbstractValidator<StartGameRequest>
    {
        public StartGameValidator()
        {
            RuleFor(x => x.ConnectionId).NotNull().NotEmpty();
            RuleFor(x => x.LobbyId).ExclusiveBetween(100, 999);
        }
    }
}