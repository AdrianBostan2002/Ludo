using FluentValidation;

namespace Ludo.Business.UseCases.Game.StartGamePreprocessing
{
    public class StartGameProcessingValidator : AbstractValidator<StartGamePreprocessingRequest>
    {
        public StartGameProcessingValidator()
        {
            RuleFor(x => x.Username).NotNull().NotEmpty();
            RuleFor(x => x.ConnectionId).NotNull().NotEmpty();
            RuleFor(x => x.LobbyId).ExclusiveBetween(100, 999);
        }
    }
}