using MassTransit;
using MassTransit.Transports;
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
        private readonly IPublishEndpoint _publishEndpoint;

        public PaymentProcessedConsumer(ILogger<PaymentProcessedConsumer> logger, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<OrderPaymentEvent> context)
        {
            _logger.LogInformation($"estoy en PaymentProcessedConsumer");
            _logger.LogInformation($"[Consumer 1] Recibido: {context.Message}");

            var message = new OrderPaymentEvent($"El pago tiene el estado: {context.Message.status}", "approved");


            await _publishEndpoint.Publish(message, context =>
            {
                context.SetRoutingKey($"orders.payment.{context.Message.status}");
                context.Headers.Set("x-delay", TimeSpan.FromSeconds(30).TotalMilliseconds);
            });
        }
    }
}
