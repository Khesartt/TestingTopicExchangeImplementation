using MassTransit;
using orders.TicketingWorker.Events;

namespace orders.TicketingWorker
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

    public class PaymentApprovedConsumer : IConsumer<OrderPaymentEvent>
    {
        private readonly ILogger<PaymentApprovedConsumer> _logger;

        public PaymentApprovedConsumer(ILogger<PaymentApprovedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<OrderPaymentEvent> context)
        {
            _logger.LogInformation($"estoy en PaymentApprovedConsumer");
            _logger.LogInformation($"[Consumer 2] Recibido: {context.Message}");
            return Task.CompletedTask;
        }
    }
}