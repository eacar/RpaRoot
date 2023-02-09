using System;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Rpa.Log.Loggers;

namespace Rpa.WinService.HostService
{
    public class ServiceBaseLifetime : ServiceBase, IHostLifetime
    {
        #region Fields

        private readonly TaskCompletionSource<object> _delayStart = new TaskCompletionSource<object>();

        #endregion

        #region Properties

        public IHostApplicationLifetime ApplicationLifetime { get; }

        #endregion

        #region Constructors

        public ServiceBaseLifetime(IHostApplicationLifetime applicationLifetime)
        {
            ApplicationLifetime = applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
        }

        #endregion


        public Task WaitForStartAsync(CancellationToken cancellationToken)
        {
            cancellationToken.Register(() => _delayStart.TrySetCanceled());
            ApplicationLifetime.ApplicationStopping.Register(Stop);

            new Thread(Run).Start(); // Otherwise this would block and prevent IHost.StartAsync from finishing.
            return _delayStart.Task;
        }


        #region Methods - Public

        public Task StopAsync(CancellationToken cancellationToken)
        {
            SharpLogger.LogInfo("Stopping...");

            Stop();
            return Task.CompletedTask;
        }

        #endregion

        #region Methods - Protected

        // Called by base.Run when the service is ready to start.
        protected override void OnStart(string[] args)
        {
            _delayStart.TrySetResult(null);
            base.OnStart(args);
        }

        // Called by base.Stop. This may be called multiple times by service Stop, ApplicationStopping, and StopAsync.
        // That's OK because StopApplication uses a CancellationTokenSource and prevents any recursion.
        protected override void OnStop()
        {
            ApplicationLifetime.StopApplication();
            base.OnStop();
            SharpLogger.LogInfo("Stopped!");
        }

        #endregion

        #region Methods - Private

        private void Run()
        {
            try
            {
                SharpLogger.LogInfo("Starting...");
                Run(this); // This blocks until the service is stopped.
                _delayStart.TrySetException(new InvalidOperationException("Stopped without starting"));
            }
            catch (Exception ex)
            {
                _delayStart.TrySetException(ex);
            }
        }

        #endregion
    }
}