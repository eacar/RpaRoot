using Microsoft.Extensions.DependencyInjection;
using System;

namespace Rpa.Handlers
{
    public sealed class DependencyHandler
    {
        #region Fields

        private static DependencyHandler _resolver;
        private readonly IServiceProvider _serviceProvider;

        #endregion

        #region Properties

        public static DependencyHandler Current
        {
            get
            {
                if (_resolver == null)
                    throw new Exception(
                        "AppDependencyResolver not initialized! You must first invoke Init method it in Startup class.");
                return _resolver;
            }
        }

        #endregion

        #region Constructors

        private DependencyHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        #endregion

        #region Methods - Public

        public static void Init(IServiceProvider services)
        {
            _resolver = new DependencyHandler(services);
        }

        public object Resolve(Type serviceType)
        {
            return _serviceProvider.GetService(serviceType);
        }

        public T Resolve<T>()
        {
            return _serviceProvider.GetService<T>();
        }

        #endregion
    }
}