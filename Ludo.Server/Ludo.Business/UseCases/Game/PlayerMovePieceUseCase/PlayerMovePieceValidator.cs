using FluentValidation;

namespace Ludo.Business.UseCases.Game.PlayerMovePieceUseCase
{
    public class PlayerMovePieceValidator: AbstractValidator<PlayerMovePieceRequest>
    {
        public PlayerMovePieceValidator()
        {
            RuleFor(x => x.Username).NotNull().NotEmpty();
            RuleFor(x => x.Position).NotEmpty();
            RuleFor(x => x.DiceNumber).ExclusiveBetween(1, 6);
        }
    }
}
