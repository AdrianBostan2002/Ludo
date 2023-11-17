using Ludo.RequestValidator.Interfaces;

namespace Ludo.RequestValidator.Entities
{
    public class RequestValidator : IRequestValidator
    {
        private readonly IServiceProvider serviceProvider;

        public RequestValidator(IServiceProvider service)
        {
            this.serviceProvider = service ?? throw new ArgumentNullException(nameof(service));
        }

        public void ValidateRequest<TRequest>(TRequest request)
        {
            var requestType = request.GetType();

            var validator = typeof(IRequestPreProcessor<>).MakeGenericType(requestType);

            var validatorType = serviceProvider.GetService(validator);

            if (validatorType != null)
            {
                var method = validatorType.GetType().GetMethod("Process");

                if (method != null)
                {
                    method.Invoke(validatorType, new object[] { request, new CancellationToken() });
                }
            }
        }
    }
}