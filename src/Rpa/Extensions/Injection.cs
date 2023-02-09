using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Rpa.Contracts;
using Rpa.Handlers;

namespace Rpa.Extensions
{
    public static class InjectionExtensions
    {
        public static IServiceCollection AddRpaServices(this IServiceCollection services)
        {
            services.TryAddSingleton<IDateTimeHandler, DateTimeHandler>();
            services.TryAddSingleton<IXmlHandler, XmlHandler>();
            services.TryAddSingleton<IFileHandler, FileHandler>();

            return services;
        }
    }
}