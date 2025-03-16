using MassTransit;
using orders.PaymentWorker;
using orders.PaymentWorker.Events;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<PaymentProcessedConsumer>();
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("amqp://guest:guest@localhost:5672");

        cfg.ReceiveEndpoint("orders.payment.processed", e =>
        {
            e.ConfigureConsumeTopology = false;

            e.ConfigureConsumer<PaymentProcessedConsumer>(ctx);
            e.Bind("orders.payment", x =>
            {
                x.ExchangeType = "x-delayed-message";
                x.SetExchangeArgument("x-delayed-type", "topic");
                x.RoutingKey = "orders.payment.*";
            });
        });

        cfg.Message<OrderPaymentEvent>(x => x.SetEntityName("orders.payment"));
        cfg.Publish<OrderPaymentEvent>(x =>
        {
            x.ExchangeType = "x-delayed-message";
            x.SetExchangeArgument("x-delayed-type", "topic");
        });

        cfg.ClearSerialization();
        cfg.UseRawJsonSerializer();
        cfg.UseRawJsonDeserializer();
    });
});

var host = builder.Build();
host.Run();
