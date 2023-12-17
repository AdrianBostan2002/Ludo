using FluentValidation;

namespace Ludo.Business.UseCases.Game.RollDiceUseCase
{
    public class RollDiceValidator : AbstractValidator<RollDiceRequest>
    {
        public RollDiceValidator()
        {
            RuleFor(x => x.ConnectionId).NotNull().NotEmpty();
            RuleFor(x => x.GameId).ExclusiveBetween(100, 999);
        }
    }
}