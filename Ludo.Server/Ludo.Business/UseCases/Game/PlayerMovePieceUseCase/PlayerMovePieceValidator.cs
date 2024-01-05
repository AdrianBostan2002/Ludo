using FluentValidation;

namespace Ludo.Business.UseCases.Game.PlayerMovePieceUseCase
{
    public class PlayerMovePieceValidator : AbstractValidator<PlayerMovePieceRequest>
    {
        public PlayerMovePieceValidator()
        {
            RuleFor(x => x.Username).NotNull().NotEmpty();
            RuleFor(x => x.GameId).ExclusiveBetween(99, 1000);
            RuleFor(x => x.Piece).NotNull().NotEmpty();
            RuleFor(x => x.Piece.PreviousPosition).NotEmpty();
            RuleFor(x => x.Piece.Color).NotEmpty();
            RuleFor(x => x.DiceNumber).ExclusiveBetween(0, 7);
            RuleFor(x => x.ConnectionId).NotNull().NotEmpty();
        }
    }
}
