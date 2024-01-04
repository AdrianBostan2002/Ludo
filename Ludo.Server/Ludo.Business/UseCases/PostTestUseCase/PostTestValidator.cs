using FluentValidation;

namespace Ludo.Business.UseCases.PostTestUseCase
{
    public class PostTestValidator : AbstractValidator<PostTestRequest>
    {
        public PostTestValidator()
        {
            RuleFor(x => x.Person).NotEmpty().WithErrorCode("NotEmpty");
        }
    }
}