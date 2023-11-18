using FluentValidation;

namespace Ludo.Business.UseCases.JoinLobbyUseCase
{
    public class JoinLobbyValidator : AbstractValidator<JoinLobbyRequest>
    {
        public JoinLobbyValidator()
        {
            RuleFor(x=> x.Username).NotNull().NotEmpty().WithErrorCode("Username shouldn't be null or empty");
        }
    }
}