namespace orders.TicketingWorker;

using MassTransit;
using Microsoft.Extensions.Logging;
using orders.TicketingWorker.Events;

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

public class PaymentApprovedConsumer(ILogger<PaymentApprovedConsumer> logger) : IConsumer<OrderPaymentEvent>
{
    private readonly ILogger<PaymentApprovedConsumer> logger = logger;

    public Task Consume(ConsumeContext<OrderPaymentEvent> context)
    {
        this.logger.LogInformation($"estoy en {nameof(PaymentApprovedConsumer)}");
        this.logger.LogInformation($"{nameof(PaymentApprovedConsumer)} Recibido: {context.Message}");
        return Task.CompletedTask;
    }
}