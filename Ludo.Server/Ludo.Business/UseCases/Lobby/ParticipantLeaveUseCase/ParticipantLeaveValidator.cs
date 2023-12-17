using FluentValidation;
using Ludo.Business.UseCases.Lobby.ParticipantLeave;

namespace Ludo.Business.UseCases.Lobby.ParticipantLeaveUseCase
{
    public class ParticipantLeaveValidator: AbstractValidator<ParticipantLeaveRequest>
    {
        public ParticipantLeaveValidator()
        {
            RuleFor(x => x.Username).NotNull().NotEmpty();
            RuleFor(x => x.LobbyId).ExclusiveBetween(100, 999);
        }
    }
}