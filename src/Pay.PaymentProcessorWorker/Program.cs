using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pay.PaymentProcessorWorker;

Host.CreateDefaultBuilder(args)
    .ConfigureServices(
        (context, services) =>
        {
            services.AddHostedService<PaymentConsumerWorker>();
        }
    )
    .Build()
    .Run();
