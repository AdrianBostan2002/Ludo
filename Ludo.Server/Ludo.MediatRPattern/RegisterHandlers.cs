using Ludo.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Ludo.Business
{
    public static class RegisterHandlers
    {
        public static Dictionary<Type, Type> RegisterHandlersFromAssembly(IServiceCollection services)
        {
            var requestHandlerTypes = new Dictionary<Type, Type>();

            var assemblyName = "Ludo.Business";

            var businessAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(assembly => assembly.FullName.StartsWith(assemblyName));

            if (businessAssembly != null)
            {
                var handlerTypes = businessAssembly.GetTypes()
                    .Where(type => type.GetInterfaces()
                        .Any(inter => inter.IsGenericType && inter.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)))
                    .ToList();

                foreach (var handlerType in handlerTypes)
                {
                    var handlerInterfaces = handlerType.GetInterfaces()
                        .Where(inter => inter.IsGenericType && inter.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
                        .ToList();

                    foreach (var handlerInterface in handlerInterfaces)
                    {
                        Type[] genericArguments = handlerInterface.GetGenericArguments();
                        //var request = handlerInterface.get
                        Type requestType = typeof(IRequest<>).MakeGenericType(genericArguments[1]);
                        Type responseType = genericArguments[1];

                        // Register the handler with the IRequestHandler interface
                        //Type serviceType = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);

                        Type type = typeof(IRequestHandler<,>).MakeGenericType(genericArguments[0], requestType.GenericTypeArguments[0]);

                        Console.WriteLine(type);
                        Console.WriteLine(handlerType);

                        services.AddSingleton(type, handlerType);

                        //requestHandlerTypes[serviceType] = handlerType;
                    }
                }

            }
            return requestHandlerTypes;
        }
    }
}
