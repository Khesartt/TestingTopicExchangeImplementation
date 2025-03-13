using ApiCorePayment.Events;
using MassTransit;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("amqp://guest:guest@localhost:5672");

        cfg.Message<OrderPaymentEvent>(x => x.SetEntityName("orders.payment"));
        cfg.Publish<OrderPaymentEvent>(x =>
        {
            x.ExchangeType = "topic";
        });

        cfg.ClearSerialization();
        cfg.UseRawJsonSerializer();  
        cfg.UseRawJsonDeserializer(); 
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Payment API",
        Version = "v1",
        Description = "API para publicar eventos de pago en RabbitMQ con MassTransit"
    });
});

var app = builder.Build();

app.MapPost("/publish/{status}", async (IPublishEndpoint publishEndpoint, string status) =>
{
    var message = new OrderPaymentEvent($"El pago tiene el estado: {status}", status);

    await publishEndpoint.Publish(message, x => x.SetRoutingKey($"orders.payment.{status}"));

    return Results.Ok($"Mensaje publicado con estado: {status}");
})
.WithName("PublishPaymentEvent");

app.Run();