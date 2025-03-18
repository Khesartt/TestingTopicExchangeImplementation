﻿namespace ApiCorePayment.Events;

using MassTransit;

[MessageUrn(nameof(OrderPaymentEvent))]
public record OrderPaymentEvent(string message, string status);