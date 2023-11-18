using FluentValidation;

namespace Ludo.Business.UseCases.CreateLobbyUseCase
{
    public class CreateLobbyValidator: AbstractValidator<CreateLobbyRequest>
    {
        public CreateLobbyValidator()
        {
            RuleFor(x => x.Username).NotEmpty().NotNull().WithErrorCode("Username shouldn't be empty or null");
        }
    }
}