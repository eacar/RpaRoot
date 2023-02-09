using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rpa.WinService.HostService;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rpa.WinService.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Fields

        private const string ConfigureServicesMethodName = "ConfigureServices";

        #endregion

        #region Methods - Public

        #region HostBuilder

        /// <summary>
        /// Specify the startup type to be used by the host.
        /// </summary>
        /// <typeparam name="TStartup">The type containing an optional constructor with
        /// an <see cref="IConfiguration"/> parameter. The implementation should contain a public
        /// method named ConfigureServices with <see cref="IServiceCollection"/> parameter.</typeparam>
        /// <param name="hostBuilder">The <see cref="IHostBuilder"/> to initialize with TStartup.</param>
        /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
        public static IHostBuilder UseStartup<TStartup>(
            this IHostBuilder hostBuilder) where TStartup : class
        {
            // Invoke the ConfigureServices method on IHostBuilder...
            hostBuilder.ConfigureServices((ctx, serviceCollection) =>
            {
                // Find a method that has this signature: ConfigureServices(IServiceCollection)
                var cfgServicesMethod = typeof(TStartup).GetMethod(ConfigureServicesMethodName,
                    new Type[] { typeof(IServiceCollection) });

                // Check if TStartup has a ctor that takes a IConfiguration parameter
                var hasConfigCtor = typeof(TStartup).GetConstructor(
                    new Type[] { typeof(IConfiguration) }) != null;

                // create a TStartup instance based on ctor
                var startUpObj = hasConfigCtor ?
                    (TStartup)Activator.CreateInstance(typeof(TStartup), ctx.Configuration) :
                    (TStartup)Activator.CreateInstance(typeof(TStartup), null);

                // finally, call the ConfigureServices implemented by the TStartup object
                cfgServicesMethod?.Invoke(startUpObj, new object[] { serviceCollection });
            });

            // chain the response
            return hostBuilder;
        }

        public static Task RunServiceAsync(this IHostBuilder hostBuilder,
            CancellationToken cancellationToken = default)
        {
            return hostBuilder.UseServiceBaseLifetime().Build().RunAsync(cancellationToken);
        }
        public static Task RunServiceAsync<T>(this IHostBuilder hostBuilder,
            CancellationToken cancellationToken = default)
            where T : ServiceBaseLifetime
        {
            return hostBuilder.UseServiceBaseLifetime<T>().Build().RunAsync(cancellationToken);
        }

        #endregion

        #endregion

        #region Methods - Private

        #region HostBuilder

        private static IHostBuilder UseServiceBaseLifetime(this IHostBuilder hostBuilder)
        {
            return hostBuilder
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IHostLifetime, ServiceBaseLifetime>();
                });
        }
        private static IHostBuilder UseServiceBaseLifetime<T>(this IHostBuilder hostBuilder)
            where T : ServiceBaseLifetime
        {
            return hostBuilder
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<IHostLifetime, T>();
                });
        }

        #endregion

        #endregion
    }
}