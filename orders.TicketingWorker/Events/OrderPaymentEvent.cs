﻿using MassTransit;

namespace orders.TicketingWorker.Events;

[MessageUrn(nameof(OrderPaymentEvent))]
public record OrderPaymentEvent(string message);