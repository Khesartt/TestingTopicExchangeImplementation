using MassTransit;

namespace ApiCorePayment.Events;

[MessageUrn(nameof(OrderPaymentEvent))]
public record OrderPaymentEvent(string message, string status);