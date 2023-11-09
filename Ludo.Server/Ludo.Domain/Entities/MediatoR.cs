using Ludo.Domain.Interfaces;
using System.Reflection;

namespace Ludo.Domain.Entities
{
    public class MediatoR : IMediator
    {
        private Dictionary<Type, Type> requestHandlerTypes;
        private readonly IServiceProvider serviceProvider;

        public MediatoR(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void RegisterRequestHandler<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> handler)
            where TRequest : IRequest<TResponse>
        {
            Type requestType = typeof(TRequest);
            Type handlerType = handler.GetType();
            requestHandlerTypes[requestType] = handlerType;
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