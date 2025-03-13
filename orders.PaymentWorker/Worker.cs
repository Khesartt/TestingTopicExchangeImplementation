using MassTransit;
using orders.PaymentWorker.Events;

namespace orders.PaymentWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                await Task.Delay(60000, stoppingToken);
            }
        }
    }

    public class PaymentProcessedConsumer : IConsumer<OrderPaymentEvent>
    {
        private readonly ILogger<PaymentProcessedConsumer> _logger;

        public PaymentProcessedConsumer(ILogger<PaymentProcessedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<OrderPaymentEvent> context)
        {
            _logger.LogInformation($"estoy en PaymentProcessedConsumer");
            _logger.LogInformation($"[Consumer 1] Recibido: {context.Message}");
            return Task.CompletedTask;
        }
    }
}
