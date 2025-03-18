namespace orders.PaymentWorker;

using MassTransit;
using orders.PaymentWorker.Events;

public class Worker(ILogger<Worker> logger) : BackgroundService
{
    private readonly ILogger<Worker> logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (this.logger.IsEnabled(LogLevel.Information))
            {
                this.logger.LogInformation($"COM COM ONLINE AND READY {DateTimeOffset.Now}:");
            }
            await Task.Delay(60000, stoppingToken);
        }
    }
}

public class PaymentProcessedConsumer(ILogger<PaymentProcessedConsumer> logger, IPublishEndpoint publishEndpoint) : IConsumer<OrderPaymentEvent>
{
    private readonly ILogger<PaymentProcessedConsumer> logger = logger;
    private readonly IPublishEndpoint publishEndpoint = publishEndpoint;

    public async Task Consume(ConsumeContext<OrderPaymentEvent> context)
    {
        this.logger.LogInformation($"estoy en {nameof(PaymentProcessedConsumer)}");
        this.logger.LogInformation($"{nameof(PaymentProcessedConsumer)} Recibido: {context.Message}");

        if (context.Message.status != "approved")
        {
            var message = new OrderPaymentEvent($"pase por {nameof(PaymentProcessedConsumer)} marcando approved", "approved");
            this.logger.LogInformation($"enviando mensaje con tiempo de retraso de 1 minuto al consumidor de aprobacion");
            await this.publishEndpoint.Publish(message, context =>
            {
                context.SetRoutingKey($"orders.payment.approved");
                context.Headers.Set("x-delay", TimeSpan.FromSeconds(1).TotalMilliseconds);
            });
        }
    }
}
