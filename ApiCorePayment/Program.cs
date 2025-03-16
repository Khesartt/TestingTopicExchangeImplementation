using ApiCorePayment.Events;
using MassTransit;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("amqp://guest:guest@localhost:5672");

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

    await publishEndpoint.Publish(message, context =>
    {
        context.SetRoutingKey($"orders.payment.{status}");
        context.Headers.Set("x-delay", TimeSpan.FromSeconds(30).TotalMilliseconds);
    });

    return Results.Ok($"Mensaje publicado con estado: {status}");
})
.WithName("PublishPaymentEvent");

app.Run();