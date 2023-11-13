using Ludo.Business;
using Ludo.MediatRPattern.Entities;
using Ludo.MediatRPattern.Interfaces;
using Ludo.RequestValidator.Interfaces;

namespace Ludo.Server
{
    public static class ServiceExtensions
    {
        public static void AddMediatR(this IServiceCollection services)
        {
            services.AddSingleton<IMediator, MediatoR>();

            RegisterHandlers.FromAssembly(services);
        }

        public static void AddValidator(this IServiceCollection services)
        {
            RegisterValidators.FromAssembly(services);

            services.AddSingleton<IRequestValidator, RequestValidator.Entities.RequestValidator>();
        }
    }
}