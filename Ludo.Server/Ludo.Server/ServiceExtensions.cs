using Ludo.Business;
using Ludo.MediatRPattern.Entities;
using Ludo.MediatRPattern.Interfaces;

namespace Ludo.Server
{
    public static class ServiceExtensions
    {
        public static void AddMediatR(this IServiceCollection services)
        {
            services.AddSingleton<IMediator, MediatoR>();

            RegisterHandlers.FromAssembly(services);
        }
    }
}