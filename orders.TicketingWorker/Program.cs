using MassTransit;
using orders.TicketingWorker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<PaymentApprovedConsumer>();
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("amqp://guest:guest@localhost:5672");

        cfg.ReceiveEndpoint("orders.payment.approved", e =>
        {
            e.ConfigureConsumeTopology = false;

            e.ConfigureConsumer<PaymentApprovedConsumer>(ctx);
            e.Bind("orders.payment", x =>
            {
                x.ExchangeType = "x-delayed-message";
                x.SetExchangeArgument("x-delayed-type", "topic");
                x.RoutingKey = "orders.payment.approved";
            });
        });

        cfg.ClearSerialization();
        cfg.UseRawJsonSerializer();
        cfg.UseRawJsonDeserializer();
    });
});

var host = builder.Build();
host.Run();