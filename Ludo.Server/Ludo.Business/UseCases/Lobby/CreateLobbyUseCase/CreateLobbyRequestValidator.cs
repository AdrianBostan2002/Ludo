using FluentValidation;

namespace Ludo.Business.UseCases.Lobby.CreateLobby
{
    public class CreateLobbyRequestValidator : AbstractValidator<CreateLobbyRequest>
    {
        public CreateLobbyRequestValidator()
        {
            RuleFor(x => x.Username).NotNull().NotEmpty();
            RuleFor(x => x.ConnectionId).NotNull().NotEmpty();
        }
    }
}