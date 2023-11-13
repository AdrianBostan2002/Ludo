using Ludo.MediatRPattern.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Ludo.Business
{
    public static class RegisterHandlers
    {
        public static void FromAssembly(IServiceCollection services)
        {
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

                        Type request = typeof(IRequest<>).MakeGenericType(genericArguments[1]);

                        Type requestType = genericArguments[0];
                        Type responseType = request.GenericTypeArguments[0];

                        Type type = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);

                        services.AddSingleton(type, handlerType);
                    }
                }
            }
        }
    }
}