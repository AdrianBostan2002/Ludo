using Ludo.Domain.Interfaces;

namespace Ludo.Domain.Entities
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
            Type responseType = typeof(IRequest<TResponse>);

            var attributes = responseType.GetGenericArguments();

            Type requestType = request.GetType();

            Type type = typeof(IRequestHandler<,>).MakeGenericType(requestType, attributes[0]);

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