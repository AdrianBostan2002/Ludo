using Ludo.MediatRPattern.Interfaces;
using Ludo.RequestValidator.Interfaces;

namespace Ludo.MediatRPattern.Entities
{
    public class MediatoR : IMediator
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IRequestValidator _requestValidator;

        public MediatoR(IServiceProvider serviceProvider, IRequestValidator requestValidator)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _requestValidator = requestValidator ?? throw new ArgumentNullException(nameof(requestValidator));
        }

        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
        {
            _requestValidator.ValidateRequest(request);

            Type requestType = request.GetType();

            Type response = typeof(IRequest<TResponse>);

            Type responseType = response.GetGenericArguments()[0];

            Type type = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);

            var handler = _serviceProvider.GetService(type);
            if (handler != null)
            {
                var method = handler.GetType().GetMethod("Handle");
                if (method != null)
                {
                    var result = method.Invoke(handler, new object[] { request });

                    if (result is Task<TResponse> task)
                    {
                        return await task;
                    }
                }
            }

            throw new InvalidOperationException("No handler registered for the request.");
        }
    }
}