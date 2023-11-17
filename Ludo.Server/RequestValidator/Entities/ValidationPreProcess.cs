using FluentValidation;
using FluentValidation.Results;
using Ludo.RequestValidator.Interfaces;

namespace Ludo.RequestValidator.Entities
{
    public class ValidationPreProcess<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationPreProcess(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators ?? throw new ArgumentNullException(nameof(validators));
        }

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return Task.CompletedTask;
            }

            ValidationContext<TRequest> context = new(request);

            ValidationFailure[] erros = _validators
                .Select(x => x.Validate(context))
                .SelectMany(x => x.Errors)
                .Where(x => x != null)
                .Distinct()
                .ToArray();

            return erros.Any() ? throw new ValidationException(erros) : Task.CompletedTask;
        }
    }
}