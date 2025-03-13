using MassTransit;

namespace orders.PaymentWorker.Events;

[MessageUrn(nameof(OrderPaymentEvent))]
public record OrderPaymentEvent(string message);