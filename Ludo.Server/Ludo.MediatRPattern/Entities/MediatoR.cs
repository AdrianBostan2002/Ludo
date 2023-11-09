using Ludo.MediatRPattern.Interfaces;

namespace Ludo.MediatRPattern.Entities
{
    public class MediatoR : IMediator
    {
        private readonly IServiceProvider serviceProvider;

        public MediatoR(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
        {
            Type response = typeof(IRequest<TResponse>);

            Type responseType = response.GetGenericArguments()[0];

            Type requestType = request.GetType();

            Type type = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);

            var handler = serviceProvider.GetService(type);
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