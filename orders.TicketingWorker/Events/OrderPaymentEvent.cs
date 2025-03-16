using MassTransit;

namespace orders.TicketingWorker.Events;

[MessageUrn(nameof(OrderPaymentEvent))]
[EntityName(nameof(OrderPaymentEvent))] 
public record OrderPaymentEvent(string message, string status);