using MassTransit;

namespace orders.PaymentWorker.Events;

[MessageUrn(nameof(OrderPaymentEvent))]
[EntityName(nameof(OrderPaymentEvent))]
public record OrderPaymentEvent(string message, string status);